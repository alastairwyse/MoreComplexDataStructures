/*
 * Copyright 2018 Alastair Wyse (https://github.com/alastairwyse/MoreComplexDataStructures/)
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
    /// Allows the 'flagging' (marking) of numbers in a specified range, and provides methods to identify numbers which were not flagged.
    /// </summary>
    public class UnflaggedNumberGenerator
    {
        /// <summary>The inclusive start of the range of numbers to permit flagging of.</summary>
        protected Int64 rangeStart;
        /// <summary>The inclusive end of the range of numbers to permit flagging of.</summary>
        protected Int64 rangeEnd;
        /// <summary>Underlying LongIntegerStatusStorer used to record which numbers have been flagged.</summary>
        protected LongIntegerStatusStorer rangeStorer;

        /// <summary>
        /// The inclusive start of the range of numbers to permit flagging of.
        /// </summary>
        public Int64 RangeStart
        {
            get { return rangeStart; }
        }

        /// <summary>
        /// The inclusive end of the range of numbers to permit flagging of.
        /// </summary>
        public Int64 RangeEnd
        {
            get { return rangeEnd; }
        }

        /// <summary>
        /// Initialises a new instance of the MoreComplexDataStructures.UnflaggedNumberGenerator class.
        /// </summary>
        /// <param name="rangeStart">The inclusive start of the range of numbers to permit flagging of.</param>
        /// <param name="rangeEnd">The inclusive end of the range of numbers to permit flagging of.</param>
        /// <exception cref="System.ArgumentException">Parameter 'rangeEnd' is less than parameter 'rangeStart'.</exception>
        /// <exception cref="System.ArgumentException">The total inclusive range exceeds Int64.MaxValue.</exception>
        public UnflaggedNumberGenerator(Int64 rangeStart, Int64 rangeEnd)
        {
            this.rangeStart = rangeStart;
            this.rangeEnd = rangeEnd;
            rangeStorer = new LongIntegerStatusStorer();
            rangeStorer.SetRangeTrue(rangeStart, rangeEnd);
        }

        /// <summary>
        /// Returns whether or not the specified number has been flagged.
        /// </summary>
        /// <param name="number">The number to check.</param>
        /// <returns>True if the number has been flagged, otherwise false.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">The specified number is outside the range of numbers specified in the constructor.</exception>
        public Boolean IsFlagged(Int64 number)
        {
            ThrowExeptionIfNumberOutsideRange(number, "number");

            return !rangeStorer.GetStatus(number);
        }

        /// <summary>
        /// Marks the specified number as flagged.
        /// </summary>
        /// <param name="number">The number to flag.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">The specified number is outside the range of numbers specified in the constructor.</exception>
        public void FlagNumber(Int64 number)
        {
            ThrowExeptionIfNumberOutsideRange(number, "number");

            rangeStorer.SetStatusFalse(number);
        }

        /// <summary>
        /// Returns the lowest unflagged number in the range.
        /// </summary>
        /// <returns>A tuple containing 2 values: a boolean indicating whether any flagged numbers exist (false if no flagged numbers exist), and the lowest unflagged number in the range (or 0 if no unflagged numbers exist).</returns>
        public Tuple<Boolean, Int64> GetLowestUnflaggedNumber()
        {
            if (rangeStorer.Count == 0)
            {
                return new Tuple<Boolean, Int64>(false, 0);
            }
            else
            {
                LongIntegerRange lowestRange = rangeStorer.MinimumRange;

                return new Tuple<Boolean, Int64>(true, lowestRange.StartValue);
            }
        }

        /// <summary>
        /// Returns the highest unflagged number in the range.
        /// </summary>
        /// <returns>A tuple containing 2 values: a boolean indicating whether any flagged numbers exist (false if no flagged numbers exist), and the highest unflagged number in the range (or 0 if no unflagged numbers exist).</returns>
        public Tuple<Boolean, Int64> GetHighestUnflaggedNumber()
        {
            if (rangeStorer.Count == 0)
            {
                return new Tuple<Boolean, Int64>(false, 0);
            }
            else
            {
                LongIntegerRange highestRange = rangeStorer.MaximumRange;

                return new Tuple<Boolean, Int64>(true, highestRange.StartValue + highestRange.Length - 1);
            }
        }

        /// <summary>
        /// Returns up to the specified number of unflagged numbers starting at the lowest.
        /// </summary>
        /// <remarks>If less than the specified count of unflagged numbers exist, all available numbers will be returned (which would be less than the specified count).</remarks>
        /// <param name="numberCount">The count of numbers to return.</param>
        /// <returns>The unflagged numbers.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameter 'numberCount' is less than 1.</exception>
        public IEnumerable<Int64> GetLowestUnflaggedNumbers(Int64 numberCount)
        {
            if (numberCount < 1)
                throw new ArgumentOutOfRangeException(nameof(numberCount), $"Parameter '{nameof(numberCount)}' must be greater than or equal to 1.");

            Int64 returnedNumberCount = 0;
            foreach (LongIntegerRange currentRange in rangeStorer.GetAllRangesAscending())
            {
                for (Int64 currUnflaggedNumber = currentRange.StartValue; currUnflaggedNumber < currentRange.StartValue + currentRange.Length; currUnflaggedNumber++)
                {
                    yield return currUnflaggedNumber;
                    returnedNumberCount++;
                    if (returnedNumberCount == numberCount)
                        break;
                }
                if (returnedNumberCount == numberCount)
                    break;
            }
        }

        /// <summary>
        /// Returns up to the specified number of unflagged numbers starting at the highest.
        /// </summary>
        /// <remarks>If less than the specified count of unflagged numbers exist, all available numbers will be returned (which would be less than the specified count).</remarks>
        /// <param name="numberCount">The count of numbers to return.</param>
        /// <returns>The unflagged numbers.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameter 'numberCount' is less than 1.</exception>
        public IEnumerable<Int64> GetHighestUnflaggedNumbers(Int64 numberCount)
        {
            if (numberCount < 1)
                throw new ArgumentOutOfRangeException(nameof(numberCount), $"Parameter '{nameof(numberCount)}' must be greater than or equal to 1.");

            Int64 returnedNumberCount = 0;
            foreach (LongIntegerRange currentRange in rangeStorer.GetAllRangesDescending())
            {
                for (Int64 currUnflaggedNumber = currentRange.StartValue + currentRange.Length - 1; currUnflaggedNumber >= currentRange.StartValue; currUnflaggedNumber--)
                {
                    yield return currUnflaggedNumber;
                    returnedNumberCount++;
                    if (returnedNumberCount == numberCount)
                        break;
                }
                if (returnedNumberCount == numberCount)
                    break;
            }
        }

        # region Private/Protected Methods

        /// <summary>
        /// Throws and exception if the specified number is outside the range of numbers specified in the constructor.
        /// </summary>
        /// <param name="number">The number to check.</param>
        /// <param name="parameterName">The name of the parameter the number was passed in.</param>
        protected void ThrowExeptionIfNumberOutsideRange(Int64 number, String parameterName)
        {
            if (number < rangeStart)
                throw new ArgumentOutOfRangeException(parameterName, $"Parameter '{parameterName}' with value {number} is less than minimum of the range specified in the constructor ({rangeStart}).");
            if (number > rangeEnd)
                throw new ArgumentOutOfRangeException(parameterName, $"Parameter '{parameterName}' with value {number} is greater than maximum of the range specified in the constructor ({rangeEnd}).");
        }

        #endregion
    }
}
