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
using NMock2;
using MoreComplexDataStructures;

namespace MoreComplexDataStructures.UnitTests
{
    /// <summary>
    /// Unit tests for the WeightedRandomGenerator class.
    /// </summary>
    public class WeightedRandomGeneratorTests
    {
        private Mockery mockery;
        private WeightedRandomGenerator<Char> testWeightedRandomGenerator;

        [SetUp]
        protected void SetUp()
        {
            mockery = new Mockery();
            testWeightedRandomGenerator = new WeightedRandomGenerator<Char>();
        }

        /// <summary>
        /// Success tests for the Clear() method.
        /// </summary>
        [Test]
        public void Clear()
        {
            List<Tuple<Char, Int64>> weightings = new List<Tuple<Char, Int64>>();
            weightings.Add(new Tuple<Char, Int64>('a', 1));
            weightings.Add(new Tuple<Char, Int64>('b', 2));
            weightings.Add(new Tuple<Char, Int64>('c', 4));

            testWeightedRandomGenerator.SetWeightings(weightings);

            Dictionary<Int64, Int64> treeContents = GetAllWeightingRanges(testWeightedRandomGenerator);
            Assert.AreNotEqual(0, treeContents.Count);

            testWeightedRandomGenerator.Clear();

            treeContents = GetAllWeightingRanges(testWeightedRandomGenerator);
            Assert.AreEqual(0, treeContents.Count);
        }

        /// <summary>
        /// Tests that an exception is thrown if the SetWeightings() method is called with an empty set of weightings.
        /// </summary>
        [Test]
        public void SetWeightings_WeightingsEmpty()
        {
            List<Tuple<Char, Int64>> weightings = new List<Tuple<Char, Int64>>();

            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                testWeightedRandomGenerator.SetWeightings(weightings);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The specified set of weightings is empty."));
            Assert.AreEqual("weightings", e.ParamName);
        }

        /// <summary>
        /// Tests that an exception is thrown if the SetWeightings() method is called with a set of weightings whose total exceeds Int64.MaxValue.
        /// </summary>
        [Test]
        public void SetWeightings_WeightingsTotalExceedsInt64Minus1()
        {
            List<Tuple<Char, Int64>> weightings = new List<Tuple<Char, Int64>>();
            weightings.Add(new Tuple<Char, Int64>('a', 9223372036854775806));
            weightings.Add(new Tuple<Char, Int64>('b', 2));

            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                testWeightedRandomGenerator.SetWeightings(weightings);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The total of the specified weightings cannot exceed Int64.MaxValue."));
            Assert.AreEqual("weightings", e.ParamName);
        }

        /// <summary>
        /// Tests that an exception is thrown if the SetWeightings() method is called with a set of weightings which contains duplicate items.
        /// </summary>
        [Test]
        public void SetWeightings_WeightingsContainDuplicateItems()
        {
            List<Tuple<Char, Int64>> weightings = new List<Tuple<Char, Int64>>();
            weightings.Add(new Tuple<Char, Int64>('a', 4));
            weightings.Add(new Tuple<Char, Int64>('b', 3));
            weightings.Add(new Tuple<Char, Int64>('a', 2));
            weightings.Add(new Tuple<Char, Int64>('c', 1));

            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                testWeightedRandomGenerator.SetWeightings(weightings);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The specified weightings contain duplicate item with value = 'a'."));
            Assert.AreEqual("weightings", e.ParamName);
        }

        /// <summary>
        /// Success tests for the SetWeightings() method.
        /// </summary>
        [Test]
        public void SetWeightings()
        {
            // Test adding weightings up to Int64.MaxValue does not throw exception
            List<Tuple<Char, Int64>> weightings = new List<Tuple<Char, Int64>>();
            weightings.Add(new Tuple<Char, Int64>('a', 9223372036854775806));
            weightings.Add(new Tuple<Char, Int64>('b', 1));

            testWeightedRandomGenerator.SetWeightings(weightings);

            Dictionary<Int64, Int64> treeContents = GetAllWeightingRanges(testWeightedRandomGenerator);
            Assert.AreEqual(2, treeContents.Count);
            Assert.AreEqual(9223372036854775806, treeContents[0]);
            Assert.AreEqual(1, treeContents[9223372036854775806]);


            // Test with more usual weightings
            testWeightedRandomGenerator = new WeightedRandomGenerator<Char>();
            weightings.Clear();
            weightings.Add(new Tuple<Char, Int64>('a', 1));
            weightings.Add(new Tuple<Char, Int64>('b', 2));
            weightings.Add(new Tuple<Char, Int64>('c', 4));

            testWeightedRandomGenerator.SetWeightings(weightings);

            treeContents = GetAllWeightingRanges(testWeightedRandomGenerator);
            Assert.AreEqual(3, treeContents.Count);
            Assert.AreEqual(1, treeContents[0]);
            Assert.AreEqual(2, treeContents[1]);
            Assert.AreEqual(4, treeContents[3]);
        }

