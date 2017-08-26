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
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoreComplexDataStructures
{
    /// <summary>
    /// Provides a method to randomize the elements of a list with O(n) time complexity using the Fisher/Yates/Knuth algorithm.
    /// </summary>
    public class ListRandomizer
    {
        /// <summary>The random number generator to use for selecting indices from the source collection.</summary>
        protected IRandomIntegerGenerator randomGenerator;

        /// <summary>
        /// Initialises a new instance of the MoreComplexDataStructures.ListRandomizer class.
        /// </summary>
        public ListRandomizer()
        {
            randomGenerator = new DefaultRandomGenerator();
        }

        /// <summary>
        /// Initialises a new instance of the MoreComplexDataStructures.ListRandomizer class.
        /// </summary>
        /// <param name="randomIntegerGenerator">An implementation of interface IRandomIntegerGenerator to use for selecting indices from the source collection.</param>
        public ListRandomizer(IRandomIntegerGenerator randomIntegerGenerator)
            : this()
        {
            randomGenerator = randomIntegerGenerator;
        }

        /// <summary>
        /// Randomizes the elements of the specified collection implementing interface IList&lt;T&gt;.
        /// </summary>
        /// <param name="inputCollection">The IList to randomize the elements of.</param>
        /// <exception cref="System.ArgumentException">The 'inputCollection' parameter contains a list exceeding the maximum supported size (Int32.MaxValue - 1 elements).</exception>
        public void Randomize(IList inputCollection)
        {
            // Need to throw an exception of the inputted collection contains Int32.MaxValue elements.
            //   In this case we can never randomly select the last value in the list (as the IRandomIntegerGenerator.Next() 'maxValue' parameter is exclusive).
            if (inputCollection.Count == Int32.MaxValue)
            {
                throw new ArgumentException("The maximum collection size supported by the ListRandomizer is " + (Int32.MaxValue - 1) + ".", "inputCollection");
            }

            Int32 randomizedPortionDividerIndex = inputCollection.Count;
            while (randomizedPortionDividerIndex > 0)
            {
                // Generate the index of the next element to move to the randomized portion of the list
                Int32 nextIndex = randomGenerator.Next(randomizedPortionDividerIndex);
                // Move the element to the randomized portion and swap with the existing object
                Object tempObject = inputCollection[nextIndex];
                inputCollection[nextIndex] = inputCollection[randomizedPortionDividerIndex - 1];
                inputCollection[randomizedPortionDividerIndex - 1] = tempObject;
                randomizedPortionDividerIndex--;
            }
        }
    }
}
