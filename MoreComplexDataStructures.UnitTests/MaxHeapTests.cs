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
    /// Unit tests for the MaxHeap class.
    /// </summary>
    public class MaxHeapTests : HeapTestBase
    {
        private MaxHeap<Int32> testMaxHeap;

        [SetUp]
        protected void SetUp()
        {
            testMaxHeap = new MaxHeap<Int32>();
        }

        /// <summary>
        /// Success tests for the Insert() method.
        /// </summary>
        [Test]
        public void Insert()
        {
            // Test when the heap is empty
            List<Int32> allItems = GetListOfItems(testMaxHeap);

            Assert.AreEqual(0, allItems.Count);


            // Test with a single item
            testMaxHeap.Insert(1);

            Dictionary<Int32, DoublyLinkedTreeNode<Int32>> allNodes = GetAllNodes(testMaxHeap);
            Assert.AreEqual(1, allNodes.Count);
            Assert.IsNull(allNodes[1].ParentNode);


            // Test with an additional layer of depth
            // Resulting tree should be...
            //     3
            //    / \
            //   1   2
            testMaxHeap.Insert(2);
            testMaxHeap.Insert(3);
            allNodes = GetAllNodes(testMaxHeap);
            Assert.AreEqual(3, allNodes.Count);
            Assert.IsNull(allNodes[1].LeftChildNode);
            Assert.IsNull(allNodes[1].RightChildNode);
            Assert.AreEqual(allNodes[3], allNodes[1].ParentNode);
            Assert.IsNull(allNodes[2].LeftChildNode);
            Assert.IsNull(allNodes[2].RightChildNode);
            Assert.AreEqual(allNodes[3], allNodes[2].ParentNode);
            Assert.AreEqual(allNodes[1], allNodes[3].LeftChildNode);
            Assert.AreEqual(allNodes[2], allNodes[3].RightChildNode);
            Assert.IsNull(allNodes[3].ParentNode);


            // Add one more item, to test moving to a new depth
            // Resulting tree should be...
            //       4
            //      / \
            //     3   2
            //    /
            //   1
            testMaxHeap.Insert(4);
            allNodes = GetAllNodes(testMaxHeap);
            Assert.AreEqual(4, allNodes.Count);
            Assert.IsNull(allNodes[1].LeftChildNode);
            Assert.IsNull(allNodes[1].RightChildNode);
            Assert.AreEqual(allNodes[3], allNodes[1].ParentNode);
            Assert.IsNull(allNodes[2].LeftChildNode);
            Assert.IsNull(allNodes[2].RightChildNode);
            Assert.AreEqual(allNodes[4], allNodes[2].ParentNode);
            Assert.AreEqual(allNodes[1], allNodes[3].LeftChildNode);
            Assert.IsNull(allNodes[3].RightChildNode);
            Assert.AreEqual(allNodes[4], allNodes[3].ParentNode);
            Assert.AreEqual(allNodes[3], allNodes[4].LeftChildNode);
            Assert.AreEqual(allNodes[2], allNodes[4].RightChildNode);
            Assert.IsNull(allNodes[4].ParentNode);


            // Test adding a duplicate value
            // Resulting tree should be...
            //       4
            //      / \
            //     4   2
            //    / \
            //   1   3
            testMaxHeap.Insert(4);
            allItems = GetListOfItems(testMaxHeap);
            Assert.AreEqual(4, allItems[0]);
            Assert.AreEqual(4, allItems[1]);
            Assert.AreEqual(2, allItems[2]);
            Assert.AreEqual(1, allItems[3]);
            Assert.AreEqual(3, allItems[4]);


            // Test where no child/parent swaps are required during insert
            // Resulting tree should be...
            //     3
            //    / \
            //   2   1
            testMaxHeap = new MaxHeap<Int32>();
            testMaxHeap.Insert(3);
            testMaxHeap.Insert(2);
            testMaxHeap.Insert(1);
            allNodes = GetAllNodes(testMaxHeap);
            Assert.AreEqual(3, allNodes.Count);
            Assert.IsNull(allNodes[2].LeftChildNode);
            Assert.IsNull(allNodes[2].RightChildNode);
            Assert.AreEqual(allNodes[3], allNodes[2].ParentNode);
            Assert.IsNull(allNodes[1].LeftChildNode);
            Assert.IsNull(allNodes[1].RightChildNode);
            Assert.AreEqual(allNodes[3], allNodes[1].ParentNode);
            Assert.AreEqual(allNodes[2], allNodes[3].LeftChildNode);
            Assert.AreEqual(allNodes[1], allNodes[3].RightChildNode);
            Assert.IsNull(allNodes[3].ParentNode);
        }

        /// <summary>
        /// Tests that an exception is thrown if the ExtractMax() method is called when the heap is empty.
        /// </summary>
        [Test]
        public void ExtractMax_HeapIsEmpty()
        {
            Exception e = Assert.Throws<Exception>(delegate
            {
                testMaxHeap.ExtractMax();
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The heap is empty."));
        }

        /// <summary>
        /// Success tests for the ExtractMax() method.
        /// </summary>
        [Test]
        public void ExtractMax()
        {
            // Test with a single item
            testMaxHeap.Insert(1);
            Int32 result = testMaxHeap.ExtractMax();
            Dictionary<Int32, DoublyLinkedTreeNode<Int32>> allNodes = GetAllNodes(testMaxHeap);
            Assert.AreEqual(1, result);
            Assert.AreEqual(0, allNodes.Count);


            // Test with the following tree
            //       4
            //      / \
            //     3   2
            //    /
            //   1
            testMaxHeap.Insert(1);
            testMaxHeap.Insert(2);
            testMaxHeap.Insert(3);
            testMaxHeap.Insert(4);

            result = testMaxHeap.ExtractMax();
            // Structure should be...
            //     3
            //    / \
            //   1   2
            allNodes = GetAllNodes(testMaxHeap);
            Assert.AreEqual(4, result);
            Assert.AreEqual(3, allNodes.Count);
            Assert.AreEqual(allNodes[1], allNodes[3].LeftChildNode);
            Assert.AreEqual(allNodes[2], allNodes[3].RightChildNode);
            Assert.IsNull(allNodes[3].ParentNode);
            Assert.IsNull(allNodes[1].LeftChildNode);
            Assert.IsNull(allNodes[1].RightChildNode);
            Assert.AreEqual(allNodes[3], allNodes[1].ParentNode);
            Assert.IsNull(allNodes[2].LeftChildNode);
            Assert.IsNull(allNodes[2].RightChildNode);
            Assert.AreEqual(allNodes[3], allNodes[2].ParentNode);


            result = testMaxHeap.ExtractMax();
            // Structure should be...
            //     2
            //    / 
            //   1  
            allNodes = GetAllNodes(testMaxHeap);
            Assert.AreEqual(3, result);
            Assert.AreEqual(2, allNodes.Count);
            Assert.AreEqual(allNodes[1], allNodes[2].LeftChildNode);
            Assert.IsNull(allNodes[2].RightChildNode);
            Assert.IsNull(allNodes[2].ParentNode);
            Assert.IsNull(allNodes[1].LeftChildNode);
            Assert.IsNull(allNodes[1].RightChildNode);
            Assert.AreEqual(allNodes[2], allNodes[1].ParentNode);


            result = testMaxHeap.ExtractMax();
            // Structure should be...
            //     1
            allNodes = GetAllNodes(testMaxHeap);
            Assert.AreEqual(2, result);
            Assert.AreEqual(1, allNodes.Count);
            Assert.IsNull(allNodes[1].ParentNode);
            Assert.IsNull(allNodes[1].LeftChildNode);
            Assert.IsNull(allNodes[1].RightChildNode);


            result = testMaxHeap.ExtractMax();
            allNodes = GetAllNodes(testMaxHeap);
            Assert.AreEqual(1, result);
            Assert.AreEqual(0, allNodes.Count);


            // Test with the following tree
            //     3
            //    / \
            //   2   1
            testMaxHeap.Insert(3);
            testMaxHeap.Insert(2);
            testMaxHeap.Insert(1);

            result = testMaxHeap.ExtractMax();
            allNodes = GetAllNodes(testMaxHeap);
            Assert.AreEqual(3, result);
            Assert.AreEqual(2, allNodes.Count);
            Assert.AreEqual(allNodes[1], allNodes[2].LeftChildNode);
            Assert.IsNull(allNodes[2].RightChildNode);
            Assert.IsNull(allNodes[2].ParentNode);
            Assert.IsNull(allNodes[1].LeftChildNode);
            Assert.IsNull(allNodes[1].RightChildNode);
            Assert.AreEqual(allNodes[2], allNodes[1].ParentNode);


            // Test with the following tree
            //       5
            //      / \
            //     3   4
            //    / \
            //   1   2
            testMaxHeap = new MaxHeap<Int32>();
            testMaxHeap.Insert(5);
            testMaxHeap.Insert(3);
            testMaxHeap.Insert(4);
            testMaxHeap.Insert(1);
            testMaxHeap.Insert(2);

            result = testMaxHeap.ExtractMax();
            allNodes = GetAllNodes(testMaxHeap);
            Assert.AreEqual(5, result);
            Assert.AreEqual(4, allNodes.Count);


            // Test with the following tree
            //        9
            //      /   \
            //     8     7
            //    / \   /
            //   2   1 3 
            testMaxHeap = new MaxHeap<Int32>();
            testMaxHeap.Insert(9);
            testMaxHeap.Insert(8);
            testMaxHeap.Insert(7);
            testMaxHeap.Insert(2);
            testMaxHeap.Insert(1);
            testMaxHeap.Insert(3);

            result = testMaxHeap.ExtractMax();
            allNodes = GetAllNodes(testMaxHeap);
            Assert.AreEqual(9, result);
            Assert.AreEqual(5, allNodes.Count);
            Assert.AreEqual(allNodes[3], allNodes[8].LeftChildNode);
            Assert.AreEqual(allNodes[7], allNodes[8].RightChildNode);
            Assert.IsNull(allNodes[8].ParentNode);
            Assert.AreEqual(allNodes[2], allNodes[3].LeftChildNode);
            Assert.AreEqual(allNodes[1], allNodes[3].RightChildNode);
            Assert.AreEqual(allNodes[8], allNodes[3].ParentNode);
            Assert.IsNull(allNodes[7].LeftChildNode);
            Assert.IsNull(allNodes[7].RightChildNode);
            Assert.AreEqual(allNodes[8], allNodes[7].ParentNode);
            Assert.IsNull(allNodes[2].LeftChildNode);
            Assert.IsNull(allNodes[2].RightChildNode);
            Assert.AreEqual(allNodes[3], allNodes[2].ParentNode);
            Assert.IsNull(allNodes[1].LeftChildNode);
            Assert.IsNull(allNodes[1].RightChildNode);
            Assert.AreEqual(allNodes[3], allNodes[1].ParentNode);


            // Test with the following tree
            //     5
            //    / \
            //   5   5
            testMaxHeap = new MaxHeap<Int32>();
            testMaxHeap.Insert(5);
            testMaxHeap.Insert(5);
            testMaxHeap.Insert(5);

            result = testMaxHeap.ExtractMax();
            Assert.AreEqual(5, result);
            result = testMaxHeap.ExtractMax();
            Assert.AreEqual(5, result);
            result = testMaxHeap.ExtractMax();
            Assert.AreEqual(5, result);
            allNodes = GetAllNodes(testMaxHeap);
            Assert.AreEqual(0, allNodes.Count);
        }

        /// <summary>
        /// Success tests for the Count property.
        /// </summary>
        [Test]
        public void Count()
        {
            Assert.AreEqual(0, testMaxHeap.Count);

            testMaxHeap.Insert(9);
            Assert.AreEqual(1, testMaxHeap.Count);
            testMaxHeap.Insert(8);
            Assert.AreEqual(2, testMaxHeap.Count);
            testMaxHeap.Insert(7);
            Assert.AreEqual(3, testMaxHeap.Count);
            testMaxHeap.Insert(2);
            Assert.AreEqual(4, testMaxHeap.Count);
            testMaxHeap.Insert(1);
            Assert.AreEqual(5, testMaxHeap.Count);
            testMaxHeap.Insert(3);
            Assert.AreEqual(6, testMaxHeap.Count);

            testMaxHeap.ExtractMax();
            Assert.AreEqual(5, testMaxHeap.Count);
            testMaxHeap.ExtractMax();
            Assert.AreEqual(4, testMaxHeap.Count);
            testMaxHeap.ExtractMax();
            Assert.AreEqual(3, testMaxHeap.Count);
            testMaxHeap.ExtractMax();
            Assert.AreEqual(2, testMaxHeap.Count);
            testMaxHeap.ExtractMax();
            Assert.AreEqual(1, testMaxHeap.Count);
            testMaxHeap.ExtractMax();
            Assert.AreEqual(0, testMaxHeap.Count);

            testMaxHeap.Insert(9);
            Assert.AreEqual(1, testMaxHeap.Count);
        }

        /// <summary>
        /// Success tests for the Clear() method.
        /// </summary>
        [Test]
        public void Clear()
        {
            testMaxHeap.Insert(9);
            testMaxHeap.Insert(8);
            testMaxHeap.Insert(7);
            testMaxHeap.Insert(2);

            testMaxHeap.Clear();
            Dictionary<Int32, DoublyLinkedTreeNode<Int32>> allNodes = GetAllNodes(testMaxHeap);
            Assert.AreEqual(0, allNodes.Count);

            testMaxHeap.Insert(1);
            allNodes = GetAllNodes(testMaxHeap);
            Assert.AreEqual(1, allNodes.Count);
            Assert.IsNull(allNodes[1].ParentNode);
            Assert.IsNull(allNodes[1].LeftChildNode);
            Assert.IsNull(allNodes[1].RightChildNode);
        }

        /// <summary>
        /// Success tests for the BreadthFirstSearch() method.
        /// </summary>
        [Test]
        public void BreadthFirstSearch()
        {
            // Test with the following tree
            //        9
            //      /   \
            //     8     7
            //    / \   /
            //   2   1 3 
            testMaxHeap.Insert(9);
            testMaxHeap.Insert(8);
            testMaxHeap.Insert(7);
            testMaxHeap.Insert(2);
            testMaxHeap.Insert(1);
            testMaxHeap.Insert(3);

            List<Int32> breadthFirstItems = GetListOfItems(testMaxHeap);
            Assert.AreEqual(9, breadthFirstItems[0]);
            Assert.AreEqual(8, breadthFirstItems[1]);
            Assert.AreEqual(7, breadthFirstItems[2]);
            Assert.AreEqual(2, breadthFirstItems[3]);
            Assert.AreEqual(1, breadthFirstItems[4]);
            Assert.AreEqual(3, breadthFirstItems[5]);
        }

        /// <summary>
        /// Tests that an exception is thrown if the Peek() method is called when the heap is empty.
        /// </summary>
        [Test]
        public void Peek_HeapIsEmpty()
        {
            Exception e = Assert.Throws<Exception>(delegate
            {
                testMaxHeap.Peek();
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The heap is empty."));
        }

        /// <summary>
        /// Success tests for the Peek() method.
        /// </summary>
        [Test]
        public void Peek()
        {
            testMaxHeap.Insert(1);
            testMaxHeap.Insert(2);
            testMaxHeap.Insert(3);
            testMaxHeap.Insert(4);
            testMaxHeap.Insert(5);
            testMaxHeap.Insert(7);

            Int32 result = testMaxHeap.Peek();
            Dictionary<Int32, DoublyLinkedTreeNode<Int32>> allNodes = GetAllNodes(testMaxHeap);
            Assert.AreEqual(7, result);
            Assert.AreEqual(6, allNodes.Count);
        }
    }
}
