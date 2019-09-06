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
    /// Unit tests for the WeightBalancedTree class.
    /// </summary>
    public class WeightBalancedTreeTests
    {
        private WeightBalancedTree<Int32> testWeightBalancedTree;

        [SetUp]
        protected void SetUp()
        {
            testWeightBalancedTree = new WeightBalancedTree<Int32>(false);
        }

        /// <summary>
        /// Tests that an exception is thrown, and that the subtree counts are maintained if a duplicate root node value is attempted to be added.
        /// </summary>
        [Test]
        public void Add_DuplicateRoot()
        {
            // Tree structure is...
            //     2
            //    / \
            //   1   3
            Int32[] treeItems = new Int32[] { 2, 1, 3 };
            foreach(Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                testWeightBalancedTree.Add(2);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("A node holding the item specified in parameter 'item' (value = '2') already exists in the tree."));
            Assert.AreEqual("item", e.ParamName);

            Dictionary<Int32, WeightBalancedTreeNode<Int32>> allNodes = PutAllNodesInDictionary(testWeightBalancedTree);
            Assert.AreEqual(3, allNodes.Count);
            Assert.AreEqual(1, allNodes[2].LeftSubtreeSize);
            Assert.AreEqual(1, allNodes[2].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[1].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[1].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[3].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[3].RightSubtreeSize);
        }

        /// <summary>
        /// Tests that an exception is thrown, and that the subtree counts are maintained if a duplicate value is attempted to be added which exists on the left of the root.
        /// </summary>
        [Test]
        public void Add_DuplicateLeftSubtree()
        {
            // Tree structure is...
            //     2
            //    / \
            //   1   3
            Int32[] treeItems = new Int32[] { 2, 1, 3 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                testWeightBalancedTree.Add(1);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("A node holding the item specified in parameter 'item' (value = '1') already exists in the tree."));
            Assert.AreEqual("item", e.ParamName);

            Dictionary<Int32, WeightBalancedTreeNode<Int32>> allNodes = PutAllNodesInDictionary(testWeightBalancedTree);
            Assert.AreEqual(3, allNodes.Count);
            Assert.AreEqual(1, allNodes[2].LeftSubtreeSize);
            Assert.AreEqual(1, allNodes[2].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[1].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[1].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[3].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[3].RightSubtreeSize);
        }

        /// <summary>
        /// Tests that an exception is thrown, and that the subtree counts are maintained if a duplicate value is attempted to be added which exists on the right of the root.
        /// </summary>
        [Test]
        public void Add_DuplicateRightSubtree()
        {
            // Tree structure is...
            //     2
            //    / \
            //   1   3
            Int32[] treeItems = new Int32[] { 2, 1, 3 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                testWeightBalancedTree.Add(3);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("A node holding the item specified in parameter 'item' (value = '3') already exists in the tree."));
            Assert.AreEqual("item", e.ParamName);

            Dictionary<Int32, WeightBalancedTreeNode<Int32>> allNodes = PutAllNodesInDictionary(testWeightBalancedTree);
            Assert.AreEqual(3, allNodes.Count);
            Assert.AreEqual(1, allNodes[2].LeftSubtreeSize);
            Assert.AreEqual(1, allNodes[2].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[1].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[1].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[3].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[3].RightSubtreeSize);
        }

        /// <summary>
        /// Success tests for the Add() method in an unbalanced tree.
        /// </summary>
        [Test]
        public void Add_Unbalanced()
        {
            // Tree structure is...
            //      4
            //     / \
            //   2     6
            //  / \   / \
            // 1   3 5   7
            Int32[] treeItems = new Int32[] { 4, 2, 6, 1, 3, 5, 7 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            Dictionary<Int32, WeightBalancedTreeNode<Int32>> allNodes = PutAllNodesInDictionary(testWeightBalancedTree);
            Assert.AreEqual(7, allNodes.Count);
            // Test the parent / child relationships
            Assert.AreSame(allNodes[2], allNodes[4].LeftChildNode);
            Assert.AreSame(allNodes[6], allNodes[4].RightChildNode);
            Assert.AreSame(allNodes[1], allNodes[2].LeftChildNode);
            Assert.AreSame(allNodes[3], allNodes[2].RightChildNode);
            Assert.AreSame(allNodes[5], allNodes[6].LeftChildNode);
            Assert.AreSame(allNodes[7], allNodes[6].RightChildNode);
            Assert.IsNull(allNodes[1].LeftChildNode);
            Assert.IsNull(allNodes[1].RightChildNode);
            Assert.IsNull(allNodes[3].LeftChildNode);
            Assert.IsNull(allNodes[3].RightChildNode);
            Assert.IsNull(allNodes[5].LeftChildNode);
            Assert.IsNull(allNodes[5].RightChildNode);
            Assert.IsNull(allNodes[7].LeftChildNode);
            Assert.IsNull(allNodes[7].RightChildNode);
            Assert.AreSame(allNodes[2], allNodes[1].ParentNode);
            Assert.AreSame(allNodes[2], allNodes[3].ParentNode);
            Assert.AreSame(allNodes[6], allNodes[5].ParentNode);
            Assert.AreSame(allNodes[6], allNodes[7].ParentNode);
            Assert.AreSame(allNodes[4], allNodes[2].ParentNode);
            Assert.AreSame(allNodes[4], allNodes[6].ParentNode);
            Assert.IsNull(allNodes[4].ParentNode);
            // Test the subtree sizes
            Assert.AreEqual(3, allNodes[4].LeftSubtreeSize);
            Assert.AreEqual(3, allNodes[4].RightSubtreeSize);
            Assert.AreEqual(1, allNodes[2].LeftSubtreeSize);
            Assert.AreEqual(1, allNodes[2].RightSubtreeSize);
            Assert.AreEqual(1, allNodes[6].LeftSubtreeSize);
            Assert.AreEqual(1, allNodes[6].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[1].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[1].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[3].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[3].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[5].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[5].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[7].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[7].RightSubtreeSize);

            // Tree structure is...
            //             7
            //            /
            //           6
            //          /
            //         5
            //        /
            //       4
            //      /
            //     3
            //    /
            //   2
            //  /
            // 1
            testWeightBalancedTree = new WeightBalancedTree<Int32>(false);
            treeItems = new Int32[] { 7, 6, 5, 4, 3, 2, 1 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            allNodes = PutAllNodesInDictionary(testWeightBalancedTree);
            Assert.AreEqual(7, allNodes.Count);
            // Test the parent / child relationships
            Assert.AreSame(allNodes[6], allNodes[7].LeftChildNode);
            Assert.AreSame(allNodes[5], allNodes[6].LeftChildNode);
            Assert.AreSame(allNodes[4], allNodes[5].LeftChildNode);
            Assert.AreSame(allNodes[3], allNodes[4].LeftChildNode);
            Assert.AreSame(allNodes[2], allNodes[3].LeftChildNode);
            Assert.AreSame(allNodes[1], allNodes[2].LeftChildNode);
            Assert.IsNull(allNodes[1].LeftChildNode);
            Assert.IsNull(allNodes[7].RightChildNode);
            Assert.IsNull(allNodes[6].RightChildNode);
            Assert.IsNull(allNodes[5].RightChildNode);
            Assert.IsNull(allNodes[4].RightChildNode);
            Assert.IsNull(allNodes[3].RightChildNode);
            Assert.IsNull(allNodes[2].RightChildNode);
            Assert.IsNull(allNodes[1].RightChildNode);
            Assert.AreSame(allNodes[2], allNodes[1].ParentNode);
            Assert.AreSame(allNodes[3], allNodes[2].ParentNode);
            Assert.AreSame(allNodes[4], allNodes[3].ParentNode);
            Assert.AreSame(allNodes[5], allNodes[4].ParentNode);
            Assert.AreSame(allNodes[6], allNodes[5].ParentNode);
            Assert.AreSame(allNodes[7], allNodes[6].ParentNode);
            Assert.IsNull(allNodes[7].ParentNode);
            // Test the subtree sizes
            Assert.AreEqual(6, allNodes[7].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[7].RightSubtreeSize);
            Assert.AreEqual(5, allNodes[6].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[6].RightSubtreeSize);
            Assert.AreEqual(4, allNodes[5].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[5].RightSubtreeSize);
            Assert.AreEqual(3, allNodes[4].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[4].RightSubtreeSize);
            Assert.AreEqual(2, allNodes[3].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[3].RightSubtreeSize);
            Assert.AreEqual(1, allNodes[2].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[2].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[1].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[1].RightSubtreeSize);

            // Tree structure is...
            // 1
            //  \
            //   2
            //    \
            //     3
            //      \
            //       4
            //        \
            //         5
            //          \
            //           6
            //            \
            //             7
            testWeightBalancedTree = new WeightBalancedTree<Int32>(false);
            treeItems = new Int32[] { 1, 2, 3, 4, 5, 6, 7 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            allNodes = PutAllNodesInDictionary(testWeightBalancedTree);
            Assert.AreEqual(7, allNodes.Count);
            // Test the parent / child relationships
            Assert.AreSame(allNodes[2], allNodes[1].RightChildNode);
            Assert.AreSame(allNodes[3], allNodes[2].RightChildNode);
            Assert.AreSame(allNodes[4], allNodes[3].RightChildNode);
            Assert.AreSame(allNodes[5], allNodes[4].RightChildNode);
            Assert.AreSame(allNodes[6], allNodes[5].RightChildNode);
            Assert.AreSame(allNodes[7], allNodes[6].RightChildNode);
            Assert.IsNull(allNodes[7].RightChildNode);
            Assert.IsNull(allNodes[1].LeftChildNode);
            Assert.IsNull(allNodes[2].LeftChildNode);
            Assert.IsNull(allNodes[3].LeftChildNode);
            Assert.IsNull(allNodes[4].LeftChildNode);
            Assert.IsNull(allNodes[5].LeftChildNode);
            Assert.IsNull(allNodes[6].LeftChildNode);
            Assert.IsNull(allNodes[7].LeftChildNode);
            Assert.AreSame(allNodes[6], allNodes[7].ParentNode);
            Assert.AreSame(allNodes[5], allNodes[6].ParentNode);
            Assert.AreSame(allNodes[4], allNodes[5].ParentNode);
            Assert.AreSame(allNodes[3], allNodes[4].ParentNode);
            Assert.AreSame(allNodes[2], allNodes[3].ParentNode);
            Assert.AreSame(allNodes[1], allNodes[2].ParentNode);
            Assert.IsNull(allNodes[1].ParentNode);
            // Test the subtree sizes
            Assert.AreEqual(0, allNodes[1].LeftSubtreeSize);
            Assert.AreEqual(6, allNodes[1].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[2].LeftSubtreeSize);
            Assert.AreEqual(5, allNodes[2].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[3].LeftSubtreeSize);
            Assert.AreEqual(4, allNodes[3].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[4].LeftSubtreeSize);
            Assert.AreEqual(3, allNodes[4].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[5].LeftSubtreeSize);
            Assert.AreEqual(2, allNodes[5].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[6].LeftSubtreeSize);
            Assert.AreEqual(1, allNodes[6].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[7].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[7].RightSubtreeSize);
        }

        /// <summary>
        /// Success tests for the Add() method in a balanced tree where items are added in ascending order.
        /// </summary>
        [Test]
        public void Add_BalancedInsertInAscendingOrder()
        {
            // Insert in ascending order.  After balancing the tree structure should be...
            //      4
            //     / \
            //   2     6
            //  / \   / \
            // 1   3 5   7
            Int32[] treeItems = new Int32[] { 1, 2, 3, 4, 5, 6, 7 };
            testWeightBalancedTree = new WeightBalancedTree<Int32>(true);
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            Dictionary<Int32, WeightBalancedTreeNode<Int32>> allNodes = PutAllNodesInDictionary(testWeightBalancedTree);
            Assert.AreEqual(7, allNodes.Count);
            // Test the parent / child relationships
            Assert.AreSame(allNodes[2], allNodes[4].LeftChildNode);
            Assert.AreSame(allNodes[6], allNodes[4].RightChildNode);
            Assert.AreSame(allNodes[1], allNodes[2].LeftChildNode);
            Assert.AreSame(allNodes[3], allNodes[2].RightChildNode);
            Assert.AreSame(allNodes[5], allNodes[6].LeftChildNode);
            Assert.AreSame(allNodes[7], allNodes[6].RightChildNode);
            Assert.IsNull(allNodes[1].LeftChildNode);
            Assert.IsNull(allNodes[1].RightChildNode);
            Assert.IsNull(allNodes[3].LeftChildNode);
            Assert.IsNull(allNodes[3].RightChildNode);
            Assert.IsNull(allNodes[5].LeftChildNode);
            Assert.IsNull(allNodes[5].RightChildNode);
            Assert.IsNull(allNodes[7].LeftChildNode);
            Assert.IsNull(allNodes[7].RightChildNode);
            Assert.AreSame(allNodes[2], allNodes[1].ParentNode);
            Assert.AreSame(allNodes[2], allNodes[3].ParentNode);
            Assert.AreSame(allNodes[6], allNodes[5].ParentNode);
            Assert.AreSame(allNodes[6], allNodes[7].ParentNode);
            Assert.AreSame(allNodes[4], allNodes[2].ParentNode);
            Assert.AreSame(allNodes[4], allNodes[6].ParentNode);
            Assert.IsNull(allNodes[4].ParentNode);
            // Test the subtree sizes
            Assert.AreEqual(3, allNodes[4].LeftSubtreeSize);
            Assert.AreEqual(3, allNodes[4].RightSubtreeSize);
            Assert.AreEqual(1, allNodes[2].LeftSubtreeSize);
            Assert.AreEqual(1, allNodes[2].RightSubtreeSize);
            Assert.AreEqual(1, allNodes[6].LeftSubtreeSize);
            Assert.AreEqual(1, allNodes[6].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[1].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[1].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[3].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[3].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[5].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[5].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[7].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[7].RightSubtreeSize);
        }

        /// <summary>
        /// Success tests for the Add() method in a balanced tree where items are added in descending order.
        /// </summary>
        [Test]
        public void Add_BalancedInsertInDescendingOrder()
        {
            // Insert in descending order.  After balancing the tree structure should be...
            //      4
            //     / \
            //   2     6
            //  / \   / \
            // 1   3 5   7
            Int32[] treeItems = new Int32[] { 7, 6, 5, 4, 3, 2, 1 };
            testWeightBalancedTree = new WeightBalancedTree<Int32>(true);
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            Dictionary<Int32, WeightBalancedTreeNode<Int32>> allNodes = PutAllNodesInDictionary(testWeightBalancedTree);
            Assert.AreEqual(7, allNodes.Count);
            // Test the parent / child relationships
            Assert.AreSame(allNodes[2], allNodes[4].LeftChildNode);
            Assert.AreSame(allNodes[6], allNodes[4].RightChildNode);
            Assert.AreSame(allNodes[1], allNodes[2].LeftChildNode);
            Assert.AreSame(allNodes[3], allNodes[2].RightChildNode);
            Assert.AreSame(allNodes[5], allNodes[6].LeftChildNode);
            Assert.AreSame(allNodes[7], allNodes[6].RightChildNode);
            Assert.IsNull(allNodes[1].LeftChildNode);
            Assert.IsNull(allNodes[1].RightChildNode);
            Assert.IsNull(allNodes[3].LeftChildNode);
            Assert.IsNull(allNodes[3].RightChildNode);
            Assert.IsNull(allNodes[5].LeftChildNode);
            Assert.IsNull(allNodes[5].RightChildNode);
            Assert.IsNull(allNodes[7].LeftChildNode);
            Assert.IsNull(allNodes[7].RightChildNode);
            Assert.AreSame(allNodes[2], allNodes[1].ParentNode);
            Assert.AreSame(allNodes[2], allNodes[3].ParentNode);
            Assert.AreSame(allNodes[6], allNodes[5].ParentNode);
            Assert.AreSame(allNodes[6], allNodes[7].ParentNode);
            Assert.AreSame(allNodes[4], allNodes[2].ParentNode);
            Assert.AreSame(allNodes[4], allNodes[6].ParentNode);
            Assert.IsNull(allNodes[4].ParentNode);
            // Test the subtree sizes
            Assert.AreEqual(3, allNodes[4].LeftSubtreeSize);
            Assert.AreEqual(3, allNodes[4].RightSubtreeSize);
            Assert.AreEqual(1, allNodes[2].LeftSubtreeSize);
            Assert.AreEqual(1, allNodes[2].RightSubtreeSize);
            Assert.AreEqual(1, allNodes[6].LeftSubtreeSize);
            Assert.AreEqual(1, allNodes[6].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[1].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[1].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[3].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[3].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[5].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[5].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[7].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[7].RightSubtreeSize);
        }

        /// <summary>
        /// Success tests for the Count property in an unbalanced tree.
        /// </summary>
        [Test]
        public void Count_Unbalanced()
        {
            Assert.AreEqual(0, testWeightBalancedTree.Count);

            // Test a fully balanced tree structure
            //      4
            //     / \
            //   2     6
            //  / \   / \
            // 1   3 5   7
            Int32[] treeItems = new Int32[] { 4, 2, 6, 1, 3, 5, 7 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            Assert.AreEqual(7, testWeightBalancedTree.Count);

            // Test the same input values but fully unbalanced on the left side
            testWeightBalancedTree = new WeightBalancedTree<Int32>(false);
            treeItems = new Int32[] { 7, 6, 5, 4, 3, 2, 1 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            Assert.AreEqual(7, testWeightBalancedTree.Count);

            // Test the same input values but fully unbalanced on the right side
            testWeightBalancedTree = new WeightBalancedTree<Int32>(false);
            treeItems = new Int32[] { 1, 2, 3, 4, 5, 6, 7 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            Assert.AreEqual(7, testWeightBalancedTree.Count);
        }

        /// <summary>
        /// Success tests for the Count property in a balanced tree.
        /// </summary>
        [Test]
        public void Count_Balanced()
        {
            testWeightBalancedTree = new WeightBalancedTree<Int32>(true);

            Assert.AreEqual(0, testWeightBalancedTree.Count);

            foreach (Int32 currentItem in Enumerable.Range(1, 7))
            {
                testWeightBalancedTree.Add(currentItem);
                Assert.AreEqual(currentItem, testWeightBalancedTree.Count);
            }

            foreach (Int32 currentItem in Enumerable.Range(1, 7))
            {
                testWeightBalancedTree.Remove(currentItem);
                Assert.AreEqual(7 - currentItem, testWeightBalancedTree.Count);
            }
        }

        /// <summary>
        /// Success tests for the Depth property in a balanced tree.
        /// </summary>
        [Test]
        public void Depth_Balanced()
        {
            testWeightBalancedTree = new WeightBalancedTree<Int32>(true);

            Assert.AreEqual(-1, testWeightBalancedTree.Depth);

            testWeightBalancedTree.Add(1);

            Assert.AreEqual(0, testWeightBalancedTree.Depth);

            testWeightBalancedTree.Add(2);

            Assert.AreEqual(1, testWeightBalancedTree.Depth);

            testWeightBalancedTree.Add(3);

            Assert.AreEqual(1, testWeightBalancedTree.Depth);

            testWeightBalancedTree.Add(4);

            Assert.AreEqual(2, testWeightBalancedTree.Depth);

            testWeightBalancedTree.Remove(4);

            Assert.AreEqual(1, testWeightBalancedTree.Depth);

            testWeightBalancedTree.Remove(3);

            Assert.AreEqual(1, testWeightBalancedTree.Depth);

            testWeightBalancedTree.Remove(2);

            Assert.AreEqual(0, testWeightBalancedTree.Depth);

            testWeightBalancedTree.Remove(1);

            Assert.AreEqual(-1, testWeightBalancedTree.Depth);

            testWeightBalancedTree.Add(1);

            Assert.AreEqual(0, testWeightBalancedTree.Depth);

            testWeightBalancedTree.Add(2);

            Assert.AreEqual(1, testWeightBalancedTree.Depth);

            testWeightBalancedTree.Clear();

            Assert.AreEqual(-1, testWeightBalancedTree.Depth);
        }

        /// <summary>
        /// Success tests for the Depth property in an unbalanced tree.
        /// </summary>
        [Test]
        public void Depth_Unbalanced()
        {
            Assert.AreEqual(-1, testWeightBalancedTree.Depth);

            // Test a fully balanced tree structure
            //      4
            //     / \
            //   2     6
            //  / \   / \
            // 1   3 5   7
            Int32[] treeItems = new Int32[] { 4, 2, 6, 1, 3, 5, 7 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            Assert.AreEqual(2, testWeightBalancedTree.Depth);

            // Test the same input values but fully unbalanced on the left side
            testWeightBalancedTree = new WeightBalancedTree<Int32>(false);
            treeItems = new Int32[] { 7, 6, 5, 4, 3, 2, 1 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            Assert.AreEqual(6, testWeightBalancedTree.Depth);

            // Test the same input values but fully unbalanced on the right side
            testWeightBalancedTree = new WeightBalancedTree<Int32>(false);
            treeItems = new Int32[] { 1, 2, 3, 4, 5, 6, 7 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            Assert.AreEqual(6, testWeightBalancedTree.Depth);
        }

        /// <summary>
        /// Success tests for the Depth property after the Remove() method has been called in an unbalanced tree.
        /// </summary>
        [Test]
        public void Depth_UnbalancedCalledAfterRemove()
        {
            // First call Remove() on the tree to force depth-first search
            testWeightBalancedTree.Add(30);
            testWeightBalancedTree.Remove(30);

            Assert.AreEqual(-1, testWeightBalancedTree.Depth);

            // Test adding a single node
            testWeightBalancedTree.Add(5);
            Assert.AreEqual(0, testWeightBalancedTree.Depth);


            // Test the following tree...
            //       5
            //      /  
            //     4 
            //    / 
            //   3 
            testWeightBalancedTree.Add(4);
            testWeightBalancedTree.Add(3);
            Assert.AreEqual(2, testWeightBalancedTree.Depth);


            // Test the following tree...
            //  5
            //   \  
            //    6 
            //     \ 
            //      7 
            testWeightBalancedTree.Remove(3);
            testWeightBalancedTree.Remove(4);
            testWeightBalancedTree.Add(6);
            testWeightBalancedTree.Add(7);
            Assert.AreEqual(2, testWeightBalancedTree.Depth);


            // Test the following tree...
            //     5
            //    /  
            //   3 
            //    \
            //     4
            testWeightBalancedTree.Remove(7);
            testWeightBalancedTree.Remove(6);
            testWeightBalancedTree.Add(3);
            testWeightBalancedTree.Add(4);
            Assert.AreEqual(2, testWeightBalancedTree.Depth);


            // Test the following tree...
            //   5
            //    \  
            //     7 
            //    /
            //   6
            testWeightBalancedTree.Remove(4);
            testWeightBalancedTree.Remove(3);
            testWeightBalancedTree.Add(7);
            testWeightBalancedTree.Add(6);
            Assert.AreEqual(2, testWeightBalancedTree.Depth);


            // Create the following tree...
            //      4
            //     / \
            //   1     6
            //  / \   / \
            // 0   2 5   7
            testWeightBalancedTree.Remove(6);
            testWeightBalancedTree.Remove(7);
            testWeightBalancedTree.Remove(5);
            Int32[] treeItems = new Int32[] { 4, 1, 6, 0, 2, 5, 7 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }
            Assert.AreEqual(2, testWeightBalancedTree.Depth);


            // Create the following tree...
            //       4
            //     /   \
            //   1       6
            //  / \     / \
            // 0   2   5   7
            //      \
            //       3
            testWeightBalancedTree.Add(3);
            Assert.AreEqual(3, testWeightBalancedTree.Depth);


            // Remove all except the root
            testWeightBalancedTree.Remove(3);
            testWeightBalancedTree.Remove(0);
            testWeightBalancedTree.Remove(2);
            testWeightBalancedTree.Remove(5);
            testWeightBalancedTree.Remove(7);
            testWeightBalancedTree.Remove(1);
            testWeightBalancedTree.Remove(6);
            Assert.AreEqual(0, testWeightBalancedTree.Depth);


            // Remove all 
            testWeightBalancedTree.Remove(4);
            Assert.AreEqual(-1, testWeightBalancedTree.Depth);
        }

        /// <summary>
        /// Success tests for the Clear() method in an unbalanced tree.
        /// </summary>
        [Test]
        public void Clear_Unbalanced()
        {
            Int32[] treeItems = new Int32[] { 4, 2, 6, 1, 3, 5, 7 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            Assert.AreEqual(7, testWeightBalancedTree.Count);
            Assert.AreEqual(2, testWeightBalancedTree.Depth);

            testWeightBalancedTree.Clear();

            Assert.AreEqual(0, testWeightBalancedTree.Count);
            Assert.AreEqual(-1, testWeightBalancedTree.Depth);
            Dictionary<Int32, WeightBalancedTreeNode<Int32>> allNodes = PutAllNodesInDictionary(testWeightBalancedTree);
            Assert.AreEqual(0, allNodes.Count);

            // Test that new items can be added to the tree after clearing
            testWeightBalancedTree.Add(2);
            testWeightBalancedTree.Add(1);
            testWeightBalancedTree.Add(3);

            Assert.AreEqual(3, testWeightBalancedTree.Count);
            Assert.AreEqual(1, testWeightBalancedTree.Depth);
            allNodes = PutAllNodesInDictionary(testWeightBalancedTree);
            Assert.AreEqual(3, allNodes.Count);
        }

        /// <summary>
        /// Success tests for the Clear() method in a balanced tree.
        /// </summary>
        [Test]
        public void Clear_Balanced()
        {
            testWeightBalancedTree = new WeightBalancedTree<Int32>(true);
            foreach (Int32 currentItem in Enumerable.Range(1, 3))
            {
                testWeightBalancedTree.Add(currentItem);
            }

            testWeightBalancedTree.Clear();

            Assert.AreEqual(-1, testWeightBalancedTree.Depth);
            Assert.AreEqual(0, testWeightBalancedTree.Count);
            Dictionary<Int32, WeightBalancedTreeNode<Int32>> allNodes = PutAllNodesInDictionary(testWeightBalancedTree);
            Assert.AreEqual(0, allNodes.Count);

            // Test that tree continues to balance after Clear() is called
            foreach (Int32 currentItem in Enumerable.Range(1, 3))
            {
                testWeightBalancedTree.Add(currentItem);
            }
            allNodes = PutAllNodesInDictionary(testWeightBalancedTree);
            Assert.AreEqual(3, allNodes.Count);
            Assert.IsNull(allNodes[2].ParentNode);
            Assert.AreSame(allNodes[1], allNodes[2].LeftChildNode);
            Assert.AreSame(allNodes[3], allNodes[2].RightChildNode);
            Assert.AreSame(allNodes[2], allNodes[1].ParentNode);
            Assert.IsNull(allNodes[1].LeftChildNode);
            Assert.IsNull(allNodes[1].RightChildNode);
            Assert.AreSame(allNodes[2], allNodes[3].ParentNode);
            Assert.IsNull(allNodes[3].LeftChildNode);
            Assert.IsNull(allNodes[3].RightChildNode);
            Assert.AreEqual(1, allNodes[2].LeftSubtreeSize);
            Assert.AreEqual(1, allNodes[2].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[1].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[1].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[3].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[3].RightSubtreeSize);
        }

        /// <summary>
        /// Success tests for the PreOrderDepthFirstSearch() method.
        /// </summary>
        [Test]
        public void PreOrderDepthFirstSearch()
        {
            // Test that nothing is done when the tree is empty
            List<Int32> preOrderItems = new List<Int32>();
            Action<WeightBalancedTreeNode<Int32>> addNodesToListAction = (node) =>
            {
                preOrderItems.Add(node.Item);
            };

            testWeightBalancedTree.PreOrderDepthFirstSearch(addNodesToListAction);

            Assert.AreEqual(0, preOrderItems.Count);


            // Create the following tree...
            //      4
            //     / \
            //   2     6
            //  / \   / \
            // 1   3 5   7
            Int32[] treeItems = new Int32[] { 4, 2, 6, 1, 3, 5, 7 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            testWeightBalancedTree.PreOrderDepthFirstSearch(addNodesToListAction);

            Assert.AreEqual(4, preOrderItems[0]);
            Assert.AreEqual(2, preOrderItems[1]);
            Assert.AreEqual(1, preOrderItems[2]);
            Assert.AreEqual(3, preOrderItems[3]);
            Assert.AreEqual(6, preOrderItems[4]);
            Assert.AreEqual(5, preOrderItems[5]);
            Assert.AreEqual(7, preOrderItems[6]);
        }

        /// <summary>
        /// Success tests for the InOrderDepthFirstSearch() method.
        /// </summary>
        [Test]
        public void InOrderDepthFirstSearch()
        {
            // Test that nothing is done when the tree is empty
            List<Int32> inOrderItems = new List<Int32>();
            Action<WeightBalancedTreeNode<Int32>> addNodesToListAction = (node) =>
            {
                inOrderItems.Add(node.Item);
            };

            testWeightBalancedTree.InOrderDepthFirstSearch(addNodesToListAction);

            Assert.AreEqual(0, inOrderItems.Count);


            // Create the following tree...
            //      4
            //     / \
            //   2     6
            //  / \   / \
            // 1   3 5   7
            Int32[] treeItems = new Int32[] { 4, 2, 6, 1, 3, 5, 7 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            testWeightBalancedTree.InOrderDepthFirstSearch(addNodesToListAction);

            Assert.AreEqual(1, inOrderItems[0]);
            Assert.AreEqual(2, inOrderItems[1]);
            Assert.AreEqual(3, inOrderItems[2]);
            Assert.AreEqual(4, inOrderItems[3]);
            Assert.AreEqual(5, inOrderItems[4]);
            Assert.AreEqual(6, inOrderItems[5]);
            Assert.AreEqual(7, inOrderItems[6]);
        }

        /// <summary>
        /// Success tests for the PostOrderDepthFirstSearch() method.
        /// </summary>
        [Test]
        public void PostOrderDepthFirstSearch()
        {
            // Test that nothing is done when the tree is empty
            List<Int32> postOrderItems = new List<Int32>();
            Action<WeightBalancedTreeNode<Int32>> addNodesToListAction = (node) =>
            {
                postOrderItems.Add(node.Item);
            };

            testWeightBalancedTree.PostOrderDepthFirstSearch(addNodesToListAction);

            Assert.AreEqual(0, postOrderItems.Count);


            // Create the following tree...
            //      4
            //     / \
            //   2     6
            //  / \   / \
            // 1   3 5   7
            Int32[] treeItems = new Int32[] { 4, 2, 6, 1, 3, 5, 7 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            testWeightBalancedTree.PostOrderDepthFirstSearch(addNodesToListAction);

            Assert.AreEqual(1, postOrderItems[0]);
            Assert.AreEqual(3, postOrderItems[1]);
            Assert.AreEqual(2, postOrderItems[2]);
            Assert.AreEqual(5, postOrderItems[3]);
            Assert.AreEqual(7, postOrderItems[4]);
            Assert.AreEqual(6, postOrderItems[5]);
            Assert.AreEqual(4, postOrderItems[6]);


            // Create the following tree...
            // 4
            //  \
            //   6
            //    \
            //     7
            testWeightBalancedTree = new WeightBalancedTree<Int32>(false);
            treeItems = new Int32[] { 4, 6, 7 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }
            postOrderItems.Clear();

            testWeightBalancedTree.PostOrderDepthFirstSearch(addNodesToListAction);
            Assert.AreEqual(7, postOrderItems[0]);
            Assert.AreEqual(6, postOrderItems[1]);
            Assert.AreEqual(4, postOrderItems[2]);


            // Create the following tree...
            //     4
            //    /
            //   2
            //  /  
            // 1    
            testWeightBalancedTree = new WeightBalancedTree<Int32>(false);
            treeItems = new Int32[] { 4, 2, 1 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }
            postOrderItems.Clear();

            testWeightBalancedTree.PostOrderDepthFirstSearch(addNodesToListAction);
            Assert.AreEqual(1, postOrderItems[0]);
            Assert.AreEqual(2, postOrderItems[1]);
            Assert.AreEqual(4, postOrderItems[2]);
        }

        /// <summary>
        /// Success tests for the BreadthFirstSearch() method.
        /// </summary>
        [Test]
        public void BreadthFirstSearch()
        {
            // Test that nothing is done when the tree is empty
            List<Int32> breadthFirstItems = new List<Int32>();
            Action<WeightBalancedTreeNode<Int32>> addNodesToListAction = (node) =>
            {
                breadthFirstItems.Add(node.Item);
            };

            testWeightBalancedTree.BreadthFirstSearch(addNodesToListAction);

            Assert.AreEqual(0, breadthFirstItems.Count);


            // Create the following tree...
            //      4
            //     / \
            //   2     6
            //  / \   / \
            // 1   3 5   7
            Int32[] treeItems = new Int32[] { 4, 2, 6, 1, 3, 5, 7 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            testWeightBalancedTree.BreadthFirstSearch(addNodesToListAction);

            Assert.AreEqual(4, breadthFirstItems[0]);
            Assert.AreEqual(2, breadthFirstItems[1]);
            Assert.AreEqual(6, breadthFirstItems[2]);
            Assert.AreEqual(1, breadthFirstItems[3]);
            Assert.AreEqual(3, breadthFirstItems[4]);
            Assert.AreEqual(5, breadthFirstItems[5]);
            Assert.AreEqual(7, breadthFirstItems[6]);
        }

        /// <summary>
        /// Success tests for the Contains() method.
        /// </summary>
        [Test]
        public void Contains()
        {
            // Test case when the tree is empty
            Assert.AreEqual(false, testWeightBalancedTree.Contains(1));

            // Create the following tree...
            //      4
            //     / \
            //    2   6
            Int32[] treeItems = new Int32[] { 4, 2, 6 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            Assert.AreEqual(false, testWeightBalancedTree.Contains(1));
            Assert.AreEqual(true, testWeightBalancedTree.Contains(2));
            Assert.AreEqual(false, testWeightBalancedTree.Contains(3));
            Assert.AreEqual(true, testWeightBalancedTree.Contains(4));
            Assert.AreEqual(false, testWeightBalancedTree.Contains(5));
            Assert.AreEqual(true, testWeightBalancedTree.Contains(6));
            Assert.AreEqual(false, testWeightBalancedTree.Contains(7));
        }

        /// <summary>
        /// Success tests for the GetNextLessThan() method.
        /// </summary>
        [Test]
        public void GetNextLessThan()
        {
            // Test case when the tree is empty
            Tuple<Boolean, Int32> result = testWeightBalancedTree.GetNextLessThan(50);
            Assert.AreEqual(false, result.Item1);
            Assert.AreEqual(default (Int32), result.Item2);

            // Create the following tree which allows for testing of all code paths...
            //       10
            //     /    \
            //    8     15
            //   / \    /
            //  6   9  11
            //          \
            //           14
            Int32[] treeItems = new Int32[] { 10, 8, 15, 6, 9, 11, 14 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            // Test cases where values exist in the tree
            result = testWeightBalancedTree.GetNextLessThan(15);
            Assert.AreEqual(true, result.Item1);
            Assert.AreEqual(14, result.Item2);

            result = testWeightBalancedTree.GetNextLessThan(11);
            Assert.AreEqual(true, result.Item1);
            Assert.AreEqual(10, result.Item2);

            // Test cases where values do not exist in the tree
            result = testWeightBalancedTree.GetNextLessThan(13);
            Assert.AreEqual(true, result.Item1);
            Assert.AreEqual(11, result.Item2);

            result = testWeightBalancedTree.GetNextLessThan(16);
            Assert.AreEqual(true, result.Item1);
            Assert.AreEqual(15, result.Item2);

            // Test where no lower values exist
            result = testWeightBalancedTree.GetNextLessThan(6);
            Assert.AreEqual(false, result.Item1);
            Assert.AreEqual(default(Int32), result.Item2);

            result = testWeightBalancedTree.GetNextLessThan(5);
            Assert.AreEqual(false, result.Item1);
            Assert.AreEqual(default(Int32), result.Item2);
        }

        /// <summary>
        /// Success tests for the GetNextLessThan() method.
        /// </summary>
        [Test]
        public void GetNextGreaterThan()
        {
            // Test case when the tree is empty
            Tuple<Boolean, Int32> result = testWeightBalancedTree.GetNextLessThan(50);
            Assert.AreEqual(false, result.Item1);
            Assert.AreEqual(default(Int32), result.Item2);

            // Create the following tree which allows for testing of all code paths...
            //       10
            //     /    \
            //    2     15
            //     \   /  \
            //      7 13  18
            //     /
            //    5
            Int32[] treeItems = new Int32[] { 10, 2, 15, 7, 13, 18, 5 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            // Test cases where values exist in the tree
            result = testWeightBalancedTree.GetNextGreaterThan(2);
            Assert.AreEqual(true, result.Item1);
            Assert.AreEqual(5, result.Item2);

            result = testWeightBalancedTree.GetNextGreaterThan(7);
            Assert.AreEqual(true, result.Item1);
            Assert.AreEqual(10, result.Item2);

            // Test cases where values do not exist in the tree
            result = testWeightBalancedTree.GetNextGreaterThan(6);
            Assert.AreEqual(true, result.Item1);
            Assert.AreEqual(7, result.Item2);

            result = testWeightBalancedTree.GetNextGreaterThan(1);
            Assert.AreEqual(true, result.Item1);
            Assert.AreEqual(2, result.Item2);

            // Test where no lower values exist
            result = testWeightBalancedTree.GetNextGreaterThan(18);
            Assert.AreEqual(false, result.Item1);
            Assert.AreEqual(default(Int32), result.Item2);

            result = testWeightBalancedTree.GetNextGreaterThan(19);
            Assert.AreEqual(false, result.Item1);
            Assert.AreEqual(default(Int32), result.Item2);
        }

        /// <summary>
        /// Success tests for the GetCountLessThan() method.
        /// </summary>
        [Test]
        public void GetCountLessThan()
        {
            // Test case when the tree is empty
            Int32 result = testWeightBalancedTree.GetCountLessThan(Int32.MaxValue);
            Assert.AreEqual(0, result);

            // Create the following tree...
            //                  10
            //           /              \
            //      4                          34
            //    /   \                     /      \
            //  1       6               28            45
            //   \     / \            /    \       /      \
            //    2   5   8         21      33    39      48
            //     \       \       /  \     /    /  \    /  \
            //      3       9     14  27   30   38  40  46  49
            //                   /  \           /    \   \
            //                 11    19        36    43  47
            //                   \   /        /  \   /
            //                   13 15       35  37 42
            Int32[] treeItems = new Int32[] { 10, 4, 34, 1, 6, 28, 45, 2, 5, 8, 21, 33, 39, 48, 3, 9, 14, 27, 30, 38, 40, 46, 49, 11, 19, 36, 43, 47, 13, 15, 35, 37, 42 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }
            Assert.AreEqual(21, testWeightBalancedTree.GetCountLessThan(36));
            Assert.AreEqual(0, testWeightBalancedTree.GetCountLessThan(1));
            Assert.AreEqual(32, testWeightBalancedTree.GetCountLessThan(49));
            Assert.AreEqual(25, testWeightBalancedTree.GetCountLessThan(40));
            Assert.AreEqual(6, testWeightBalancedTree.GetCountLessThan(8));
            Assert.AreEqual(14, testWeightBalancedTree.GetCountLessThan(20));
            Assert.AreEqual(33, testWeightBalancedTree.GetCountLessThan(50));
            Assert.AreEqual(0, testWeightBalancedTree.GetCountLessThan(-1));
            Assert.AreEqual(6, testWeightBalancedTree.GetCountLessThan(7));
        }

        /// <summary>
        /// Success tests for the GetCountGreaterThan() method.
        /// </summary>
        [Test]
        public void GetCountGreaterThan()
        {
            // Test case when the tree is empty
            Int32 result = testWeightBalancedTree.GetCountGreaterThan(Int32.MaxValue);
            Assert.AreEqual(0, result);

            // Create the following tree...
            //                  10
            //           /              \
            //      4                          34
            //    /   \                     /      \
            //  1       6               28            45
            //   \     / \            /    \       /      \
            //    2   5   8         21      33    39      48
            //     \       \       /  \     /    /  \    /  \
            //      3       9     14  27   30   38  40  46  49
            //                   /  \           /    \   \
            //                 11    19        36    43  47
            //                   \   /        /  \   /
            //                   13 15       35  37 42
            Int32[] treeItems = new Int32[] { 10, 4, 34, 1, 6, 28, 45, 2, 5, 8, 21, 33, 39, 48, 3, 9, 14, 27, 30, 38, 40, 46, 49, 11, 19, 36, 43, 47, 13, 15, 35, 37, 42 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            Assert.AreEqual(11, testWeightBalancedTree.GetCountGreaterThan(36));
            Assert.AreEqual(32, testWeightBalancedTree.GetCountGreaterThan(1));
            Assert.AreEqual(0, testWeightBalancedTree.GetCountGreaterThan(49));
            Assert.AreEqual(7, testWeightBalancedTree.GetCountGreaterThan(40));
            Assert.AreEqual(26, testWeightBalancedTree.GetCountGreaterThan(8));
            Assert.AreEqual(19, testWeightBalancedTree.GetCountGreaterThan(20));
            Assert.AreEqual(0, testWeightBalancedTree.GetCountGreaterThan(50));
            Assert.AreEqual(33, testWeightBalancedTree.GetCountGreaterThan(-1));
            Assert.AreEqual(27, testWeightBalancedTree.GetCountGreaterThan(7));
        }

        /// <summary>
        /// Success tests for the GetAllLessThan() method.
        /// </summary>
        [Test]
        public void GetAllLessThan()
        {
            // Test case when the tree is empty
            IEnumerable<Int32> result = testWeightBalancedTree.GetAllLessThan(5);
            Assert.Zero(result.Count<Int32>());

            // Create the following tree which allows for testing of all code paths...
            //       10
            //     /    \
            //    8     15
            //   / \    /
            //  6   9  11
            //          \
            //           14
            Int32[] treeItems = new Int32[] { 10, 8, 15, 6, 9, 11, 14 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            // Test case where specified item doesn't exist and there are no lower items
            result = testWeightBalancedTree.GetAllLessThan(5);
            Assert.Zero(result.Count<Int32>());

            // Test case where specified item exists but there are no lower items
            result = testWeightBalancedTree.GetAllLessThan(6);
            Assert.Zero(result.Count<Int32>());

            // Test case where specified item doesn't exist but there are lower items
            List<Int32> listResult = new List<Int32>(testWeightBalancedTree.GetAllLessThan(12));
            Assert.AreEqual(5, listResult.Count);
            Assert.AreEqual(11, listResult[0]);
            Assert.AreEqual(10, listResult[1]);
            Assert.AreEqual(9, listResult[2]);
            Assert.AreEqual(8, listResult[3]);
            Assert.AreEqual(6, listResult[4]);

            // Test case where specified item exists and there are lower items
            listResult = new List<Int32>(testWeightBalancedTree.GetAllLessThan(14));
            Assert.AreEqual(5, listResult.Count);
            Assert.AreEqual(11, listResult[0]);
            Assert.AreEqual(10, listResult[1]);
            Assert.AreEqual(9, listResult[2]);
            Assert.AreEqual(8, listResult[3]);
            Assert.AreEqual(6, listResult[4]);

            listResult = new List<Int32>(testWeightBalancedTree.GetAllLessThan(11));
            Assert.AreEqual(4, listResult.Count);
            Assert.AreEqual(10, listResult[0]);
            Assert.AreEqual(9, listResult[1]);
            Assert.AreEqual(8, listResult[2]);
            Assert.AreEqual(6, listResult[3]);

            // Test case where specified item is higher than the largest item
            listResult = new List<Int32>(testWeightBalancedTree.GetAllLessThan(20));
            Assert.AreEqual(7, listResult.Count);
            Assert.AreEqual(15, listResult[0]);
            Assert.AreEqual(14, listResult[1]);
            Assert.AreEqual(11, listResult[2]);
            Assert.AreEqual(10, listResult[3]);
            Assert.AreEqual(9, listResult[4]);
            Assert.AreEqual(8, listResult[5]);
            Assert.AreEqual(6, listResult[6]);
        }

        /// <summary>
        /// Success tests for the GetAllGreaterThan() method.
        /// </summary>
        [Test]
        public void GetAllGreaterThan()
        {
            // Test case when the tree is empty
            IEnumerable<Int32> result = testWeightBalancedTree.GetAllGreaterThan(5);
            Assert.Zero(result.Count<Int32>());

            // Create the following tree which allows for testing of all code paths...
            //       10
            //     /    \
            //    2     15
            //     \   /  \
            //      7 13  18
            //     /
            //    5
            Int32[] treeItems = new Int32[] { 10, 2, 15, 7, 13, 18, 5 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            // Test case where specified item doesn't exist and there are no greater items
            result = testWeightBalancedTree.GetAllGreaterThan(19);
            Assert.Zero(result.Count<Int32>());

            // Test case where specified item exists but there are no greater items
            result = testWeightBalancedTree.GetAllGreaterThan(18);
            Assert.Zero(result.Count<Int32>());

            // Test case where specified item doesn't exist but there are greater items
            List<Int32> listResult = new List<Int32>(testWeightBalancedTree.GetAllGreaterThan(6));
            Assert.AreEqual(5, listResult.Count);
            Assert.AreEqual(7, listResult[0]);
            Assert.AreEqual(10, listResult[1]);
            Assert.AreEqual(13, listResult[2]);
            Assert.AreEqual(15, listResult[3]);
            Assert.AreEqual(18, listResult[4]);

            // Test case where specified item exists and there are greater items
            listResult = new List<Int32>(testWeightBalancedTree.GetAllGreaterThan(5));
            Assert.AreEqual(5, listResult.Count);
            Assert.AreEqual(7, listResult[0]);
            Assert.AreEqual(10, listResult[1]);
            Assert.AreEqual(13, listResult[2]);
            Assert.AreEqual(15, listResult[3]);
            Assert.AreEqual(18, listResult[4]);

            listResult = new List<Int32>(testWeightBalancedTree.GetAllGreaterThan(7));
            Assert.AreEqual(4, listResult.Count);
            Assert.AreEqual(10, listResult[0]);
            Assert.AreEqual(13, listResult[1]);
            Assert.AreEqual(15, listResult[2]);
            Assert.AreEqual(18, listResult[3]);

            // Test case where specified item is lower than the lowest item
            listResult = new List<Int32>(testWeightBalancedTree.GetAllGreaterThan(1));
            Assert.AreEqual(7, listResult.Count);
            Assert.AreEqual(2, listResult[0]);
            Assert.AreEqual(5, listResult[1]);
            Assert.AreEqual(7, listResult[2]);
            Assert.AreEqual(10, listResult[3]);
            Assert.AreEqual(13, listResult[4]);
            Assert.AreEqual(15, listResult[5]);
            Assert.AreEqual(18, listResult[6]);
        }

        /// <summary>
        /// Tests that an exception is thrown when the Remove() method is called on an empty tree.
        /// </summary>
        [Test]
        public void Remove_TreeIsEmpty()
        {
            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                testWeightBalancedTree.Remove(50);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The specified item ('50') does not exist in the tree."));
            Assert.AreEqual("item", e.ParamName);
        }

        /// <summary>
        /// Tests that an exception is thrown when the Remove() method is called with an 'item' parameter that does not exist in the tree.
        /// </summary>
        [Test]
        public void Remove_ItemDoesNotExist()
        {
            // Create the following tree...
            //     7
            //    / \
            //   5   9
            //  / \
            // 3   6
            Int32[] treeItems = new Int32[] { 7, 5, 9, 3, 6 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            // Test when 'item' should be on the left of a node
            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                testWeightBalancedTree.Remove(2);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The specified item ('2') does not exist in the tree."));
            Assert.AreEqual("item", e.ParamName);

            // Check that the subtree counts have been correctly incremented
            Dictionary<Int32, Tuple<Int32, Int32>> subtreeValueStore = new Dictionary<Int32, Tuple<Int32, Int32>>();
            Action<WeightBalancedTreeNode<Int32>> populateStoreAction = (node) =>
            {
                subtreeValueStore.Add(node.Item, new Tuple<Int32, Int32>(node.LeftSubtreeSize, node.RightSubtreeSize));
            };
            testWeightBalancedTree.BreadthFirstSearch(populateStoreAction);
            Assert.AreEqual(5, subtreeValueStore.Count);
            Assert.AreEqual(3, subtreeValueStore[7].Item1);
            Assert.AreEqual(1, subtreeValueStore[7].Item2);
            Assert.AreEqual(1, subtreeValueStore[5].Item1);
            Assert.AreEqual(1, subtreeValueStore[5].Item2);
            Assert.AreEqual(0, subtreeValueStore[3].Item1);
            Assert.AreEqual(0, subtreeValueStore[3].Item2);


            // Test when 'item' should be on the right of a node
            e = Assert.Throws<ArgumentException>(delegate
            {
                testWeightBalancedTree.Remove(4);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The specified item ('4') does not exist in the tree."));
            Assert.AreEqual("item", e.ParamName);

            // Check that the subtree counts have been correctly incremented
            subtreeValueStore.Clear();
            testWeightBalancedTree.BreadthFirstSearch(populateStoreAction);
            Assert.AreEqual(5, subtreeValueStore.Count);
            Assert.AreEqual(3, subtreeValueStore[7].Item1);
            Assert.AreEqual(1, subtreeValueStore[7].Item2);
            Assert.AreEqual(1, subtreeValueStore[5].Item1);
            Assert.AreEqual(1, subtreeValueStore[5].Item2);
            Assert.AreEqual(0, subtreeValueStore[3].Item1);
            Assert.AreEqual(0, subtreeValueStore[3].Item2);
        }

        /// <summary>
        /// Success tests for the Remove() method in an unbalanced tree.
        /// </summary>
        [Test]
        public void Remove_Unbalanced()
        {
            Dictionary<Int32, WeightBalancedTreeNode<Int32>> allNodes = new Dictionary<Int32, WeightBalancedTreeNode<Int32>>();
            Dictionary<Int32, Tuple<Int32, Int32>> subtreeValueStore = new Dictionary<Int32, Tuple<Int32, Int32>>();
            Action<WeightBalancedTreeNode<Int32>> populateStoreAction = (node) =>
            {
                allNodes.Add(node.Item, node);
                subtreeValueStore.Add(node.Item, new Tuple<Int32, Int32>(node.LeftSubtreeSize, node.RightSubtreeSize));
            };

            testWeightBalancedTree.Add(20);
            testWeightBalancedTree.Remove(20);

            testWeightBalancedTree.BreadthFirstSearch(populateStoreAction);
            Assert.AreEqual(0, allNodes.Count);
            Assert.AreEqual(0, testWeightBalancedTree.Count);


            // Create the following tree...
            //       20
            //      /  \
            //     10  22
            //    /  \
            //   7    13
            //  /      \
            // 5        17
            testWeightBalancedTree = new WeightBalancedTree<Int32>(false);
            Int32[] treeItems = new Int32[] { 20, 10, 22, 7, 13, 5, 17 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            testWeightBalancedTree.Remove(5);

            allNodes.Clear();
            subtreeValueStore.Clear();
            testWeightBalancedTree.BreadthFirstSearch(populateStoreAction);
            Assert.AreEqual(6, subtreeValueStore.Count);
            Assert.AreEqual(4, subtreeValueStore[20].Item1);
            Assert.AreEqual(1, subtreeValueStore[10].Item1);
            Assert.AreEqual(0, subtreeValueStore[7].Item1);


            testWeightBalancedTree = new WeightBalancedTree<Int32>(false);
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            testWeightBalancedTree.Remove(7);

            allNodes.Clear();
            subtreeValueStore.Clear();
            testWeightBalancedTree.BreadthFirstSearch(populateStoreAction);
            Assert.AreEqual(6, subtreeValueStore.Count);
            Assert.AreEqual(4, subtreeValueStore[20].Item1);
            Assert.AreEqual(1, subtreeValueStore[10].Item1);
            Assert.AreSame(allNodes[5], allNodes[10].LeftChildNode);
            Assert.AreSame(allNodes[10], allNodes[5].ParentNode);


            testWeightBalancedTree.Remove(13);

            allNodes.Clear();
            subtreeValueStore.Clear();
            testWeightBalancedTree.BreadthFirstSearch(populateStoreAction);
            Assert.AreEqual(5, subtreeValueStore.Count);
            Assert.AreEqual(3, subtreeValueStore[20].Item1);
            Assert.AreEqual(1, subtreeValueStore[10].Item2);
            Assert.AreSame(allNodes[17], allNodes[10].RightChildNode);
            Assert.AreSame(allNodes[10], allNodes[17].ParentNode);


            // Create the following tree...
            //       20
            //      /  
            //     10 
            //    / 
            //   7 
            testWeightBalancedTree = new WeightBalancedTree<Int32>(false);
            treeItems = new Int32[] { 20, 10, 7 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            testWeightBalancedTree.Remove(20);

            allNodes.Clear();
            subtreeValueStore.Clear();
            testWeightBalancedTree.BreadthFirstSearch(populateStoreAction);
            Assert.AreEqual(2, subtreeValueStore.Count);
            Assert.AreEqual(1, subtreeValueStore[10].Item1);
            Assert.AreSame(allNodes[7], allNodes[10].LeftChildNode);
            Assert.IsNull(allNodes[10].ParentNode);


            // Create the following tree...
            // 7
            //  \  
            //   10 
            //    \ 
            //     20 
            testWeightBalancedTree = new WeightBalancedTree<Int32>(false);
            treeItems = new Int32[] { 7, 10, 20};
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            testWeightBalancedTree.Remove(7);

            allNodes.Clear();
            subtreeValueStore.Clear();
            testWeightBalancedTree.BreadthFirstSearch(populateStoreAction);
            Assert.AreEqual(2, subtreeValueStore.Count);
            Assert.AreEqual(1, subtreeValueStore[10].Item2);
            Assert.AreSame(allNodes[20], allNodes[10].RightChildNode);
            Assert.IsNull(allNodes[10].ParentNode);


            // Create the following tree...
            //   3
            //    \
            //     9 
            //    / 
            //   8 
            testWeightBalancedTree = new WeightBalancedTree<Int32>(false);
            treeItems = new Int32[] { 3, 9, 8 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            testWeightBalancedTree.Remove(9);

            allNodes.Clear();
            subtreeValueStore.Clear();
            testWeightBalancedTree.BreadthFirstSearch(populateStoreAction);
            Assert.AreEqual(2, subtreeValueStore.Count);
            Assert.AreEqual(1, subtreeValueStore[3].Item2);
            Assert.AreSame(allNodes[8], allNodes[3].RightChildNode);
            Assert.AreSame(allNodes[3], allNodes[8].ParentNode);


            // Create the following tree...
            //     3
            //    / 
            //   1   
            //    \ 
            //     2 
            testWeightBalancedTree = new WeightBalancedTree<Int32>(false);
            treeItems = new Int32[] { 3, 1, 2 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            testWeightBalancedTree.Remove(1);

            allNodes.Clear();
            subtreeValueStore.Clear();
            testWeightBalancedTree.BreadthFirstSearch(populateStoreAction);
            Assert.AreEqual(2, subtreeValueStore.Count);
            Assert.AreEqual(1, subtreeValueStore[3].Item1);
            Assert.AreSame(allNodes[2], allNodes[3].LeftChildNode);
            Assert.AreSame(allNodes[3], allNodes[2].ParentNode);
            

            // Create the following tree...
            //                40
            //               /  \
            //             35    45
            //            /  \
            //          20    37
            //        /    \
            //     10        25
            //    /  \      /  \
            //   5   12    22   26
            //        \    /
            //        14  21
            testWeightBalancedTree = new WeightBalancedTree<Int32>(false);
            treeItems = new Int32[] { 40, 35, 45, 20, 37, 10, 25, 5, 12, 22, 26, 14, 21 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            // Expect that node holding 20 is swapped with node holding 21, as 20 subtree sizes are equal
            testWeightBalancedTree.Remove(20);

            allNodes.Clear();
            subtreeValueStore.Clear();
            testWeightBalancedTree.BreadthFirstSearch(populateStoreAction);
            Assert.IsFalse(allNodes.ContainsKey(20));
            Assert.AreEqual(12, subtreeValueStore.Count);
            Assert.AreEqual(10, subtreeValueStore[40].Item1);
            Assert.AreEqual(8, subtreeValueStore[35].Item1);
            Assert.AreEqual(4, subtreeValueStore[21].Item1);
            Assert.AreEqual(3, subtreeValueStore[21].Item2);
            Assert.AreEqual(1, subtreeValueStore[25].Item1);
            Assert.AreEqual(0, subtreeValueStore[22].Item1);
            Assert.AreEqual(0, subtreeValueStore[14].Item1);
            Assert.AreEqual(0, subtreeValueStore[14].Item2);
            Assert.AreSame(allNodes[21], allNodes[35].LeftChildNode);
            Assert.AreSame(allNodes[35], allNodes[21].ParentNode);
            Assert.AreSame(allNodes[10], allNodes[21].LeftChildNode);
            Assert.AreSame(allNodes[21], allNodes[10].ParentNode);
            Assert.AreSame(allNodes[25], allNodes[21].RightChildNode);
            Assert.AreSame(allNodes[21], allNodes[25].ParentNode);
            Assert.IsNull(allNodes[22].LeftChildNode);
            Assert.IsNull(allNodes[14].LeftChildNode);
            Assert.IsNull(allNodes[14].RightChildNode);
            Assert.AreSame(allNodes[12], allNodes[14].ParentNode);

            // Create the following tree...
            //                40
            //               /  \
            //             35    45
            //            /  \
            //          21    37
            //        /    \
            //     10        25
            //    /  \      /  \
            //   5   12    22   26
            //        \    
            //        14  
            // Expect that node holding 21 is swapped with node holding 14, as size of left subtree of 21 is greater than right subtree
            testWeightBalancedTree.Remove(21);

            allNodes.Clear();
            subtreeValueStore.Clear();
            testWeightBalancedTree.BreadthFirstSearch(populateStoreAction);
            Assert.IsFalse(allNodes.ContainsKey(20));
            Assert.AreEqual(11, subtreeValueStore.Count);
            Assert.AreEqual(9, subtreeValueStore[40].Item1);
            Assert.AreEqual(7, subtreeValueStore[35].Item1);
            Assert.AreEqual(3, subtreeValueStore[14].Item1);
            Assert.AreEqual(3, subtreeValueStore[14].Item2);
            Assert.AreEqual(1, subtreeValueStore[10].Item2);
            Assert.AreEqual(0, subtreeValueStore[12].Item2);
            Assert.AreEqual(0, subtreeValueStore[22].Item1);
            Assert.AreEqual(0, subtreeValueStore[22].Item2);
            Assert.AreSame(allNodes[14], allNodes[35].LeftChildNode);
            Assert.AreSame(allNodes[35], allNodes[14].ParentNode);
            Assert.AreSame(allNodes[10], allNodes[14].LeftChildNode);
            Assert.AreSame(allNodes[14], allNodes[10].ParentNode);
            Assert.AreSame(allNodes[25], allNodes[14].RightChildNode);
            Assert.AreSame(allNodes[14], allNodes[25].ParentNode);
            Assert.IsNull(allNodes[12].RightChildNode);
            Assert.IsNull(allNodes[22].LeftChildNode);
            Assert.IsNull(allNodes[22].RightChildNode);
            Assert.AreSame(allNodes[25], allNodes[22].ParentNode);


            // Create the following tree...
            //                40
            //               /  \
            //             35    45
            //            /  \
            //          14    37
            //        /    \
            //     10        25
            //    /  \      /  \
            //   5   12    22  26
            //                   \    
            //                   27  
            // Expect that node holding 14 is swapped with node holding 22, as size of right subtree of 14 is greater than left subtree
            testWeightBalancedTree.Add(27);
            testWeightBalancedTree.Remove(14);

            allNodes.Clear();
            subtreeValueStore.Clear();
            testWeightBalancedTree.BreadthFirstSearch(populateStoreAction);
            Assert.IsFalse(allNodes.ContainsKey(20));
            Assert.AreEqual(11, subtreeValueStore.Count);
            Assert.AreEqual(9, subtreeValueStore[40].Item1);
            Assert.AreEqual(7, subtreeValueStore[35].Item1);
            Assert.AreEqual(3, subtreeValueStore[22].Item1);
            Assert.AreEqual(3, subtreeValueStore[22].Item2);
            Assert.AreEqual(0, subtreeValueStore[25].Item1);
            Assert.AreEqual(2, subtreeValueStore[25].Item2);
            Assert.AreEqual(0, subtreeValueStore[12].Item1);
            Assert.AreEqual(0, subtreeValueStore[12].Item2);
            Assert.AreSame(allNodes[22], allNodes[35].LeftChildNode);
            Assert.AreSame(allNodes[35], allNodes[22].ParentNode);
            Assert.AreSame(allNodes[10], allNodes[22].LeftChildNode);
            Assert.AreSame(allNodes[22], allNodes[10].ParentNode);
            Assert.AreSame(allNodes[25], allNodes[22].RightChildNode);
            Assert.AreSame(allNodes[22], allNodes[25].ParentNode);
            Assert.IsNull(allNodes[25].LeftChildNode);
            Assert.IsNull(allNodes[12].LeftChildNode);
            Assert.IsNull(allNodes[12].RightChildNode);
            Assert.AreSame(allNodes[10], allNodes[12].ParentNode);
        }

        /// <summary>
        /// Tests that balance is maintained when the Remove() method is called removing a node with no children.
        /// </summary>
        [Test]
        public void Remove_BalancedRemoveNodeWithNoChildren()
        {
            // Test by removing node 3 in the following tree...
            //      4
            //     / 
            //    2 
            //   / \
            //  1   3 
            // 
            // ...and expect the result to look like...
            //      2
            //     / \
            //    1   4
            testWeightBalancedTree = new WeightBalancedTree<Int32>(true);
            Int32[] treeItems = new Int32[] { 4, 2, 3, 1 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            testWeightBalancedTree.Remove(3);

            Dictionary<Int32, WeightBalancedTreeNode<Int32>> allNodes = PutAllNodesInDictionary(testWeightBalancedTree);
            Assert.AreEqual(3, allNodes.Count);
            Assert.IsNull(allNodes[2].ParentNode);
            Assert.AreSame(allNodes[1], allNodes[2].LeftChildNode);
            Assert.AreSame(allNodes[4], allNodes[2].RightChildNode);
            Assert.AreSame(allNodes[2], allNodes[1].ParentNode);
            Assert.IsNull(allNodes[1].LeftChildNode);
            Assert.IsNull(allNodes[1].RightChildNode);
            Assert.AreSame(allNodes[2], allNodes[4].ParentNode);
            Assert.IsNull(allNodes[4].LeftChildNode);
            Assert.IsNull(allNodes[4].RightChildNode);
            Assert.AreEqual(1, allNodes[2].LeftSubtreeSize);
            Assert.AreEqual(1, allNodes[2].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[1].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[1].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[4].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[4].RightSubtreeSize);


            // Test by removing node 2 in the following tree...
            //  1  
            //   \  
            //    3 
            //   / \
            //  2   4 
            // 
            // ...and expect the result to look like...
            //      3
            //     / \
            //    1   4
            testWeightBalancedTree.Clear();
            treeItems = new Int32[] { 1, 3, 2, 4 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            testWeightBalancedTree.Remove(2);

            allNodes = PutAllNodesInDictionary(testWeightBalancedTree);
            Assert.AreEqual(3, allNodes.Count);
            Assert.IsNull(allNodes[3].ParentNode);
            Assert.AreSame(allNodes[1], allNodes[3].LeftChildNode);
            Assert.AreSame(allNodes[4], allNodes[3].RightChildNode);
            Assert.AreSame(allNodes[3], allNodes[1].ParentNode);
            Assert.IsNull(allNodes[1].LeftChildNode);
            Assert.IsNull(allNodes[1].RightChildNode);
            Assert.AreSame(allNodes[3], allNodes[4].ParentNode);
            Assert.IsNull(allNodes[4].LeftChildNode);
            Assert.IsNull(allNodes[4].RightChildNode);
            Assert.AreEqual(1, allNodes[3].LeftSubtreeSize);
            Assert.AreEqual(1, allNodes[3].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[1].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[1].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[4].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[4].RightSubtreeSize);
        }

        /// <summary>
        /// Tests that balance is maintained when the Remove() method is called removing a node with no left child.
        /// </summary>
        [Test]
        public void Remove_BalancedRemoveNodeWithNoLeftChild()
        {
            // Test by removing node 1 in the following tree...
            //      3
            //    /   \
            //  1       5
            //   \     / \
            //    2   4   7
            //           /
            //          6
            // 
            // ...and expect the result to look like...
            //      5
            //    /   \
            //   3     7
            //  / \   /
            // 2   4 6 
            testWeightBalancedTree = new WeightBalancedTree<Int32>(true);
            Int32[] treeItems = new Int32[] { 3, 1, 5, 2, 4, 7, 6 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            testWeightBalancedTree.Remove(1);

            Dictionary<Int32, WeightBalancedTreeNode<Int32>> allNodes = PutAllNodesInDictionary(testWeightBalancedTree);
            Assert.AreEqual(6, allNodes.Count);
            Assert.IsNull(allNodes[5].ParentNode);
            Assert.AreSame(allNodes[3], allNodes[5].LeftChildNode);
            Assert.AreSame(allNodes[7], allNodes[5].RightChildNode);
            Assert.AreSame(allNodes[5], allNodes[3].ParentNode);
            Assert.AreSame(allNodes[2], allNodes[3].LeftChildNode);
            Assert.AreSame(allNodes[4], allNodes[3].RightChildNode);
            Assert.AreSame(allNodes[5], allNodes[7].ParentNode);
            Assert.AreSame(allNodes[6], allNodes[7].LeftChildNode);
            Assert.IsNull(allNodes[7].RightChildNode);
            Assert.AreSame(allNodes[3], allNodes[2].ParentNode);
            Assert.IsNull(allNodes[2].LeftChildNode);
            Assert.IsNull(allNodes[2].RightChildNode);
            Assert.AreSame(allNodes[3], allNodes[4].ParentNode);
            Assert.IsNull(allNodes[4].LeftChildNode);
            Assert.IsNull(allNodes[4].RightChildNode);
            Assert.AreSame(allNodes[7], allNodes[6].ParentNode);
            Assert.IsNull(allNodes[6].LeftChildNode);
            Assert.IsNull(allNodes[6].RightChildNode);
            Assert.AreEqual(3, allNodes[5].LeftSubtreeSize);
            Assert.AreEqual(2, allNodes[5].RightSubtreeSize);
            Assert.AreEqual(1, allNodes[3].LeftSubtreeSize);
            Assert.AreEqual(1, allNodes[3].RightSubtreeSize);
            Assert.AreEqual(1, allNodes[7].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[7].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[2].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[2].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[4].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[4].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[6].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[6].RightSubtreeSize);
        }

        /// <summary>
        /// Tests that balance is maintained when the Remove() method is called removing a node with no right child.
        /// </summary>
        [Test]
        public void Remove_BalancedRemoveNodeWithNoRightChild()
        {
            // Test by removing node 7 in the following tree...
            //       5
            //     /   \
            //   3       7
            //  / \     /
            // 1   4   6
            //  \ 
            //   2
            // 
            // ...and expect the result to look like...
            //       3
            //     /   \
            //    1     5
            //     \   / \
            //      2 4   6
            testWeightBalancedTree = new WeightBalancedTree<Int32>(true);
            Int32[] treeItems = new Int32[] { 5, 3, 7, 4, 6, 1, 2 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            testWeightBalancedTree.Remove(7);

            Dictionary<Int32, WeightBalancedTreeNode<Int32>> allNodes = PutAllNodesInDictionary(testWeightBalancedTree);
            Assert.AreEqual(6, allNodes.Count);
            Assert.IsNull(allNodes[3].ParentNode);
            Assert.AreSame(allNodes[1], allNodes[3].LeftChildNode);
            Assert.AreSame(allNodes[5], allNodes[3].RightChildNode);
            Assert.AreSame(allNodes[3], allNodes[1].ParentNode);
            Assert.IsNull(allNodes[1].LeftChildNode);
            Assert.AreSame(allNodes[2], allNodes[1].RightChildNode);
            Assert.AreSame(allNodes[3], allNodes[5].ParentNode);
            Assert.AreSame(allNodes[4], allNodes[5].LeftChildNode);
            Assert.AreSame(allNodes[6], allNodes[5].RightChildNode);
            Assert.AreSame(allNodes[1], allNodes[2].ParentNode);
            Assert.IsNull(allNodes[2].LeftChildNode);
            Assert.IsNull(allNodes[2].RightChildNode);
            Assert.AreSame(allNodes[5], allNodes[4].ParentNode);
            Assert.IsNull(allNodes[4].LeftChildNode);
            Assert.IsNull(allNodes[4].RightChildNode);
            Assert.AreSame(allNodes[5], allNodes[6].ParentNode);
            Assert.IsNull(allNodes[6].LeftChildNode);
            Assert.IsNull(allNodes[6].RightChildNode);
            Assert.AreEqual(2, allNodes[3].LeftSubtreeSize);
            Assert.AreEqual(3, allNodes[3].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[1].LeftSubtreeSize);
            Assert.AreEqual(1, allNodes[1].RightSubtreeSize);
            Assert.AreEqual(1, allNodes[5].LeftSubtreeSize);
            Assert.AreEqual(1, allNodes[5].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[2].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[2].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[4].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[4].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[6].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[6].RightSubtreeSize);
        }

        /// <summary>
        /// Tests that balance is maintained when the Remove() method is called removing a node left and right children.
        /// </summary>
        [Test]
        public void Remove_BalancedRemoveNodeWithLeftAndRightChildren()
        {
            // Test by removing node 8 in the following tree...
            //        6
            //      /   \
            //    4       8
            //   / \     / \
            //  1   5   7   9 
            //   \ 
            //    3
            //   /
            //  2
            // 
            // ...and expect the result to look like...
            //        4
            //      /   \
            //    2       6
            //   / \     / \
            //  1   3   5   9 
            //             /
            //            7
            testWeightBalancedTree = new WeightBalancedTree<Int32>(true);
            Int32[] treeItems = new Int32[] { 6, 4, 8, 1, 5, 7, 9, 3, 2 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            testWeightBalancedTree.Remove(8);

            Dictionary<Int32, WeightBalancedTreeNode<Int32>> allNodes = PutAllNodesInDictionary(testWeightBalancedTree);
            Assert.AreEqual(8, allNodes.Count);
            Assert.IsNull(allNodes[4].ParentNode);
            Assert.AreSame(allNodes[2], allNodes[4].LeftChildNode);
            Assert.AreSame(allNodes[6], allNodes[4].RightChildNode);
            Assert.AreSame(allNodes[4], allNodes[2].ParentNode);
            Assert.AreSame(allNodes[1], allNodes[2].LeftChildNode);
            Assert.AreSame(allNodes[3], allNodes[2].RightChildNode);
            Assert.AreSame(allNodes[4], allNodes[6].ParentNode);
            Assert.AreSame(allNodes[5], allNodes[6].LeftChildNode);
            Assert.AreSame(allNodes[9], allNodes[6].RightChildNode);
            Assert.AreSame(allNodes[2], allNodes[1].ParentNode);
            Assert.IsNull(allNodes[1].LeftChildNode);
            Assert.IsNull(allNodes[1].RightChildNode);
            Assert.AreSame(allNodes[2], allNodes[3].ParentNode);
            Assert.IsNull(allNodes[3].LeftChildNode);
            Assert.IsNull(allNodes[3].RightChildNode);
            Assert.AreSame(allNodes[6], allNodes[5].ParentNode);
            Assert.IsNull(allNodes[5].LeftChildNode);
            Assert.IsNull(allNodes[5].RightChildNode);
            Assert.AreSame(allNodes[6], allNodes[9].ParentNode);
            Assert.AreSame(allNodes[7], allNodes[9].LeftChildNode);
            Assert.IsNull(allNodes[9].RightChildNode);
            Assert.AreSame(allNodes[9], allNodes[7].ParentNode);
            Assert.IsNull(allNodes[7].LeftChildNode);
            Assert.IsNull(allNodes[7].RightChildNode);
            Assert.AreEqual(3, allNodes[4].LeftSubtreeSize);
            Assert.AreEqual(4, allNodes[4].RightSubtreeSize);
            Assert.AreEqual(1, allNodes[2].LeftSubtreeSize);
            Assert.AreEqual(1, allNodes[2].RightSubtreeSize);
            Assert.AreEqual(1, allNodes[6].LeftSubtreeSize);
            Assert.AreEqual(2, allNodes[6].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[1].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[1].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[3].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[3].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[5].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[5].RightSubtreeSize);
            Assert.AreEqual(1, allNodes[9].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[9].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[7].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[7].RightSubtreeSize);
        }

        /// <summary>
        /// Tests that the state of the tree is maintained when the Remove() method is called removing the root node in a balanced tree, and the root node is the only node.
        /// </summary>
        [Test]
        public void Remove_BalancedRemoveRootNode()
        {
            testWeightBalancedTree = new WeightBalancedTree<Int32>(true);
            testWeightBalancedTree.Add(1);

            testWeightBalancedTree.Remove(1);

            Dictionary<Int32, WeightBalancedTreeNode<Int32>> allNodes = PutAllNodesInDictionary(testWeightBalancedTree);
            Assert.AreEqual(0, allNodes.Count);
        }

        /// <summary>
        /// Tests that an exception is thrown when the Min property accessed on an empty tree.
        /// </summary>
        [Test]
        public void Min_TreeIsEmpty()
        {
            InvalidOperationException e = Assert.Throws<InvalidOperationException>(delegate
            {
                Int32 result = testWeightBalancedTree.Min;
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The tree is empty."));
        }

        /// <summary>
        /// Success tests for the Min property.
        /// </summary>
        [Test]
        public void Min()
        {
            // Create the following tree...
            //     7
            //      \
            //       9
            //      / \
            //     8   10
            foreach (Int32 currentItem in new Int32[] { 7, 9, 8, 10 })
            {
                testWeightBalancedTree.Add(currentItem);
            }

            Assert.AreEqual(7, testWeightBalancedTree.Min);


            // Add nodes to the left side to produce this tree...
            //     7
            //    / \
            //   6   9
            //  /   / \
            // 5   8   10
            foreach (Int32 currentItem in new Int32[] { 6, 5 })
            {
                testWeightBalancedTree.Add(currentItem);
            }

            Assert.AreEqual(5, testWeightBalancedTree.Min);
        }

        /// <summary>
        /// Tests that an exception is thrown when the Max property accessed on an empty tree.
        /// </summary>
        [Test]
        public void Max_TreeIsEmpty()
        {
            InvalidOperationException e = Assert.Throws<InvalidOperationException>(delegate
            {
                Int32 result = testWeightBalancedTree.Max;
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The tree is empty."));
        }

        /// <summary>
        /// Success tests for the Max property.
        /// </summary>
        [Test]
        public void Max()
        {
            // Create the following tree...
            //     7
            //    / 
            //   5    
            //  / \   
            // 4   6
            foreach (Int32 currentItem in new Int32[] { 7, 5, 4, 6 })
            {
                testWeightBalancedTree.Add(currentItem);
            }

            Assert.AreEqual(7, testWeightBalancedTree.Max);


            // Add nodes to the right side to produce this tree...
            //     7
            //    / \ 
            //   5   8  
            //  / \   \ 
            // 4   6   9
            foreach (Int32 currentItem in new Int32[] { 8, 9 })
            {
                testWeightBalancedTree.Add(currentItem);
            }

            Assert.AreEqual(9, testWeightBalancedTree.Max);
        }

        /// <summary>
        /// Tests that an exception is thrown when the Get() method is called with an 'item' parameter that does not exist in the tree.
        /// </summary>
        [Test]
        public void Get_ItemDoesNotExist()
        {
            // Create the following tree...
            //     7
            //    / \
            //   5   9
            //  / \
            // 3   6
            Int32[] treeItems = new Int32[] { 7, 5, 9, 3, 6 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            // Test when 'item' should be on the left of a node
            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                testWeightBalancedTree.Get(2);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The specified item ('2') does not exist in the tree."));
            Assert.AreEqual("item", e.ParamName);
            

            // Test when 'item' should be on the right of a node
            e = Assert.Throws<ArgumentException>(delegate
            {
                testWeightBalancedTree.Get(4);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The specified item ('4') does not exist in the tree."));
            Assert.AreEqual("item", e.ParamName);
        }

        /// <summary>
        /// Tests that an exception is thrown when the Get() method is called on an empty tree.
        /// </summary>
        [Test]
        public void Get_TreeIsEmpty()
        {
            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                testWeightBalancedTree.Get(50);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The specified item ('50') does not exist in the tree."));
            Assert.AreEqual("item", e.ParamName);
        }

        /// <summary>
        /// Success tests for the Get() method.
        /// </summary>
        [Test]
        public void Get()
        {
            WeightBalancedTree<CharacterAndCount> testWeightBalancedTree = new WeightBalancedTree<CharacterAndCount>();
            testWeightBalancedTree.Add(new CharacterAndCount('c', 1));
            testWeightBalancedTree.Add(new CharacterAndCount('a', 5));
            testWeightBalancedTree.Add(new CharacterAndCount('e', 10));
            testWeightBalancedTree.Add(new CharacterAndCount('b', 15));
            testWeightBalancedTree.Add(new CharacterAndCount('d', 20));

            CharacterAndCount cCount = testWeightBalancedTree.Get(new CharacterAndCount('c', 0));
            CharacterAndCount aCount = testWeightBalancedTree.Get(new CharacterAndCount('a', 0));
            CharacterAndCount dCount = testWeightBalancedTree.Get(new CharacterAndCount('d', 0));

            Assert.AreEqual(1, cCount.Count);
            Assert.AreEqual(5, aCount.Count);
            Assert.AreEqual(20, dCount.Count);


            dCount.Count++;

            CharacterAndCount dCount2 = testWeightBalancedTree.Get(new CharacterAndCount('d', 0));

            Assert.AreEqual(21, dCount.Count);
        }

        /// <summary>
        /// Tests that an exception is thrown if the RotateNodeLeft() method is called on a node whose right child is null.
        /// </summary>
        [Test]
        public void RotateNodeLeft_NodeRightChildIsNull()
        {
            // Test with the following tree
            //      3
            //     /
            //    2
            //   /
            //  1
            WeightBalancedTreeWithProtectedMethods<Int32> testWeightBalancedTree = new WeightBalancedTreeWithProtectedMethods<Int32>(new Int32[] { 3, 2, 1 }, false);
            WeightBalancedTreeNode<Int32> nodeToRotate = null;
            Action<WeightBalancedTreeNode<Int32>> getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 3)
                    nodeToRotate = currentNode;
            };
            testWeightBalancedTree.InOrderDepthFirstSearch(getNodeAction);

            InvalidOperationException e = Assert.Throws<InvalidOperationException>(delegate
            {
                testWeightBalancedTree.RotateNodeLeft(nodeToRotate);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The node containing item '3' cannot be left-rotated as its right child is null."));
        }

        /// <summary>
        /// Success tests for the WillLeftRotationImproveBalance() method.
        /// </summary>
        [Test]
        public void WillLeftRotationImproveBalance()
        {
            // Test with nodes 1 and 2 in the following tree
            //  1
            //   \
            //    2
            //     \
            //      3
            WeightBalancedTreeWithProtectedMethods<Int32> testWeightBalancedTree = new WeightBalancedTreeWithProtectedMethods<Int32>(new Int32[] { 1, 2, 3 }, false);
            WeightBalancedTreeNode<Int32> nodeToAssess = null;
            Action<WeightBalancedTreeNode<Int32>> getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 1)
                    nodeToAssess = currentNode;
            };
            testWeightBalancedTree.BreadthFirstSearch(getNodeAction);

            Boolean result = testWeightBalancedTree.WillLeftRotationImproveBalance(nodeToAssess);

            Assert.IsTrue(result);


            getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 2)
                    nodeToAssess = currentNode;
            };
            testWeightBalancedTree.BreadthFirstSearch(getNodeAction);

            result = testWeightBalancedTree.WillLeftRotationImproveBalance(nodeToAssess);

            Assert.IsFalse(result);


            // Test with node 3 in the following tree
            //      3
            //     / \
            //    2   4
            //   /
            //  1
            testWeightBalancedTree = new WeightBalancedTreeWithProtectedMethods<Int32>(new Int32[] { 3, 2, 4, 1 }, false);
            getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 3)
                    nodeToAssess = currentNode;
            };
            testWeightBalancedTree.BreadthFirstSearch(getNodeAction);

            result = testWeightBalancedTree.WillLeftRotationImproveBalance(nodeToAssess);

            Assert.IsFalse(result);
        }

        /// <summary>
        /// Success tests for the WillRightRotationImproveBalance() method.
        /// </summary>
        [Test]
        public void WillRightRotationImproveBalance()
        {
            // Test with nodes 3 and 2 in the following tree
            //      3
            //     / 
            //    2   
            //   /
            //  1
            WeightBalancedTreeWithProtectedMethods<Int32> testWeightBalancedTree = new WeightBalancedTreeWithProtectedMethods<Int32>(new Int32[] { 3, 2, 1 }, false);
            WeightBalancedTreeNode<Int32> nodeToAssess = null;
            Action<WeightBalancedTreeNode<Int32>> getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 3)
                    nodeToAssess = currentNode;
            };
            testWeightBalancedTree.BreadthFirstSearch(getNodeAction);

            Boolean result = testWeightBalancedTree.WillRightRotationImproveBalance(nodeToAssess);

            Assert.IsTrue(result);


            getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 2)
                    nodeToAssess = currentNode;
            };
            testWeightBalancedTree.BreadthFirstSearch(getNodeAction);

            result = testWeightBalancedTree.WillRightRotationImproveBalance(nodeToAssess);

            Assert.IsFalse(result);


            // Test with node 2 in the following tree
            //    2
            //   / \
            //  1   3
            //       \
            //        4
            testWeightBalancedTree = new WeightBalancedTreeWithProtectedMethods<Int32>(new Int32[] { 2, 1, 3, 4 }, false);
            getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 2)
                    nodeToAssess = currentNode;
            };
            testWeightBalancedTree.BreadthFirstSearch(getNodeAction);

            result = testWeightBalancedTree.WillRightRotationImproveBalance(nodeToAssess);

            Assert.IsFalse(result);
        }

        /// <summary>
        /// Success test for the RotateNodeLeft() method.
        /// </summary>
        [Test]
        public void RotateNodeLeft()
        {
            // Test by left-rotating the node containing 3 in the following tree...
            //            12
            //         /      \
            //       3         13
            //     /   \
            //   2       7
            //  /      /   \
            // 1     5       9
            //      / \     / \
            //     4   6   8   10
            //                   \
            //                    11
            //
            // ...and expect the result to look like...
            //              12
            //           /      \
            //         7         13
            //       /   \
            //     3       9
            //    / \     / \
            //   2   5   8   10
            //  /   / \        \
            // 1   4   6        11
            var allNodes = new Dictionary<Int32, WeightBalancedTreeNode<Int32>>();
            var subtreeValueStore = new Dictionary<Int32, Tuple<Int32, Int32>>();
            Action<WeightBalancedTreeNode<Int32>> populateStoreAction = (node) =>
            {
                allNodes.Add(node.Item, node);
                subtreeValueStore.Add(node.Item, new Tuple<Int32, Int32>(node.LeftSubtreeSize, node.RightSubtreeSize));
            };
            WeightBalancedTreeWithProtectedMethods<Int32> testWeightBalancedTree = new WeightBalancedTreeWithProtectedMethods<Int32>(new Int32[] { 12, 3, 13, 2, 7, 1, 5, 9, 4, 6, 8, 10, 11 }, false);
            WeightBalancedTreeNode<Int32> nodeToRotate = null;
            Action<WeightBalancedTreeNode<Int32>> getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 3)
                    nodeToRotate = currentNode;
            };
            testWeightBalancedTree.InOrderDepthFirstSearch(getNodeAction);

            testWeightBalancedTree.RotateNodeLeft(nodeToRotate);

            testWeightBalancedTree.BreadthFirstSearch(populateStoreAction);
            Assert.AreEqual(13, allNodes.Count);
            Assert.IsNull(allNodes[12].ParentNode);
            Assert.AreSame(allNodes[7], allNodes[12].LeftChildNode);
            Assert.AreSame(allNodes[13], allNodes[12].RightChildNode);
            Assert.AreSame(allNodes[12], allNodes[7].ParentNode);
            Assert.AreSame(allNodes[3], allNodes[7].LeftChildNode);
            Assert.AreSame(allNodes[9], allNodes[7].RightChildNode);
            Assert.AreSame(allNodes[12], allNodes[13].ParentNode);
            Assert.IsNull(allNodes[13].LeftChildNode);
            Assert.IsNull(allNodes[13].RightChildNode);
            Assert.AreSame(allNodes[7], allNodes[3].ParentNode);
            Assert.AreSame(allNodes[2], allNodes[3].LeftChildNode);
            Assert.AreSame(allNodes[5], allNodes[3].RightChildNode);
            Assert.AreSame(allNodes[7], allNodes[9].ParentNode);
            Assert.AreSame(allNodes[8], allNodes[9].LeftChildNode);
            Assert.AreSame(allNodes[10], allNodes[9].RightChildNode);
            Assert.AreSame(allNodes[3], allNodes[2].ParentNode);
            Assert.AreSame(allNodes[1], allNodes[2].LeftChildNode);
            Assert.IsNull(allNodes[2].RightChildNode);
            Assert.AreSame(allNodes[3], allNodes[5].ParentNode);
            Assert.AreSame(allNodes[4], allNodes[5].LeftChildNode);
            Assert.AreSame(allNodes[6], allNodes[5].RightChildNode);
            Assert.AreSame(allNodes[9], allNodes[8].ParentNode);
            Assert.IsNull(allNodes[8].LeftChildNode);
            Assert.IsNull(allNodes[8].RightChildNode);
            Assert.AreSame(allNodes[9], allNodes[10].ParentNode);
            Assert.IsNull(allNodes[10].LeftChildNode);
            Assert.AreSame(allNodes[11], allNodes[10].RightChildNode);
            Assert.AreSame(allNodes[2], allNodes[1].ParentNode);
            Assert.IsNull(allNodes[1].LeftChildNode);
            Assert.IsNull(allNodes[1].LeftChildNode);
            Assert.AreSame(allNodes[5], allNodes[4].ParentNode);
            Assert.IsNull(allNodes[4].LeftChildNode);
            Assert.IsNull(allNodes[4].LeftChildNode);
            Assert.AreSame(allNodes[5], allNodes[6].ParentNode);
            Assert.IsNull(allNodes[6].LeftChildNode);
            Assert.IsNull(allNodes[6].LeftChildNode);
            Assert.AreSame(allNodes[10], allNodes[11].ParentNode);
            Assert.IsNull(allNodes[11].LeftChildNode);
            Assert.IsNull(allNodes[11].LeftChildNode);
            Assert.AreEqual(11, subtreeValueStore[12].Item1);
            Assert.AreEqual(1, subtreeValueStore[12].Item2);
            Assert.AreEqual(6, subtreeValueStore[7].Item1);
            Assert.AreEqual(4, subtreeValueStore[7].Item2);
            Assert.AreEqual(0, subtreeValueStore[13].Item1);
            Assert.AreEqual(0, subtreeValueStore[13].Item2);
            Assert.AreEqual(2, subtreeValueStore[3].Item1);
            Assert.AreEqual(3, subtreeValueStore[3].Item2);
            Assert.AreEqual(1, subtreeValueStore[9].Item1);
            Assert.AreEqual(2, subtreeValueStore[9].Item2);
            Assert.AreEqual(1, subtreeValueStore[2].Item1);
            Assert.AreEqual(0, subtreeValueStore[2].Item2);
            Assert.AreEqual(1, subtreeValueStore[5].Item1);
            Assert.AreEqual(1, subtreeValueStore[5].Item2);
            Assert.AreEqual(0, subtreeValueStore[8].Item1);
            Assert.AreEqual(0, subtreeValueStore[8].Item2);
            Assert.AreEqual(0, subtreeValueStore[10].Item1);
            Assert.AreEqual(1, subtreeValueStore[10].Item2);
            Assert.AreEqual(0, subtreeValueStore[1].Item1);
            Assert.AreEqual(0, subtreeValueStore[1].Item2);
            Assert.AreEqual(0, subtreeValueStore[4].Item1);
            Assert.AreEqual(0, subtreeValueStore[4].Item2);
            Assert.AreEqual(0, subtreeValueStore[6].Item1);
            Assert.AreEqual(0, subtreeValueStore[6].Item2);
            Assert.AreEqual(0, subtreeValueStore[11].Item1);
            Assert.AreEqual(0, subtreeValueStore[11].Item2);
        }

        /// <summary>
        /// Success test for the RotateNodeLeft() method where the parent of the rotated node is null.
        /// </summary>
        [Test]
        public void RotateNodeLeft_NodeParentIsNull()
        {
            // Test by left-rotating the node containing 3 in the following tree...
            //       3        
            //     /   \
            //   2       7
            //  /      /   \
            // 1     5       9
            //      / \     / \
            //     4   6   8   10
            //                   \
            //                    11
            //
            // ...and expect the result to look like...
            //         7 
            //       /   \
            //     3       9
            //    / \     / \
            //   2   5   8   10
            //  /   / \        \
            // 1   4   6        11
            var allNodes = new Dictionary<Int32, WeightBalancedTreeNode<Int32>>();
            var subtreeValueStore = new Dictionary<Int32, Tuple<Int32, Int32>>();
            Action<WeightBalancedTreeNode<Int32>> populateStoreAction = (node) =>
            {
                allNodes.Add(node.Item, node);
                subtreeValueStore.Add(node.Item, new Tuple<Int32, Int32>(node.LeftSubtreeSize, node.RightSubtreeSize));
            };
            WeightBalancedTreeWithProtectedMethods<Int32> testWeightBalancedTree = new WeightBalancedTreeWithProtectedMethods<Int32>(new Int32[] { 3, 2, 7, 1, 5, 9, 4, 6, 8, 10, 11 }, false);
            WeightBalancedTreeNode<Int32> nodeToRotate = null;
            Action<WeightBalancedTreeNode<Int32>> getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 3)
                    nodeToRotate = currentNode;
            };
            testWeightBalancedTree.InOrderDepthFirstSearch(getNodeAction);

            testWeightBalancedTree.RotateNodeLeft(nodeToRotate);

            // Need to reassign the root node, as rotation of the root would have changed it 
            testWeightBalancedTree.RootNode = nodeToRotate.ParentNode;
            testWeightBalancedTree.BreadthFirstSearch(populateStoreAction);
            Assert.AreEqual(11, allNodes.Count);
            Assert.IsNull(allNodes[7].ParentNode);
            Assert.AreSame(allNodes[3], allNodes[7].LeftChildNode);
            Assert.AreSame(allNodes[9], allNodes[7].RightChildNode);
            Assert.AreSame(allNodes[7], allNodes[3].ParentNode);
            Assert.AreSame(allNodes[2], allNodes[3].LeftChildNode);
            Assert.AreSame(allNodes[5], allNodes[3].RightChildNode);
            Assert.AreSame(allNodes[7], allNodes[9].ParentNode);
            Assert.AreSame(allNodes[8], allNodes[9].LeftChildNode);
            Assert.AreSame(allNodes[10], allNodes[9].RightChildNode);
            Assert.AreSame(allNodes[3], allNodes[2].ParentNode);
            Assert.AreSame(allNodes[1], allNodes[2].LeftChildNode);
            Assert.IsNull(allNodes[2].RightChildNode);
            Assert.AreSame(allNodes[3], allNodes[5].ParentNode);
            Assert.AreSame(allNodes[4], allNodes[5].LeftChildNode);
            Assert.AreSame(allNodes[6], allNodes[5].RightChildNode);
            Assert.AreSame(allNodes[9], allNodes[8].ParentNode);
            Assert.IsNull(allNodes[8].LeftChildNode);
            Assert.IsNull(allNodes[8].RightChildNode);
            Assert.AreSame(allNodes[9], allNodes[10].ParentNode);
            Assert.IsNull(allNodes[10].LeftChildNode);
            Assert.AreSame(allNodes[11], allNodes[10].RightChildNode);
            Assert.AreSame(allNodes[2], allNodes[1].ParentNode);
            Assert.IsNull(allNodes[1].LeftChildNode);
            Assert.IsNull(allNodes[1].LeftChildNode);
            Assert.AreSame(allNodes[5], allNodes[4].ParentNode);
            Assert.IsNull(allNodes[4].LeftChildNode);
            Assert.IsNull(allNodes[4].LeftChildNode);
            Assert.AreSame(allNodes[5], allNodes[6].ParentNode);
            Assert.IsNull(allNodes[6].LeftChildNode);
            Assert.IsNull(allNodes[6].LeftChildNode);
            Assert.AreSame(allNodes[10], allNodes[11].ParentNode);
            Assert.IsNull(allNodes[11].LeftChildNode);
            Assert.IsNull(allNodes[11].LeftChildNode);
            Assert.AreEqual(6, subtreeValueStore[7].Item1);
            Assert.AreEqual(4, subtreeValueStore[7].Item2);
            Assert.AreEqual(2, subtreeValueStore[3].Item1);
            Assert.AreEqual(3, subtreeValueStore[3].Item2);
            Assert.AreEqual(1, subtreeValueStore[9].Item1);
            Assert.AreEqual(2, subtreeValueStore[9].Item2);
            Assert.AreEqual(1, subtreeValueStore[2].Item1);
            Assert.AreEqual(0, subtreeValueStore[2].Item2);
            Assert.AreEqual(1, subtreeValueStore[5].Item1);
            Assert.AreEqual(1, subtreeValueStore[5].Item2);
            Assert.AreEqual(0, subtreeValueStore[8].Item1);
            Assert.AreEqual(0, subtreeValueStore[8].Item2);
            Assert.AreEqual(0, subtreeValueStore[10].Item1);
            Assert.AreEqual(1, subtreeValueStore[10].Item2);
            Assert.AreEqual(0, subtreeValueStore[1].Item1);
            Assert.AreEqual(0, subtreeValueStore[1].Item2);
            Assert.AreEqual(0, subtreeValueStore[4].Item1);
            Assert.AreEqual(0, subtreeValueStore[4].Item2);
            Assert.AreEqual(0, subtreeValueStore[6].Item1);
            Assert.AreEqual(0, subtreeValueStore[6].Item2);
            Assert.AreEqual(0, subtreeValueStore[11].Item1);
            Assert.AreEqual(0, subtreeValueStore[11].Item2);
        }

        /// <summary>
        /// Success test for the RotateNodeLeft() method where the left child of the right child of the node being rotated is null.
        /// </summary>
        [Test]
        public void RotateNodeLeft_LeftChildOfRightChildOfNodeIsNull()
        {
            // Test by left-rotating the node containing 3 in the following tree...
            //            12
            //         /      \
            //       3         13
            //     /   \
            //   2       7
            //  /          \
            // 1             9
            //              / \
            //             8   10
            //                   \
            //                    11
            //
            // ...and expect the result to look like...
            //              12
            //           /      \
            //         7         13  
            //       /   \
            //     3       9
            //    /       / \
            //   2       8   10
            //  /              \
            // 1                11
            var allNodes = new Dictionary<Int32, WeightBalancedTreeNode<Int32>>();
            var subtreeValueStore = new Dictionary<Int32, Tuple<Int32, Int32>>();
            Action<WeightBalancedTreeNode<Int32>> populateStoreAction = (node) =>
            {
                allNodes.Add(node.Item, node);
                subtreeValueStore.Add(node.Item, new Tuple<Int32, Int32>(node.LeftSubtreeSize, node.RightSubtreeSize));
            };
            WeightBalancedTreeWithProtectedMethods<Int32> testWeightBalancedTree = new WeightBalancedTreeWithProtectedMethods<Int32>(new Int32[] { 12, 3, 13, 2, 7, 1, 9, 8, 10, 11 }, false);
            WeightBalancedTreeNode<Int32> nodeToRotate = null;
            Action<WeightBalancedTreeNode<Int32>> getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 3)
                    nodeToRotate = currentNode;
            };
            testWeightBalancedTree.InOrderDepthFirstSearch(getNodeAction);

            testWeightBalancedTree.RotateNodeLeft(nodeToRotate);

            testWeightBalancedTree.BreadthFirstSearch(populateStoreAction);
            Assert.AreEqual(10, allNodes.Count);
            Assert.IsNull(allNodes[12].ParentNode);
            Assert.AreSame(allNodes[7], allNodes[12].LeftChildNode);
            Assert.AreSame(allNodes[13], allNodes[12].RightChildNode);
            Assert.AreSame(allNodes[12], allNodes[7].ParentNode);
            Assert.AreSame(allNodes[3], allNodes[7].LeftChildNode);
            Assert.AreSame(allNodes[9], allNodes[7].RightChildNode);
            Assert.AreSame(allNodes[12], allNodes[13].ParentNode);
            Assert.IsNull(allNodes[13].LeftChildNode);
            Assert.IsNull(allNodes[13].RightChildNode);
            Assert.AreSame(allNodes[7], allNodes[3].ParentNode);
            Assert.AreSame(allNodes[2], allNodes[3].LeftChildNode);
            Assert.IsNull(allNodes[3].RightChildNode);
            Assert.AreSame(allNodes[7], allNodes[9].ParentNode);
            Assert.AreSame(allNodes[8], allNodes[9].LeftChildNode);
            Assert.AreSame(allNodes[10], allNodes[9].RightChildNode);
            Assert.AreSame(allNodes[3], allNodes[2].ParentNode);
            Assert.AreSame(allNodes[1], allNodes[2].LeftChildNode);
            Assert.IsNull(allNodes[2].RightChildNode);
            Assert.AreSame(allNodes[9], allNodes[8].ParentNode);
            Assert.IsNull(allNodes[8].LeftChildNode);
            Assert.IsNull(allNodes[8].RightChildNode);
            Assert.AreSame(allNodes[9], allNodes[10].ParentNode);
            Assert.IsNull(allNodes[10].LeftChildNode);
            Assert.AreSame(allNodes[11], allNodes[10].RightChildNode);
            Assert.AreSame(allNodes[2], allNodes[1].ParentNode);
            Assert.IsNull(allNodes[1].LeftChildNode);
            Assert.IsNull(allNodes[1].LeftChildNode);
            Assert.AreSame(allNodes[10], allNodes[11].ParentNode);
            Assert.IsNull(allNodes[11].LeftChildNode);
            Assert.IsNull(allNodes[11].LeftChildNode);
            Assert.AreEqual(8, subtreeValueStore[12].Item1);
            Assert.AreEqual(1, subtreeValueStore[12].Item2);
            Assert.AreEqual(3, subtreeValueStore[7].Item1);
            Assert.AreEqual(4, subtreeValueStore[7].Item2);
            Assert.AreEqual(0, subtreeValueStore[13].Item1);
            Assert.AreEqual(0, subtreeValueStore[13].Item2);
            Assert.AreEqual(2, subtreeValueStore[3].Item1);
            Assert.AreEqual(0, subtreeValueStore[3].Item2);
            Assert.AreEqual(1, subtreeValueStore[9].Item1);
            Assert.AreEqual(2, subtreeValueStore[9].Item2);
            Assert.AreEqual(1, subtreeValueStore[2].Item1);
            Assert.AreEqual(0, subtreeValueStore[2].Item2);
            Assert.AreEqual(0, subtreeValueStore[8].Item1);
            Assert.AreEqual(0, subtreeValueStore[8].Item2);
            Assert.AreEqual(0, subtreeValueStore[10].Item1);
            Assert.AreEqual(1, subtreeValueStore[10].Item2);
            Assert.AreEqual(0, subtreeValueStore[1].Item1);
            Assert.AreEqual(0, subtreeValueStore[1].Item2);
            Assert.AreEqual(0, subtreeValueStore[11].Item1);
            Assert.AreEqual(0, subtreeValueStore[11].Item2);
        }

        /// <summary>
        /// Tests that an exception is thrown if the RotateNodeRight() method is called on a node whose left child is null.
        /// </summary>
        [Test]
        public void RotateNodeRight_NodeLeftChildIsNull()
        {
            // Test with the following tree
            //  3
            //   \
            //    4
            //     \
            //      5
            WeightBalancedTreeWithProtectedMethods<Int32> testWeightBalancedTree = new WeightBalancedTreeWithProtectedMethods<Int32>(new Int32[] { 3, 4, 5 }, false);
            WeightBalancedTreeNode<Int32> nodeToRotate = null;
            Action<WeightBalancedTreeNode<Int32>> getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 3)
                    nodeToRotate = currentNode;
            };
            testWeightBalancedTree.InOrderDepthFirstSearch(getNodeAction);

            InvalidOperationException e = Assert.Throws<InvalidOperationException>(delegate
            {
                testWeightBalancedTree.RotateNodeRight(nodeToRotate);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The node containing item '3' cannot be right-rotated as its left child is null."));
        }

        /// <summary>
        /// Success test for the RotateNodeRight() method.
        /// </summary>
        [Test]
        public void RotateNodeRight()
        {
            // Test by left-rotating the node containing 11 in the following tree...
            //            2
            //         /     \
            //       1        11
            //              /    \
            //             7      12
            //           /   \      \
            //         5       9     13
            //        / \     / \
            //       4   6   8   10
            //      /
            //     3
            //
            // ...and expect the result to look like...
            //            2
            //         /     \
            //       1        7
            //              /   \
            //            5       11
            //           / \    /    \
            //          4   6  9      12
            //         /      / \       \
            //        3      8   10      13
            var allNodes = new Dictionary<Int32, WeightBalancedTreeNode<Int32>>();
            var subtreeValueStore = new Dictionary<Int32, Tuple<Int32, Int32>>();
            Action<WeightBalancedTreeNode<Int32>> populateStoreAction = (node) =>
            {
                allNodes.Add(node.Item, node);
                subtreeValueStore.Add(node.Item, new Tuple<Int32, Int32>(node.LeftSubtreeSize, node.RightSubtreeSize));
            };
            WeightBalancedTreeWithProtectedMethods<Int32> testWeightBalancedTree = new WeightBalancedTreeWithProtectedMethods<Int32>(new Int32[] { 2, 1, 11, 7, 12, 5, 9, 13, 4, 6, 8, 10, 3 }, false);
            WeightBalancedTreeNode<Int32> nodeToRotate = null;
            Action<WeightBalancedTreeNode<Int32>> getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 11)
                    nodeToRotate = currentNode;
            };
            testWeightBalancedTree.InOrderDepthFirstSearch(getNodeAction);

            testWeightBalancedTree.RotateNodeRight(nodeToRotate);

            testWeightBalancedTree.BreadthFirstSearch(populateStoreAction);
            Assert.AreEqual(13, allNodes.Count);
            Assert.IsNull(allNodes[2].ParentNode);
            Assert.AreSame(allNodes[1], allNodes[2].LeftChildNode);
            Assert.AreSame(allNodes[7], allNodes[2].RightChildNode);
            Assert.AreSame(allNodes[2], allNodes[1].ParentNode);
            Assert.IsNull(allNodes[1].LeftChildNode);
            Assert.IsNull(allNodes[1].RightChildNode);
            Assert.AreSame(allNodes[2], allNodes[7].ParentNode);
            Assert.AreSame(allNodes[5], allNodes[7].LeftChildNode);
            Assert.AreSame(allNodes[11], allNodes[7].RightChildNode);
            Assert.AreSame(allNodes[7], allNodes[5].ParentNode);
            Assert.AreSame(allNodes[4], allNodes[5].LeftChildNode);
            Assert.AreSame(allNodes[6], allNodes[5].RightChildNode);
            Assert.AreSame(allNodes[7], allNodes[11].ParentNode);
            Assert.AreSame(allNodes[9], allNodes[11].LeftChildNode);
            Assert.AreSame(allNodes[12], allNodes[11].RightChildNode);
            Assert.AreSame(allNodes[5], allNodes[4].ParentNode);
            Assert.AreSame(allNodes[3], allNodes[4].LeftChildNode);
            Assert.IsNull(allNodes[4].RightChildNode);
            Assert.AreSame(allNodes[5], allNodes[6].ParentNode);
            Assert.IsNull(allNodes[6].LeftChildNode);
            Assert.IsNull(allNodes[6].RightChildNode);
            Assert.AreSame(allNodes[11], allNodes[9].ParentNode);
            Assert.AreSame(allNodes[8], allNodes[9].LeftChildNode);
            Assert.AreSame(allNodes[10], allNodes[9].RightChildNode);
            Assert.AreSame(allNodes[11], allNodes[12].ParentNode);
            Assert.IsNull(allNodes[12].LeftChildNode);
            Assert.AreSame(allNodes[13], allNodes[12].RightChildNode);
            Assert.AreSame(allNodes[4], allNodes[3].ParentNode);
            Assert.IsNull(allNodes[3].LeftChildNode);
            Assert.IsNull(allNodes[3].RightChildNode);
            Assert.AreSame(allNodes[9], allNodes[8].ParentNode);
            Assert.IsNull(allNodes[8].LeftChildNode);
            Assert.IsNull(allNodes[8].RightChildNode);
            Assert.AreSame(allNodes[9], allNodes[10].ParentNode);
            Assert.IsNull(allNodes[10].LeftChildNode);
            Assert.IsNull(allNodes[10].RightChildNode);
            Assert.AreSame(allNodes[12], allNodes[13].ParentNode);
            Assert.IsNull(allNodes[13].LeftChildNode);
            Assert.IsNull(allNodes[13].RightChildNode);
            Assert.AreEqual(1, subtreeValueStore[2].Item1);
            Assert.AreEqual(11, subtreeValueStore[2].Item2);
            Assert.AreEqual(0, subtreeValueStore[1].Item1);
            Assert.AreEqual(0, subtreeValueStore[1].Item2);
            Assert.AreEqual(4, subtreeValueStore[7].Item1);
            Assert.AreEqual(6, subtreeValueStore[7].Item2);
            Assert.AreEqual(2, subtreeValueStore[5].Item1);
            Assert.AreEqual(1, subtreeValueStore[5].Item2);
            Assert.AreEqual(3, subtreeValueStore[11].Item1);
            Assert.AreEqual(2, subtreeValueStore[11].Item2);
            Assert.AreEqual(1, subtreeValueStore[4].Item1);
            Assert.AreEqual(0, subtreeValueStore[4].Item2);
            Assert.AreEqual(0, subtreeValueStore[6].Item1);
            Assert.AreEqual(0, subtreeValueStore[6].Item2);
            Assert.AreEqual(1, subtreeValueStore[9].Item1);
            Assert.AreEqual(1, subtreeValueStore[9].Item2);
            Assert.AreEqual(0, subtreeValueStore[12].Item1);
            Assert.AreEqual(1, subtreeValueStore[12].Item2);
            Assert.AreEqual(0, subtreeValueStore[3].Item1);
            Assert.AreEqual(0, subtreeValueStore[3].Item2);
            Assert.AreEqual(0, subtreeValueStore[8].Item1);
            Assert.AreEqual(0, subtreeValueStore[8].Item2);
            Assert.AreEqual(0, subtreeValueStore[10].Item1);
            Assert.AreEqual(0, subtreeValueStore[10].Item2);
            Assert.AreEqual(0, subtreeValueStore[13].Item1);
            Assert.AreEqual(0, subtreeValueStore[13].Item2);
        }

        /// <summary>
        /// Success test for the RotateNodeRight() method where the parent of the rotated node is null.
        /// </summary>
        [Test]
        public void RotateNodeRight_NodeParentIsNull()
        {
            // Test by left-rotating the node containing 11 in the following tree...
            //                11
            //              /    \
            //             7      12
            //           /   \      \
            //         5       9     13
            //        / \     / \
            //       4   6   8   10
            //      /
            //     3
            //
            // ...and expect the result to look like...
            //                7
            //              /   \
            //            5       11
            //           / \    /    \
            //          4   6  9      12
            //         /      / \       \
            //        3      8   10      13
            var allNodes = new Dictionary<Int32, WeightBalancedTreeNode<Int32>>();
            var subtreeValueStore = new Dictionary<Int32, Tuple<Int32, Int32>>();
            Action<WeightBalancedTreeNode<Int32>> populateStoreAction = (node) =>
            {
                allNodes.Add(node.Item, node);
                subtreeValueStore.Add(node.Item, new Tuple<Int32, Int32>(node.LeftSubtreeSize, node.RightSubtreeSize));
            };
            WeightBalancedTreeWithProtectedMethods<Int32> testWeightBalancedTree = new WeightBalancedTreeWithProtectedMethods<Int32>(new Int32[] { 11, 7, 12, 5, 9, 13, 4, 6, 8, 10, 3 }, false);
            WeightBalancedTreeNode<Int32> nodeToRotate = null;
            Action<WeightBalancedTreeNode<Int32>> getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 11)
                    nodeToRotate = currentNode;
            };
            testWeightBalancedTree.InOrderDepthFirstSearch(getNodeAction);

            testWeightBalancedTree.RotateNodeRight(nodeToRotate);

            // Need to reassign the root node, as rotation of the root would have changed it 
            testWeightBalancedTree.RootNode = nodeToRotate.ParentNode;
            testWeightBalancedTree.BreadthFirstSearch(populateStoreAction);
            Assert.AreEqual(11, allNodes.Count);
            Assert.IsNull(allNodes[7].ParentNode);
            Assert.AreSame(allNodes[5], allNodes[7].LeftChildNode);
            Assert.AreSame(allNodes[11], allNodes[7].RightChildNode);
            Assert.AreSame(allNodes[7], allNodes[5].ParentNode);
            Assert.AreSame(allNodes[4], allNodes[5].LeftChildNode);
            Assert.AreSame(allNodes[6], allNodes[5].RightChildNode);
            Assert.AreSame(allNodes[7], allNodes[11].ParentNode);
            Assert.AreSame(allNodes[9], allNodes[11].LeftChildNode);
            Assert.AreSame(allNodes[12], allNodes[11].RightChildNode);
            Assert.AreSame(allNodes[5], allNodes[4].ParentNode);
            Assert.AreSame(allNodes[3], allNodes[4].LeftChildNode);
            Assert.IsNull(allNodes[4].RightChildNode);
            Assert.AreSame(allNodes[5], allNodes[6].ParentNode);
            Assert.IsNull(allNodes[6].LeftChildNode);
            Assert.IsNull(allNodes[6].RightChildNode);
            Assert.AreSame(allNodes[11], allNodes[9].ParentNode);
            Assert.AreSame(allNodes[8], allNodes[9].LeftChildNode);
            Assert.AreSame(allNodes[10], allNodes[9].RightChildNode);
            Assert.AreSame(allNodes[11], allNodes[12].ParentNode);
            Assert.IsNull(allNodes[12].LeftChildNode);
            Assert.AreSame(allNodes[13], allNodes[12].RightChildNode);
            Assert.AreSame(allNodes[4], allNodes[3].ParentNode);
            Assert.IsNull(allNodes[3].LeftChildNode);
            Assert.IsNull(allNodes[3].RightChildNode);
            Assert.AreSame(allNodes[9], allNodes[8].ParentNode);
            Assert.IsNull(allNodes[8].LeftChildNode);
            Assert.IsNull(allNodes[8].RightChildNode);
            Assert.AreSame(allNodes[9], allNodes[10].ParentNode);
            Assert.IsNull(allNodes[10].LeftChildNode);
            Assert.IsNull(allNodes[10].RightChildNode);
            Assert.AreSame(allNodes[12], allNodes[13].ParentNode);
            Assert.IsNull(allNodes[13].LeftChildNode);
            Assert.IsNull(allNodes[13].RightChildNode);
            Assert.AreEqual(4, subtreeValueStore[7].Item1);
            Assert.AreEqual(6, subtreeValueStore[7].Item2);
            Assert.AreEqual(2, subtreeValueStore[5].Item1);
            Assert.AreEqual(1, subtreeValueStore[5].Item2);
            Assert.AreEqual(3, subtreeValueStore[11].Item1);
            Assert.AreEqual(2, subtreeValueStore[11].Item2);
            Assert.AreEqual(1, subtreeValueStore[4].Item1);
            Assert.AreEqual(0, subtreeValueStore[4].Item2);
            Assert.AreEqual(0, subtreeValueStore[6].Item1);
            Assert.AreEqual(0, subtreeValueStore[6].Item2);
            Assert.AreEqual(1, subtreeValueStore[9].Item1);
            Assert.AreEqual(1, subtreeValueStore[9].Item2);
            Assert.AreEqual(0, subtreeValueStore[12].Item1);
            Assert.AreEqual(1, subtreeValueStore[12].Item2);
            Assert.AreEqual(0, subtreeValueStore[3].Item1);
            Assert.AreEqual(0, subtreeValueStore[3].Item2);
            Assert.AreEqual(0, subtreeValueStore[8].Item1);
            Assert.AreEqual(0, subtreeValueStore[8].Item2);
            Assert.AreEqual(0, subtreeValueStore[10].Item1);
            Assert.AreEqual(0, subtreeValueStore[10].Item2);
            Assert.AreEqual(0, subtreeValueStore[13].Item1);
            Assert.AreEqual(0, subtreeValueStore[13].Item2);
        }

        /// <summary>
        /// Success test for the RotateNodeRight() method where the right child of the left child of the node being rotated is null.
        /// </summary>
        [Test]
        public void RotateNodeRight_RightChildOfLeftChildOfNodeIsNull()
        {
            // Test by left-rotating the node containing 11 in the following tree...
            //           2
            //         /   \
            //       1      11
            //            /    \
            //           7      12
            //          /         \
            //         5           13
            //        / \ 
            //       4   6 
            //      /
            //     3
            //
            // ...and expect the result to look like...
            //         2
            //       /   \
            //     1      7
            //          /   \
            //         5     11
            //        / \      \
            //       4   6      12
            //      /             \
            //     3               13
            var allNodes = new Dictionary<Int32, WeightBalancedTreeNode<Int32>>();
            var subtreeValueStore = new Dictionary<Int32, Tuple<Int32, Int32>>();
            Action<WeightBalancedTreeNode<Int32>> populateStoreAction = (node) =>
            {
                allNodes.Add(node.Item, node);
                subtreeValueStore.Add(node.Item, new Tuple<Int32, Int32>(node.LeftSubtreeSize, node.RightSubtreeSize));
            };
            WeightBalancedTreeWithProtectedMethods<Int32> testWeightBalancedTree = new WeightBalancedTreeWithProtectedMethods<Int32>(new Int32[] { 2, 1, 11, 7, 12, 5, 13, 4, 6, 3 }, false);
            WeightBalancedTreeNode<Int32> nodeToRotate = null;
            Action<WeightBalancedTreeNode<Int32>> getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 11)
                    nodeToRotate = currentNode;
            };
            testWeightBalancedTree.InOrderDepthFirstSearch(getNodeAction);

            testWeightBalancedTree.RotateNodeRight(nodeToRotate);

            testWeightBalancedTree.BreadthFirstSearch(populateStoreAction);
            Assert.AreEqual(10, allNodes.Count);
            Assert.IsNull(allNodes[2].ParentNode);
            Assert.AreSame(allNodes[1], allNodes[2].LeftChildNode);
            Assert.AreSame(allNodes[7], allNodes[2].RightChildNode);
            Assert.AreSame(allNodes[2], allNodes[1].ParentNode);
            Assert.IsNull(allNodes[1].LeftChildNode);
            Assert.IsNull(allNodes[1].RightChildNode);
            Assert.AreSame(allNodes[2], allNodes[7].ParentNode);
            Assert.AreSame(allNodes[5], allNodes[7].LeftChildNode);
            Assert.AreSame(allNodes[11], allNodes[7].RightChildNode);
            Assert.AreSame(allNodes[7], allNodes[5].ParentNode);
            Assert.AreSame(allNodes[4], allNodes[5].LeftChildNode);
            Assert.AreSame(allNodes[6], allNodes[5].RightChildNode);
            Assert.AreSame(allNodes[7], allNodes[11].ParentNode);
            Assert.IsNull(allNodes[11].LeftChildNode);
            Assert.AreSame(allNodes[12], allNodes[11].RightChildNode);
            Assert.AreSame(allNodes[5], allNodes[4].ParentNode);
            Assert.AreSame(allNodes[3], allNodes[4].LeftChildNode);
            Assert.IsNull(allNodes[4].RightChildNode);
            Assert.AreSame(allNodes[5], allNodes[6].ParentNode);
            Assert.IsNull(allNodes[6].LeftChildNode);
            Assert.IsNull(allNodes[6].RightChildNode);
            Assert.AreSame(allNodes[11], allNodes[12].ParentNode);
            Assert.IsNull(allNodes[12].LeftChildNode);
            Assert.AreSame(allNodes[13], allNodes[12].RightChildNode);
            Assert.AreSame(allNodes[4], allNodes[3].ParentNode);
            Assert.IsNull(allNodes[3].LeftChildNode);
            Assert.IsNull(allNodes[3].RightChildNode);
            Assert.AreSame(allNodes[12], allNodes[13].ParentNode);
            Assert.IsNull(allNodes[13].LeftChildNode);
            Assert.IsNull(allNodes[13].RightChildNode);
            Assert.AreEqual(1, subtreeValueStore[2].Item1);
            Assert.AreEqual(8, subtreeValueStore[2].Item2);
            Assert.AreEqual(0, subtreeValueStore[1].Item1);
            Assert.AreEqual(0, subtreeValueStore[1].Item2);
            Assert.AreEqual(4, subtreeValueStore[7].Item1);
            Assert.AreEqual(3, subtreeValueStore[7].Item2);
            Assert.AreEqual(2, subtreeValueStore[5].Item1);
            Assert.AreEqual(1, subtreeValueStore[5].Item2);
            Assert.AreEqual(0, subtreeValueStore[11].Item1);
            Assert.AreEqual(2, subtreeValueStore[11].Item2);
            Assert.AreEqual(1, subtreeValueStore[4].Item1);
            Assert.AreEqual(0, subtreeValueStore[4].Item2);
            Assert.AreEqual(0, subtreeValueStore[6].Item1);
            Assert.AreEqual(0, subtreeValueStore[6].Item2);
            Assert.AreEqual(0, subtreeValueStore[12].Item1);
            Assert.AreEqual(1, subtreeValueStore[12].Item2);
            Assert.AreEqual(0, subtreeValueStore[3].Item1);
            Assert.AreEqual(0, subtreeValueStore[3].Item2);
            Assert.AreEqual(0, subtreeValueStore[13].Item1);
            Assert.AreEqual(0, subtreeValueStore[13].Item2);
        }

        /// <summary>
        /// Success test for the BalanceTreeUpFromNode() method for a small tree with 3 nodes requiring right rotation.
        /// </summary>
        [Test]
        public void BalanceTreeUpFromNode_3NodesRequiresRightRotation()
        {
            // Test by balancing node 2 in the following tree...
            //       3
            //      / 
            //     2
            //    /
            //   1
            //
            // ...and expect the result to look like...
            //     2
            //    / \
            //   1   3
            var allNodes = new Dictionary<Int32, WeightBalancedTreeNode<Int32>>();
            var subtreeValueStore = new Dictionary<Int32, Tuple<Int32, Int32>>();
            Action<WeightBalancedTreeNode<Int32>> populateStoreAction = (node) =>
            {
                allNodes.Add(node.Item, node);
                subtreeValueStore.Add(node.Item, new Tuple<Int32, Int32>(node.LeftSubtreeSize, node.RightSubtreeSize));
            };
            WeightBalancedTreeWithProtectedMethods<Int32> testWeightBalancedTree = new WeightBalancedTreeWithProtectedMethods<Int32>(new Int32[] { 3, 2, 1 }, false);
            WeightBalancedTreeNode<Int32> nodeToBalance = null;
            Action<WeightBalancedTreeNode<Int32>> getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 2)
                    nodeToBalance = currentNode;
            };
            testWeightBalancedTree.InOrderDepthFirstSearch(getNodeAction);

            testWeightBalancedTree.BalanceTreeUpFromNode(nodeToBalance);

            testWeightBalancedTree.BreadthFirstSearch(populateStoreAction);
            Assert.AreEqual(3, allNodes.Count);
            Assert.IsNull(allNodes[2].ParentNode);
            Assert.AreSame(allNodes[1], allNodes[2].LeftChildNode);
            Assert.AreSame(allNodes[3], allNodes[2].RightChildNode);
            Assert.AreSame(allNodes[2], allNodes[1].ParentNode);
            Assert.IsNull(allNodes[1].LeftChildNode);
            Assert.IsNull(allNodes[1].RightChildNode);
            Assert.AreSame(allNodes[2], allNodes[3].ParentNode);
            Assert.IsNull(allNodes[3].LeftChildNode);
            Assert.IsNull(allNodes[3].RightChildNode);
            Assert.AreEqual(1, subtreeValueStore[2].Item1);
            Assert.AreEqual(1, subtreeValueStore[2].Item2);
            Assert.AreEqual(0, subtreeValueStore[1].Item1);
            Assert.AreEqual(0, subtreeValueStore[1].Item2);
            Assert.AreEqual(0, subtreeValueStore[3].Item1);
            Assert.AreEqual(0, subtreeValueStore[3].Item2);
        }

        /// <summary>
        /// Success test for the BalanceTreeUpFromNode() method for a small tree with 3 nodes requiring left rotation.
        /// </summary>
        [Test]
        public void BalanceTreeUpFromNode_3NodesRequiresLeftRotation()
        {
            // Test by balancing node 2 in the following tree...
            //   1 
            //    \ 
            //     2
            //      \
            //       3
            //
            // ...and expect the result to look like...
            //     2
            //    / \
            //   1   3
            var allNodes = new Dictionary<Int32, WeightBalancedTreeNode<Int32>>();
            var subtreeValueStore = new Dictionary<Int32, Tuple<Int32, Int32>>();
            Action<WeightBalancedTreeNode<Int32>> populateStoreAction = (node) =>
            {
                allNodes.Add(node.Item, node);
                subtreeValueStore.Add(node.Item, new Tuple<Int32, Int32>(node.LeftSubtreeSize, node.RightSubtreeSize));
            };
            WeightBalancedTreeWithProtectedMethods<Int32> testWeightBalancedTree = new WeightBalancedTreeWithProtectedMethods<Int32>(new Int32[] { 1, 2, 3 }, false);
            WeightBalancedTreeNode<Int32> nodeToBalance = null;
            Action<WeightBalancedTreeNode<Int32>> getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 2)
                    nodeToBalance = currentNode;
            };
            testWeightBalancedTree.InOrderDepthFirstSearch(getNodeAction);

            testWeightBalancedTree.BalanceTreeUpFromNode(nodeToBalance);

            testWeightBalancedTree.BreadthFirstSearch(populateStoreAction);
            Assert.AreEqual(3, allNodes.Count);
            Assert.IsNull(allNodes[2].ParentNode);
            Assert.AreSame(allNodes[1], allNodes[2].LeftChildNode);
            Assert.AreSame(allNodes[3], allNodes[2].RightChildNode);
            Assert.AreSame(allNodes[2], allNodes[1].ParentNode);
            Assert.IsNull(allNodes[1].LeftChildNode);
            Assert.IsNull(allNodes[1].RightChildNode);
            Assert.AreSame(allNodes[2], allNodes[3].ParentNode);
            Assert.IsNull(allNodes[3].LeftChildNode);
            Assert.IsNull(allNodes[3].RightChildNode);
            Assert.AreEqual(1, subtreeValueStore[2].Item1);
            Assert.AreEqual(1, subtreeValueStore[2].Item2);
            Assert.AreEqual(0, subtreeValueStore[1].Item1);
            Assert.AreEqual(0, subtreeValueStore[1].Item2);
            Assert.AreEqual(0, subtreeValueStore[3].Item1);
            Assert.AreEqual(0, subtreeValueStore[3].Item2);
        }

        /// <summary>
        /// Success tests for the BalanceTreeUpFromNode() method for small trees with 2/3 nodes which don't require balancing (i.e. tests that the method doesn't do anything).
        /// </summary>
        [Test]
        public void BalanceTreeUpFromNode_3NodesDoesntRequireRotation()
        {
            // Test that the following tree is not changed if BalanceTreeUpFromNode() is called on node containing 1...
            //   1 
            //    \ 
            //     2
            var allNodes = new Dictionary<Int32, WeightBalancedTreeNode<Int32>>();
            var subtreeValueStore = new Dictionary<Int32, Tuple<Int32, Int32>>();
            Action<WeightBalancedTreeNode<Int32>> populateStoreAction = (node) =>
            {
                allNodes.Add(node.Item, node);
                subtreeValueStore.Add(node.Item, new Tuple<Int32, Int32>(node.LeftSubtreeSize, node.RightSubtreeSize));
            };
            WeightBalancedTreeWithProtectedMethods<Int32> testWeightBalancedTree = new WeightBalancedTreeWithProtectedMethods<Int32>(new Int32[] { 1, 2 }, false);
            WeightBalancedTreeNode<Int32> nodeToBalance = null;
            Action<WeightBalancedTreeNode<Int32>> getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 1)
                    nodeToBalance = currentNode;
            };
            testWeightBalancedTree.InOrderDepthFirstSearch(getNodeAction);

            testWeightBalancedTree.BalanceTreeUpFromNode(nodeToBalance);

            testWeightBalancedTree.BreadthFirstSearch(populateStoreAction);
            Assert.AreEqual(2, allNodes.Count);
            Assert.IsNull(allNodes[1].ParentNode);
            Assert.IsNull(allNodes[1].LeftChildNode);
            Assert.AreSame(allNodes[2], allNodes[1].RightChildNode);
            Assert.AreSame(allNodes[1], allNodes[2].ParentNode);
            Assert.IsNull(allNodes[2].LeftChildNode);
            Assert.IsNull(allNodes[2].RightChildNode);
            Assert.AreEqual(0, subtreeValueStore[1].Item1);
            Assert.AreEqual(1, subtreeValueStore[1].Item2);
            Assert.AreEqual(0, subtreeValueStore[2].Item1);
            Assert.AreEqual(0, subtreeValueStore[2].Item2);


            // Test that the following tree is not changed if BalanceTreeUpFromNode() is called on node containing 2...
            //     2 
            //    / 
            //   1
            allNodes = new Dictionary<Int32, WeightBalancedTreeNode<Int32>>();
            subtreeValueStore = new Dictionary<Int32, Tuple<Int32, Int32>>();
            testWeightBalancedTree = new WeightBalancedTreeWithProtectedMethods<Int32>(new Int32[] { 2, 1 }, false);
            getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 2)
                    nodeToBalance = currentNode;
            };
            testWeightBalancedTree.InOrderDepthFirstSearch(getNodeAction);

            testWeightBalancedTree.BalanceTreeUpFromNode(nodeToBalance);

            testWeightBalancedTree.BreadthFirstSearch(populateStoreAction);
            Assert.AreEqual(2, allNodes.Count);
            Assert.IsNull(allNodes[2].ParentNode);
            Assert.AreSame(allNodes[1], allNodes[2].LeftChildNode);
            Assert.IsNull(allNodes[2].RightChildNode);
            Assert.AreSame(allNodes[2], allNodes[1].ParentNode);
            Assert.IsNull(allNodes[1].LeftChildNode);
            Assert.IsNull(allNodes[1].RightChildNode);
            Assert.AreEqual(1, subtreeValueStore[2].Item1);
            Assert.AreEqual(0, subtreeValueStore[2].Item2);
            Assert.AreEqual(0, subtreeValueStore[1].Item1);
            Assert.AreEqual(0, subtreeValueStore[1].Item2);


            // Test that the following tree is not changed if BalanceTreeUpFromNode() is called on node containing 2...
            //     2 
            //    / \
            //   1   3
            allNodes = new Dictionary<Int32, WeightBalancedTreeNode<Int32>>();
            subtreeValueStore = new Dictionary<Int32, Tuple<Int32, Int32>>();
            testWeightBalancedTree = new WeightBalancedTreeWithProtectedMethods<Int32>(new Int32[] { 2, 1, 3 }, false);
            getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 2)
                    nodeToBalance = currentNode;
            };
            testWeightBalancedTree.InOrderDepthFirstSearch(getNodeAction);

            testWeightBalancedTree.BalanceTreeUpFromNode(nodeToBalance);

            testWeightBalancedTree.BreadthFirstSearch(populateStoreAction);
            Assert.AreEqual(3, allNodes.Count);
            Assert.IsNull(allNodes[2].ParentNode);
            Assert.AreSame(allNodes[1], allNodes[2].LeftChildNode);
            Assert.AreSame(allNodes[3], allNodes[2].RightChildNode);
            Assert.AreSame(allNodes[2], allNodes[1].ParentNode);
            Assert.IsNull(allNodes[1].LeftChildNode);
            Assert.IsNull(allNodes[1].RightChildNode);
            Assert.AreSame(allNodes[2], allNodes[3].ParentNode);
            Assert.IsNull(allNodes[3].LeftChildNode);
            Assert.IsNull(allNodes[3].RightChildNode);
            Assert.AreEqual(1, subtreeValueStore[2].Item1);
            Assert.AreEqual(1, subtreeValueStore[2].Item2);
            Assert.AreEqual(0, subtreeValueStore[1].Item1);
            Assert.AreEqual(0, subtreeValueStore[1].Item2);
            Assert.AreEqual(0, subtreeValueStore[3].Item1);
            Assert.AreEqual(0, subtreeValueStore[3].Item2);
        }

        /// <summary>
        /// Success tests for the BalanceTreeUpFromNode() method for a small tree with 3 nodes which requires a left zig-zag operation.
        /// </summary>
        [Test]
        public void BalanceTreeUpFromNode_3NodesRequiresLeftZigZag()
        {
            // Test by balancing node 2 in the following tree...
            //    1
            //     \
            //      3
            //     /
            //    2
            //
            // ...and expect the result to look like...
            //      2
            //     / \
            //    1   3
            var allNodes = new Dictionary<Int32, WeightBalancedTreeNode<Int32>>();
            var subtreeValueStore = new Dictionary<Int32, Tuple<Int32, Int32>>();
            Action<WeightBalancedTreeNode<Int32>> populateStoreAction = (node) =>
            {
                allNodes.Add(node.Item, node);
                subtreeValueStore.Add(node.Item, new Tuple<Int32, Int32>(node.LeftSubtreeSize, node.RightSubtreeSize));
            };
            WeightBalancedTreeWithProtectedMethods<Int32> testWeightBalancedTree = new WeightBalancedTreeWithProtectedMethods<Int32>(new Int32[] { 1, 3, 2 }, false);
            WeightBalancedTreeNode<Int32> nodeToBalance = null;
            Action<WeightBalancedTreeNode<Int32>> getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 2)
                    nodeToBalance = currentNode;
            };
            testWeightBalancedTree.InOrderDepthFirstSearch(getNodeAction);

            testWeightBalancedTree.BalanceTreeUpFromNode(nodeToBalance);

            testWeightBalancedTree.BreadthFirstSearch(populateStoreAction);
            Assert.AreEqual(3, allNodes.Count);
            Assert.IsNull(allNodes[2].ParentNode);
            Assert.AreSame(allNodes[1], allNodes[2].LeftChildNode);
            Assert.AreSame(allNodes[3], allNodes[2].RightChildNode);
            Assert.AreSame(allNodes[2], allNodes[1].ParentNode);
            Assert.IsNull(allNodes[1].LeftChildNode);
            Assert.IsNull(allNodes[1].RightChildNode);
            Assert.AreSame(allNodes[2], allNodes[3].ParentNode);
            Assert.IsNull(allNodes[3].LeftChildNode);
            Assert.IsNull(allNodes[3].RightChildNode);
            Assert.AreEqual(1, subtreeValueStore[2].Item1);
            Assert.AreEqual(1, subtreeValueStore[2].Item2);
            Assert.AreEqual(0, subtreeValueStore[1].Item1);
            Assert.AreEqual(0, subtreeValueStore[1].Item2);
            Assert.AreEqual(0, subtreeValueStore[3].Item1);
            Assert.AreEqual(0, subtreeValueStore[3].Item2);
        }

        /// <summary>
        /// Success tests for the BalanceTreeUpFromNode() method for a small tree with 3 nodes which requires a right zig-zag operation.
        /// </summary>
        [Test]
        public void BalanceTreeUpFromNode_3NodesRequiresRightZigZag()
        {
            // Test by balancing node 2 in the following tree...
            //      3
            //     /
            //    1 
            //     \
            //      2
            //
            // ...and expect the result to look like...
            //      2
            //     / \
            //    1   3
            var allNodes = new Dictionary<Int32, WeightBalancedTreeNode<Int32>>();
            var subtreeValueStore = new Dictionary<Int32, Tuple<Int32, Int32>>();
            Action<WeightBalancedTreeNode<Int32>> populateStoreAction = (node) =>
            {
                allNodes.Add(node.Item, node);
                subtreeValueStore.Add(node.Item, new Tuple<Int32, Int32>(node.LeftSubtreeSize, node.RightSubtreeSize));
            };
            WeightBalancedTreeWithProtectedMethods<Int32> testWeightBalancedTree = new WeightBalancedTreeWithProtectedMethods<Int32>(new Int32[] { 3, 1, 2 }, false);
            WeightBalancedTreeNode<Int32> nodeToBalance = null;
            Action<WeightBalancedTreeNode<Int32>> getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 2)
                    nodeToBalance = currentNode;
            };
            testWeightBalancedTree.InOrderDepthFirstSearch(getNodeAction);

            testWeightBalancedTree.BalanceTreeUpFromNode(nodeToBalance);

            testWeightBalancedTree.BreadthFirstSearch(populateStoreAction);
            Assert.AreEqual(3, allNodes.Count);
            Assert.IsNull(allNodes[2].ParentNode);
            Assert.AreSame(allNodes[1], allNodes[2].LeftChildNode);
            Assert.AreSame(allNodes[3], allNodes[2].RightChildNode);
            Assert.AreSame(allNodes[2], allNodes[1].ParentNode);
            Assert.IsNull(allNodes[1].LeftChildNode);
            Assert.IsNull(allNodes[1].RightChildNode);
            Assert.AreSame(allNodes[2], allNodes[3].ParentNode);
            Assert.IsNull(allNodes[3].LeftChildNode);
            Assert.IsNull(allNodes[3].RightChildNode);
            Assert.AreEqual(1, subtreeValueStore[2].Item1);
            Assert.AreEqual(1, subtreeValueStore[2].Item2);
            Assert.AreEqual(0, subtreeValueStore[1].Item1);
            Assert.AreEqual(0, subtreeValueStore[1].Item2);
            Assert.AreEqual(0, subtreeValueStore[3].Item1);
            Assert.AreEqual(0, subtreeValueStore[3].Item2);
        }

        /// <summary>
        /// Success tests for the BalanceTreeUpFromNode() method for trees with 6 nodes which requires a right zig-zag operation.
        /// </summary>
        [Test]
        public void BalanceTreeUpFromNode_6NodesRequiresRightZigZag()
        {
            // Test by balancing node 3 in the following tree...
            //      5 
            //     / \ 
            //    2   6
            //   / \
            //  1   3
            //       \
            //        4
            //
            // ...and expect the result to look like...
            //      3 
            //     / \ 
            //    2   5
            //   /   / \
            //  1   4   6
            var allNodes = new Dictionary<Int32, WeightBalancedTreeNode<Int32>>();
            var subtreeValueStore = new Dictionary<Int32, Tuple<Int32, Int32>>();
            Action<WeightBalancedTreeNode<Int32>> populateStoreAction = (node) =>
            {
                allNodes.Add(node.Item, node);
                subtreeValueStore.Add(node.Item, new Tuple<Int32, Int32>(node.LeftSubtreeSize, node.RightSubtreeSize));
            };
            WeightBalancedTreeWithProtectedMethods<Int32> testWeightBalancedTree = new WeightBalancedTreeWithProtectedMethods<Int32>(new Int32[] { 5, 2, 6, 1, 3, 4 }, false);
            WeightBalancedTreeNode<Int32> nodeToBalance = null;
            Action<WeightBalancedTreeNode<Int32>> getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 3)
                    nodeToBalance = currentNode;
            };
            testWeightBalancedTree.InOrderDepthFirstSearch(getNodeAction);

            testWeightBalancedTree.BalanceTreeUpFromNode(nodeToBalance);

            testWeightBalancedTree.BreadthFirstSearch(populateStoreAction);
            Assert.AreEqual(6, allNodes.Count);
            Assert.IsNull(allNodes[3].ParentNode);
            Assert.AreSame(allNodes[2], allNodes[3].LeftChildNode);
            Assert.AreSame(allNodes[5], allNodes[3].RightChildNode);
            Assert.AreSame(allNodes[3], allNodes[2].ParentNode);
            Assert.AreSame(allNodes[1], allNodes[2].LeftChildNode);
            Assert.IsNull(allNodes[2].RightChildNode);
            Assert.AreSame(allNodes[3], allNodes[5].ParentNode);
            Assert.AreSame(allNodes[4], allNodes[5].LeftChildNode);
            Assert.AreSame(allNodes[6], allNodes[5].RightChildNode);
            Assert.AreSame(allNodes[2], allNodes[1].ParentNode);
            Assert.IsNull(allNodes[1].LeftChildNode);
            Assert.IsNull(allNodes[1].RightChildNode);
            Assert.AreSame(allNodes[5], allNodes[4].ParentNode);
            Assert.IsNull(allNodes[4].LeftChildNode);
            Assert.IsNull(allNodes[4].RightChildNode);
            Assert.AreSame(allNodes[5], allNodes[6].ParentNode);
            Assert.IsNull(allNodes[6].LeftChildNode);
            Assert.IsNull(allNodes[6].RightChildNode);
            Assert.AreEqual(2, subtreeValueStore[3].Item1);
            Assert.AreEqual(3, subtreeValueStore[3].Item2);
            Assert.AreEqual(1, subtreeValueStore[2].Item1);
            Assert.AreEqual(0, subtreeValueStore[2].Item2);
            Assert.AreEqual(1, subtreeValueStore[5].Item1);
            Assert.AreEqual(1, subtreeValueStore[5].Item2);
            Assert.AreEqual(0, subtreeValueStore[1].Item1);
            Assert.AreEqual(0, subtreeValueStore[1].Item2);
            Assert.AreEqual(0, subtreeValueStore[4].Item1);
            Assert.AreEqual(0, subtreeValueStore[4].Item2);
            Assert.AreEqual(0, subtreeValueStore[6].Item1);
            Assert.AreEqual(0, subtreeValueStore[6].Item2);


            // Test by balancing node 4 in the following tree...
            //      2 
            //     / \ 
            //    1   5
            //       / \
            //      4   6
            //     /
            //    3
            //
            // ...and expect the result to look like...
            //      4 
            //     / \ 
            //    2   5
            //   / \   \
            //  1   3   6
            allNodes = new Dictionary<Int32, WeightBalancedTreeNode<Int32>>();
            subtreeValueStore = new Dictionary<Int32, Tuple<Int32, Int32>>();
            testWeightBalancedTree = new WeightBalancedTreeWithProtectedMethods<Int32>(new Int32[] { 2, 1, 5, 4, 6, 3 }, false);
            getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 4)
                    nodeToBalance = currentNode;
            };
            testWeightBalancedTree.InOrderDepthFirstSearch(getNodeAction);

            testWeightBalancedTree.BalanceTreeUpFromNode(nodeToBalance);

            testWeightBalancedTree.BreadthFirstSearch(populateStoreAction);
            Assert.AreEqual(6, allNodes.Count);
            Assert.IsNull(allNodes[4].ParentNode);
            Assert.AreSame(allNodes[2], allNodes[4].LeftChildNode);
            Assert.AreSame(allNodes[5], allNodes[4].RightChildNode);
            Assert.AreSame(allNodes[4], allNodes[2].ParentNode);
            Assert.AreSame(allNodes[1], allNodes[2].LeftChildNode);
            Assert.AreSame(allNodes[3], allNodes[2].RightChildNode);
            Assert.AreSame(allNodes[4], allNodes[5].ParentNode);
            Assert.IsNull(allNodes[5].LeftChildNode);
            Assert.AreSame(allNodes[6], allNodes[5].RightChildNode);
            Assert.AreSame(allNodes[2], allNodes[1].ParentNode);
            Assert.IsNull(allNodes[1].LeftChildNode);
            Assert.IsNull(allNodes[1].RightChildNode);
            Assert.AreSame(allNodes[2], allNodes[3].ParentNode);
            Assert.IsNull(allNodes[3].LeftChildNode);
            Assert.IsNull(allNodes[3].RightChildNode);
            Assert.AreSame(allNodes[5], allNodes[6].ParentNode);
            Assert.IsNull(allNodes[6].LeftChildNode);
            Assert.IsNull(allNodes[6].RightChildNode);

            Assert.AreEqual(3, subtreeValueStore[4].Item1);
            Assert.AreEqual(2, subtreeValueStore[4].Item2);
            Assert.AreEqual(1, subtreeValueStore[2].Item1);
            Assert.AreEqual(1, subtreeValueStore[2].Item2);
            Assert.AreEqual(0, subtreeValueStore[5].Item1);
            Assert.AreEqual(1, subtreeValueStore[5].Item2);
            Assert.AreEqual(0, subtreeValueStore[1].Item1);
            Assert.AreEqual(0, subtreeValueStore[1].Item2);
            Assert.AreEqual(0, subtreeValueStore[3].Item1);
            Assert.AreEqual(0, subtreeValueStore[3].Item2);
            Assert.AreEqual(0, subtreeValueStore[6].Item1);
            Assert.AreEqual(0, subtreeValueStore[6].Item2);
        }

        /// <summary>
        /// Success test for the BalanceTreeUpFromNode() method for a tree with 6 nodes requiring right rotation.
        /// </summary>
        [Test]
        public void BalanceTreeUpFromNode_6NodesRequiresRightRotation()
        {
            // Test by balancing node 2 in the following tree...
            //         5
            //        / \
            //       3   6
            //      / \
            //     2   4
            //    /
            //   1
            //
            // ...and expect the result to look like...
            //       3
            //      / \
            //     2   5
            //    /   / \
            //   1   4   6
            var allNodes = new Dictionary<Int32, WeightBalancedTreeNode<Int32>>();
            var subtreeValueStore = new Dictionary<Int32, Tuple<Int32, Int32>>();
            Action<WeightBalancedTreeNode<Int32>> populateStoreAction = (node) =>
            {
                allNodes.Add(node.Item, node);
                subtreeValueStore.Add(node.Item, new Tuple<Int32, Int32>(node.LeftSubtreeSize, node.RightSubtreeSize));
            };
            WeightBalancedTreeWithProtectedMethods<Int32> testWeightBalancedTree = new WeightBalancedTreeWithProtectedMethods<Int32>(new Int32[] { 5, 3, 6, 2, 4, 1 }, false);
            WeightBalancedTreeNode<Int32> nodeToBalance = null;
            Action<WeightBalancedTreeNode<Int32>> getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 2)
                    nodeToBalance = currentNode;
            };
            testWeightBalancedTree.InOrderDepthFirstSearch(getNodeAction);

            testWeightBalancedTree.BalanceTreeUpFromNode(nodeToBalance);

            testWeightBalancedTree.BreadthFirstSearch(populateStoreAction);
            Assert.AreEqual(6, allNodes.Count);
            Assert.IsNull(allNodes[3].ParentNode);
            Assert.AreSame(allNodes[2], allNodes[3].LeftChildNode);
            Assert.AreSame(allNodes[5], allNodes[3].RightChildNode);
            Assert.AreSame(allNodes[3], allNodes[2].ParentNode);
            Assert.AreSame(allNodes[1], allNodes[2].LeftChildNode);
            Assert.IsNull(allNodes[2].RightChildNode);
            Assert.AreSame(allNodes[3], allNodes[5].ParentNode);
            Assert.AreSame(allNodes[4], allNodes[5].LeftChildNode);
            Assert.AreSame(allNodes[6], allNodes[5].RightChildNode);
            Assert.AreSame(allNodes[2], allNodes[1].ParentNode);
            Assert.IsNull(allNodes[1].LeftChildNode);
            Assert.IsNull(allNodes[1].RightChildNode);
            Assert.AreSame(allNodes[5], allNodes[4].ParentNode);
            Assert.IsNull(allNodes[4].LeftChildNode);
            Assert.IsNull(allNodes[4].RightChildNode);
            Assert.AreSame(allNodes[5], allNodes[6].ParentNode);
            Assert.IsNull(allNodes[6].LeftChildNode);
            Assert.IsNull(allNodes[6].RightChildNode);
            Assert.AreEqual(2, subtreeValueStore[3].Item1);
            Assert.AreEqual(3, subtreeValueStore[3].Item2);
            Assert.AreEqual(1, subtreeValueStore[2].Item1);
            Assert.AreEqual(0, subtreeValueStore[2].Item2);
            Assert.AreEqual(1, subtreeValueStore[5].Item1);
            Assert.AreEqual(1, subtreeValueStore[5].Item2);
            Assert.AreEqual(0, subtreeValueStore[1].Item1);
            Assert.AreEqual(0, subtreeValueStore[1].Item2);
            Assert.AreEqual(0, subtreeValueStore[4].Item1);
            Assert.AreEqual(0, subtreeValueStore[4].Item2);
            Assert.AreEqual(0, subtreeValueStore[6].Item1);
            Assert.AreEqual(0, subtreeValueStore[6].Item2);
        }

        /// <summary>
        /// Success test for the BalanceTreeUpFromNode() method for a tree with 6 nodes requiring left rotation.
        /// </summary>
        [Test]
        public void BalanceTreeUpFromNode_6NodesRequiresLeftRotation()
        {
            // Test by balancing node 5 in the following tree...
            //     2
            //    / \
            //   1   4
            //      / \
            //     3   5
            //          \
            //           6
            //
            // ...and expect the result to look like...
            //       4
            //      / \
            //     2   5
            //    / \   \
            //   1   3   6
            var allNodes = new Dictionary<Int32, WeightBalancedTreeNode<Int32>>();
            var subtreeValueStore = new Dictionary<Int32, Tuple<Int32, Int32>>();
            Action<WeightBalancedTreeNode<Int32>> populateStoreAction = (node) =>
            {
                allNodes.Add(node.Item, node);
                subtreeValueStore.Add(node.Item, new Tuple<Int32, Int32>(node.LeftSubtreeSize, node.RightSubtreeSize));
            };
            WeightBalancedTreeWithProtectedMethods<Int32> testWeightBalancedTree = new WeightBalancedTreeWithProtectedMethods<Int32>(new Int32[] { 2, 1, 4, 3, 5, 6 }, false);
            WeightBalancedTreeNode<Int32> nodeToBalance = null;
            Action<WeightBalancedTreeNode<Int32>> getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 5)
                    nodeToBalance = currentNode;
            };
            testWeightBalancedTree.InOrderDepthFirstSearch(getNodeAction);

            testWeightBalancedTree.BalanceTreeUpFromNode(nodeToBalance);

            testWeightBalancedTree.BreadthFirstSearch(populateStoreAction);
            Assert.AreEqual(6, allNodes.Count);
            Assert.IsNull(allNodes[4].ParentNode);
            Assert.AreSame(allNodes[2], allNodes[4].LeftChildNode);
            Assert.AreSame(allNodes[5], allNodes[4].RightChildNode);
            Assert.AreSame(allNodes[4], allNodes[2].ParentNode);
            Assert.AreSame(allNodes[1], allNodes[2].LeftChildNode);
            Assert.AreSame(allNodes[3], allNodes[2].RightChildNode);
            Assert.AreSame(allNodes[4], allNodes[5].ParentNode);
            Assert.IsNull(allNodes[5].LeftChildNode);
            Assert.AreSame(allNodes[6], allNodes[5].RightChildNode);
            Assert.AreSame(allNodes[2], allNodes[1].ParentNode);
            Assert.IsNull(allNodes[1].LeftChildNode);
            Assert.IsNull(allNodes[1].RightChildNode);
            Assert.AreSame(allNodes[2], allNodes[3].ParentNode);
            Assert.IsNull(allNodes[3].LeftChildNode);
            Assert.IsNull(allNodes[3].RightChildNode);
            Assert.AreSame(allNodes[5], allNodes[6].ParentNode);
            Assert.IsNull(allNodes[6].LeftChildNode);
            Assert.IsNull(allNodes[6].RightChildNode);
            Assert.AreEqual(3, subtreeValueStore[4].Item1);
            Assert.AreEqual(2, subtreeValueStore[4].Item2);
            Assert.AreEqual(1, subtreeValueStore[2].Item1);
            Assert.AreEqual(1, subtreeValueStore[2].Item2);
            Assert.AreEqual(0, subtreeValueStore[5].Item1);
            Assert.AreEqual(1, subtreeValueStore[5].Item2);
            Assert.AreEqual(0, subtreeValueStore[1].Item1);
            Assert.AreEqual(0, subtreeValueStore[1].Item2);
            Assert.AreEqual(0, subtreeValueStore[3].Item1);
            Assert.AreEqual(0, subtreeValueStore[3].Item2);
            Assert.AreEqual(0, subtreeValueStore[6].Item1);
            Assert.AreEqual(0, subtreeValueStore[6].Item2);
        }

        /// <summary>
        /// Success test for the BalanceTreeUpFromNode() method for a tree with requires a left zig-zag operation.
        /// </summary>
        [Test]
        public void BalanceTreeUpFromNode_IncludesLeftZigZag()
        {
            // Test by balancing node 3 in the following tree...
            //     1
            //      \
            //       5
            //      / 
            //     2  
            //      \
            //       4
            //      /
            //     3
            //
            // ...and expect the result to look like...
            //        3
            //      /   \
            //     1     5
            //      \   /
            //       2 4
            var allNodes = new Dictionary<Int32, WeightBalancedTreeNode<Int32>>();
            var subtreeValueStore = new Dictionary<Int32, Tuple<Int32, Int32>>();
            Action<WeightBalancedTreeNode<Int32>> populateStoreAction = (node) =>
            {
                allNodes.Add(node.Item, node);
                subtreeValueStore.Add(node.Item, new Tuple<Int32, Int32>(node.LeftSubtreeSize, node.RightSubtreeSize));
            };
            WeightBalancedTreeWithProtectedMethods<Int32> testWeightBalancedTree = new WeightBalancedTreeWithProtectedMethods<Int32>(new Int32[] { 1, 5, 2, 4, 3 }, false);
            WeightBalancedTreeNode<Int32> nodeToBalance = null;
            Action<WeightBalancedTreeNode<Int32>> getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 3)
                    nodeToBalance = currentNode;
            };
            testWeightBalancedTree.InOrderDepthFirstSearch(getNodeAction);

            testWeightBalancedTree.BalanceTreeUpFromNode(nodeToBalance);

            testWeightBalancedTree.BreadthFirstSearch(populateStoreAction);
            Assert.AreEqual(5, allNodes.Count);
            Assert.IsNull(allNodes[3].ParentNode);
            Assert.AreSame(allNodes[1], allNodes[3].LeftChildNode);
            Assert.AreSame(allNodes[5], allNodes[3].RightChildNode);
            Assert.AreSame(allNodes[3], allNodes[1].ParentNode);
            Assert.IsNull(allNodes[1].LeftChildNode);
            Assert.AreSame(allNodes[2], allNodes[1].RightChildNode);
            Assert.AreSame(allNodes[3], allNodes[5].ParentNode);
            Assert.AreSame(allNodes[4], allNodes[5].LeftChildNode);
            Assert.IsNull(allNodes[5].RightChildNode);
            Assert.AreSame(allNodes[1], allNodes[2].ParentNode);
            Assert.IsNull(allNodes[2].LeftChildNode);
            Assert.IsNull(allNodes[2].RightChildNode);
            Assert.AreSame(allNodes[5], allNodes[4].ParentNode);
            Assert.IsNull(allNodes[4].LeftChildNode);
            Assert.IsNull(allNodes[4].RightChildNode);
            Assert.AreEqual(2, subtreeValueStore[3].Item1);
            Assert.AreEqual(2, subtreeValueStore[3].Item2);
            Assert.AreEqual(0, subtreeValueStore[1].Item1);
            Assert.AreEqual(1, subtreeValueStore[1].Item2);
            Assert.AreEqual(1, subtreeValueStore[5].Item1);
            Assert.AreEqual(0, subtreeValueStore[5].Item2);
            Assert.AreEqual(0, subtreeValueStore[2].Item1);
            Assert.AreEqual(0, subtreeValueStore[2].Item2);
            Assert.AreEqual(0, subtreeValueStore[4].Item1);
            Assert.AreEqual(0, subtreeValueStore[4].Item2);
        }

        /// <summary>
        /// Success test for the BalanceTreeUpFromNode() method for a tree with requires a right zig-zag operation.
        /// </summary>
        [Test]
        public void BalanceTreeUpFromNode_IncludesRightZigZag()
        {
            // Test by balancing node 3 in the following tree...
            //     5
            //    /
            //   1
            //    \ 
            //     4  
            //    /
            //   2
            //    \
            //     3
            //
            // ...and expect the result to look like...
            //      3
            //    /   \
            //   1     5
            //    \   /
            //     2 4
            var allNodes = new Dictionary<Int32, WeightBalancedTreeNode<Int32>>();
            var subtreeValueStore = new Dictionary<Int32, Tuple<Int32, Int32>>();
            Action<WeightBalancedTreeNode<Int32>> populateStoreAction = (node) =>
            {
                allNodes.Add(node.Item, node);
                subtreeValueStore.Add(node.Item, new Tuple<Int32, Int32>(node.LeftSubtreeSize, node.RightSubtreeSize));
            };
            WeightBalancedTreeWithProtectedMethods<Int32> testWeightBalancedTree = new WeightBalancedTreeWithProtectedMethods<Int32>(new Int32[] { 5, 1, 4, 2, 3 }, false);
            WeightBalancedTreeNode<Int32> nodeToBalance = null;
            Action<WeightBalancedTreeNode<Int32>> getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 3)
                    nodeToBalance = currentNode;
            };
            testWeightBalancedTree.InOrderDepthFirstSearch(getNodeAction);

            testWeightBalancedTree.BalanceTreeUpFromNode(nodeToBalance);

            testWeightBalancedTree.BreadthFirstSearch(populateStoreAction);
            Assert.AreEqual(5, allNodes.Count);
            Assert.IsNull(allNodes[3].ParentNode);
            Assert.AreSame(allNodes[1], allNodes[3].LeftChildNode);
            Assert.AreSame(allNodes[5], allNodes[3].RightChildNode);
            Assert.AreSame(allNodes[3], allNodes[1].ParentNode);
            Assert.IsNull(allNodes[1].LeftChildNode);
            Assert.AreSame(allNodes[2], allNodes[1].RightChildNode);
            Assert.AreSame(allNodes[3], allNodes[5].ParentNode);
            Assert.AreSame(allNodes[4], allNodes[5].LeftChildNode);
            Assert.IsNull(allNodes[5].RightChildNode);
            Assert.AreSame(allNodes[1], allNodes[2].ParentNode);
            Assert.IsNull(allNodes[2].LeftChildNode);
            Assert.IsNull(allNodes[2].RightChildNode);
            Assert.AreSame(allNodes[5], allNodes[4].ParentNode);
            Assert.IsNull(allNodes[4].LeftChildNode);
            Assert.IsNull(allNodes[4].RightChildNode);
            Assert.AreEqual(2, subtreeValueStore[3].Item1);
            Assert.AreEqual(2, subtreeValueStore[3].Item2);
            Assert.AreEqual(0, subtreeValueStore[1].Item1);
            Assert.AreEqual(1, subtreeValueStore[1].Item2);
            Assert.AreEqual(1, subtreeValueStore[5].Item1);
            Assert.AreEqual(0, subtreeValueStore[5].Item2);
            Assert.AreEqual(0, subtreeValueStore[2].Item1);
            Assert.AreEqual(0, subtreeValueStore[2].Item2);
            Assert.AreEqual(0, subtreeValueStore[4].Item1);
            Assert.AreEqual(0, subtreeValueStore[4].Item2);
        }

        /// <summary>
        /// Success tests for the WillLeftZigZagOperationImproveBalance() method.
        /// </summary>
        [Test]
        public void WillLeftZigZagOperationImproveBalance()
        {
            // Test with node 3 in the following tree
            //    3 
            //     \ 
            //      5   
            //     /
            //    4
            WeightBalancedTreeWithProtectedMethods<Int32> testWeightBalancedTree = new WeightBalancedTreeWithProtectedMethods<Int32>(new Int32[] { 3, 5, 4 }, false);
            WeightBalancedTreeNode<Int32> nodeToAssess = null;
            Action<WeightBalancedTreeNode<Int32>> getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 3)
                    nodeToAssess = currentNode;
            };
            testWeightBalancedTree.BreadthFirstSearch(getNodeAction);

            Boolean result = testWeightBalancedTree.WillLeftZigZagOperationImproveBalance(nodeToAssess);

            Assert.IsTrue(result);


            // Test with nodes 3 and 5 in the following tree
            //    3 
            //     \ 
            //      5   
            testWeightBalancedTree = new WeightBalancedTreeWithProtectedMethods<Int32>(new Int32[] { 3, 5 }, false);
            nodeToAssess = null;
            getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 3)
                    nodeToAssess = currentNode;
            };
            testWeightBalancedTree.BreadthFirstSearch(getNodeAction);

            result = testWeightBalancedTree.WillLeftZigZagOperationImproveBalance(nodeToAssess);

            Assert.IsFalse(result);

            getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 5)
                    nodeToAssess = currentNode;
            };
            testWeightBalancedTree.BreadthFirstSearch(getNodeAction);

            result = testWeightBalancedTree.WillLeftZigZagOperationImproveBalance(nodeToAssess);

            Assert.IsFalse(result);


            // Test with node 3 in the following tree
            //     3
            //    / \
            //   2   5
            //      / \
            //     4   6
            testWeightBalancedTree = new WeightBalancedTreeWithProtectedMethods<Int32>(new Int32[] { 3, 2, 5, 4, 6 }, false);
            nodeToAssess = null;
            getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 3)
                    nodeToAssess = currentNode;
            };
            testWeightBalancedTree.BreadthFirstSearch(getNodeAction);

            result = testWeightBalancedTree.WillLeftZigZagOperationImproveBalance(nodeToAssess);

            Assert.IsFalse(result);
        }

        /// <summary>
        /// Success tests for the WillRightZigZagOperationImproveBalance() method.
        /// </summary>
        [Test]
        public void WillRightZigZagOperationImproveBalance()
        {
            // Test with node 5 in the following tree
            //      5
            //     / 
            //    3   
            //     \
            //      4
            WeightBalancedTreeWithProtectedMethods<Int32> testWeightBalancedTree = new WeightBalancedTreeWithProtectedMethods<Int32>(new Int32[] { 5, 3, 4 }, false);
            WeightBalancedTreeNode<Int32> nodeToAssess = null;
            Action<WeightBalancedTreeNode<Int32>> getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 5)
                    nodeToAssess = currentNode;
            };
            testWeightBalancedTree.BreadthFirstSearch(getNodeAction);

            Boolean result = testWeightBalancedTree.WillRightZigZagOperationImproveBalance(nodeToAssess);

            Assert.IsTrue(result);


            // Test with nodes 5 and 3 in the following tree
            //      5
            //     / 
            //    3   
            testWeightBalancedTree = new WeightBalancedTreeWithProtectedMethods<Int32>(new Int32[] { 5, 3 }, false);
            nodeToAssess = null;
            getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 5)
                    nodeToAssess = currentNode;
            };
            testWeightBalancedTree.BreadthFirstSearch(getNodeAction);

            result = testWeightBalancedTree.WillRightZigZagOperationImproveBalance(nodeToAssess);

            Assert.IsFalse(result);

            getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 3)
                    nodeToAssess = currentNode;
            };
            testWeightBalancedTree.BreadthFirstSearch(getNodeAction);

            result = testWeightBalancedTree.WillRightZigZagOperationImproveBalance(nodeToAssess);

            Assert.IsFalse(result);


            // Test with node 5 in the following tree
            //      5
            //     / \
            //    3   6
            //   / \
            //  2   4
            testWeightBalancedTree = new WeightBalancedTreeWithProtectedMethods<Int32>(new Int32[] { 5, 3, 6, 2, 4 }, false);
            nodeToAssess = null;
            getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 5)
                    nodeToAssess = currentNode;
            };
            testWeightBalancedTree.BreadthFirstSearch(getNodeAction);

            result = testWeightBalancedTree.WillRightZigZagOperationImproveBalance(nodeToAssess);

            Assert.IsFalse(result);
        }

        /// <summary>
        /// Success test for the ZigZagNodeRight() method for a synthetic tree (with fake subtree sizes).
        /// </summary>
        [Test]
        public void ZigZagNodeRight_SyntheticTree()
        {
            // Test by performing a right zig-zag operation on node 6 in the following tree...
            //                    (10)
            //                 131    31
            //                 /
            //               (8)
            //             77   53
            //            /       \
            //         (4)        (9)
            //       13   63    23   29
            //      /       \
            //    (3)       (6)
            //   5   7    25   37
            //            /     \
            //         (5)       (7)
            //       11   13   17   19
            //
            // ...and expect the result to look like...
            //                     (10)
            //                  131    31
            //                  / 
            //                (6)   
            //            39       91
            //           /           \   
            //        (4)             (8)
            //      13   25         37   53
            //     /       \       /       \
            //    (3)     (5)     (7)     (9)
            //   5   7  11   13 17   19 23   29
            var allNodes = new Dictionary<Int32, WeightBalancedTreeNode<Int32>>();
            var subtreeValueStore = new Dictionary<Int32, Tuple<Int32, Int32>>();
            Action<WeightBalancedTreeNode<Int32>> populateStoreAction = (node) =>
            {
                allNodes.Add(node.Item, node);
                subtreeValueStore.Add(node.Item, new Tuple<Int32, Int32>(node.LeftSubtreeSize, node.RightSubtreeSize));
            };
            WeightBalancedTreeWithProtectedMethods<Int32> testWeightBalancedTree = new WeightBalancedTreeWithProtectedMethods<Int32>(new Int32[] { 10, 8, 4, 9, 3, 6, 5, 7 }, false);
            WeightBalancedTreeNode<Int32> nodeToZigZag = null;
            Action<WeightBalancedTreeNode<Int32>> subtreeSizeUpdateAction = ((currentNode) => 
            {
                switch(currentNode.Item)
                {
                    case 10:
                        currentNode.LeftSubtreeSize = 131;
                        currentNode.RightSubtreeSize = 31;
                        break;
                    case 8:
                        currentNode.LeftSubtreeSize = 77;
                        currentNode.RightSubtreeSize = 53;
                        break;
                    case 4:
                        currentNode.LeftSubtreeSize = 13;
                        currentNode.RightSubtreeSize = 63;
                        break;
                    case 9:
                        currentNode.LeftSubtreeSize = 23;
                        currentNode.RightSubtreeSize = 29;
                        break;
                    case 3:
                        currentNode.LeftSubtreeSize = 5;
                        currentNode.RightSubtreeSize = 7;
                        break;
                    case 6:
                        currentNode.LeftSubtreeSize = 25;
                        currentNode.RightSubtreeSize = 37;
                        nodeToZigZag = currentNode;
                        break;
                    case 5:
                        currentNode.LeftSubtreeSize = 11;
                        currentNode.RightSubtreeSize = 13;
                        break;
                    case 7:
                        currentNode.LeftSubtreeSize = 17;
                        currentNode.RightSubtreeSize = 19;
                        break;
                }
            });
            testWeightBalancedTree.BreadthFirstSearch(subtreeSizeUpdateAction);

            testWeightBalancedTree.ZigZagNodeRight(nodeToZigZag);

            testWeightBalancedTree.BreadthFirstSearch(populateStoreAction);
            Assert.AreEqual(8, allNodes.Count);
            Assert.IsNull(allNodes[10].ParentNode);
            Assert.AreSame(allNodes[6], allNodes[10].LeftChildNode);
            Assert.IsNull(allNodes[10].RightChildNode);
            Assert.AreSame(allNodes[10], allNodes[6].ParentNode);
            Assert.AreSame(allNodes[4], allNodes[6].LeftChildNode);
            Assert.AreSame(allNodes[8], allNodes[6].RightChildNode);
            Assert.AreSame(allNodes[6], allNodes[4].ParentNode);
            Assert.AreSame(allNodes[3], allNodes[4].LeftChildNode);
            Assert.AreSame(allNodes[5], allNodes[4].RightChildNode);
            Assert.AreSame(allNodes[6], allNodes[8].ParentNode);
            Assert.AreSame(allNodes[7], allNodes[8].LeftChildNode);
            Assert.AreSame(allNodes[9], allNodes[8].RightChildNode);
            Assert.AreSame(allNodes[4], allNodes[3].ParentNode);
            Assert.IsNull(allNodes[3].LeftChildNode);
            Assert.IsNull(allNodes[3].RightChildNode);
            Assert.AreSame(allNodes[4], allNodes[5].ParentNode);
            Assert.IsNull(allNodes[5].LeftChildNode);
            Assert.IsNull(allNodes[5].RightChildNode);
            Assert.AreSame(allNodes[8], allNodes[7].ParentNode);
            Assert.IsNull(allNodes[7].LeftChildNode);
            Assert.IsNull(allNodes[7].RightChildNode);
            Assert.AreSame(allNodes[8], allNodes[9].ParentNode);
            Assert.IsNull(allNodes[9].LeftChildNode);
            Assert.IsNull(allNodes[9].RightChildNode);
            Assert.AreEqual(131, subtreeValueStore[10].Item1);
            Assert.AreEqual(31, subtreeValueStore[10].Item2);
            Assert.AreEqual(39, subtreeValueStore[6].Item1);
            Assert.AreEqual(91, subtreeValueStore[6].Item2);
            Assert.AreEqual(13, subtreeValueStore[4].Item1);
            Assert.AreEqual(25, subtreeValueStore[4].Item2);
            Assert.AreEqual(37, subtreeValueStore[8].Item1);
            Assert.AreEqual(53, subtreeValueStore[8].Item2);
            Assert.AreEqual(5, subtreeValueStore[3].Item1);
            Assert.AreEqual(7, subtreeValueStore[3].Item2);
            Assert.AreEqual(11, subtreeValueStore[5].Item1);
            Assert.AreEqual(13, subtreeValueStore[5].Item2);
            Assert.AreEqual(17, subtreeValueStore[7].Item1);
            Assert.AreEqual(19, subtreeValueStore[7].Item2);
            Assert.AreEqual(23, subtreeValueStore[9].Item1);
            Assert.AreEqual(29, subtreeValueStore[9].Item2);
        }

        /// <summary>
        /// Success test for the ZigZagNodeRight() method where the nodes within the zig-zag shape all have children (and parents).
        /// </summary>
        [Test]
        public void ZigZagNodeRight_ZigZagNodesHaveChildren()
        {
            // Test by performing a right zig-zag operation on node 6 in the following tree...
            //         10
            //        /
            //       8
            //      / \
            //     4   9
            //    / \
            //   3   6
            //      / \
            //     5   7
            //
            // ...and expect the result to look like...
            //          10
            //         / 
            //        6
            //      /   \   
            //     4     8
            //    / \   / \
            //   3   5 7   9
            var allNodes = new Dictionary<Int32, WeightBalancedTreeNode<Int32>>();
            var subtreeValueStore = new Dictionary<Int32, Tuple<Int32, Int32>>();
            Action<WeightBalancedTreeNode<Int32>> populateStoreAction = (node) =>
            {
                allNodes.Add(node.Item, node);
                subtreeValueStore.Add(node.Item, new Tuple<Int32, Int32>(node.LeftSubtreeSize, node.RightSubtreeSize));
            };
            WeightBalancedTreeWithProtectedMethods<Int32> testWeightBalancedTree = new WeightBalancedTreeWithProtectedMethods<Int32>(new Int32[] { 10, 8, 4, 9, 3, 6, 5, 7 }, false);
            WeightBalancedTreeNode<Int32> nodeToZigZag = null;
            Action<WeightBalancedTreeNode<Int32>> getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 6)
                    nodeToZigZag = currentNode;
            };
            testWeightBalancedTree.BreadthFirstSearch(getNodeAction);

            testWeightBalancedTree.ZigZagNodeRight(nodeToZigZag);

            testWeightBalancedTree.BreadthFirstSearch(populateStoreAction);
            Assert.AreEqual(8, allNodes.Count);
            Assert.IsNull(allNodes[10].ParentNode);
            Assert.AreSame(allNodes[6], allNodes[10].LeftChildNode);
            Assert.IsNull(allNodes[10].RightChildNode);
            Assert.AreSame(allNodes[10], allNodes[6].ParentNode);
            Assert.AreSame(allNodes[4], allNodes[6].LeftChildNode);
            Assert.AreSame(allNodes[8], allNodes[6].RightChildNode);
            Assert.AreSame(allNodes[6], allNodes[4].ParentNode);
            Assert.AreSame(allNodes[3], allNodes[4].LeftChildNode);
            Assert.AreSame(allNodes[5], allNodes[4].RightChildNode);
            Assert.AreSame(allNodes[6], allNodes[8].ParentNode);
            Assert.AreSame(allNodes[7], allNodes[8].LeftChildNode);
            Assert.AreSame(allNodes[9], allNodes[8].RightChildNode);
            Assert.AreSame(allNodes[4], allNodes[3].ParentNode);
            Assert.IsNull(allNodes[3].LeftChildNode);
            Assert.IsNull(allNodes[3].RightChildNode);
            Assert.AreSame(allNodes[4], allNodes[5].ParentNode);
            Assert.IsNull(allNodes[5].LeftChildNode);
            Assert.IsNull(allNodes[5].RightChildNode);
            Assert.AreSame(allNodes[8], allNodes[7].ParentNode);
            Assert.IsNull(allNodes[7].LeftChildNode);
            Assert.IsNull(allNodes[7].RightChildNode);
            Assert.AreSame(allNodes[8], allNodes[9].ParentNode);
            Assert.IsNull(allNodes[9].LeftChildNode);
            Assert.IsNull(allNodes[9].RightChildNode);
            Assert.AreEqual(7, subtreeValueStore[10].Item1);
            Assert.AreEqual(0, subtreeValueStore[10].Item2);
            Assert.AreEqual(3, subtreeValueStore[6].Item1);
            Assert.AreEqual(3, subtreeValueStore[6].Item2);
            Assert.AreEqual(1, subtreeValueStore[4].Item1);
            Assert.AreEqual(1, subtreeValueStore[4].Item2);
            Assert.AreEqual(1, subtreeValueStore[8].Item1);
            Assert.AreEqual(1, subtreeValueStore[8].Item2);
            Assert.AreEqual(0, subtreeValueStore[3].Item1);
            Assert.AreEqual(0, subtreeValueStore[3].Item2);
            Assert.AreEqual(0, subtreeValueStore[5].Item1);
            Assert.AreEqual(0, subtreeValueStore[5].Item2);
            Assert.AreEqual(0, subtreeValueStore[7].Item1);
            Assert.AreEqual(0, subtreeValueStore[7].Item2);
            Assert.AreEqual(0, subtreeValueStore[9].Item1);
            Assert.AreEqual(0, subtreeValueStore[9].Item2);
        }

        /// <summary>
        /// Success test for the ZigZagNodeRight() method where the nodes within the zig-zag shape don't have children (nor parents).
        /// </summary>
        [Test]
        public void ZigZagNodeRight_ZigZagNodesHaveNoChildren()
        {
            // Test by performing a right zig-zag operation on node 6 in the following tree...
            //     8
            //    / 
            //   4   
            //    \
            //     6
            //
            // ...and expect the result to look like...
            //     6
            //    / \   
            //   4   8
            var allNodes = new Dictionary<Int32, WeightBalancedTreeNode<Int32>>();
            var subtreeValueStore = new Dictionary<Int32, Tuple<Int32, Int32>>();
            Action<WeightBalancedTreeNode<Int32>> populateStoreAction = (node) =>
            {
                allNodes.Add(node.Item, node);
                subtreeValueStore.Add(node.Item, new Tuple<Int32, Int32>(node.LeftSubtreeSize, node.RightSubtreeSize));
            };
            WeightBalancedTreeWithProtectedMethods<Int32> testWeightBalancedTree = new WeightBalancedTreeWithProtectedMethods<Int32>(new Int32[] { 8, 4, 6 }, false);
            WeightBalancedTreeNode<Int32> nodeToZigZag = null;
            Action<WeightBalancedTreeNode<Int32>> getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 6)
                    nodeToZigZag = currentNode;
            };
            testWeightBalancedTree.BreadthFirstSearch(getNodeAction);

            testWeightBalancedTree.ZigZagNodeRight(nodeToZigZag);

            testWeightBalancedTree.BreadthFirstSearch(populateStoreAction);
            Assert.AreEqual(3, allNodes.Count);
            Assert.IsNull(allNodes[6].ParentNode);
            Assert.AreSame(allNodes[4], allNodes[6].LeftChildNode);
            Assert.AreSame(allNodes[8], allNodes[6].RightChildNode);
            Assert.AreSame(allNodes[6], allNodes[4].ParentNode);
            Assert.IsNull(allNodes[4].LeftChildNode);
            Assert.IsNull(allNodes[4].RightChildNode);
            Assert.AreSame(allNodes[6], allNodes[8].ParentNode);
            Assert.IsNull(allNodes[8].LeftChildNode);
            Assert.IsNull(allNodes[8].RightChildNode);
            Assert.AreEqual(1, subtreeValueStore[6].Item1);
            Assert.AreEqual(1, subtreeValueStore[6].Item2);
            Assert.AreEqual(0, subtreeValueStore[4].Item1);
            Assert.AreEqual(0, subtreeValueStore[4].Item2);
            Assert.AreEqual(0, subtreeValueStore[8].Item1);
            Assert.AreEqual(0, subtreeValueStore[8].Item2);
        }

        /// <summary>
        /// Test that an exception is thrown if the ZigZagNodeRight() method is called on a node which is a left child.
        /// </summary>
        [Test]
        public void ZigZagNodeRight_ZigZagNodeIsALeftChild()
        {
            WeightBalancedTreeWithProtectedMethods<Int32> testWeightBalancedTree = new WeightBalancedTreeWithProtectedMethods<Int32>(new Int32[] { 3, 2, 1 }, false);
            WeightBalancedTreeNode<Int32> nodeToZigZag = null;
            Action<WeightBalancedTreeNode<Int32>> getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 1)
                    nodeToZigZag = currentNode;
            };
            testWeightBalancedTree.BreadthFirstSearch(getNodeAction);

            var e = Assert.Throws<InvalidOperationException>(delegate
            {
                testWeightBalancedTree.ZigZagNodeRight(nodeToZigZag);
            });

            Assert.That(e.Message, Does.StartWith("The node containing item '1' cannot have a right zig-zag operation applied as it is a left child of its parent."));
        }

        /// <summary>
        /// Test that an exception is thrown if the ZigZagNodeRight() method is called on a node whose parent is a right child.
        /// </summary>
        [Test]
        public void ZigZagNodeRight_ZigZagNodesParentIsARightChild()
        {
            WeightBalancedTreeWithProtectedMethods<Int32> testWeightBalancedTree = new WeightBalancedTreeWithProtectedMethods<Int32>(new Int32[] { 1, 2, 3 }, false);
            WeightBalancedTreeNode<Int32> nodeToZigZag = null;
            Action<WeightBalancedTreeNode<Int32>> getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 3)
                    nodeToZigZag = currentNode;
            };
            testWeightBalancedTree.BreadthFirstSearch(getNodeAction);

            var e = Assert.Throws<InvalidOperationException>(delegate
            {
                testWeightBalancedTree.ZigZagNodeRight(nodeToZigZag);
            });

            Assert.That(e.Message, Does.StartWith("The node containing item '3' cannot have a right zig-zag operation applied as its parent is a right child of its grandparent."));
        }

        /// <summary>
        /// Success test for the ZigZagNodeLeft() method for a synthetic tree (with fake subtree sizes).
        /// </summary>
        [Test]
        public void ZigZagNodeLeft_SyntheticTree()
        {
            // Test by performing a left zig-zag operation on node 7 in the following tree...
            //     (3)
            //   31   131
            //           \
            //           (5)
            //         53   77
            //        /       \
            //      (4)       (9)
            //    29   23   63   13
            //             /       \
            //           (7)      (10)
            //         37   25   7    5
            //         /     \
            //      (6)       (8)
            //    19   17   13   11
            //
            // ...and expect the result to look like...
            //          (3)
            //          31   131
            //                 \
            //                 (7)
            //             91       39
            //            /           \
            //         (5)             (9)
            //       53   37         25   13
            //      /       \       /       \
            //    (4)      (6)     (8)      (10)
            //  29   23  19   17 13   11   7    5
            var allNodes = new Dictionary<Int32, WeightBalancedTreeNode<Int32>>();
            var subtreeValueStore = new Dictionary<Int32, Tuple<Int32, Int32>>();
            Action<WeightBalancedTreeNode<Int32>> populateStoreAction = (node) =>
            {
                allNodes.Add(node.Item, node);
                subtreeValueStore.Add(node.Item, new Tuple<Int32, Int32>(node.LeftSubtreeSize, node.RightSubtreeSize));
            };
            WeightBalancedTreeWithProtectedMethods<Int32> testWeightBalancedTree = new WeightBalancedTreeWithProtectedMethods<Int32>(new Int32[] { 3, 5, 4, 9, 7, 10, 6, 8 }, false);
            WeightBalancedTreeNode<Int32> nodeToZigZag = null;
            Action<WeightBalancedTreeNode<Int32>> subtreeSizeUpdateAction = ((currentNode) =>
            {
                switch (currentNode.Item)
                {
                    case 3:
                        currentNode.LeftSubtreeSize = 31;
                        currentNode.RightSubtreeSize = 131;
                        break;
                    case 5:
                        currentNode.LeftSubtreeSize = 53;
                        currentNode.RightSubtreeSize = 77;
                        break;
                    case 4:
                        currentNode.LeftSubtreeSize = 29;
                        currentNode.RightSubtreeSize = 23;
                        break;
                    case 9:
                        currentNode.LeftSubtreeSize = 63;
                        currentNode.RightSubtreeSize = 13;
                        break;
                    case 7:
                        currentNode.LeftSubtreeSize = 37;
                        currentNode.RightSubtreeSize = 25;
                        nodeToZigZag = currentNode;
                        break;
                    case 10:
                        currentNode.LeftSubtreeSize = 7;
                        currentNode.RightSubtreeSize = 5;
                        break;
                    case 6:
                        currentNode.LeftSubtreeSize = 19;
                        currentNode.RightSubtreeSize = 17;
                        break;
                    case 8:
                        currentNode.LeftSubtreeSize = 13;
                        currentNode.RightSubtreeSize = 11;
                        break;
                }
            });
            testWeightBalancedTree.BreadthFirstSearch(subtreeSizeUpdateAction);

            testWeightBalancedTree.ZigZagNodeLeft(nodeToZigZag);

            testWeightBalancedTree.BreadthFirstSearch(populateStoreAction);
            Assert.AreEqual(8, allNodes.Count);
            Assert.IsNull(allNodes[3].ParentNode);
            Assert.IsNull(allNodes[3].LeftChildNode);
            Assert.AreSame(allNodes[7], allNodes[3].RightChildNode);
            Assert.AreSame(allNodes[3], allNodes[7].ParentNode);
            Assert.AreSame(allNodes[5], allNodes[7].LeftChildNode);
            Assert.AreSame(allNodes[9], allNodes[7].RightChildNode);
            Assert.AreSame(allNodes[7], allNodes[5].ParentNode);
            Assert.AreSame(allNodes[4], allNodes[5].LeftChildNode);
            Assert.AreSame(allNodes[6], allNodes[5].RightChildNode);
            Assert.AreSame(allNodes[7], allNodes[9].ParentNode);
            Assert.AreSame(allNodes[8], allNodes[9].LeftChildNode);
            Assert.AreSame(allNodes[10], allNodes[9].RightChildNode);
            Assert.AreSame(allNodes[5], allNodes[4].ParentNode);
            Assert.IsNull(allNodes[4].LeftChildNode);
            Assert.IsNull(allNodes[4].RightChildNode);
            Assert.AreSame(allNodes[5], allNodes[6].ParentNode);
            Assert.IsNull(allNodes[6].LeftChildNode);
            Assert.IsNull(allNodes[6].RightChildNode);
            Assert.AreSame(allNodes[9], allNodes[8].ParentNode);
            Assert.IsNull(allNodes[8].LeftChildNode);
            Assert.IsNull(allNodes[8].RightChildNode);
            Assert.AreSame(allNodes[9], allNodes[10].ParentNode);
            Assert.IsNull(allNodes[10].LeftChildNode);
            Assert.IsNull(allNodes[10].RightChildNode);
            Assert.AreEqual(31, subtreeValueStore[3].Item1);
            Assert.AreEqual(131, subtreeValueStore[3].Item2);
            Assert.AreEqual(91, subtreeValueStore[7].Item1);
            Assert.AreEqual(39, subtreeValueStore[7].Item2);
            Assert.AreEqual(53, subtreeValueStore[5].Item1);
            Assert.AreEqual(37, subtreeValueStore[5].Item2);
            Assert.AreEqual(25, subtreeValueStore[9].Item1);
            Assert.AreEqual(13, subtreeValueStore[9].Item2);
            Assert.AreEqual(29, subtreeValueStore[4].Item1);
            Assert.AreEqual(23, subtreeValueStore[4].Item2);
            Assert.AreEqual(19, subtreeValueStore[6].Item1);
            Assert.AreEqual(17, subtreeValueStore[6].Item2);
            Assert.AreEqual(13, subtreeValueStore[8].Item1);
            Assert.AreEqual(11, subtreeValueStore[8].Item2);
            Assert.AreEqual(7, subtreeValueStore[10].Item1);
            Assert.AreEqual(5, subtreeValueStore[10].Item2);
        }

        /// <summary>
        /// Success test for the  ZigZagNodeLeft() method where the nodes within the zig-zag shape all have children (and parents).
        /// </summary>
        [Test]
        public void ZigZagNodeLeft_ZigZagNodesHaveChildren()
        {
            // Test by performing a right zig-zag operation on node 7 in the following tree...
            //    3
            //     \
            //      5
            //     / \
            //    4   9
            //       / \
            //      7   10
            //     / \
            //    6   8
            //
            // ...and expect the result to look like...
            //       3
            //        \
            //         7
            //       /   \
            //      5     9
            //     / \   / \
            //    4   6 8   10
            var allNodes = new Dictionary<Int32, WeightBalancedTreeNode<Int32>>();
            var subtreeValueStore = new Dictionary<Int32, Tuple<Int32, Int32>>();
            Action<WeightBalancedTreeNode<Int32>> populateStoreAction = (node) =>
            {
                allNodes.Add(node.Item, node);
                subtreeValueStore.Add(node.Item, new Tuple<Int32, Int32>(node.LeftSubtreeSize, node.RightSubtreeSize));
            };
            WeightBalancedTreeWithProtectedMethods<Int32> testWeightBalancedTree = new WeightBalancedTreeWithProtectedMethods<Int32>(new Int32[] { 3, 5, 4, 9, 7, 10, 6, 8 }, false);
            WeightBalancedTreeNode<Int32> nodeToZigZag = null;
            Action<WeightBalancedTreeNode<Int32>> getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 7)
                    nodeToZigZag = currentNode;
            };
            testWeightBalancedTree.BreadthFirstSearch(getNodeAction);

            testWeightBalancedTree.ZigZagNodeLeft(nodeToZigZag);

            testWeightBalancedTree.BreadthFirstSearch(populateStoreAction);
            Assert.AreEqual(8, allNodes.Count);
            Assert.AreEqual(8, allNodes.Count);
            Assert.IsNull(allNodes[3].ParentNode);
            Assert.IsNull(allNodes[3].LeftChildNode);
            Assert.AreSame(allNodes[7], allNodes[3].RightChildNode);
            Assert.AreSame(allNodes[3], allNodes[7].ParentNode);
            Assert.AreSame(allNodes[5], allNodes[7].LeftChildNode);
            Assert.AreSame(allNodes[9], allNodes[7].RightChildNode);
            Assert.AreSame(allNodes[7], allNodes[5].ParentNode);
            Assert.AreSame(allNodes[4], allNodes[5].LeftChildNode);
            Assert.AreSame(allNodes[6], allNodes[5].RightChildNode);
            Assert.AreSame(allNodes[7], allNodes[9].ParentNode);
            Assert.AreSame(allNodes[8], allNodes[9].LeftChildNode);
            Assert.AreSame(allNodes[10], allNodes[9].RightChildNode);
            Assert.AreSame(allNodes[5], allNodes[4].ParentNode);
            Assert.IsNull(allNodes[4].LeftChildNode);
            Assert.IsNull(allNodes[4].RightChildNode);
            Assert.AreSame(allNodes[5], allNodes[6].ParentNode);
            Assert.IsNull(allNodes[6].LeftChildNode);
            Assert.IsNull(allNodes[6].RightChildNode);
            Assert.AreSame(allNodes[9], allNodes[8].ParentNode);
            Assert.IsNull(allNodes[8].LeftChildNode);
            Assert.IsNull(allNodes[8].RightChildNode);
            Assert.AreSame(allNodes[9], allNodes[10].ParentNode);
            Assert.IsNull(allNodes[10].LeftChildNode);
            Assert.IsNull(allNodes[10].RightChildNode);
            Assert.AreEqual(0, subtreeValueStore[3].Item1);
            Assert.AreEqual(7, subtreeValueStore[3].Item2);
            Assert.AreEqual(3, subtreeValueStore[7].Item1);
            Assert.AreEqual(3, subtreeValueStore[7].Item2);
            Assert.AreEqual(1, subtreeValueStore[5].Item1);
            Assert.AreEqual(1, subtreeValueStore[5].Item2);
            Assert.AreEqual(1, subtreeValueStore[9].Item1);
            Assert.AreEqual(1, subtreeValueStore[9].Item2);
            Assert.AreEqual(0, subtreeValueStore[4].Item1);
            Assert.AreEqual(0, subtreeValueStore[4].Item2);
            Assert.AreEqual(0, subtreeValueStore[6].Item1);
            Assert.AreEqual(0, subtreeValueStore[6].Item2);
            Assert.AreEqual(0, subtreeValueStore[8].Item1);
            Assert.AreEqual(0, subtreeValueStore[8].Item2);
            Assert.AreEqual(0, subtreeValueStore[10].Item1);
            Assert.AreEqual(0, subtreeValueStore[10].Item2);
        }

        /// <summary>
        /// Success test for the ZigZagNodeLeft() method where the nodes within the zig-zag shape don't have children (nor parents).
        /// </summary>
        [Test]
        public void ZigZagNodeLeft_ZigZagNodesHaveNoChildren()
        {
            // Test by performing a right zig-zag operation on node 6 in the following tree...
            //     4
            //      \
            //       8 
            //      /
            //     6
            //
            // ...and expect the result to look like...
            //     6
            //    / \   
            //   4   8
            var allNodes = new Dictionary<Int32, WeightBalancedTreeNode<Int32>>();
            var subtreeValueStore = new Dictionary<Int32, Tuple<Int32, Int32>>();
            Action<WeightBalancedTreeNode<Int32>> populateStoreAction = (node) =>
            {
                allNodes.Add(node.Item, node);
                subtreeValueStore.Add(node.Item, new Tuple<Int32, Int32>(node.LeftSubtreeSize, node.RightSubtreeSize));
            };
            WeightBalancedTreeWithProtectedMethods<Int32> testWeightBalancedTree = new WeightBalancedTreeWithProtectedMethods<Int32>(new Int32[] { 4, 8, 6 }, false);
            WeightBalancedTreeNode<Int32> nodeToZigZag = null;
            Action<WeightBalancedTreeNode<Int32>> getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 6)
                    nodeToZigZag = currentNode;
            };
            testWeightBalancedTree.BreadthFirstSearch(getNodeAction);

            testWeightBalancedTree.ZigZagNodeLeft(nodeToZigZag);

            testWeightBalancedTree.BreadthFirstSearch(populateStoreAction);
            Assert.AreEqual(3, allNodes.Count);
            Assert.IsNull(allNodes[6].ParentNode);
            Assert.AreSame(allNodes[4], allNodes[6].LeftChildNode);
            Assert.AreSame(allNodes[8], allNodes[6].RightChildNode);
            Assert.AreSame(allNodes[6], allNodes[4].ParentNode);
            Assert.IsNull(allNodes[4].LeftChildNode);
            Assert.IsNull(allNodes[4].RightChildNode);
            Assert.AreSame(allNodes[6], allNodes[8].ParentNode);
            Assert.IsNull(allNodes[8].LeftChildNode);
            Assert.IsNull(allNodes[8].RightChildNode);
            Assert.AreEqual(1, subtreeValueStore[6].Item1);
            Assert.AreEqual(1, subtreeValueStore[6].Item2);
            Assert.AreEqual(0, subtreeValueStore[4].Item1);
            Assert.AreEqual(0, subtreeValueStore[4].Item2);
            Assert.AreEqual(0, subtreeValueStore[8].Item1);
            Assert.AreEqual(0, subtreeValueStore[8].Item2);
        }

        /// <summary>
        /// Test that an exception is thrown if the ZigZagNodeLeft() method is called on a node which is a right child.
        /// </summary>
        [Test]
        public void ZigZagNodeLeft_ZigZagNodeIsARightChild()
        {
            WeightBalancedTreeWithProtectedMethods<Int32> testWeightBalancedTree = new WeightBalancedTreeWithProtectedMethods<Int32>(new Int32[] { 1, 2, 3 }, false);
            WeightBalancedTreeNode<Int32> nodeToZigZag = null;
            Action<WeightBalancedTreeNode<Int32>> getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 3)
                    nodeToZigZag = currentNode;
            };
            testWeightBalancedTree.BreadthFirstSearch(getNodeAction);

            var e = Assert.Throws<InvalidOperationException>(delegate
            {
                testWeightBalancedTree.ZigZagNodeLeft(nodeToZigZag);
            });

            Assert.That(e.Message, Does.StartWith("The node containing item '3' cannot have a left zig-zag operation applied as it is a right child of its parent."));
        }

        /// <summary>
        /// Test that an exception is thrown if the ZigZagNodeLeft() method is called on a node whose parent is a left child.
        /// </summary>
        [Test]
        public void ZigZagNodeLeft_ZigZagNodesParentIsALeftChild()
        {
            WeightBalancedTreeWithProtectedMethods<Int32> testWeightBalancedTree = new WeightBalancedTreeWithProtectedMethods<Int32>(new Int32[] { 3, 2, 1 }, false);
            WeightBalancedTreeNode<Int32> nodeToZigZag = null;
            Action<WeightBalancedTreeNode<Int32>> getNodeAction = (currentNode) =>
            {
                if (currentNode.Item == 1)
                    nodeToZigZag = currentNode;
            };
            testWeightBalancedTree.BreadthFirstSearch(getNodeAction);

            var e = Assert.Throws<InvalidOperationException>(delegate
            {
                testWeightBalancedTree.ZigZagNodeLeft(nodeToZigZag);
            });

            Assert.That(e.Message, Does.StartWith("The node containing item '1' cannot have a left zig-zag operation applied as its parent is a left child of its grandparent."));
        }

        #region Private Methods

        /// <summary>
        /// Returns a dictionary containing all the nodes of the specified tree as its values, and keyed by node's items.
        /// </summary>
        /// <param name="inputTree">The tree to add the nodes of.</param>
        /// <returns>A dictionary containing all the nodes of the tree.</returns>
        private Dictionary<Int32, WeightBalancedTreeNode<Int32>> PutAllNodesInDictionary(WeightBalancedTree<Int32> inputTree)
        {
            Dictionary<Int32, WeightBalancedTreeNode<Int32>> returnDictionary = new Dictionary<Int32, WeightBalancedTreeNode<Int32>>();
            Action<WeightBalancedTreeNode<Int32>> addNodesToDictionaryAction = (node) =>
            {
                returnDictionary.Add(node.Item, node);
            };
            inputTree.PreOrderDepthFirstSearch(addNodesToDictionaryAction);

            return returnDictionary;
        }

        #endregion

        #region Nested Classes

        /// <summary>
        /// Version of the WeightBalancedTree class where private and protected methods are exposed as public so that they can be unit tested.
        /// </summary>
        /// <typeparam name="T">Specifies the type of items held by nodes of the tree.</typeparam>
        private class WeightBalancedTreeWithProtectedMethods<T> : WeightBalancedTree<T> where T : IComparable<T>
        {
            /// <summary>
            /// The root node of the tree.
            /// </summary>
            public WeightBalancedTreeNode<T> RootNode
            {
                set { rootNode = value; }
            }

            /// <summary>
            /// Initialises a new instance of the MoreComplexDataStructures.UnitTests.WeightBalancedTreeTests+WeightBalancedTreeWithProtectedMethods class that contains elements copied from the specified collection.
            /// </summary>
            /// <param name="collection">The collection whose elements are copied to the new WeightBalancedTree.</param>
            public WeightBalancedTreeWithProtectedMethods(IEnumerable<T> collection)
                : base(collection)
            {
            }

            /// <summary>
            /// Initialises a new instance of the MoreComplexDataStructures.UnitTests.WeightBalancedTreeTests+WeightBalancedTreeWithProtectedMethods class that contains elements copied from the specified collection.
            /// </summary>
            /// <param name="collection">The collection whose elements are copied to the new WeightBalancedTree.</param>
            /// <param name="maintainBalance">Determines whether balance of the tree should be maintained when adding or removing items.</param>
            public WeightBalancedTreeWithProtectedMethods(IEnumerable<T> collection, Boolean maintainBalance)
                : base(collection, maintainBalance)
            {
            }

            /// <summary>
            /// Assesses whether performing a left rotation on the specified node will improve the tree's balance.
            /// </summary>
            /// <param name="inputNode">The node to assess.</param>
            /// <returns>True if the rotation will improve the balance.  False otherwise.</returns>
            public new Boolean WillLeftRotationImproveBalance(WeightBalancedTreeNode<T> inputNode)
            {
                return base.WillLeftRotationImproveBalance(inputNode);
            }

            /// <summary>
            /// Assesses whether performing a right rotation on the specified node will improve the tree's balance.
            /// </summary>
            /// <param name="inputNode">The node to assess.</param>
            /// <returns>True if the rotation will improve the balance.  False otherwise.</returns>
            public new Boolean WillRightRotationImproveBalance(WeightBalancedTreeNode<T> inputNode)
            {
                return base.WillRightRotationImproveBalance(inputNode);
            }

            /// <summary>
            /// Performs a left rotation on the specified node.
            /// </summary>
            /// <param name="inputNode">The node to perform the rotation on.</param>
            public new void RotateNodeLeft(WeightBalancedTreeNode<T> inputNode)
            {
                base.RotateNodeLeft(inputNode);
            }

            /// <summary>
            /// Performs a right rotation on the specified node.
            /// </summary>
            /// <param name="inputNode">The node to perform the rotation on.</param>
            public new void RotateNodeRight(WeightBalancedTreeNode<T> inputNode)
            {
                base.RotateNodeRight(inputNode);
            }

            /// <summary>
            /// Assesses whether performing a left zig-zag operation on the specified node will improve the tree's balance.
            /// </summary>
            /// <param name="inputNode">The node to assess.</param>
            /// <returns>True if the zig-zag operation will improve the balance.  False otherwise.</returns>
            public new Boolean WillLeftZigZagOperationImproveBalance(WeightBalancedTreeNode<T> inputNode)
            {
                return base.WillLeftZigZagOperationImproveBalance(inputNode);
            }

            /// <summary>
            /// Assesses whether performing a right zig-zag operation on the specified node will improve the tree's balance.
            /// </summary>
            /// <param name="inputNode">The node to assess.</param>
            /// <returns>True if the zig-zag operation will improve the balance.  False otherwise.</returns>
            public new Boolean WillRightZigZagOperationImproveBalance(WeightBalancedTreeNode<T> inputNode)
            {
                return base.WillRightZigZagOperationImproveBalance(inputNode);
            }

            /// <summary>
            /// Performs a left zig-zag operation on the specified node.
            /// </summary>
            /// <param name="inputNode">The node to perform the zig-zag operation on.</param>
            public new void ZigZagNodeLeft(WeightBalancedTreeNode<T> inputNode)
            {
                base.ZigZagNodeLeft(inputNode);
            }

            /// <summary>
            /// Performs a right zig-zag operation on the specified node.
            /// </summary>
            /// <param name="inputNode">The node to perform the zig-zag operation on.</param>
            public new void ZigZagNodeRight(WeightBalancedTreeNode<T> inputNode)
            {
                base.ZigZagNodeRight(inputNode);
            }

            /// <summary>
            /// Traverses upwards from the specified node, performing node rotations to balance the tree.
            /// </summary>
            /// <param name="inputNode">The node at which to start balancing.</param>
            public new void BalanceTreeUpFromNode(WeightBalancedTreeNode<T> inputNode)
            {
                base.BalanceTreeUpFromNode(inputNode);
            }
        }

        /// <summary>
        /// Simple container class used to test the Get() method.
        /// </summary>
        private class CharacterAndCount : IComparable<CharacterAndCount>
        {
            protected Char character;
            protected Int32 count;

            public Char Character
            {
                get { return character; }
            }

            public Int32 Count
            {
                get { return count; }
                set { count = value; }
            }

            public CharacterAndCount(Char character, Int32 count)
            {
                this.character = character;
                this.count = count;
            }

            public Int32 CompareTo(CharacterAndCount other)
            {
                return this.character.CompareTo(other.character);
            }
        }

        #endregion
    }
}
