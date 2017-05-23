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
    /// An individual node of a weight-balanced tree.  Includes a reference to the parent node, and the size of the left and right subtrees.
    /// </summary>
    /// <typeparam name="T">Specifies the type of item held by the node.</typeparam>
    public class WeightBalancedTreeNode<T> where T : IComparable<T>
    {
        private WeightBalancedTreeNode<T> parentNode;
        private T item;
        private WeightBalancedTreeNode<T> leftChildNode;
        private WeightBalancedTreeNode<T> rightChildNode;
        private Int32 leftSubtreeSize;
        private Int32 rightSubtreeSize;

        /// <summary>
        /// The parent node of this node.
        /// </summary>
        public WeightBalancedTreeNode<T> ParentNode
        {
            get
            {
                return parentNode;
            }
            set
            {
                parentNode = value;
            }
        }

        /// <summary>
        /// The item held by the node.
        /// </summary>
        public T Item
        {
            get
            {
                return item;
            }
            set
            {
                item = value;
            }
        }

        /// <summary>
        /// The left child node of this node.
        /// </summary>
        public WeightBalancedTreeNode<T> LeftChildNode
        {
            get
            {
                return leftChildNode;
            }
            set
            {
                leftChildNode = value;
            }
        }

        /// <summary>
        /// The right child node of this node.
        /// </summary>
        public WeightBalancedTreeNode<T> RightChildNode
        {
            get
            {
                return rightChildNode;
            }
            set
            {
                rightChildNode = value;
            }
        }

        /// <summary>
        /// The number of nodes in the left subtree of this node.
        /// </summary>
        public Int32 LeftSubtreeSize
        {
            get
            {
                return leftSubtreeSize;
            }
            set
            {
                leftSubtreeSize = value;
            }
        }

        /// <summary>
        /// The number of nodes in the right subtree of this node.
        /// </summary>
        public Int32 RightSubtreeSize
        {
            get
            {
                return rightSubtreeSize;
            }
            set
            {
                rightSubtreeSize = value;
            }
        }

        /// <summary>
        /// Initialises a new instance of the MoreComplexDataStructures.WeightBalancedTreeNode class.
        /// </summary>
        /// <param name="item">The item held by the node.</param>
        /// <param name="parentNode">The parent node of this node.</param>
        public WeightBalancedTreeNode(T item, WeightBalancedTreeNode<T> parentNode)
        {
            this.parentNode = parentNode;
            this.item = item;
            leftChildNode = null;
            rightChildNode = null;
            leftSubtreeSize = 0;
            rightSubtreeSize = 0;
        }
    }
}
