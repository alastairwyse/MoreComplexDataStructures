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
    /// Provides a method to randomize the elements of a list with O(n * log(n)) time complexity.
    /// </summary>
    /// <typeparam name="T">Specifies the type of items in the list.</typeparam>
    /// <remarks>I came up with the algorithm in this class before finding the Fisher/Yates/Knuth algorithm (https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle) which is both more memory efficient, and able to randomize with O(n) time complexity.  Hence the ListRandomizer class which uses the Fisher/Yates/Knuth algorithm should be used in preference to this class.</remarks>
    class TreeBasedListRandomizer<T> where T : IComparable<T>
    {
        // TODO : Still bugs in this.
        // E.g. the following array of items (X marks used, O marks unused)
        //
        //   Index:  0 1 2 3 4 5 6 7
        //   Used:   X X X O X X O O
        //
        // If random number generator gives a next index of 2, index 7 should be used, but algorithm returns index 6

        /// <summary>Holds indices in the list to be randomized that have already been used during the randomizing process.</summary>
        protected WeightBalancedTree<Int32> usedIndices;
        /// <summary>Holds ranges of indices in the list to be randomized that have already been used during the randomizing process.</summary>
        protected WeightBalancedTree<IntegerRange> usedIndexRanges;
        /// <summary>The random number generator to use for selecting indices from the source list.</summary>
        protected IRandomIntegerGenerator randomGenerator;

        /// <summary>
        /// Initialises a new instance of the MoreComplexDataStructures.TreeBasedListRandomizer class.
        /// </summary>
        public TreeBasedListRandomizer()
        {
            usedIndices = new WeightBalancedTree<Int32>();
            usedIndexRanges = new WeightBalancedTree<IntegerRange>();
            randomGenerator = new DefaultRandomGenerator();
        }

        /// <summary>
        /// Initialises a new instance of the MoreComplexDataStructures.TreeBasedListRandomizer class.
        /// </summary>
        /// <param name="randomIntegerGenerator">An implementation of interface IRandomIntegerGenerator to use for selecting indices from the source list.</param>
        public TreeBasedListRandomizer(IRandomIntegerGenerator randomIntegerGenerator)
            : this()
        {
            randomGenerator = randomIntegerGenerator;
        }

        /// <summary>
        /// Randomizes the elements of the specified collection implementing interface IList&lt;T&gt;.
        /// </summary>
        /// <param name="inputList">The IList to randomize the elements of.</param>
        /// <returns>A new list containing the same elements as the inputted list, in random order.</returns>
        public List<T> Randomize(IList<T> inputList)
        {
            usedIndices.Clear();
            usedIndexRanges.Clear();

            List<T> returnList = new List<T>(inputList.Count);
            Int32 remainingUnusedIndexCount = inputList.Count;

            while (remainingUnusedIndexCount > 0)
            {
                Int32 nextRandomIndex = randomGenerator.Next(remainingUnusedIndexCount);

                // We need to translate the random index into a valid position in the input list, based on the indices which have already been used
                Int32 translatedIndex = nextRandomIndex;
                // Shuffle the index up by the number of used indices less than it
                if (usedIndices.Contains(translatedIndex) == true)
                {
                    translatedIndex += usedIndices.GetCountLessThan(translatedIndex);
                    translatedIndex++;
                }
                else
                {
                    translatedIndex += usedIndices.GetCountLessThan(translatedIndex);
                }
                // If the resulting index is used we need to find the next free index after the range containing the index
                if (usedIndices.Contains(translatedIndex) == true)
                {
                    // The IntegerRange classes' equality method returns true if the start value of two ranges is the same
                    //   Hence create a fake range and call Contains() to see if the relevant range starts at 'translatedIndex'
                    IntegerRange rangeContainingIndex = new IntegerRange(translatedIndex, 1);
                    if (usedIndexRanges.Contains(rangeContainingIndex) == true)
                    {
                        rangeContainingIndex = usedIndexRanges.GetNextLessThan(new IntegerRange(translatedIndex + 1, 1)).Item2;
                    }
                    else
                    {
                        // Otherwise the relevant range must start at a lower value than 'translatedIndex'
                        rangeContainingIndex = usedIndexRanges.GetNextLessThan(rangeContainingIndex).Item2;
                    }

                    translatedIndex = rangeContainingIndex.StartValue + rangeContainingIndex.Length;
                }

                // Add the value at 'translatedIndex' to the randomized list
                returnList.Add(inputList[translatedIndex]);

                usedIndices.Add(translatedIndex);
                AddIndexToUsedRanges(translatedIndex);
                remainingUnusedIndexCount--;
            }

            return returnList;
        }

        # region Private/Protected Methods

        /// <summary>
        /// Adds the list index specified by the inputted integer to the tree holding used ranges of indices.
        /// </summary>
        /// <param name="index">The index to add.</param>
        protected void AddIndexToUsedRanges(Int32 index)
        {
            // Create an integer range based on the inputted index
            IntegerRange indexRange = new IntegerRange(index, 1);

            // Check whether a range starting with the specified index already exists in the tree
            if (usedIndexRanges.Contains(indexRange) == true)
            {
                throw new ArgumentException("The specified index already exists in the tree.", "index");
            }

            Tuple<Boolean, IntegerRange> nextLowerRange = usedIndexRanges.GetNextLessThan(indexRange);
            Tuple<Boolean, IntegerRange> nextGreaterRange = usedIndexRanges.GetNextGreaterThan(indexRange);

            // Check that the specified index doesn't exist in the next lower range
            if (nextLowerRange.Item1 == true && ( index <= (nextLowerRange.Item2.StartValue + nextLowerRange.Item2.Length - 1) ))
            {
                throw new ArgumentException("The specified index already exists in the tree.", "index");
            }
            // Handle the case where the new index 'merges' two existing ranges
            if (IndexIsImmediatelyGreaterThanRange(index, nextLowerRange) && IndexIsImmediatelyLessThanRange(index, nextGreaterRange))
            {
                usedIndexRanges.Remove(nextGreaterRange.Item2);
                nextLowerRange.Item2.Length = nextLowerRange.Item2.Length + 1 + nextGreaterRange.Item2.Length;
            }
            // Handle the case where the new index 'extends' the next lower range by 1
            else if (IndexIsImmediatelyGreaterThanRange(index, nextLowerRange))
            {
                nextLowerRange.Item2.Length = nextLowerRange.Item2.Length + 1;
            }
            // Handle the case where the new index reduces the start value of the next greater range by 1
            else if (IndexIsImmediatelyLessThanRange(index, nextGreaterRange))
            {
                nextGreaterRange.Item2.StartValue = nextGreaterRange.Item2.StartValue - 1;
                nextGreaterRange.Item2.Length = nextGreaterRange.Item2.Length + 1;
            }
            // Otherwise add a new range starting at 'index' with length = 1
            else
            {
                usedIndexRanges.Add(indexRange);
            }
        }

        /// <summary>
        /// Returns true if the specified index sits immediately to the right of (i.e. greater than) the range specified in the inputted tuple (as would be the case with range 1-3 and index 4).
        /// </summary>
        /// <param name="index">The index to test.</param>
        /// <param name="range">A tuple containing 2 values: a boolean indicating whether a range exists in Item2, and an integer range.</param>
        /// <returns>True if the index sits immediately to the right.  False otherwise (or if no range exists).</returns>
        /// <remarks>Tuple parameter is the same format as that returned by WeightBalancedTree.GetNextLessThan().</remarks>
        protected Boolean IndexIsImmediatelyGreaterThanRange(Int32 index, Tuple<Boolean, IntegerRange> range)
        {
            if (range.Item1 == true)
            {
                if (range.Item2.StartValue + range.Item2.Length == index)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns true if the specified index sits immediately to the left of (i.e. less than) the range specified in the inputted tuple (as would be the case with range 3-5 and index 2).
        /// </summary>
        /// <param name="index">The index to test.</param>
        /// <param name="range">A tuple containing 2 values: a boolean indicating whether a range exists in Item2, and an integer range.</param>
        /// <returns>True if the index sits immediately to the left.  False otherwise (or if no range exists).</returns>
        /// <remarks>Tuple parameter is the same format as that returned by WeightBalancedTree.GetNextGreaterThan().</remarks>
        protected Boolean IndexIsImmediatelyLessThanRange(Int32 index, Tuple<Boolean, IntegerRange> range)
        {
            if (range.Item1 == true)
            {
                if (range.Item2.StartValue - 1 == index)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        # region Nested Classes

        /// <summary>
        /// Container class used to represent a contiguous range of integer values (i.e. a sequence of integers which is an arithmetic progression with a common difference of 1).
        /// </summary>
        protected class IntegerRange : IComparable<IntegerRange>
        {
            /// <summary>The first value in the range.</summary>
            private Int32 startValue;
            /// <summary>The length of the range (e.g the range containing values { 2, 3, 4, 5 } would have length 4).</summary>
            private Int32 length;

            /// <summary>
            /// The first value in the range.
            /// </summary>
            public Int32 StartValue
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
            /// The length of the range (e.g the range containing values { 2, 3, 4, 5 } would have length 4).
            /// </summary>
            public Int32 Length
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
            /// Initialises a new instance of the MoreComplexDataStructures.TreeBasedListRandomizer+IntegerRange class.
            /// </summary>
            /// <param name="startValue">The first value in the range.</param>
            /// <param name="length">The length of the range (e.g the range containing values { 2, 3, 4, 5 } would have length 4).</param>
            public IntegerRange(Int32 startValue, Int32 length)
            {
                CheckLengthValue(length);
                this.startValue = startValue;
                this.length = length;
            }

            public int CompareTo(IntegerRange other)
            {
                return startValue.CompareTo(other.startValue);
            }

            private void CheckLengthValue(Int32 inputLength)
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
