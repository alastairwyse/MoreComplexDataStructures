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

namespace MoreComplexDataStructures
{
    /// <summary>
    /// Generates random numbers within a specified range, never generating the same number twice.
    /// </summary>
    public class UniqueRandomGenerator
    {
        /// <summary>The underlying tree of integer ranges used to generate randoms from.</summary>
        protected RangeTree rangeTree;
        /// <summary>The inclusive start of the range of numbers to generate randoms from.</summary>
        protected Int64 rangeStart;
        /// <summary>The inclusive end of the range of numbers to generate randoms from.</summary>
        protected Int64 rangeEnd;
        /// <summary>A count of the random numbers generated.</summary>
        protected Int64 numbersGeneratedCount;
        /// <summary>The random number generator to use for selecting random items.</summary>
        protected IRandomIntegerGenerator randomGenerator;

        /// <summary>
        /// The inclusive start of the range of numbers to generate randoms from.
        /// </summary>
        public Int64 RangeStart
        {
            get { return rangeStart; }
        }

        /// <summary>
        /// The inclusive end of the range of numbers to generate randoms from.
        /// </summary>
        public Int64 RangeEnd
        {
            get { return rangeEnd; }
        }

        /// <summary>
        /// A count of the random numbers generated.
        /// </summary>
        public Int64 NumbersGeneratedCount
        {
            get { return numbersGeneratedCount; }
        }

        /// <summary>
        /// A count of the numbers remaining (yet to be generated) within the range.
        /// </summary>
        public Int64 NumbersRemainingCount
        {
            get { return (rangeEnd - rangeStart + 1) - numbersGeneratedCount; }
        }

        /// <summary>
        /// Initialises a new instance of the MoreComplexDataStructures.UniqueRandomGenerator class.
        /// </summary>
        /// <param name="rangeStart">The inclusive start of the range to generate random numbers from.</param>
        /// <param name="rangeEnd">The inclusive end of the range to generate random numbers from.</param>
        /// <exception cref="System.ArgumentException">Parameter 'rangeEnd' is less than parameter 'rangeStart'.</exception>
        /// <exception cref="System.ArgumentException">The total inclusive range exceeds Int64.MaxValue.</exception>
        public UniqueRandomGenerator(Int64 rangeStart, Int64 rangeEnd)
        {
            if (rangeEnd < rangeStart)
                throw new ArgumentException("Parameter 'rangeEnd' must be greater than or equal to parameter 'rangeStart'.", "rangeEnd");
            if (rangeStart < 1)
            {
                if (rangeStart + Int64.MaxValue <= rangeEnd)
                    throw new ArgumentException("The total inclusive range cannot exceed Int64.MaxValue.", "rangeEnd");
            }
            else if (rangeEnd > -2)
            {
                if (rangeEnd - Int64.MaxValue >= rangeStart)
                    throw new ArgumentException("The total inclusive range cannot exceed Int64.MaxValue.", "rangeEnd");
            }
            rangeTree = new RangeTree();
            var baseRange = new LongIntegerRange(rangeStart, rangeEnd - rangeStart + 1);
            var baseRangeAndCounts = new RangeAndSubtreeCounts(baseRange, 0, 0);
            rangeTree.Add(baseRangeAndCounts);
            this.rangeStart = rangeStart;
            this.rangeEnd = rangeEnd;
            numbersGeneratedCount = 0;
            randomGenerator = new DefaultRandomGenerator();
        }

        /// <summary>
        /// Initialises a new instance of the MoreComplexDataStructures.UniqueRandomGenerator class.
        /// </summary>
        /// <param name="rangeStart">The inclusive start of the range to generate random numbers from.</param>
        /// <param name="rangeEnd">The inclusive end of the range to generate random numbers from.</param>
        /// <param name="randomIntegerGenerator">An implementation of interface IRandomIntegerGenerator to use for selecting random items.</param>
        /// <exception cref="System.ArgumentException">Parameter 'rangeEnd' is less than parameter 'rangeStart'.</exception>
        /// <exception cref="System.ArgumentException">The total inclusive range exceeds Int64.MaxValue.</exception>
        public UniqueRandomGenerator(Int64 rangeStart, Int64 rangeEnd, IRandomIntegerGenerator randomIntegerGenerator)
            : this(rangeStart, rangeEnd)
        {
            randomGenerator = randomIntegerGenerator;
        }

