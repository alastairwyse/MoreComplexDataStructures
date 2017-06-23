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
using MoreComplexDataStructures;

namespace MoreComplexDataStructures.UnitTests
{
    /// <summary>
    /// Base class for unit tests for heap classes.
    /// </summary>
    public abstract class HeapTestBase
    {
        /// <summary>
        /// Performs a breadth-first search on the tree underlying the specied integer heap, returning all the items in the tree in breadth-first order.
        /// </summary>
        /// <param name="inputHeap">The heap to search.</param>
        /// <returns>A list containing the items in the tree in breadth-first order.</returns>
        protected List<Int32> GetListOfItems(HeapBase<Int32> inputHeap)
        {
            List<Int32> returnList = new List<Int32>();
            Action<DoublyLinkedTreeNode<Int32>> nodeAction = (node) =>
            {
                returnList.Add(node.Item);
            };
            inputHeap.BreadthFirstSearch(nodeAction);
            return returnList;
        }

        /// <summary>
        /// Performs a breadth-first search on the tree underlying the specied integer heap, returning all the nodes of the tree in a Dictionary.
        /// </summary>
        /// <param name="inputHeap">The heap to search.</param>
        /// <returns>A Dictionary keyed by the integer item of each node, and containing all the nodes in the tree.</returns>
        protected Dictionary<Int32, DoublyLinkedTreeNode<Int32>> GetAllNodes(HeapBase<Int32> inputHeap)
        {
            Dictionary<Int32, DoublyLinkedTreeNode<Int32>> returnDictionary = new Dictionary<Int32, DoublyLinkedTreeNode<Int32>>();
            Action<DoublyLinkedTreeNode<Int32>> nodeAction = (node) =>
            {
                returnDictionary.Add(node.Item, node);
            };
            inputHeap.BreadthFirstSearch(nodeAction);
            return returnDictionary;
        }
    }
}
