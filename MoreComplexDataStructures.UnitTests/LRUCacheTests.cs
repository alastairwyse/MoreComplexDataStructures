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
using NMock2;
using MoreComplexDataStructures;

namespace MoreComplexDataStructures.UnitTests
{
    /// <summary>
    /// Unit tests for the LRUCache class.
    /// </summary>
    public class LRUCacheTests
    {
        private Mockery mockery;
        private IBackingStore mockBackingStore;
        private Func<Int32, String> fakeBackingStoreRequest;
        private LinkedList<Int32> expiryQueue;
        private LRUCache<Int32, String> testLRUCache;

        [SetUp]
        protected void SetUp()
        {
            mockery = new Mockery();
            mockBackingStore = mockery.NewMock<IBackingStore>();
            fakeBackingStoreRequest = (Int32 key) =>
            {
                return mockBackingStore.Retrieve(key);
            };
            testLRUCache = new LRUCache<Int32, String>(fakeBackingStoreRequest, 3, out expiryQueue);
        }

        /// <summary>
        /// Tests that an exception is thrown if the constructor is called with a 'itemLimit' parameter with value less than 1.
        /// </summary>
        [Test]
        public void Constructor_ItemLimitLessThan1()
        {
            ArgumentOutOfRangeException e = Assert.Throws<ArgumentOutOfRangeException>(delegate
            {
                testLRUCache = new LRUCache<Int32, String>(fakeBackingStoreRequest, 0);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("Parameter 'itemLimit' must be greater than or equal to 1."));
            Assert.AreEqual("itemLimit", e.ParamName);
        }

        /// <summary>
        /// Success tests for the Read() method.
        /// </summary>
        [Test]
        public void Read()
        {
            // Test case where item doesn't exist in cache
            Expect.Once.On(mockBackingStore).Method("Retrieve").With(1).Will(Return.Value("One"));
            Expect.Once.On(mockBackingStore).Method("Retrieve").With(2).Will(Return.Value("Two"));
            Expect.Once.On(mockBackingStore).Method("Retrieve").With(3).Will(Return.Value("Three"));

            String result = testLRUCache.Read(1);
            Assert.AreEqual("One", result);
            result = testLRUCache.Read(2);
            Assert.AreEqual("Two", result);
            result = testLRUCache.Read(3);

            Assert.AreEqual("Three", result);
            Assert.AreEqual(3, expiryQueue.ElementAt<Int32>(0));
            Assert.AreEqual(2, expiryQueue.ElementAt<Int32>(1));
            Assert.AreEqual(1, expiryQueue.ElementAt<Int32>(2));
            Assert.AreEqual(3, expiryQueue.Count);
            mockery.VerifyAllExpectationsHaveBeenMet();


            // Test case where item exists in the cache
            mockery.ClearExpectation(mockBackingStore);

            result = testLRUCache.Read(2);
            Assert.AreEqual("Two", result);
            Assert.AreEqual(2, expiryQueue.ElementAt<Int32>(0));
            Assert.AreEqual(3, expiryQueue.ElementAt<Int32>(1));
            Assert.AreEqual(1, expiryQueue.ElementAt<Int32>(2));
            Assert.AreEqual(3, expiryQueue.Count);

            result = testLRUCache.Read(1);
            Assert.AreEqual("One", result);
            Assert.AreEqual(1, expiryQueue.ElementAt<Int32>(0));
            Assert.AreEqual(2, expiryQueue.ElementAt<Int32>(1));
            Assert.AreEqual(3, expiryQueue.ElementAt<Int32>(2));
            Assert.AreEqual(3, expiryQueue.Count);

            result = testLRUCache.Read(1);
            Assert.AreEqual("One", result);
            Assert.AreEqual(1, expiryQueue.ElementAt<Int32>(0));
            Assert.AreEqual(2, expiryQueue.ElementAt<Int32>(1));
            Assert.AreEqual(3, expiryQueue.ElementAt<Int32>(2));
            Assert.AreEqual(3, expiryQueue.Count);

            // Test expiring an item from the cache
            Expect.Once.On(mockBackingStore).Method("Retrieve").With(4).Will(Return.Value("Four"));

            result = testLRUCache.Read(4);
            Assert.AreEqual("Four", result);
            Assert.AreEqual(4, expiryQueue.ElementAt<Int32>(0));
            Assert.AreEqual(1, expiryQueue.ElementAt<Int32>(1));
            Assert.AreEqual(2, expiryQueue.ElementAt<Int32>(2));
            Assert.AreEqual(3, expiryQueue.Count);
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// Success tests for the Read() method where the routine to check whether the cache is full is overridden in the constructor.
        /// </summary>
        [Test]
        public void Read_CacheFullCheckRoutineOverridden()
        {
            Boolean fullIndicator = false;
            Func<Dictionary<Int32, Tuple<String, LinkedListNode<Int32>>>, Boolean> cacheFullCheckRoutine = (cachestore) =>
            {
                return fullIndicator;
            };
            testLRUCache = new LRUCache<Int32, String>(fakeBackingStoreRequest, cacheFullCheckRoutine, out expiryQueue);

            // Cache is not marked as full, so should hold all requested items
            Expect.Once.On(mockBackingStore).Method("Retrieve").With(1).Will(Return.Value("One"));
            Expect.Once.On(mockBackingStore).Method("Retrieve").With(2).Will(Return.Value("Two"));
            Expect.Once.On(mockBackingStore).Method("Retrieve").With(3).Will(Return.Value("Three"));

            testLRUCache.Read(1);
            testLRUCache.Read(2);
            testLRUCache.Read(3);

            Assert.AreEqual(3, expiryQueue.ElementAt<Int32>(0));
            Assert.AreEqual(2, expiryQueue.ElementAt<Int32>(1));
            Assert.AreEqual(1, expiryQueue.ElementAt<Int32>(2));
            Assert.AreEqual(3, expiryQueue.Count);
            mockery.VerifyAllExpectationsHaveBeenMet();

            // Mark cache as full, and expect items to be expired
            fullIndicator = true;
            mockery.ClearExpectation(mockBackingStore);
            Expect.Once.On(mockBackingStore).Method("Retrieve").With(4).Will(Return.Value("Four"));

            testLRUCache.Read(4);

            Assert.AreEqual(4, expiryQueue.ElementAt<Int32>(0));
            Assert.AreEqual(3, expiryQueue.ElementAt<Int32>(1));
            Assert.AreEqual(2, expiryQueue.ElementAt<Int32>(2));
            Assert.AreEqual(3, expiryQueue.Count);
            mockery.VerifyAllExpectationsHaveBeenMet();

            // Set back to not full and expect items again to be stored
            fullIndicator = false;
            mockery.ClearExpectation(mockBackingStore);
            Expect.Once.On(mockBackingStore).Method("Retrieve").With(5).Will(Return.Value("Five"));

            String result = testLRUCache.Read(5);

            Assert.AreEqual("Five", result);
            Assert.AreEqual(5, expiryQueue.ElementAt<Int32>(0));
            Assert.AreEqual(4, expiryQueue.ElementAt<Int32>(1));
            Assert.AreEqual(3, expiryQueue.ElementAt<Int32>(2));
            Assert.AreEqual(2, expiryQueue.ElementAt<Int32>(3));
            Assert.AreEqual(4, expiryQueue.Count);
            mockery.VerifyAllExpectationsHaveBeenMet();


            mockery.ClearExpectation(mockBackingStore);
            Expect.Never.On(mockBackingStore);

            result = testLRUCache.Read(2);

            Assert.AreEqual("Two", result);
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// Success tests for the ContainsKey() method.
        /// </summary>
        [Test]
        public void ContainsKey()
        {
            Expect.Once.On(mockBackingStore).Method("Retrieve").With(1).Will(Return.Value("One"));
            Expect.Once.On(mockBackingStore).Method("Retrieve").With(2).Will(Return.Value("Two"));
            Expect.Once.On(mockBackingStore).Method("Retrieve").With(3).Will(Return.Value("Three"));

            testLRUCache.Read(1);
            testLRUCache.Read(2);;
            testLRUCache.Read(3);

            Assert.IsTrue(testLRUCache.ContainsKey(1));
            Assert.IsTrue(testLRUCache.ContainsKey(2));
            Assert.IsTrue(testLRUCache.ContainsKey(3));
            Assert.IsFalse(testLRUCache.ContainsKey(0));
            Assert.IsFalse(testLRUCache.ContainsKey(4));
        }

        /// <summary>
        /// Tests that an exception is thrown if the Expire() method called with a 'key' parameter where an item with that key doesn't exist.
        /// </summary>
        [Test]
        public void Expire_ItemDoesntExist()
        {
            Expect.Once.On(mockBackingStore).Method("Retrieve").With(1).Will(Return.Value("One"));
            Expect.Once.On(mockBackingStore).Method("Retrieve").With(2).Will(Return.Value("Two"));
            Expect.Once.On(mockBackingStore).Method("Retrieve").With(3).Will(Return.Value("Three"));

            testLRUCache.Read(1);
            testLRUCache.Read(2);
            testLRUCache.Read(3);

            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                testLRUCache.Expire(4);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("An item with the specified key '4' does not exist in the cache."));
            Assert.AreEqual("key", e.ParamName);
        }

        /// <summary>
        /// Success tests for the Expire() method.
        /// </summary>
        [Test]
        public void Expire()
        {
            Expect.Once.On(mockBackingStore).Method("Retrieve").With(1).Will(Return.Value("One"));
            Expect.Once.On(mockBackingStore).Method("Retrieve").With(2).Will(Return.Value("Two"));
            Expect.Once.On(mockBackingStore).Method("Retrieve").With(3).Will(Return.Value("Three"));

            testLRUCache.Read(1);
            testLRUCache.Read(2);
            testLRUCache.Read(3);
            Assert.AreEqual(3, expiryQueue.ElementAt<Int32>(0));
            Assert.AreEqual(2, expiryQueue.ElementAt<Int32>(1));
            Assert.AreEqual(1, expiryQueue.ElementAt<Int32>(2));
            Assert.AreEqual(3, expiryQueue.Count);

            testLRUCache.Expire(2);
            Assert.AreEqual(3, expiryQueue.ElementAt<Int32>(0));
            Assert.AreEqual(1, expiryQueue.ElementAt<Int32>(1));
            Assert.AreEqual(2, expiryQueue.Count);

            testLRUCache.Expire(1);
            Assert.AreEqual(3, expiryQueue.ElementAt<Int32>(0));
            Assert.AreEqual(1, expiryQueue.Count);

            testLRUCache.Expire(3);
            Assert.AreEqual(0, expiryQueue.Count);
        }

        /// <summary>
        /// Success tests for the Count property.
        /// </summary>
        [Test]
        public void Count()
        {
            Expect.Once.On(mockBackingStore).Method("Retrieve").With(1).Will(Return.Value("One"));
            Expect.Once.On(mockBackingStore).Method("Retrieve").With(2).Will(Return.Value("Two"));
            Expect.Once.On(mockBackingStore).Method("Retrieve").With(3).Will(Return.Value("Three"));
            Expect.Once.On(mockBackingStore).Method("Retrieve").With(4).Will(Return.Value("Four"));

            Assert.AreEqual(0, testLRUCache.Count);

            testLRUCache.Read(1);
            Assert.AreEqual(1, testLRUCache.Count);

            testLRUCache.Read(2);
            Assert.AreEqual(2, testLRUCache.Count);

            testLRUCache.Read(3);
            Assert.AreEqual(3, testLRUCache.Count);

            testLRUCache.Read(4);
            Assert.AreEqual(3, testLRUCache.Count);
        }

        #region Nested Classes

        /// <summary>
        /// A fake backing store used to test the cache.
        /// </summary>
        public interface IBackingStore
        {
            String Retrieve(Int32 key);
        }

        #endregion
    }
}
