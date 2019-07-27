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
    /// Returns items randomly based on configured weightings.
    /// </summary>
    /// <typeparam name="T">Specifies the type of items generated.</typeparam>
    public class WeightedRandomGenerator<T>
    {
        /// <summary>The random number generator to use for selecting random items.</summary>
        protected IRandomIntegerGenerator randomGenerator;
        /// <summary>Holds ranges of integers which correspond to the random weightings and the item the weighting maps to.</summary>
        protected WeightBalancedTree<ItemAndWeighting<T>> weightingRangesAndItems;
        /// <summary>Maps items to weightings.</summary>
        protected Dictionary<T, LongIntegerRange> itemToWeightingMap;
        /// <summary>The maximum inclusive value of the combined weightings (i.e. if weightings are { 1, 2, 4 } weightingsTotal will be 7).</summary>
        protected Int64 weightingsTotal;
        /// <summary>The offset from 0 from which the weighting definitions start.</summary>
        protected Int64 weightingStartOffset;

        /// <summary>
        /// The total number of weightings (and items) in the generator.
        /// </summary>
        public Int32 WeightingCount
        {
            get
            {
                return weightingRangesAndItems.Count;
            }
        }

        /// <summary>
        /// Initialises a new instance of the MoreComplexDataStructures.WeightedRandomGenerator class.
        /// </summary>
        public WeightedRandomGenerator()
        {
            randomGenerator = new DefaultRandomGenerator();
            weightingRangesAndItems = new WeightBalancedTree<ItemAndWeighting<T>>();
            itemToWeightingMap = new Dictionary<T, LongIntegerRange>();
            weightingsTotal = 0;
            weightingStartOffset = 0;
        }

        /// <summary>
        /// Initialises a new instance of the MoreComplexDataStructures.ListRandomizer class.
        /// </summary>
        /// <param name="randomIntegerGenerator">An implementation of interface IRandomIntegerGenerator to use for selecting random items.</param>
        public WeightedRandomGenerator(IRandomIntegerGenerator randomIntegerGenerator)
            : this()
        {
            randomGenerator = randomIntegerGenerator;
        }

        /// <summary>
        /// Removes all registered items and weightings from the generator.
        /// </summary>
        public void Clear()
        {
            weightingRangesAndItems.Clear();
            itemToWeightingMap.Clear();
            weightingsTotal = 0;
            weightingStartOffset = 0;
        }

        /// <summary>
        /// Sets the random weightings, and the item corresponding to each weighting.
        /// </summary>
        /// <param name="weightings">The set of weightings defined by list of tuples containing 2 values: the item to attach to the weighting, and a weighting allocated to random selection of the item (i.e. an item with weight 2 will be twice as likely to be selected as an item with weight 1).</param>
        /// <exception cref="System.ArgumentException">The specified set of weightings is empty.</exception>
        /// <exception cref="System.ArgumentException">The total of the specified weightings exceeds Int64.MaxValue.</exception>
        /// <exception cref="System.ArgumentException">The specified weightings contain a duplicate item.</exception>
        public void SetWeightings(IList<Tuple<T, Int64>> weightings)
        {
            if (weightings.Count == 0)
            {
                throw new ArgumentException("The specified set of weightings is empty.", nameof(weightings));
            }

            Clear();
            var weightingsAsRanges = new ItemAndWeighting<T>[weightings.Count];
            Int64 nextRangeStartValue = 0;
            Int32 currentIndex = 0;
            foreach (Tuple<T, Int64> currentWeighting in weightings)
            {
                if ((Int64.MaxValue - 1) - nextRangeStartValue < (currentWeighting.Item2 - 1))
                {
                    throw new ArgumentException("The total of the specified weightings cannot exceed Int64.MaxValue.", nameof(weightings));
                }

                var currentRange = new LongIntegerRange(nextRangeStartValue, currentWeighting.Item2);
                weightingsAsRanges[currentIndex] = new ItemAndWeighting<T>(currentWeighting.Item1, currentRange);
                currentIndex++;
                weightingsTotal += (currentWeighting.Item2);
                nextRangeStartValue += currentWeighting.Item2;
            }
            foreach (ItemAndWeighting<T> currentItemAndWeighting in weightingsAsRanges)
            {
                if (itemToWeightingMap.ContainsKey(currentItemAndWeighting.Item) == true)
                {
                    throw new ArgumentException($"The specified weightings contain duplicate item with value = '{currentItemAndWeighting.Item.ToString()}'.", nameof(weightings));
                }

                weightingRangesAndItems.Add(currentItemAndWeighting);
                itemToWeightingMap.Add(currentItemAndWeighting.Item, currentItemAndWeighting.Weighting);
            }
        }

        /// <summary>
        /// Removes an item and its weighting from the generator.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <remarks>This method consumes O(n * log(n)) time complexity (where n is the number of weightings defined).</remarks>
        /// <exception cref="System.ArgumentException">A weighting for the specified item does not exist.</exception>
        public void RemoveWeighting(T item)
        {
            if (itemToWeightingMap.ContainsKey(item) == false)
            {
                throw new ArgumentException("A weighting for the specified item does not exist.", nameof(item));
            }

            var itemAndWeightingToRemove = new ItemAndWeighting<T>(item, itemToWeightingMap[item]);
            Int32 numberOfLowerRanges = weightingRangesAndItems.GetCountLessThan(itemAndWeightingToRemove);
            Int32 numberOfHigherRanges = weightingRangesAndItems.GetCountGreaterThan(itemAndWeightingToRemove);

            // Remove the weighting corresponding to the specified item
            weightingRangesAndItems.Remove(itemAndWeightingToRemove);
            itemToWeightingMap.Remove(item);

            // TODO: Could potentially refactor each half of if statement since most codes lines are the same... abstract GetNextLessThan() and GetNextGreaterThan() behind lambdas (and also have a lambda negating rangeToRemove.Length for -= and += operators)
            if (numberOfLowerRanges < numberOfHigherRanges)
            {
                // Shuffle up the values of all the ranges lower than the one removed
                Tuple<Boolean, ItemAndWeighting<T>> nextLowerRangeResult = weightingRangesAndItems.GetNextLessThan(itemAndWeightingToRemove);
                while (nextLowerRangeResult.Item1 == true)
                {
                    nextLowerRangeResult.Item2.Weighting.StartValue += itemAndWeightingToRemove.Weighting.Length;
                    itemToWeightingMap[nextLowerRangeResult.Item2.Item] = nextLowerRangeResult.Item2.Weighting;
                    nextLowerRangeResult = weightingRangesAndItems.GetNextLessThan(nextLowerRangeResult.Item2);
                }

                weightingStartOffset += itemAndWeightingToRemove.Weighting.Length;
            }
            else
            {
                // Shuffle down the values of all the ranges higher than the one removed
                Tuple<Boolean, ItemAndWeighting<T>> nextHigherRangeResult = weightingRangesAndItems.GetNextGreaterThan(itemAndWeightingToRemove);
                while (nextHigherRangeResult.Item1 == true)
                {
                    nextHigherRangeResult.Item2.Weighting.StartValue -= itemAndWeightingToRemove.Weighting.Length;
                    itemToWeightingMap[nextHigherRangeResult.Item2.Item] = nextHigherRangeResult.Item2.Weighting;
                    nextHigherRangeResult = weightingRangesAndItems.GetNextGreaterThan(nextHigherRangeResult.Item2);
                }
            }

            weightingsTotal -= itemAndWeightingToRemove.Weighting.Length;

            if (weightingRangesAndItems.Count == 0)
            {
                weightingStartOffset = 0;
            }
        }

        /// <summary>
        /// Returns an item randomly, based on the specified weightings.
        /// </summary>
        /// <returns>The item generated.</returns>
        /// <exception cref="System.InvalidOperationException">No weightings are defined.</exception>
        public T Generate()
        {
            if (weightingRangesAndItems.Count == 0)
            {
                throw new InvalidOperationException("No weightings are defined.");
            }

            Int64 randomIndex = randomGenerator.Next(weightingsTotal) + weightingStartOffset;

            // Create an item and weighting with the random index
            var randomItemAndWeighting = new ItemAndWeighting<T>(default(T), new LongIntegerRange(randomIndex, 1));

            if (weightingRangesAndItems.Contains(randomItemAndWeighting) == true)
            {
                // Overwrite 'randomItemAndWeighting' with the actual item and weighting from the tree (which will contain the correct item... not a default)
                randomItemAndWeighting = weightingRangesAndItems.Get(randomItemAndWeighting);
            }
            else
            {
                // If a range starting with the random index doesn't exist, get the next lower range (which will include the random index)
                randomItemAndWeighting = weightingRangesAndItems.GetNextLessThan(randomItemAndWeighting).Item2;
            }

            return randomItemAndWeighting.Item;
        }

        /// <summary>
        /// Performs a breadth-first search of the underlying tree, invoking the specified action on the start value and length of each range.
        /// </summary>
        /// <param name="rangeAction">The action to perform on each range.  Accepts a two parameters which are the start value and length of the current range.</param>
        public void TraverseTree(Action<Int64, Int64> rangeAction)
        {
            Action<WeightBalancedTreeNode<ItemAndWeighting<T>>> nodeAction = (node) =>
            {
                rangeAction.Invoke(node.Item.Weighting.StartValue, node.Item.Weighting.Length);
            };

            weightingRangesAndItems.BreadthFirstSearch(nodeAction);
        }

        #region Nested Classes

        #pragma warning disable 0693

        /// <summary>
        /// Container class holding an item attached to a weighting, and the weighing represented by in integer range.
        /// </summary>
        /// <typeparam name="T">Specifies the type of item attached to the weighting.</typeparam>
        /// <remarks>Note that the CompareTo() method considers only the weighting, so the class can be used in a binary search tree to order and search by weighting, and map to the corresponding item.</remarks>
        protected class ItemAndWeighting<T> : IComparable<ItemAndWeighting<T>>
        {
            /// <summary>The item attached to the weighting.</summary>
            protected T item;
            /// <summary>The weighting.</summary>
            protected LongIntegerRange weighting;

            /// <summary>The item attached to the weighting.</summary>
            public T Item
            {
                get { return item; }
            }

            /// <summary>The weighting.</summary>
            public LongIntegerRange Weighting
            {
                get { return weighting; }
            }

            /// <summary>
            /// Initialises a new instance of the MoreComplexDataStructures.WeightedRandomGenerator+ItemAndWeighting class.
            /// </summary>
            /// <param name="item">The item attached to the weighting.</param>
            /// <param name="weighting">The weighting.</param>
            public ItemAndWeighting(T item, LongIntegerRange weighting)
            {
                this.item = item;
                this.weighting = weighting;
            }

            #pragma warning disable 1591
            public Int32 CompareTo(ItemAndWeighting<T> other)
            {
                return this.weighting.CompareTo(other.weighting);
            }
            #pragma warning restore 1591
        }

        /// <summary>
        /// Wraps a List&lt;T&gt; object, and implements IBinarySearchTree&lt;T&gt; methods required by the BinarySearchTreeBalancedInserter.Insert() method.  The BinarySearchTreeBalancedInserter class cannot be used directly on the 'weightingRangesAndItems' member of the outer class, as each of the weightings needs to be checked for duplicates before insertion.  Hence this class is used in the outer class SetWeightings() method to first put the weightings into a list in an order which will result in a balanced tree.  Then the weightings can be inserted from the list into the tree with the required duplicate checking and exception handling.  Since it's expected that use-cases for this class will be 'construct once, use many', the performance trade-off of having to insert the weightings in a separate structure is acceptable.
        /// </summary>
        /// <typeparam name="T">Specifies the type of items held by the underlying list.</typeparam>
        protected class BinarySearchTreeListWrapper<T> : IBinarySearchTree<T> where T : IComparable<T>
        {
            /// <summary>The list wrapped by the class.</summary>
            protected List<T> wrappedList;

            /// <summary>
            /// Initialises a new instance of the MoreComplexDataStructures.WeightedRandomGenerator+BinarySearchTreeListWrapper class.
            /// </summary>
            /// <param name="wrappedList">The string wrapped by the class.</param>
            public BinarySearchTreeListWrapper(List<T> wrappedList)
            {
                this.wrappedList = wrappedList;
            }

            #region IBinarySearchTree<T> methods

            #pragma warning disable 1591
            public int Count
            {
                get { throw new NotImplementedException(); }
            }

            public T Min
            {
                get { throw new NotImplementedException(); }
            }

            public T Max
            {
                get { throw new NotImplementedException(); }
            }

            public void Clear()
            {
                wrappedList.Clear();
            }

            public void Add(T item)
            {
                wrappedList.Add(item);
            }

            public void Remove(T item)
            {
                throw new NotImplementedException();
            }

            public T Get(T item)
            {
                throw new NotImplementedException();
            }

            public bool Contains(T item)
            {
                throw new NotImplementedException();
            }

            public void PreOrderDepthFirstSearch(Action<WeightBalancedTreeNode<T>> nodeAction)
            {
                throw new NotImplementedException();
            }

            public void InOrderDepthFirstSearch(Action<WeightBalancedTreeNode<T>> nodeAction)
            {
                throw new NotImplementedException();
            }

            public void PostOrderDepthFirstSearch(Action<WeightBalancedTreeNode<T>> nodeAction)
            {
                throw new NotImplementedException();
            }

            public void BreadthFirstSearch(Action<WeightBalancedTreeNode<T>> nodeAction)
            {
                throw new NotImplementedException();
            }
            #pragma warning restore 1591

            #endregion
        }

        #pragma warning restore 0693

        #endregion
    }
}
