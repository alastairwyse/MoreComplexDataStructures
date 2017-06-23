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