        /// <summary>
        /// Returns a unique random number from within the range.
        /// </summary>
        /// <returns>The random number.</returns>
        /// <exception cref="System.InvalidOperationException">No further unique numbers remain in the range.</exception>
        public Int64 Generate()
        {
            if (NumbersRemainingCount == 0)
                throw new InvalidOperationException("Cannot generate a random number as no further unique numbers exist in the specified range.");

            WeightBalancedTreeNode<RangeAndSubtreeCounts> currentNode = rangeTree.RootNode;
            Int64 returnNumber = 0;
            while (true)
            {
                // Decide whether to stop at this node, or to move left or right
                Int64 randomRange = currentNode.Item.LeftSubtreeRangeCount + currentNode.Item.Range.Length + currentNode.Item.RightSubtreeRangeCount;
                Int64 randomNumber = randomGenerator.Next(randomRange);
                if (randomNumber < currentNode.Item.LeftSubtreeRangeCount)
                {
                    // Move to the left child node
                    currentNode.Item.LeftSubtreeRangeCount--;
                    currentNode = currentNode.LeftChildNode;
                }
                else if (randomNumber >= (currentNode.Item.LeftSubtreeRangeCount + currentNode.Item.Range.Length))
                {
                    // Move to the right child node
                    currentNode.Item.RightSubtreeRangeCount--;
                    currentNode = currentNode.RightChildNode;
                }
                else
                {
                    // Stop traversing at the current node
                    returnNumber = (randomNumber - currentNode.Item.LeftSubtreeRangeCount) + currentNode.Item.Range.StartValue;
                    break;
                }
            }
            if (returnNumber == currentNode.Item.Range.StartValue)
            {
                if (currentNode.Item.Range.Length == 1)
                {
                    if (currentNode.LeftChildNode != null && currentNode.RightChildNode != null)
                    {
                        // The current node will be swapped with the next less than or next greater than, so need to update the subtree range counts between the current and swapped nodes
                        WeightBalancedTreeNode<RangeAndSubtreeCounts> swapNode = null;
                        if (currentNode.LeftSubtreeSize > currentNode.RightSubtreeSize)
                        {
                            // The current node will be swapped with the next less than
                            swapNode = rangeTree.GetNextLessThan(currentNode);
                            swapNode.Item.LeftSubtreeRangeCount = currentNode.Item.LeftSubtreeRangeCount - swapNode.Item.Range.Length;
                            swapNode.Item.RightSubtreeRangeCount = currentNode.Item.RightSubtreeRangeCount;
                        }
                        else
                        {
                            // The current node will be swapped with the next greater than
                            swapNode = rangeTree.GetNextGreaterThan(currentNode);
                            swapNode.Item.LeftSubtreeRangeCount = currentNode.Item.LeftSubtreeRangeCount;
                            swapNode.Item.RightSubtreeRangeCount = currentNode.Item.RightSubtreeRangeCount - swapNode.Item.Range.Length;
                        }
                        // Update the subtree range counts
                        Func<WeightBalancedTreeNode<RangeAndSubtreeCounts>, Nullable<Boolean>, Boolean> updateSubtreeRangeCountFunc = (node, nodeTraversedToFromLeft) =>
                        {
                            if (node == currentNode)
                                return false;
                            if (nodeTraversedToFromLeft.HasValue)
                            {
                                if (nodeTraversedToFromLeft == true)
                                    node.Item.LeftSubtreeRangeCount -= swapNode.Item.Range.Length;
                                else
                                    node.Item.RightSubtreeRangeCount -= swapNode.Item.Range.Length;
                            }
                            return true;
                        };
                        rangeTree.TraverseUpToNode(swapNode, updateSubtreeRangeCountFunc);
                    }

                    rangeTree.Remove(currentNode.Item);
                }
                else
                {
                    // Shorten the range on the left side
                    currentNode.Item.Range.StartValue++;
                    currentNode.Item.Range.Length--;
                }
            }
            else if (returnNumber == (currentNode.Item.Range.StartValue + currentNode.Item.Range.Length - 1))
            {
                // Shorten the range on the right side
                currentNode.Item.Range.Length--;
            }
            else
            {
                // Split the range
                var leftSideRange = new LongIntegerRange(currentNode.Item.Range.StartValue, returnNumber - currentNode.Item.Range.StartValue);
                var newNodeItem = new RangeAndSubtreeCounts(leftSideRange, currentNode.Item.LeftSubtreeRangeCount, currentNode.Item.RightSubtreeRangeCount);
                var newNode = new WeightBalancedTreeNode<RangeAndSubtreeCounts>(newNodeItem, null);
                currentNode.Item.Range.Length -= (returnNumber - currentNode.Item.Range.StartValue + 1); 
                currentNode.Item.Range.StartValue = returnNumber + 1;
                WeightBalancedTreeNode<RangeAndSubtreeCounts> upperNode = null;
                if (currentNode.LeftSubtreeSize > currentNode.RightSubtreeSize)
                {
                    // 'Push' the current node down right
                    if (currentNode == rangeTree.RootNode)
                    {
                        rangeTree.RootNode = newNode;
                    }
                    else
                    {
                        if (currentNode.ParentNode.LeftChildNode == currentNode)
                        {
                            currentNode.ParentNode.LeftChildNode = newNode;
                        }
                        else
                        {
                            currentNode.ParentNode.RightChildNode = newNode;
                        }
                        newNode.ParentNode = currentNode.ParentNode;
                    }
                    newNode.LeftChildNode = currentNode.LeftChildNode; 
                    currentNode.LeftChildNode.ParentNode = newNode;
                    newNode.RightChildNode = currentNode;
                    newNode.LeftSubtreeSize = currentNode.LeftSubtreeSize;
                    newNode.RightSubtreeSize = 1 + currentNode.RightSubtreeSize;
                    newNode.Item.LeftSubtreeRangeCount = currentNode.Item.LeftSubtreeRangeCount; ;
                    newNode.Item.RightSubtreeRangeCount = currentNode.Item.Range.Length + currentNode.Item.RightSubtreeRangeCount;
                    currentNode.ParentNode = newNode;
                    currentNode.LeftChildNode = null;
                    currentNode.LeftSubtreeSize = 0;
                    currentNode.Item.LeftSubtreeRangeCount = 0;
                    upperNode = newNode;
                }
                else
                {
                    // 'Push' the new node down left
                    newNode.LeftChildNode = currentNode.LeftChildNode;
                    newNode.RightChildNode = null;
                    newNode.LeftSubtreeSize = currentNode.LeftSubtreeSize;
                    newNode.RightSubtreeSize = 0;
                    newNode.Item.LeftSubtreeRangeCount = currentNode.Item.LeftSubtreeRangeCount;
                    newNode.Item.RightSubtreeRangeCount = 0;
                    newNode.ParentNode = currentNode;
                    if (newNode.LeftChildNode != null)
                    {
                        newNode.LeftChildNode.ParentNode = newNode;
                    }
                    currentNode.LeftChildNode = newNode;
                    currentNode.LeftSubtreeSize++;
                    currentNode.Item.LeftSubtreeRangeCount += newNode.Item.Range.Length;
                    upperNode = currentNode;
                }

                // Update the subtree sizes
                Action<WeightBalancedTreeNode<RangeAndSubtreeCounts>, Nullable<Boolean>> incrementSubtreeSizeAction = (node, nodeTraversedToFromLeft) =>
                {
                    if (nodeTraversedToFromLeft.HasValue)
                    {
                        if (nodeTraversedToFromLeft.Value == true)
                            node.LeftSubtreeSize++;
                        else
                            node.RightSubtreeSize++;
                    }
                };
                rangeTree.TraverseUpFromNode(upperNode, incrementSubtreeSizeAction);
                // Balance the tree
                rangeTree.BalanceTreeUpFromNode(upperNode);
            }

            numbersGeneratedCount++;

            return returnNumber;
        }

