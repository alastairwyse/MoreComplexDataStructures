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
using NUnit.Framework;
using MoreComplexDataStructures;

namespace MoreComplexDataStructures.UnitTests
{
    /// <summary>
    /// Unit tests for the UnflaggedNumberGenerator class.
    /// </summary>
    public class UnflaggedNumberGeneratorTests
    {
        private UnflaggedNumberGenerator testUnflaggedNumberGenerator;

        [SetUp]
        protected void SetUp()
        {
        }

        /// <summary>
        /// Tests that an exception is thrown if the constructor is called with a 'rangeEnd' parameter less than the 'rangeStart' parameter.
        /// </summary>
        [Test]
        public void Constructor_RangeEndParameterLessThanRangeStart()
        {
            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                testUnflaggedNumberGenerator = new UnflaggedNumberGenerator(2, 1);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("Parameter 'rangeEnd' must be greater than or equal to parameter 'rangeStart'."));
            Assert.AreEqual("rangeEnd", e.ParamName);
        }

        /// <summary>
        /// Tests that an exception is thrown if the constructor is called with a 'rangeStart' and 'rangeEnd' parameters whose inclusive range is greater than Int64.MaxValue.
        /// </summary>
        [Test]
        public void Constructor_RangeGreaterThanInt64MaxValue()
        {
            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                testUnflaggedNumberGenerator = new UnflaggedNumberGenerator(0, 9223372036854775807);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The total inclusive range cannot exceed Int64.MaxValue."));
            Assert.AreEqual("rangeEnd", e.ParamName);


            e = Assert.Throws<ArgumentException>(delegate
            {
                testUnflaggedNumberGenerator = new UnflaggedNumberGenerator(-1, 9223372036854775806);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The total inclusive range cannot exceed Int64.MaxValue."));
            Assert.AreEqual("rangeEnd", e.ParamName);


            e = Assert.Throws<ArgumentException>(delegate
            {
                testUnflaggedNumberGenerator = new UnflaggedNumberGenerator(-2, 9223372036854775805);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The total inclusive range cannot exceed Int64.MaxValue."));
            Assert.AreEqual("rangeEnd", e.ParamName);


            e = Assert.Throws<ArgumentException>(delegate
            {
                testUnflaggedNumberGenerator = new UnflaggedNumberGenerator(-4611686018427387903, 4611686018427387904);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The total inclusive range cannot exceed Int64.MaxValue."));
            Assert.AreEqual("rangeEnd", e.ParamName);


            e = Assert.Throws<ArgumentException>(delegate
            {
                testUnflaggedNumberGenerator = new UnflaggedNumberGenerator(-9223372036854775808, -1);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The total inclusive range cannot exceed Int64.MaxValue."));
            Assert.AreEqual("rangeEnd", e.ParamName);


            e = Assert.Throws<ArgumentException>(delegate
            {
                testUnflaggedNumberGenerator = new UnflaggedNumberGenerator(-9223372036854775807, 0);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The total inclusive range cannot exceed Int64.MaxValue."));
            Assert.AreEqual("rangeEnd", e.ParamName);


            e = Assert.Throws<ArgumentException>(delegate
            {
                testUnflaggedNumberGenerator = new UnflaggedNumberGenerator(-9223372036854775806, 1);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The total inclusive range cannot exceed Int64.MaxValue."));
            Assert.AreEqual("rangeEnd", e.ParamName);
        }

        /// <summary>
        /// Success tests for the constructor where the specified range is equal to Int64.MaxValue.
        /// </summary>
        [Test]
        public void Constructor_Int64MaxValueRange()
        {
            testUnflaggedNumberGenerator = new UnflaggedNumberGenerator(1, 9223372036854775807);

            Assert.AreEqual(1, testUnflaggedNumberGenerator.RangeStart);
            Assert.AreEqual(9223372036854775807, testUnflaggedNumberGenerator.RangeEnd);

            testUnflaggedNumberGenerator = new UnflaggedNumberGenerator(0, 9223372036854775806);

            testUnflaggedNumberGenerator = new UnflaggedNumberGenerator(-1, 9223372036854775805);

            testUnflaggedNumberGenerator = new UnflaggedNumberGenerator(-4611686018427387903, 4611686018427387903);

            testUnflaggedNumberGenerator = new UnflaggedNumberGenerator(-9223372036854775806, 0);

            testUnflaggedNumberGenerator = new UnflaggedNumberGenerator(-9223372036854775807, -1);

            testUnflaggedNumberGenerator = new UnflaggedNumberGenerator(-9223372036854775808, -2);
        }

        /// <summary>
        /// Tests that an exception is thrown if the IsFlagged() method is called with a 'number' parameter less than the range of numbers specified in the constructor.
        /// </summary>
        [Test]
        public void IsFlagged_NumberParameterLessThanAllowedRange()
        {
            testUnflaggedNumberGenerator = new UnflaggedNumberGenerator(1, 5);

            ArgumentOutOfRangeException e = Assert.Throws<ArgumentOutOfRangeException>(delegate
            {
                testUnflaggedNumberGenerator.IsFlagged(0);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("Parameter 'number' with value 0 is less than minimum of the range specified in the constructor (1)."));
            Assert.AreEqual("number", e.ParamName);
        }

        /// <summary>
        /// Tests that an exception is thrown if the IsFlagged() method is called with a 'number' parameter greater than the range of numbers specified in the constructor.
        /// </summary>
        [Test]
        public void IsFlagged_NumberParameterGreaterThanAllowedRange()
        {
            testUnflaggedNumberGenerator = new UnflaggedNumberGenerator(1, 5);

            ArgumentOutOfRangeException e = Assert.Throws<ArgumentOutOfRangeException>(delegate
            {
                testUnflaggedNumberGenerator.IsFlagged(6);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("Parameter 'number' with value 6 is greater than maximum of the range specified in the constructor (5)."));
            Assert.AreEqual("number", e.ParamName);
        }

        /// <summary>
        /// Tests that an exception is thrown if the FlagNumber() method is called with a 'number' parameter less than the range of numbers specified in the constructor.
        /// </summary>
        [Test]
        public void FlagNumber_NumberParameterLessThanAllowedRange()
        {
            testUnflaggedNumberGenerator = new UnflaggedNumberGenerator(1, 5);

            ArgumentOutOfRangeException e = Assert.Throws<ArgumentOutOfRangeException>(delegate
            {
                testUnflaggedNumberGenerator.FlagNumber(0);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("Parameter 'number' with value 0 is less than minimum of the range specified in the constructor (1)."));
            Assert.AreEqual("number", e.ParamName);
        }

        /// <summary>
        /// Tests that an exception is thrown if the FlagNumber() method is called with a 'number' parameter greater than the range of numbers specified in the constructor.
        /// </summary>
        [Test]
        public void FlagNumber_NumberParameterGreaterThanAllowedRange()
        {
            testUnflaggedNumberGenerator = new UnflaggedNumberGenerator(1, 5);

            ArgumentOutOfRangeException e = Assert.Throws<ArgumentOutOfRangeException>(delegate
            {
                testUnflaggedNumberGenerator.FlagNumber(6);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("Parameter 'number' with value 6 is greater than maximum of the range specified in the constructor (5)."));
            Assert.AreEqual("number", e.ParamName);
        }

        /// <summary>
        /// Success tests for the FlagNumber() and IsFlagged() methods.
        /// </summary>
        [Test]
        public void FlagNumberIsFlagged()
        {
            testUnflaggedNumberGenerator = new UnflaggedNumberGenerator(1, 5);

            Assert.IsFalse(testUnflaggedNumberGenerator.IsFlagged(1));
            Assert.IsFalse(testUnflaggedNumberGenerator.IsFlagged(2));
            Assert.IsFalse(testUnflaggedNumberGenerator.IsFlagged(3));
            Assert.IsFalse(testUnflaggedNumberGenerator.IsFlagged(4));
            Assert.IsFalse(testUnflaggedNumberGenerator.IsFlagged(5));

            testUnflaggedNumberGenerator.FlagNumber(1);

            Assert.IsTrue(testUnflaggedNumberGenerator.IsFlagged(1));
            Assert.IsFalse(testUnflaggedNumberGenerator.IsFlagged(2));
            Assert.IsFalse(testUnflaggedNumberGenerator.IsFlagged(3));
            Assert.IsFalse(testUnflaggedNumberGenerator.IsFlagged(4));
            Assert.IsFalse(testUnflaggedNumberGenerator.IsFlagged(5));

            testUnflaggedNumberGenerator.FlagNumber(5);

            Assert.IsTrue(testUnflaggedNumberGenerator.IsFlagged(1));
            Assert.IsFalse(testUnflaggedNumberGenerator.IsFlagged(2));
            Assert.IsFalse(testUnflaggedNumberGenerator.IsFlagged(3));
            Assert.IsFalse(testUnflaggedNumberGenerator.IsFlagged(4));
            Assert.IsTrue(testUnflaggedNumberGenerator.IsFlagged(5));

            testUnflaggedNumberGenerator.FlagNumber(3);

            Assert.IsTrue(testUnflaggedNumberGenerator.IsFlagged(1));
            Assert.IsFalse(testUnflaggedNumberGenerator.IsFlagged(2));
            Assert.IsTrue(testUnflaggedNumberGenerator.IsFlagged(3));
            Assert.IsFalse(testUnflaggedNumberGenerator.IsFlagged(4));
            Assert.IsTrue(testUnflaggedNumberGenerator.IsFlagged(5));

            testUnflaggedNumberGenerator.FlagNumber(2);

            Assert.IsTrue(testUnflaggedNumberGenerator.IsFlagged(1));
            Assert.IsTrue(testUnflaggedNumberGenerator.IsFlagged(2));
            Assert.IsTrue(testUnflaggedNumberGenerator.IsFlagged(3));
            Assert.IsFalse(testUnflaggedNumberGenerator.IsFlagged(4));
            Assert.IsTrue(testUnflaggedNumberGenerator.IsFlagged(5));
        }

        /// <summary>
        /// Success tests for the GetLowestUnflaggedNumber() method where no unflagged numbers exist.
        /// </summary>
        [Test]
        public void GetLowestUnflaggedNumber_NoUnFlaggedNumbersExist()
        {
            testUnflaggedNumberGenerator = new UnflaggedNumberGenerator(1, 3);
            testUnflaggedNumberGenerator.FlagNumber(1);
            testUnflaggedNumberGenerator.FlagNumber(2);
            testUnflaggedNumberGenerator.FlagNumber(3);

            Tuple<Boolean, Int64> result = testUnflaggedNumberGenerator.GetLowestUnflaggedNumber();

            Assert.AreEqual(false, result.Item1);
            Assert.AreEqual(0, result.Item2);
        }

        /// <summary>
        /// Success tests for the GetLowestUnflaggedNumber() method.
        /// </summary>
        [Test]
        public void GetLowestUnflaggedNumber()
        {
            testUnflaggedNumberGenerator = new UnflaggedNumberGenerator(1, 8);
            testUnflaggedNumberGenerator.FlagNumber(1);
            testUnflaggedNumberGenerator.FlagNumber(5);
            testUnflaggedNumberGenerator.FlagNumber(7);

            Tuple<Boolean, Int64> result = testUnflaggedNumberGenerator.GetLowestUnflaggedNumber();

            Assert.AreEqual(true, result.Item1);
            Assert.AreEqual(2, result.Item2);
        }

        /// <summary>
        /// Success tests for the GetHighestUnflaggedNumber() method where no unflagged numbers exist.
        /// </summary>
        [Test]
        public void GetHighestUnflaggedNumber_NoUnFlaggedNumbersExist()
        {
            testUnflaggedNumberGenerator = new UnflaggedNumberGenerator(1, 3);
            testUnflaggedNumberGenerator.FlagNumber(1);
            testUnflaggedNumberGenerator.FlagNumber(2);
            testUnflaggedNumberGenerator.FlagNumber(3);

            Tuple<Boolean, Int64> result = testUnflaggedNumberGenerator.GetHighestUnflaggedNumber();

            Assert.AreEqual(false, result.Item1);
            Assert.AreEqual(0, result.Item2);
        }

        /// <summary>
        /// Success tests for the GetHighestUnflaggedNumber() method.
        /// </summary>
        [Test]
        public void GetHighestUnflaggedNumber()
        {
            testUnflaggedNumberGenerator = new UnflaggedNumberGenerator(1, 8);
            testUnflaggedNumberGenerator.FlagNumber(8);
            testUnflaggedNumberGenerator.FlagNumber(4);
            testUnflaggedNumberGenerator.FlagNumber(2);

            Tuple<Boolean, Int64> result = testUnflaggedNumberGenerator.GetHighestUnflaggedNumber();

            Assert.AreEqual(true, result.Item1);
            Assert.AreEqual(7, result.Item2);
        }

        /// <summary>
        /// Tests that an exception is thrown if the GetLowestUnflaggedNumbers() method is called with a 'numberCount' parameter less than 1.
        /// </summary>
        [Test]
        public void GetLowestUnflaggedNumbers_NumberCountParameterLessThan1()
        {
            ArgumentOutOfRangeException e = Assert.Throws<ArgumentOutOfRangeException>(delegate
            {
                IEnumerable<Int64> result = testUnflaggedNumberGenerator.GetLowestUnflaggedNumbers(0);
                result.Count();
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("Parameter 'numberCount' must be greater than or equal to 1."));
            Assert.AreEqual("numberCount", e.ParamName);
        }

        /// <summary>
        /// Success tests for the GetLowestUnflaggedNumbers() method.
        /// </summary>
        [Test]
        public void GetLowestUnflaggedNumbers()
        {
            testUnflaggedNumberGenerator = new UnflaggedNumberGenerator(1, 8);
            testUnflaggedNumberGenerator.FlagNumber(2);
            testUnflaggedNumberGenerator.FlagNumber(5);
            testUnflaggedNumberGenerator.FlagNumber(6);

            var result = new List<Int64>(testUnflaggedNumberGenerator.GetLowestUnflaggedNumbers(1));

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(1, result[0]);


            result = new List<Int64>(testUnflaggedNumberGenerator.GetLowestUnflaggedNumbers(3));

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(3, result[1]);
            Assert.AreEqual(4, result[2]);


            result = new List<Int64>(testUnflaggedNumberGenerator.GetLowestUnflaggedNumbers(5));

            Assert.AreEqual(5, result.Count);
            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(3, result[1]);
            Assert.AreEqual(4, result[2]);
            Assert.AreEqual(7, result[3]);
            Assert.AreEqual(8, result[4]);


            result = new List<Int64>(testUnflaggedNumberGenerator.GetLowestUnflaggedNumbers(6));

            Assert.AreEqual(5, result.Count);
            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(3, result[1]);
            Assert.AreEqual(4, result[2]);
            Assert.AreEqual(7, result[3]);
            Assert.AreEqual(8, result[4]);
        }

        /// <summary>
        /// Tests that an exception is thrown if the GetHighestUnflaggedNumbers() method is called with a 'numberCount' parameter less than 1.
        /// </summary>
        [Test]
        public void GetHighestUnflaggedNumbers_NumberCountParameterLessThan1()
        {
            ArgumentOutOfRangeException e = Assert.Throws<ArgumentOutOfRangeException>(delegate
            {
                IEnumerable<Int64> result = testUnflaggedNumberGenerator.GetHighestUnflaggedNumbers(0);
                result.Count();
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("Parameter 'numberCount' must be greater than or equal to 1."));
            Assert.AreEqual("numberCount", e.ParamName);
        }
        
        /// <summary>
        /// Success tests for the GetHighestUnflaggedNumbers() method.
        /// </summary>
        [Test]
        public void GetHighestUnflaggedNumbers()
        {
            testUnflaggedNumberGenerator = new UnflaggedNumberGenerator(1, 8);
            testUnflaggedNumberGenerator.FlagNumber(2);
            testUnflaggedNumberGenerator.FlagNumber(5);
            testUnflaggedNumberGenerator.FlagNumber(6);

            var result = new List<Int64>(testUnflaggedNumberGenerator.GetHighestUnflaggedNumbers(1));

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(8, result[0]);


            result = new List<Int64>(testUnflaggedNumberGenerator.GetHighestUnflaggedNumbers(3));

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(8, result[0]);
            Assert.AreEqual(7, result[1]);
            Assert.AreEqual(4, result[2]);


            result = new List<Int64>(testUnflaggedNumberGenerator.GetHighestUnflaggedNumbers(5));

            Assert.AreEqual(5, result.Count);
            Assert.AreEqual(8, result[0]);
            Assert.AreEqual(7, result[1]);
            Assert.AreEqual(4, result[2]);
            Assert.AreEqual(3, result[3]);
            Assert.AreEqual(1, result[4]);


            result = new List<Int64>(testUnflaggedNumberGenerator.GetHighestUnflaggedNumbers(6));

            Assert.AreEqual(5, result.Count);
            Assert.AreEqual(8, result[0]);
            Assert.AreEqual(7, result[1]);
            Assert.AreEqual(4, result[2]);
            Assert.AreEqual(3, result[3]);
            Assert.AreEqual(1, result[4]);
        }
    }
}
