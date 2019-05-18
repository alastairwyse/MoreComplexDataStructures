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
    /// Unit tests for the LongIntegerStatusStorer class.
    /// </summary>
    public class LongIntegerStatusStorerTests
    {
        private LongIntegerStatusStorer testLongIntegerStatusStorer;

        [SetUp]
        protected void SetUp()
        {
            testLongIntegerStatusStorer = new LongIntegerStatusStorer();
        }

        /// <summary>
        /// Tests that an exception is thrown when calling the SetStatusTrue() method would result in merging two ranges whose new total range would exceed Int64.MaxValue.
        /// </summary>
        [Test]
        public void SetStatusTrue_MergedRangeTotalExceedsInt64MaxValue()
        {
            testLongIntegerStatusStorer.SetRangeTrue(0, 9223372036854775805);
            testLongIntegerStatusStorer.SetStatusTrue(9223372036854775807);

            InvalidOperationException e = Assert.Throws<InvalidOperationException>(delegate
            {
                testLongIntegerStatusStorer.SetStatusTrue(9223372036854775806);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The class cannot support storing statuses for greater than Int64.MaxValue integers."));
        }

        /// <summary>
        /// Tests that an exception is thrown when calling the SetStatusTrue() method would result in extending a range upwards whose new total range would exceed Int64.MaxValue.
        /// </summary>
        [Test]
        public void SetStatusTrue_UpwardExtendedRangeTotalExceedsInt64MaxValue()
        {
            testLongIntegerStatusStorer.SetRangeTrue(-1, 9223372036854775805);

            InvalidOperationException e = Assert.Throws<InvalidOperationException>(delegate
            {
                testLongIntegerStatusStorer.SetStatusTrue(9223372036854775806);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The class cannot support storing statuses for greater than Int64.MaxValue integers."));
        }

        /// <summary>
        /// Tests that an exception is thrown when calling the SetStatusTrue() method would result in extending a range downwards whose new total range would exceed Int64.MaxValue.
        /// </summary>
        [Test]
        public void SetStatusTrue_DownwardExtendedRangeTotalExceedsInt64MaxValue()
        {
            testLongIntegerStatusStorer.SetRangeTrue(-1, 9223372036854775805);

            InvalidOperationException e = Assert.Throws<InvalidOperationException>(delegate
            {
                testLongIntegerStatusStorer.SetStatusTrue(-2);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The class cannot support storing statuses for greater than Int64.MaxValue integers."));
        }

        /// <summary>
        /// Tests that an exception is thrown when the SetStatusTrue() method is called, but status has already been set for Int64.MaxValue integers.
        /// </summary>
        [Test]
        public void SetStatusTrue_StatusesStoredForGreaterThanInt64MaxValueIntegers()
        {
            testLongIntegerStatusStorer.SetRangeTrue(-9223372036854775808, -4);
            testLongIntegerStatusStorer.SetStatusTrue(2);
            testLongIntegerStatusStorer.SetStatusTrue(4);

            InvalidOperationException e = Assert.Throws<InvalidOperationException>(delegate
            {
                testLongIntegerStatusStorer.SetStatusTrue(6);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The class cannot support storing statuses for greater than Int64.MaxValue integers."));
        }

        /// <summary>
        /// Success tests for the SetStatusTrue() method extending and merging ranges to become Int64.MaxValue.
        /// </summary>
        [Test]
        public void SetStatusTrue_LargeRanges()
        {
            testLongIntegerStatusStorer.SetRangeTrue(0, 9223372036854775804);
            testLongIntegerStatusStorer.SetStatusTrue(9223372036854775806);

            testLongIntegerStatusStorer.SetStatusTrue(9223372036854775805);

            Assert.IsFalse(testLongIntegerStatusStorer.GetStatus(-1));
            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(0));
            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(9223372036854775804));
            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(9223372036854775805));
            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(9223372036854775806));
            Assert.IsFalse(testLongIntegerStatusStorer.GetStatus(9223372036854775807));


            testLongIntegerStatusStorer.Clear();
            testLongIntegerStatusStorer.SetRangeTrue(-1, 9223372036854775804);
            testLongIntegerStatusStorer.SetStatusTrue(9223372036854775805);

            Assert.IsFalse(testLongIntegerStatusStorer.GetStatus(-2));
            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(-1));
            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(9223372036854775804));
            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(9223372036854775805));
            Assert.IsFalse(testLongIntegerStatusStorer.GetStatus(9223372036854775806));


            testLongIntegerStatusStorer.Clear();
            testLongIntegerStatusStorer.SetRangeTrue(-1, 9223372036854775804);
            testLongIntegerStatusStorer.SetStatusTrue(-2);

            Assert.IsFalse(testLongIntegerStatusStorer.GetStatus(-3));
            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(-2));
            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(-1));
            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(9223372036854775804));
            Assert.IsFalse(testLongIntegerStatusStorer.GetStatus(9223372036854775805));
        }

        /// <summary>
        /// Success tests for the SetStatusTrue() method.
        /// </summary>
        [Test]
        public void SetStatusTrue()
        {
            Dictionary<Int64, Int64> treeContents;

            testLongIntegerStatusStorer.SetStatusTrue(2);
            treeContents = GetAllRanges(testLongIntegerStatusStorer);
            Assert.AreEqual(1, treeContents.Count);
            Assert.AreEqual(1, treeContents[2]);

            // Setting an integer to true which has already been set true should have no effect on the tree contents
            testLongIntegerStatusStorer.SetStatusTrue(2);
            treeContents = GetAllRanges(testLongIntegerStatusStorer);
            Assert.AreEqual(1, treeContents.Count);
            Assert.AreEqual(1, treeContents[2]);

            testLongIntegerStatusStorer.SetStatusTrue(1);
            testLongIntegerStatusStorer.SetStatusTrue(2);
            treeContents = GetAllRanges(testLongIntegerStatusStorer);
            Assert.AreEqual(1, treeContents.Count);
            Assert.AreEqual(2, treeContents[1]);

            // Test extending a range by 1 on the right
            testLongIntegerStatusStorer.SetStatusTrue(3);
            treeContents = GetAllRanges(testLongIntegerStatusStorer);
            Assert.AreEqual(1, treeContents.Count);
            Assert.AreEqual(3, treeContents[1]);

            // Test adding a new range where existing ranges are only to the left
            testLongIntegerStatusStorer.SetStatusTrue(10);
            treeContents = GetAllRanges(testLongIntegerStatusStorer);
            Assert.AreEqual(2, treeContents.Count);
            Assert.AreEqual(3, treeContents[1]);
            Assert.AreEqual(1, treeContents[10]);

            // Test adding a new range where existing ranges are only to the right
            testLongIntegerStatusStorer.SetStatusTrue(-10);
            treeContents = GetAllRanges(testLongIntegerStatusStorer);
            Assert.AreEqual(3, treeContents.Count);
            Assert.AreEqual(3, treeContents[1]);
            Assert.AreEqual(1, treeContents[10]);
            Assert.AreEqual(1, treeContents[-10]);

            // Test extending a range by 1 on the right (other range exists to the right)
            testLongIntegerStatusStorer.SetStatusTrue(4);
            treeContents = GetAllRanges(testLongIntegerStatusStorer);
            Assert.AreEqual(3, treeContents.Count);
            Assert.AreEqual(4, treeContents[1]);
            Assert.AreEqual(1, treeContents[10]);
            Assert.AreEqual(1, treeContents[-10]);

            // Test extending a range by 1 on the left (other range exists to the left)
            testLongIntegerStatusStorer.SetStatusTrue(0);
            treeContents = GetAllRanges(testLongIntegerStatusStorer);
            Assert.AreEqual(3, treeContents.Count);
            Assert.AreEqual(5, treeContents[0]);
            Assert.AreEqual(1, treeContents[10]);
            Assert.AreEqual(1, treeContents[-10]);

            // Test adding a new range where existing ranges are on the left and right
            testLongIntegerStatusStorer.SetStatusTrue(6);
            treeContents = GetAllRanges(testLongIntegerStatusStorer);
            Assert.AreEqual(4, treeContents.Count);
            Assert.AreEqual(5, treeContents[0]);
            Assert.AreEqual(1, treeContents[10]);
            Assert.AreEqual(1, treeContents[-10]);
            Assert.AreEqual(1, treeContents[6]);

            // Test merging two existing ranges
            testLongIntegerStatusStorer.SetStatusTrue(5);
            treeContents = GetAllRanges(testLongIntegerStatusStorer);
            Assert.AreEqual(3, treeContents.Count);
            Assert.AreEqual(7, treeContents[0]);
            Assert.AreEqual(1, treeContents[10]);
            Assert.AreEqual(1, treeContents[-10]);

            // Test with max and min Int64 values
            testLongIntegerStatusStorer.Clear();
            testLongIntegerStatusStorer.SetStatusTrue(Int64.MaxValue - 1);
            treeContents = GetAllRanges(testLongIntegerStatusStorer);
            Assert.AreEqual(1, treeContents.Count);
            Assert.AreEqual(1, treeContents[Int64.MaxValue - 1]);
            testLongIntegerStatusStorer.SetStatusTrue(Int64.MaxValue);
            treeContents = GetAllRanges(testLongIntegerStatusStorer);
            Assert.AreEqual(1, treeContents.Count);
            Assert.AreEqual(2, treeContents[Int64.MaxValue - 1]);
            testLongIntegerStatusStorer.SetStatusTrue(Int64.MinValue + 1);
            treeContents = GetAllRanges(testLongIntegerStatusStorer);
            Assert.AreEqual(2, treeContents.Count);
            Assert.AreEqual(2, treeContents[Int64.MaxValue - 1]);
            Assert.AreEqual(1, treeContents[Int64.MinValue + 1]);
            testLongIntegerStatusStorer.SetStatusTrue(Int64.MinValue);
            treeContents = GetAllRanges(testLongIntegerStatusStorer);
            Assert.AreEqual(2, treeContents.Count);
            Assert.AreEqual(2, treeContents[Int64.MaxValue - 1]);
            Assert.AreEqual(2, treeContents[Int64.MinValue]);
        }

        /// <summary>
        /// Success tests for the SetStatusFalse() method.
        /// </summary>
        [Test]
        public void SetStatusFalse()
        {
            Dictionary<Int64, Int64> treeContents;

            for (Int32 i = 1; i <= 8; i++)
            {
                testLongIntegerStatusStorer.SetStatusTrue(i);
            }

            // Test that nothing is changed for numbers which are already set false
            testLongIntegerStatusStorer.SetStatusFalse(0);
            testLongIntegerStatusStorer.SetStatusFalse(-1);
            testLongIntegerStatusStorer.SetStatusFalse(9);
            testLongIntegerStatusStorer.SetStatusFalse(10);
            treeContents = GetAllRanges(testLongIntegerStatusStorer);
            Assert.AreEqual(1, treeContents.Count);
            Assert.AreEqual(8, treeContents[1]);

            // Test shrinking a range by 1 on the left
            testLongIntegerStatusStorer.SetStatusFalse(1);
            treeContents = GetAllRanges(testLongIntegerStatusStorer);
            Assert.AreEqual(1, treeContents.Count);
            Assert.AreEqual(7, treeContents[2]);

            // Test shrinking a range by 1 on the right
            testLongIntegerStatusStorer.SetStatusFalse(8);
            treeContents = GetAllRanges(testLongIntegerStatusStorer);
            Assert.AreEqual(1, treeContents.Count);
            Assert.AreEqual(6, treeContents[2]);

            // Test splitting a range in the middle
            testLongIntegerStatusStorer.SetStatusFalse(3);
            treeContents = GetAllRanges(testLongIntegerStatusStorer);
            Assert.AreEqual(2, treeContents.Count);
            Assert.AreEqual(1, treeContents[2]);
            Assert.AreEqual(4, treeContents[4]);

            // Test removing a range
            testLongIntegerStatusStorer.SetStatusFalse(2);
            treeContents = GetAllRanges(testLongIntegerStatusStorer);
            Assert.AreEqual(1, treeContents.Count);
            Assert.AreEqual(4, treeContents[4]);

            // Test with max and min Int64 values
            testLongIntegerStatusStorer.Clear();
            testLongIntegerStatusStorer.SetStatusTrue(Int64.MaxValue - 1);
            testLongIntegerStatusStorer.SetStatusTrue(Int64.MaxValue);
            testLongIntegerStatusStorer.SetStatusTrue(Int64.MinValue + 1);
            testLongIntegerStatusStorer.SetStatusTrue(Int64.MinValue);
            treeContents = GetAllRanges(testLongIntegerStatusStorer);
            Assert.AreEqual(2, treeContents.Count);
            Assert.AreEqual(2, treeContents[Int64.MaxValue - 1]);
            Assert.AreEqual(2, treeContents[Int64.MinValue]);
            testLongIntegerStatusStorer.SetStatusFalse(Int64.MaxValue - 1);
            testLongIntegerStatusStorer.SetStatusFalse(Int64.MinValue + 1);
            testLongIntegerStatusStorer.SetStatusFalse(Int64.MinValue);
            testLongIntegerStatusStorer.SetStatusFalse(Int64.MaxValue);
            treeContents = GetAllRanges(testLongIntegerStatusStorer);
            Assert.AreEqual(0, treeContents.Count);
            
        }

        /// <summary>
        /// Success tests for the GetStatus() method.
        /// </summary>
        [Test]
        public void GetStatus()
        {
            testLongIntegerStatusStorer.SetStatusTrue(3);
            testLongIntegerStatusStorer.SetStatusTrue(4);
            testLongIntegerStatusStorer.SetStatusTrue(5);
            testLongIntegerStatusStorer.SetStatusTrue(8);
            testLongIntegerStatusStorer.SetStatusTrue(9);
            testLongIntegerStatusStorer.SetStatusTrue(10);

            Assert.AreEqual(false, testLongIntegerStatusStorer.GetStatus(1));
            Assert.AreEqual(false, testLongIntegerStatusStorer.GetStatus(2));
            Assert.AreEqual(true, testLongIntegerStatusStorer.GetStatus(3));
            Assert.AreEqual(true, testLongIntegerStatusStorer.GetStatus(4));
            Assert.AreEqual(true, testLongIntegerStatusStorer.GetStatus(5));
            Assert.AreEqual(false, testLongIntegerStatusStorer.GetStatus(6));
            Assert.AreEqual(false, testLongIntegerStatusStorer.GetStatus(7));
            Assert.AreEqual(true, testLongIntegerStatusStorer.GetStatus(8));
            Assert.AreEqual(true, testLongIntegerStatusStorer.GetStatus(9));
            Assert.AreEqual(true, testLongIntegerStatusStorer.GetStatus(10));
            Assert.AreEqual(false, testLongIntegerStatusStorer.GetStatus(11));
            Assert.AreEqual(false, testLongIntegerStatusStorer.GetStatus(12));
            Assert.AreEqual(false, testLongIntegerStatusStorer.GetStatus(Int64.MinValue));
            Assert.AreEqual(false, testLongIntegerStatusStorer.GetStatus(Int64.MaxValue));

            testLongIntegerStatusStorer.SetStatusTrue(Int64.MinValue + 1);
            testLongIntegerStatusStorer.SetStatusTrue(Int64.MaxValue - 1);
            Assert.AreEqual(false, testLongIntegerStatusStorer.GetStatus(Int64.MinValue));
            Assert.AreEqual(false, testLongIntegerStatusStorer.GetStatus(Int64.MaxValue));

            testLongIntegerStatusStorer.SetStatusTrue(Int64.MinValue);
            testLongIntegerStatusStorer.SetStatusTrue(Int64.MaxValue);
            Assert.AreEqual(true, testLongIntegerStatusStorer.GetStatus(Int64.MinValue));
            Assert.AreEqual(true, testLongIntegerStatusStorer.GetStatus(Int64.MaxValue));
        }

        /// <summary>
        /// Success tests for the Count property.
        /// </summary>
        [Test]
        public void Count()
        {
            // Tests same scenerios as for SetStatusTrue() and SetStatusFalse()
            testLongIntegerStatusStorer.SetStatusTrue(2);
            Assert.AreEqual(1, testLongIntegerStatusStorer.Count);

            testLongIntegerStatusStorer.SetStatusTrue(2);
            Assert.AreEqual(1, testLongIntegerStatusStorer.Count);

            testLongIntegerStatusStorer.SetStatusTrue(1);
            testLongIntegerStatusStorer.SetStatusTrue(2);
            Assert.AreEqual(2, testLongIntegerStatusStorer.Count);

            testLongIntegerStatusStorer.SetStatusTrue(3);
            Assert.AreEqual(3, testLongIntegerStatusStorer.Count);

            testLongIntegerStatusStorer.SetStatusTrue(10);
            Assert.AreEqual(4, testLongIntegerStatusStorer.Count);

            testLongIntegerStatusStorer.SetStatusTrue(-10);
            Assert.AreEqual(5, testLongIntegerStatusStorer.Count);

            testLongIntegerStatusStorer.SetStatusTrue(4);
            Assert.AreEqual(6, testLongIntegerStatusStorer.Count);

            testLongIntegerStatusStorer.SetStatusTrue(0);
            Assert.AreEqual(7, testLongIntegerStatusStorer.Count);

            testLongIntegerStatusStorer.SetStatusTrue(6);
            Assert.AreEqual(8, testLongIntegerStatusStorer.Count);

            testLongIntegerStatusStorer.SetStatusTrue(5);
            Assert.AreEqual(9, testLongIntegerStatusStorer.Count);


            testLongIntegerStatusStorer.Clear();
            Assert.AreEqual(0, testLongIntegerStatusStorer.Count);

            for (Int32 i = 1; i <= 8; i++)
            {
                testLongIntegerStatusStorer.SetStatusTrue(i);
            }
            Assert.AreEqual(8, testLongIntegerStatusStorer.Count);

            testLongIntegerStatusStorer.SetStatusFalse(0);
            testLongIntegerStatusStorer.SetStatusFalse(-1);
            testLongIntegerStatusStorer.SetStatusFalse(9);
            testLongIntegerStatusStorer.SetStatusFalse(10);
            Assert.AreEqual(8, testLongIntegerStatusStorer.Count);

            testLongIntegerStatusStorer.SetStatusFalse(1);
            Assert.AreEqual(7, testLongIntegerStatusStorer.Count);

            testLongIntegerStatusStorer.SetStatusFalse(8);
            Assert.AreEqual(6, testLongIntegerStatusStorer.Count);

            testLongIntegerStatusStorer.SetStatusFalse(3);
            Assert.AreEqual(5, testLongIntegerStatusStorer.Count);

            testLongIntegerStatusStorer.SetStatusFalse(2);
            Assert.AreEqual(4, testLongIntegerStatusStorer.Count);


            // Test with max and min Int64 values
            testLongIntegerStatusStorer.Clear();
            Assert.AreEqual(0, testLongIntegerStatusStorer.Count);
            testLongIntegerStatusStorer.SetStatusTrue(Int64.MaxValue - 1);
            Assert.AreEqual(1, testLongIntegerStatusStorer.Count);

            testLongIntegerStatusStorer.SetStatusTrue(Int64.MaxValue);
            Assert.AreEqual(2, testLongIntegerStatusStorer.Count);

            testLongIntegerStatusStorer.SetStatusTrue(Int64.MinValue + 1);
            Assert.AreEqual(3, testLongIntegerStatusStorer.Count);

            testLongIntegerStatusStorer.SetStatusTrue(Int64.MinValue);
            Assert.AreEqual(4, testLongIntegerStatusStorer.Count);
        }

        /// <summary>
        /// Success tests for the Count property after calling method SetRangeTrue().
        /// </summary>
        [Test]
        public void Count_SetRangeTrue()
        {
            testLongIntegerStatusStorer.SetRangeTrue(1, 1);

            Assert.AreEqual(1, testLongIntegerStatusStorer.Count);

            testLongIntegerStatusStorer.Clear();
            testLongIntegerStatusStorer.SetRangeTrue(-2, 2);

            Assert.AreEqual(5, testLongIntegerStatusStorer.Count);

            testLongIntegerStatusStorer.Clear();
            testLongIntegerStatusStorer.SetRangeTrue(0, 9223372036854775806);

            Assert.AreEqual(9223372036854775807, testLongIntegerStatusStorer.Count);
        }

        /// <summary>
        /// Tests that an exception is thrown if the SetRangeTrue() method is called when statuses have already been set to true.
        /// </summary>
        [Test]
        public void SetRangeTrue_ExistingStatusesTrue()
        {
            testLongIntegerStatusStorer.SetStatusTrue(1);

            InvalidOperationException e = Assert.Throws<InvalidOperationException>(delegate
            {
                testLongIntegerStatusStorer.SetRangeTrue(2, 4);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("A range of statuses can only be set true when all existing statuses are false."));
        }

        /// <summary>
        /// Tests that an exception is thrown if the SetRangeTrue() method is called with a 'rangeEnd' parameter less than the 'rangeStart' parameter.
        /// </summary>
        [Test]
        public void SetRangeTrue_RangeEndParameterLessThanRangeStart()
        {
            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                testLongIntegerStatusStorer.SetRangeTrue(2, 1);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("Parameter 'rangeEnd' must be greater than or equal to parameter 'rangeStart'."));
            Assert.AreEqual("rangeEnd", e.ParamName);
        }

        /// <summary>
        /// Tests that an exception is thrown if the SetRangeTrue() method is called with a 'rangeStart' and 'rangeEnd' parameters whose inclusive range is greater than Int64.MaxValue.
        /// </summary>
        [Test]
        public void SetRangeTrue_RangeGreaterThanInt64MaxValue()
        {
            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                testLongIntegerStatusStorer.SetRangeTrue(0, 9223372036854775807);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The total inclusive range cannot exceed Int64.MaxValue."));
            Assert.AreEqual("rangeEnd", e.ParamName);


            e = Assert.Throws<ArgumentException>(delegate
            {
                testLongIntegerStatusStorer.SetRangeTrue(-1, 9223372036854775806);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The total inclusive range cannot exceed Int64.MaxValue."));
            Assert.AreEqual("rangeEnd", e.ParamName);


            e = Assert.Throws<ArgumentException>(delegate
            {
                testLongIntegerStatusStorer.SetRangeTrue(-2, 9223372036854775805);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The total inclusive range cannot exceed Int64.MaxValue."));
            Assert.AreEqual("rangeEnd", e.ParamName);


            e = Assert.Throws<ArgumentException>(delegate
            {
                testLongIntegerStatusStorer.SetRangeTrue(-4611686018427387903, 4611686018427387904);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The total inclusive range cannot exceed Int64.MaxValue."));
            Assert.AreEqual("rangeEnd", e.ParamName);


            e = Assert.Throws<ArgumentException>(delegate
            {
                testLongIntegerStatusStorer.SetRangeTrue(-9223372036854775808, -1);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The total inclusive range cannot exceed Int64.MaxValue."));
            Assert.AreEqual("rangeEnd", e.ParamName);


            e = Assert.Throws<ArgumentException>(delegate
            {
                testLongIntegerStatusStorer.SetRangeTrue(-9223372036854775807, 0);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The total inclusive range cannot exceed Int64.MaxValue."));
            Assert.AreEqual("rangeEnd", e.ParamName);


            e = Assert.Throws<ArgumentException>(delegate
            {
                testLongIntegerStatusStorer.SetRangeTrue(-9223372036854775806, 1);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The total inclusive range cannot exceed Int64.MaxValue."));
            Assert.AreEqual("rangeEnd", e.ParamName);
        }

        /// <summary>
        /// Success tests for the SetRangeTrue() method where the specified range is equal to Int64.MaxValue.
        /// </summary>
        [Test]
        public void SetRangeTrue_Int64MaxValueRange()
        {
            testLongIntegerStatusStorer.SetRangeTrue(1, 9223372036854775807);

            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(1));
            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(9223372036854775807));
            Assert.IsFalse(testLongIntegerStatusStorer.GetStatus(0));
            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(9223372036854775806 / 2));


            testLongIntegerStatusStorer.Clear();
            testLongIntegerStatusStorer.SetRangeTrue(0, 9223372036854775806);

            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(0));
            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(9223372036854775806));
            Assert.IsFalse(testLongIntegerStatusStorer.GetStatus(-1));
            Assert.IsFalse(testLongIntegerStatusStorer.GetStatus(9223372036854775807));
            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(9223372036854775806 / 2));


            testLongIntegerStatusStorer.Clear();
            testLongIntegerStatusStorer.SetRangeTrue(-1, 9223372036854775805);

            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(-1));
            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(9223372036854775805));
            Assert.IsFalse(testLongIntegerStatusStorer.GetStatus(-2));
            Assert.IsFalse(testLongIntegerStatusStorer.GetStatus(9223372036854775806));
            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(9223372036854775806 / 2));
            

            testLongIntegerStatusStorer.Clear();
            testLongIntegerStatusStorer.SetRangeTrue(-4611686018427387903, 4611686018427387903);

            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(-4611686018427387903));
            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(4611686018427387903));
            Assert.IsFalse(testLongIntegerStatusStorer.GetStatus(-4611686018427387904));
            Assert.IsFalse(testLongIntegerStatusStorer.GetStatus(4611686018427387904));
            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(0));


            testLongIntegerStatusStorer.Clear();
            testLongIntegerStatusStorer.SetRangeTrue(-9223372036854775806, 0);

            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(-9223372036854775806));
            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(0));
            Assert.IsFalse(testLongIntegerStatusStorer.GetStatus(-9223372036854775807));
            Assert.IsFalse(testLongIntegerStatusStorer.GetStatus(1));
            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(-9223372036854775807 / 2));


            testLongIntegerStatusStorer.Clear();
            testLongIntegerStatusStorer.SetRangeTrue(-9223372036854775807, -1);

            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(-9223372036854775807));
            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(-1));
            Assert.IsFalse(testLongIntegerStatusStorer.GetStatus(-9223372036854775808));
            Assert.IsFalse(testLongIntegerStatusStorer.GetStatus(0));
            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(-9223372036854775807 / 2));


            testLongIntegerStatusStorer.Clear();
            testLongIntegerStatusStorer.SetRangeTrue(-9223372036854775808, -2);

            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(-9223372036854775808));
            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(-2));
            Assert.IsFalse(testLongIntegerStatusStorer.GetStatus(-1));
            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(-9223372036854775807 / 2));
        }

        /// <summary>
        /// Success tests for the SetRangeTrue() method where the specified range is 1.
        /// </summary>
        [Test]
        public void SetRangeTrue_Range1()
        {
            testLongIntegerStatusStorer.SetRangeTrue(1, 1);

            Assert.IsFalse(testLongIntegerStatusStorer.GetStatus(0));
            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(1));
            Assert.IsFalse(testLongIntegerStatusStorer.GetStatus(2));
        }

        /// <summary>
        /// Success tests for the SetRangeTrue() method.
        /// </summary>
        [Test]
        public void SetRangeTrue()
        {
            testLongIntegerStatusStorer.SetRangeTrue(-1, 1);

            Assert.IsFalse(testLongIntegerStatusStorer.GetStatus(-2));
            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(-1));
            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(0));
            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(1));
            Assert.IsFalse(testLongIntegerStatusStorer.GetStatus(2));


            testLongIntegerStatusStorer.Clear();
            testLongIntegerStatusStorer.SetRangeTrue(-3, -1);

            Assert.IsFalse(testLongIntegerStatusStorer.GetStatus(-4));
            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(-3));
            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(-2));
            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(-1));
            Assert.IsFalse(testLongIntegerStatusStorer.GetStatus(0));


            testLongIntegerStatusStorer.Clear();
            testLongIntegerStatusStorer.SetRangeTrue(1, 3);

            Assert.IsFalse(testLongIntegerStatusStorer.GetStatus(0));
            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(1));
            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(2));
            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(3));
            Assert.IsFalse(testLongIntegerStatusStorer.GetStatus(4));


            testLongIntegerStatusStorer.Clear();
            testLongIntegerStatusStorer.SetRangeTrue(-9223372036854775808, -4);

            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(-9223372036854775808));
            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(-4));
            Assert.IsFalse(testLongIntegerStatusStorer.GetStatus(-3));
            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(-9223372036854775807 / 2));


            testLongIntegerStatusStorer.Clear();
            testLongIntegerStatusStorer.SetRangeTrue(3, 9223372036854775807);

            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(3));
            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(9223372036854775807));
            Assert.IsFalse(testLongIntegerStatusStorer.GetStatus(2));
            Assert.IsTrue(testLongIntegerStatusStorer.GetStatus(9223372036854775807 / 2));
        }

        /// <summary>
        /// Tests that an exception is thrown if the MinimumRange property is accessed when the underlying tree is empty.
        /// </summary>
        [Test]
        public void MinimumRange_TreeIsEmpty()
        {
            InvalidOperationException e = Assert.Throws<InvalidOperationException>(delegate
            {
                LongIntegerRange result = testLongIntegerStatusStorer.MinimumRange;
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The underlying tree is empty."));


            testLongIntegerStatusStorer = new LongIntegerStatusStorer();
            testLongIntegerStatusStorer.SetStatusTrue(5);
            testLongIntegerStatusStorer.SetStatusFalse(5);

            e = Assert.Throws<InvalidOperationException>(delegate
            {
                LongIntegerRange result = testLongIntegerStatusStorer.MinimumRange;
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The underlying tree is empty."));
        }

        /// <summary>
        /// Success tests for the MinimumRange property.
        /// </summary>
        [Test]
        public void MinimumRange()
        {
            testLongIntegerStatusStorer.SetRangeTrue(2, 6);
            testLongIntegerStatusStorer.SetStatusTrue(0);
            testLongIntegerStatusStorer.SetStatusTrue(8);
            testLongIntegerStatusStorer.SetStatusFalse(5);
            testLongIntegerStatusStorer.SetStatusFalse(0);

            LongIntegerRange result = testLongIntegerStatusStorer.MinimumRange;

            Assert.AreEqual(2, result.StartValue);
            Assert.AreEqual(3, result.Length);
        }

        /// <summary>
        /// Tests that an exception is thrown if the MaximumRange property is accessed when the underlying tree is empty.
        /// </summary>
        [Test]
        public void MaximumRange_TreeIsEmpty()
        {
            InvalidOperationException e = Assert.Throws<InvalidOperationException>(delegate
            {
                LongIntegerRange result = testLongIntegerStatusStorer.MaximumRange;
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The underlying tree is empty."));


            testLongIntegerStatusStorer = new LongIntegerStatusStorer();
            testLongIntegerStatusStorer.SetStatusTrue(5);
            testLongIntegerStatusStorer.SetStatusFalse(5);

            e = Assert.Throws<InvalidOperationException>(delegate
            {
                LongIntegerRange result = testLongIntegerStatusStorer.MaximumRange;
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The underlying tree is empty."));
        }

        /// <summary>
        /// Success tests for the MaximumRange property.
        /// </summary>
        [Test]
        public void MaximumRange()
        {
            testLongIntegerStatusStorer.SetRangeTrue(2, 6);
            testLongIntegerStatusStorer.SetStatusTrue(8);
            testLongIntegerStatusStorer.SetStatusTrue(0);
            testLongIntegerStatusStorer.SetStatusFalse(3);
            testLongIntegerStatusStorer.SetStatusFalse(8);

            LongIntegerRange result = testLongIntegerStatusStorer.MaximumRange;

            Assert.AreEqual(4, result.StartValue);
            Assert.AreEqual(3, result.Length);
        }

        /// <summary>
        /// Tests that an empty enumerable is returned if the GetAllRangesAscending() method is called when the underlying tree is empty.
        /// </summary>
        [Test]
        public void GetAllRangesAscending_TreeIsEmpty()
        {
            IEnumerable<LongIntegerRange> result = testLongIntegerStatusStorer.GetAllRangesAscending();

            Assert.AreEqual(0, result.Count());


            testLongIntegerStatusStorer = new LongIntegerStatusStorer();
            testLongIntegerStatusStorer.SetStatusTrue(5);
            testLongIntegerStatusStorer.SetStatusFalse(5);

            result = testLongIntegerStatusStorer.GetAllRangesAscending();

            Assert.AreEqual(0, result.Count());
        }

        /// <summary>
        /// Success tests for the GetAllRangesAscending() method.
        /// </summary>
        [Test]
        public void GetAllRangesAscending()
        {
            testLongIntegerStatusStorer.SetRangeTrue(2, 6);

            var result = new List<LongIntegerRange>(testLongIntegerStatusStorer.GetAllRangesAscending());

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(2, result[0].StartValue);
            Assert.AreEqual(5, result[0].Length);


            testLongIntegerStatusStorer.Clear();
            testLongIntegerStatusStorer.SetRangeTrue(2, 6);
            testLongIntegerStatusStorer.SetStatusTrue(8);
            testLongIntegerStatusStorer.SetStatusTrue(0);
            testLongIntegerStatusStorer.SetStatusFalse(3);
            testLongIntegerStatusStorer.SetStatusFalse(8);

            result = new List<LongIntegerRange>(testLongIntegerStatusStorer.GetAllRangesAscending());

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(0, result[0].StartValue);
            Assert.AreEqual(1, result[0].Length);
            Assert.AreEqual(2, result[1].StartValue);
            Assert.AreEqual(1, result[1].Length);
            Assert.AreEqual(4, result[2].StartValue);
            Assert.AreEqual(3, result[2].Length);
        }

        /// <summary>
        /// Tests that an empty enumerable is returned if the GetAllRangesDescending() method is called when the underlying tree is empty.
        /// </summary>
        [Test]
        public void GetAllRangesDescending_TreeIsEmpty()
        {
            IEnumerable<LongIntegerRange> result = testLongIntegerStatusStorer.GetAllRangesDescending();

            Assert.AreEqual(0, result.Count());


            testLongIntegerStatusStorer = new LongIntegerStatusStorer();
            testLongIntegerStatusStorer.SetStatusTrue(5);
            testLongIntegerStatusStorer.SetStatusFalse(5);

            result = testLongIntegerStatusStorer.GetAllRangesDescending();

            Assert.AreEqual(0, result.Count());
        }

        /// <summary>
        /// Success tests for the GetAllRangesDescending() method.
        /// </summary>
        [Test]
        public void GetAllRangesDescending()
        {
            testLongIntegerStatusStorer.SetRangeTrue(2, 6);

            var result = new List<LongIntegerRange>(testLongIntegerStatusStorer.GetAllRangesDescending());

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(2, result[0].StartValue);
            Assert.AreEqual(5, result[0].Length);


            testLongIntegerStatusStorer.Clear();
            testLongIntegerStatusStorer.SetRangeTrue(2, 6);
            testLongIntegerStatusStorer.SetStatusTrue(0);
            testLongIntegerStatusStorer.SetStatusTrue(8);
            testLongIntegerStatusStorer.SetStatusFalse(5);
            testLongIntegerStatusStorer.SetStatusFalse(0);

            result = new List<LongIntegerRange>(testLongIntegerStatusStorer.GetAllRangesDescending());

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(8, result[0].StartValue);
            Assert.AreEqual(1, result[0].Length);
            Assert.AreEqual(6, result[1].StartValue);
            Assert.AreEqual(1, result[1].Length);
            Assert.AreEqual(2, result[2].StartValue);
            Assert.AreEqual(3, result[2].Length);
        }

        #region Private Methods

        private Dictionary<Int64, Int64> GetAllRanges(LongIntegerStatusStorer inputStorer)
        {
            Dictionary<Int64, Int64> returnDictionary = new Dictionary<Int64, Int64>();
            Action<Int64, Int64> storeInDictionaryAction = (startValue, length) =>
            {
                returnDictionary.Add(startValue, length);
            };
            inputStorer.TraverseTree(storeInDictionaryAction);
            return returnDictionary;
        }

        #endregion
    }
}
