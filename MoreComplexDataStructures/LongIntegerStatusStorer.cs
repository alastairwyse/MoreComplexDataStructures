/*
 * Copyright 2017 Alastair Wyse (https://github.com/alastairwyse/MoreComplexDataStructures/)
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreComplexDataStructures
{
    /// <summary>
    /// Stores a boolean status for a complete set of long integers, using a tree of number ranges as the underlying storage mechanism.
    /// </summary>
    /// <remarks>Example use case is to store a yes/no status against a large set of numeric key values (e.g. is a given key active?).  O(1) solution would require multiple arrays, and woule be prohibitive in terms of memory usage.  This implementation uses objects representing ranges of numbers (to reduce memory usage) in a tree (hence O(log(n)) time complexity for status set/retrieve operations).</remarks>
    public class LongIntegerStatusStorer
    {
        /// <summary>Holds ranges of integers which have been set to true.</summary>
        protected WeightBalancedTree<LongIntegerRange> rangeStatuses;
        /// <summary>The total number of integers set with true status.</summary>
        protected Int64 count;

        /// <summary>
        /// The total number of integers set with true status.
        /// </summary>
        public Int64 Count
        {
            get
            {
                return count;
            }
        }

        /// <summary>
        /// Initialises a new instance of the MoreComplexDataStructures.LongIntegerStatusStorer class, assuming all integer statuses are initially false.
        /// </summary>
        public LongIntegerStatusStorer()
        {
            rangeStatuses = new WeightBalancedTree<LongIntegerRange>();
            count = 0;
        }

        /// <summary>
        /// Sets all statuses to false.
        /// </summary>
        public void Clear()
        {
            rangeStatuses.Clear();
            count = 0;
        }

        /// <summary>
        /// Sets the status for the specified integer to true.
        /// </summary>
        /// <param name="inputInteger">The integer to set the status for.</param>
        public void SetStatusTrue(Int64 inputInteger)
        {
            // Create an integer range based on the inputted integer
            LongIntegerRange inputIntegerRange = new LongIntegerRange(inputInteger, 1);

            // Check whether a range starting with the specified integer already exists in the tree
            if (rangeStatuses.Contains(inputIntegerRange) == true)
            {
                return;
            }

            Tuple<Boolean, LongIntegerRange> nextLowerRange = rangeStatuses.GetNextLessThan(inputIntegerRange);
            Tuple<Boolean, LongIntegerRange> nextGreaterRange = rangeStatuses.GetNextGreaterThan(inputIntegerRange);

            // Check whether the specified integer exists in the next lower range
            if (nextLowerRange.Item1 == true && (inputInteger <= (nextLowerRange.Item2.StartValue + nextLowerRange.Item2.Length - 1)))
            {
                return;
            }
            // Handle the case where the new integer 'merges' two existing ranges
            if (IntegerIsImmediatelyGreaterThanRange(inputInteger, nextLowerRange) && IntegerIsImmediatelyLessThanRange(inputInteger, nextGreaterRange))
            {
                rangeStatuses.Remove(nextGreaterRange.Item2);
                nextLowerRange.Item2.Length = nextLowerRange.Item2.Length + 1 + nextGreaterRange.Item2.Length;
            }
            // Handle the case where the new integer 'extends' the next lower range by 1
            else if (IntegerIsImmediatelyGreaterThanRange(inputInteger, nextLowerRange))
            {
                nextLowerRange.Item2.Length = nextLowerRange.Item2.Length + 1;
            }
            // Handle the case where the new integer reduces the start value of the next greater range by 1
            else if (IntegerIsImmediatelyLessThanRange(inputInteger, nextGreaterRange))
            {
                nextGreaterRange.Item2.StartValue = nextGreaterRange.Item2.StartValue - 1;
                nextGreaterRange.Item2.Length = nextGreaterRange.Item2.Length + 1;
            }
            // Otherwise add a new range starting at 'inputInteger' with length = 1
            else
            {
                rangeStatuses.Add(inputIntegerRange);
            }

            count++;
        }

        /// <summary>
        /// Sets the status for the specified integer to false.
        /// </summary>
        /// <param name="inputInteger">The integer to set the status for.</param>
        public void SetStatusFalse(Int64 inputInteger)
        {
            LongIntegerRange rangeToModify = null;

            // Create an integer range based on the inputted integer
            LongIntegerRange inputIntegerRange = new LongIntegerRange(inputInteger, 1);

            // Attempt to find the range holding the inputted integer
            Tuple<Boolean, LongIntegerRange> nextLowerRange = rangeStatuses.GetNextLessThan(inputIntegerRange);

            // Check whether a range starting with the specified integer exists in the tree
            if (rangeStatuses.Contains(inputIntegerRange) == true)
            {
                // Need special handling if inputInteger is Int64.MaxValue
                if (inputInteger == Int64.MaxValue)
                {
                    rangeToModify = new LongIntegerRange(inputInteger, 1);
                }
                else
                {
                    rangeToModify = rangeStatuses.GetNextLessThan(new LongIntegerRange(inputInteger + 1, 1)).Item2;
                }
            }
            // Otherwise check whether the specified integer exists in the next lower range
            else if (nextLowerRange.Item1 == true && (inputInteger <= (nextLowerRange.Item2.StartValue + nextLowerRange.Item2.Length - 1)))
            {
                rangeToModify = nextLowerRange.Item2;
            }

            if (rangeToModify != null)
            {
                // Handle the case where the inputted integer is at the start of the range to modify
                if (inputInteger == rangeToModify.StartValue)
                {
                    if (rangeToModify.Length == 1)
                    {
                        rangeStatuses.Remove(rangeToModify);
                    }
                    else
                    {
                        rangeToModify.StartValue = rangeToModify.StartValue + 1;
                        rangeToModify.Length = rangeToModify.Length - 1;
                    }
                }
                // Handle the case where the inputted integer is at the end of the range to modify
                else if (inputInteger == (rangeToModify.StartValue + rangeToModify.Length - 1))
                {
                    rangeToModify.Length = rangeToModify.Length - 1;
                }
                // Handle the case where the inputted integer is in the middle of the range to modify
                else
                {
                    LongIntegerRange newRange = new LongIntegerRange(inputInteger + 1, rangeToModify.Length - (inputInteger - rangeToModify.StartValue + 1));
                    rangeToModify.Length = inputInteger - rangeToModify.StartValue;
                    rangeStatuses.Add(newRange);
                }

                count--;
            }
        }

        /// <summary>
        /// Retrieves the current status for the specified integer.
        /// </summary>
        /// <param name="inputInteger">The integer to retrieve the status for.</param>
        /// <returns>The status of the integer.</returns>
        public Boolean GetStatus(Int64 inputInteger)
        {
            // Create an integer range based on the inputted integer
            LongIntegerRange inputIntegerRange = new LongIntegerRange(inputInteger, 1);

            // Check whether a range starting with the specified integer exists in the tree
            if (rangeStatuses.Contains(inputIntegerRange) == true)
            {
                return true;
            }

            Tuple<Boolean, LongIntegerRange> nextLowerRange = rangeStatuses.GetNextLessThan(inputIntegerRange);
            // Check whether the specified integer exists in the next lower range
            if (nextLowerRange.Item1 == true && (inputInteger <= (nextLowerRange.Item2.StartValue + nextLowerRange.Item2.Length - 1)))
            {
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Performs a breadth-first search of the underlying tree, invoking the specified action on the start value and length of each range.
        /// </summary>
        /// <param name="rangeAction">The action to perform on each range.  Accepts a two parameters which are the start value and length of the current range.</param>
        public void TraverseTree(Action<Int64, Int64> rangeAction)
        {
            Action<WeightBalancedTreeNode<LongIntegerRange>> nodeAction = (node) =>
            {
                rangeAction.Invoke(node.Item.StartValue, node.Item.Length);
            };

            rangeStatuses.BreadthFirstSearch(nodeAction);
        }

        # region Private/Protected Methods

        /// <summary>
        /// Returns true if the specified integer sits immediately to the right of (i.e. greater than) the range specified in the inputted tuple (as would be the case with range 1-3 and integer 4).
        /// </summary>
        /// <param name="inputInteger">The integer to test.</param>
        /// <param name="range">A tuple containing 2 values: a boolean indicating whether a range exists in Item2, and an integer range.</param>
        /// <returns>True if the integer sits immediately to the right.  False otherwise (or if no range exists).</returns>
        /// <remarks>Tuple parameter is the same format as that returned by WeightBalancedTree.GetNextLessThan().</remarks>
        protected Boolean IntegerIsImmediatelyGreaterThanRange(Int64 inputInteger, Tuple<Boolean, LongIntegerRange> range)
        {
            if (range.Item1 == true)
            {
                if (range.Item2.StartValue + range.Item2.Length == inputInteger)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns true if the specified integer sits immediately to the left of (i.e. less than) the range specified in the inputted tuple (as would be the case with range 3-5 and integer 2).
        /// </summary>
        /// <param name="inputInteger">The integer to test.</param>
        /// <param name="range">A tuple containing 2 values: a boolean indicating whether a range exists in Item2, and an integer range.</param>
        /// <returns>True if the integer sits immediately to the left.  False otherwise (or if no range exists).</returns>
        /// <remarks>Tuple parameter is the same format as that returned by WeightBalancedTree.GetNextGreaterThan().</remarks>
        protected Boolean IntegerIsImmediatelyLessThanRange(Int64 inputInteger, Tuple<Boolean, LongIntegerRange> range)
        {
            if (range.Item1 == true)
            {
                if (range.Item2.StartValue - 1 == inputInteger)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        # region Nested Classes

        /// <summary>
        /// Container class used to represent a contiguous range of long integer values (i.e. a sequence of integers which is an arithmetic progression with a common difference of 1).
        /// </summary>
        protected class LongIntegerRange : IComparable<LongIntegerRange>
        {
            /// <summary>The first value in the range.</summary>
            private Int64 startValue;
            /// <summary>The inclusive length of the range (e.g the range containing values { 2, 3, 4, 5 } would have length 4).</summary>
            private Int64 length;

            /// <summary>
            /// The first value in the range.
            /// </summary>
            public Int64 StartValue
            {
                get
                {
                    return startValue;
                }
                set
                {
                    startValue = value;
                }
            }

            /// <summary>
            /// The inclusive length of the range (e.g the range containing values { 2, 3, 4, 5 } would have length 4).
            /// </summary>
            public Int64 Length
            {
                get
                {
                    return length;
                }
                set
                {
                    CheckLengthValue(value);
                    length = value;
                }
            }

            /// <summary>
            /// Initialises a new instance of the MoreComplexDataStructures.LongIntegerStatusStorer+LongIntegerRange class.
            /// </summary>
            /// <param name="startValue">The first value in the range.</param>
            /// <param name="length">The inclusive length of the range (e.g the range containing values { 2, 3, 4, 5 } would have length 4).</param>
            public LongIntegerRange(Int64 startValue, Int64 length)
            {
                CheckLengthValue(length);
                this.startValue = startValue;
                this.length = length;
            }

            public Int32 CompareTo(LongIntegerRange other)
            {
                return startValue.CompareTo(other.startValue);
            }

            private void CheckLengthValue(Int64 inputLength)
            {
                if (inputLength < 1)
                {
                    throw new ArgumentException("Parameter 'length' must be greater than or equal to 1.", "length");
                }
            }
        }

        #endregion
    }
}
