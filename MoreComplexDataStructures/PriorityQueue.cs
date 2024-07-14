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
using System.Collections;
using System.Collections.Generic;

namespace MoreComplexDataStructures
{
    /// <summary>
    /// A tree-based double-ended priority queue.
    /// </summary>
    /// <typeparam name="T">Specifies the type of items held in the queue.</typeparam>
    public class PriorityQueue<T> : IEnumerable<KeyValuePair<Double, T>> where T : IEquatable<T>
    {
        /// <summary>Tree holding all of the priorities and items in the priority queue.</summary>
        protected WeightBalancedTreeWithProtectedMethods<PriorityAndItems<T>> tree;
        /// <summary>Dictionary mapping the items in the priority queue to their corresponding priorities.</summary>
        /// <remarks>Dictionary value is a hashset of doubles to support having the same item inserted multiple times with different priorities.</remarks>
        protected Dictionary<T, HashSet<Double>> itemToPriorityMap;
        /// <summary>The total number of items in the priority queue.</summary>
        protected Int32 count;
        /// <summary>Utility class used to increment priority values for the EnqueueAsMax() and EnqueueAsMin() methods.</summary>
        protected DoubleBinaryIncrementer priorityIncrementer;

        /// <summary>
        /// The total number of items in the priority queue.
        /// </summary>
        public Int32 Count
        {
            get { return count; }
        }

        /// <summary>
        /// The maximum priority of items in the queue.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">The queue is empty.</exception>
        public Double MaxPriority
        {
            get
            {
                ThrowExceptionIfQueueIsEmpty();

                return tree.Max.Priority;
            }
        }

        /// <summary>
        /// The minimum priority of items in the queue.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">The queue is empty.</exception>
        public Double MinPriority
        {
            get
            {
                ThrowExceptionIfQueueIsEmpty();

                return tree.Min.Priority;
            }
        }

        /// <summary>
        /// Initialises a new instance of the MoreComplexDataStructures.PriorityQueue class.
        /// </summary>
        public PriorityQueue()
        {
            tree = new WeightBalancedTreeWithProtectedMethods<PriorityAndItems<T>>(true);
            itemToPriorityMap = new Dictionary<T, HashSet<Double>>();
            count = 0;
            priorityIncrementer = new DoubleBinaryIncrementer();
        }

        /// <summary>
        /// Initialises a new instance of the MoreComplexDataStructures.PriorityQueue class.
        /// </summary>
        /// <param name="initialPrioritiesAndItems">The collection of priorities and corresponding items to be enqueued in the new PriorityQueue.</param>
        public PriorityQueue(IEnumerable<KeyValuePair<Double, T>> initialPrioritiesAndItems)
            : this()
        {
            foreach (KeyValuePair<Double, T> currentPriorityAndItem in initialPrioritiesAndItems)
            {
                Enqueue(currentPriorityAndItem.Value, currentPriorityAndItem.Key);
            }
        }

        /// <summary>
        /// Returns the number of instances of the specified item in the priority queue.
        /// </summary>
        /// <param name="item">The item to return the count for.</param>
        /// <returns>The number of instances of the specified item in the queue.</returns>
        public Int32 GetItemCountByItem(T item)
        {
            if (itemToPriorityMap.ContainsKey(item) == false)
                return 0;
            else
            {
                Int32 returnCount = 0;
                foreach (Double currentPriority in itemToPriorityMap[item])
                {
                    var priorityAndItems = tree.Get(new PriorityAndItems<T>(currentPriority));
                    returnCount += priorityAndItems.Items[item];
                }

                return returnCount;
            }
        }

        /// <summary>
        /// Returns the number of items enqueued with the specified priority in the queue.
        /// </summary>
        /// <param name="priority">The priority to return the count for.</param>
        /// <returns>The number of items enqueued with the specified priority in the queue.</returns>
        public Int32 GetItemCountByPriority(Double priority)
        {
            WeightBalancedTreeNode<PriorityAndItems<T>> priorityNode = tree.TraverseDownToNodeHoldingItem(new PriorityAndItems<T>(priority));
            if (priorityNode == null)
                return 0;
            else
                return GetTotalItemCount(priorityNode.Item);
        }

