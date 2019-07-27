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
    /// An individual node of a trie.  Includes a reference to the parent node, and the size of each subtree.
    /// </summary>
    /// <typeparam name="T">Specifies the type of item held by the node.</typeparam>
    public class TrieNode<T>
    {
        /// <summary>The children of this node.</summary>
        protected Dictionary<T, TrieNode<T>> childNodes;
        /// <summary>The number of nodes in each subtree of this node.</summary>
        protected Dictionary<T, Int32> subtreeSizes;

        /// <summary>
        /// The item held by the node.
        /// </summary>
        public T Item
        {
            get;
            set;
        }

        /// <summary>
        /// The parent node of this node.
        /// </summary>
        public TrieNode<T> ParentNode
        {
            get;
            set;
        }

        /// <summary>
        /// The children of this node.
        /// </summary>
        public IEnumerable<TrieNode<T>> ChildNodes
        {
            get
            {
                return childNodes.Values;
            }
        }

        /// <summary>
        /// Initialises a new instance of the MoreComplexDataStructures.TrieNode class.
        /// </summary>
        /// <param name="item">The item held by the node.</param>
        /// <param name="parentNode">The parent node of this node.</param>
        public TrieNode(T item, TrieNode<T> parentNode)
        {
            childNodes = new Dictionary<T, TrieNode<T>>();
            subtreeSizes = new Dictionary<T, Int32>();
            Item = item;
            ParentNode = parentNode;
        }

        /// <summary>
        /// Checks whether a child node for the specified item exists.
        /// </summary>
        /// <param name="item">The item corresponding to the child node.</param>
        /// <returns>True if a child node for the specified item exists.  False otherwise.</returns>
        public Boolean ChildNodeExists(T item)
        {
            return childNodes.ContainsKey(item);
        }

        /// <summary>
        /// Returns the child node for the specified item.
        /// </summary>
        /// <param name="item">The item corresponding to the child node.</param>
        /// <returns>The child node.</returns>
        /// <exception cref="System.ArgumentException">A child node for the specified item does not exist.</exception>
        public TrieNode<T> GetChildNode(T item)
        {
            ThrowExceptionIfChildNodeDoesntExist(item);

            return childNodes[item];
        }

        /// <summary>
        /// Adds a child node to the current node, incrementing the subtree size for the specified item.
        /// </summary>
        /// <param name="item">The item corresponding to the child node.</param>
        /// <param name="childNode">The child node.</param>
        /// <exception cref="System.ArgumentException">A child node for the specified item already exists.</exception>
        public void AddChildNode(T item, TrieNode<T> childNode)
        {
            if (childNodes.ContainsKey(item) == true)
            {
                throw new ArgumentException($"A child node for the specified item '{item.ToString()}' already exists.", nameof(item));
            }

            childNode.ParentNode = this;
            childNodes.Add(item, childNode);
            subtreeSizes.Add(item, 1);
        }

        /// <summary>
        /// Removes the child node for the specified item from the current node.
        /// </summary>
        /// <param name="item">The item to remove the child node for.</param>
        public void RemoveChildNode(T item)
        {
            ThrowExceptionIfChildNodeDoesntExist(item);

            childNodes.Remove(item);
            subtreeSizes.Remove(item);
        }

        /// <summary>
        /// Removes all child nodes from the current node.
        /// </summary>
        public void RemoveAllChildren()
        {
            childNodes.Clear();
            subtreeSizes.Clear();
        }

        /// <summary>
        /// Increments the subtree size for the specified item.
        /// </summary>
        /// <param name="item">The item to increment the subtree size for.</param>
        /// <exception cref="System.ArgumentException">A child node for the specified item does not exist.</exception>
        public void IncrementSubtreeSize(T item)
        {
            ThrowExceptionIfChildNodeDoesntExist(item);

            subtreeSizes[item]++;
        }

        /// <summary>
        /// Decrements the subtree size for the specified item.
        /// </summary>
        /// <param name="item">The item to decrement the subtree size for.</param>
        /// <exception cref="System.ArgumentException">A child node for the specified item does not exist.</exception>
        public void DecrementSubtreeSize(T item)
        {
            ThrowExceptionIfChildNodeDoesntExist(item);

            subtreeSizes[item]--;
        }

        /// <summary>
        /// Returns the subtree size for the specified item.
        /// </summary>
        /// <param name="item">The item to get the subtree size for.</param>
        /// <returns>The subtree size.</returns>
        /// <exception cref="System.ArgumentException">A child node for the specified item does not exist.</exception>
        public Int32 GetSubtreeSize(T item)
        {
            ThrowExceptionIfChildNodeDoesntExist(item);

            return subtreeSizes[item];
        }

        /// <summary>
        /// Sets the subtree size for the specified item.
        /// </summary>
        /// <param name="item">The item to set the subtree size for.</param>
        /// <param name="subTreeSize">The subtree size.</param>
        public void SetSubTreeSize(T item, Int32 subTreeSize)
        {
            ThrowExceptionIfChildNodeDoesntExist(item);

            subtreeSizes[item] = subTreeSize;
        }

        /// <summary>
        /// Creates a replica of the current node (i.e. a copy of the current node where the parent and child nodes hold the same item, but are not the same parent and child nodes as those of the current node).
        /// </summary>
        /// <returns>A replicated copy of the current node.</returns>
        /// <remarks>This method is primarily included for unit tests where visibility of the internal trie structure is required, but without the risk of clients being able to modify the internal structure.</remarks>
        public TrieNode<T> Replicate()
        {
            TrieNode<T> replicaNode;
            if (this.ParentNode != null)
            {
                replicaNode = new TrieNode<T>(this.Item, new TrieNode<T>(this.ParentNode.Item, null));
            }
            else
            {
                replicaNode = new TrieNode<T>(this.Item, null);
            }
            foreach (T currentChildNodeItem in childNodes.Keys)
            {
                replicaNode.AddChildNode(currentChildNodeItem, new TrieNode<T>(currentChildNodeItem, replicaNode));
                replicaNode.SetSubTreeSize(currentChildNodeItem, this.subtreeSizes[currentChildNodeItem]);
            }

            return replicaNode;
        }

        /// <summary>
        /// Creates a clone of the current node as a sequence terminator node.
        /// </summary>
        /// <returns>The current node cloned as a sequence terminator.</returns>
        /// <remarks>This method is primarily included for the case where a sequence is added to the trie which is a sub-sequence of an existing sequence (e.g. in the case of adding 'judge' when 'judgement' already exists).  In such a case the last item in the new sequence being added needs to be changed from a TrieNode to a SequenceTerminatorTrieNode.</remarks>
        public SequenceTerminatorTrieNode<T> CloneAsSequenceTerminator()
        {
            SequenceTerminatorTrieNode<T> clonedNode = new SequenceTerminatorTrieNode<T>(this.Item, this.ParentNode);
            foreach (KeyValuePair<T, TrieNode<T>> currentChildNode in childNodes)
            {
                clonedNode.AddChildNode(currentChildNode.Key, currentChildNode.Value);
                clonedNode.SetSubTreeSize(currentChildNode.Key, this.subtreeSizes[currentChildNode.Key]);
            }

            return clonedNode;
        }

        # region Private/Protected Methods

        /// <summary>
        /// Throws a System.ArgumentException if a child node for the specified item doesn't exist.
        /// </summary>
        /// <param name="item">The item to check the child node for.</param>
        protected void ThrowExceptionIfChildNodeDoesntExist(T item)
        {
            if (childNodes.ContainsKey(item) == false)
            {
                throw new ArgumentException($"A child node for the specified item '{item.ToString()}' does not exist.", nameof(item));
            }
        }

        #endregion
    }
}
