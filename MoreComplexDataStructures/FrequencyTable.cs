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
    /// Stores the frequency of occurrence of objects.
    /// </summary>
    /// <typeparam name="T">Specifies the type of items to store the frequency of occurrence of.</typeparam>
    public class FrequencyTable<T> : IEnumerable<KeyValuePair<T, Int32>>
    {
        /// <summary>Dictionary with the value storing the frequency of occurrence.</summary>
        protected Dictionary<T, Int32> frequencyStore;

        /// <summary>
        /// Returns the total unique items which have a frequency greater than 0.
        /// </summary>
        public Int32 ItemCount
        {
            get
            {
                return frequencyStore.Keys.Count;
            }
        }

        /// <summary>
        /// Returns the total frequency count for all items.
        /// </summary>
        public Int32 FrequencyCount
        {
            get
            {
                Int32 frequencyCount = 0;
                foreach(Int32 currentFrequency in frequencyStore.Values)
                {
                    frequencyCount += currentFrequency;
                }

                return frequencyCount;
            }
        }

        /// <summary>
        /// Initialises a new instance of the MoreComplexDataStructures.FrequencyTable class.
        /// </summary>
        public FrequencyTable()
        {
            frequencyStore = new Dictionary<T, Int32>();
        }

        /// <summary>
        /// Initialises a new instance of the MoreComplexDataStructures.FrequencyTable class.
        /// </summary>
        /// <param name="itemsAndCounts">The collection of items and corresponding counts to be initialised in the new FrequencyTable.</param>
        public FrequencyTable(IEnumerable<KeyValuePair<T, Int32>> itemsAndCounts)
            : this()
        {
            foreach (KeyValuePair<T, Int32> currentItemsAndCount in itemsAndCounts)
            {
                IncrementBy(currentItemsAndCount.Key, currentItemsAndCount.Value);
            }
        }

        /// <summary>
        /// Resets all frequency counts to 0.
        /// </summary>
        public void Clear()
        {
            frequencyStore.Clear();
        }

        /// <summary>
        /// Increments the frequency by 1 for the specified item.
        /// </summary>
        /// <param name="item">The item to increment the frequency for.</param>
        public void Increment(T item)
        {
            if (frequencyStore.ContainsKey(item) == true)
            {
                frequencyStore[item]++;
            }
            else
            {
                frequencyStore.Add(item, 1);
            }
        }

        /// <summary>
        /// Decrements the frequency by 1 for the specified item.
        /// </summary>
        /// <param name="item">The item to decrement the frequency for.</param>
        /// <exception cref="System.ArgumentException">The frequency for the specified item is 0.</exception>
        public void Decrement(T item)
        {
            if (frequencyStore.ContainsKey(item) == false)
            {
                throw new ArgumentException($"The frequency for the specified item '{item.ToString()}' is 0.", nameof(item));
            }
            else if (frequencyStore[item] == 1)
            {
                frequencyStore.Remove(item);
            }
            else
            {
                frequencyStore[item]--;
            }
        }

        /// <summary>
        /// Increments the frequency of the specified item by the specified count.
        /// </summary>
        /// <param name="item">The item to increment the frequency for.</param>
        /// <param name="count">The count to increment the frequency by</param>
        /// <exception cref="System.ArgumentException">The value of the 'count' parameter must be greater than 0.</exception>
        public void IncrementBy(T item, Int32 count)
        {
            ThrowExceptionIfCountParameterLessThan1(count);

            if (frequencyStore.ContainsKey(item) == true)
            {
                frequencyStore[item] += count;
            }
            else
            {
                frequencyStore.Add(item, count);
            }
        }

        /// <summary>
        /// Decrements the frequency of the specified item by the specified count.
        /// </summary>
        /// <param name="item">The item to decrement the frequency for.</param>
        /// <param name="count">The count to decrement the frequency by</param>
        /// <exception cref="System.ArgumentException">The value of the 'count' parameter must be greater than 0.</exception>
        /// <exception cref="System.ArgumentException">The frequency for the specified item is 0.</exception>
        /// <exception cref="System.ArgumentException">The frequency for the specified item is less than the value of the 'count' parameter.</exception>
        public void DecrementBy(T item, Int32 count)
        {
            ThrowExceptionIfCountParameterLessThan1(count);

            if (frequencyStore.ContainsKey(item) == false)
            {
                throw new ArgumentException($"The frequency for the specified item '{item.ToString()}' is 0.", nameof(item));
            }
            else if (frequencyStore[item] < count)
            {
                throw new ArgumentException($"The frequency for the specified item '{item.ToString()}' ({frequencyStore[item]}) is less than the value of the '{nameof(count)}' parameter.", nameof(count));
            }
            else if (frequencyStore[item] == count)
            {
                frequencyStore.Remove(item);
            }
            else
            {
                frequencyStore[item] -= count;
            }
        }

        /// <summary>
        /// Returns the frequency for the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The frequency.</returns>
        public Int32 GetFrequency(T item)
        {
            if (frequencyStore.ContainsKey(item) == false)
            {
                return 0;
            }
            else
            {
                return frequencyStore[item];
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<KeyValuePair<T, Int32>> GetEnumerator()
        {
            return frequencyStore.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return frequencyStore.GetEnumerator();
        }

        # region Private/Protected Methods

        /// <summary>
        /// Throws an ArgumentException of the specified count parameter value is less than 1.
        /// </summary>
        /// <param name="count">The count parameter to check the value of.</param>
        protected void ThrowExceptionIfCountParameterLessThan1(Int32 count)
        {
            if (count < 1)
            {
                throw new ArgumentException($"The value of the '{nameof(count)}' parameter must be greater than 0.", nameof(count));
            }
        }

        #endregion
    }
}
