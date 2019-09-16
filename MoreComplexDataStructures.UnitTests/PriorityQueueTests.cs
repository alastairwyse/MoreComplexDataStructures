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
    /// Unit tests for the PriorityQueue class.
    /// </summary>
    /// <remarks>Deriving from PriorityQueue so that nested class PriorityAndItems can be accessed.</remarks>
    public class PriorityQueueTests : PriorityQueue<Char>
    {
        private PriorityQueueWithProtectedMethods<Char> testPriorityQueue;

        [SetUp]
        protected void SetUp()
        {
            testPriorityQueue = new PriorityQueueWithProtectedMethods<Char>();
        }

        /// <summary>
        /// Tests that an exception is thrown when the Enqueue() method is called with an NaN priority.
        /// </summary>
        [Test]
        public void Enqueue_NaNPriority()
        {
            var e = Assert.Throws<ArgumentException>(delegate
            {
                testPriorityQueue.Enqueue('A', Double.NaN);
            });

            Assert.That(e.Message, Does.StartWith("Parameter 'priority' cannot be 'NaN'."));
            Assert.AreEqual("priority", e.ParamName);
        }

        /// <summary>
        /// Success tests for the Enqueue() method which test the internal structure of the class.
        /// </summary>
        [Test]
        public void Enqueue_InternalStructureTests()
        {
            EnqueueTestData(testPriorityQueue);

            Assert.AreEqual(4, testPriorityQueue.ItemToPriorityMap.Count);
            Assert.IsTrue(testPriorityQueue.ItemToPriorityMap.ContainsKey('A'));
            Assert.IsTrue(testPriorityQueue.ItemToPriorityMap.ContainsKey('B'));
            Assert.IsTrue(testPriorityQueue.ItemToPriorityMap.ContainsKey('C'));
            Assert.IsTrue(testPriorityQueue.ItemToPriorityMap.ContainsKey('D'));
            Assert.AreEqual(1, testPriorityQueue.ItemToPriorityMap['A'].Count);
            Assert.AreEqual(1, testPriorityQueue.ItemToPriorityMap['B'].Count);
            Assert.AreEqual(3, testPriorityQueue.ItemToPriorityMap['C'].Count);
            Assert.AreEqual(1, testPriorityQueue.ItemToPriorityMap['D'].Count);
            Assert.IsTrue(testPriorityQueue.ItemToPriorityMap['A'].Contains(5.0));
            Assert.IsTrue(testPriorityQueue.ItemToPriorityMap['B'].Contains(4.0));
            Assert.IsTrue(testPriorityQueue.ItemToPriorityMap['C'].Contains(3.0));
            Assert.IsTrue(testPriorityQueue.ItemToPriorityMap['C'].Contains(6.0));
            Assert.IsTrue(testPriorityQueue.ItemToPriorityMap['C'].Contains(7.0));
            Assert.IsTrue(testPriorityQueue.ItemToPriorityMap['D'].Contains(4.0));
            Assert.AreEqual(1, testPriorityQueue.Tree.Get(new PriorityAndItems<Char>(3.0)).Items.Count);
            Assert.AreEqual(2, testPriorityQueue.Tree.Get(new PriorityAndItems<Char>(4.0)).Items.Count);
            Assert.AreEqual(1, testPriorityQueue.Tree.Get(new PriorityAndItems<Char>(5.0)).Items.Count);
            Assert.AreEqual(1, testPriorityQueue.Tree.Get(new PriorityAndItems<Char>(6.0)).Items.Count);
            Assert.AreEqual(1, testPriorityQueue.Tree.Get(new PriorityAndItems<Char>(7.0)).Items.Count);
            Assert.IsTrue(testPriorityQueue.Tree.Get(new PriorityAndItems<Char>(3.0)).Items.ContainsKey('C'));
            Assert.IsTrue(testPriorityQueue.Tree.Get(new PriorityAndItems<Char>(4.0)).Items.ContainsKey('B'));
            Assert.IsTrue(testPriorityQueue.Tree.Get(new PriorityAndItems<Char>(4.0)).Items.ContainsKey('D'));
            Assert.IsTrue(testPriorityQueue.Tree.Get(new PriorityAndItems<Char>(5.0)).Items.ContainsKey('A'));
            Assert.IsTrue(testPriorityQueue.Tree.Get(new PriorityAndItems<Char>(6.0)).Items.ContainsKey('C'));
            Assert.IsTrue(testPriorityQueue.Tree.Get(new PriorityAndItems<Char>(7.0)).Items.ContainsKey('C'));
            Assert.AreEqual(2, testPriorityQueue.Tree.Get(new PriorityAndItems<Char>(3.0)).Items['C']);
            Assert.AreEqual(1, testPriorityQueue.Tree.Get(new PriorityAndItems<Char>(4.0)).Items['B']);
            Assert.AreEqual(2, testPriorityQueue.Tree.Get(new PriorityAndItems<Char>(4.0)).Items['D']);
            Assert.AreEqual(1, testPriorityQueue.Tree.Get(new PriorityAndItems<Char>(5.0)).Items['A']);
            Assert.AreEqual(1, testPriorityQueue.Tree.Get(new PriorityAndItems<Char>(6.0)).Items['C']);
            Assert.AreEqual(1, testPriorityQueue.Tree.Get(new PriorityAndItems<Char>(7.0)).Items['C']);
        }

        /// <summary>
        /// Success tests for the Dequeue() method which test the internal structure of the class.
        /// </summary>
        [Test]
        public void Dequeue_InternalStructureTests()
        {
            EnqueueTestData(testPriorityQueue);

            testPriorityQueue.Dequeue('D', 4.0);
            Assert.IsTrue(testPriorityQueue.ItemToPriorityMap.ContainsKey('D'));
            Assert.AreEqual(1, testPriorityQueue.ItemToPriorityMap['D'].Count);
            Assert.IsTrue(testPriorityQueue.ItemToPriorityMap['D'].Contains(4.0));
            Assert.IsTrue(testPriorityQueue.Tree.Get(new PriorityAndItems<Char>(4.0)).Items.ContainsKey('D'));
            Assert.AreEqual(2, testPriorityQueue.Tree.Get(new PriorityAndItems<Char>(4.0)).Items.Count);
            Assert.AreEqual(1, testPriorityQueue.Tree.Get(new PriorityAndItems<Char>(4.0)).Items['D']);

            testPriorityQueue.Dequeue('C', 3.0);
            Assert.IsTrue(testPriorityQueue.ItemToPriorityMap.ContainsKey('C'));
            Assert.AreEqual(3, testPriorityQueue.ItemToPriorityMap['C'].Count);
            Assert.IsTrue(testPriorityQueue.ItemToPriorityMap['C'].Contains(3.0));
            Assert.IsTrue(testPriorityQueue.Tree.Get(new PriorityAndItems<Char>(3.0)).Items.ContainsKey('C'));
            Assert.AreEqual(1, testPriorityQueue.Tree.Get(new PriorityAndItems<Char>(3.0)).Items.Count);
            Assert.AreEqual(1, testPriorityQueue.Tree.Get(new PriorityAndItems<Char>(3.0)).Items['C']);

            testPriorityQueue.Dequeue('D', 4.0);
            Assert.IsFalse(testPriorityQueue.ItemToPriorityMap.ContainsKey('D'));
            Assert.IsFalse(testPriorityQueue.Tree.Get(new PriorityAndItems<Char>(4.0)).Items.ContainsKey('D'));
            Assert.AreEqual(1, testPriorityQueue.Tree.Get(new PriorityAndItems<Char>(4.0)).Items.Count);

            testPriorityQueue.Dequeue('C', 7.0);
            Assert.IsTrue(testPriorityQueue.ItemToPriorityMap.ContainsKey('C'));
            Assert.AreEqual(2, testPriorityQueue.ItemToPriorityMap['C'].Count);
            Assert.IsFalse(testPriorityQueue.ItemToPriorityMap['C'].Contains(7.0));
            Assert.IsFalse(testPriorityQueue.Tree.Contains(new PriorityAndItems<Char>(7.0)));

            testPriorityQueue.Dequeue('A');
            Assert.IsFalse(testPriorityQueue.ItemToPriorityMap.ContainsKey('A'));
            Assert.IsFalse(testPriorityQueue.Tree.Contains(new PriorityAndItems<Char>(5.0)));

            testPriorityQueue.Dequeue('C', 3.0);
            Assert.IsTrue(testPriorityQueue.ItemToPriorityMap.ContainsKey('C'));
            Assert.AreEqual(1, testPriorityQueue.ItemToPriorityMap['C'].Count);
            Assert.IsFalse(testPriorityQueue.ItemToPriorityMap['C'].Contains(3.0));
            Assert.IsFalse(testPriorityQueue.Tree.Contains(new PriorityAndItems<Char>(3.0)));

            testPriorityQueue.Dequeue('C', 6.0);
            Assert.IsFalse(testPriorityQueue.ItemToPriorityMap.ContainsKey('C'));
            Assert.IsFalse(testPriorityQueue.Tree.Contains(new PriorityAndItems<Char>(6.0)));

            testPriorityQueue.Dequeue('B', 4.0);
            Assert.IsFalse(testPriorityQueue.ItemToPriorityMap.ContainsKey('B'));
            Assert.IsFalse(testPriorityQueue.Tree.Contains(new PriorityAndItems<Char>(4.0)));
            Assert.AreEqual(0, testPriorityQueue.ItemToPriorityMap.Count);
            Assert.AreEqual(0, testPriorityQueue.Tree.Count);
        }

        /// <summary>
        /// Tests that an exception is thrown if the Dequeue() method is called with an item which doesn't exist in the queue.
        /// </summary>
        [Test]
        public void Dequeue_ItemDoesntExist()
        {
            testPriorityQueue.Enqueue('A', 1.0);
            testPriorityQueue.Enqueue('B', 1.0);
            testPriorityQueue.Enqueue('C', 1.0);
            testPriorityQueue.Enqueue('A', 4.0);

            var e = Assert.Throws<ArgumentException>(delegate
            {
                testPriorityQueue.Dequeue('D');
            });

            Assert.That(e.Message, Does.StartWith("Item 'D' does not exist in the priority queue."));
            Assert.AreEqual("item", e.ParamName);
        }

        /// <summary>
        /// Tests that an exception is thrown if the Dequeue() method is called with an item for which multiple instances exist with different priorities.
        /// </summary>
        [Test]
        public void Dequeue_MultipleInstancesOfItemExist()
        {
            testPriorityQueue.Enqueue('A', 1.0);
            testPriorityQueue.Enqueue('B', 1.0);
            testPriorityQueue.Enqueue('C', 1.0);
            testPriorityQueue.Enqueue('A', 4.0);

            var e = Assert.Throws<InvalidOperationException>(delegate
            {
                testPriorityQueue.Dequeue('A');
            });

            Assert.That(e.Message, Does.StartWith("Multiple instances of item 'A' exist in the queue."));
        }

        /// <summary>
        /// Success tests for the Enqueue(), Dequeue(), GetItemCountByItem(), and GetItemCountByPriority() methods and the 'Count' property.
        /// </summary>
        [Test]
        public void Enqueue_Dequeue_GetItemCountByItem_GetItemCountByPriority_Count()
        {
            Assert.AreEqual(0, testPriorityQueue.GetItemCountByItem('A'));
            Assert.AreEqual(0, testPriorityQueue.GetItemCountByPriority(5.0));
            Assert.AreEqual(0, testPriorityQueue.Count);

            testPriorityQueue.Enqueue('A', 5.0);
            Assert.AreEqual(1, testPriorityQueue.GetItemCountByItem('A'));
            Assert.AreEqual(1, testPriorityQueue.GetItemCountByPriority(5.0));
            Assert.AreEqual(1, testPriorityQueue.Count);

            testPriorityQueue.Enqueue('B', 4.0);
            Assert.AreEqual(1, testPriorityQueue.GetItemCountByItem('B'));
            Assert.AreEqual(1, testPriorityQueue.GetItemCountByPriority(4.0));
            Assert.AreEqual(2, testPriorityQueue.Count);

            testPriorityQueue.Enqueue('C', 6.0);
            Assert.AreEqual(1, testPriorityQueue.GetItemCountByItem('C'));
            Assert.AreEqual(1, testPriorityQueue.GetItemCountByPriority(6.0));
            Assert.AreEqual(3, testPriorityQueue.Count);

            testPriorityQueue.Enqueue('C', 3.0);
            Assert.AreEqual(2, testPriorityQueue.GetItemCountByItem('C'));
            Assert.AreEqual(1, testPriorityQueue.GetItemCountByPriority(3.0));
            Assert.AreEqual(4, testPriorityQueue.Count);

            testPriorityQueue.Enqueue('C', 7.0);
            Assert.AreEqual(3, testPriorityQueue.GetItemCountByItem('C'));
            Assert.AreEqual(1, testPriorityQueue.GetItemCountByPriority(7.0));
            Assert.AreEqual(5, testPriorityQueue.Count);

            testPriorityQueue.Enqueue('D', 4.0);
            Assert.AreEqual(1, testPriorityQueue.GetItemCountByItem('D'));
            Assert.AreEqual(2, testPriorityQueue.GetItemCountByPriority(4.0));
            Assert.AreEqual(6, testPriorityQueue.Count);

            testPriorityQueue.Enqueue('C', 3.0);
            Assert.AreEqual(4, testPriorityQueue.GetItemCountByItem('C'));
            Assert.AreEqual(2, testPriorityQueue.GetItemCountByPriority(3.0));
            Assert.AreEqual(7, testPriorityQueue.Count);

            testPriorityQueue.Enqueue('D', 4.0);
            Assert.AreEqual(2, testPriorityQueue.GetItemCountByItem('D'));
            Assert.AreEqual(3, testPriorityQueue.GetItemCountByPriority(4.0));
            Assert.AreEqual(8, testPriorityQueue.Count);

            testPriorityQueue.Dequeue('D', 4.0);
            Assert.AreEqual(1, testPriorityQueue.GetItemCountByItem('D'));
            Assert.AreEqual(2, testPriorityQueue.GetItemCountByPriority(4.0));
            Assert.AreEqual(7, testPriorityQueue.Count);

            testPriorityQueue.Dequeue('C', 3.0);
            Assert.AreEqual(3, testPriorityQueue.GetItemCountByItem('C'));
            Assert.AreEqual(1, testPriorityQueue.GetItemCountByPriority(3.0));
            Assert.AreEqual(6, testPriorityQueue.Count);

            testPriorityQueue.Dequeue('D', 4.0);
            Assert.AreEqual(0, testPriorityQueue.GetItemCountByItem('D'));
            Assert.AreEqual(1, testPriorityQueue.GetItemCountByPriority(4.0));
            Assert.AreEqual(5, testPriorityQueue.Count);

            testPriorityQueue.Dequeue('C', 7.0);
            Assert.AreEqual(2, testPriorityQueue.GetItemCountByItem('C'));
            Assert.AreEqual(0, testPriorityQueue.GetItemCountByPriority(7.0));
            Assert.AreEqual(4, testPriorityQueue.Count);

            testPriorityQueue.Dequeue('C', 3.0);
            Assert.AreEqual(1, testPriorityQueue.GetItemCountByItem('C'));
            Assert.AreEqual(0, testPriorityQueue.GetItemCountByPriority(3.0));
            Assert.AreEqual(3, testPriorityQueue.Count);

            testPriorityQueue.Dequeue('C');
            Assert.AreEqual(0, testPriorityQueue.GetItemCountByItem('C'));
            Assert.AreEqual(0, testPriorityQueue.GetItemCountByPriority(6.0));
            Assert.AreEqual(2, testPriorityQueue.Count);

            testPriorityQueue.Dequeue('B');
            Assert.AreEqual(0, testPriorityQueue.GetItemCountByItem('B'));
            Assert.AreEqual(0, testPriorityQueue.GetItemCountByPriority(4.0));
            Assert.AreEqual(1, testPriorityQueue.Count);

            testPriorityQueue.Dequeue('A');
            Assert.AreEqual(0, testPriorityQueue.GetItemCountByItem('A'));
            Assert.AreEqual(0, testPriorityQueue.GetItemCountByPriority(5.0));
            Assert.AreEqual(0, testPriorityQueue.Count);
        }

        /// <summary>
        /// Success tests for the GetItemCountByItem() method.
        /// </summary>
        [Test]
        public void GetItemCountByItem()
        {
            Assert.AreEqual(0, testPriorityQueue.GetItemCountByItem('E'));

            EnqueueTestData(testPriorityQueue);

            Assert.AreEqual(1, testPriorityQueue.GetItemCountByItem('A'));
            Assert.AreEqual(1, testPriorityQueue.GetItemCountByItem('B'));
            Assert.AreEqual(4, testPriorityQueue.GetItemCountByItem('C'));
            Assert.AreEqual(2, testPriorityQueue.GetItemCountByItem('D'));
            Assert.AreEqual(0, testPriorityQueue.GetItemCountByItem('E'));
        }

        /// <summary>
        /// Success tests for the GetItemCountByPriority() method.
        /// </summary>
        [Test]
        public void GetItemCountByPriority()
        {
            Assert.AreEqual(0, testPriorityQueue.GetItemCountByPriority(8.0));

            EnqueueTestData(testPriorityQueue);

            Assert.AreEqual(2, testPriorityQueue.GetItemCountByPriority(3.0));
            Assert.AreEqual(3, testPriorityQueue.GetItemCountByPriority(4.0));
            Assert.AreEqual(1, testPriorityQueue.GetItemCountByPriority(5.0));
            Assert.AreEqual(1, testPriorityQueue.GetItemCountByPriority(6.0));
            Assert.AreEqual(1, testPriorityQueue.GetItemCountByPriority(7.0));
            Assert.AreEqual(0, testPriorityQueue.GetItemCountByPriority(8.0));
        }

        /// <summary>
        /// Success tests for the Clear() method.
        /// </summary>
        [Test]
        public new void Clear()
        {
            EnqueueTestData(testPriorityQueue);
            testPriorityQueue.Clear();

            Assert.AreEqual(0, testPriorityQueue.Count);
            Assert.AreEqual(0, testPriorityQueue.ItemToPriorityMap.Count);
            Assert.AreEqual(0, testPriorityQueue.Tree.Count);
        }

        /// <summary>
        /// Tests that items can EnqueueAsMax() method continues to enqueue items with priority Double.PositiveInfinity when the maximum priority is already Double.PositiveInfinity.
        /// </summary>
        [Test]
        public void EnqueueAsMax_MaximumPriorityAlreadyPositiveInfinity()
        {
            testPriorityQueue.Enqueue('A', Double.PositiveInfinity);

            testPriorityQueue.EnqueueAsMax('B');

            Assert.AreEqual(2, testPriorityQueue.Count);
            var priorities = new List<Double>(testPriorityQueue.GetPrioritiesForItem('B'));
            Assert.AreEqual(1, priorities.Count);
            Assert.AreEqual(Double.PositiveInfinity, priorities[0]);
        }

        /// <summary>
        /// Success tests for the EnqueueAsMax() method.
        /// </summary>
        [Test]
        public void EnqueueAsMax()
        {
            testPriorityQueue.Enqueue('A', 10.0);
            testPriorityQueue.Enqueue('B', 11.0);
            testPriorityQueue.Enqueue('C', 9.0);
            testPriorityQueue.Enqueue('D', 12.0);
            testPriorityQueue.Enqueue('E', 8.0);

            testPriorityQueue.EnqueueAsMax('F');
            
            Assert.AreEqual(6, testPriorityQueue.Count);
            Assert.AreEqual('F', testPriorityQueue.PeekMax());
            var priorities = new List<Double>(testPriorityQueue.GetPrioritiesForItem('F'));
            Assert.AreEqual(1, priorities.Count);
            Assert.IsTrue(12.0 < priorities[0]);
        }

        /// <summary>
        /// Success tests for the EnqueueAsMax() method, where the existing maximum priority is a very large double value.
        /// </summary>
        [Test]
        public void EnqueueAsMax_LargePriorityValue()
        {
            Double largePriorityValue = BitConverter.Int64BitsToDouble(0x7FEFFFFFFFFFFFF0);
            testPriorityQueue.Enqueue('A', largePriorityValue);

            testPriorityQueue.EnqueueAsMax('B');

            Assert.AreEqual(2, testPriorityQueue.Count);
            Assert.AreEqual(1, testPriorityQueue.GetItemCountByPriority(largePriorityValue));
            Assert.AreEqual('B', testPriorityQueue.GetItemsWithPriorityGreaterThan(largePriorityValue, 1).First().Value);
            Assert.AreEqual(1, testPriorityQueue.GetItemsWithPriorityGreaterThan(largePriorityValue, 1).Count());
            Assert.IsTrue(largePriorityValue < testPriorityQueue.MaxPriority);
        }

        /// <summary>
        /// Tests that items can EnqueueAsMin() method continues to enqueue items with priority Double.NegativeInfinity when the minimum priority is already Double.NegativeInfinity.
        /// </summary>
        [Test]
        public void EnqueueAsMin_MinimumPriorityAlreadyNegativeInfinity()
        {
            testPriorityQueue.Enqueue('A', Double.NegativeInfinity);

            testPriorityQueue.EnqueueAsMin('B');

            Assert.AreEqual(2, testPriorityQueue.Count);
            var priorities = new List<Double>(testPriorityQueue.GetPrioritiesForItem('B'));
            Assert.AreEqual(1, priorities.Count);
            Assert.AreEqual(Double.NegativeInfinity, priorities[0]);
        }

        /// <summary>
        /// Success tests for the EnqueueAsMin() method.
        /// </summary>
        [Test]
        public void EnqueueAsMin()
        {
            testPriorityQueue.Enqueue('A', 10.0);
            testPriorityQueue.Enqueue('B', 11.0);
            testPriorityQueue.Enqueue('C', 9.0);
            testPriorityQueue.Enqueue('D', 12.0);
            testPriorityQueue.Enqueue('E', 8.0);

            testPriorityQueue.EnqueueAsMin('F');

            Assert.AreEqual(6, testPriorityQueue.Count);
            Assert.AreEqual('F', testPriorityQueue.PeekMin());
            var priorities = new List<Double>(testPriorityQueue.GetPrioritiesForItem('F'));
            Assert.AreEqual(1, priorities.Count);
            Assert.IsTrue(8.0 > priorities[0]);
        }

        /// <summary>
        /// Success tests for the EnqueueAsMin() method, where the existing minimum priority is a very small double value.
        /// </summary>
        [Test]
        public void EnqueueAsMin_SmallPriorityValue()
        {
            Double smallPriorityValue = BitConverter.Int64BitsToDouble(unchecked((long)0xFFEFFFFFFFFFFFF0));
            testPriorityQueue.Enqueue('A', smallPriorityValue);

            testPriorityQueue.EnqueueAsMin('B');

            Assert.AreEqual(2, testPriorityQueue.Count);
            Assert.AreEqual(1, testPriorityQueue.GetItemCountByPriority(smallPriorityValue));
            Assert.AreEqual('B', testPriorityQueue.GetItemsWithPriorityLessThan(smallPriorityValue, 1).First().Value);
            Assert.AreEqual(1, testPriorityQueue.GetItemsWithPriorityLessThan(smallPriorityValue, 1).Count());
            Assert.IsTrue(smallPriorityValue > testPriorityQueue.MinPriority);
        }

        /// <summary>
        /// Tests that an exception is thrown if the PeekMax() method is called when the queue is empty.
        /// </summary>
        [Test]
        public void PeekMax_QueueEmpty()
        {
            var e = Assert.Throws<InvalidOperationException>(delegate
            {
                testPriorityQueue.PeekMax();
            });

            Assert.That(e.Message, Does.StartWith("The priority queue is empty."));
        }

        /// <summary>
        /// Success tests for the PeekMax() method.
        /// </summary>
        [Test]
        public new void PeekMax()
        {
            testPriorityQueue.Enqueue('A', 5.0);
            testPriorityQueue.Enqueue('B', 4.0);
            testPriorityQueue.Enqueue('C', 6.0);
            testPriorityQueue.Enqueue('C', 3.0);
            testPriorityQueue.Enqueue('E', 7.0);
            testPriorityQueue.Enqueue('D', 4.0);
            testPriorityQueue.Enqueue('C', 3.0);
            testPriorityQueue.Enqueue('D', 4.0);
            testPriorityQueue.Enqueue('E', 7.0);
            Assert.AreEqual(9, testPriorityQueue.Count);

            Char result = testPriorityQueue.PeekMax();

            Assert.AreEqual('E', result);
            Assert.AreEqual(2, testPriorityQueue.GetItemCountByPriority(7.0));
            Assert.AreEqual(9, testPriorityQueue.Count);
        }

        /// <summary>
        /// Tests that an exception is thrown if the PeekMin() method is called when the queue is empty.
        /// </summary>
        [Test]
        public void PeekMin_QueueEmpty()
        {
            var e = Assert.Throws<InvalidOperationException>(delegate
            {
                testPriorityQueue.PeekMin();
            });

            Assert.That(e.Message, Does.StartWith("The priority queue is empty."));
        }

        /// <summary>
        /// Success tests for the PeekMin() method.
        /// </summary>
        [Test]
        public new void PeekMin()
        {
            testPriorityQueue.Enqueue('A', 5.0);
            testPriorityQueue.Enqueue('B', 4.0);
            testPriorityQueue.Enqueue('C', 6.0);
            testPriorityQueue.Enqueue('C', 3.0);
            testPriorityQueue.Enqueue('E', 7.0);
            testPriorityQueue.Enqueue('D', 4.0);
            testPriorityQueue.Enqueue('C', 3.0);
            testPriorityQueue.Enqueue('D', 4.0);
            Assert.AreEqual(8, testPriorityQueue.Count);

            Char result = testPriorityQueue.PeekMin();

            Assert.AreEqual('C', result);
            Assert.AreEqual(2, testPriorityQueue.GetItemCountByPriority(3.0));
            Assert.AreEqual(8, testPriorityQueue.Count);
        }

        /// <summary>
        /// Success tests for the ContainsItem() method.
        /// </summary>
        [Test]
        public void ContainsItem()
        {
            Assert.IsFalse(testPriorityQueue.ContainsItem('A'));

            EnqueueTestData(testPriorityQueue);

            Assert.IsTrue(testPriorityQueue.ContainsItem('A'));
            Assert.IsTrue(testPriorityQueue.ContainsItem('B'));
            Assert.IsTrue(testPriorityQueue.ContainsItem('C'));
            Assert.IsTrue(testPriorityQueue.ContainsItem('D'));
            Assert.IsFalse(testPriorityQueue.ContainsItem('E'));
        }

        /// <summary>
        /// Success tests for the ContainsPriority() method.
        /// </summary>
        [Test]
        public void ContainsPriority()
        {
            Assert.IsFalse(testPriorityQueue.ContainsPriority(3.0));

            EnqueueTestData(testPriorityQueue);

            Assert.IsTrue(testPriorityQueue.ContainsPriority(3.0));
            Assert.IsTrue(testPriorityQueue.ContainsPriority(4.0));
            Assert.IsTrue(testPriorityQueue.ContainsPriority(5.0));
            Assert.IsTrue(testPriorityQueue.ContainsPriority(6.0));
            Assert.IsTrue(testPriorityQueue.ContainsPriority(7.0));
            Assert.IsFalse(testPriorityQueue.ContainsPriority(8.0));
        }

        /// <summary>
        /// Tests that an exception is thrown if the GetPrioritiesForItem() method is called for an item which doesn't exist.
        /// </summary>
        [Test]
        public void GetPrioritiesForItem_ItemDoesntExist()
        {
            EnqueueTestData(testPriorityQueue);

            var e = Assert.Throws<ArgumentException>(delegate
            {
                testPriorityQueue.GetPrioritiesForItem('E');
            });

            Assert.That(e.Message, Does.StartWith("Item 'E' does not exist in the priority queue."));
            Assert.AreEqual("item", e.ParamName);
        }

        /// <summary>
        /// Success tests for the GetPrioritiesForItem() method.
        /// </summary>
        [Test]
        public void GetPrioritiesForItem()
        {
            EnqueueTestData(testPriorityQueue);

            var priorities = new HashSet<Double>(testPriorityQueue.GetPrioritiesForItem('A'));
            Assert.AreEqual(1, priorities.Count);
            Assert.IsTrue(priorities.Contains(5.0));
            priorities = new HashSet<Double>(testPriorityQueue.GetPrioritiesForItem('B'));
            Assert.AreEqual(1, priorities.Count);
            Assert.IsTrue(priorities.Contains(4.0));
            priorities = new HashSet<Double>(testPriorityQueue.GetPrioritiesForItem('C'));
            Assert.AreEqual(3, priorities.Count);
            Assert.IsTrue(priorities.Contains(3.0));
            Assert.IsTrue(priorities.Contains(6.0));
            Assert.IsTrue(priorities.Contains(7.0));
            priorities = new HashSet<Double>(testPriorityQueue.GetPrioritiesForItem('D'));
            Assert.AreEqual(1, priorities.Count);
            Assert.IsTrue(priorities.Contains(4.0));
        }

        /// <summary>
        /// Tests that an exception is thrown if the DequeueMax() method is called when the queue is empty.
        /// </summary>
        [Test]
        public void DequeueMaxQueueEmpty()
        {
            var e = Assert.Throws<InvalidOperationException>(delegate
            {
                testPriorityQueue.DequeueMax();
            });

            Assert.That(e.Message, Does.StartWith("The priority queue is empty."));
        }

        /// <summary>
        /// Success tests for the DequeueMax() method.
        /// </summary>
        [Test]
        public new void DequeueMax()
        {
            testPriorityQueue.Enqueue('A', 5.0);
            testPriorityQueue.Enqueue('B', 4.0);
            testPriorityQueue.Enqueue('C', 6.0);
            testPriorityQueue.Enqueue('C', 3.0);
            testPriorityQueue.Enqueue('E', 7.0);
            testPriorityQueue.Enqueue('D', 4.0);
            testPriorityQueue.Enqueue('C', 3.0);
            testPriorityQueue.Enqueue('D', 4.0);
            testPriorityQueue.Enqueue('E', 7.0);
            Assert.AreEqual(9, testPriorityQueue.Count);

            Char result = testPriorityQueue.DequeueMax();

            Assert.AreEqual('E', result);
            Assert.AreEqual(1, testPriorityQueue.GetItemCountByPriority(7.0));
            Assert.AreEqual(8, testPriorityQueue.Count);
        }

        /// <summary>
        /// Tests that an exception is thrown if the DequeueMin() method is called when the queue is empty.
        /// </summary>
        [Test]
        public void DequeueMinQueueEmpty()
        {
            var e = Assert.Throws<InvalidOperationException>(delegate
            {
                testPriorityQueue.DequeueMin();
            });

            Assert.That(e.Message, Does.StartWith("The priority queue is empty."));
        }

        /// <summary>
        /// Success tests for the DequeueMin() method.
        /// </summary>
        [Test]
        public new void DequeueMin()
        {
            testPriorityQueue.Enqueue('A', 5.0);
            testPriorityQueue.Enqueue('B', 4.0);
            testPriorityQueue.Enqueue('C', 6.0);
            testPriorityQueue.Enqueue('C', 3.0);
            testPriorityQueue.Enqueue('E', 7.0);
            testPriorityQueue.Enqueue('D', 4.0);
            testPriorityQueue.Enqueue('C', 3.0);
            testPriorityQueue.Enqueue('D', 4.0);
            Assert.AreEqual(8, testPriorityQueue.Count);

            Char result = testPriorityQueue.DequeueMin();

            Assert.AreEqual('C', result);
            Assert.AreEqual(1, testPriorityQueue.GetItemCountByPriority(3.0));
            Assert.AreEqual(7, testPriorityQueue.Count);
        }

        /// <summary>
        /// Success tests for the GetAllWithPriorityGreaterThan() method.
        /// </summary>
        [Test]
        public void GetAllWithPriorityGreaterThan()
        {
            EnqueueTestData(testPriorityQueue);

            var result = new List<KeyValuePair<Double, Char>>(testPriorityQueue.GetAllWithPriorityGreaterThan(2.0));

            Assert.AreEqual(8, result.Count);
            Assert.AreEqual(3.0, result[0].Key);
            Assert.AreEqual('C', result[0].Value);
            Assert.AreEqual(3.0, result[1].Key);
            Assert.AreEqual('C', result[1].Value);
            Assert.AreEqual(4.0, result[2].Key);
            Assert.AreEqual(4.0, result[3].Key);
            Assert.AreEqual(4.0, result[4].Key);
            // Order of values with priority 4.0 is not deterministic, so put all into a FrequencyTable to check that they exist irrespective of order
            var equalPriorityCharcterCounts = new FrequencyTable<Char>();
            equalPriorityCharcterCounts.Increment(result[2].Value);
            equalPriorityCharcterCounts.Increment(result[3].Value);
            equalPriorityCharcterCounts.Increment(result[4].Value);
            Assert.AreEqual(1, equalPriorityCharcterCounts.GetFrequency('B'));
            Assert.AreEqual(2, equalPriorityCharcterCounts.GetFrequency('D'));
            Assert.AreEqual(5.0, result[5].Key);
            Assert.AreEqual('A', result[5].Value);
            Assert.AreEqual(6.0, result[6].Key);
            Assert.AreEqual('C', result[6].Value);
            Assert.AreEqual(7.0, result[7].Key);
            Assert.AreEqual('C', result[7].Value);


            result = new List<KeyValuePair<Double, Char>>(testPriorityQueue.GetAllWithPriorityGreaterThan(6.5));

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(7.0, result[0].Key);
            Assert.AreEqual('C', result[0].Value);


            result = new List<KeyValuePair<Double, Char>>(testPriorityQueue.GetAllWithPriorityGreaterThan(7.0));

            Assert.AreEqual(0, result.Count);
            

            testPriorityQueue.Clear();

            result = new List<KeyValuePair<Double, Char>>(testPriorityQueue.GetAllWithPriorityLessThan(3.0));

            Assert.AreEqual(0, result.Count);
        }

        /// <summary>
        /// Success tests for the GetAllWithPriorityLessThan() method.
        /// </summary>
        [Test]
        public void GetAllWithPriorityLessThan()
        {
            EnqueueTestData(testPriorityQueue);

            var result = new List<KeyValuePair<Double, Char>>(testPriorityQueue.GetAllWithPriorityLessThan(8.0));

            Assert.AreEqual(8, result.Count);
            Assert.AreEqual(7.0, result[0].Key);
            Assert.AreEqual('C', result[0].Value);
            Assert.AreEqual(6.0, result[1].Key);
            Assert.AreEqual('C', result[1].Value);
            Assert.AreEqual(5.0, result[2].Key);
            Assert.AreEqual('A', result[2].Value);
            Assert.AreEqual(4.0, result[3].Key);
            Assert.AreEqual(4.0, result[4].Key);
            Assert.AreEqual(4.0, result[5].Key);
            // Order of values with priority 4.0 is not deterministic, so put all into a FrequencyTable to check that they exist irrespective of order
            var equalPriorityCharcterCounts = new FrequencyTable<Char>();
            equalPriorityCharcterCounts.Increment(result[3].Value);
            equalPriorityCharcterCounts.Increment(result[4].Value);
            equalPriorityCharcterCounts.Increment(result[5].Value);
            Assert.AreEqual(1, equalPriorityCharcterCounts.GetFrequency('B'));
            Assert.AreEqual(2, equalPriorityCharcterCounts.GetFrequency('D'));
            Assert.AreEqual(3.0, result[6].Key);
            Assert.AreEqual('C', result[6].Value);
            Assert.AreEqual(3.0, result[7].Key);
            Assert.AreEqual('C', result[7].Value);
            

            result = new List<KeyValuePair<Double, Char>>(testPriorityQueue.GetAllWithPriorityLessThan(3.5));

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(3.0, result[0].Key);
            Assert.AreEqual('C', result[0].Value);
            Assert.AreEqual(3.0, result[1].Key);
            Assert.AreEqual('C', result[1].Value);


            result = new List<KeyValuePair<Double, Char>>(testPriorityQueue.GetAllWithPriorityLessThan(3.0));

            Assert.AreEqual(0, result.Count);


            testPriorityQueue.Clear();

            result = new List<KeyValuePair<Double, Char>>(testPriorityQueue.GetAllWithPriorityLessThan(3.0));

            Assert.AreEqual(0, result.Count);
        }

        /// <summary>
        /// Tests that an exception is thrown if the GetItemsWithPriorityGreaterThan() method is called with an 'itemCount' parameter less than 1.
        /// </summary>
        [Test]
        public void GetItemsWithPriorityGreaterThan_ItemCountLessThan1()
        {
            var e = Assert.Throws<ArgumentOutOfRangeException>(delegate
            {
                testPriorityQueue.GetItemsWithPriorityGreaterThan(3.0, 0).Count();
            });

            Assert.That(e.Message, Does.StartWith("Parameter 'itemCount' must be greater than or equal to 1."));
            Assert.AreEqual(e.ParamName, "itemCount");
        }

        /// <summary>
        /// Success tests for the GetItemsWithPriorityGreaterThan() method.
        /// </summary>
        [Test]
        public void GetItemsWithPriorityGreaterThan()
        {
            EnqueueTestData(testPriorityQueue);

            var result = new List<KeyValuePair<Double, Char>>(testPriorityQueue.GetItemsWithPriorityGreaterThan(3.0, 4));

            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(4.0, result[0].Key);
            Assert.AreEqual(4.0, result[1].Key);
            Assert.AreEqual(4.0, result[2].Key);
            // Order of values with priority 4.0 is not deterministic, so put all into a FrequencyTable to check that they exist irrespective of order
            var equalPriorityCharcterCounts = new FrequencyTable<Char>();
            equalPriorityCharcterCounts.Increment(result[0].Value);
            equalPriorityCharcterCounts.Increment(result[1].Value);
            equalPriorityCharcterCounts.Increment(result[2].Value);
            Assert.AreEqual(1, equalPriorityCharcterCounts.GetFrequency('B'));
            Assert.AreEqual(2, equalPriorityCharcterCounts.GetFrequency('D'));
            Assert.AreEqual(5.0, result[3].Key);
            Assert.AreEqual('A', result[3].Value);

            result = new List<KeyValuePair<Double, Char>>(testPriorityQueue.GetItemsWithPriorityGreaterThan(2.0, 1));

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(3.0, result[0].Key);
            Assert.AreEqual('C', result[0].Value);
        }

        /// <summary>
        /// Tests that an exception is thrown if the GetItemsWithPriorityLessThan() method is called with an 'itemCount' parameter less than 1.
        /// </summary>
        [Test]
        public void GetItemsWithPriorityLessThan_ItemCountLessThan1()
        {
            var e = Assert.Throws<ArgumentOutOfRangeException>(delegate
            {
                testPriorityQueue.GetItemsWithPriorityLessThan(3.0, 0).Count();
            });

            Assert.That(e.Message, Does.StartWith("Parameter 'itemCount' must be greater than or equal to 1."));
            Assert.AreEqual(e.ParamName, "itemCount");
        }

        /// <summary>
        /// Success tests for the GetItemsWithPriorityLessThan() method.
        /// </summary>
        [Test]
        public void GetItemsWithPriorityLessThan()
        {
            EnqueueTestData(testPriorityQueue);

            var result = new List<KeyValuePair<Double, Char>>(testPriorityQueue.GetItemsWithPriorityLessThan(6.0, 4));

            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(5.0, result[0].Key);
            Assert.AreEqual('A', result[0].Value);
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(4.0, result[1].Key);
            Assert.AreEqual(4.0, result[2].Key);
            Assert.AreEqual(4.0, result[3].Key);
            // Order of values with priority 4.0 is not deterministic, so put all into a FrequencyTable to check that they exist irrespective of order
            var equalPriorityCharcterCounts = new FrequencyTable<Char>();
            equalPriorityCharcterCounts.Increment(result[1].Value);
            equalPriorityCharcterCounts.Increment(result[2].Value);
            equalPriorityCharcterCounts.Increment(result[3].Value);
            Assert.AreEqual(1, equalPriorityCharcterCounts.GetFrequency('B'));
            Assert.AreEqual(2, equalPriorityCharcterCounts.GetFrequency('D'));


            result = new List<KeyValuePair<Double, Char>>(testPriorityQueue.GetItemsWithPriorityLessThan(4.0, 1));

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(3.0, result[0].Key);
            Assert.AreEqual('C', result[0].Value);
        }

        /// <summary>
        /// Success tests for the constructor overload which pre-populates the priority queue with a collection of items and corresponding priorities.
        /// </summary>
        [Test]
        public void Constructor_InitialiseWithItems()
        {
            var testData = new List<KeyValuePair<Double, Char>>
            {
                new KeyValuePair<Double, Char>(10.0, 'A'),
                new KeyValuePair<Double, Char>(9.0, 'B'),
                new KeyValuePair<Double, Char>(8.0, 'B'),
                new KeyValuePair<Double, Char>(8.0, 'B'),
                new KeyValuePair<Double, Char>(7.0, 'C'),
            };

            testPriorityQueue = new PriorityQueueWithProtectedMethods<Char>(testData);

            var result = new List<KeyValuePair<Double, Char>>(testPriorityQueue.GetAllWithPriorityGreaterThan(6.0));
            Assert.AreEqual(5, result.Count);
            Assert.AreEqual(7.0, result[0].Key);
            Assert.AreEqual('C', result[0].Value);
            Assert.AreEqual(8.0, result[1].Key);
            Assert.AreEqual('B', result[1].Value);
            Assert.AreEqual(8.0, result[2].Key);
            Assert.AreEqual('B', result[2].Value);
            Assert.AreEqual(9.0, result[3].Key);
            Assert.AreEqual('B', result[3].Value);
            Assert.AreEqual(10.0, result[4].Key);
            Assert.AreEqual('A', result[4].Value);
        }

        /// <summary>
        /// Tests that an exception is thrown if the MaxPriority property is accessed when the queue is empty.
        /// </summary>
        [Test]
        public void MaxPriority_QueueEmpty()
        {
            var e = Assert.Throws<InvalidOperationException>(delegate
            {
                Double result = testPriorityQueue.MaxPriority;
            });

            Assert.That(e.Message, Does.StartWith("The priority queue is empty."));
        }

        /// <summary>
        /// Success tests for the MaxPriority property.
        /// </summary>
        [Test]
        public new void MaxPriority()
        {
            EnqueueTestData(testPriorityQueue);

            Double result = testPriorityQueue.MaxPriority;

            Assert.AreEqual(7.0, result);
        }

        /// <summary>
        /// Tests that an exception is thrown if the MinPriority property is accessed when the queue is empty.
        /// </summary>
        [Test]
        public void MinPriority_QueueEmpty()
        {
            var e = Assert.Throws<InvalidOperationException>(delegate
            {
                Double result = testPriorityQueue.MinPriority;
            });

            Assert.That(e.Message, Does.StartWith("The priority queue is empty."));
        }

        /// <summary>
        /// Success tests for the MinPriority property.
        /// </summary>
        [Test]
        public new void MinPriority()
        {
            EnqueueTestData(testPriorityQueue);

            Double result = testPriorityQueue.MinPriority;

            Assert.AreEqual(3.0, result);
        }

        #region Private Methods

        private void EnqueueTestData(PriorityQueue<Char> queue) 
        {
            queue.Enqueue('A', 5.0);
            queue.Enqueue('B', 4.0);
            queue.Enqueue('C', 6.0);
            queue.Enqueue('C', 3.0);
            queue.Enqueue('C', 7.0);
            queue.Enqueue('D', 4.0);
            queue.Enqueue('C', 3.0);
            queue.Enqueue('D', 4.0);
        }

        #endregion

        #region Nested Classes

        /// <summary>
        /// Version of the PriorityQueue class where private and protected methods and members are exposed as public so that they can be unit tested.
        /// </summary>
        /// <typeparam name="T">Specifies the type of items held in the queue.</typeparam>
        private class PriorityQueueWithProtectedMethods<T> : PriorityQueue<T> where T : IEquatable<T>
        {
            /// <summary>
            /// Initialises a new instance of the MoreComplexDataStructures.UnitTests.PriorityQueueTests+PriorityQueueWithProtectedMethods class.
            /// </summary>
            public PriorityQueueWithProtectedMethods()
                : base()
            {
            }

            /// <summary>
            /// Initialises a new instance of the MoreComplexDataStructures.UnitTests.PriorityQueueTests+PriorityQueueWithProtectedMethods class.
            /// </summary>
            /// <param name="initialPrioritiesAndItems">The collection of items and corresponding priorities which are enqueued in the new PriorityQueue.</param>
            public PriorityQueueWithProtectedMethods(IEnumerable<KeyValuePair<Double, T>> initialPrioritiesAndItems)
                : base(initialPrioritiesAndItems)
            {
            }

            /// <summary>
            /// Tree holding all of the priorities and items in the priority queue.
            /// </summary>
            public WeightBalancedTreeWithProtectedMethods<PriorityAndItems<T>> Tree
            {
                get { return tree; }
            }

            /// <summary>
            /// Dictionary mapping the items in the priority queue to their corresponding priorities.
            /// </summary>
            public Dictionary<T, HashSet<Double>> ItemToPriorityMap
            {
                get { return itemToPriorityMap; }
            }
        }

        #endregion
    }
}
