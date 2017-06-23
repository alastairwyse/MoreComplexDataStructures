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
    /// A tree-based implementation of a min heap.
    /// </summary>
    /// <typeparam name="T">Specifies the type of items held in the heap.</typeparam>
    public class MinHeap<T> : HeapBase<T> where T : IComparable<T>
    {
        /// <summary>
        /// Initialises a new instance of the MoreComplexDataStructures.MinHeap class.
        /// </summary>
        public MinHeap()
            : base()
        {
        }

        /// <summary>
        /// Initialises a new instance of the MoreComplexDataStructures.MinHeap class that contains elements copied from the specified collection.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new MinHeap.</param>
        public MinHeap(IEnumerable<T> collection)
            : base(collection)
        {
        }

        /// <summary>
        /// Returns the minimum item in the heap without removing it.
        /// </summary>
        /// <returns>The minimum item in the heap.</returns>
        public T Peek()
        {
            CheckNotEmpty();
            return rootNode.Item;
        }

        /// <summary>
        /// Adds the specified item to the heap.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public override void Insert(T item)
        {
            if (rootNode == null)
            {
                rootNode = new DoublyLinkedTreeNode<T>(item, null);
            }
            else
            {
                // Navigate to the parent of the next insert position
                Int32 nextInsertPositionParentHorizontalPosition = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(nextInsertHorizontalPosition) / 2.0));
                DoublyLinkedTreeNode<T> parentNode = TraverseToPosition(nextInsertDepth - 1, nextInsertPositionParentHorizontalPosition);

                // Create the new node
                DoublyLinkedTreeNode<T> newNode = new DoublyLinkedTreeNode<T>(item, parentNode);

                // If nextInsertHorizontalPosition has a remainder after division by 2 insert on the left, otherwise insert on the right
                if ((Convert.ToDouble(nextInsertHorizontalPosition) % 2) != 0)
                {
                    parentNode.LeftChildNode = newNode;
                }
                else
                {
                    parentNode.RightChildNode = newNode;
                }

                // Traverse up the tree to preserve the correct ordering.  Keep traversing and swapping values with the parent until the parent item is less than the child item.
                DoublyLinkedTreeNode<T> currentNode = newNode;
                while (currentNode.ParentNode != null && currentNode.Item.CompareTo(currentNode.ParentNode.Item) <= -1)
                {
                    // Swap the values
                    T tempItem = currentNode.Item;
                    currentNode.Item = currentNode.ParentNode.Item;
                    currentNode.ParentNode.Item = tempItem;

                    currentNode = currentNode.ParentNode;
                }
            }

            IncrementNextInsertPosition();

            count++;
        }

        /// <summary>
        /// Removes and returns the minimum item in the heap.
        /// </summary>
        /// <returns>The minimum item in the heap.</returns>
        public T ExtractMin()
        {
            T returnItem = default(T);

            CheckNotEmpty();
            if (count == 1)
            {
                returnItem = rootNode.Item;
                rootNode = null;
            }
            else
            {
                returnItem = rootNode.Item;

                // Find the position in the tree which was the last inserted to
                Tuple<Int32, Int32> previousPosition = CalculatePreviousPosition(nextInsertDepth, nextInsertHorizontalPosition);
                Int32 previoustDepth = previousPosition.Item1;
                Int32 previoustHorizontalPosition = previousPosition.Item2;

                // Get the node at this position 
                DoublyLinkedTreeNode<T> lastInsertedNode = TraverseToPosition(previoustDepth, previoustHorizontalPosition);

                // Set root node's value to that of the last inserted node
                rootNode.Item = lastInsertedNode.Item;

                // Remove the last inserted node from the tree
                if (lastInsertedNode.ParentNode.LeftChildNode == lastInsertedNode)
                {
                    lastInsertedNode.ParentNode.LeftChildNode = null;
                }
                else
                {
                    lastInsertedNode.ParentNode.RightChildNode = null;
                }

                // Traverse down the tree to preserve the proper ordering
                DoublyLinkedTreeNode<T> currentNode = rootNode;
                Boolean keepTraversing = true;
                // Need to keep traversing until either of the following conditions are true...
                //   Both child nodes of currentNode are null
                //   The items of both child nodes of currentNode are greater than currentNode's item
                while (keepTraversing == true)
                {
                    // If currentNode is not the root, swap its item with its parent
                    if (currentNode.ParentNode != null)
                    {
                        T tempItem = currentNode.ParentNode.Item;
                        currentNode.ParentNode.Item = currentNode.Item;
                        currentNode.Item = tempItem;
                    }
                    if (currentNode.LeftChildNode == null && currentNode.RightChildNode == null)
                    {
                        keepTraversing = false;
                    }
                    else if (currentNode.RightChildNode == null)
                    {
                        // Traverse to the left child if it's item is less than currentNode's item
                        if (currentNode.Item.CompareTo(currentNode.LeftChildNode.Item) > 0)
                        {
                            currentNode = currentNode.LeftChildNode;
                        }
                        else
                        {
                            keepTraversing = false;
                        }
                    }
                    // TODO: If the tree structure is properly maintained, this else branch will never be used (impossible to have a tree node which has a right child, but no left child)
                    //   Consider removing this branch once functionality is solid
                    else if (currentNode.LeftChildNode == null)
                    {
                        // Traverse to the right child if it's item is less than currentNode's item
                        if (currentNode.Item.CompareTo(currentNode.RightChildNode.Item) > 0)
                        {
                            currentNode = currentNode.RightChildNode;
                        }
                        else
                        {
                            keepTraversing = false;
                        }
                    }
                    else
                    {
                        if (currentNode.Item.CompareTo(currentNode.LeftChildNode.Item) > 0 || currentNode.Item.CompareTo(currentNode.RightChildNode.Item) > 0)
                        {
                            // Traverse to the node with the lower value
                            if (currentNode.LeftChildNode.Item.CompareTo(currentNode.RightChildNode.Item) < 0)
                            {
                                currentNode = currentNode.LeftChildNode;
                            }
                            else
                            {
                                currentNode = currentNode.RightChildNode;
                            }
                        }
                        else
                        {
                            keepTraversing = false;
                        }
                    }
                }
            }

            DecrementNextInsertPosition();

            count--;

            return returnItem;
        }
    }
}
