/*
 * Copyright 2024 Alastair Wyse (https://github.com/alastairwyse/MoreComplexDataStructures/)
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
    /// Extension of a <see cref="WeightBalancedTree{T}"/> which holds both a key and a value in each node, but compares only on the key.
    /// </summary>
    /// <typeparam name="TKey">The type of the key held at each node of the tree.</typeparam>
    /// <typeparam name="TValue">The type of the value held at each node of the tree.</typeparam>
    public class WeightBalancedTreeMap<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>> where TKey : IComparable<TKey>
    {
        /// <summary>
        /// The total number of items stored in the tree.
        /// </summary>
        public Int32 Count
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// The depth of nodes in the tree (0-based).
        /// </summary>
        /// <remarks>If the tree is not balanced, and the Remove() method has not been called on the tree since initialising or clearing, the depth is maintained in an internal variable and can be returned with O(1) time complexity.  In a balanced tree, and/or where the Remove() method has been called, the depth is found via a depth first search and hence consumes O(n) time complexity (where 'n' is the number of items currently held by the tree).</remarks>
        public Int32 Depth
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Retrieves the <see cref="KeyValuePair{TKey, TValue}"/> with minimum-valued key in the tree.
        /// </summary>
        public KeyValuePair<TKey, TValue> Min
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Retrieves the <see cref="KeyValuePair{TKey, TValue}"/> with maximum-valued key in the tree.
        /// </summary>
        public KeyValuePair<TKey, TValue> Max
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Removes all items from the tree.
        /// </summary>
        public void Clear()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds the specified key and value to the tree.
        /// </summary>
        /// <param name="key">The key to add.</param>
        /// <param name="value">The value to add.</param>
        /// <exception cref="ArgumentException">The specified key already exists in the tree.</exception>
        public void Add(TKey key, TValue value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes the item with the specified key from the tree.
        /// </summary>
        /// <param name="key">The key of the items to remove.</param>
        /// <exception cref="ArgumentException">The specified key does not exist in the tree.</exception>
        public void Remove(TKey key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieves the item with the specified key from the tree.
        /// </summary>
        /// <param name="key">The key of the item to retrieve.</param>
        /// <returns>The item.</returns>
        /// <exception cref="ArgumentException">The specified key does not exist in the tree.</exception>
        public KeyValuePair<TKey, TValue> Get(TKey key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines whether the tree contains an item with the specified key..
        /// </summary>
        /// <param name="key">The key of the item to locate in the tree.</param>
        /// <returns>True if the tree contains the specified item, otherwise false.</returns>
        public Boolean Contains(TKey key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the next item in the tree with key less than the specified key.
        /// </summary>
        /// <param name="key">The key of the item to retrieve the next less of.</param>
        /// <returns>A tuple containing 2 values: a boolean indicatig whether a lower value was found (false if no lower value exists), and the next item less than the specified item (or null / type default if no lower item exists).</returns>
        public Tuple<Boolean, KeyValuePair<TKey, TValue>> GetNextLessThan(TKey key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the next item in the tree with key greater than the specified key.
        /// </summary>
        /// <param name="key">The key of the item to retrieve the next greater of.</param>
        /// <returns>A tuple containing 2 values: a boolean indicating whether a greater value was found (false if no greater value exists), and the next item greater than the specified item (or null / type default if no greater item exists).</returns>
        public Tuple<Boolean, KeyValuePair<TKey, TValue>> GetNextGreaterThan(TKey key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a count of the number of items in the tree with keys less than the specified key.
        /// </summary>
        /// <param name="key">The key of the item to retrieve the count of items less than.</param>
        /// <returns>The number of items in the tree with key less than the specified key.</returns>
        public Int32 GetCountLessThan(TKey key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a count of the number of items in the tree with keys greater than the specified key.
        /// </summary>
        /// <param name="key">The key of the item to retrieve the count of items greater than.</param>
        /// <returns>The number of items in the tree with key greater than the specified key.</returns>
        public Int32 GetCountGreaterThan(TKey key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns an enumerator containing all items in the tree with keys less than the specified key.
        /// </summary>
        /// <param name="key">The key of the item to retrieve all items less than.</param>
        /// <returns>An enumerator containing all items in the tree with key less than the specified key.</returns>
        public IEnumerable<KeyValuePair<TKey, TValue>> GetAllLessThan(TKey key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns an enumerator containing all items in the tree with keys greater than the specified key.
        /// </summary>
        /// <param name="key">The key of the item to retrieve all items greater than.</param>
        /// <returns>An enumerator containing all items in the tree with key greater than the specified key.</returns>
        public IEnumerable<KeyValuePair<TKey, TValue>> GetAllGreaterThan(TKey key)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns an enumerator containing all items in the tree with keys between between the specified two keys, in ascending order.
        /// </summary>
        /// <param name="lowKey">The lower of the two keys.</param>
        /// <param name="highKey">The greater of the two keys.</param>
        /// <returns>An enumerator containing all items in the tree with keys between the specified two keys, in ascending order.</returns>
        public IEnumerable<KeyValuePair<TKey, TValue>> GetAllBetweenAscending(TKey lowKey, TKey highKey)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns an enumerator containing all items in the tree with keys between between the specified two keys, in descending order.
        /// </summary>
        /// <param name="highKey">The greater of the two keys.</param>
        /// <param name="lowKey">The lower of the two keys.</param>
        /// <returns>An enumerator containing all items in the tree with keys between the specified two keys, in descending order.</returns>
        public IEnumerable<KeyValuePair<TKey, TValue>> GetAllBetweenDescending(TKey highKey, TKey lowKey)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a random item from the tree.
        /// </summary>
        /// <returns>A random item.</returns>
        /// <exception cref="Exception">The tree is empty.</exception>
        public KeyValuePair<TKey, TValue> GetRandomItem()
        {
            throw new NotImplementedException();
        }

        // TODO: To I want to include traverse methods like PreOrderDepthFirstSearch()
        //   Maybe, if they worTKey with a protected 'ComparableKeyValuePair' as envisaged
        // Java Methods
        //   containsValue()

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #region Nested Classes

        protected class ComparableKeyValuePair<TKey, TValue> : KeyValuePair<TKey, TValue>
        {
            // Create TreeMapNode??
        }

        #endregion
    }
}
