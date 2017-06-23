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

namespace MoreComplexDataStructures
{
    /// <summary>
    /// Base class for tree-based implementations of heaps.
    /// </summary>
    /// <typeparam name="T">Specifies the type of items held in the heap.</typeparam>
    public abstract class HeapBase<T> where T : IComparable<T>
    {
        /// <summary>The root node of the tree.</summary>
        protected DoublyLinkedTreeNode<T> rootNode;
        /// <summary>The number of items in the heap.</summary>
        protected Int32 count;
        /// <summary>The 1-based vertical position in the tree where the next new item should be inserted (i.e. the root node is at depth 1).</summary>
        protected Int32 nextInsertDepth;
        /// <summary>The 1-based horizontal position in the tree where the next new item should be inserted.</summary>
        protected Int32 nextInsertHorizontalPosition;

        /// <summary>
        /// The total number of items stored in the heap.
        /// </summary>
        public Int32 Count
        {
            get
            {
                return count;
            }
        }

        /// <summary>
        /// Initialises a new instance of the MoreComplexDataStructures.HeapBase class.
        /// </summary>
        protected HeapBase()
        {
            Initialise();
        }

        /// <summary>
        /// Initialises a new instance of the MoreComplexDataStructures.HeapBase class that contains elements copied from the specified collection.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new HeapBase.</param>
        protected HeapBase(IEnumerable<T> collection)
            : this()
        {
            foreach (T currentElement in collection)
            {
                Insert(currentElement);
            }
        }

        /// <summary>
        /// Adds the specified item to the heap.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public abstract void Insert(T item);

        /// <summary>
        /// Removes all items from the heap.
        /// </summary>
        public void Clear()
        {
            Initialise();
        }

        /// <summary>
        /// Performs breadth-first search of the tree underlying the heap, invoking the specified action at each node.
        /// </summary>
        /// <param name="nodeAction">The action to perform at each node.  Accepts a single parameter which is the current node to perform the action on.</param>
        public void BreadthFirstSearch(Action<DoublyLinkedTreeNode<T>> nodeAction)
        {
            Queue<DoublyLinkedTreeNode<T>> traversalQueue = new Queue<DoublyLinkedTreeNode<T>>();
            if (rootNode != null)
            {
                traversalQueue.Enqueue(rootNode);
            }

            while (traversalQueue.Count > 0)
            {
                DoublyLinkedTreeNode<T> currentNode = traversalQueue.Dequeue();
                nodeAction.Invoke(currentNode);
                if (currentNode.LeftChildNode != null)
                {
                    traversalQueue.Enqueue(currentNode.LeftChildNode);
                }
                if (currentNode.RightChildNode != null)
                {
                    traversalQueue.Enqueue(currentNode.RightChildNode);
                }
            }
        }

        # region Private/Protected Methods

        /// <summary>
        /// Sets the heap to an initial state (e.g. used on construction or resetting/clearing).
        /// </summary>
        protected void Initialise()
        {
            rootNode = null;
            count = 0;
            nextInsertDepth = 1;
            nextInsertHorizontalPosition = 1;
        }

        /// <summary>
        /// Throws an exception if the heap is empty.
        /// </summary>
        protected void CheckNotEmpty()
        {
            if (rootNode == null)
            {
                throw new Exception("The heap is empty.");
            }
        }

        /// <summary>
        /// Increments members nextInsertDepth and nextInsertPosition.
        /// </summary>
        protected void IncrementNextInsertPosition()
        {
            if (nextInsertHorizontalPosition == Convert.ToInt32(Math.Pow(2, nextInsertDepth - 1)))
            {
                nextInsertHorizontalPosition = 1;
                nextInsertDepth++;
            }
            else
            {
                nextInsertHorizontalPosition++;
            }
        }

        /// <summary>
        /// Decrements members nextInsertDepth and nextInsertPosition.
        /// </summary>
        protected void DecrementNextInsertPosition()
        {
            Tuple<Int32, Int32> newPositionData = CalculatePreviousPosition(nextInsertDepth, nextInsertHorizontalPosition);
            nextInsertDepth = newPositionData.Item1;
            nextInsertHorizontalPosition = newPositionData.Item2;
        }