        # region Nested Classes

        /// <summary>
        /// Container class which holds an integer range and counts of range values in left and right subtrees.  To be used as the item in a weight balanced tree (the counts pertain to the left and right subtrees at each node of that tree).
        /// </summary>
        protected class RangeAndSubtreeCounts : IComparable<RangeAndSubtreeCounts>
        {
            /// <summary>The range of integers.</summary>
            protected LongIntegerRange range;
            /// <summary>The total count of integers in the left subtree of a node holding an instance of this class.</summary>
            protected Int64 leftSubtreeRangeCount;
            /// <summary>The total count of integers in the right subtree of a node holding an instance of this class.</summary>
            protected Int64 rightSubtreeRangeCount;

            /// <summary>
            /// The range of integers.
            /// </summary>
            public LongIntegerRange Range
            {
                get { return range; }
            }

            /// <summary>
            /// The total count of integers in the left subtree of a node holding an instance of this class.
            /// </summary>
            public Int64 LeftSubtreeRangeCount
            {
                get { return leftSubtreeRangeCount; }
                set 
                {
                    ValidateSubtreeRangeCount(leftSubtreeRangeCount, "leftSubtreeRangeCount");

                    leftSubtreeRangeCount = value; 
                }
            }

            /// <summary>
            /// The total count of integers in the right subtree of a node holding an instance of this class.
            /// </summary>
            public Int64 RightSubtreeRangeCount
            {
                get { return rightSubtreeRangeCount; }
                set 
                {
                    ValidateSubtreeRangeCount(rightSubtreeRangeCount, "rightSubtreeRangeCount");

                    rightSubtreeRangeCount = value; 
                }
            }

