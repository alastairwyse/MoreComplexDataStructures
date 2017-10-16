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
    /// Defines methods for a binary search tree which does not allow duplicate values.
    /// </summary>
    /// <typeparam name="T">Specifies the type of items held by nodes of the tree.</typeparam>
    public interface IBinarySearchTree<T> where T : IComparable<T>
    {
        /// <include file='InterfaceDocumentationComments.xml' path='doc/members/member[@name="P:MoreComplexDataStructures.IBinarySearchTree`1.Count"]/*'/>
        Int32 Count
        {
            get;
        }

        /// <include file='InterfaceDocumentationComments.xml' path='doc/members/member[@name="M:MoreComplexDataStructures.IBinarySearchTree`1.Clear"]/*'/>
        void Clear();

        /// <include file='InterfaceDocumentationComments.xml' path='doc/members/member[@name="M:MoreComplexDataStructures.IBinarySearchTree`1.Add(`0)"]/*'/>
        void Add(T item);

        /// <include file='InterfaceDocumentationComments.xml' path='doc/members/member[@name="M:MoreComplexDataStructures.IBinarySearchTree`1.Remove(`0)"]/*'/>
        void Remove(T item);

        /// <include file='InterfaceDocumentationComments.xml' path='doc/members/member[@name="M:MoreComplexDataStructures.IBinarySearchTree`1.Contains(`0)"]/*'/>
        Boolean Contains(T item);

        /// <include file='InterfaceDocumentationComments.xml' path='doc/members/member[@name="M:MoreComplexDataStructures.IBinarySearchTree`1.PreOrderDepthFirstSearch(System.Action{MoreComplexDataStructures.WeightBalancedTreeNode{`0}})"]/*'/>
        void PreOrderDepthFirstSearch(Action<WeightBalancedTreeNode<T>> nodeAction);

        /// <include file='InterfaceDocumentationComments.xml' path='doc/members/member[@name="M:MoreComplexDataStructures.IBinarySearchTree`1.InOrderDepthFirstSearch(System.Action{MoreComplexDataStructures.WeightBalancedTreeNode{`0}})"]/*'/>
        void InOrderDepthFirstSearch(Action<WeightBalancedTreeNode<T>> nodeAction);

        /// <include file='InterfaceDocumentationComments.xml' path='doc/members/member[@name="M:MoreComplexDataStructures.IBinarySearchTree`1.PostOrderDepthFirstSearch(System.Action{MoreComplexDataStructures.WeightBalancedTreeNode{`0}})"]/*'/>
        void PostOrderDepthFirstSearch(Action<WeightBalancedTreeNode<T>> nodeAction);

        /// <include file='InterfaceDocumentationComments.xml' path='doc/members/member[@name="M:MoreComplexDataStructures.IBinarySearchTree`1.BreadthFirstSearch(System.Action{MoreComplexDataStructures.WeightBalancedTreeNode{`0}})"]/*'/>
        void BreadthFirstSearch(Action<WeightBalancedTreeNode<T>> nodeAction);
    }
}
