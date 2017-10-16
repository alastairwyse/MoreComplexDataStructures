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
    /// Unit tests for the FrequencyTable class.
    /// </summary>
    public class FrequencyTableTests
    {
        private FrequencyTable<Char> testFrequencyTable;

        [SetUp]
        protected void SetUp()
        {
            testFrequencyTable = new FrequencyTable<Char>();
        }

        /// <summary>
        /// Tests that an exception is thrown if the Decrement() method is called for an item whose frequency is 0.
        /// </summary>
        [Test]
        public void Decrement_ItemFrequencyIs0()
        {
            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                testFrequencyTable.Decrement('a');
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The frequency for the specified item 'a' is 0."));
            Assert.AreEqual("item", e.ParamName);
        }

        /// <summary>
        /// Tests that an exception is thrown if the IncrementBy() method is called with a 'count' parameter less than 1.
        /// </summary>
        [Test]
        public void IncrementBy_CountParamterLessThan1()
        {
            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                testFrequencyTable.IncrementBy('a', 0);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The value of the 'count' parameter must be greater than 0."));
            Assert.AreEqual("count", e.ParamName);
        }

        /// <summary>
        /// Tests that an exception is thrown if the DecrementBy() method is called with a 'count' parameter less than 1.Tests that an exception is thrown if the IncrementBy() method is called with a 'count' parameter less than 1.
        /// </summary>
        public void DecrementBy_CountParamterLessThan1()
        {
            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                testFrequencyTable.DecrementBy('a', 0);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The value of the 'count' parameter must be greater than 0."));
            Assert.AreEqual("count", e.ParamName);
        }

        /// <summary>
        /// Tests that an exception is thrown if the DecrementBy() method is called for an item whose frequency is 0.
        /// </summary>
        [Test]
        public void DecrementBy_ItemFrequencyIs0()
        {
            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                testFrequencyTable.DecrementBy('a', 2);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The frequency for the specified item 'a' is 0."));
            Assert.AreEqual("item", e.ParamName);
        }

        /// <summary>
        /// Tests that an exception is thrown if the DecrementBy() method is called with a 'count' parameter which is greater than the frequency of the item.
        /// </summary>
        [Test]
        public void DecrementBy_ItemFrequencyIsLessThanCount()
        {
            testFrequencyTable.Increment('a');

            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                testFrequencyTable.DecrementBy('a', 2);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The frequency for the specified item 'a' (1) is less than the value of the 'count' parameter."));
            Assert.AreEqual("count", e.ParamName);
        }

        /// <summary>
        /// Success tests for the Increment(), Decrement(), IncrementBy(), DecrementBy(), and GetFrequency() methods.
        /// </summary>
        [Test]
        public void Increment_Decrement_GetFrequency()
        {
            Assert.AreEqual(0, testFrequencyTable.GetFrequency('a'));

            testFrequencyTable.Increment('a');

            Assert.AreEqual(1, testFrequencyTable.GetFrequency('a'));

            testFrequencyTable.Increment('a');

            Assert.AreEqual(2, testFrequencyTable.GetFrequency('a'));

            testFrequencyTable.Decrement('a');

            Assert.AreEqual(1, testFrequencyTable.GetFrequency('a'));

            testFrequencyTable.Decrement('a');

            Assert.AreEqual(0, testFrequencyTable.GetFrequency('a'));

            testFrequencyTable.Increment('a');

            Assert.AreEqual(1, testFrequencyTable.GetFrequency('a'));


            Assert.AreEqual(0, testFrequencyTable.GetFrequency('b'));

            testFrequencyTable.IncrementBy('b', 2);

            Assert.AreEqual(2, testFrequencyTable.GetFrequency('b'));

            testFrequencyTable.IncrementBy('b', 3);

            Assert.AreEqual(5, testFrequencyTable.GetFrequency('b'));

            testFrequencyTable.DecrementBy('b', 2);

            Assert.AreEqual(3, testFrequencyTable.GetFrequency('b'));

            testFrequencyTable.DecrementBy('b', 3);

            Assert.AreEqual(0, testFrequencyTable.GetFrequency('b'));

            testFrequencyTable.IncrementBy('b', 2);

            Assert.AreEqual(2, testFrequencyTable.GetFrequency('b'));
        }

        /// <summary>
        /// Success tests for the 'ItemCount' property.
        /// </summary>
        [Test]
        public void ItemCount()
        {
            testFrequencyTable.Increment('a');
            testFrequencyTable.IncrementBy('b', 2);
            testFrequencyTable.Increment('c');
            testFrequencyTable.Increment('c');

            Assert.AreEqual(3, testFrequencyTable.ItemCount);

            testFrequencyTable.DecrementBy('c', 2);

            Assert.AreEqual(2, testFrequencyTable.ItemCount);
        }

        /// <summary>
        /// Success tests for the 'FrequencyCount' property.
        /// </summary>
        [Test]
        public void FrequencyCount()
        {
            testFrequencyTable.Increment('a');
            testFrequencyTable.IncrementBy('b', 2);
            testFrequencyTable.Increment('c');
            testFrequencyTable.Increment('c');

            Assert.AreEqual(5, testFrequencyTable.FrequencyCount);

            testFrequencyTable.DecrementBy('c', 2);

            Assert.AreEqual(3, testFrequencyTable.FrequencyCount);
        }

        /// <summary>
        /// Success tests for the GetEnumerator() method.
        /// </summary>
        [Test]
        public void GetEnumerator()
        {
            testFrequencyTable.Increment('a');
            testFrequencyTable.IncrementBy('b', 2);
            testFrequencyTable.Increment('c');
            testFrequencyTable.Increment('c');

            Dictionary<Char, Int32> enumerationResults = new Dictionary<Char, Int32>();
            foreach (KeyValuePair<Char, Int32> currentKeyValuePair in testFrequencyTable)
            {
                enumerationResults.Add(currentKeyValuePair.Key, currentKeyValuePair.Value);
            }

            Assert.AreEqual(3, enumerationResults.Keys.Count);
            Assert.AreEqual(1, enumerationResults['a']);
            Assert.AreEqual(2, enumerationResults['b']);
            Assert.AreEqual(2, enumerationResults['c']);
        }
    }
}
