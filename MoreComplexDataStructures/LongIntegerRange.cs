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
    /// Container class used to represent a contiguous range of long integer values (i.e. a sequence of integers which is an arithmetic progression with a common difference of 1).
    /// </summary>
    public class LongIntegerRange : IComparable<LongIntegerRange>, IEquatable<LongIntegerRange>
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
                CheckRangeDoesNotExceedInt64Limit(value, length);
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
                CheckRangeDoesNotExceedInt64Limit(startValue, value);
                length = value;
            }
        }

        /// <summary>
        /// The last (inclusive) value in the range.
        /// </summary>
        public Int64 EndValue
        {
            get
            {
                return startValue + length - 1;
            }
        }

        /// <summary>
        /// Initialises a new instance of the MoreComplexDataStructures.LongIntegerRange class.
        /// </summary>
        /// <param name="startValue">The first value in the range.</param>
        /// <param name="length">The inclusive length of the range (e.g the range containing values { 2, 3, 4, 5 } would have length 4).</param>
        public LongIntegerRange(Int64 startValue, Int64 length)
        {
            CheckLengthValue(length);
            CheckRangeDoesNotExceedInt64Limit(startValue, length);
            this.startValue = startValue;
            this.length = length;
        }

        /// <summary>
        /// Compares this instance to specified LongIntegerRange and returns an indication of their relative values.
        /// </summary>
        /// <param name="other">A LongIntegerRange to compare. </param>
        /// <returns>A value that indicates the relative order of the objects being compared.</returns>
        /// <remarks>Note that this method only compares on the start value of the range.</remarks>
        public Int32 CompareTo(LongIntegerRange other)
        {
            return startValue.CompareTo(other.startValue);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another MoreComplexDataStructures.LongIntegerRange object.
        /// </summary>
        /// <param name="obj">The LongIntegerRange to compare with the current.</param>
        /// <returns>True if the specified LongIntegerRange is equal to the current, otherwise false.</returns>
        public override Boolean Equals(object obj)
        {
            if (!(obj is LongIntegerRange))
            {
                return false;
            }
            else
            {
                return this.Equals((LongIntegerRange)obj);
            }
        }

        /// <summary>
        /// Indicates whether the current object is equal to another MoreComplexDataStructures.LongIntegerRange object.
        /// </summary>
        /// <param name="other">The LongIntegerRange to compare.</param>
        /// <returns>True if the specified LongIntegerRange is equal to the current, otherwise false.</returns>
        public Boolean Equals(LongIntegerRange other)
        {
            if (startValue == other.startValue && length == other.length)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// The hash function for the current LongIntegerRange.
        /// </summary>
        /// <returns>A hash code for this LongIntegerRange.</returns>
        public override Int32 GetHashCode()
        {
            return Convert.ToInt32((startValue * length * 31) % Int32.MaxValue);
        }

        # region Private/Protected Methods

        /// <summary>
        /// Throws an exception if the value of parameter 'inputLength' is less than 1.
        /// </summary>
        /// <param name="inputLength">The value of parameter 'inputLength'.</param>
        private void CheckLengthValue(Int64 inputLength)
        {
            if (inputLength < 1)
            {
                throw new ArgumentException($"Parameter '{nameof(length)}' must be greater than or equal to 1.", nameof(length));
            }
        }

        /// <summary>
        /// Throws an exception if the sum of parameters 'startValue' and 'length' exceeds the limit of an Int64
        /// </summary>
        private void CheckRangeDoesNotExceedInt64Limit(Int64 startValue, Int64 length)
        {
            if (startValue >= 0)
            {
                if ((Int64.MaxValue - length) < (startValue - 1))
                {
                    throw new ArgumentException($"The specified combination of parameters '{nameof(startValue)}'={startValue} and '{nameof(length)}'={length} exceeds the maximum value of an Int64.", nameof(length));
                }
            }
        }

        #endregion
    }
}
