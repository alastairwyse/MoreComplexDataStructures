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
    /// Inserts an array of elements into an IBinarySearchTree, ensuring that the resulting tree is balanced, and the depth is minimized.
    /// </summary>
    public class BinarySearchTreeBalancedInserter
    {
        /// <summary>
        /// Initialises a new instance of the MoreComplexDataStructures.BinarySearchTreeBalancedInserter class.
        /// </summary>
        public BinarySearchTreeBalancedInserter()
        {
        }

        /// <summary>
        /// Clears any existing items from the specified tree, and then inserts the array of elements, ensuring that the tree is balanced and the depth minimized.
        /// </summary>
        /// <typeparam name="T">Specifies the type of items held by nodes of the tree.</typeparam>
        /// <param name="tree">The tree to add the elements to.</param>
        /// <param name="elements">The array of elements to add to the tree.</param>
        /// <exception cref="System.ArgumentException">The specified array of elements is empty.</exception>
        /// <exception cref="System.Exception">Error encountered when adding item to the tree.</exception>
        public void Insert<T>(IBinarySearchTree<T> tree, T[] elements) where T : IComparable<T>
        {
            if (elements.Length == 0)
            {
                throw new ArgumentException("The specified array of elements is empty.", "elements");
            }

            tree.Clear();
            Array.Sort(elements);
            InsertRecurse(tree, elements, 0, elements.Length - 1);
        }

        # region Private/Protected Methods

        /// <summary>
        /// Recursively inserts a porition of the specified array of elements into the tree.
        /// </summary>
        /// <typeparam name="T">Specifies the type of items held by nodes of the tree.</typeparam>
        /// <param name="tree">The tree to add the elements to.</param>
        /// <param name="elements">The array of elements to add to the tree.</param>
        /// <param name="startIndex">The index of the first element of the portion of the array to insert.</param>
        /// <param name="endIndex">The index of the last element of the portion of the array to insert.</param>
        /// <exception cref="System.Exception">Error encountered when adding item to the tree.</exception>
        protected void InsertRecurse<T>(IBinarySearchTree<T> tree, T[] elements, Int32 startIndex, Int32 endIndex) where T : IComparable<T>
        {
            Int32 portionLength = endIndex - startIndex + 1;
            Int32 midPointIndex = (portionLength / 2) + startIndex;

            try
            {
                tree.Add(elements[midPointIndex]);
            }
            catch (Exception e)
            {
                throw new Exception("Error encountered when adding item '" + elements[midPointIndex].ToString() + "' to the tree.", e);
            }

            if (startIndex < midPointIndex)
            {
                InsertRecurse(tree, elements, startIndex, midPointIndex - 1);
            }
            if (midPointIndex < endIndex)
            {
                InsertRecurse(tree, elements, midPointIndex + 1, endIndex);
            }
        }

        #endregion
    }
}
