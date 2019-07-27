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
using NMock2;
using MoreComplexDataStructures;

namespace MoreComplexDataStructures.UnitTests
{
    /// <summary>
    /// Unit tests for the UniqueRandomGenerator class.
    /// </summary>
    /// <remarks>Deriving from UniqueRandomGenerator so that nested class RangeAndSubtreeCounts can be accessed.</remarks>
    public class UniqueRandomGeneratorTests : UniqueRandomGenerator
    {
        private Mockery mockery;
        private IRandomIntegerGenerator mockRandomIntegerGenerator;
        private UniqueRandomGenerator testUniqueRandomGenerator;
        
        /// <summary>
        /// Placeholder constructor.
        /// </summary>
        public UniqueRandomGeneratorTests()
            : base(0, 0)
        {
        }

        [SetUp]
        protected void SetUp()
        {
            mockery = new Mockery();
            mockRandomIntegerGenerator = mockery.NewMock<IRandomIntegerGenerator>();
        }

        /// <summary>
        /// Tests that an exception is thrown if the constructor is called with a 'rangeEnd' parameter less than the 'rangeStart' parameter.
        /// </summary>
        [Test]
        public void Constructor_RangeEndParameterLessThanRangeStart()
        {
            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                testUniqueRandomGenerator = new UniqueRandomGenerator(2, 1);
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
                testUniqueRandomGenerator = new UniqueRandomGenerator(0, 9223372036854775807);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The total inclusive range cannot exceed Int64.MaxValue."));
            Assert.AreEqual("rangeEnd", e.ParamName);


            e = Assert.Throws<ArgumentException>(delegate
            {
                testUniqueRandomGenerator = new UniqueRandomGenerator(-1, 9223372036854775806);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The total inclusive range cannot exceed Int64.MaxValue."));
            Assert.AreEqual("rangeEnd", e.ParamName);


            e = Assert.Throws<ArgumentException>(delegate
            {
                testUniqueRandomGenerator = new UniqueRandomGenerator(-2, 9223372036854775805);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The total inclusive range cannot exceed Int64.MaxValue."));
            Assert.AreEqual("rangeEnd", e.ParamName);


            e = Assert.Throws<ArgumentException>(delegate
            {
                testUniqueRandomGenerator = new UniqueRandomGenerator(-4611686018427387903, 4611686018427387904);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The total inclusive range cannot exceed Int64.MaxValue."));
            Assert.AreEqual("rangeEnd", e.ParamName);


            e = Assert.Throws<ArgumentException>(delegate
            {
                testUniqueRandomGenerator = new UniqueRandomGenerator(-9223372036854775808, -1);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The total inclusive range cannot exceed Int64.MaxValue."));
            Assert.AreEqual("rangeEnd", e.ParamName);


            e = Assert.Throws<ArgumentException>(delegate
            {
                testUniqueRandomGenerator = new UniqueRandomGenerator(-9223372036854775807, 0);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The total inclusive range cannot exceed Int64.MaxValue."));
            Assert.AreEqual("rangeEnd", e.ParamName);


            e = Assert.Throws<ArgumentException>(delegate
            {
                testUniqueRandomGenerator = new UniqueRandomGenerator(-9223372036854775806, 1);
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
            testUniqueRandomGenerator = new UniqueRandomGenerator(1, 9223372036854775807);

            Assert.AreEqual(1, testUniqueRandomGenerator.RangeStart);
            Assert.AreEqual(9223372036854775807, testUniqueRandomGenerator.RangeEnd);

            testUniqueRandomGenerator = new UniqueRandomGenerator(0, 9223372036854775806);

            testUniqueRandomGenerator = new UniqueRandomGenerator(-1, 9223372036854775805);

            testUniqueRandomGenerator = new UniqueRandomGenerator(-4611686018427387903, 4611686018427387903);

            testUniqueRandomGenerator = new UniqueRandomGenerator(-9223372036854775806, 0);

            testUniqueRandomGenerator = new UniqueRandomGenerator(-9223372036854775807, -1);

            testUniqueRandomGenerator = new UniqueRandomGenerator(-9223372036854775808, -2);
        }

        /// <summary>
        /// Tests that an exception is thrown if the Generate() method is called after all numbers in the specified range have already been generated.
        /// </summary>
        [Test]
        public void Generate_NoMoreNumbersExist()
        {
            UniqueRandomGeneratorWithProtectedMethods testUniqueRandomGenerator = new UniqueRandomGeneratorWithProtectedMethods(-5, 4, mockRandomIntegerGenerator);
            var numbersToRemove = new List<Int64>() { -3, 1, 3, -4, -5, 2, 4, -1, 0, -2 };
            Assert.AreEqual(10, testUniqueRandomGenerator.NumbersRemainingCount);
            foreach (Int64 currentNumber in numbersToRemove)
            {
                SetMockRandomIntegerGeneratorExpectations(testUniqueRandomGenerator, Convert.ToInt64(currentNumber), mockery, mockRandomIntegerGenerator);
                testUniqueRandomGenerator.Generate();
                mockery.ClearExpectation(mockRandomIntegerGenerator);
            }

            Assert.AreEqual(0, testUniqueRandomGenerator.NumbersRemainingCount);
            Assert.AreEqual(10, testUniqueRandomGenerator.NumbersGeneratedCount);

            InvalidOperationException e = Assert.Throws<InvalidOperationException>(delegate
            {
                testUniqueRandomGenerator.Generate();
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("Cannot generate a random number as no further unique numbers exist in the specified range."));
        }

        /// <summary>
        /// Success tests for the Generate() method.
        /// </summary>
        /// <remarks>Need to declare as 'new' due to test class inheriting from UniqueRandomGenerator.</remarks>
        [Test]
        public new void Generate()
        {
            UniqueRandomGeneratorWithProtectedMethods testUniqueRandomGenerator = new UniqueRandomGeneratorWithProtectedMethods(3, 11, mockRandomIntegerGenerator);

            // Test shortening a range on the left side.
            using (mockery.Ordered)
            {
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(9L).Will(Return.Value(0L));
            }

            Int64 result = testUniqueRandomGenerator.Generate();

            Assert.AreEqual(3, result);
            Dictionary<Int64, WeightBalancedTreeNode<RangeAndSubtreeCounts>> allNodes = testUniqueRandomGenerator.AllNodes;
            Assert.AreEqual(1, allNodes.Count);
            Assert.IsNull(allNodes[4].ParentNode);
            Assert.IsNull(allNodes[4].LeftChildNode);
            Assert.IsNull(allNodes[4].RightChildNode);
            Assert.AreEqual(0, allNodes[4].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[4].RightSubtreeSize);
            Assert.AreEqual(8, allNodes[4].Item.Range.Length);
            Assert.AreEqual(0, allNodes[4].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[4].Item.RightSubtreeRangeCount);
            mockery.VerifyAllExpectationsHaveBeenMet();
            ValidateRangeTree(testUniqueRandomGenerator);


            // Test shortening a range on the right side.
            mockery.ClearExpectation(mockRandomIntegerGenerator);
            using (mockery.Ordered)
            {
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(8L).Will(Return.Value(7L));
            }

            result = testUniqueRandomGenerator.Generate();

            Assert.AreEqual(11, result);
            allNodes = testUniqueRandomGenerator.AllNodes;
            Assert.AreEqual(1, allNodes.Count);
            Assert.IsNull(allNodes[4].ParentNode);
            Assert.IsNull(allNodes[4].LeftChildNode);
            Assert.IsNull(allNodes[4].RightChildNode);
            Assert.AreEqual(0, allNodes[4].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[4].RightSubtreeSize);
            Assert.AreEqual(7, allNodes[4].Item.Range.Length);
            Assert.AreEqual(0, allNodes[4].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[4].Item.RightSubtreeRangeCount);
            mockery.VerifyAllExpectationsHaveBeenMet();
            ValidateRangeTree(testUniqueRandomGenerator);


            // Test splitting a node and pushing the left part of the split down left
            mockery.ClearExpectation(mockRandomIntegerGenerator);
            using (mockery.Ordered)
            {
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(7L).Will(Return.Value(2L));
            }

            result = testUniqueRandomGenerator.Generate();

            Assert.AreEqual(6, result);
            allNodes = testUniqueRandomGenerator.AllNodes;
            Assert.AreEqual(2, allNodes.Count);
            Assert.IsNull(allNodes[7].ParentNode);
            Assert.AreSame(allNodes[4], allNodes[7].LeftChildNode);
            Assert.IsNull(allNodes[7].RightChildNode);
            Assert.AreSame(allNodes[7], allNodes[4].ParentNode);
            Assert.IsNull(allNodes[4].LeftChildNode);
            Assert.IsNull(allNodes[4].RightChildNode);
            Assert.AreEqual(1, allNodes[7].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[7].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[4].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[4].RightSubtreeSize);
            Assert.AreEqual(4, allNodes[7].Item.Range.Length);
            Assert.AreEqual(2, allNodes[4].Item.Range.Length);
            Assert.AreEqual(2, allNodes[7].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[7].Item.RightSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[4].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[4].Item.RightSubtreeRangeCount);
            mockery.VerifyAllExpectationsHaveBeenMet();
            ValidateRangeTree(testUniqueRandomGenerator);


            // Test splitting a node and pushing the right part of the split down right
            mockery.ClearExpectation(mockRandomIntegerGenerator);
            using (mockery.Ordered)
            {
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(6L).Will(Return.Value(4L));
            }

            result = testUniqueRandomGenerator.Generate();

            Assert.AreEqual(9, result);
            allNodes = testUniqueRandomGenerator.AllNodes;
            Assert.AreEqual(3, allNodes.Count);
            Assert.IsNull(allNodes[7].ParentNode);
            Assert.AreSame(allNodes[4], allNodes[7].LeftChildNode);
            Assert.AreSame(allNodes[10], allNodes[7].RightChildNode);
            Assert.AreSame(allNodes[7], allNodes[4].ParentNode);
            Assert.IsNull(allNodes[4].LeftChildNode);
            Assert.IsNull(allNodes[4].RightChildNode);
            Assert.AreSame(allNodes[7], allNodes[10].ParentNode);
            Assert.IsNull(allNodes[10].LeftChildNode);
            Assert.IsNull(allNodes[10].RightChildNode);
            Assert.AreEqual(1, allNodes[7].LeftSubtreeSize);
            Assert.AreEqual(1, allNodes[7].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[4].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[4].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[10].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[10].RightSubtreeSize);
            Assert.AreEqual(2, allNodes[7].Item.Range.Length);
            Assert.AreEqual(2, allNodes[4].Item.Range.Length);
            Assert.AreEqual(1, allNodes[10].Item.Range.Length);
            Assert.AreEqual(2, allNodes[7].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(1, allNodes[7].Item.RightSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[4].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[4].Item.RightSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[10].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[10].Item.RightSubtreeRangeCount);
            mockery.VerifyAllExpectationsHaveBeenMet();
            ValidateRangeTree(testUniqueRandomGenerator);


            // Setup the following tree...
            //          50-86(37)
            //        29         44
            //       /             \    
            //   20-48(29)     88-131(44)
            //  0         0   0          0
            testUniqueRandomGenerator = new UniqueRandomGeneratorWithProtectedMethods(20, 131, mockRandomIntegerGenerator);
            mockery.ClearExpectation(mockRandomIntegerGenerator);
            using (mockery.Ordered)
            {
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(112L).Will(Return.Value(29L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(111L).Will(Return.Value(66L));
            }
            testUniqueRandomGenerator.Generate();
            testUniqueRandomGenerator.Generate();
            allNodes = testUniqueRandomGenerator.AllNodes;
            Assert.AreEqual(3, allNodes.Count);
            Assert.IsNull(allNodes[50].ParentNode);
            Assert.AreSame(allNodes[20], allNodes[50].LeftChildNode);
            Assert.AreSame(allNodes[88], allNodes[50].RightChildNode);
            Assert.AreSame(allNodes[50], allNodes[20].ParentNode);
            Assert.IsNull(allNodes[20].LeftChildNode);
            Assert.IsNull(allNodes[20].RightChildNode);
            Assert.AreSame(allNodes[50], allNodes[88].ParentNode);
            Assert.IsNull(allNodes[88].LeftChildNode);
            Assert.IsNull(allNodes[88].RightChildNode);
            Assert.AreEqual(1, allNodes[50].LeftSubtreeSize);
            Assert.AreEqual(1, allNodes[50].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[20].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[20].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[88].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[88].RightSubtreeSize);
            Assert.AreEqual(29, allNodes[50].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(44, allNodes[50].Item.RightSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[20].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[20].Item.RightSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[88].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[88].Item.RightSubtreeRangeCount);
            Assert.AreEqual(37, allNodes[50].Item.Range.Length);
            Assert.AreEqual(29, allNodes[20].Item.Range.Length);
            Assert.AreEqual(44, allNodes[88].Item.Range.Length);
            mockery.VerifyAllExpectationsHaveBeenMet();
            ValidateRangeTree(testUniqueRandomGenerator);


            // ...then generate 34 so the result looks like...
            //                50-86(37)
            //              28         44
            //             /             \    
            //         35-48(14)     88-131(44)
            //       14         0   0          0
            //      /
            //   20-33(14)
            //  0         0
            mockery.ClearExpectation(mockRandomIntegerGenerator);
            using (mockery.Ordered)
            {
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(110L).Will(Return.Value(28L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(29L).Will(Return.Value(14L));
            }
            result = testUniqueRandomGenerator.Generate();

            Assert.AreEqual(34, result);
            allNodes = testUniqueRandomGenerator.AllNodes;
            Assert.AreEqual(4, allNodes.Count);
            Assert.IsNull(allNodes[50].ParentNode);
            Assert.AreSame(allNodes[35], allNodes[50].LeftChildNode);
            Assert.AreSame(allNodes[88], allNodes[50].RightChildNode);
            Assert.AreSame(allNodes[50], allNodes[35].ParentNode);
            Assert.AreSame(allNodes[20], allNodes[35].LeftChildNode);
            Assert.IsNull(allNodes[35].RightChildNode);
            Assert.AreSame(allNodes[50], allNodes[88].ParentNode);
            Assert.IsNull(allNodes[88].LeftChildNode);
            Assert.IsNull(allNodes[88].RightChildNode);
            Assert.AreSame(allNodes[35], allNodes[20].ParentNode);
            Assert.IsNull(allNodes[20].LeftChildNode);
            Assert.IsNull(allNodes[20].RightChildNode);
            Assert.AreEqual(2, allNodes[50].LeftSubtreeSize);
            Assert.AreEqual(1, allNodes[50].RightSubtreeSize);
            Assert.AreEqual(1, allNodes[35].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[35].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[88].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[88].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[20].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[20].RightSubtreeSize);
            Assert.AreEqual(28, allNodes[50].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(44, allNodes[50].Item.RightSubtreeRangeCount);
            Assert.AreEqual(14, allNodes[35].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[35].Item.RightSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[88].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[88].Item.RightSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[20].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[20].Item.RightSubtreeRangeCount);
            Assert.AreEqual(37, allNodes[50].Item.Range.Length);
            Assert.AreEqual(14, allNodes[35].Item.Range.Length);
            Assert.AreEqual(44, allNodes[88].Item.Range.Length);
            Assert.AreEqual(14, allNodes[20].Item.Range.Length);
            mockery.VerifyAllExpectationsHaveBeenMet();
            ValidateRangeTree(testUniqueRandomGenerator);


            // ...then generate 109 so the result looks like...
            //                50-86(37)
            //              28         43
            //             /             \    
            //         35-48(14)      110-131(22)
            //       14         0   21           0
            //      /              /
            //   20-33(14)     88-108(21)
            //  0         0   0          0
            mockery.ClearExpectation(mockRandomIntegerGenerator);
            using (mockery.Ordered)
            {
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(109L).Will(Return.Value(65L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(44L).Will(Return.Value(21L));
            }
            result = testUniqueRandomGenerator.Generate();
            Assert.AreEqual(109, result);
            allNodes = testUniqueRandomGenerator.AllNodes;
            Assert.AreEqual(5, allNodes.Count);
            Assert.IsNull(allNodes[50].ParentNode);
            Assert.AreSame(allNodes[35], allNodes[50].LeftChildNode);
            Assert.AreSame(allNodes[110], allNodes[50].RightChildNode);
            Assert.AreSame(allNodes[50], allNodes[35].ParentNode);
            Assert.AreSame(allNodes[20], allNodes[35].LeftChildNode);
            Assert.IsNull(allNodes[35].RightChildNode);
            Assert.AreSame(allNodes[50], allNodes[110].ParentNode);
            Assert.AreSame(allNodes[88], allNodes[110].LeftChildNode);
            Assert.IsNull(allNodes[110].RightChildNode);
            Assert.AreSame(allNodes[35], allNodes[20].ParentNode);
            Assert.IsNull(allNodes[20].LeftChildNode);
            Assert.IsNull(allNodes[20].RightChildNode);
            Assert.AreSame(allNodes[110], allNodes[88].ParentNode);
            Assert.IsNull(allNodes[88].LeftChildNode);
            Assert.IsNull(allNodes[88].RightChildNode);
            Assert.AreEqual(2, allNodes[50].LeftSubtreeSize);
            Assert.AreEqual(2, allNodes[50].RightSubtreeSize);
            Assert.AreEqual(1, allNodes[35].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[35].RightSubtreeSize);
            Assert.AreEqual(1, allNodes[110].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[110].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[20].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[20].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[88].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[88].RightSubtreeSize);
            Assert.AreEqual(28, allNodes[50].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(43, allNodes[50].Item.RightSubtreeRangeCount);
            Assert.AreEqual(14, allNodes[35].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[35].Item.RightSubtreeRangeCount);
            Assert.AreEqual(21, allNodes[110].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[110].Item.RightSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[20].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[20].Item.RightSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[88].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[88].Item.RightSubtreeRangeCount);
            Assert.AreEqual(37, allNodes[50].Item.Range.Length);
            Assert.AreEqual(14, allNodes[35].Item.Range.Length);
            Assert.AreEqual(22, allNodes[110].Item.Range.Length);
            Assert.AreEqual(14, allNodes[20].Item.Range.Length);
            Assert.AreEqual(21, allNodes[88].Item.Range.Length);
            mockery.VerifyAllExpectationsHaveBeenMet();
            ValidateRangeTree(testUniqueRandomGenerator);


            // ...then generate 42 so the result looks like...
            //                        50-86(37)
            //                      27         43
            //                    /               \    
            //            35-41(7)                 110-131(22)
            //          14        6              21           0
            //         /           \             /
            //   20-33(14)      43-48(6)     88-108(21)
            //  0         0    0        0   0          0
            mockery.ClearExpectation(mockRandomIntegerGenerator);
            using (mockery.Ordered)
            {
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(108L).Will(Return.Value(27L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(28L).Will(Return.Value(21L));
            }
            result = testUniqueRandomGenerator.Generate();
            Assert.AreEqual(42, result);
            allNodes = testUniqueRandomGenerator.AllNodes;
            Assert.AreEqual(6, allNodes.Count);
            Assert.IsNull(allNodes[50].ParentNode);
            Assert.AreSame(allNodes[35], allNodes[50].LeftChildNode);
            Assert.AreSame(allNodes[110], allNodes[50].RightChildNode);
            Assert.AreSame(allNodes[50], allNodes[35].ParentNode);
            Assert.AreSame(allNodes[20], allNodes[35].LeftChildNode);
            Assert.AreSame(allNodes[43], allNodes[35].RightChildNode);
            Assert.AreSame(allNodes[50], allNodes[110].ParentNode);
            Assert.AreSame(allNodes[88], allNodes[110].LeftChildNode);
            Assert.IsNull(allNodes[110].RightChildNode);
            Assert.AreSame(allNodes[35], allNodes[20].ParentNode);
            Assert.IsNull(allNodes[20].LeftChildNode);
            Assert.IsNull(allNodes[20].RightChildNode);
            Assert.AreSame(allNodes[35], allNodes[43].ParentNode);
            Assert.IsNull(allNodes[43].LeftChildNode);
            Assert.IsNull(allNodes[43].RightChildNode);
            Assert.AreSame(allNodes[110], allNodes[88].ParentNode);
            Assert.IsNull(allNodes[88].LeftChildNode);
            Assert.IsNull(allNodes[88].RightChildNode);
            Assert.AreEqual(3, allNodes[50].LeftSubtreeSize);
            Assert.AreEqual(2, allNodes[50].RightSubtreeSize);
            Assert.AreEqual(1, allNodes[35].LeftSubtreeSize);
            Assert.AreEqual(1, allNodes[35].RightSubtreeSize);
            Assert.AreEqual(1, allNodes[110].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[110].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[20].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[20].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[43].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[43].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[88].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[88].RightSubtreeSize);
            Assert.AreEqual(27, allNodes[50].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(43, allNodes[50].Item.RightSubtreeRangeCount);
            Assert.AreEqual(14, allNodes[35].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(6, allNodes[35].Item.RightSubtreeRangeCount);
            Assert.AreEqual(21, allNodes[110].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[110].Item.RightSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[20].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[20].Item.RightSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[43].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[43].Item.RightSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[88].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[88].Item.RightSubtreeRangeCount);
            Assert.AreEqual(37, allNodes[50].Item.Range.Length);
            Assert.AreEqual(7, allNodes[35].Item.Range.Length);
            Assert.AreEqual(22, allNodes[110].Item.Range.Length);
            Assert.AreEqual(14, allNodes[20].Item.Range.Length);
            Assert.AreEqual(6, allNodes[43].Item.Range.Length);
            Assert.AreEqual(21, allNodes[88].Item.Range.Length);
            mockery.VerifyAllExpectationsHaveBeenMet();
            ValidateRangeTree(testUniqueRandomGenerator);


            // ...then generate 119 so the result looks like...
            //                        50-86(37)
            //                      27         42
            //                    /               \    
            //            35-41(7)                 110-118(9)
            //          14        6              21          12
            //         /           \             /             \ 
            //   20-33(14)      43-48(6)     88-108(21)     120-131(12)
            //  0         0    0        0   0          0   0           0
            mockery.ClearExpectation(mockRandomIntegerGenerator);
            using (mockery.Ordered)
            {
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(107L).Will(Return.Value(64L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(43L).Will(Return.Value(30L));
            }
            result = testUniqueRandomGenerator.Generate();
            Assert.AreEqual(119, result);
            allNodes = testUniqueRandomGenerator.AllNodes;
            Assert.AreEqual(7, allNodes.Count);
            Assert.IsNull(allNodes[50].ParentNode);
            Assert.AreSame(allNodes[35], allNodes[50].LeftChildNode);
            Assert.AreSame(allNodes[110], allNodes[50].RightChildNode);
            Assert.AreSame(allNodes[50], allNodes[35].ParentNode);
            Assert.AreSame(allNodes[20], allNodes[35].LeftChildNode);
            Assert.AreSame(allNodes[43], allNodes[35].RightChildNode);
            Assert.AreSame(allNodes[50], allNodes[110].ParentNode);
            Assert.AreSame(allNodes[88], allNodes[110].LeftChildNode);
            Assert.AreSame(allNodes[120], allNodes[110].RightChildNode);
            Assert.AreSame(allNodes[35], allNodes[20].ParentNode);
            Assert.IsNull(allNodes[20].LeftChildNode);
            Assert.IsNull(allNodes[20].RightChildNode);
            Assert.AreSame(allNodes[35], allNodes[43].ParentNode);
            Assert.IsNull(allNodes[43].LeftChildNode);
            Assert.IsNull(allNodes[43].RightChildNode);
            Assert.AreSame(allNodes[110], allNodes[88].ParentNode);
            Assert.IsNull(allNodes[88].LeftChildNode);
            Assert.IsNull(allNodes[88].RightChildNode);
            Assert.AreSame(allNodes[110], allNodes[120].ParentNode);
            Assert.IsNull(allNodes[120].LeftChildNode);
            Assert.IsNull(allNodes[120].RightChildNode);
            Assert.AreEqual(3, allNodes[50].LeftSubtreeSize);
            Assert.AreEqual(3, allNodes[50].RightSubtreeSize);
            Assert.AreEqual(1, allNodes[35].LeftSubtreeSize);
            Assert.AreEqual(1, allNodes[35].RightSubtreeSize);
            Assert.AreEqual(1, allNodes[110].LeftSubtreeSize);
            Assert.AreEqual(1, allNodes[110].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[20].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[20].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[43].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[43].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[88].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[88].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[120].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[120].RightSubtreeSize);
            Assert.AreEqual(27, allNodes[50].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(42, allNodes[50].Item.RightSubtreeRangeCount);
            Assert.AreEqual(14, allNodes[35].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(6, allNodes[35].Item.RightSubtreeRangeCount);
            Assert.AreEqual(21, allNodes[110].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(12, allNodes[110].Item.RightSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[20].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[20].Item.RightSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[43].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[43].Item.RightSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[88].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[88].Item.RightSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[120].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[120].Item.RightSubtreeRangeCount);
            Assert.AreEqual(37, allNodes[50].Item.Range.Length);
            Assert.AreEqual(7, allNodes[35].Item.Range.Length);
            Assert.AreEqual(9, allNodes[110].Item.Range.Length);
            Assert.AreEqual(14, allNodes[20].Item.Range.Length);
            Assert.AreEqual(6, allNodes[43].Item.Range.Length);
            Assert.AreEqual(21, allNodes[88].Item.Range.Length);
            Assert.AreEqual(12, allNodes[120].Item.Range.Length);
            mockery.VerifyAllExpectationsHaveBeenMet();
            ValidateRangeTree(testUniqueRandomGenerator);


            // ...then generate 41-36 so the result looks like...
            //                        50-86(37)
            //                      21         42
            //                    /               \    
            //            35-35(1)                 110-118(9)
            //          14        6              21          12
            //         /           \             /             \ 
            //   20-33(14)      43-48(6)     88-108(21)     120-131(12)
            //  0         0    0        0   0          0   0           0
            mockery.ClearExpectation(mockRandomIntegerGenerator);
            using (mockery.Ordered)
            {
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(106L).Will(Return.Value(26L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(27L).Will(Return.Value(20L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(105L).Will(Return.Value(25L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(26L).Will(Return.Value(19L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(104L).Will(Return.Value(24L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(25L).Will(Return.Value(18L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(103L).Will(Return.Value(23L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(24L).Will(Return.Value(17L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(102L).Will(Return.Value(22L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(23L).Will(Return.Value(16L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(101L).Will(Return.Value(21L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(22L).Will(Return.Value(15L));
            }
            result = testUniqueRandomGenerator.Generate();
            Assert.AreEqual(41, result);
            result = testUniqueRandomGenerator.Generate();
            Assert.AreEqual(40, result);
            result = testUniqueRandomGenerator.Generate();
            Assert.AreEqual(39, result);
            result = testUniqueRandomGenerator.Generate();
            Assert.AreEqual(38, result);
            result = testUniqueRandomGenerator.Generate();
            Assert.AreEqual(37, result);
            result = testUniqueRandomGenerator.Generate();
            Assert.AreEqual(36, result);
            allNodes = testUniqueRandomGenerator.AllNodes;
            Assert.AreEqual(7, allNodes.Count);
            Assert.IsNull(allNodes[50].ParentNode);
            Assert.AreSame(allNodes[35], allNodes[50].LeftChildNode);
            Assert.AreSame(allNodes[110], allNodes[50].RightChildNode);
            Assert.AreSame(allNodes[50], allNodes[35].ParentNode);
            Assert.AreSame(allNodes[20], allNodes[35].LeftChildNode);
            Assert.AreSame(allNodes[43], allNodes[35].RightChildNode);
            Assert.AreSame(allNodes[50], allNodes[110].ParentNode);
            Assert.AreSame(allNodes[88], allNodes[110].LeftChildNode);
            Assert.AreSame(allNodes[120], allNodes[110].RightChildNode);
            Assert.AreSame(allNodes[35], allNodes[20].ParentNode);
            Assert.IsNull(allNodes[20].LeftChildNode);
            Assert.IsNull(allNodes[20].RightChildNode);
            Assert.AreSame(allNodes[35], allNodes[43].ParentNode);
            Assert.IsNull(allNodes[43].LeftChildNode);
            Assert.IsNull(allNodes[43].RightChildNode);
            Assert.AreSame(allNodes[110], allNodes[88].ParentNode);
            Assert.IsNull(allNodes[88].LeftChildNode);
            Assert.IsNull(allNodes[88].RightChildNode);
            Assert.AreSame(allNodes[110], allNodes[120].ParentNode);
            Assert.IsNull(allNodes[120].LeftChildNode);
            Assert.IsNull(allNodes[120].RightChildNode);
            Assert.AreEqual(3, allNodes[50].LeftSubtreeSize);
            Assert.AreEqual(3, allNodes[50].RightSubtreeSize);
            Assert.AreEqual(1, allNodes[35].LeftSubtreeSize);
            Assert.AreEqual(1, allNodes[35].RightSubtreeSize);
            Assert.AreEqual(1, allNodes[110].LeftSubtreeSize);
            Assert.AreEqual(1, allNodes[110].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[20].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[20].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[43].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[43].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[88].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[88].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[120].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[120].RightSubtreeSize);
            Assert.AreEqual(21, allNodes[50].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(42, allNodes[50].Item.RightSubtreeRangeCount);
            Assert.AreEqual(14, allNodes[35].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(6, allNodes[35].Item.RightSubtreeRangeCount);
            Assert.AreEqual(21, allNodes[110].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(12, allNodes[110].Item.RightSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[20].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[20].Item.RightSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[43].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[43].Item.RightSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[88].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[88].Item.RightSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[120].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[120].Item.RightSubtreeRangeCount);
            Assert.AreEqual(37, allNodes[50].Item.Range.Length);
            Assert.AreEqual(1, allNodes[35].Item.Range.Length);
            Assert.AreEqual(9, allNodes[110].Item.Range.Length);
            Assert.AreEqual(14, allNodes[20].Item.Range.Length);
            Assert.AreEqual(6, allNodes[43].Item.Range.Length);
            Assert.AreEqual(21, allNodes[88].Item.Range.Length);
            Assert.AreEqual(12, allNodes[120].Item.Range.Length);
            mockery.VerifyAllExpectationsHaveBeenMet();
            ValidateRangeTree(testUniqueRandomGenerator);


            // ...then generate 35 so node with start value 35 is removed and the result looks like...
            //                        50-86(37)
            //                      20         42
            //                    /               \    
            //            43-48(6)                 110-118(9)
            //          14        0              21          12
            //         /                         /             \ 
            //   20-33(14)                   88-108(21)     120-131(12)
            //  0         0                 0          0   0           0
            mockery.ClearExpectation(mockRandomIntegerGenerator);
            using (mockery.Ordered)
            {
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(100L).Will(Return.Value(20L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(21L).Will(Return.Value(14L));
            }

            result = testUniqueRandomGenerator.Generate();

            Assert.AreEqual(35, result);
            allNodes = testUniqueRandomGenerator.AllNodes;
            Assert.AreEqual(6, allNodes.Count);
            Assert.IsNull(allNodes[50].ParentNode);
            Assert.AreSame(allNodes[43], allNodes[50].LeftChildNode);
            Assert.AreSame(allNodes[110], allNodes[50].RightChildNode);
            Assert.AreSame(allNodes[50], allNodes[43].ParentNode);
            Assert.AreSame(allNodes[20], allNodes[43].LeftChildNode);
            Assert.IsNull(allNodes[43].RightChildNode);
            Assert.AreSame(allNodes[50], allNodes[110].ParentNode);
            Assert.AreSame(allNodes[88], allNodes[110].LeftChildNode);
            Assert.AreSame(allNodes[120], allNodes[110].RightChildNode);
            Assert.AreSame(allNodes[43], allNodes[20].ParentNode);
            Assert.IsNull(allNodes[20].LeftChildNode);
            Assert.IsNull(allNodes[20].RightChildNode);
            Assert.AreSame(allNodes[110], allNodes[88].ParentNode);
            Assert.IsNull(allNodes[88].LeftChildNode);
            Assert.IsNull(allNodes[88].RightChildNode);
            Assert.AreSame(allNodes[110], allNodes[120].ParentNode);
            Assert.IsNull(allNodes[120].LeftChildNode);
            Assert.IsNull(allNodes[120].RightChildNode);
            Assert.AreEqual(2, allNodes[50].LeftSubtreeSize);
            Assert.AreEqual(3, allNodes[50].RightSubtreeSize);
            Assert.AreEqual(1, allNodes[43].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[43].RightSubtreeSize);
            Assert.AreEqual(1, allNodes[110].LeftSubtreeSize);
            Assert.AreEqual(1, allNodes[110].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[20].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[20].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[88].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[88].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[120].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[120].RightSubtreeSize);
            Assert.AreEqual(20, allNodes[50].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(42, allNodes[50].Item.RightSubtreeRangeCount);
            Assert.AreEqual(14, allNodes[43].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[43].Item.RightSubtreeRangeCount);
            Assert.AreEqual(21, allNodes[110].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(12, allNodes[110].Item.RightSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[20].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[20].Item.RightSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[88].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[88].Item.RightSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[120].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[120].Item.RightSubtreeRangeCount);
            Assert.AreEqual(37, allNodes[50].Item.Range.Length);
            Assert.AreEqual(6, allNodes[43].Item.Range.Length);
            Assert.AreEqual(9, allNodes[110].Item.Range.Length);
            Assert.AreEqual(14, allNodes[20].Item.Range.Length);
            Assert.AreEqual(21, allNodes[88].Item.Range.Length);
            Assert.AreEqual(12, allNodes[120].Item.Range.Length);
            mockery.VerifyAllExpectationsHaveBeenMet();
            ValidateRangeTree(testUniqueRandomGenerator);
        }

        /// <summary>
        /// Success tests for the Generate() method where a node is split, and the new node is 'pushed' down to the left.
        /// </summary>
        [Test]
        public void Generate_SplitNewNodePushedDownToLeft()
        {
            // Special case to fix bug in setting sub tree sizes during bulk/volume/random testing
            // Setup the following tree...
            //              6-8(3)
            //             3      1
            //            /        \    
            //        4-4(1)      10-10(1)
            //       2      0    0        0
            //      /
            //   1-2(2)
            //  0      0
            UniqueRandomGeneratorWithProtectedMethods testUniqueRandomGenerator = new UniqueRandomGeneratorWithProtectedMethods(1, 10, mockRandomIntegerGenerator);
            mockery.ClearExpectation(mockRandomIntegerGenerator);
            using (mockery.Ordered)
            {
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(10L).Will(Return.Value(2L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(9L).Will(Return.Value(7L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(8L).Will(Return.Value(3L));
            }

            Int64 result = testUniqueRandomGenerator.Generate();
            Assert.AreEqual(3, result);
            result = testUniqueRandomGenerator.Generate();
            Assert.AreEqual(9, result);
            result = testUniqueRandomGenerator.Generate();
            Assert.AreEqual(5, result);

            Dictionary<Int64, WeightBalancedTreeNode<RangeAndSubtreeCounts>> allNodes = testUniqueRandomGenerator.AllNodes;
            Assert.AreEqual(4, allNodes.Count);
            Assert.IsNull(allNodes[6].ParentNode);
            Assert.AreSame(allNodes[4], allNodes[6].LeftChildNode);
            Assert.AreSame(allNodes[10], allNodes[6].RightChildNode);
            Assert.AreSame(allNodes[6], allNodes[4].ParentNode);
            Assert.AreSame(allNodes[1], allNodes[4].LeftChildNode);
            Assert.IsNull(allNodes[4].RightChildNode);
            Assert.AreSame(allNodes[6], allNodes[10].ParentNode);
            Assert.IsNull(allNodes[10].RightChildNode);
            Assert.IsNull(allNodes[10].LeftChildNode);
            Assert.AreSame(allNodes[4], allNodes[1].ParentNode);
            Assert.IsNull(allNodes[1].LeftChildNode);
            Assert.IsNull(allNodes[1].RightChildNode);
            Assert.AreEqual(2, allNodes[6].LeftSubtreeSize);
            Assert.AreEqual(1, allNodes[6].RightSubtreeSize);
            Assert.AreEqual(1, allNodes[4].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[4].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[10].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[10].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[1].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[1].RightSubtreeSize);
            Assert.AreEqual(3, allNodes[6].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(1, allNodes[6].Item.RightSubtreeRangeCount);
            Assert.AreEqual(2, allNodes[4].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[4].Item.RightSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[10].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[10].Item.RightSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[1].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[1].Item.RightSubtreeRangeCount);
            Assert.AreEqual(3, allNodes[6].Item.Range.Length);
            Assert.AreEqual(1, allNodes[4].Item.Range.Length);
            Assert.AreEqual(1, allNodes[10].Item.Range.Length);
            Assert.AreEqual(2, allNodes[1].Item.Range.Length);
            mockery.VerifyAllExpectationsHaveBeenMet();
            ValidateRangeTree(testUniqueRandomGenerator);
        }

        /// <summary>
        /// Success tests for the Generate() method where the underlying WeightBalancedTree.Remove() method is called.
        /// </summary>
        [Test]
        public void Generate_RemoveSingleLengthNodes()
        {
            // Test removing a single node by replacing with the next greater node
            UniqueRandomGeneratorWithProtectedMethods testUniqueRandomGenerator = CreateRangeTreeForRemoveTests(mockery, mockRandomIntegerGenerator);
            using (mockery.Ordered)
            {
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(20L).Will(Return.Value(8L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(12L).Will(Return.Value(3L));
            }

            Int64 result = testUniqueRandomGenerator.Generate();

            Assert.AreEqual(26, result);
            Dictionary<Int64, WeightBalancedTreeNode<RangeAndSubtreeCounts>> allNodes = testUniqueRandomGenerator.AllNodes;
            Assert.IsFalse(allNodes.ContainsKey(26));
            Assert.AreSame(allNodes[30], allNodes[32].ParentNode); 
            Assert.AreSame(allNodes[34], allNodes[30].ParentNode);
            Assert.IsNull(allNodes[30].LeftChildNode);
            Assert.AreSame(allNodes[32], allNodes[30].RightChildNode);
            Assert.AreEqual(0, allNodes[30].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(1, allNodes[30].Item.RightSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[30].LeftSubtreeSize);
            Assert.AreEqual(1, allNodes[30].RightSubtreeSize);
            Assert.AreSame(allNodes[28], allNodes[34].ParentNode);
            Assert.AreSame(allNodes[30], allNodes[34].LeftChildNode);
            Assert.AreSame(allNodes[38], allNodes[34].RightChildNode);
            Assert.AreEqual(2, allNodes[34].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(4, allNodes[34].Item.RightSubtreeRangeCount);
            Assert.AreEqual(2, allNodes[34].LeftSubtreeSize);
            Assert.AreEqual(3, allNodes[34].RightSubtreeSize);
            Assert.AreSame(allNodes[18], allNodes[28].ParentNode);
            Assert.AreSame(allNodes[22], allNodes[28].LeftChildNode);
            Assert.AreSame(allNodes[34], allNodes[28].RightChildNode);
            Assert.AreEqual(3, allNodes[28].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(7, allNodes[28].Item.RightSubtreeRangeCount);
            Assert.AreEqual(3, allNodes[28].LeftSubtreeSize);
            Assert.AreEqual(6, allNodes[28].RightSubtreeSize);
            Assert.AreSame(allNodes[28], allNodes[18].RightChildNode);
            mockery.VerifyAllExpectationsHaveBeenMet();
            ValidateRangeTree(testUniqueRandomGenerator);


            // Test removing a single node by replacing with the next lesser node
            testUniqueRandomGenerator = CreateRangeTreeForRemoveTests(mockery, mockRandomIntegerGenerator);
            mockery.ClearExpectation(mockRandomIntegerGenerator);
            using (mockery.Ordered)
            {
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(20L).Will(Return.Value(8L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(12L).Will(Return.Value(3L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(19L).Will(Return.Value(6L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(7L).Will(Return.Value(3L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(18L).Will(Return.Value(5L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(6L).Will(Return.Value(3L));
            } 
            // Remove a node on the right side of the tree to prevent automatic balancing
            result = testUniqueRandomGenerator.Generate();
            Assert.AreEqual(26, result);
            result = testUniqueRandomGenerator.Generate();
            Assert.AreEqual(10, result);

            result = testUniqueRandomGenerator.Generate();

            Assert.AreEqual(12, result);
            allNodes = testUniqueRandomGenerator.AllNodes;
            Assert.IsFalse(allNodes.ContainsKey(10));
            Assert.IsFalse(allNodes.ContainsKey(12));
            Assert.AreSame(allNodes[18], allNodes[8].ParentNode);
            Assert.AreSame(allNodes[6], allNodes[8].LeftChildNode);
            Assert.AreSame(allNodes[14], allNodes[8].RightChildNode);
            Assert.AreEqual(2, allNodes[8].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(2, allNodes[8].Item.RightSubtreeRangeCount);
            Assert.AreEqual(2, allNodes[8].LeftSubtreeSize);
            Assert.AreEqual(2, allNodes[8].RightSubtreeSize);
            Assert.AreSame(allNodes[8], allNodes[6].ParentNode);
            Assert.AreSame(allNodes[4], allNodes[6].LeftChildNode);
            Assert.IsNull(allNodes[6].RightChildNode);
            Assert.AreEqual(1, allNodes[6].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[6].Item.RightSubtreeRangeCount);
            Assert.AreEqual(1, allNodes[6].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[6].RightSubtreeSize);
            Assert.AreSame(allNodes[8], allNodes[14].ParentNode);
            Assert.IsNull(allNodes[14].LeftChildNode);
            Assert.AreSame(allNodes[16], allNodes[14].RightChildNode);
            Assert.AreEqual(0, allNodes[14].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(1, allNodes[14].Item.RightSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[14].LeftSubtreeSize);
            Assert.AreEqual(1, allNodes[14].RightSubtreeSize);
            Assert.AreSame(allNodes[6], allNodes[4].ParentNode);
            Assert.IsNull(allNodes[4].LeftChildNode);
            Assert.IsNull(allNodes[4].RightChildNode);
            Assert.AreEqual(0, allNodes[4].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[4].Item.RightSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[4].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[4].RightSubtreeSize);
            Assert.AreSame(allNodes[14], allNodes[16].ParentNode);
            Assert.IsNull(allNodes[16].LeftChildNode);
            Assert.IsNull(allNodes[16].RightChildNode);
            Assert.AreEqual(0, allNodes[16].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[16].Item.RightSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[16].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[16].RightSubtreeSize);
            mockery.VerifyAllExpectationsHaveBeenMet();
            ValidateRangeTree(testUniqueRandomGenerator);
        }

        /// <summary>
        /// Success tests for the Generate() method where the underlying WeightBalancedTree.Remove() method is on the root node.
        /// </summary>
        [Test]
        public void Generate_RemoveSingleLengthRootNode()
        {
            UniqueRandomGeneratorWithProtectedMethods testUniqueRandomGenerator = CreateRangeTreeForRemoveTests(mockery, mockRandomIntegerGenerator);
            using (mockery.Ordered)
            {
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(20L).Will(Return.Value(8L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(12L).Will(Return.Value(4L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(8L).Will(Return.Value(4L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(4L).Will(Return.Value(0L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(1L).Will(Return.Value(0L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(19L).Will(Return.Value(7L));
            }
            // Remove a node on the right side of the tree to prevent automatic balancing
            Int64 result = testUniqueRandomGenerator.Generate();
            Assert.AreEqual(36, result);

            result = testUniqueRandomGenerator.Generate();

            Assert.AreEqual(18, result);
            Dictionary<Int64, WeightBalancedTreeNode<RangeAndSubtreeCounts>> allNodes = testUniqueRandomGenerator.AllNodes;
            Assert.IsFalse(allNodes.ContainsKey(18));
            Assert.IsNull(allNodes[20].ParentNode);
            Assert.AreSame(allNodes[10], allNodes[20].LeftChildNode);
            Assert.AreSame(allNodes[26], allNodes[20].RightChildNode);
            Assert.AreEqual(7, allNodes[20].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(10, allNodes[20].Item.RightSubtreeRangeCount);
            Assert.AreEqual(7, allNodes[20].LeftSubtreeSize);
            Assert.AreEqual(9, allNodes[20].RightSubtreeSize);
            Assert.AreSame(allNodes[20], allNodes[26].ParentNode);
            Assert.AreSame(allNodes[22], allNodes[26].LeftChildNode);
            Assert.AreSame(allNodes[34], allNodes[26].RightChildNode);
            Assert.AreEqual(2, allNodes[26].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(7, allNodes[26].Item.RightSubtreeRangeCount);
            Assert.AreEqual(2, allNodes[26].LeftSubtreeSize);
            Assert.AreEqual(6, allNodes[26].RightSubtreeSize);
            Assert.AreSame(allNodes[26], allNodes[22].ParentNode);
            Assert.IsNull(allNodes[22].LeftChildNode);
            Assert.AreSame(allNodes[24], allNodes[22].RightChildNode);
            Assert.AreEqual(0, allNodes[22].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(1, allNodes[22].Item.RightSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[22].LeftSubtreeSize);
            Assert.AreEqual(1, allNodes[22].RightSubtreeSize);
            Assert.AreSame(allNodes[22], allNodes[24].ParentNode);
            Assert.IsNull(allNodes[24].LeftChildNode);
            Assert.IsNull(allNodes[24].RightChildNode);
            Assert.AreEqual(0, allNodes[24].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[24].Item.RightSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[24].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[24].RightSubtreeSize);
            mockery.VerifyAllExpectationsHaveBeenMet();
            ValidateRangeTree(testUniqueRandomGenerator);
        }

        /// <summary>
        /// Tests that the maximum value in Int64.MaxValue range can be generated.
        /// </summary>
        [Test]
        public void Generate_HighestValueInInt64MaxValueRangeCanBeGenerated()
        {
            UniqueRandomGeneratorWithProtectedMethods testUniqueRandomGenerator = new UniqueRandomGeneratorWithProtectedMethods(1, Int64.MaxValue, mockRandomIntegerGenerator);

            using(mockery.Ordered)
            {
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(Int64.MaxValue).Will(Return.Value(Int64.MaxValue - 1));
            }

            Int64 result = testUniqueRandomGenerator.Generate();

            Assert.AreEqual(Int64.MaxValue, result);
        }

        #region Private Methods

        /// <summary>
        /// Validates the structure of the range tree underlying the specified unique random generator. 
        /// </summary>
        /// <param name="testUniqueRandomGenerator">The unique random generator to validate.</param>
        private void ValidateRangeTree(UniqueRandomGeneratorWithProtectedMethods testUniqueRandomGenerator)
        {
            WeightBalancedTreeNode<RangeAndSubtreeCounts> rootNode = testUniqueRandomGenerator.RangeTree.RootNode;
            Int64 totalRangeCount = rootNode.Item.LeftSubtreeRangeCount + rootNode.Item.Range.Length + rootNode.Item.RightSubtreeRangeCount;
            Int32 totalNodeCount = rootNode.LeftSubtreeSize + 1 + rootNode.RightSubtreeSize;
            ValidateRangeTreeRecurse(rootNode, null, totalRangeCount, totalNodeCount, Int64.MinValue, Int64.MaxValue);
        }

        /// <summary>
        /// Recursively Validates the structure of a node of a range tree underlying a unique random generator. 
        /// </summary>
        /// <param name="currentNode">The node to validate.</param>
        /// <param name="parentNode">The parent of the node to validate.</param>
        /// <param name="totalRangeCount">The expected total range count for the node and its children.</param>
        /// <param name="totalNodeCount">The expected total node count for the node and its children.</param>
        /// <param name="lowerRangeLimit">The inclusive lower limit of the ranges at and below the current node.</param>
        /// <param name="upperRangeLimit">The inclusive upper limit of the ranges at and below the current node.</param>
        private void ValidateRangeTreeRecurse(WeightBalancedTreeNode<RangeAndSubtreeCounts> currentNode, WeightBalancedTreeNode<RangeAndSubtreeCounts> parentNode, Int64 totalRangeCount, Int32 totalNodeCount, Int64 lowerRangeLimit, Int64 upperRangeLimit)
        {
            // Validate
            if (currentNode.ParentNode != parentNode)
            {
                if (currentNode.ParentNode != null)
                    throw new Exception($"Parent of node containing range starting with {currentNode.Item.Range.StartValue} was expected to be node {parentNode.Item.Range.StartValue} but was node {currentNode.ParentNode.Item.Range.StartValue}.");
                else
                    throw new Exception($"Parent of node containing range starting with {currentNode.Item.Range.StartValue} was expected to be node {parentNode.Item.Range.StartValue} but was null.");
            }
            Int64 actualTotalRangeCount = currentNode.Item.LeftSubtreeRangeCount + currentNode.Item.Range.Length + currentNode.Item.RightSubtreeRangeCount;
            if (actualTotalRangeCount != totalRangeCount)
                throw new Exception($"Node containing range starting with {currentNode.Item.Range.StartValue} was expected to have total range count of {totalRangeCount} but actually had {actualTotalRangeCount} ({currentNode.Item.LeftSubtreeRangeCount}, {currentNode.Item.Range.Length}, {currentNode.Item.RightSubtreeRangeCount}).");
            Int32 actualTotalNodeCount = currentNode.LeftSubtreeSize + 1 + currentNode.RightSubtreeSize;
            if (actualTotalNodeCount != totalNodeCount)
                throw new Exception($"Node containing range starting with {currentNode.Item.Range.StartValue} was expected to have total node count of {totalNodeCount} but actually had {actualTotalNodeCount} ({currentNode.LeftSubtreeSize}, {currentNode.Item.Range.Length}, {currentNode.RightSubtreeSize}).");
            if (currentNode.Item.Range.StartValue < lowerRangeLimit)
                throw new Exception($"Node containing range starting with {currentNode.Item.Range.StartValue} should be greater than or equal to lower limit {lowerRangeLimit}.");
            if (currentNode.Item.Range.EndValue > upperRangeLimit)
                throw new Exception($"Node containing range starting with {currentNode.Item.Range.StartValue} and inclusive end value {currentNode.Item.Range.EndValue} should be less than or equal to upper limit {upperRangeLimit}.");

            // Recurse
            if (currentNode.LeftChildNode != null)
                ValidateRangeTreeRecurse(currentNode.LeftChildNode, currentNode, currentNode.Item.LeftSubtreeRangeCount, currentNode.LeftSubtreeSize, lowerRangeLimit, currentNode.Item.Range.StartValue - 1);
            if (currentNode.RightChildNode != null)
                ValidateRangeTreeRecurse(currentNode.RightChildNode, currentNode, currentNode.Item.RightSubtreeRangeCount, currentNode.RightSubtreeSize, currentNode.Item.Range.EndValue + 1, upperRangeLimit);
        }

        /// <summary>
        /// Creates a unique random generator with an underlying range tree containing many nodes with range length 1, for test cases where the underlying WeightBalancedTree.Remove() method is called.
        /// </summary>
        /// <param name="mockery">The mockery that mocks are created from.</param>
        /// <param name="mockRandomIntegerGenerator">The mock random integer generator.</param>
        /// <returns>The initialized unique random generator.</returns>
        private UniqueRandomGeneratorWithProtectedMethods CreateRangeTreeForRemoveTests(Mockery mockery, IRandomIntegerGenerator mockRandomIntegerGenerator)
        {
            // The following tree is created...
            //                                        18-18(1)
            //                                       7        12
            //                                /                       \
            //                 10-10(1)                                            26-26(1)
            //                3        3                                          3        8
            //               /          \                                      /              \
            //          6-6(1)          14-14(1)                     22-22(1)                    34-34(1)
            //         1      1        1        1                   1        1                  3        4 
            //        /        \      /          \                 /           \            /                \
            //   4-4(1)    8-8(1)    12-12(1)    16-16(1)    20-20(1)    24-24(1)    30-30(1)                 38-38(1)
            //  0      0  0      0  0        0  0        0  0        0  0        0  1        1               1        2
            //                                                                     /          \             /          \ 
            //                                                                 28-28(1)     32-32(1)    36-36(1)     40-41(2)
            //                                                                0        0   0        0  0        0   0        0
            UniqueRandomGeneratorWithProtectedMethods returnUniqueRandomGenerator = new UniqueRandomGeneratorWithProtectedMethods(4, 41, mockRandomIntegerGenerator);
            for (Int64 numberToGenerate = returnUniqueRandomGenerator.RangeStart + 1; numberToGenerate < returnUniqueRandomGenerator.RangeEnd; numberToGenerate += 2)
            {
                SetMockRandomIntegerGeneratorExpectations(returnUniqueRandomGenerator, numberToGenerate, mockery, mockRandomIntegerGenerator);
                Int64 result = returnUniqueRandomGenerator.Generate();
                Assert.AreEqual(numberToGenerate, result);
            }
            mockery.ClearExpectation(mockRandomIntegerGenerator);
            // Check the structure of the tree
            Dictionary<Int64, WeightBalancedTreeNode<RangeAndSubtreeCounts>> allNodes = returnUniqueRandomGenerator.AllNodes;
            Assert.IsNull(allNodes[18].ParentNode);
            Assert.AreSame(allNodes[10], allNodes[18].LeftChildNode);
            Assert.AreSame(allNodes[26], allNodes[18].RightChildNode);
            Assert.AreEqual(7, allNodes[18].LeftSubtreeSize);
            Assert.AreEqual(11, allNodes[18].RightSubtreeSize);
            Assert.AreEqual(7, allNodes[18].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(12, allNodes[18].Item.RightSubtreeRangeCount);
            Assert.AreEqual(1, allNodes[18].Item.Range.Length);
            Assert.AreSame(allNodes[18], allNodes[10].ParentNode);
            Assert.AreSame(allNodes[6], allNodes[10].LeftChildNode);
            Assert.AreSame(allNodes[14], allNodes[10].RightChildNode);
            Assert.AreEqual(3, allNodes[10].LeftSubtreeSize);
            Assert.AreEqual(3, allNodes[10].RightSubtreeSize);
            Assert.AreEqual(3, allNodes[10].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(3, allNodes[10].Item.RightSubtreeRangeCount);
            Assert.AreEqual(1, allNodes[10].Item.Range.Length);
            Assert.AreSame(allNodes[10], allNodes[6].ParentNode);
            Assert.AreSame(allNodes[4], allNodes[6].LeftChildNode);
            Assert.AreSame(allNodes[8], allNodes[6].RightChildNode);
            Assert.AreEqual(1, allNodes[6].LeftSubtreeSize);
            Assert.AreEqual(1, allNodes[6].RightSubtreeSize);
            Assert.AreEqual(1, allNodes[6].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(1, allNodes[6].Item.RightSubtreeRangeCount);
            Assert.AreEqual(1, allNodes[6].Item.Range.Length);
            Assert.AreSame(allNodes[6], allNodes[4].ParentNode);
            Assert.IsNull(allNodes[4].LeftChildNode);
            Assert.IsNull(allNodes[4].RightChildNode);
            Assert.AreEqual(0, allNodes[4].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[4].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[4].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[4].Item.RightSubtreeRangeCount);
            Assert.AreEqual(1, allNodes[4].Item.Range.Length);
            Assert.AreSame(allNodes[6], allNodes[8].ParentNode);
            Assert.IsNull(allNodes[8].LeftChildNode);
            Assert.IsNull(allNodes[8].RightChildNode);
            Assert.AreEqual(0, allNodes[8].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[8].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[8].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[8].Item.RightSubtreeRangeCount);
            Assert.AreEqual(1, allNodes[8].Item.Range.Length);
            Assert.AreSame(allNodes[10], allNodes[14].ParentNode);
            Assert.AreSame(allNodes[12], allNodes[14].LeftChildNode);
            Assert.AreSame(allNodes[16], allNodes[14].RightChildNode);
            Assert.AreEqual(1, allNodes[14].LeftSubtreeSize);
            Assert.AreEqual(1, allNodes[14].RightSubtreeSize);
            Assert.AreEqual(1, allNodes[14].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(1, allNodes[14].Item.RightSubtreeRangeCount);
            Assert.AreEqual(1, allNodes[14].Item.Range.Length);
            Assert.AreSame(allNodes[14], allNodes[12].ParentNode);
            Assert.IsNull(allNodes[12].LeftChildNode);
            Assert.IsNull(allNodes[12].RightChildNode);
            Assert.AreEqual(0, allNodes[12].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[12].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[12].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[12].Item.RightSubtreeRangeCount);
            Assert.AreEqual(1, allNodes[12].Item.Range.Length);
            Assert.AreSame(allNodes[14], allNodes[16].ParentNode);
            Assert.IsNull(allNodes[16].LeftChildNode);
            Assert.IsNull(allNodes[16].RightChildNode);
            Assert.AreEqual(0, allNodes[16].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[16].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[16].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[16].Item.RightSubtreeRangeCount);
            Assert.AreEqual(1, allNodes[16].Item.Range.Length);
            Assert.AreSame(allNodes[18], allNodes[26].ParentNode);
            Assert.AreSame(allNodes[22], allNodes[26].LeftChildNode);
            Assert.AreSame(allNodes[34], allNodes[26].RightChildNode);
            Assert.AreEqual(3, allNodes[26].LeftSubtreeSize);
            Assert.AreEqual(7, allNodes[26].RightSubtreeSize);
            Assert.AreEqual(3, allNodes[26].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(8, allNodes[26].Item.RightSubtreeRangeCount);
            Assert.AreEqual(1, allNodes[26].Item.Range.Length);
            Assert.AreSame(allNodes[26], allNodes[22].ParentNode);
            Assert.AreSame(allNodes[20], allNodes[22].LeftChildNode);
            Assert.AreSame(allNodes[24], allNodes[22].RightChildNode);
            Assert.AreEqual(1, allNodes[22].LeftSubtreeSize);
            Assert.AreEqual(1, allNodes[22].RightSubtreeSize);
            Assert.AreEqual(1, allNodes[22].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(1, allNodes[22].Item.RightSubtreeRangeCount);
            Assert.AreEqual(1, allNodes[22].Item.Range.Length);
            Assert.AreSame(allNodes[22], allNodes[20].ParentNode);
            Assert.IsNull(allNodes[20].LeftChildNode);
            Assert.IsNull(allNodes[20].RightChildNode);
            Assert.AreEqual(0, allNodes[20].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[20].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[20].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[20].Item.RightSubtreeRangeCount);
            Assert.AreEqual(1, allNodes[20].Item.Range.Length);
            Assert.AreSame(allNodes[22], allNodes[24].ParentNode);
            Assert.IsNull(allNodes[24].LeftChildNode);
            Assert.IsNull(allNodes[24].RightChildNode);
            Assert.AreEqual(0, allNodes[24].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[24].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[24].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[24].Item.RightSubtreeRangeCount);
            Assert.AreEqual(1, allNodes[24].Item.Range.Length);
            Assert.AreSame(allNodes[26], allNodes[34].ParentNode);
            Assert.AreSame(allNodes[30], allNodes[34].LeftChildNode);
            Assert.AreSame(allNodes[38], allNodes[34].RightChildNode);
            Assert.AreEqual(3, allNodes[34].LeftSubtreeSize);
            Assert.AreEqual(3, allNodes[34].RightSubtreeSize);
            Assert.AreEqual(3, allNodes[34].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(4, allNodes[34].Item.RightSubtreeRangeCount);
            Assert.AreEqual(1, allNodes[34].Item.Range.Length);
            Assert.AreSame(allNodes[34], allNodes[30].ParentNode);
            Assert.AreSame(allNodes[28], allNodes[30].LeftChildNode);
            Assert.AreSame(allNodes[32], allNodes[30].RightChildNode);
            Assert.AreEqual(1, allNodes[30].LeftSubtreeSize);
            Assert.AreEqual(1, allNodes[30].RightSubtreeSize);
            Assert.AreEqual(1, allNodes[30].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(1, allNodes[30].Item.RightSubtreeRangeCount);
            Assert.AreEqual(1, allNodes[30].Item.Range.Length);
            Assert.AreSame(allNodes[30], allNodes[28].ParentNode);
            Assert.IsNull(allNodes[28].LeftChildNode);
            Assert.IsNull(allNodes[28].RightChildNode);
            Assert.AreEqual(0, allNodes[28].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[28].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[28].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[28].Item.RightSubtreeRangeCount);
            Assert.AreEqual(1, allNodes[28].Item.Range.Length);
            Assert.AreSame(allNodes[30], allNodes[32].ParentNode);
            Assert.IsNull(allNodes[32].LeftChildNode);
            Assert.IsNull(allNodes[32].RightChildNode);
            Assert.AreEqual(0, allNodes[32].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[32].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[32].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[32].Item.RightSubtreeRangeCount);
            Assert.AreEqual(1, allNodes[32].Item.Range.Length);
            Assert.AreSame(allNodes[34], allNodes[38].ParentNode);
            Assert.AreSame(allNodes[36], allNodes[38].LeftChildNode);
            Assert.AreSame(allNodes[40], allNodes[38].RightChildNode);
            Assert.AreEqual(1, allNodes[38].LeftSubtreeSize);
            Assert.AreEqual(1, allNodes[38].RightSubtreeSize);
            Assert.AreEqual(1, allNodes[38].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(2, allNodes[38].Item.RightSubtreeRangeCount);
            Assert.AreEqual(1, allNodes[38].Item.Range.Length);
            Assert.AreSame(allNodes[38], allNodes[36].ParentNode);
            Assert.IsNull(allNodes[36].LeftChildNode);
            Assert.IsNull(allNodes[36].RightChildNode);
            Assert.AreEqual(0, allNodes[36].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[36].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[36].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[36].Item.RightSubtreeRangeCount);
            Assert.AreEqual(1, allNodes[36].Item.Range.Length);
            Assert.AreSame(allNodes[38], allNodes[40].ParentNode);
            Assert.IsNull(allNodes[40].LeftChildNode);
            Assert.IsNull(allNodes[40].RightChildNode);
            Assert.AreEqual(0, allNodes[40].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[40].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[40].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[40].Item.RightSubtreeRangeCount);
            Assert.AreEqual(2, allNodes[40].Item.Range.Length);

            return returnUniqueRandomGenerator;
        }

        /// <summary>
        /// Sets expects on a mock random integer generator for generating a specified number from a call to the Generate() method.
        /// </summary>
        /// <param name="testUniqueRandomGenerator">The unique random generator the Generate() method will be called on.</param>
        /// <param name="numberToGenerate">The number to be returned by the call to the Generate() method.</param>
        /// <param name="mockery">The mockery object the mock random integer generator was created from.</param>
        /// <param name="mockRandomIntegerGenerator">The mock random integer generator.</param>
        private void SetMockRandomIntegerGeneratorExpectations(UniqueRandomGeneratorWithProtectedMethods testUniqueRandomGenerator, Int64 numberToGenerate, Mockery mockery, IRandomIntegerGenerator mockRandomIntegerGenerator)
        {
            mockery.ClearExpectation(mockRandomIntegerGenerator);
            Dictionary<Int64, WeightBalancedTreeNode<RangeAndSubtreeCounts>> allNodes = testUniqueRandomGenerator.AllNodes;
            // Find the root node of the range tree
            WeightBalancedTreeNode<RangeAndSubtreeCounts> currentNode = null;
            foreach (WeightBalancedTreeNode<RangeAndSubtreeCounts> currNode in allNodes.Values)
            {
                if (currNode.ParentNode == null)
                {
                    currentNode = currNode;
                    break;
                }
            }
            if (currentNode == null)
                throw new Exception("Root node of range tree could not be found.");

            while (true)
            {
                Int64 nodeStartValue = currentNode.Item.Range.StartValue;
                Int64 nodeEndValue = currentNode.Item.Range.StartValue + currentNode.Item.Range.Length - 1;
                Int64 randomGeneratorNextParameter = currentNode.Item.LeftSubtreeRangeCount + currentNode.Item.Range.Length + currentNode.Item.RightSubtreeRangeCount;
                if (numberToGenerate >= nodeStartValue && numberToGenerate <= nodeEndValue)
                {
                    Int64 returnValue = currentNode.Item.LeftSubtreeRangeCount + numberToGenerate - currentNode.Item.Range.StartValue;
                    Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(randomGeneratorNextParameter).Will(Return.Value(returnValue));
                    // DEBUG: Console.WriteLine("{0}, {1}", randomGeneratorNextParameter, returnValue);
                    break;
                }
                else if (numberToGenerate < nodeStartValue)
                {
                    if (currentNode.Item.LeftSubtreeRangeCount == 0)
                        throw new ArgumentException($"Value in parameter '{nameof(numberToGenerate)}' ({numberToGenerate}) doesn't exist in the range tree.", nameof(numberToGenerate));
                    else
                    {
                        Int64 returnValue = currentNode.Item.LeftSubtreeRangeCount - 1;
                        Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(randomGeneratorNextParameter).Will(Return.Value(returnValue));
                        currentNode = currentNode.LeftChildNode;
                        // DEBUG: Console.WriteLine("{0}, {1}", randomGeneratorNextParameter, returnValue);
                    }
                }
                else
                {
                    if (currentNode.Item.RightSubtreeRangeCount == 0)
                        throw new ArgumentException($"Value in parameter '{nameof(numberToGenerate)}' ({numberToGenerate}) doesn't exist in the range tree.", nameof(numberToGenerate));
                    else
                    {
                        Int64 returnValue = currentNode.Item.LeftSubtreeRangeCount + currentNode.Item.Range.Length;
                        Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(randomGeneratorNextParameter).Will(Return.Value(returnValue));
                        currentNode = currentNode.RightChildNode;
                        // DEBUG: Console.WriteLine("{0}, {1}", randomGeneratorNextParameter, returnValue);
                    }
                }
            }
        }

        /// <summary>
        /// Generates NUnit Assert statements for the range tree underlying a unique random generator.
        /// </summary>
        /// <param name="testUniqueRandomGenerator">The unique random generator to generate the statements for.</param>
        /// <remarks>Should only be used on range trees which are known to have a correct/valid structure.</remarks>
        private void GenerateAssertStatementsForRangeTree(UniqueRandomGeneratorWithProtectedMethods testUniqueRandomGenerator)
        {
            Dictionary<Int64, WeightBalancedTreeNode<RangeAndSubtreeCounts>> allNodes = testUniqueRandomGenerator.AllNodes;
            foreach (WeightBalancedTreeNode<RangeAndSubtreeCounts> currNode in allNodes.Values)
            {
                if (currNode.ParentNode == null)
                    Console.WriteLine("Assert.IsNull(allNodes[{0}].ParentNode);", currNode.Item.Range.StartValue);
                else
                    Console.WriteLine("Assert.AreSame(allNodes[{0}], allNodes[{1}].ParentNode);", currNode.ParentNode.Item.Range.StartValue, currNode.Item.Range.StartValue);
                if (currNode.LeftChildNode == null)
                    Console.WriteLine("Assert.IsNull(allNodes[{0}].LeftChildNode);", currNode.Item.Range.StartValue);
                else
                    Console.WriteLine("Assert.AreSame(allNodes[{0}], allNodes[{1}].LeftChildNode);", currNode.LeftChildNode.Item.Range.StartValue, currNode.Item.Range.StartValue);
                if (currNode.RightChildNode == null)
                    Console.WriteLine("Assert.IsNull(allNodes[{0}].RightChildNode);", currNode.Item.Range.StartValue);
                else
                    Console.WriteLine("Assert.AreSame(allNodes[{0}], allNodes[{1}].RightChildNode);", currNode.RightChildNode.Item.Range.StartValue, currNode.Item.Range.StartValue);
                Console.WriteLine("Assert.AreEqual({0}, allNodes[{1}].LeftSubtreeSize);", currNode.LeftSubtreeSize, currNode.Item.Range.StartValue);
                Console.WriteLine("Assert.AreEqual({0}, allNodes[{1}].RightSubtreeSize);", currNode.RightSubtreeSize, currNode.Item.Range.StartValue);
                Console.WriteLine("Assert.AreEqual({0}, allNodes[{1}].Item.LeftSubtreeRangeCount);", currNode.Item.LeftSubtreeRangeCount, currNode.Item.Range.StartValue);
                Console.WriteLine("Assert.AreEqual({0}, allNodes[{1}].Item.RightSubtreeRangeCount);", currNode.Item.RightSubtreeRangeCount, currNode.Item.Range.StartValue);
                Console.WriteLine("Assert.AreEqual({0}, allNodes[{1}].Item.Range.Length);", currNode.Item.Range.Length, currNode.Item.Range.StartValue);
            }
        }

        #endregion

        #region Nested Classes

        /// <summary>
        /// Version of the UniqueRandomGenerator class where private and protected methods and members are exposed as public so that they can be unit tested.
        /// </summary>
        private class UniqueRandomGeneratorWithProtectedMethods : UniqueRandomGenerator
        {
            /// <summary>
            /// The underlying tree of integer ranges used to generate randoms from.
            /// </summary>
            public new RangeTree RangeTree
            {
                get { return rangeTree; }
            }

            /// <summary>
            /// Returns a dictionary containing all the nodes of the specified range tree as its values, and keyed by start value of the node's range.
            /// </summary>
            public Dictionary<Int64, WeightBalancedTreeNode<RangeAndSubtreeCounts>> AllNodes
            {
                get
                {
                    var returnDictionary = new Dictionary<Int64, WeightBalancedTreeNode<RangeAndSubtreeCounts>>();
                    Action<WeightBalancedTreeNode<RangeAndSubtreeCounts>> addNodesToDictionaryAction = (node) =>
                    {
                        returnDictionary.Add(node.Item.Range.StartValue, node);
                    };
                    rangeTree.PreOrderDepthFirstSearch(addNodesToDictionaryAction);

                    return returnDictionary;
                }
            }

            /// <summary>
            /// Initialises a new instance of the MoreComplexDataStructures.UnitTests.UniqueRandomGeneratorTests+UniqueRandomGeneratorWithProtectedMethods class.
            /// </summary>
            /// <param name="rangeStart">The inclusive start of the range to generate random numbers from.</param>
            /// <param name="rangeEnd">The inclusive end of the range to generate random numbers from.</param>
            /// <param name="randomIntegerGenerator">An implementation of interface IRandomIntegerGenerator to use for selecting random items.</param>
            public UniqueRandomGeneratorWithProtectedMethods(Int64 rangeStart, Int64 rangeEnd, IRandomIntegerGenerator randomIntegerGenerator)
                : base(rangeStart, rangeEnd, randomIntegerGenerator)
            {
            }
        }

        #endregion
    }
}
