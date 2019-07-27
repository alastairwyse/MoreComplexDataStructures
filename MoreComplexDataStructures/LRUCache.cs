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
    /// A simple least recently used cache which identifies value items by a unique key.
    /// </summary>
    /// <typeparam name="TKey">Specifies the type of key items.</typeparam>
    /// <typeparam name="TValue">Specifies the type of value items.</typeparam>
    public class LRUCache<TKey, TValue> where TKey : IEquatable<TKey>
    {
        /// <summary>A Dictionary storing the cached keys and values.</summary>
        protected Dictionary<TKey, Tuple<TValue, LinkedListNode<TKey>>> cacheStore;
        /// <summary>Maintains the order of use of items in the cache, to identify which should be expired.</summary>
        protected LinkedList<TKey> expiryQueue;
        /// <summary>A function which retrieves value items from a backing store, identified by a key.</summary>
        protected Func<TKey, TValue> backingStoreRequest;
        /// <summary>A function which accepts the cache store dictionary as a parameter, and decides whether the cache is full or not, returing a boolean to indicate (true = full).</summary>
        protected Func<Dictionary<TKey, Tuple<TValue, LinkedListNode<TKey>>>, Boolean> cacheFullCheckRoutine;

        /// <summary>
        /// Gets the number of items stored in the cache.
        /// </summary>
        public Int32 Count
        {
            get
            {
                return cacheStore.Count;
            }
        }

        /// <summary>
        /// Initialises a new instance of the MoreComplexDataStructures.LRUCache class.
        /// </summary>
        /// <param name="backingStoreRequest">A function which retrieves a value from the backing store of the cache, identified by the specified key.</param>
        /// <param name="itemLimit">The maximum number if items to store in the cache.</param>
        public LRUCache(Func<TKey, TValue> backingStoreRequest, Int32 itemLimit)
        {
            if (itemLimit < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(itemLimit), $"Parameter '{nameof(itemLimit)}' must be greater than or equal to 1.");
            }

            cacheStore = new Dictionary<TKey, Tuple<TValue, LinkedListNode<TKey>>>();
            expiryQueue = new LinkedList<TKey>();
            this.backingStoreRequest = backingStoreRequest;
            cacheFullCheckRoutine = (inputCacheStore) =>
            {
                if (inputCacheStore.Count == itemLimit)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            };
        }

        /// <summary>
        /// Initialises a new instance of the MoreComplexDataStructures.LRUCache class.  Note this is an additional constructor to facilitate unit tests, and should not be used to instantiate the class for normal use.
        /// </summary>
        /// <param name="backingStoreRequest">A function which retrieves a value from the backing store of the cache, identified by the specified key.</param>
        /// <param name="itemLimit">The maximum number if items to store in the cache.</param>
        /// <param name="expiryQueue">Populated with a reference to the expiry queue insode the cache.</param>
        public LRUCache(Func<TKey, TValue> backingStoreRequest, Int32 itemLimit, out LinkedList<TKey> expiryQueue)
            : this(backingStoreRequest, itemLimit)
        {
            expiryQueue = this.expiryQueue;
        }

        /// <summary>
        /// Initialises a new instance of the MoreComplexDataStructures.LRUCache class.
        /// </summary>
        /// <param name="backingStoreRequest">A function which retrieves a value from the backing store of the cache, identified by the specified key.</param>
        /// <param name="cacheFullCheckRoutine">A function which accepts the cache store dictionary as a parameter, and decides whether the cache is full or not, returing a boolean to indicate (true = full).</param>
        public LRUCache(Func<TKey, TValue> backingStoreRequest, Func<Dictionary<TKey, Tuple<TValue, LinkedListNode<TKey>>>, Boolean> cacheFullCheckRoutine)
            : this(backingStoreRequest, 1)
        {
            this.cacheFullCheckRoutine = cacheFullCheckRoutine;
        }

        /// <summary>
        /// Initialises a new instance of the MoreComplexDataStructures.LRUCache class.  Note this is an additional constructor to facilitate unit tests, and should not be used to instantiate the class for normal use.
        /// </summary>
        /// <param name="backingStoreRequest">A function which retrieves a value from the backing store of the cache, identified by the specified key.</param>
        /// <param name="cacheFullCheckRoutine">A function which accepts the cache store dictionary as a parameter, and decides whether the cache is full or not, returing a boolean to indicate (true = full).</param>
        /// <param name="expiryQueue">Populated with a reference to the expiry queue insode the cache.</param>
        public LRUCache(Func<TKey, TValue> backingStoreRequest, Func<Dictionary<TKey, Tuple<TValue, LinkedListNode<TKey>>>, Boolean> cacheFullCheckRoutine, out LinkedList<TKey> expiryQueue)
            : this(backingStoreRequest, cacheFullCheckRoutine)
        {
            expiryQueue = this.expiryQueue;
        }

        /// <summary>
        /// Retrieves an item from the cache, or the backing store if the item does not exist in the cache.
        /// </summary>
        /// <param name="key">The key of the item to retrieve.</param>
        /// <returns>The item identified by the specified key.</returns>
        public TValue Read(TKey key)
        {
            if (cacheStore.ContainsKey(key))
            {
                // Move the node for the item to the head of the list.
                LinkedListNode<TKey> itemListCode = cacheStore[key].Item2;
                if (itemListCode.Previous != null)
                {
                    expiryQueue.Remove(itemListCode);
                    expiryQueue.AddFirst(itemListCode);
                }
                return cacheStore[key].Item1;
            }
            else
            {
                TValue newItem = backingStoreRequest.Invoke(key);
                if (cacheFullCheckRoutine.Invoke(cacheStore) == true)
                {
                    // Cache is full, so remove the least recently used item
                    cacheStore.Remove(expiryQueue.Last.Value);
                    expiryQueue.RemoveLast();
                }

                // Add the new item
                expiryQueue.AddFirst(key);
                cacheStore.Add(key, new Tuple<TValue, LinkedListNode<TKey>>(newItem, expiryQueue.First));

                return newItem;
            }
        }

        /// <summary>
        /// Returns true if the cache contains an item with the specified key.
        /// </summary>
        /// <param name="key">The key of the item to check for.</param>
        /// <returns>True if an item with the specified key exists in the cache, otherwise false.</returns>
        public Boolean ContainsKey(TKey key)
        {
            return cacheStore.ContainsKey(key);
        }

        /// <summary>
        /// Expires the item from the cache with the specified key.
        /// </summary>
        /// <param name="key">The key of the item to expire.</param>
        public void Expire(TKey key)
        {
            if (cacheStore.ContainsKey(key) == false)
            {
                throw new ArgumentException($"An item with the specified key '{key.ToString()}' does not exist in the cache.", nameof(key));
            }
            else
            {
                LinkedListNode<TKey> queueItemToExpire = cacheStore[key].Item2;
                expiryQueue.Remove(queueItemToExpire);
                cacheStore.Remove(key);
            }
        }
    }
}
