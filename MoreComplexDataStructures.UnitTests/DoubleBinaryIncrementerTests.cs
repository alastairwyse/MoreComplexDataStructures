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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using MoreComplexDataStructures;

namespace MoreComplexDataStructures.UnitTests
{
    /// <summary>
    /// Unit tests for the DoubleBinaryIncrementer class.
    /// </summary>
    public class DoubleBinaryIncrementerTests
    {
        private DoubleBinaryIncrementer testDoubleBinaryIncrementer;

        [SetUp]
        protected void SetUp()
        {
            testDoubleBinaryIncrementer = new DoubleBinaryIncrementer();
        }

        [Test]
        public void IncrementNaN()
        {
            Double result = testDoubleBinaryIncrementer.Increment(Double.NaN);

            Assert.IsTrue(Double.IsNaN(result));
        }

        [Test]
        public void DecrementNaN()
        {
            Double result = testDoubleBinaryIncrementer.Decrement(Double.NaN);

            Assert.IsTrue(Double.IsNaN(result));
        }

        [Test]
        public void IncrementPositiveInfinity()
        {
            Double result = testDoubleBinaryIncrementer.Increment(Double.PositiveInfinity);

            Assert.IsTrue(Double.IsPositiveInfinity(result));
        }

        [Test]
        public void DecrementPositiveInfinity()
        {
            Double result = testDoubleBinaryIncrementer.Decrement(Double.PositiveInfinity);

            Assert.AreEqual(Double.MaxValue, result);
        }

        [Test]
        public void IncrementLargestPositive()
        {
            Double result = testDoubleBinaryIncrementer.Increment(Double.MaxValue);

            Assert.IsTrue(Double.IsPositiveInfinity(result));
        }

        [Test]
        public void DecrementLargestPositive()
        {
            Double result = testDoubleBinaryIncrementer.Decrement(Double.MaxValue);

            Assert.AreEqual(BitConverter.Int64BitsToDouble(0x7feffffffffffffe), result);
        }

        [Test]
        public void IncrementSmallestPositive()
        {
            Double result = testDoubleBinaryIncrementer.Increment(testDoubleBinaryIncrementer.SmallestPositiveValue);

            Assert.AreEqual(BitConverter.Int64BitsToDouble(0x0000000000000002), result);
        }

        [Test]
        public void DecrementSmallestPositive()
        {
            Double result = testDoubleBinaryIncrementer.Decrement(testDoubleBinaryIncrementer.SmallestPositiveValue);

            Assert.AreEqual(0L, result);
        }

        [Test]
        public void IncrementPositiveZero()
        {
            Double result = testDoubleBinaryIncrementer.Increment(testDoubleBinaryIncrementer.PositiveZero);

            Assert.AreEqual(testDoubleBinaryIncrementer.SmallestPositiveValue, result);
        }

        [Test]
        public void DecrementPositiveZero()
        {
            Double result = testDoubleBinaryIncrementer.Decrement(testDoubleBinaryIncrementer.PositiveZero);

            Assert.AreEqual(testDoubleBinaryIncrementer.SmallestNegativeValue, result);
        }

        [Test]
        public void IncrementNegativeZero()
        {
            Double result = testDoubleBinaryIncrementer.Increment(testDoubleBinaryIncrementer.NegativeZero);

            Assert.AreEqual(testDoubleBinaryIncrementer.SmallestPositiveValue, result);
        }

        [Test]
        public void DecrementNegativeZero()
        {
            Double result = testDoubleBinaryIncrementer.Decrement(testDoubleBinaryIncrementer.NegativeZero);

            Assert.AreEqual(testDoubleBinaryIncrementer.SmallestNegativeValue, result);
        }

        [Test]
        public void IncrementSmallestNegative()
        {
            Double result = testDoubleBinaryIncrementer.Increment(testDoubleBinaryIncrementer.SmallestNegativeValue);

            Assert.AreEqual(0L, result);
        }

        [Test]
        public void DecrementSmallestNegative()
        {
            Double result = testDoubleBinaryIncrementer.Decrement(testDoubleBinaryIncrementer.SmallestNegativeValue);

            Assert.AreEqual(BitConverter.Int64BitsToDouble(unchecked((Int64)0x8000000000000002)), result);
        }

        [Test]
        public void IncrementLargestNegative()
        {
            Double result = testDoubleBinaryIncrementer.Increment(Double.MinValue);

            Assert.AreEqual(BitConverter.Int64BitsToDouble(unchecked((Int64)0xffeffffffffffffe)), result);
        }

        [Test]
        public void DecrementLargestNegative()
        {
            Double result = testDoubleBinaryIncrementer.Decrement(Double.MinValue);

            Assert.AreEqual(Double.NegativeInfinity, result);
        }

        [Test]
        public void IncrementNegativeInfinity()
        {
            Double result = testDoubleBinaryIncrementer.Increment(Double.NegativeInfinity);

            Assert.AreEqual(Double.MinValue, result);
        }

        [Test]
        public void DecrementNegativeInfinity()
        {
            Double result = testDoubleBinaryIncrementer.Decrement(Double.NegativeInfinity);

            Assert.IsTrue(Double.IsNegativeInfinity(result));
        }

        [Test]
        public void IncrementPositiveDoubleGreaterThanOne()
        {
            Double result = testDoubleBinaryIncrementer.Increment(1234.56789d);

            Assert.AreEqual(BitConverter.Int64BitsToDouble(0x40934a4584f4c6e8), result);
        }

        [Test]
        public void DecrementPositiveDoubleGreaterThanOne()
        {
            Double result = testDoubleBinaryIncrementer.Decrement(1234.56789d);

            Assert.AreEqual(BitConverter.Int64BitsToDouble(0x40934a4584f4c6e6), result);
        }

        [Test]
        public void IncrementPositiveDoubleLessThanOne()
        {
            Double result = testDoubleBinaryIncrementer.Increment(0.0000123456789d);

            Assert.AreEqual(BitConverter.Int64BitsToDouble(0x3ee9e409301b5a03), result);
        }

        [Test]
        public void DecrementPositiveDoubleLessThanOne()
        {
            Double result = testDoubleBinaryIncrementer.Decrement(0.0000123456789d);

            Assert.AreEqual(BitConverter.Int64BitsToDouble(0x3ee9e409301b5a01), result);
        }

        [Test]
        public void IncrementNegativeDoubleLessThanNegativeOne()
        {
            Double result = testDoubleBinaryIncrementer.Increment(-1234.56789d);

            Assert.AreEqual(BitConverter.Int64BitsToDouble(unchecked((Int64)0xc0934a4584f4c6e6)), result);
        }

        [Test]
        public void DecrementNegativeDoubleLessThanNegativeOne()
        {
            Double result = testDoubleBinaryIncrementer.Decrement(-1234.56789d);

            Assert.AreEqual(BitConverter.Int64BitsToDouble(unchecked((Int64)0xc0934a4584f4c6e8)), result);
        }

        [Test]
        public void IncrementNegativeDoubleGreaterThanNegativeOne()
        {
            Double result = testDoubleBinaryIncrementer.Increment(-0.0000123456789d);

            Assert.AreEqual(BitConverter.Int64BitsToDouble(unchecked((Int64)0xbee9e409301b5a01)), result);
        }

        [Test]
        public void DecrementNegativeDoubleGreaterThanNegativeOne()
        {
            Double result = testDoubleBinaryIncrementer.Decrement(-0.0000123456789d);

            Assert.AreEqual(BitConverter.Int64BitsToDouble(unchecked((Int64)0xbee9e409301b5a03)), result);
        }
    }
}
