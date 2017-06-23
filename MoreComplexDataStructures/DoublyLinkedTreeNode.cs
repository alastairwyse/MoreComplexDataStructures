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
    /// An individual node of a tree which includes a reference to the parent node.
    /// </summary>
    /// <typeparam name="T">Specifies the type of item held by the node.</typeparam>
    public class DoublyLinkedTreeNode<T> : TreeNodeBase<T> where T : IComparable<T>
    {
        private DoublyLinkedTreeNode<T> parentNode;
        private DoublyLinkedTreeNode<T> leftChildNode;
        private DoublyLinkedTreeNode<T> rightChildNode;

        /// <summary>
        /// The parent node of this node.
        /// </summary>
        public DoublyLinkedTreeNode<T> ParentNode
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
        /// The left child node of this node.
        /// </summary>
        public DoublyLinkedTreeNode<T> LeftChildNode
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
        public DoublyLinkedTreeNode<T> RightChildNode
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
        /// Initialises a new instance of the MoreComplexDataStructures.DoublyLinkedTreeNode class.
        /// </summary>
        /// <param name="item">The item held by the node.</param>
        /// <param name="parentNode">The parent node of this node.</param>
        public DoublyLinkedTreeNode(T item, DoublyLinkedTreeNode<T> parentNode)
            : base(item)
        {
            this.parentNode = parentNode;
            leftChildNode = null;
            rightChildNode = null;
        }
    }
}
