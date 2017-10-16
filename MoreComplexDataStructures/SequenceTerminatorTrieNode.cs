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
    /// An individual node of a trie, where the item held by the node is a terminator for a sequence stored in the trie.
    /// </summary>
    /// <typeparam name="T">Specifies the type of item held by the node.</typeparam>
    public class SequenceTerminatorTrieNode<T> : TrieNode<T> 
    {
        /// <summary>
        /// Initialises a new instance of the MoreComplexDataStructures.SequenceTerminatorTrieNode class.
        /// </summary>
        /// <param name="item">The item held by the node.</param>
        /// <param name="parentNode">The parent node of this node.</param>
        public SequenceTerminatorTrieNode(T item, TrieNode<T> parentNode)
            : base(item, parentNode)
        {
        }

        /// <summary>
        /// Creates a replica of the current node (i.e. a copy of the current node where the parent and child nodes hold the same item, but are not the same parent and child nodes as those of the current node).
        /// </summary>
        /// <returns>A replicated copy of the current node.</returns>
        /// <remarks>This method is primarily included for unit tests where visibility of the internal trie structure is required, but without the risk of clients being able to modify the internal structure.</remarks>
        public new SequenceTerminatorTrieNode<T> Replicate()
        {
            SequenceTerminatorTrieNode<T> replicaNode;
            if (this.ParentNode != null)
            {
                replicaNode = new SequenceTerminatorTrieNode<T>(this.Item, new SequenceTerminatorTrieNode<T>(this.ParentNode.Item, null));
            }
            else
            {
                replicaNode = new SequenceTerminatorTrieNode<T>(this.Item, null);
            }
            foreach (T currentChildNodeItem in childNodes.Keys)
            {
                replicaNode.AddChildNode(currentChildNodeItem, new TrieNode<T>(currentChildNodeItem, replicaNode));
                replicaNode.SetSubTreeSize(currentChildNodeItem, this.subtreeSizes[currentChildNodeItem]);
            }

            return replicaNode;
        }

        /// <summary>
        /// Creates a clone of the current sequence terminator node as standard trie node.
        /// </summary>
        /// <returns>The current node cloned as a standard trie node.</returns>
        /// <remarks>This method is primarily included for the case where a sequence is removed from a trie which is a sub-sequence of an existing sequence (e.g. in the case of removing 'judge' when 'judgement' already exists).  In such a case the last item in the sequence being removed needs to be changed from a SequenceTerminatorTrieNode to a TrieNode.</remarks>
        public TrieNode<T> CloneAsTrieNode()
        {
            TrieNode<T> clonedNode = new TrieNode<T>(this.Item, this.ParentNode);
            foreach (KeyValuePair<T, TrieNode<T>> currentChildNode in childNodes)
            {
                clonedNode.AddChildNode(currentChildNode.Key, currentChildNode.Value);
                clonedNode.SetSubTreeSize(currentChildNode.Key, this.subtreeSizes[currentChildNode.Key]);
            }

            return clonedNode;
        }
    }
}