        /// <summary>
        /// Removes all items from the priority queue.
        /// </summary>
        public void Clear()
        {
            tree.Clear();
            itemToPriorityMap.Clear();
            count = 0;
        }

        /// <summary>
        /// Stores an item in the queue with the specified priority.
        /// </summary>
        /// <param name="item">The item to enqueue.</param>
        /// <param name="priority">The priority of the item.</param>
        /// <exception cref="System.ArgumentException">The parameter 'priority' cannot be NaN.</exception>
        /// <exception cref="System.InvalidOperationException">The queue cannot hold more than Int32.MaxValue items.</exception>
        public void Enqueue(T item, Double priority)
        {
            if (Double.IsNaN(priority))
                throw new ArgumentException($"Parameter '{nameof(priority)}' cannot be '{nameof(Double.NaN)}'.", nameof(priority));
            if (count == Int32.MaxValue)
                throw new InvalidOperationException($"The queue cannot hold greater than {Int32.MaxValue} items.");

            // Add the item to the tree
            var newNode = new PriorityAndItems<T>(priority);
            WeightBalancedTreeNode<PriorityAndItems<T>> priorityNode = tree.TraverseDownToNodeHoldingItem(newNode);
            if (priorityNode == null)
            {
                newNode.Items.Add(item, 1);
                tree.Add(newNode);
            }
            else
            {
                if (priorityNode.Item.Items.ContainsKey(item))
                    priorityNode.Item.Items[item]++;
                else
                    priorityNode.Item.Items.Add(item, 1);
            }
            // Add the item to the item to priority map
            if (itemToPriorityMap.ContainsKey(item))
            {
                if (itemToPriorityMap[item].Contains(priority) == false)
                    itemToPriorityMap[item].Add(priority);
            }
            else
            {
                itemToPriorityMap.Add(item, new HashSet<Double>() { priority });
            }
            count++;
        }

        /// <summary>
        /// Stores an item in the queue with maximum priority (i.e. higher than all existing items).
        /// </summary>
        /// <param name="item">The item to enqueue.</param>
        public void EnqueueAsMax(T item)
        {
            if (count == 0)
            {
                Enqueue(item, 0.0);
            }
            else
            {
                PriorityAndItems<T> maxNode = tree.Max;
                Enqueue(item, priorityIncrementer.Increment(maxNode.Priority));
            }
        }

        /// <summary>
        /// Stores an item in the queue with minimum priority (i.e. lower than all existing items).
        /// </summary>
        /// <param name="item">The item to enqueue.</param>
        public void EnqueueAsMin(T item)
        {
            if (count == 0)
            {
                Enqueue(item, 0.0);
            }
            else
            {
                PriorityAndItems<T> minNode = tree.Min;
                Enqueue(item, priorityIncrementer.Decrement(minNode.Priority));
            }
        }

        /// <summary>
        /// Removes the specified item from the queue.
        /// </summary>
        /// <returns>The item to remove.</returns>
        /// <exception cref="System.ArgumentException">The item does not exist in the queue.</exception>
        /// <exception cref="System.InvalidOperationException">Multiple instances of the specified item exist in the queue with different priorities.</exception>
        public void Dequeue(T item)
        {
            ThrowExceptionIfItemDoesntExistInQueue(item);
            if (itemToPriorityMap[item].Count > 1)
                throw new InvalidOperationException($"Multiple instances of item '{item.ToString()}' exist in the queue.");

            Double priority = GetFirstItemFromHashSet(itemToPriorityMap[item]);
            var priorityAndItems = tree.Get(new PriorityAndItems<T>(priority));
            Remove(priorityAndItems, item);
        }

