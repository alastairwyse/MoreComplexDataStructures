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
    /// Unit tests for the MinHeap class.
    /// </summary>
    public class MinHeapTests : HeapTestBase
    {
        private MinHeap<Int32> testMinHeap;

        [SetUp]
        protected void SetUp()
        {
            testMinHeap = new MinHeap<Int32>();
        }

        /// <summary>
        /// Success tests for the Insert() method.
        /// </summary>
        [Test]
        public void Insert()
        {
            // Test when the heap is empty
            List<Int32> allItems = GetListOfItems(testMinHeap);

            Assert.AreEqual(0, allItems.Count);


            // Test with a single item
            testMinHeap.Insert(4);

            Dictionary<Int32, DoublyLinkedTreeNode<Int32>> allNodes = GetAllNodes(testMinHeap);
            Assert.AreEqual(1, allNodes.Count);
            Assert.IsNull(allNodes[4].ParentNode);


            // Test with an additional layer of depth
            // Resulting tree should be...
            //     2
            //    / \
            //   4   3
            testMinHeap.Insert(3);
            testMinHeap.Insert(2);
            allNodes = GetAllNodes(testMinHeap);
            Assert.AreEqual(3, allNodes.Count);
            Assert.IsNull(allNodes[4].LeftChildNode);
            Assert.IsNull(allNodes[4].RightChildNode);
            Assert.AreEqual(allNodes[2], allNodes[4].ParentNode);
            Assert.IsNull(allNodes[3].LeftChildNode);
            Assert.IsNull(allNodes[3].RightChildNode);
            Assert.AreEqual(allNodes[2], allNodes[3].ParentNode);
            Assert.AreEqual(allNodes[4], allNodes[2].LeftChildNode);
            Assert.AreEqual(allNodes[3], allNodes[2].RightChildNode);
            Assert.IsNull(allNodes[2].ParentNode);


            // Add one more item, to test moving to a new depth
            // Resulting tree should be...
            //       1
            //      / \
            //     2   3
            //    /
            //   4
            testMinHeap.Insert(1);
            allNodes = GetAllNodes(testMinHeap);
            Assert.AreEqual(4, allNodes.Count);
            Assert.IsNull(allNodes[4].LeftChildNode);
            Assert.IsNull(allNodes[4].RightChildNode);
            Assert.AreEqual(allNodes[2], allNodes[4].ParentNode);
            Assert.IsNull(allNodes[3].LeftChildNode);
            Assert.IsNull(allNodes[3].RightChildNode);
            Assert.AreEqual(allNodes[1], allNodes[3].ParentNode);
            Assert.AreEqual(allNodes[4], allNodes[2].LeftChildNode);
            Assert.IsNull(allNodes[2].RightChildNode);
            Assert.AreEqual(allNodes[1], allNodes[2].ParentNode);
            Assert.AreEqual(allNodes[2], allNodes[1].LeftChildNode);
            Assert.AreEqual(allNodes[3], allNodes[1].RightChildNode);
            Assert.IsNull(allNodes[1].ParentNode);


            // Test adding a duplicate value
            // Resulting tree should be...
            //       1
            //      / \
            //     1   3
            //    / \
            //   4   2
            testMinHeap.Insert(1);
            allItems = GetListOfItems(testMinHeap);
            Assert.AreEqual(1, allItems[0]);
            Assert.AreEqual(1, allItems[1]);
            Assert.AreEqual(3, allItems[2]);
            Assert.AreEqual(4, allItems[3]);
            Assert.AreEqual(2, allItems[4]);


            // Test where no child/parent swaps are required during insert
            // Resulting tree should be...
            //     1
            //    / \
            //   2   3
            testMinHeap = new MinHeap<Int32>();
            testMinHeap.Insert(1);
            testMinHeap.Insert(2);
            testMinHeap.Insert(3);
            allNodes = GetAllNodes(testMinHeap);
            Assert.AreEqual(3, allNodes.Count);
            Assert.IsNull(allNodes[2].LeftChildNode);
            Assert.IsNull(allNodes[2].RightChildNode);
            Assert.AreEqual(allNodes[1], allNodes[2].ParentNode);
            Assert.IsNull(allNodes[3].LeftChildNode);
            Assert.IsNull(allNodes[3].RightChildNode);
            Assert.AreEqual(allNodes[1], allNodes[3].ParentNode);
            Assert.AreEqual(allNodes[2], allNodes[1].LeftChildNode);
            Assert.AreEqual(allNodes[3], allNodes[1].RightChildNode);
            Assert.IsNull(allNodes[1].ParentNode);
        }

        /// <summary>
        /// Tests that an exception is thrown if the ExtractMin() method is called when the heap is empty.
        /// </summary>
        [Test]
        public void ExtractMin_HeapIsEmpty()
        {
            Exception e = Assert.Throws<Exception>(delegate
            {
                testMinHeap.ExtractMin();
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The heap is empty."));
        }

        /// <summary>
        /// Success tests for the ExtractMin() method.
        /// </summary>
        [Test]
        public void ExtractMin()
        {
            // Test with a single item
            testMinHeap.Insert(4);
            Int32 result = testMinHeap.ExtractMin();
            Dictionary<Int32, DoublyLinkedTreeNode<Int32>> allNodes = GetAllNodes(testMinHeap);
            Assert.AreEqual(4, result);
            Assert.AreEqual(0, allNodes.Count);


            // Test with the following tree
            //       4
            //      / \
            //     2   3
            //    /
            //   4
            testMinHeap.Insert(4);
            testMinHeap.Insert(3);
            testMinHeap.Insert(2);
            testMinHeap.Insert(1);

            result = testMinHeap.ExtractMin();
            // Structure should be...
            //     2
            //    / \
            //   4   3
            allNodes = GetAllNodes(testMinHeap);
            Assert.AreEqual(1, result);
            Assert.AreEqual(3, allNodes.Count);
            Assert.AreEqual(allNodes[4], allNodes[2].LeftChildNode);
            Assert.AreEqual(allNodes[3], allNodes[2].RightChildNode);
            Assert.IsNull(allNodes[2].ParentNode);
            Assert.IsNull(allNodes[4].LeftChildNode);
            Assert.IsNull(allNodes[4].RightChildNode);
            Assert.AreEqual(allNodes[2], allNodes[4].ParentNode);
            Assert.IsNull(allNodes[3].LeftChildNode);
            Assert.IsNull(allNodes[3].RightChildNode);
            Assert.AreEqual(allNodes[2], allNodes[3].ParentNode);


            result = testMinHeap.ExtractMin();
            // Structure should be...
            //     3
            //    / 
            //   4   
            allNodes = GetAllNodes(testMinHeap);
            Assert.AreEqual(2, result);
            Assert.AreEqual(2, allNodes.Count);
            Assert.AreEqual(allNodes[4], allNodes[3].LeftChildNode);
            Assert.IsNull(allNodes[3].RightChildNode);
            Assert.IsNull(allNodes[3].ParentNode);
            Assert.IsNull(allNodes[4].LeftChildNode);
            Assert.IsNull(allNodes[4].RightChildNode);
            Assert.AreEqual(allNodes[3], allNodes[4].ParentNode);


            result = testMinHeap.ExtractMin();
            // Structure should be...
            //     4
            allNodes = GetAllNodes(testMinHeap);
            Assert.AreEqual(3, result);
            Assert.AreEqual(1, allNodes.Count);
            Assert.IsNull(allNodes[4].ParentNode);
            Assert.IsNull(allNodes[4].LeftChildNode);
            Assert.IsNull(allNodes[4].RightChildNode);


            result = testMinHeap.ExtractMin();
            allNodes = GetAllNodes(testMinHeap);
            Assert.AreEqual(4, result);
            Assert.AreEqual(0, allNodes.Count);


            // Test with the following tree
            //     1
            //    / \
            //   2   3
            testMinHeap.Insert(1);
            testMinHeap.Insert(2);
            testMinHeap.Insert(3);

            result = testMinHeap.ExtractMin();
            allNodes = GetAllNodes(testMinHeap);
            Assert.AreEqual(1, result);
            Assert.AreEqual(2, allNodes.Count);
            Assert.AreEqual(allNodes[3], allNodes[2].LeftChildNode);
            Assert.IsNull(allNodes[2].RightChildNode);
            Assert.IsNull(allNodes[2].ParentNode);
            Assert.IsNull(allNodes[3].LeftChildNode);
            Assert.IsNull(allNodes[3].RightChildNode);
            Assert.AreEqual(allNodes[2], allNodes[3].ParentNode);


            // Test with the following tree
            //       1
            //      / \
            //     2   4
            //    / \
            //   3   5
            testMinHeap = new MinHeap<Int32>();
            testMinHeap.Insert(2);
            testMinHeap.Insert(1);
            testMinHeap.Insert(4);
            testMinHeap.Insert(3);
            testMinHeap.Insert(5);

            result = testMinHeap.ExtractMin();
            allNodes = GetAllNodes(testMinHeap);
            Assert.AreEqual(1, result);
            Assert.AreEqual(4, allNodes.Count);


            // Test with the following tree
            //        1
            //      /   \
            //     3     2
            //    / \   /
            //   7   8 9 
            testMinHeap = new MinHeap<Int32>();
            testMinHeap.Insert(3);
            testMinHeap.Insert(1);
            testMinHeap.Insert(2);
            testMinHeap.Insert(7);
            testMinHeap.Insert(8);
            testMinHeap.Insert(9);

            result = testMinHeap.ExtractMin();
            allNodes = GetAllNodes(testMinHeap);
            Assert.AreEqual(1, result);
            Assert.AreEqual(5, allNodes.Count);
            Assert.AreEqual(allNodes[3], allNodes[2].LeftChildNode);
            Assert.AreEqual(allNodes[9], allNodes[2].RightChildNode);
            Assert.IsNull(allNodes[2].ParentNode);
            Assert.AreEqual(allNodes[7], allNodes[3].LeftChildNode);
            Assert.AreEqual(allNodes[8], allNodes[3].RightChildNode);
            Assert.AreEqual(allNodes[2], allNodes[3].ParentNode);
            Assert.IsNull(allNodes[9].LeftChildNode);
            Assert.IsNull(allNodes[9].RightChildNode);
            Assert.AreEqual(allNodes[2], allNodes[9].ParentNode);
            Assert.IsNull(allNodes[7].LeftChildNode);
            Assert.IsNull(allNodes[7].RightChildNode);
            Assert.AreEqual(allNodes[3], allNodes[7].ParentNode);
            Assert.IsNull(allNodes[8].LeftChildNode);
            Assert.IsNull(allNodes[8].RightChildNode);
            Assert.AreEqual(allNodes[3], allNodes[8].ParentNode);


            // Test with the following tree
            //     5
            //    / \
            //   5   5
            testMinHeap = new MinHeap<Int32>();
            testMinHeap.Insert(5);
            testMinHeap.Insert(5);
            testMinHeap.Insert(5);

            result = testMinHeap.ExtractMin();
            Assert.AreEqual(5, result);
            result = testMinHeap.ExtractMin();
            Assert.AreEqual(5, result);
            result = testMinHeap.ExtractMin();
            Assert.AreEqual(5, result);
            allNodes = GetAllNodes(testMinHeap);
            Assert.AreEqual(0, allNodes.Count);
        }

        /// <summary>
        /// Success tests for the Count property.
        /// </summary>
        [Test]
        public void Count()
        {
            Assert.AreEqual(0, testMinHeap.Count);

            testMinHeap.Insert(3);
            Assert.AreEqual(1, testMinHeap.Count);
            testMinHeap.Insert(1);
            Assert.AreEqual(2, testMinHeap.Count);
            testMinHeap.Insert(2);
            Assert.AreEqual(3, testMinHeap.Count);
            testMinHeap.Insert(2);
            Assert.AreEqual(4, testMinHeap.Count);
            testMinHeap.Insert(8);
            Assert.AreEqual(5, testMinHeap.Count);
            testMinHeap.Insert(9);
            Assert.AreEqual(6, testMinHeap.Count);

            testMinHeap.ExtractMin();
            Assert.AreEqual(5, testMinHeap.Count);
            testMinHeap.ExtractMin();
            Assert.AreEqual(4, testMinHeap.Count);
            testMinHeap.ExtractMin();
            Assert.AreEqual(3, testMinHeap.Count);
            testMinHeap.ExtractMin();
            Assert.AreEqual(2, testMinHeap.Count);
            testMinHeap.ExtractMin();
            Assert.AreEqual(1, testMinHeap.Count);
            testMinHeap.ExtractMin();
            Assert.AreEqual(0, testMinHeap.Count);

            testMinHeap.Insert(3);
            Assert.AreEqual(1, testMinHeap.Count);
        }

        /// <summary>
        /// Success tests for the Clear() method.
        /// </summary>
        [Test]
        public void Clear()
        {
            testMinHeap.Insert(2);
            testMinHeap.Insert(7);
            testMinHeap.Insert(8);
            testMinHeap.Insert(9);

            testMinHeap.Clear();
            Dictionary<Int32, DoublyLinkedTreeNode<Int32>> allNodes = GetAllNodes(testMinHeap);
            Assert.AreEqual(0, allNodes.Count);

            testMinHeap.Insert(10);
            allNodes = GetAllNodes(testMinHeap);
            Assert.AreEqual(1, allNodes.Count);
            Assert.IsNull(allNodes[10].ParentNode);
            Assert.IsNull(allNodes[10].LeftChildNode);
            Assert.IsNull(allNodes[10].RightChildNode);
        }

        /// <summary>
        /// Success tests for the BreadthFirstSearch() method.
        /// </summary>
        [Test]
        public void BreadthFirstSearch()
        {
            // Test with the following tree
            //        1
            //      /   \
            //     3     2
            //    / \   /
            //   7   8 9 
            testMinHeap.Insert(3);
            testMinHeap.Insert(1);
            testMinHeap.Insert(2);
            testMinHeap.Insert(7);
            testMinHeap.Insert(8);
            testMinHeap.Insert(9);

            List<Int32> breadthFirstItems = GetListOfItems(testMinHeap);
            Assert.AreEqual(1, breadthFirstItems[0]);
            Assert.AreEqual(3, breadthFirstItems[1]);
            Assert.AreEqual(2, breadthFirstItems[2]);
            Assert.AreEqual(7, breadthFirstItems[3]);
            Assert.AreEqual(8, breadthFirstItems[4]);
            Assert.AreEqual(9, breadthFirstItems[5]);
        }

        /// <summary>
        /// Tests that an exception is thrown if the Peek() method is called when the heap is empty.
        /// </summary>
        [Test]
        public void Peek_HeapIsEmpty()
        {
            Exception e = Assert.Throws<Exception>(delegate
            {
                testMinHeap.Peek();
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The heap is empty."));
        }

        /// <summary>
        /// Success tests for the Peek() method.
        /// </summary>
        [Test]
        public void Peek()
        {
            testMinHeap.Insert(7);
            testMinHeap.Insert(6);
            testMinHeap.Insert(5);
            testMinHeap.Insert(4);
            testMinHeap.Insert(3);
            testMinHeap.Insert(1);

            Int32 result = testMinHeap.Peek();
            Dictionary<Int32, DoublyLinkedTreeNode<Int32>> allNodes = GetAllNodes(testMinHeap);
            Assert.AreEqual(1, result);
            Assert.AreEqual(6, allNodes.Count);
        }
    }
}
