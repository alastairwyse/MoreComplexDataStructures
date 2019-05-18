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
using NUnit.Framework;
using MoreComplexDataStructures;

namespace MoreComplexDataStructures.UnitTests
{
    /// <summary>
    /// Unit tests for the LongIntegerRange class.
    /// </summary>
    public class LongIntegerRangeTests
    {
        /// <summary>
        /// Tests that an exception is thrown if the 'StartValue' property is set which makes the range exceed the limit of an Int64 variable.
        /// </summary>
        [Test]
        public void StartValue_RangeExceedsInt64Limit()
        {
            LongIntegerRange testLongIntegerRange = new LongIntegerRange(1, Int64.MaxValue);

            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                testLongIntegerRange.StartValue = 2;
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The specified combination of parameters 'startValue'=2 and 'length'=9223372036854775807 exceeds the maximum value of an Int64."));
            Assert.AreEqual("length", e.ParamName);
        }

        /// <summary>
        /// Success tests for setting the 'StartValue' property.
        /// </summary>
        /// <remarks>Mainly tests not that values are stored correctly, but that exceptions are not thrown with boundary values.</remarks>
        [Test]
        public void StartValue()
        {
            // Test with max and min Int64 values
            LongIntegerRange testLongIntegerRange = new LongIntegerRange(Int64.MinValue, 1);
            testLongIntegerRange.StartValue = Int64.MaxValue;

            Assert.AreEqual(Int64.MaxValue, testLongIntegerRange.StartValue);


            testLongIntegerRange.StartValue = 1;
            testLongIntegerRange.Length = Int64.MaxValue;

            Assert.AreEqual(1, testLongIntegerRange.StartValue);
        }

        /// <summary>
        /// Tests that an exception is thrown if the 'Length' property is set with a value less than 0.
        /// </summary>
        [Test]
        public void Length_ValueLessThan1()
        {
            LongIntegerRange testLongIntegerRange = new LongIntegerRange(1, 1);

            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                testLongIntegerRange.Length = 0;
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("Parameter 'length' must be greater than or equal to 1."));
            Assert.AreEqual("length", e.ParamName);
        }

