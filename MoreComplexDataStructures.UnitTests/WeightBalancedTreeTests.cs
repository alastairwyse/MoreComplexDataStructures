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
    /// Unit tests for the WeightBalancedTree class.
    /// </summary>
    public class WeightBalancedTreeTests
    {
        private WeightBalancedTree<Int32> testWeightBalancedTree;

        [SetUp]
        protected void SetUp()
        {
            testWeightBalancedTree = new WeightBalancedTree<Int32>();
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

            Dictionary<Int32, WeightBalancedTreeNode<Int32>> allNodes = PutAddNodesInDictionary(testWeightBalancedTree);
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

            Dictionary<Int32, WeightBalancedTreeNode<Int32>> allNodes = PutAddNodesInDictionary(testWeightBalancedTree);
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

            Dictionary<Int32, WeightBalancedTreeNode<Int32>> allNodes = PutAddNodesInDictionary(testWeightBalancedTree);
            Assert.AreEqual(3, allNodes.Count);
            Assert.AreEqual(1, allNodes[2].LeftSubtreeSize);
            Assert.AreEqual(1, allNodes[2].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[1].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[1].RightSubtreeSize);
            Assert.AreEqual(0, allNodes[3].LeftSubtreeSize);
            Assert.AreEqual(0, allNodes[3].RightSubtreeSize);
        }

        /// <summary>
        /// Success tests for the Add() method.
        /// </summary>
        [Test]
        public void Add()
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

            Dictionary<Int32, WeightBalancedTreeNode<Int32>> allNodes = PutAddNodesInDictionary(testWeightBalancedTree);
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
            testWeightBalancedTree = new WeightBalancedTree<Int32>();
            treeItems = new Int32[] { 7, 6, 5, 4, 3, 2, 1 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            allNodes = PutAddNodesInDictionary(testWeightBalancedTree);
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
            testWeightBalancedTree = new WeightBalancedTree<Int32>();
            treeItems = new Int32[] { 1, 2, 3, 4, 5, 6, 7 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            allNodes = PutAddNodesInDictionary(testWeightBalancedTree);
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
        /// Success tests for the Count property.
        /// </summary>
        [Test]
        public void Count()
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
            testWeightBalancedTree = new WeightBalancedTree<Int32>();
            treeItems = new Int32[] { 7, 6, 5, 4, 3, 2, 1 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            Assert.AreEqual(7, testWeightBalancedTree.Count);

            // Test the same input values but fully unbalanced on the right side
            testWeightBalancedTree = new WeightBalancedTree<Int32>();
            treeItems = new Int32[] { 1, 2, 3, 4, 5, 6, 7 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            Assert.AreEqual(7, testWeightBalancedTree.Count);
        }

        /// <summary>
        /// Tests that an exception is thrown if the Depth property is gotten after the Remove() method is called.
        /// </summary>
        [Test]
        public void Depth_CalledAfterRemove()
        {
            Int32 result = testWeightBalancedTree.Depth;
            testWeightBalancedTree.Add(3);
            testWeightBalancedTree.Add(2);
            testWeightBalancedTree.Remove(2);

            InvalidOperationException e = Assert.Throws<InvalidOperationException>(delegate
            {
                result = testWeightBalancedTree.Depth;
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The Depth property cannot be retrieved after nodes are removed from the tree."));
        }

        /// <summary>
        /// Success tests for the Depth property.
        /// </summary>
        [Test]
        public void Depth()
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
            testWeightBalancedTree = new WeightBalancedTree<Int32>();
            treeItems = new Int32[] { 7, 6, 5, 4, 3, 2, 1 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            Assert.AreEqual(6, testWeightBalancedTree.Depth);

            // Test the same input values but fully unbalanced on the right side
            testWeightBalancedTree = new WeightBalancedTree<Int32>();
            treeItems = new Int32[] { 1, 2, 3, 4, 5, 6, 7 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            Assert.AreEqual(6, testWeightBalancedTree.Depth);
        }

        /// <summary>
        /// Success tests for the Clear() method.
        /// </summary>
        [Test]
        public void Clear()
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
            Dictionary<Int32, WeightBalancedTreeNode<Int32>> allNodes = PutAddNodesInDictionary(testWeightBalancedTree);
            Assert.AreEqual(0, allNodes.Count);

            // Test that new items can be added to the tree after clearing
            testWeightBalancedTree.Add(2);
            testWeightBalancedTree.Add(1);
            testWeightBalancedTree.Add(3);

            Assert.AreEqual(3, testWeightBalancedTree.Count);
            Assert.AreEqual(1, testWeightBalancedTree.Depth);
            allNodes = PutAddNodesInDictionary(testWeightBalancedTree);
            Assert.AreEqual(3, allNodes.Count);
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
            testWeightBalancedTree = new WeightBalancedTree<Int32>();
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
            testWeightBalancedTree = new WeightBalancedTree<Int32>();
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
        /// Success tests for the Remove() method.
        /// </summary>
        [Test]
        public void Remove()
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
            testWeightBalancedTree = new WeightBalancedTree<Int32>();
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


            testWeightBalancedTree = new WeightBalancedTree<Int32>();
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
            testWeightBalancedTree = new WeightBalancedTree<Int32>();
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
            testWeightBalancedTree = new WeightBalancedTree<Int32>();
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
            testWeightBalancedTree = new WeightBalancedTree<Int32>();
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
            testWeightBalancedTree = new WeightBalancedTree<Int32>();
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
            testWeightBalancedTree = new WeightBalancedTree<Int32>();
            treeItems = new Int32[] { 40, 35, 45, 20, 37, 10, 25, 5, 12, 22, 26, 14, 21 };
            foreach (Int32 currentItem in treeItems)
            {
                testWeightBalancedTree.Add(currentItem);
            }

            testWeightBalancedTree.Remove(20);

            allNodes.Clear();
            subtreeValueStore.Clear();
            testWeightBalancedTree.BreadthFirstSearch(populateStoreAction);
            Assert.IsFalse(allNodes.ContainsKey(20));
            Assert.AreEqual(12, subtreeValueStore.Count);
            Assert.AreEqual(10, subtreeValueStore[40].Item1);
            Assert.AreEqual(8, subtreeValueStore[35].Item1);
            Assert.AreEqual(3, subtreeValueStore[14].Item1);
            Assert.AreEqual(4, subtreeValueStore[14].Item2);
            Assert.AreEqual(1, subtreeValueStore[10].Item2);
            Assert.AreEqual(0, subtreeValueStore[12].Item2);
            Assert.AreSame(allNodes[14], allNodes[35].LeftChildNode);
            Assert.AreSame(allNodes[35], allNodes[14].ParentNode);
            Assert.AreSame(allNodes[10], allNodes[14].LeftChildNode);
            Assert.AreSame(allNodes[14], allNodes[10].ParentNode);
            Assert.AreSame(allNodes[25], allNodes[14].RightChildNode);
            Assert.AreSame(allNodes[14], allNodes[25].ParentNode);
            Assert.IsNull(allNodes[12].RightChildNode);


            testWeightBalancedTree.Remove(14);

            allNodes.Clear();
            subtreeValueStore.Clear();
            testWeightBalancedTree.BreadthFirstSearch(populateStoreAction);
            Assert.IsFalse(allNodes.ContainsKey(20));
            Assert.AreEqual(11, subtreeValueStore.Count);
            Assert.AreEqual(9, subtreeValueStore[40].Item1);
            Assert.AreEqual(7, subtreeValueStore[35].Item1);
            Assert.AreEqual(3, subtreeValueStore[21].Item1);
            Assert.AreEqual(3, subtreeValueStore[21].Item2);
            Assert.AreEqual(1, subtreeValueStore[25].Item1);
            Assert.AreEqual(0, subtreeValueStore[22].Item1);
            Assert.AreSame(allNodes[21], allNodes[35].LeftChildNode);
            Assert.AreSame(allNodes[35], allNodes[21].ParentNode);
            Assert.AreSame(allNodes[10], allNodes[21].LeftChildNode);
            Assert.AreSame(allNodes[21], allNodes[10].ParentNode);
            Assert.AreSame(allNodes[25], allNodes[21].RightChildNode);
            Assert.AreSame(allNodes[21], allNodes[25].ParentNode);
            Assert.IsNull(allNodes[22].LeftChildNode);
        }

        #region Private Methods

        /// <summary>
        /// Returns a dictionary containing all the nodes of a the specified tree as its values, and keyed by node's items.
        /// </summary>
        /// <param name="inputTree">The tree to add the nodes of.</param>
        /// <returns>A dictionary containing all the nodes of the tree.</returns>
        private Dictionary<Int32, WeightBalancedTreeNode<Int32>> PutAddNodesInDictionary(WeightBalancedTree<Int32> inputTree)
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
    }
}