        /// <summary>
        /// Calculates the previous left-to-right, top-to-bottom position in the tree, based on the inputted depth and horizontal position.
        /// </summary>
        /// <param name="depth">The depth to calculate the previous position of.</param>
        /// <param name="horizontalPosition">The horizontal position to calculate the previous position of.</param>
        /// <returns>A tuple containing 2 integers: the previous depth, and the previous horizontal position.</returns>
        protected Tuple<Int32, Int32> CalculatePreviousPosition(Int32 depth, Int32 horizontalPosition)
        {
            Int32 previousDepth, previousHorizontalPosition;

            if (horizontalPosition == 1)
            {
                previousDepth = depth - 1;
                previousHorizontalPosition = Convert.ToInt32(Math.Pow(2, previousDepth - 1));
            }
            else
            {
                previousDepth = depth;
                previousHorizontalPosition = horizontalPosition - 1;
            }

            return new Tuple<Int32, Int32>(previousDepth, previousHorizontalPosition);
        }

        /// <summary>
        /// Traverses to the specified position in the tree, returning the node at that position.
        /// </summary>
        /// <param name="depth">The depth to traverse to.</param>
        /// <param name="horizontalPosition">The horizontal position to traverse to.</param>
        /// <returns>The node at the specified position.</returns>
        protected DoublyLinkedTreeNode<T> TraverseToPosition(Int32 depth, Int32 horizontalPosition)
        {
            if (depth < 1)
            {
                throw new ArgumentException("Parameter 'depth' must be greater than or equal to 1.", "depth");
            }
            if (horizontalPosition < 1)
            {
                throw new ArgumentException("Parameter 'horizontalPosition' must be greater than or equal to 1.", "horizontalPosition");
            }
            Int32 maximumPossiblePosition = Convert.ToInt32(Math.Pow(2, depth - 1));
            if (horizontalPosition > maximumPossiblePosition)
            {
                throw new ArgumentException("Parameter 'horizontalPosition' is greater than maximum possible value " + maximumPossiblePosition + " for depth " + depth + ".", "horizontalPosition");
            }
            if (rootNode == null)
            {
                throw new Exception("The heap is empty.");
            }

            Int32 currentDepth = 1;
            Int32 currentHorizontalPosition = 1;
            // The target horizontal position with respect to the current level
            Int32 targetHorizontalPosition = horizontalPosition;
            DoublyLinkedTreeNode<T> currentNode = rootNode;

            while (currentDepth < depth)
            {
                // Calculate the depth of the subtree rooted by currentNode (0-based)
                Int32 currentSubtreeDepth = depth - currentDepth;

                // Decide which direction to traverse
                // If targetHorizontalPosition is within the bottom half of the leaf nodes of the current current subtree move left, otherwise move right
                Int32 numberOfLeafNodesInCurrentSubtree = Convert.ToInt32(Math.Pow(2, currentSubtreeDepth));
                if (targetHorizontalPosition <= (numberOfLeafNodesInCurrentSubtree / 2))
                {
                    currentHorizontalPosition = (currentHorizontalPosition * 2) - 1;
                    // Move left (in this case we don't need to update targetHorizontalPosition)
                    if (currentNode.LeftChildNode == null)
                    {
                        throw new Exception("Failed to traverse to position " + depth + ", " + horizontalPosition + ".  No node found at position " + (currentDepth + 1) + ", " + currentHorizontalPosition + ".");
                    }
                    else
                    {
                        currentNode = currentNode.LeftChildNode;
                    }
                }
                else
                {
                    // Move right
                    currentHorizontalPosition = currentHorizontalPosition * 2;
                    if (currentNode.RightChildNode == null)
                    {
                        throw new Exception("Failed to traverse to position " + depth + ", " + horizontalPosition + ".  No node found at position " + (currentDepth + 1) + ", " + currentHorizontalPosition + ".");
                    }
                    else
                    {
                        targetHorizontalPosition = targetHorizontalPosition - (numberOfLeafNodesInCurrentSubtree / 2);
                        currentNode = currentNode.RightChildNode;
                    }
                }
                currentDepth++;
            }

            return currentNode;
        }

        #endregion
    }
}
