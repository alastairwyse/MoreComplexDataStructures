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

namespace MoreComplexDataStructures
{
    /// <summary>
    /// A weight-balanced binary search tree where each node maintains the size of its left and right subtrees.
    /// </summary>
    /// <typeparam name="T">Specifies the type of items held by nodes of the tree.</typeparam>
    /// <remarks>Weight-balanced trees are generally self balancing, however this class does not implement any auto-balancing at this stage.</remarks>
    public class WeightBalancedTree<T> : IBinarySearchTree<T> where T : IComparable<T>
    {
        /// <summary>The root node of the tree.</summary>
        protected WeightBalancedTreeNode<T> rootNode;
        /// <summary>The depth of nodes in the tree.</summary>
        protected Int32 depth;
        /// <summary>Determines whether an item has been removed from the tree.  The Depth property is must be calculated by full traversal after an item has been removed.</summary>
        protected Boolean itemHasBeenRemoved;
        /// <summary>Random number generator to use for the GetRandomItem() method.</summary>
        protected Random randomGenerator;
        /// <summary>Used when removing items from the tree, to decide whether to use the next less than or next greater than when swapping a lower node's value with the one to be removed.</summary>
        protected Boolean swapNextLessThanOnRemove;
        /// <summary>Used when searching up the tree for a node containing a next lower value.  Passed to the TraverseUpToNode() method.</summary>
        protected Func<WeightBalancedTreeNode<T>, Nullable<Boolean>, Boolean> getNextLessThanTraverseUpAction;
        /// <summary>Used when searching up the tree for a node containing a next greater value.  Passed to the TraverseUpToNode() method.</summary>
        protected Func<WeightBalancedTreeNode<T>, Nullable<Boolean>, Boolean> getNextGreaterThanTraverseUpAction;

        /// <include file='InterfaceDocumentationComments.xml' path='doc/members/member[@name="P:MoreComplexDataStructures.IBinarySearchTree`1.Count"]/*'/>
        public Int32 Count
        {
            get
            {
                if (rootNode == null)
                {
                    return 0;
                }
                else
                {
                    return (1 + rootNode.LeftSubtreeSize + rootNode.RightSubtreeSize);
                }
            }
        }

        /// <summary>
        /// The depth of nodes in the tree (0-based).
        /// </summary>
        /// <remarks>If the Remove() method has not been called on the tree since initialising or clearing, the depth is maintained in an internal variable and can be returned with O(1) time complexity.  If the Remove() method has been called, the depth is found via a depth first search and hence consumes O(n) time complexity (where 'n' is the number of items currently held by the tree).</remarks>
        public Int32 Depth
        {
            get
            {
                if (itemHasBeenRemoved == false)
                {
                    return depth;
                }
                else
                {
                    return GetDepthViaDepthFirstSearch();
                }
            }
        }