        /// <summary>
        /// Removes the item with the specified priority from the queue.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <param name="priority">The priority of the item to remove.</param>
        /// <remarks>Can be used to remove a specific item when multiple instances of the item exist in the queue with different priorities.</remarks>
        /// <exception cref="System.ArgumentException">An item with the specified priority does not exist in the queue.</exception>
        public void Dequeue(T item, Double priority)
        {
            if (itemToPriorityMap.ContainsKey(item) == false)
                throw new ArgumentException($"Item '{item.ToString()}' with priority {priority} does not exist in the queue.", nameof(item));

            var priorityAndItems = tree.Get(new PriorityAndItems<T>(priority));
            Remove(priorityAndItems, item);
        }

        /// <summary>
        /// Removes and returns the item with the highest priority.
        /// </summary>
        /// <returns>The item with the highest priority.</returns>
        /// <exception cref="System.InvalidOperationException">The queue is empty.</exception>
        public T DequeueMax()
        {
            ThrowExceptionIfQueueIsEmpty();

            PriorityAndItems<T> maxPriorityAndItems = tree.Max;
            KeyValuePair<T, Int32> firstItemAndCount = GetFirstKeyValuePairFromDictionary(maxPriorityAndItems.Items);
            Dequeue(firstItemAndCount.Key, maxPriorityAndItems.Priority);

            return firstItemAndCount.Key;
        }

        /// <summary>
        /// Removes and returns the item with the lowest priority.
        /// </summary>
        /// <returns>The item with the lowest priority.</returns>
        /// <exception cref="System.InvalidOperationException">The queue is empty.</exception>
        public T DequeueMin()
        {
            ThrowExceptionIfQueueIsEmpty();

            PriorityAndItems<T> minPriorityAndItems = tree.Min;
            KeyValuePair<T, Int32> firstItemAndCount = GetFirstKeyValuePairFromDictionary(minPriorityAndItems.Items);
            Dequeue(firstItemAndCount.Key, minPriorityAndItems.Priority);

            return firstItemAndCount.Key;
        }

        /// <summary>
        /// Returns the item with the highest priority without removing it.
        /// </summary>
        /// <returns>The item with the highest priority.</returns>
        /// <exception cref="System.InvalidOperationException">The queue is empty.</exception>
        /// <remarks>If multiple items exist with the maximum priority, PeekMax() may return any of those items, and in such a case it is not guaranteed to repeated calls to PeekMax() will return the same item.</remarks>
        public T PeekMax()
        {
            ThrowExceptionIfQueueIsEmpty();

            return GetFirstKeyValuePairFromDictionary(tree.Max.Items).Key;
        }

        /// <summary>
        /// Returns the item with the lowest priority without removing it.
        /// </summary>
        /// <returns>The item with the lowest priority.</returns>
        /// <exception cref="System.InvalidOperationException">The queue is empty.</exception>
        /// <remarks>>If multiple items exist with the minimum priority, PeekMin() may return any of those items, and in such a case it is not guaranteed to repeated calls to PeekMin() will return the same item.</remarks>
        public T PeekMin()
        {
            ThrowExceptionIfQueueIsEmpty();

            return GetFirstKeyValuePairFromDictionary(tree.Min.Items).Key;
        }