            /// <summary>
            /// Initialises a new instance of the MoreComplexDataStructures.UniqueRandomGenerator+RangeAndSubtreeCounts class.
            /// </summary>
            /// <param name="range">The range of integers.</param>
            /// <param name="leftSubtreeRangeCount">The total count of integers in the left subtree of a node holding an instance of this class.</param>
            /// <param name="rightSubtreeRangeCount">The total count of integers in the right subtree of a node holding an instance of this class.</param>
            public RangeAndSubtreeCounts(LongIntegerRange range, Int64 leftSubtreeRangeCount, Int64 rightSubtreeRangeCount)
            {
                ValidateSubtreeRangeCount(leftSubtreeRangeCount, "leftSubtreeRangeCount");
                ValidateSubtreeRangeCount(rightSubtreeRangeCount, "rightSubtreeRangeCount");

                this.range = range;
                this.leftSubtreeRangeCount = leftSubtreeRangeCount;
                this.rightSubtreeRangeCount = rightSubtreeRangeCount;
            }

            #pragma warning disable 1591
            public Int32 CompareTo(RangeAndSubtreeCounts other)
            {
                return this.range.CompareTo(other.range);
            }
            #pragma warning restore 1591

            #region Private/Protected Methods

            /// <summary>
            /// Throws an exception if the specified subtree range count parameter is less than 0. 
            /// </summary>
            /// <param name="value">The value of the parameter.</param>
            /// <param name="parameterName">The name of the parameter.</param>
            protected void ValidateSubtreeRangeCount(Int64 value, String parameterName)
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(parameterName, "Parameter '" + parameterName + "' must be greater than or equal to 0.");
            }

            #endregion
        }

        /// <summary>
        /// A weight balanced tree whose node item is a long integer range.
        /// </summary>
        protected class RangeTree : WeightBalancedTree<RangeAndSubtreeCounts>
        {
            /// <summary>
            /// The root node of the tree.
            /// </summary>
            public WeightBalancedTreeNode<RangeAndSubtreeCounts> RootNode
            {
                get { return rootNode; }
                set { rootNode = value; }
            }

            /// <summary>
            /// Determines whether balance of the tree should be maintained when adding or removing items.
            /// </summary>
            /// <remarks>This is exposed as public for easier unit testing (i.e. can build a specific structure in a test tree without automatic balancing).</remarks>
            public Boolean MaintainBalance
            {
                set { maintainBalance = value; }
            }

            /// <summary>
            /// Gets the node in the tree with the next range less than the range of the specified start node.
            /// </summary>
            /// <param name="startNode">The node to retrieve the next less of.</param>
            /// <returns>The node with the next range less, or null if no lower range exists.</returns>
            public new WeightBalancedTreeNode<RangeAndSubtreeCounts> GetNextLessThan(WeightBalancedTreeNode<RangeAndSubtreeCounts> startNode)
            {
                return base.GetNextLessThan(startNode);
            }