        /// <summary>
        /// Initialises a new instance of the MoreComplexDataStructures.WeightBalancedTree class.
        /// </summary>
        public WeightBalancedTree()
        {
            rootNode = null;
            depth = -1;
            itemHasBeenRemoved = false;
            randomGenerator = new Random();
            swapNextLessThanOnRemove = true;

            getNextLessThanTraverseUpAction = (node, traversedFromLeft) =>
            {
                if (node.ParentNode == null)
                {
                    // This is the root, so stop traversing
                    return false;
                }
                else
                {
                    // We want to keep moving up until the last move was from the right (i.e. until we encounter the parent of a right child)
                    //   Null value means this is the first node traversed to
                    if (traversedFromLeft == null || traversedFromLeft == true)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            };

            getNextGreaterThanTraverseUpAction = (node, traversedFromLeft) =>
            {
                if (node.ParentNode == null)
                {
                    // This is the root, so stop traversing
                    return false;
                }
                else
                {
                    // We want to keep moving up until the last move was from the left (i.e. until we encounter the parent of a left child)
                    //   Null value means this is the first node traversed to
                    if (traversedFromLeft == null || traversedFromLeft == false)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            };
        }

        /// <summary>
        /// Initialises a new instance of the MoreComplexDataStructures.WeightBalancedTree class that contains elements copied from the specified collection.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new WeightBalancedTree.</param>
        public WeightBalancedTree(IEnumerable<T> collection)
            : this()
        {
            foreach (T currentElement in collection)
            {
                Add(currentElement);
            }
        }

        /// <include file='InterfaceDocumentationComments.xml' path='doc/members/member[@name="M:MoreComplexDataStructures.IBinarySearchTree`1.Clear"]/*'/>
        public void Clear()
        {
            rootNode = null;
            depth = -1;
            itemHasBeenRemoved = false;
            swapNextLessThanOnRemove = true;
        }

        /// <include file='InterfaceDocumentationComments.xml' path='doc/members/member[@name="M:MoreComplexDataStructures.IBinarySearchTree`1.Add(`0)"]/*'/>
        public void Add(T item)
        {
            if (rootNode == null)
            {
                rootNode = new WeightBalancedTreeNode<T>(item, null);
                depth = 0;
            }
            else
            {
                WeightBalancedTreeNode<T> currentNode = rootNode;
                Int32 currentDepth = 0;
                Boolean added = false;

                while (added == false)
                {
                    Int32 comparisonResult = currentNode.Item.CompareTo(item);
                    if (comparisonResult == 0)
                    {
                        // Decrement the subtree counts
                        Action<WeightBalancedTreeNode<T>, Nullable<Boolean>> decrementCountsAction = (node, traversedFromLeft) =>
                        {
                            // currentNode was not incremented, so no need to decrement
                            if (node != currentNode)
                            {
                                if (traversedFromLeft == true)
                                {
                                    node.LeftSubtreeSize--;
                                }
                                else
                                {
                                    node.RightSubtreeSize--;
                                }
                            }
                        };
                        TraverseUpFromNode(currentNode, decrementCountsAction);

                        throw new ArgumentException("A node holding the item specified in parameter 'item' (value = '" + item.ToString() + "') already exists in the tree.", "item");
                    }
                    else if (comparisonResult < 0)
                    {
                        currentNode.RightSubtreeSize++;
                        currentDepth++;
                        // currentNode.Item < item so add to or move to the right
                        if (currentNode.RightChildNode == null)
                        {
                            currentNode.RightChildNode = new WeightBalancedTreeNode<T>(item, currentNode);
                            if (currentDepth > depth) depth = currentDepth;
                            added = true;
                        }
                        else
                        {
                            currentNode = currentNode.RightChildNode;
                        }
                    }
                    else if (comparisonResult > 0)
                    {
                        currentNode.LeftSubtreeSize++;
                        currentDepth++;
                        // currentNode.Item > item so add to or move to the left
                        if (currentNode.LeftChildNode == null)
                        {
                            currentNode.LeftChildNode = new WeightBalancedTreeNode<T>(item, currentNode);
                            if (currentDepth > depth) depth = currentDepth;
                            added = true;
                        }
                        else
                        {
                            currentNode = currentNode.LeftChildNode;
                        }
                    }
                }
            }
        }

        /// <include file='InterfaceDocumentationComments.xml' path='doc/members/member[@name="M:MoreComplexDataStructures.IBinarySearchTree`1.Remove(`0)"]/*'/>
        public void Remove(T item)
        {
            if (rootNode == null)
            {
                throw new ArgumentException("The specified item ('" + item.ToString() + "') does not exist in the tree.", "item");
            }
            else
            {
                // Attempt to traverse to the node holding the item
                WeightBalancedTreeNode<T> currentNode = rootNode;
                Boolean removed = false;

                try
                {
                    while (removed == false)
                    {
                        Int32 comparisonResult = currentNode.Item.CompareTo(item);
                        if (comparisonResult == 0)
                        {
                            if (currentNode.LeftChildNode == null && currentNode.RightChildNode == null)
                            {
                                // The current node can be deleted
                                if (currentNode.ParentNode == null)
                                {
                                    // The current node is the root node
                                    rootNode = null;
                                }
                                else
                                {
                                    RemoveNode(currentNode);
                                }
                            }
                            else if (currentNode.LeftChildNode == null)
                            {
                                // The right child of the current node can replace the current node
                                if (currentNode.ParentNode == null)
                                {
                                    rootNode = currentNode.RightChildNode;
                                    currentNode.RightChildNode = null;
                                    rootNode.ParentNode = null;
                                }
                                else
                                {
                                    RemoveNode(currentNode);
                                }
                            }
                            else if (currentNode.RightChildNode == null)
                            {
                                // The left child of the current node can replace the current node
                                if (currentNode.ParentNode == null)
                                {
                                    rootNode = currentNode.LeftChildNode;
                                    currentNode.LeftChildNode = null;
                                    rootNode.ParentNode = null;
                                }
                                else
                                {
                                    RemoveNode(currentNode);
                                }
                            }
                            else
                            {
                                // The current node has left and right children, so we have to swap the current node with its next less than, or next greater than.
                                WeightBalancedTreeNode<T> nodeContainingItemToDelete = currentNode;
                                if (swapNextLessThanOnRemove == true)
                                {
                                    // Swap with the next less than... find the next less than by moving left and then as far right as possible
                                    currentNode.LeftSubtreeSize--;
                                    currentNode = currentNode.LeftChildNode;
                                    while (currentNode.RightChildNode != null)
                                    {
                                        currentNode.RightSubtreeSize--;
                                        currentNode = currentNode.RightChildNode;
                                    }
                                    swapNextLessThanOnRemove = false;
                                }
                                else
                                {
                                    // Swap with the next greater than... find the next greater than by moving right and then as far left as possible
                                    currentNode.RightSubtreeSize--;
                                    currentNode = currentNode.RightChildNode;
                                    while (currentNode.LeftChildNode != null)
                                    {
                                        currentNode.LeftSubtreeSize--;
                                        currentNode = currentNode.LeftChildNode;
                                    }
                                    swapNextLessThanOnRemove = true;
                                }
                                // Swap the node items
                                RemoveNode(currentNode);
                                nodeContainingItemToDelete.Item = currentNode.Item;
                            }
                            itemHasBeenRemoved = true;
                            removed = true;
                        }
                        else if (comparisonResult < 0)
                        {
                            // currentNode.Item < item so move to the right
                            if (currentNode.RightChildNode != null)
                            {
                                currentNode.RightSubtreeSize--;
                                currentNode = currentNode.RightChildNode;
                            }
                            else
                            {
                                throw new ArgumentException("The specified item ('" + item.ToString() + "') does not exist in the tree.", "item");
                            }
                        }
                        else if (comparisonResult > 0)
                        {
                            // currentNode.Item > item so move to the left
                            if (currentNode.LeftChildNode != null)
                            {
                                currentNode.LeftSubtreeSize--;
                                currentNode = currentNode.LeftChildNode;
                            }
                            else
                            {
                                throw new ArgumentException("The specified item ('" + item.ToString() + "') does not exist in the tree.", "item");
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    // Traverse back up the tree, incrementing the subtree counts
                    Action<WeightBalancedTreeNode<T>, Nullable<Boolean>> incrementCountsAction = (node, traversedFromLeft) =>
                    {
                        // currentNode was not incremented, so no need to decrement
                        if (node != currentNode)
                        {
                            if (traversedFromLeft == true)
                            {
                                node.LeftSubtreeSize++;
                            }
                            else
                            {
                                node.RightSubtreeSize++;
                            }
                        }
                    };
                    TraverseUpFromNode(currentNode, incrementCountsAction);

                    throw;
                }
            }
        }

        /// <include file='InterfaceDocumentationComments.xml' path='doc/members/member[@name="M:MoreComplexDataStructures.IBinarySearchTree`1.Contains(`0)"]/*'/>
        public Boolean Contains(T item)
        {
            WeightBalancedTreeNode<T> itemNode = TraverseDownToNodeHoldingItem(item);

            if (itemNode != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the next value in the tree less than the specified item.
        /// </summary>
        /// <param name="item">The item to retrieve the next less of.</param>
        /// <returns>A tuple containing 2 values: a boolean indicating whether a lower value was found (false if no lower value exists), and the next item less than the specified item (or null / type default if no lower item exists).</returns>
        public Tuple<Boolean, T> GetNextLessThan(T item)
        {
            if (rootNode == null)
            {
                return new Tuple<Boolean, T>(false, default (T));
            }

            Tuple<Boolean, WeightBalancedTreeNode<T>> searchResult = TraverseDownToNodeHoldingItemOrParent(item, (node) => { });

            if (searchResult.Item1 == false)
            {
                // Compare the value of the returned node against parameter item
                Int32 comparisonResult = searchResult.Item2.Item.CompareTo(item);
                if (comparisonResult < 0)
                {
                    // result item < item 
                    return new Tuple<Boolean, T>(true, searchResult.Item2.Item);
                }
                else
                {
                    // result item > item 
                    WeightBalancedTreeNode<T> returnNode = TraverseUpToNode(searchResult.Item2, getNextLessThanTraverseUpAction);
                    // TraverseUpToNode() will not move past the root node, so need to check whether the returned node is actually less than parameter item
                    if (returnNode.Item.CompareTo(item) < 0)
                    {
                        return new Tuple<Boolean, T>(true, returnNode.Item);
                    }
                    else
                    {
                        // There are no lower items in the tree
                        return new Tuple<Boolean, T>(false, default(T));
                    }
                }
            }
            else
            {
                WeightBalancedTreeNode<T> returnNode = GetNextLessThan(searchResult.Item2);
                if (returnNode == null)
                {
                    return new Tuple<Boolean, T>(false, default(T));
                }
                else
                {
                    return new Tuple<Boolean, T>(true, returnNode.Item);
                }
            }
        }

        /// <summary>
        /// Gets the next value in the tree greater than the specified item.
        /// </summary>
        /// <param name="item">The item to retrieve the next greater of.</param>
        /// <returns>A tuple containing 2 values: a boolean indicating whether a greater value was found (false if no greater value exists), and the next item greater than the specified item (or null / type default if no greater item exists).</returns>
        public Tuple<Boolean, T> GetNextGreaterThan(T item)
        {
            if (rootNode == null)
            {
                return new Tuple<Boolean, T>(false, default(T));
            }

            Tuple<Boolean, WeightBalancedTreeNode<T>> searchResult = TraverseDownToNodeHoldingItemOrParent(item, (node) => { });

            if (searchResult.Item1 == false)
            {
                // Compare the value of the returned node against parameter item
                Int32 comparisonResult = searchResult.Item2.Item.CompareTo(item);
                if (comparisonResult < 0)
                {
                    // result item < item 
                    WeightBalancedTreeNode<T> returnNode = TraverseUpToNode(searchResult.Item2, getNextGreaterThanTraverseUpAction);
                    // TraverseUpToNode() will not move past the root node, so need to check whether the returned node is actually greater than parameter item
                    if (returnNode.Item.CompareTo(item) > 0)
                    {
                        return new Tuple<Boolean, T>(true, returnNode.Item);
                    }
                    else
                    {
                        // There are no greater items in the tree
                        return new Tuple<Boolean, T>(false, default(T));
                    }
                }
                else
                {
                    // result item > item 
                    return new Tuple<Boolean, T>(true, searchResult.Item2.Item);
                }
            }
            else
            {
                WeightBalancedTreeNode<T> returnNode = GetNextGreaterThan(searchResult.Item2);
                if (returnNode == null)
                {
                    return new Tuple<Boolean, T>(false, default(T));
                }
                else
                {
                    return new Tuple<Boolean, T>(true, returnNode.Item);
                }
            }
        }

        /// <summary>
        /// Returns a count of the number of items in the tree less than the specified item.
        /// </summary>
        /// <param name="item">The item to retrieve the count of items less than.</param>
        /// <returns>The number of items in the tree less than the specified item.</returns>
        public Int32 GetCountLessThan(T item)
        {
            if (rootNode == null)
            {
                return 0;
            }

            Int32 count = 0;
            Action<WeightBalancedTreeNode<T>> traverseAction = (node) =>
            {
                // If this is the root node, add the node and both subtrees to the count
                if (node.ParentNode == null)
                {
                    count = count + 1 + node.LeftSubtreeSize + node.RightSubtreeSize;
                }
                Int32 comparisonResult = node.Item.CompareTo(item);
                if (comparisonResult >= 0)
                {
                    // node.Item >= item, so remove this node and all greater (in right subtree) from the count
                    count = count - (1 + node.RightSubtreeSize);
                }
            };
            Tuple<Boolean, WeightBalancedTreeNode<T>> searchResult = TraverseDownToNodeHoldingItemOrParent(item, traverseAction);

            return count;
        }

        /// <summary>
        /// Returns a count of the number of items in the tree greater than the specified item.
        /// </summary>
        /// <param name="item">The item to retrieve the count of items greater than.</param>
        /// <returns>The number of items in the tree greater than the specified item.</returns>
        public Int32 GetCountGreaterThan(T item)
        {
            if (rootNode == null)
            {
                return 0;
            }

            Int32 count = 0;
            Action<WeightBalancedTreeNode<T>> traverseAction = (node) =>
            {
                // If this is the root node, add the node and both subtrees to the count
                if (node.ParentNode == null)
                {
                    count = count + 1 + node.LeftSubtreeSize + node.RightSubtreeSize;
                }
                Int32 comparisonResult = node.Item.CompareTo(item);
                if (comparisonResult <= 0)
                {
                    // node.Item <= item, so remove this node and all less (in left subtree) from the count
                    count = count - (1 + node.LeftSubtreeSize);
                }
            };
            Tuple<Boolean, WeightBalancedTreeNode<T>> searchResult = TraverseDownToNodeHoldingItemOrParent(item, traverseAction);

            return count;
        }
        
        /// <summary>
        /// Returns an enumerator containing all items in the tree less than the specified item.
        /// </summary>
        /// <param name="item">The item to retrieve all items greater than.</param>
        /// <returns>An enumerator containing all items in the tree less than the specified item.</returns>
        public IEnumerable<T> GetAllLessThan(T item)
        {
            Tuple<Boolean, T> firstNodeResult = GetNextLessThan(item);
            if (firstNodeResult.Item1 == true)
            {
                // TODO: Could improve efficiency here by not traversing to the first node twice
                //   Could refactor a private version of GetNextLessThan(T item) which returns Tuple<Boolean, WeightBalancedTreeNode<T>> rather than Tuple<Boolean, T>
                WeightBalancedTreeNode<T> currentNode = TraverseDownToNodeHoldingItem(firstNodeResult.Item2);
                while (currentNode != null)
                {
                    yield return currentNode.Item;
                    currentNode = GetNextLessThan(currentNode);
                }
            }
        }

        /// <summary>
        /// Returns an enumerator containing all items in the tree greater than the specified item.
        /// </summary>
        /// <param name="item">The item to retrieve all items less than.</param>
        /// <returns>An enumerator containing all items in the tree greater than the specified item.</returns>
        public IEnumerable<T> GetAllGreaterThan(T item)
        {
            Tuple<Boolean, T> firstNodeResult = GetNextGreaterThan(item);
            if (firstNodeResult.Item1 == true)
            {
                // TODO: Could improve efficiency here by not traversing to the first node twice
                //   Could refactor a private version of GetNextGreaterThan(T item) which returns Tuple<Boolean, WeightBalancedTreeNode<T>> rather than Tuple<Boolean, T>
                WeightBalancedTreeNode<T> currentNode = TraverseDownToNodeHoldingItem(firstNodeResult.Item2);
                while (currentNode != null)
                {
                    yield return currentNode.Item;
                    currentNode = GetNextGreaterThan(currentNode);
                }
            }
        }

        /// <summary>
        /// Returns a random item from the tree.
        /// </summary>
        /// <returns>A random item.</returns>
        /// <exception cref="System.Exception">The tree is empty.</exception>
        public T GetRandomItem()
        {
            WeightBalancedTreeNode<T> currentNode = rootNode;

            if (currentNode == null)
            {
                throw new Exception("The tree is empty.");
            }

            while (true)
            {
                // Decide whether to stop at this node
                Int32 stopIndicator = randomGenerator.Next(1 + currentNode.LeftSubtreeSize + currentNode.RightSubtreeSize);
                if (stopIndicator == 0)
                {
                    break;
                }

                // Decide whether to move left or right
                Int32 directionIndicator = randomGenerator.Next(currentNode.LeftSubtreeSize + currentNode.RightSubtreeSize);
                if (directionIndicator < currentNode.LeftSubtreeSize)
                {
                    if (currentNode.LeftChildNode == null)
                    {
                        break;
                    }
                    else
                    {
                        currentNode = currentNode.LeftChildNode;
                    }
                }
                else
                {
                    if (currentNode.RightChildNode == null)
                    {
                        break;
                    }
                    else
                    {
                        currentNode = currentNode.RightChildNode;
                    }
                }
            }

            return currentNode.Item;
        }

        /// <include file='InterfaceDocumentationComments.xml' path='doc/members/member[@name="M:MoreComplexDataStructures.IBinarySearchTree`1.PreOrderDepthFirstSearch(System.Action{MoreComplexDataStructures.WeightBalancedTreeNode{`0}})"]/*'/>
        public void PreOrderDepthFirstSearch(Action<WeightBalancedTreeNode<T>> nodeAction)
        {
            // Stack to hold the nodes to recurse to
            Stack<WeightBalancedTreeNode<T>> recursionStack = new Stack<WeightBalancedTreeNode<T>>();
            if (rootNode != null) recursionStack.Push(rootNode);

            while (recursionStack.Count > 0)
            {
                WeightBalancedTreeNode<T> currentNode = recursionStack.Pop();
                nodeAction.Invoke(currentNode);
                
                if (currentNode.RightChildNode != null)
                {
                    recursionStack.Push(currentNode.RightChildNode);
                }
                if (currentNode.LeftChildNode != null)
                {
                    recursionStack.Push(currentNode.LeftChildNode);
                }
            }
        }

        /// <include file='InterfaceDocumentationComments.xml' path='doc/members/member[@name="M:MoreComplexDataStructures.IBinarySearchTree`1.InOrderDepthFirstSearch(System.Action{MoreComplexDataStructures.WeightBalancedTreeNode{`0}})"]/*'/>
        public void InOrderDepthFirstSearch(Action<WeightBalancedTreeNode<T>> nodeAction)
        {
            // Stack to hold the nodes to recurse to
            Stack<WeightBalancedTreeNode<T>> recursionStack = new Stack<WeightBalancedTreeNode<T>>();
            WeightBalancedTreeNode<T> currentNode = rootNode;
            // Traverse as far to the left as possible, pushing all nodes onto the stack
            while (currentNode != null)
            {
                recursionStack.Push(currentNode);
                currentNode = currentNode.LeftChildNode;
            }

            while (recursionStack.Count > 0)
            {
                currentNode = recursionStack.Pop();
                nodeAction.Invoke(currentNode);

                if (currentNode.RightChildNode != null)
                {
                    currentNode = currentNode.RightChildNode;

                    while (currentNode != null)
                    {
                        recursionStack.Push(currentNode);
                        currentNode = currentNode.LeftChildNode;
                    }
                }
            }
        }

        /// <include file='InterfaceDocumentationComments.xml' path='doc/members/member[@name="M:MoreComplexDataStructures.IBinarySearchTree`1.PostOrderDepthFirstSearch(System.Action{MoreComplexDataStructures.WeightBalancedTreeNode{`0}})"]/*'/>
        public void PostOrderDepthFirstSearch(Action<WeightBalancedTreeNode<T>> nodeAction)
        {
            // TODO: Looked at many suggested algorithms for post-order search online, but couldn't find any nice implementation (e.g. some involved storing all nodes in the tree in a stack, others modified the stack while traversing).
            //   Below implementation mimics recursion... easier to understand at the expense of having to create wrapping NodeRecursionStatus objects.
            //   For the case of a balanced tree, wrapping objects are negligable, as there will only be as many wrapping objects as a single path through the tree.
            //   For completely unbalanced, below implementation is a bit costly in terms of memory, as wrapping objects will be created for every node in the tree.

            // Stack to hold the nodes to recurse to
            Stack<NodeRecursionStatus<T>> recursionStack = new Stack<NodeRecursionStatus<T>>();

            if (rootNode != null)
            {
                recursionStack.Push(new NodeRecursionStatus<T>(rootNode));
            }

            while (recursionStack.Count > 0)
            {
                NodeRecursionStatus<T> currentNodeRecursionStatus = recursionStack.Peek();

                if (currentNodeRecursionStatus.LeftChildTreeProcessed == false && currentNodeRecursionStatus.Node.LeftChildNode != null)
                {
                    currentNodeRecursionStatus.LeftChildTreeProcessed = true;
                    NodeRecursionStatus<T> leftChildNodeRecursionStatus = new NodeRecursionStatus<T>(currentNodeRecursionStatus.Node.LeftChildNode);
                    recursionStack.Push(leftChildNodeRecursionStatus);
                }
                else if (currentNodeRecursionStatus.RightChildTreeProcessed == false && currentNodeRecursionStatus.Node.RightChildNode != null)
                {
                    currentNodeRecursionStatus.RightChildTreeProcessed = true;
                    NodeRecursionStatus<T> rightChildNodeRecursionStatus = new NodeRecursionStatus<T>(currentNodeRecursionStatus.Node.RightChildNode);
                    recursionStack.Push(rightChildNodeRecursionStatus);
                }
                else
                {
                    nodeAction.Invoke(currentNodeRecursionStatus.Node);
                    recursionStack.Pop();
                }
            }
        }

        /// <include file='InterfaceDocumentationComments.xml' path='doc/members/member[@name="M:MoreComplexDataStructures.IBinarySearchTree`1.BreadthFirstSearch(System.Action{MoreComplexDataStructures.WeightBalancedTreeNode{`0}})"]/*'/>
        public void BreadthFirstSearch(Action<WeightBalancedTreeNode<T>> nodeAction)
        {
            Queue<WeightBalancedTreeNode<T>> traversalQueue = new Queue<WeightBalancedTreeNode<T>>();
            if (rootNode != null)
            {
                traversalQueue.Enqueue(rootNode);
            }

            while (traversalQueue.Count > 0)
            {
                WeightBalancedTreeNode<T> currentNode = traversalQueue.Dequeue();
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
        /// Gets the node in the tree with the next value less than the value of the specified start node.
        /// </summary>
        /// <param name="item">The node to retrieve the next less of.</param>
        /// <returns>The node with the next value less, or null if no lower value exists.</returns>
        private WeightBalancedTreeNode<T> GetNextLessThan(WeightBalancedTreeNode<T> startNode)
        {
            T startNodeItem = startNode.Item;

            if (startNode.LeftChildNode != null)
            {
                // Move left once, and then move as far right as possible... resulting node contains the next lowest item
                WeightBalancedTreeNode<T> currentNode = startNode.LeftChildNode;
                while (currentNode.RightChildNode != null)
                {
                    currentNode = currentNode.RightChildNode;
                }
                return currentNode;
            }
            else
            {
                WeightBalancedTreeNode<T> returnNode = TraverseUpToNode(startNode, getNextLessThanTraverseUpAction);
                // TraverseUpToNode() will not move past the root node, so need to check whether the returned node is actually less than parameter item
                if (returnNode.Item.CompareTo(startNodeItem) < 0)
                {
                    return returnNode;
                }
                else
                {
                    // There are no lower items in the tree
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the node in the tree with the next value greater than the value of the specified start node.
        /// </summary>
        /// <param name="startNode">The node to retrieve the next greater of.</param>
        /// <returns>The node with the next value greater, or null if no greater value exists.</returns>
        private WeightBalancedTreeNode<T> GetNextGreaterThan(WeightBalancedTreeNode<T> startNode)
        {
            T startNodeItem = startNode.Item;

            if (startNode.RightChildNode != null)
            {
                // Move right once, and then move as far left as possible... resulting node contains the next greater item
                WeightBalancedTreeNode<T> currentNode = startNode.RightChildNode;
                while (currentNode.LeftChildNode != null)
                {
                    currentNode = currentNode.LeftChildNode;
                }
                return currentNode;
            }
            else
            {
                WeightBalancedTreeNode<T> returnNode = TraverseUpToNode(startNode, getNextGreaterThanTraverseUpAction);
                // TraverseUpToNode() will not move past the root node, so need to check whether the returned node is actually greater than parameter item
                if (returnNode.Item.CompareTo(startNodeItem) > 0)
                {
                    return returnNode;
                }
                else
                {
                    // There are no greater items in the tree
                    return null;
                }
            }
        }

        /// <summary>
        /// Traverses up the tree from the specified node to the root, invoking an action at each node.
        /// </summary>
        /// <param name="startNode">The node to start traversing at.</param>
        /// <param name="nodeAction">The action to perform at each node.  Accepts 2 parameters: the node to perform the action on, and a boolean indicating whether the current node was traversed to from the left or right (true if from the left, false if from the right, and null if the current node is the start node).</param>
        protected void TraverseUpFromNode(WeightBalancedTreeNode<T> startNode, Action<WeightBalancedTreeNode<T>, Nullable<Boolean>> nodeAction)
        {
            Nullable<Boolean> traversedFromLeft = null;
            WeightBalancedTreeNode<T> currentNode = startNode;

            while (currentNode != null)
            {
                nodeAction.Invoke(currentNode, traversedFromLeft);

                WeightBalancedTreeNode<T> parentNode = currentNode.ParentNode;
                if (parentNode != null)
                {
                    traversedFromLeft = IsLeftChildOf(currentNode, parentNode);
                }
                currentNode = parentNode;
            }
        }

        /// <summary>
        /// Traverses up the tree starting at the specified node, invoking the predicate function at each node to decide whether to continue traversing.  Returns the node traversal stopped at.
        /// </summary>
        /// <param name="startNode">The node to start traversing at.</param>
        /// <param name="traversePredicateFunc">A function used to decide whether traversal should continue.  Accepts 2 parameters: the node to perform the action on, and a boolean indicating whether the current node was traversed to from the left or right (true if from the left, false if from the right, and null if the current node is the start node).  Returns a boolean indicating whether traversal should continue.</param>
        /// <returns>The node of the tree where traversal stopped.</returns>
        protected WeightBalancedTreeNode<T> TraverseUpToNode(WeightBalancedTreeNode<T> startNode, Func<WeightBalancedTreeNode<T>, Nullable<Boolean>, Boolean> traversePredicateFunc)
        {
            Nullable<Boolean> traversedFromLeft = null;
            WeightBalancedTreeNode<T> currentNode = startNode;

            while (traversePredicateFunc.Invoke(currentNode, traversedFromLeft) == true)
            {
                traversedFromLeft = IsLeftChildOf(currentNode, currentNode.ParentNode);
                currentNode = currentNode.ParentNode;
            }

            return currentNode;
        }

        /// <summary>
        /// Traverses down the tree from the root searching for a node holding the specified item.
        /// </summary>
        /// <param name="item">The item to traverse to.</param>
        /// <returns>The node holding the specified item if it exists, otherwise null.</returns>
        protected WeightBalancedTreeNode<T> TraverseDownToNodeHoldingItem(T item)
        {
            WeightBalancedTreeNode<T> currentNode = rootNode;

            while (currentNode != null)
            {
                Int32 comparisonResult = currentNode.Item.CompareTo(item);
                if (comparisonResult == 0)
                {
                    break;
                }
                else if (comparisonResult < 0)
                {
                    // currentNode.Item < item so move to the right
                    currentNode = currentNode.RightChildNode;
                }
                else if (comparisonResult > 0)
                {
                    // currentNode.Item > item so move to the left
                    currentNode = currentNode.LeftChildNode;
                }
            }

            return currentNode;
        }

        /// <summary>
        /// Traverses down the tree from the root searching for a node holding the specified item, and invokes the specified action at each node during traversal.  If the item doesn't exist the parent of the position where the item should have been is returned.
        /// </summary>
        /// <param name="item">The item to attempt to traverse to.</param>
        /// <param name="nodeAction">The action to invoke at each node.</param>
        /// <returns>A tuple holding a boolean and a node.  The boolean indicates whether a node with the specified value was found (true if found, false otherwise).  If true the second item in the tuple contains the node holding the specified item.  If false, second item in the tuple contains the node which would be the parent of the item node, if it existed.  The second item in the tuple will be null if the tree is empty.</returns>
        /// <remarks>Used to find the next less than or next greater than.</remarks>
        protected Tuple<Boolean, WeightBalancedTreeNode<T>> TraverseDownToNodeHoldingItemOrParent(T item, Action<WeightBalancedTreeNode<T>> nodeAction)
        {
            WeightBalancedTreeNode<T> parentNode = null;
            WeightBalancedTreeNode<T> currentNode = rootNode;

            while (currentNode != null)
            {
                nodeAction.Invoke(currentNode);

                Int32 comparisonResult = currentNode.Item.CompareTo(item);
                if (comparisonResult == 0)
                {
                    break;
                }
                else if (comparisonResult < 0)
                {
                    // currentNode.Item < item so move to the right
                    parentNode = currentNode;
                    currentNode = currentNode.RightChildNode;
                }
                else if (comparisonResult > 0)
                {
                    // currentNode.Item > item so move to the left
                    parentNode = currentNode;
                    currentNode = currentNode.LeftChildNode;
                }
            }

            if (currentNode == null)
            {
                return new Tuple<Boolean, WeightBalancedTreeNode<T>>(false, parentNode);
            }
            else
            {
                return new Tuple<Boolean, WeightBalancedTreeNode<T>>(true, currentNode);
            }
        }

        /// <summary>
        /// Determines whether the specified child node is the left or right child of the specified parent node.
        /// </summary>
        /// <param name="childNode">The child node.</param>
        /// <param name="parentNode">The parent node.</param>
        /// <returns>Returns true if the child is the left child, and false is the child is the right child.</returns>
        /// <exception cref="System.Exception">The specified child node is not a child of the parent.</exception>
        protected Boolean IsLeftChildOf(WeightBalancedTreeNode<T> childNode, WeightBalancedTreeNode<T> parentNode)
        {
            if (parentNode.LeftChildNode != null && parentNode.LeftChildNode == childNode)
            {
                return true;
            }
            else if (parentNode.RightChildNode != null && parentNode.RightChildNode == childNode)
            {
                return false;
            }
            else
            {
                throw new Exception("Node containing item '" + childNode.Item.ToString() + "' is not a child of node containing item '" + parentNode.Item.ToString() + "'.");
            }
        }

        /// <summary>
        /// Removes the specified node from the tree, which has either no left nor right children, or just a single child (i.e. cannot handle removing nodes that have both left and right children).
        /// </summary>
        /// <param name="node">The node to remove.</param>
        /// <remarks>Will fail if the 'node' parameter is the root node.</remarks>
        protected void RemoveNode(WeightBalancedTreeNode<T> node)
        {
            // TODO: Could potentially remove this exception once functionality is solid
            if (node.LeftChildNode != null && node.RightChildNode != null)
            {
                throw new Exception("Node with item '" + node.Item.ToString() + "' has left and right children, and cannot be handled by this method.");
            }

            if (IsLeftChildOf(node, node.ParentNode) == true)
            {
                if (node.RightChildNode == null && node.LeftChildNode == null)
                {
                    node.ParentNode.LeftChildNode = null;
                }
                else if (node.RightChildNode != null)
                {
                    node.ParentNode.LeftChildNode = node.RightChildNode;
                    node.RightChildNode.ParentNode = node.ParentNode;
                }
                else
                {
                    node.ParentNode.LeftChildNode = node.LeftChildNode;
                    node.LeftChildNode.ParentNode = node.ParentNode;
                }
            }
            else
            {
                if (node.RightChildNode == null && node.LeftChildNode == null)
                {
                    node.ParentNode.RightChildNode = null;
                }
                else if (node.RightChildNode != null)
                {
                    node.ParentNode.RightChildNode = node.RightChildNode;
                    node.RightChildNode.ParentNode = node.ParentNode;
                }
                else 
                {
                    node.ParentNode.RightChildNode = node.LeftChildNode;
                    node.LeftChildNode.ParentNode = node.ParentNode;
                }
            }

            // Nullify all the references from the deleted node
            node.ParentNode = null;
            node.LeftChildNode = null;
            node.RightChildNode = null;
        }

        /// <summary>
        /// Retrieves the depth of the tree via a pre-order depth-first search.
        /// </summary>
        /// <returns>The depth of the tree.</returns>
        protected Int32 GetDepthViaDepthFirstSearch()
        {
            if (rootNode == null)
            {
                return -1;
            }

            Int32 depth = -1;
            Stack<NodeRecursionStatus<T>> recursionStack = new Stack<NodeRecursionStatus<T>>();

            recursionStack.Push(new NodeRecursionStatus<T>(rootNode));
            while (recursionStack.Count > 0)
            {
                if ((recursionStack.Count - 1) > depth) depth = (recursionStack.Count - 1);

                NodeRecursionStatus<T> currentNodeRecursionStatus = recursionStack.Peek();
                if (currentNodeRecursionStatus.LeftChildTreeProcessed == false && currentNodeRecursionStatus.Node.LeftChildNode != null)
                {
                    currentNodeRecursionStatus.LeftChildTreeProcessed = true;
                    NodeRecursionStatus<T> leftChildNodeRecursionStatus = new NodeRecursionStatus<T>(currentNodeRecursionStatus.Node.LeftChildNode);
                    recursionStack.Push(leftChildNodeRecursionStatus);
                }
                else if (currentNodeRecursionStatus.RightChildTreeProcessed == false && currentNodeRecursionStatus.Node.RightChildNode != null)
                {
                    currentNodeRecursionStatus.RightChildTreeProcessed = true;
                    NodeRecursionStatus<T> rightChildNodeRecursionStatus = new NodeRecursionStatus<T>(currentNodeRecursionStatus.Node.RightChildNode);
                    recursionStack.Push(rightChildNodeRecursionStatus);
                }
                else
                {
                    recursionStack.Pop();
                }
            }

            return depth;
        }
        
        # endregion

        # region Nested Classes

        /// <summary>
        /// Container class used when recursing through the tree for depth first search.
        /// </summary>
        /// <typeparam name="T">Specifies the type of item held by the node of the recursion status.</typeparam>
        private class NodeRecursionStatus<T> where T : IComparable<T>
        {
            private WeightBalancedTreeNode<T> node;
            private Boolean leftChildTreeProcessed;
            private Boolean rightChildTreeProcessed;
 
            /// <summary>
            /// The node to record the recursion status for.
            /// </summary>
            public WeightBalancedTreeNode<T> Node
            {
                get
                {
                    return node;
                }
            }

            /// <summary>
            /// Whether the left subtree of the node has been recursed.
            /// </summary>
            public Boolean LeftChildTreeProcessed
            {
                get
                {
                    return leftChildTreeProcessed;
                }
                set
                {
                    leftChildTreeProcessed = value;
                }
            }

            /// <summary>
            /// Whether the right subtree of the node has been recursed.
            /// </summary>
            public Boolean RightChildTreeProcessed
            {
                get
                {
                    return rightChildTreeProcessed;
                }
                set
                {
                    rightChildTreeProcessed = value;
                }
            }

            /// <summary>
            /// Initialises a new instance of the MoreComplexDataStructures.WeightBalancedTree+NodeRecursionStatus class.
            /// </summary>
            /// <param name="node"></param>
            public NodeRecursionStatus(WeightBalancedTreeNode<T> node)
            {
                this.node = node;
                leftChildTreeProcessed = false;
                rightChildTreeProcessed = false;
            }
        }

        # endregion
    }
}