        /// <summary>
        /// Tests that an exception is thrown if the 'Length' property is set which makes the range exceed the limit of an Int64 variable.
        /// </summary>
        [Test]
        public void Length_RangeExceedsInt64Limit()
        {
            LongIntegerRange testLongIntegerRange = new LongIntegerRange(Int64.MaxValue, 1);

            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                testLongIntegerRange.Length = 2;
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The specified combination of parameters 'startValue'=9223372036854775807 and 'length'=2 exceeds the maximum value of an Int64."));
            Assert.AreEqual("length", e.ParamName);


            testLongIntegerRange.StartValue = 2;
            testLongIntegerRange.Length = Int64.MaxValue - 1;

            e = Assert.Throws<ArgumentException>(delegate
            {
                testLongIntegerRange.Length = Int64.MaxValue;
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The specified combination of parameters 'startValue'=2 and 'length'=9223372036854775807 exceeds the maximum value of an Int64."));
            Assert.AreEqual("length", e.ParamName);
        }

        /// <summary>
        /// Success tests for setting the 'Length' property.
        /// </summary>
        /// <remarks>Mainly tests not that values are stored correctly, but that exceptions are not thrown with boundary values.</remarks>
        [Test]
        public void Length()
        {
            // Test with max and min Int64 values
            LongIntegerRange testLongIntegerRange = new LongIntegerRange(1, 1);
            testLongIntegerRange.Length = Int64.MaxValue;

            Assert.AreEqual(Int64.MaxValue, testLongIntegerRange.Length);


            testLongIntegerRange = new LongIntegerRange(Int64.MaxValue, 1);
            testLongIntegerRange.Length = 1;

            Assert.AreEqual(1, testLongIntegerRange.Length);


            testLongIntegerRange = new LongIntegerRange(Int64.MinValue, 1);
            testLongIntegerRange.Length = 1;

            Assert.AreEqual(1, testLongIntegerRange.Length);


            testLongIntegerRange.Length = Int64.MaxValue;

            Assert.AreEqual(Int64.MaxValue, testLongIntegerRange.Length);
        }

        /// <summary>
        /// Success tests for setting the 'EndValue' property.
        /// </summary>
        [Test]
        public void EndValue()
        {
            LongIntegerRange testLongIntegerRange = new LongIntegerRange(1, 1);
            Assert.AreEqual(1, testLongIntegerRange.EndValue);

            testLongIntegerRange = new LongIntegerRange(2, 5);
            Assert.AreEqual(6, testLongIntegerRange.EndValue);

            testLongIntegerRange = new LongIntegerRange(-10, 3);
            Assert.AreEqual(-8, testLongIntegerRange.EndValue);

            testLongIntegerRange = new LongIntegerRange(-3, 9);
            Assert.AreEqual(5, testLongIntegerRange.EndValue);

            testLongIntegerRange = new LongIntegerRange(Int64.MaxValue, 1);
            Assert.AreEqual(Int64.MaxValue, testLongIntegerRange.EndValue);

            testLongIntegerRange = new LongIntegerRange(Int64.MinValue, 1);
            Assert.AreEqual(Int64.MinValue, testLongIntegerRange.EndValue);

            testLongIntegerRange = new LongIntegerRange(1, Int64.MaxValue);
            Assert.AreEqual(Int64.MaxValue, testLongIntegerRange.EndValue);

            testLongIntegerRange = new LongIntegerRange(Int64.MinValue, Int64.MaxValue);
            Assert.AreEqual(-2, testLongIntegerRange.EndValue);
        }

        /// <summary>
        /// Tests that an exception is thrown if the object is constructed with a 'length' parameter less than 1.
        /// </summary>
        [Test]
        public void Constructor_LengthLessThan1()
        {
            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                LongIntegerRange testLongIntegerRange = new LongIntegerRange(1, 0);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("Parameter 'length' must be greater than or equal to 1."));
            Assert.AreEqual("length", e.ParamName);
        }

        /// <summary>
        /// Tests that an exception is thrown if the object is constructed with parameters which makes the range exceed the limit of an Int64 variable.
        /// </summary>
        [Test]
        public void Constructor_RangeExceedsInt64Limit()
        {
            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                LongIntegerRange testLongIntegerRange = new LongIntegerRange(Int64.MaxValue, 2);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The specified combination of parameters 'startValue'=9223372036854775807 and 'length'=2 exceeds the maximum value of an Int64."));
            Assert.AreEqual("length", e.ParamName);


            e = Assert.Throws<ArgumentException>(delegate
            {
                LongIntegerRange testLongIntegerRange = new LongIntegerRange(2, Int64.MaxValue);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The specified combination of parameters 'startValue'=2 and 'length'=9223372036854775807 exceeds the maximum value of an Int64."));
            Assert.AreEqual("length", e.ParamName);
        }

        /// <summary>
        /// Success tests for the CompareTo() method.
        /// </summary>
        [Test]
        public void CompareTo()
        {
            // Test with different 'startValue' and varying 'length' parameters (length should have no effect on return value from CompareTo())
            LongIntegerRange lower = new LongIntegerRange(1, 5);
            LongIntegerRange higher = new LongIntegerRange(5, 6);

            Int32 result = lower.CompareTo(higher);
            Int32 oppositeResult = higher.CompareTo(lower);

            Assert.AreEqual(-1, result);
            Assert.AreEqual(1, oppositeResult);
            

            higher = new LongIntegerRange(5, 5);

            result = lower.CompareTo(higher);
            oppositeResult = higher.CompareTo(lower);

            Assert.AreEqual(-1, result);
            Assert.AreEqual(1, oppositeResult);


            higher = new LongIntegerRange(2, 2);

            result = lower.CompareTo(higher);
            oppositeResult = higher.CompareTo(lower);

            Assert.AreEqual(-1, result);
            Assert.AreEqual(1, oppositeResult);


            // Test with matching 'startValue' and varying 'length' parameters (length should have no effect on return value from CompareTo())
            higher = new LongIntegerRange(1, 4);

            result = lower.CompareTo(higher);
            oppositeResult = higher.CompareTo(lower);

            Assert.AreEqual(0, result);
            Assert.AreEqual(0, oppositeResult);


            higher = new LongIntegerRange(1, 5);

            result = lower.CompareTo(higher);
            oppositeResult = higher.CompareTo(lower);

            Assert.AreEqual(0, result);
            Assert.AreEqual(0, oppositeResult);


            higher = new LongIntegerRange(1, 6);

            result = lower.CompareTo(higher);
            oppositeResult = higher.CompareTo(lower);

            Assert.AreEqual(0, result);
            Assert.AreEqual(0, oppositeResult);
        }
    }
}