            /// <summary>
            /// Gets the node in the tree with the next range greater than the range of the specified start node.
            /// </summary>
            /// <param name="startNode">The node to retrieve the next greater of.</param>
            /// <returns>The node with the next range greater, or null if no greater range exists.</returns>
            public new WeightBalancedTreeNode<RangeAndSubtreeCounts> GetNextGreaterThan(WeightBalancedTreeNode<RangeAndSubtreeCounts> startNode)
            {
                return base.GetNextGreaterThan(startNode);
            }

            /// <summary>
            /// Traverses up the tree from the specified node to the root, invoking an action at each node.
            /// </summary>
            /// <param name="startNode">The node to start traversing at.</param>
            /// <param name="nodeAction">The action to perform at each node.  Accepts 2 parameters: the node to perform the action on, and a boolean indicating whether the current node was traversed to from the left or right (true if from the left, false if from the right, and null if the current node is the start node).</param>
            public new void TraverseUpFromNode(WeightBalancedTreeNode<RangeAndSubtreeCounts> startNode, Action<WeightBalancedTreeNode<RangeAndSubtreeCounts>, Nullable<Boolean>> nodeAction)
            {
                base.TraverseUpFromNode(startNode, nodeAction);
            }

            /// <summary>
            /// Traverses up the tree starting at the specified node, invoking the predicate function at each node to decide whether to continue traversing.  Returns the node traversal stopped at.
            /// </summary>
            /// <param name="startNode">The node to start traversing at.</param>
            /// <param name="traversePredicateFunc">A function used to decide whether traversal should continue.  Accepts 2 parameters: the node to perform the action on, and a boolean indicating whether the current node was traversed to from the left or right (true if from the left, false if from the right, and null if the current node is the start node).  Returns a boolean indicating whether traversal should continue.</param>
            /// <returns>The node of the tree where traversal stopped.</returns>
            public new WeightBalancedTreeNode<RangeAndSubtreeCounts> TraverseUpToNode(WeightBalancedTreeNode<RangeAndSubtreeCounts> startNode, Func<WeightBalancedTreeNode<RangeAndSubtreeCounts>, Nullable<Boolean>, Boolean> traversePredicateFunc)
            {
                return base.TraverseUpToNode(startNode, traversePredicateFunc);
            }

            /// <summary>
            /// Traverses upwards from the specified node, performing node rotations to balance the tree.
            /// </summary>
            /// <param name="inputNode">The node at which to start balancing.</param>
            public new void BalanceTreeUpFromNode(WeightBalancedTreeNode<RangeAndSubtreeCounts> inputNode)
            {
                base.BalanceTreeUpFromNode(inputNode);
            }

            # region Private/Protected Methods

            /// <include file='InterfaceDocumentationComments.xml' path='doc/members/member[@name="M:MoreComplexDataStructures.WeightBalancedTree`1.RotateNodeLeft(MoreComplexDataStructures.WeightBalancedTreeNode{`0})"]/*'/>
            protected override void RotateNodeLeft(WeightBalancedTreeNode<RangeAndSubtreeCounts> inputNode)
            {
                // Update the node's subtree range count properties
                inputNode.Item.RightSubtreeRangeCount = inputNode.RightChildNode.Item.LeftSubtreeRangeCount;
                inputNode.RightChildNode.Item.LeftSubtreeRangeCount += inputNode.Item.LeftSubtreeRangeCount + inputNode.Item.Range.Length;
                base.RotateNodeLeft(inputNode);
            }

            /// <include file='InterfaceDocumentationComments.xml' path='doc/members/member[@name="M:MoreComplexDataStructures.WeightBalancedTree`1.RotateNodeRight(MoreComplexDataStructures.WeightBalancedTreeNode{`0})"]/*'/>
            protected override void RotateNodeRight(WeightBalancedTreeNode<RangeAndSubtreeCounts> inputNode)
            {
                // Update the node's subtree range count properties
                inputNode.Item.LeftSubtreeRangeCount = inputNode.LeftChildNode.Item.RightSubtreeRangeCount;
                inputNode.LeftChildNode.Item.RightSubtreeRangeCount += inputNode.Item.RightSubtreeRangeCount + inputNode.Item.Range.Length;
                base.RotateNodeRight(inputNode);
            }

            #endregion
        }

        #endregion
    }
}