        /// <summary>
        /// Tests that an exception is thrown if the Generate() method is called when no weightings have been defined.
        /// </summary>
        [Test]
        public void Generate_NoWeightingsDefined()
        {
            InvalidOperationException e = Assert.Throws<InvalidOperationException>(delegate
            {
                testWeightedRandomGenerator.Generate();
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("No weightings are defined."));
        }

        /// <summary>
        /// Success tests for the Generate() method.
        /// </summary>
        [Test]
        public void Generate()
        {
            IRandomIntegerGenerator mockRandomIntegerGenerator = mockery.NewMock<IRandomIntegerGenerator>();
            testWeightedRandomGenerator = new WeightedRandomGenerator<Char>(mockRandomIntegerGenerator);

            // Test with a single weighting of 1
            //   First expect on mockRandomIntegerGenerator is for the call to SetWeightings(), second expect is for the call to Generate()
            using (mockery.Ordered)
            {
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(1).Will(Return.Value(0));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(1L).Will(Return.Value(0L));
            }
            testWeightedRandomGenerator.SetWeightings(new List<Tuple<Char, Int64>>() { new Tuple<Char, Int64>('a', 1) });
            Char result = testWeightedRandomGenerator.Generate();

            Assert.AreEqual('a', result);
            mockery.VerifyAllExpectationsHaveBeenMet();


            // Test with 2 weightings, first a single value of 1, and the second the entire remaining space in the (Int64.MaxValue - 1) range
            mockery.ClearExpectation(mockRandomIntegerGenerator);
            List<Tuple<Char, Int64>> weightings = new List<Tuple<Char, Int64>>
            {
                new Tuple<Char, Int64>('a', 1), 
                new Tuple<Char, Int64>('b', Int64.MaxValue - 1)
            };
            using (mockery.Ordered)
            {
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(2).Will(Return.Value(1));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(1).Will(Return.Value(0));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(Int64.MaxValue).Will(Return.Value(0L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(Int64.MaxValue).Will(Return.Value(1L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(Int64.MaxValue).Will(Return.Value(Int64.MaxValue - 1));
            }
            testWeightedRandomGenerator.SetWeightings(weightings);

            result = testWeightedRandomGenerator.Generate();
            Assert.AreEqual('a', result);
            result = testWeightedRandomGenerator.Generate();
            Assert.AreEqual('b', result);
            result = testWeightedRandomGenerator.Generate();
            Assert.AreEqual('b', result);
            mockery.VerifyAllExpectationsHaveBeenMet();


            // Test with 2 weightings, first with (Int64.MaxValue - 1) range, and the second with 1
            mockery.ClearExpectation(mockRandomIntegerGenerator);
            weightings = new List<Tuple<Char, Int64>>
            {
                new Tuple<Char, Int64>('a', Int64.MaxValue - 1), 
                new Tuple<Char, Int64>('b', 1) 
            };
            using (mockery.Ordered)
            {
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(2).Will(Return.Value(1));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(1).Will(Return.Value(0));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(Int64.MaxValue).Will(Return.Value(0L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(Int64.MaxValue).Will(Return.Value(Int64.MaxValue - 2));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(Int64.MaxValue).Will(Return.Value(Int64.MaxValue - 1));
            }
            testWeightedRandomGenerator.SetWeightings(weightings);

            result = testWeightedRandomGenerator.Generate();
            Assert.AreEqual('a', result);
            result = testWeightedRandomGenerator.Generate();
            Assert.AreEqual('a', result);
            result = testWeightedRandomGenerator.Generate();
            Assert.AreEqual('b', result);
            mockery.VerifyAllExpectationsHaveBeenMet();


            // Test with more standard weightings
            mockery.ClearExpectation(mockRandomIntegerGenerator);
            weightings = new List<Tuple<Char, Int64>>
            {
                new Tuple<Char, Int64>('a', 1), 
                new Tuple<Char, Int64>('b', 2), 
                new Tuple<Char, Int64>('c', 4)
            };
            using (mockery.Ordered)
            {
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(3).Will(Return.Value(1));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(2).Will(Return.Value(2));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(1).Will(Return.Value(0));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(7L).Will(Return.Value(0L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(7L).Will(Return.Value(1L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(7L).Will(Return.Value(2L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(7L).Will(Return.Value(3L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(7L).Will(Return.Value(4L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(7L).Will(Return.Value(5L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(7L).Will(Return.Value(6L));
            }
            testWeightedRandomGenerator.SetWeightings(weightings);

            result = testWeightedRandomGenerator.Generate();
            Assert.AreEqual('a', result);
            result = testWeightedRandomGenerator.Generate();
            Assert.AreEqual('b', result);
            result = testWeightedRandomGenerator.Generate();
            Assert.AreEqual('b', result);
            result = testWeightedRandomGenerator.Generate();
            Assert.AreEqual('c', result);
            result = testWeightedRandomGenerator.Generate();
            Assert.AreEqual('c', result);
            result = testWeightedRandomGenerator.Generate();
            Assert.AreEqual('c', result);
            result = testWeightedRandomGenerator.Generate();
            Assert.AreEqual('c', result);
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// Tests that an exception is thrown if the RemoveWeighting() method is called with an item for which a weighting doesn't exist.
        /// </summary>
        [Test]
        public void RemoveWeighting_WeightingForItemDoesNotExist()
        {
            List<Tuple<Char, Int64>> weightings = new List<Tuple<Char, Int64>>
            {
                new Tuple<Char, Int64>('a', 1), 
                new Tuple<Char, Int64>('b', 2)
            };

            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                testWeightedRandomGenerator.RemoveWeighting('c');
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("A weighting for the specified item does not exist."));
            Assert.AreEqual("item", e.ParamName);
        }

        /// <summary>
        /// Success tests for the RemoveWeighting() method.
        /// </summary>
        [Test]
        public void RemoveWeighting()
        {
            IRandomIntegerGenerator mockRandomIntegerGenerator = mockery.NewMock<IRandomIntegerGenerator>();
            testWeightedRandomGenerator = new WeightedRandomGenerator<Char>(mockRandomIntegerGenerator);

            // Add the test weightings
            // Initial weighting ranges can be visualised as follows...
            //   0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18
            //        | |     |   |          |              |
            //   a     b c     d   e          f
            using (mockery.Ordered)
            {
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(6).Will(Return.Value(3));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(5).Will(Return.Value(1));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(4).Will(Return.Value(3));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(3).Will(Return.Value(0));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(2).Will(Return.Value(0));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(1).Will(Return.Value(0));
            }
            testWeightedRandomGenerator.SetWeightings(new List<Tuple<Char, Int64>>()
            { 
                new Tuple<Char, Int64>('a', 3), 
                new Tuple<Char, Int64>('b', 1), 
                new Tuple<Char, Int64>('c', 3), 
                new Tuple<Char, Int64>('d', 2),
                new Tuple<Char, Int64>('e', 4),
                new Tuple<Char, Int64>('f', 5)
            });
            mockery.VerifyAllExpectationsHaveBeenMet();


            // Test shuffling up the values of all the ranges lower than the one removed
            // Weighting ranges after removing 'c' can be visualised as follows...
            //   3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18
            //        | |   |          |              |
            //   a     b d   e          f
            testWeightedRandomGenerator.RemoveWeighting('c');

            Dictionary<Int64, Int64> treeContents = GetAllWeightingRanges(testWeightedRandomGenerator);
            Assert.AreEqual(5, treeContents.Count);
            Assert.AreEqual(3, treeContents[3]);
            Assert.AreEqual(1, treeContents[6]);
            Assert.AreEqual(2, treeContents[7]);
            Assert.AreEqual(4, treeContents[9]);
            Assert.AreEqual(5, treeContents[13]);

            // Test that the Generate() method still works correctly
            mockery.ClearExpectation(mockRandomIntegerGenerator);
            using (mockery.Ordered)
            {
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(15L).Will(Return.Value(14L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(15L).Will(Return.Value(10L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(15L).Will(Return.Value(9L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(15L).Will(Return.Value(6L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(15L).Will(Return.Value(5L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(15L).Will(Return.Value(4L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(15L).Will(Return.Value(3L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(15L).Will(Return.Value(2L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(15L).Will(Return.Value(0L));
            }

            Char result = testWeightedRandomGenerator.Generate();
            Assert.AreEqual('f', result);
            result = testWeightedRandomGenerator.Generate();
            Assert.AreEqual('f', result);
            result = testWeightedRandomGenerator.Generate();
            Assert.AreEqual('e', result);
            result = testWeightedRandomGenerator.Generate();
            Assert.AreEqual('e', result);
            result = testWeightedRandomGenerator.Generate();
            Assert.AreEqual('d', result);
            result = testWeightedRandomGenerator.Generate();
            Assert.AreEqual('d', result);
            result = testWeightedRandomGenerator.Generate();
            Assert.AreEqual('b', result);
            result = testWeightedRandomGenerator.Generate();
            Assert.AreEqual('a', result);
            result = testWeightedRandomGenerator.Generate();
            Assert.AreEqual('a', result);
            mockery.VerifyAllExpectationsHaveBeenMet();

            
            // Test shuffling down the values of all the ranges higher than the one removed
            // Weighting ranges after removing 'd' can be visualised as follows...
            //   3 4 5 6 7 8 9 10 11 12 13 14 15 16
            //        | |        |              |
            //   a     b e        f
            testWeightedRandomGenerator.RemoveWeighting('d');

            treeContents = GetAllWeightingRanges(testWeightedRandomGenerator);
            Assert.AreEqual(4, treeContents.Count);
            Assert.AreEqual(3, treeContents[3]);
            Assert.AreEqual(1, treeContents[6]);
            Assert.AreEqual(4, treeContents[7]);
            Assert.AreEqual(5, treeContents[11]);

            // Test that the Generate() method still works correctly
            mockery.ClearExpectation(mockRandomIntegerGenerator);
            using (mockery.Ordered)
            {
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(13L).Will(Return.Value(12L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(13L).Will(Return.Value(8L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(13L).Will(Return.Value(7L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(13L).Will(Return.Value(4L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(13L).Will(Return.Value(3L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(13L).Will(Return.Value(2L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(13L).Will(Return.Value(0L));
            }

            result = testWeightedRandomGenerator.Generate();
            Assert.AreEqual('f', result);
            result = testWeightedRandomGenerator.Generate();
            Assert.AreEqual('f', result);
            result = testWeightedRandomGenerator.Generate();
            Assert.AreEqual('e', result);
            result = testWeightedRandomGenerator.Generate();
            Assert.AreEqual('e', result);
            result = testWeightedRandomGenerator.Generate();
            Assert.AreEqual('b', result);
            result = testWeightedRandomGenerator.Generate();
            Assert.AreEqual('a', result);
            result = testWeightedRandomGenerator.Generate();
            Assert.AreEqual('a', result);
            mockery.VerifyAllExpectationsHaveBeenMet();


            // Test removing the lowest range
            // Weighting ranges after removing 'a' can be visualised as follows...
            //   6 7 8 9 10 11 12 13 14 15 16
            //    |        |              |
            //   b e        f
            testWeightedRandomGenerator.RemoveWeighting('a');

            treeContents = GetAllWeightingRanges(testWeightedRandomGenerator);
            Assert.AreEqual(3, treeContents.Count);
            Assert.AreEqual(1, treeContents[6]);
            Assert.AreEqual(4, treeContents[7]);
            Assert.AreEqual(5, treeContents[11]);

            // Test that the Generate() method still works correctly
            mockery.ClearExpectation(mockRandomIntegerGenerator);
            using (mockery.Ordered)
            {
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(10L).Will(Return.Value(9L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(10L).Will(Return.Value(5L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(10L).Will(Return.Value(4L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(10L).Will(Return.Value(1L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(10L).Will(Return.Value(0L));
            }

            result = testWeightedRandomGenerator.Generate();
            Assert.AreEqual('f', result);
            result = testWeightedRandomGenerator.Generate();
            Assert.AreEqual('f', result);
            result = testWeightedRandomGenerator.Generate();
            Assert.AreEqual('e', result);
            result = testWeightedRandomGenerator.Generate();
            Assert.AreEqual('e', result);
            result = testWeightedRandomGenerator.Generate();
            Assert.AreEqual('b', result);
            mockery.VerifyAllExpectationsHaveBeenMet();


            // Test removing the highest range
            // Weighting ranges after removing 'f' can be visualised as follows...
            //   6 7 8 9 10 11 
            //    |        |  
            //   b e        
            testWeightedRandomGenerator.RemoveWeighting('f');

            treeContents = GetAllWeightingRanges(testWeightedRandomGenerator);
            Assert.AreEqual(2, treeContents.Count);
            Assert.AreEqual(1, treeContents[6]);
            Assert.AreEqual(4, treeContents[7]);

            // Test that the Generate() method still works correctly
            mockery.ClearExpectation(mockRandomIntegerGenerator);
            using (mockery.Ordered)
            {
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(5L).Will(Return.Value(4L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(5L).Will(Return.Value(1L));
                Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(5L).Will(Return.Value(0L));
            }

            result = testWeightedRandomGenerator.Generate();
            Assert.AreEqual('e', result);
            result = testWeightedRandomGenerator.Generate();
            Assert.AreEqual('e', result);
            result = testWeightedRandomGenerator.Generate();
            Assert.AreEqual('b', result);
            mockery.VerifyAllExpectationsHaveBeenMet();


            // Remove all remaining, and then set the weights again to ensure that field 'weightingStartOffset' is reset to 0
            testWeightedRandomGenerator.RemoveWeighting('b');
            testWeightedRandomGenerator.RemoveWeighting('e');
            mockery.ClearExpectation(mockRandomIntegerGenerator);
            Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(1).Will(Return.Value(0));
            testWeightedRandomGenerator.SetWeightings(new List<Tuple<Char, Int64>>()
            { 
                new Tuple<Char, Int64>('g', 5)
            });
            mockery.VerifyAllExpectationsHaveBeenMet();

            treeContents = GetAllWeightingRanges(testWeightedRandomGenerator);
            Assert.AreEqual(1, treeContents.Count);
            Assert.AreEqual(5, treeContents[0]);

            // Test that the Generate() method still works correctly
            mockery.ClearExpectation(mockRandomIntegerGenerator);
            Expect.Once.On(mockRandomIntegerGenerator).Method("Next").With(5L).Will(Return.Value(4L));

            result = testWeightedRandomGenerator.Generate();
            Assert.AreEqual('g', result);
        }

        /// <summary>
        /// Success tests for the WeightingCount property.
        /// </summary>
        [Test]
        public void WeightingCount()
        {
            Assert.AreEqual(0, testWeightedRandomGenerator.WeightingCount);


            testWeightedRandomGenerator.SetWeightings(new List<Tuple<Char, Int64>>()
            { 
                new Tuple<Char, Int64>('a', 3), 
                new Tuple<Char, Int64>('b', 1), 
                new Tuple<Char, Int64>('c', 3), 
                new Tuple<Char, Int64>('d', 2),
                new Tuple<Char, Int64>('e', 4),
                new Tuple<Char, Int64>('f', 5)
            });
            Assert.AreEqual(6, testWeightedRandomGenerator.WeightingCount);


            testWeightedRandomGenerator.RemoveWeighting('d');
            Assert.AreEqual(5, testWeightedRandomGenerator.WeightingCount);


            testWeightedRandomGenerator.Clear();
            Assert.AreEqual(0, testWeightedRandomGenerator.WeightingCount);
        }

        #region Private Methods

        private Dictionary<Int64, Int64> GetAllWeightingRanges(WeightedRandomGenerator<Char> inputRandomGenerator)
        {
            Dictionary<Int64, Int64> returnDictionary = new Dictionary<Int64, Int64>();
            Action<Int64, Int64> storeInDictionaryAction = (startValue, length) =>
            {
                returnDictionary.Add(startValue, length);
            };
            inputRandomGenerator.TraverseTree(storeInDictionaryAction);
            return returnDictionary;
        }

        #endregion
    }
}
