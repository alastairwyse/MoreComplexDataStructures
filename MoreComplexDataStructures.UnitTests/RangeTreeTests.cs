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
    /// Unit tests for the UniqueRandomGenerator+RangeTree class.
    /// </summary>
    /// <remarks>Deriving from UniqueRandomGenerator so that nested class RangeAndSubtreeCounts can be accessed.</remarks>
    public class RangeTreeTests : UniqueRandomGenerator
    {
        private RangeTreeWithProtectedMethods testRangeTree;

        /// <summary>
        /// Placeholder constructor.
        /// </summary>
        public RangeTreeTests()
            : base(0, 0)
        {
        }

        [SetUp]
        protected void SetUp()
        {
            testRangeTree = new RangeTreeWithProtectedMethods();
        }

        /// <summary>
        /// Success tests for the RotateNodeLeft() method of nested class RangeTree, where the right child of the node to rotate has no left child.
        /// </summary>
        [Test]
        public void RotateNodeLeft_LeftChildOfRightChildOfNodeIsNull()
        {
            // Test by left-rotating the node containing 20-26 in the following tree (subscript numbers show LeftSubtreeRangeCount and RightSubtreeRangeCount properties)...
            //                     70-88(19)
            //                   46         23
            //                   /           \
            //               20-26(7)      90-112(23)
            //             10        29   0          0
            //             /          \
            //         10-14(5)     30-40(11)
            //        2        3   0         18
            //       /          \             \
            //     0-1(2)    15-17(3)      50-67(18)
            //    0      0  0        0    0         0
            //
            // ...and expect the result to look like...
            //                           70-88(19)
            //                         46         23
            //                         /           \
            //                    30-40(11)      90-112(23)
            //                  17         18   0          0
            //                  /           \
            //              20-26(7)       50-66(18)
            //            10        0     0         0
            //            /
            //         10-14(5)    
            //        2        3  
            //       /          \ 
            //     0-1(2)    15-17(3)      
            //    0      0  0        0  
            var allNodes = new Dictionary<Int64, WeightBalancedTreeNode<RangeAndSubtreeCounts>>();
            var subtreeValueStore = new Dictionary<Int64, Tuple<Int32, Int32>>();
            Action<WeightBalancedTreeNode<RangeAndSubtreeCounts>> populateStoreAction = (node) =>
            {
                allNodes.Add(node.Item.Range.StartValue, node);
                subtreeValueStore.Add(node.Item.Range.StartValue, new Tuple<Int32, Int32>(node.LeftSubtreeSize, node.RightSubtreeSize));
            };
            testRangeTree.MaintainBalance = false;
            testRangeTree.Add(new RangeAndSubtreeCounts(new LongIntegerRange(70, 19), 46, 23));
            testRangeTree.Add(new RangeAndSubtreeCounts(new LongIntegerRange(20, 7), 10, 29));
            testRangeTree.Add(new RangeAndSubtreeCounts(new LongIntegerRange(90, 23), 0, 0));
            testRangeTree.Add(new RangeAndSubtreeCounts(new LongIntegerRange(10, 5), 2, 3));
            testRangeTree.Add(new RangeAndSubtreeCounts(new LongIntegerRange(30, 11), 0, 18));
            testRangeTree.Add(new RangeAndSubtreeCounts(new LongIntegerRange(0, 2), 0, 0));
            testRangeTree.Add(new RangeAndSubtreeCounts(new LongIntegerRange(15, 3), 0, 0));
            testRangeTree.Add(new RangeAndSubtreeCounts(new LongIntegerRange(50, 18), 0, 0));
            WeightBalancedTreeNode<RangeAndSubtreeCounts> nodeToRotate = null;
            Action<WeightBalancedTreeNode<RangeAndSubtreeCounts>> getNodeAction = (currentNode) =>
            {
                if (currentNode.Item.Range.StartValue == 20)
                    nodeToRotate = currentNode;
            };
            testRangeTree.InOrderDepthFirstSearch(getNodeAction);

            testRangeTree.RotateNodeLeft(nodeToRotate);

            testRangeTree.BreadthFirstSearch(populateStoreAction);
            Assert.AreEqual(allNodes[70], allNodes[30].ParentNode);
            Assert.AreEqual(allNodes[20], allNodes[30].LeftChildNode);
            Assert.AreEqual(allNodes[50], allNodes[30].RightChildNode);
            Assert.AreEqual(allNodes[30], allNodes[20].ParentNode);
            Assert.AreEqual(allNodes[10], allNodes[20].LeftChildNode);
            Assert.IsNull(allNodes[20].RightChildNode);
            Assert.AreEqual(46, allNodes[70].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(10, allNodes[20].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[20].Item.RightSubtreeRangeCount);
            Assert.AreEqual(17, allNodes[30].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(18, allNodes[30].Item.RightSubtreeRangeCount);
        }
        
        /// <summary>
        /// Success tests for the RotateNodeLeft() method of nested class RangeTree, where the right child of the node to rotate has a left child.
        /// </summary>
        [Test]
        public void RotateNodeLeft_LeftChildOfRightChildOfNodeNotNull()
        {
            // Test by left-rotating the node containing 20-26 in the following tree (subscript numbers show LeftSubtreeRangeCount and RightSubtreeRangeCount properties)...
            //                           70-88(19)
            //                         48         23
            //                       /               \
            //                    20-26(7)         90-112(23)
            //                  10        31      0          0
            //                /              \
            //         10-14(5)               30-40(11)
            //        2        3             1         19
            //       /          \           /            \
            //     0-1(2)    15-17(3)    28-28(1)      50-68(19)
            //    0      0  0        0  0        0    0         0
            //
            // ...and expect the result to look like...
            //                               70-88(19)
            //                             48         23
            //                           /               \
            //                      30-40(11)          90-112(23)
            //                    18         19       0          0
            //                   /             \
            //              20-26(7)         50-68(19)       
            //            10        1       0         0       
            //            /          \      
            //         10-14(5)     28-28(1)                
            //        2        3   0        0               
            //       /          \ 
            //     0-1(2)    15-17(3)
            //    0      0  0        0
            var allNodes = new Dictionary<Int64, WeightBalancedTreeNode<RangeAndSubtreeCounts>>();
            var subtreeValueStore = new Dictionary<Int64, Tuple<Int32, Int32>>();
            Action<WeightBalancedTreeNode<RangeAndSubtreeCounts>> populateStoreAction = (node) =>
            {
                allNodes.Add(node.Item.Range.StartValue, node);
                subtreeValueStore.Add(node.Item.Range.StartValue, new Tuple<Int32, Int32>(node.LeftSubtreeSize, node.RightSubtreeSize));
            };
            testRangeTree.MaintainBalance = false;
            testRangeTree.Add(new RangeAndSubtreeCounts(new LongIntegerRange(70, 19), 48, 23));
            testRangeTree.Add(new RangeAndSubtreeCounts(new LongIntegerRange(20, 7), 10, 31));
            testRangeTree.Add(new RangeAndSubtreeCounts(new LongIntegerRange(90, 23), 0, 0));
            testRangeTree.Add(new RangeAndSubtreeCounts(new LongIntegerRange(10, 5), 2, 3));
            testRangeTree.Add(new RangeAndSubtreeCounts(new LongIntegerRange(30, 11), 1, 19));
            testRangeTree.Add(new RangeAndSubtreeCounts(new LongIntegerRange(0, 2), 0, 0));
            testRangeTree.Add(new RangeAndSubtreeCounts(new LongIntegerRange(15, 3), 0, 0));
            testRangeTree.Add(new RangeAndSubtreeCounts(new LongIntegerRange(28, 1), 0, 0));
            testRangeTree.Add(new RangeAndSubtreeCounts(new LongIntegerRange(50, 19), 0, 0));
            WeightBalancedTreeNode<RangeAndSubtreeCounts> nodeToRotate = null;
            Action<WeightBalancedTreeNode<RangeAndSubtreeCounts>> getNodeAction = (currentNode) =>
            {
                if (currentNode.Item.Range.StartValue == 20)
                    nodeToRotate = currentNode;
            };
            testRangeTree.InOrderDepthFirstSearch(getNodeAction);

            testRangeTree.RotateNodeLeft(nodeToRotate);

            testRangeTree.BreadthFirstSearch(populateStoreAction);
            Assert.AreEqual(allNodes[70], allNodes[30].ParentNode);
            Assert.AreEqual(allNodes[20], allNodes[30].LeftChildNode);
            Assert.AreEqual(allNodes[50], allNodes[30].RightChildNode);
            Assert.AreEqual(allNodes[30], allNodes[20].ParentNode);
            Assert.AreEqual(allNodes[10], allNodes[20].LeftChildNode);
            Assert.AreEqual(allNodes[28], allNodes[20].RightChildNode);
            Assert.AreEqual(48, allNodes[70].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(10, allNodes[20].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(1, allNodes[20].Item.RightSubtreeRangeCount);
            Assert.AreEqual(18, allNodes[30].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(19, allNodes[30].Item.RightSubtreeRangeCount);
        }

        /// <summary>
        /// Success tests for the RotateNodeRight() method of nested class RangeTree, where the left child of the node to rotate has no right child.
        /// </summary>
        [Test]
        public void RotateNodeRight_LeftChildOfRightChildOfNodeIsNull()
        {
            // Test by right-rotating the node containing 100-106 in the following tree (subscript numbers show LeftSubtreeRangeCount and RightSubtreeRangeCount properties)...
            //            30-48(19)
            //          23         46
            //          /           \
            //      0-22(23)      100-106(7)
            //     0        0   29          10
            //                  /            \
            //             70-80(11)      120-124(5)
            //           18         0    3          2
            //           /              /            \
            //       50-67(18)      110-112(3)    130-131(2)
            //      0         0    0          0  0          0
            //
            // ...and expect the result to look like...
            //            30-48(19)
            //          23         46
            //          /           \
            //      0-22(23)      70-80(11)
            //     0        0   18         17
            //                 /            \
            //             50-67(18)      100-106(7)
            //            0         0    0          10
            //                                       \
            //                                    120-124(5)
            //                                   3          2
            //                                  /            \
            //                            110-112(3)      130-131(2)
            //                           0          0    0          0
            var allNodes = new Dictionary<Int64, WeightBalancedTreeNode<RangeAndSubtreeCounts>>();
            var subtreeValueStore = new Dictionary<Int64, Tuple<Int32, Int32>>();
            Action<WeightBalancedTreeNode<RangeAndSubtreeCounts>> populateStoreAction = (node) =>
            {
                allNodes.Add(node.Item.Range.StartValue, node);
                subtreeValueStore.Add(node.Item.Range.StartValue, new Tuple<Int32, Int32>(node.LeftSubtreeSize, node.RightSubtreeSize));
            };
            testRangeTree.MaintainBalance = false;
            testRangeTree.Add(new RangeAndSubtreeCounts(new LongIntegerRange(30, 19), 23, 46));
            testRangeTree.Add(new RangeAndSubtreeCounts(new LongIntegerRange(0, 23), 0, 0));
            testRangeTree.Add(new RangeAndSubtreeCounts(new LongIntegerRange(100, 7), 29, 10));
            testRangeTree.Add(new RangeAndSubtreeCounts(new LongIntegerRange(70, 11), 18, 0));
            testRangeTree.Add(new RangeAndSubtreeCounts(new LongIntegerRange(120, 5), 3, 2));
            testRangeTree.Add(new RangeAndSubtreeCounts(new LongIntegerRange(50, 18), 0, 0));
            testRangeTree.Add(new RangeAndSubtreeCounts(new LongIntegerRange(110, 3), 0, 0));
            testRangeTree.Add(new RangeAndSubtreeCounts(new LongIntegerRange(130, 2), 0, 0));
            WeightBalancedTreeNode<RangeAndSubtreeCounts> nodeToRotate = null;
            Action<WeightBalancedTreeNode<RangeAndSubtreeCounts>> getNodeAction = (currentNode) =>
            {
                if (currentNode.Item.Range.StartValue == 100)
                    nodeToRotate = currentNode;
            };
            testRangeTree.InOrderDepthFirstSearch(getNodeAction);

            testRangeTree.RotateNodeRight(nodeToRotate);

            testRangeTree.BreadthFirstSearch(populateStoreAction);
            Assert.AreEqual(allNodes[30], allNodes[70].ParentNode);
            Assert.AreEqual(allNodes[50], allNodes[70].LeftChildNode);
            Assert.AreEqual(allNodes[100], allNodes[70].RightChildNode);
            Assert.AreEqual(allNodes[70], allNodes[100].ParentNode);
            Assert.IsNull(allNodes[100].LeftChildNode);
            Assert.AreEqual(allNodes[120], allNodes[100].RightChildNode);
            Assert.AreEqual(46, allNodes[30].Item.RightSubtreeRangeCount);
            Assert.AreEqual(18, allNodes[70].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(17, allNodes[70].Item.RightSubtreeRangeCount);
            Assert.AreEqual(0, allNodes[100].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(10, allNodes[100].Item.RightSubtreeRangeCount);
        }

        /// <summary>
        /// Success tests for the RotateNodeRight() method of nested class RangeTree, where the left child of the node to rotate has a right child.
        /// </summary>
        [Test]
        public void RotateNodeRight_LeftChildOfRightChildOfNodeNotNull()
        {
            // Test by right-rotating the node containing 100-106 in the following tree (subscript numbers show LeftSubtreeRangeCount and RightSubtreeRangeCount properties)...
            //              30-48(19)
            //            23         48
            //          /               \
            //      0-22(23)          100-106(7)
            //     0        0       31          10
            //                    /                \
            //             70-80(11)              120-124(5)
            //           19         1            3          2
            //          /            \          /            \
            //     50-68(19)      90-90(1)   110-112(3)    130-131(2)
            //    0         0    0        0 0          0  0          0
            //
            // ...and expect the result to look like...
            //            30-48(19)
            //          23         48
            //          /           \
            //      0-22(23)      70-80(11)
            //     0        0   19         18
            //                 /            \
            //             50-68(19)      100-106(7)
            //            0         0    1          10
            //                          /             \
            //                      90-90(1)       120-124(5)
            //                     0        0     3          2
            //                                   /            \
            //                              110-112(3)      130-131(2)
            //                             0          0    0          0
            var allNodes = new Dictionary<Int64, WeightBalancedTreeNode<RangeAndSubtreeCounts>>();
            var subtreeValueStore = new Dictionary<Int64, Tuple<Int32, Int32>>();
            Action<WeightBalancedTreeNode<RangeAndSubtreeCounts>> populateStoreAction = (node) =>
            {
                allNodes.Add(node.Item.Range.StartValue, node);
                subtreeValueStore.Add(node.Item.Range.StartValue, new Tuple<Int32, Int32>(node.LeftSubtreeSize, node.RightSubtreeSize));
            };
            testRangeTree.MaintainBalance = false;
            testRangeTree.Add(new RangeAndSubtreeCounts(new LongIntegerRange(30, 19), 23, 48));
            testRangeTree.Add(new RangeAndSubtreeCounts(new LongIntegerRange(0, 23), 0, 0));
            testRangeTree.Add(new RangeAndSubtreeCounts(new LongIntegerRange(100, 7), 31, 10));
            testRangeTree.Add(new RangeAndSubtreeCounts(new LongIntegerRange(70, 11), 19, 1));
            testRangeTree.Add(new RangeAndSubtreeCounts(new LongIntegerRange(120, 5), 3, 2));
            testRangeTree.Add(new RangeAndSubtreeCounts(new LongIntegerRange(50, 19), 0, 0));
            testRangeTree.Add(new RangeAndSubtreeCounts(new LongIntegerRange(90, 1), 0, 0));
            testRangeTree.Add(new RangeAndSubtreeCounts(new LongIntegerRange(110, 3), 0, 0));
            testRangeTree.Add(new RangeAndSubtreeCounts(new LongIntegerRange(130, 2), 0, 0));
            WeightBalancedTreeNode<RangeAndSubtreeCounts> nodeToRotate = null;
            Action<WeightBalancedTreeNode<RangeAndSubtreeCounts>> getNodeAction = (currentNode) =>
            {
                if (currentNode.Item.Range.StartValue == 100)
                    nodeToRotate = currentNode;
            };
            testRangeTree.InOrderDepthFirstSearch(getNodeAction);

            testRangeTree.RotateNodeRight(nodeToRotate);

            testRangeTree.BreadthFirstSearch(populateStoreAction);
            Assert.AreEqual(allNodes[30], allNodes[70].ParentNode);
            Assert.AreEqual(allNodes[50], allNodes[70].LeftChildNode);
            Assert.AreEqual(allNodes[100], allNodes[70].RightChildNode);
            Assert.AreEqual(allNodes[70], allNodes[100].ParentNode);
            Assert.AreEqual(allNodes[90], allNodes[100].LeftChildNode);
            Assert.AreEqual(allNodes[120], allNodes[100].RightChildNode);
            Assert.AreEqual(48, allNodes[30].Item.RightSubtreeRangeCount);
            Assert.AreEqual(19, allNodes[70].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(18, allNodes[70].Item.RightSubtreeRangeCount);
            Assert.AreEqual(1, allNodes[100].Item.LeftSubtreeRangeCount);
            Assert.AreEqual(10, allNodes[100].Item.RightSubtreeRangeCount);
        }

        #region Nested Classes

        /// <summary>
        /// Version of the RangeTree class where private and protected methods are exposed as public so that they can be unit tested.
        /// </summary>
        private class RangeTreeWithProtectedMethods : RangeTree
        {
            /// <summary>
            /// Performs a left rotation on the specified node.
            /// </summary>
            /// <param name="inputNode">The node to perform the rotation on.</param>
            public new void RotateNodeLeft(WeightBalancedTreeNode<RangeAndSubtreeCounts> inputNode)
            {
                base.RotateNodeLeft(inputNode);
            }

            /// <summary>
            /// Performs a right rotation on the specified node.
            /// </summary>
            /// <param name="inputNode">The node to perform the rotation on.</param>
            public new void RotateNodeRight(WeightBalancedTreeNode<RangeAndSubtreeCounts> inputNode)
            {
                base.RotateNodeRight(inputNode);
            }
        }

        #endregion
    }
}
