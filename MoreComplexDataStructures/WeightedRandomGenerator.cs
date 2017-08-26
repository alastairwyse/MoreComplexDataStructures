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
    /// Returns items randomly based on configured weightings.
    /// </summary>
    /// <typeparam name="T">Specifies the type of items generated.</typeparam>
    public class WeightedRandomGenerator<T>
    {
        /// <summary>The random number generator to use for selecting random items.</summary>
        protected IRandomIntegerGenerator randomGenerator;
        /// <summary>Holds ranges of integers which correspond to the random weightings.</summary>
        protected WeightBalancedTree<LongIntegerRange> weightingRanges;
        /// <summary>Maps items to weightings.</summary>
        protected Dictionary<T, LongIntegerRange> itemToWeightingMap;
        /// <summary>Maps weightings to items.</summary>
        protected Dictionary<LongIntegerRange, T> weightingToItemMap;
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
                return weightingRanges.Count;
            }
        }

        /// <summary>
        /// Initialises a new instance of the MoreComplexDataStructures.WeightedRandomGenerator class.
        /// </summary>
        public WeightedRandomGenerator()
        {
            randomGenerator = new DefaultRandomGenerator();
            weightingRanges = new WeightBalancedTree<LongIntegerRange>();
            itemToWeightingMap = new Dictionary<T, LongIntegerRange>();
            weightingToItemMap = new Dictionary<LongIntegerRange, T>();
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
            weightingRanges.Clear();
            itemToWeightingMap.Clear();
            weightingToItemMap.Clear();
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
                throw new ArgumentException("The specified set of weightings is empty.", "weightings");
            }

            Clear();
            List<Tuple<T, LongIntegerRange>> weightingsAsRanges = new List<Tuple<T, LongIntegerRange>>(weightings.Count);
            Int64 nextRangeStartValue = 0;
            foreach (Tuple<T, Int64> currentWeighting in weightings)
            {
                if ((Int64.MaxValue - 1) - nextRangeStartValue < (currentWeighting.Item2 - 1))
                {
                    throw new ArgumentException("The total of the specified weightings cannot exceed Int64.MaxValue.", "weightings");
                }

                LongIntegerRange currentRange = new LongIntegerRange(nextRangeStartValue, currentWeighting.Item2);
                weightingsAsRanges.Add(new Tuple<T, LongIntegerRange>(currentWeighting.Item1, currentRange));
                weightingsTotal += (currentWeighting.Item2);
                nextRangeStartValue += currentWeighting.Item2;
            }
            // TODO: Currently randomizing the weightings before inserting into the WeightBalancedTree as the tree does not auto-balance.
            //         When auto-balancing is implemented this can be removed.
            ListRandomizer listRandomizer = new ListRandomizer(randomGenerator);
            listRandomizer.Randomize(weightingsAsRanges);
            foreach (Tuple<T, LongIntegerRange> currentWeighting in weightingsAsRanges)
            {
                if (itemToWeightingMap.ContainsKey(currentWeighting.Item1) == true)
                {
                    throw new ArgumentException("The specified weightings contain duplicate item with value = '" + currentWeighting.Item1.ToString() + "'.", "weightings");
                }

                weightingRanges.Add(currentWeighting.Item2);
                itemToWeightingMap.Add(currentWeighting.Item1, currentWeighting.Item2);
                weightingToItemMap.Add(currentWeighting.Item2, currentWeighting.Item1);
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
                throw new ArgumentException("A weighting for the specified item does not exist.", "item");
            }

            LongIntegerRange rangeToRemove = itemToWeightingMap[item];
            Int32 numberOfLowerRanges = weightingRanges.GetCountLessThan(rangeToRemove);
            Int32 numberOfHigherRanges = weightingRanges.GetCountGreaterThan(rangeToRemove);

            // Remove the weighting corresponding to the specified item
            weightingRanges.Remove(rangeToRemove);
            itemToWeightingMap.Remove(item);
            weightingToItemMap.Remove(rangeToRemove);

            // TODO: Could potentially refactor each half of if statement since most codes lines are the same... abstract GetNextLessThan() and GetNextGreaterThan() behind lambdas (and also have a lambda negating rangeToRemove.Length for -= and += operators)
            if (numberOfLowerRanges < numberOfHigherRanges)
            {
                // Shuffle up the values of all the ranges lower than the one removed
                Tuple<Boolean, LongIntegerRange> nextLowerRange = weightingRanges.GetNextLessThan(rangeToRemove);
                while (nextLowerRange.Item1 == true)
                {
                    T currentRangeItem = weightingToItemMap[nextLowerRange.Item2];
                    weightingToItemMap.Remove(nextLowerRange.Item2);
                    nextLowerRange.Item2.StartValue += rangeToRemove.Length;
                    itemToWeightingMap[currentRangeItem] = nextLowerRange.Item2;
                    weightingToItemMap.Add(nextLowerRange.Item2, currentRangeItem);
                    nextLowerRange = weightingRanges.GetNextLessThan(nextLowerRange.Item2);
                }

                weightingStartOffset += rangeToRemove.Length;
            }
            else
            {
                // Shuffle down the values of all the ranges higher than the one removed
                Tuple<Boolean, LongIntegerRange> nextHigherRange = weightingRanges.GetNextGreaterThan(rangeToRemove);
                while (nextHigherRange.Item1 == true)
                {
                    T currentRangeItem = weightingToItemMap[nextHigherRange.Item2];
                    weightingToItemMap.Remove(nextHigherRange.Item2);
                    nextHigherRange.Item2.StartValue -= rangeToRemove.Length;
                    itemToWeightingMap[currentRangeItem] = nextHigherRange.Item2;
                    weightingToItemMap.Add(nextHigherRange.Item2, currentRangeItem);
                    nextHigherRange = weightingRanges.GetNextGreaterThan(nextHigherRange.Item2);
                }
            }

            weightingsTotal -= rangeToRemove.Length;

            if (weightingRanges.Count == 0)
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
            if (weightingRanges.Count == 0)
            {
                throw new InvalidOperationException("No weightings are defined.");
            }

            Int64 randomIndex = randomGenerator.Next(weightingsTotal) + weightingStartOffset;

            // Create an range based on the random index
            LongIntegerRange randomIndexRange = new LongIntegerRange(randomIndex, 1);

            // If a range starting with the random index doesn't exist, get the next lower range (which will include the random index)
            if (weightingRanges.Contains(randomIndexRange) == true)
            {
                if (randomIndex != Int64.MaxValue)
                {
                    randomIndexRange = weightingRanges.GetNextLessThan(new LongIntegerRange(randomIndex + 1, 1)).Item2;
                }
            }
            else
            {
                randomIndexRange = weightingRanges.GetNextLessThan(randomIndexRange).Item2;
            }

            return weightingToItemMap[randomIndexRange];
        }

        /// <summary>
        /// Performs a breadth-first search of the underlying tree, invoking the specified action on the start value and length of each range.
        /// </summary>
        /// <param name="rangeAction">The action to perform on each range.  Accepts a two parameters which are the start value and length of the current range.</param>
        public void TraverseTree(Action<Int64, Int64> rangeAction)
        {
            Action<WeightBalancedTreeNode<LongIntegerRange>> nodeAction = (node) =>
            {
                rangeAction.Invoke(node.Item.StartValue, node.Item.Length);
            };

            weightingRanges.BreadthFirstSearch(nodeAction);
        }
    }
}
