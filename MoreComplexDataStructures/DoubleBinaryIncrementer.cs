/*
 * Copyright 2019 Alastair Wyse (https://github.com/alastairwyse/MoreComplexDataStructures/)
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
using System.Text;

namespace MoreComplexDataStructures
{
    /// <summary>
    /// Increments and decrements the binary representation of double values (i.e. to provide the next value after and next value before a double value).
    /// </summary>
    public class DoubleBinaryIncrementer
    {
        /// <summary>The largest possible positive double value.</summary>
        protected Double largestPositiveValue;
        /// <summary>The smallest possible positive double value.</summary>
        protected Double smallestPositiveValue;
        /// <summary>Positive 0.</summary>
        protected Double positiveZero;
        /// <summary>Negative 0.</summary>
        protected Double negativeZero;
        /// <summary>The smallest possible negative double value.</summary>
        protected Double smallestNegativeValue;
        /// <summary>The largest possible negative double value.</summary>
        protected Double largestNegativeValue;
        /// <summary>Maps a double value to delegates which perform binary increment and decrement functions on that double.</summary>
        protected Dictionary<Double, IncrementAndDecrementFunctions> doubleToDelegateMap;

        /// <summary>
        /// The smallest possible positive double value.
        /// </summary>
        public Double SmallestPositiveValue
        {
            get { return smallestPositiveValue; }
        }

        /// <summary>
        /// Positive 0.
        /// </summary>
        public Double PositiveZero
        {
            get { return positiveZero; }
        }

        /// <summary>
        /// Negative 0.
        /// </summary>
        public Double NegativeZero
        {
            get { return negativeZero; }
        }

        /// <summary>
        /// The smallest possible negative double value.
        /// </summary>
        public Double SmallestNegativeValue
        {
            get { return smallestNegativeValue; }
        }

        /// <summary>
        /// Initialises a new instance of the MoreComplexDataStructures.DoubleBinaryIncrementer class.
        /// </summary>
        public DoubleBinaryIncrementer()
        {
            largestPositiveValue = Double.MaxValue;
            smallestPositiveValue = BitConverter.Int64BitsToDouble(0x0000000000000001);
            positiveZero = BitConverter.Int64BitsToDouble(0x0000000000000000);
            negativeZero = BitConverter.Int64BitsToDouble(unchecked((Int64)0x8000000000000000));
            smallestNegativeValue = BitConverter.Int64BitsToDouble(unchecked((Int64)0x8000000000000001));
            largestNegativeValue = Double.MinValue;
            InitializeDelegateMap();
        }

        /// <summary>
        /// Increments the specified double value (i.e. gets the next double after).
        /// </summary>
        /// <param name="inputDouble">The double value to increment.</param>
        /// <returns>The next double value.</returns>
        public Double Increment(Double inputDouble)
        {
            if (doubleToDelegateMap.ContainsKey(inputDouble))
                return doubleToDelegateMap[inputDouble].IncrementDelegate.Invoke(inputDouble);
            else
                return PerformBinaryIncrement(inputDouble);
        }

        /// <summary>
        /// Decrements the specified double value (i.e. gets the next double before).
        /// </summary>
        /// <param name="inputDouble">The double value to decrement.</param>
        /// <returns>The previous double value.</returns>
        public Double Decrement(Double inputDouble)
        {
            if (doubleToDelegateMap.ContainsKey(inputDouble))
                return doubleToDelegateMap[inputDouble].DecrementDelegate.Invoke(inputDouble);
            else
                return PerformBinaryDecrement(inputDouble);
        }

        /// <summary>
        /// Initializes field 'doubleToDelegateMap'.
        /// </summary>
        protected void InitializeDelegateMap()
        {
            doubleToDelegateMap = new Dictionary<Double, IncrementAndDecrementFunctions>()
            {
                {
                    Double.PositiveInfinity,
                    new IncrementAndDecrementFunctions
                    (
                        // Increment function
                        new Func<Double, Double>((inputDouble) => { return Double.PositiveInfinity; }),
                        // Decrement function
                        new Func<Double, Double>((inputDouble) => { return largestPositiveValue; })
                    )
                },
                {
                    largestPositiveValue,
                    new IncrementAndDecrementFunctions
                    (
                        new Func<Double, Double>((inputDouble) => { return Double.PositiveInfinity; }),
                        new Func<Double, Double>((inputDouble) => { return PerformBinaryDecrement(inputDouble); })
                    )
                },
                {
                    smallestPositiveValue,
                    new IncrementAndDecrementFunctions
                    (
                        new Func<Double, Double>((inputDouble) => { return PerformBinaryIncrement(inputDouble); }),
                        new Func<Double, Double>((inputDouble) => { return positiveZero; })
                    )
                }, 
                {
                    positiveZero,
                    new IncrementAndDecrementFunctions
                    (
                        new Func<Double, Double>((inputDouble) => { return smallestPositiveValue; }),
                        new Func<Double, Double>((inputDouble) => { return smallestNegativeValue; })
                    )
                },
                {
                    smallestNegativeValue,
                    new IncrementAndDecrementFunctions
                    (
                        new Func<Double, Double>((inputDouble) => { return positiveZero; }),
                        new Func<Double, Double>((inputDouble) => { return PerformBinaryDecrement(inputDouble); })
                    )
                },
                {
                    largestNegativeValue,
                    new IncrementAndDecrementFunctions
                    (
                        new Func<Double, Double>((inputDouble) => { return PerformBinaryIncrement(inputDouble); }),
                        new Func<Double, Double>((inputDouble) => { return Double.NegativeInfinity; })
                    )
                },
                {
                    Double.NegativeInfinity,
                    new IncrementAndDecrementFunctions
                    (
                        new Func<Double, Double>((inputDouble) => { return largestNegativeValue; }),
                        new Func<Double, Double>((inputDouble) => { return Double.NegativeInfinity; })
                    )
                },
                {
                    Double.NaN,
                    new IncrementAndDecrementFunctions
                    (
                        new Func<Double, Double>((inputDouble) => { return Double.NaN; }),
                        new Func<Double, Double>((inputDouble) => { return Double.NaN; })
                    )
                }
            };
        }

        /// <summary>
        /// Performs a binary increment operation on the specified double.
        /// </summary>
        /// <param name="inputDouble">The double to increment.</param>
        /// <returns>The double after incrementing.</returns>
        protected Double PerformBinaryIncrement(Double inputDouble)
        {
            Int64 integerRespresentation = BitConverter.DoubleToInt64Bits(inputDouble);
            if (inputDouble < 0)
                integerRespresentation--;
            else
                integerRespresentation++;

            return BitConverter.Int64BitsToDouble(integerRespresentation);
        }

        /// <summary>
        /// Performs a binary decrement operation on the specified double.
        /// </summary>
        /// <param name="inputDouble">The double to decrement.</param>
        /// <returns>The double after decrementing.</returns>
        protected Double PerformBinaryDecrement(Double inputDouble)
        {
            Int64 integerRespresentation = BitConverter.DoubleToInt64Bits(inputDouble);
            if (inputDouble < 0)
                integerRespresentation++;
            else
                integerRespresentation--;

            return BitConverter.Int64BitsToDouble(integerRespresentation);
        }

        /// <summary>
        /// Contains delegates which increment and decrement double values.
        /// </summary>
        protected class IncrementAndDecrementFunctions
        {
            /// <summary>The delegate used to increment a double.</summary>
            protected Func<Double, Double> incrementDelegate;
            /// <summary>The delegate used to decrement a double.</summary>
            protected Func<Double, Double> decrementDelegate;

            /// <summary>
            /// The delegate used to increment a double.
            /// </summary>
            public Func<Double, Double> IncrementDelegate
            {
                get { return incrementDelegate; }
            }

            /// <summary>
            /// The delegate used to decrement a double.
            /// </summary>
            public Func<Double, Double> DecrementDelegate
            {
                get { return decrementDelegate; }
            }

            /// <summary>
            /// Initialises a new instance of the MoreComplexDataStructures.DoubleBinaryIncrementer+IncrementAndDecrementFunctions class.
            /// </summary>
            /// <param name="incrementDelegate">The delegate used to increment a double.</param>
            /// <param name="decrementDelegate">The delegate used to decrement a double.</param>
            public IncrementAndDecrementFunctions(Func<Double, Double> incrementDelegate, Func<Double, Double> decrementDelegate)
            {
                this.incrementDelegate = incrementDelegate;
                this.decrementDelegate = decrementDelegate;
            }
        }
    }
}