        /// <summary>
        /// Checks whether the specified item exists in the queue.
        /// </summary>
        /// <param name="item">The item to check for.</param>
        /// <returns>True if the item exists in the queue.  False otherwise.</returns>
        public Boolean ContainsItem(T item)
        {
            if (itemToPriorityMap.ContainsKey(item))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks whether an item with the specified priority exists in the queue.
        /// </summary>
        /// <param name="priority">The priority to check for.</param>
        /// <returns>True if the an item with the specified priority exists in the queue.  False otherwise.</returns>
        public Boolean ContainsPriority(Double priority)
        {
            WeightBalancedTreeNode<PriorityAndItems<T>> priorityNode = tree.TraverseDownToNodeHoldingItem(new PriorityAndItems<T>(priority));
            if (priorityNode == null)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Get the priorities assigned to the specified item.
        /// </summary>
        /// <param name="item">The item to retrieve the priorities for.</param>
        /// <returns>The priorities for the item.</returns>
        /// <exception cref="System.ArgumentException">The item does not exist in the queue.</exception>
        /// <remarks>As the priority queue can hold multiple instances of the same item with different priorities, this method can return multiple priority values.  However if multiple instances of an item exist each with the same priority, that priority will only be returned once.</remarks>
        public IEnumerable<Double> GetPrioritiesForItem(T item)
        {
            ThrowExceptionIfItemDoesntExistInQueue(item);

            return itemToPriorityMap[item];
        }

        /// <summary>
        /// Returns an enumerator containing items in the queue and their associated priority, with priority greater than the specified priority.
        /// </summary>
        /// <param name="priority">The priority to retrieve all priorities greater than.</param>
        /// <param name="itemCount">The maximum number of items to return.</param>
        /// <returns>An enumerator containing all items in the queue and their associated priority, with priority greater than the specified priority.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameter 'itemCount' is less than 1.</exception>
        public IEnumerable<KeyValuePair<Double, T>> GetItemsWithPriorityGreaterThan(Double priority, Int32 itemCount)
        {
            ThrowExceptionIfItemCountIsLessThan1(itemCount);

            foreach (PriorityAndItems<T> currentTreeNode in tree.GetAllGreaterThan(new PriorityAndItems<T>(priority)))
            {
                foreach (KeyValuePair<T, Int32> currentItemAndCount in currentTreeNode.Items)
                {
                    for (Int32 i = 0; i < currentItemAndCount.Value; i++)
                    {
                        if (itemCount > 0)
                        {
                            yield return new KeyValuePair<Double, T>(currentTreeNode.Priority, currentItemAndCount.Key);
                            itemCount--;
                        }
                        else
                        {
                            yield break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns an enumerator containing items in the queue and their associated priority, with priority less than the specified priority.
        /// </summary>
        /// <param name="priority">The priority to retrieve all priorities less than.</param>
        /// <param name="itemCount">The maximum number of items to return.</param>
        /// <returns>An enumerator containing all items in the queue and their associated priority, with priority less than the specified priority.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Parameter 'itemCount' is less than 1.</exception>
        public IEnumerable<KeyValuePair<Double, T>> GetItemsWithPriorityLessThan(Double priority, Int32 itemCount)
        {
            ThrowExceptionIfItemCountIsLessThan1(itemCount);

            foreach (PriorityAndItems<T> currentTreeNode in tree.GetAllLessThan(new PriorityAndItems<T>(priority)))
            {
                foreach (KeyValuePair<T, Int32> currentItemAndCount in currentTreeNode.Items)
                {
                    for (Int32 i = 0; i < currentItemAndCount.Value; i++)
                    {
                        if (itemCount > 0)
                        {
                            yield return new KeyValuePair<Double, T>(currentTreeNode.Priority, currentItemAndCount.Key);
                            itemCount--;
                        }
                        else
                        {
                            yield break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns an enumerator containing all items in the queue and their associated priority, with priority greater than the specified priority.
        /// </summary>
        /// <param name="priority">The priority to retrieve all priorities greater than.</param>
        /// <returns>An enumerator containing all items in the queue and their associated priority, with priority greater than the specified priority.</returns>
        public IEnumerable<KeyValuePair<Double, T>> GetAllWithPriorityGreaterThan(Double priority)
        {
            return GetItemsWithPriorityGreaterThan(priority, Int32.MaxValue);
        }

        /// <summary>
        /// Returns an enumerator containing all items in the queue and their associated priority, with priority less than the specified priority.
        /// </summary>
        /// <param name="priority">The priority to retrieve all priorities less than.</param>
        /// <returns>An enumerator containing all items in the queue and their associated priority, with priority less than the specified priority.</returns>
        public IEnumerable<KeyValuePair<Double, T>> GetAllWithPriorityLessThan(Double priority)
        {
            return GetItemsWithPriorityLessThan(priority, Int32.MaxValue);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a <see cref="PriorityQueue{T}"/>.
        /// </summary>
        /// <returns>An enumerator containing all items in the queue and their associated priority.</returns>
        public IEnumerator<KeyValuePair<Double, T>> GetEnumerator()
        {
            if (Count == 0)
            {
                yield break;
            }
            else
            {
                foreach (PriorityAndItems<T> currentPriorityAndItems in tree)
                {
                    foreach (KeyValuePair<T, Int32> currentPriorityAndItem in currentPriorityAndItems.Items)
                    {
                        for (Int32 i = 0; i < currentPriorityAndItem.Value; i++)
                        {
                            yield return new KeyValuePair<Double, T>(currentPriorityAndItems.Priority, currentPriorityAndItem.Key);
                        }
                    }
                }
            }
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #region Private/Protected Methods

        /// <summary>
        /// Removes the specified item from the PriorityAndItems object, and removes that object from the tree and the item to priority map, if the specified item is the last in the object.
        /// </summary>
        /// <param name="priorityAndItems">The object to remove the item from.</param>
        /// <param name="item">The item to remove.</param>
        protected void Remove(PriorityAndItems<T> priorityAndItems, T item)
        {
            Boolean removeFromMap = true;

            // Remove the item from the tree
            if (priorityAndItems.Items.Count == 1 && priorityAndItems.Items[item] == 1)
            {
                tree.Remove(priorityAndItems);
            }
            else
            {
                if (priorityAndItems.Items[item] == 1)
                {
                    priorityAndItems.Items.Remove(item);
                }
                else
                {
                    priorityAndItems.Items[item]--;
                    removeFromMap = false;
                }
            }
            // Remove the item from the item to priority map
            if (removeFromMap == true)
            {
                itemToPriorityMap[item].Remove(priorityAndItems.Priority);
                if (itemToPriorityMap[item].Count == 0)
                    itemToPriorityMap.Remove(item);
            }
            count--;
        }

        /// <summary>
        /// Returns the first double value in the specified HashSet of doubles.
        /// </summary>
        /// <param name="inputHashSet">The hashset to return the value from.</param>
        /// <returns>The first double element in the hashset.</returns>
        protected Double GetFirstItemFromHashSet(HashSet<Double> inputHashSet)
        {
            foreach (Double currentElement in inputHashSet)
                return currentElement;

            throw new ArgumentException($"The HashSet specified in parameter '{nameof(inputHashSet)}' contains no elements.", nameof(inputHashSet));
        }
        
        /// <summary>
        /// Returns the first key/value pair in the specified Dictionary with Int32 value type.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <param name="inputDictionary">The dictionary to return the key/value pair from.</param>
        /// <returns>The first key/value pair in the Dictionary.</returns>
        protected KeyValuePair<TKey, Int32> GetFirstKeyValuePairFromDictionary<TKey>(Dictionary<TKey, Int32> inputDictionary)
        {
            foreach (KeyValuePair<TKey, Int32> currentKvp in inputDictionary)
                return currentKvp;

            throw new ArgumentException($"The Dictionary specified in parameter '{nameof(inputDictionary)}' contains no elements.", nameof(inputDictionary));
        }

        /// <summary>
        /// Returns the total number of items in the specified PriorityAndItems object.
        /// </summary>
        /// <param name="priorityAndItems">The PriorityAndItems object to retrieve the total for.</param>
        /// <returns>The total number of items in the PriorityAndItems object.</returns>
        protected Int32 GetTotalItemCount(PriorityAndItems<T> priorityAndItems)
        {
            Int32 returnCount = 0;
            foreach (Int32 currentItemCount in priorityAndItems.Items.Values)
                returnCount += currentItemCount;

            return returnCount;
        }

        /// <summary>
        /// Throws an InvalidOperationException if the priority queue is empty.
        /// </summary>
        protected void ThrowExceptionIfQueueIsEmpty()
        {
            if (count == 0)
                throw new InvalidOperationException("The priority queue is empty.");
        }

        /// <summary>
        /// Throws an ArgumentException if the specified item doesn't exist in the queue.
        /// </summary>
        /// <param name="item">The item.</param>
        protected void ThrowExceptionIfItemDoesntExistInQueue(T item)
        {
            if (itemToPriorityMap.ContainsKey(item) == false)
                throw new ArgumentException($"Item '{item.ToString()}' does not exist in the priority queue.", nameof(item));
        }

        /// <summary>
        /// Throws an ArgumentOutOfRangeException if the specified item count is less than 1.
        /// </summary>
        /// <param name="itemCount">The item count.</param>
        protected void ThrowExceptionIfItemCountIsLessThan1(Int32 itemCount)
        {
            if (itemCount < 1)
                throw new ArgumentOutOfRangeException(nameof(itemCount), $"Parameter '{nameof(itemCount)}' must be greater than or equal to 1.");
        }

        #endregion

        #region Nested Classes

        #pragma warning disable 0693

        /// <summary>
        /// The container class held as the item in the WeightBalancedTree which implements the priority queue.  Holds the priority and the items that have that priority in the queue.
        /// </summary>
        /// <typeparam name="T">The type of items held in the priority queue.</typeparam>
        protected class PriorityAndItems<T> : IComparable<PriorityAndItems<T>> where T : IEquatable<T>
        {
            private Double priority;
            private readonly Dictionary<T, Int32> items;

            /// <summary>
            /// The priority of the items.
            /// </summary>
            public Double Priority
            {
                get { return priority; }
            }

            /// <summary>
            /// The items, and the number (count) of each item.
            /// </summary>
            public Dictionary<T, Int32> Items
            {
                get { return items; }
            }

            /// <summary>
            /// Initialises a new instance of the MoreComplexDataStructures.PriorityQueue+PriorityAndItems class.
            /// </summary>
            /// <param name="priority">The priority of the items.</param>
            public PriorityAndItems(Double priority)
            {
                this.priority = priority;
                items = new Dictionary<T, Int32>();
            }

            #pragma warning disable 1591
            public Int32 CompareTo(PriorityAndItems<T> other)
            {
                return this.priority.CompareTo(other.priority);
            }
            #pragma warning restore 1591
        }

        /// <summary>
        /// A version of the WeightBalancedTree class where private and protected methods are exposed as public.
        /// </summary>
        /// <typeparam name="T">Specifies the type of items held by nodes of the tree.</typeparam>
        protected class WeightBalancedTreeWithProtectedMethods<T> : WeightBalancedTree<T> where T : IComparable<T>
        {
            /// <summary>
            /// Initialises a new instance of the MoreComplexDataStructures.PriorityQueue+WeightBalancedTreeWithProtectedMethods class.
            /// </summary>
            /// <param name="maintainBalance">Determines whether balance of the tree should be maintained when adding or removing items.</param>
            public WeightBalancedTreeWithProtectedMethods(Boolean maintainBalance)
                : base(maintainBalance)
            {
            }

            /// <summary>
            /// Traverses down the tree from the root searching for a node holding the specified item.
            /// </summary>
            /// <param name="item">The item to traverse to.</param>
            /// <returns>The node holding the specified item if it exists, otherwise null.</returns>
            public new WeightBalancedTreeNode<T> TraverseDownToNodeHoldingItem(T item)
            {
                return base.TraverseDownToNodeHoldingItem(item);
            }
        }

        #pragma warning restore 0693

        #endregion
    }
}
