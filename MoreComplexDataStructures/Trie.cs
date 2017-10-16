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
    /// A trie (or prefix tree).
    /// </summary>
    /// <typeparam name="T">Specifies the type of item held by the trie.</typeparam>
    public class Trie<T>
    {
        /// <summary>The root node of the trie.</summary>
        protected TrieNode<T> rootNode;
        
        /// <summary>
        /// The total number of sequences stored in the Trie.
        /// </summary>
        public Int32 Count
        {
            get
            {
                Int32 totalSequences = 0;
                foreach (TrieNode<T> currentChildNode in rootNode.ChildNodes)
                {
                    totalSequences += rootNode.GetSubtreeSize(currentChildNode.Item);
                }
                return totalSequences;
            }
        }

        /// <summary>
        /// Initialises a new instance of the MoreComplexDataStructures.Trie class.
        /// </summary>
        public Trie()
        {
            rootNode = new TrieRootNode<T>();
        }

        /// <summary>
        /// Removes all sequences from the tree.
        /// </summary>
        public void Clear()
        {
            rootNode.RemoveAllChildren();
        }

        /// <summary>
        /// Adds the specified sequence of items to the trie.
        /// </summary>
        /// <param name="sequence">The sequence of items to add.</param>
        /// <exception cref="System.ArgumentException">The specified sequence is empty.</exception>
        /// <exception cref="System.ArgumentException">The specified sequence already exists in the trie.</exception>
        public void Insert(IList<T> sequence)
        {
            ThrowExceptionIfSequenceCountIsZero(sequence);

            TrieNode<T> currentNode = rootNode;

            for (Int32 i = 0; i < sequence.Count; i++)
            {
                // Need special handling for the last item in the sequence
                if (i == (sequence.Count - 1))
                {
                    if (currentNode.ChildNodeExists(sequence[i]) == false)
                    {
                        // Create a new terminator node to hold the last item in the sequence
                        SequenceTerminatorTrieNode<T> newNode = new SequenceTerminatorTrieNode<T>(sequence[i], currentNode);
                        currentNode.AddChildNode(sequence[i], newNode);
                    }
                    else
                    {
                        TrieNode<T> existingNode = currentNode.GetChildNode(sequence[i]);
                        if (existingNode is SequenceTerminatorTrieNode<T>)
                        {
                            // If a node for the last item already exists and is a terminator node, throw an exception
                            Action<TrieNode<T>, TrieNode<T>> decrementCountsAction = (parentNode, childNode) =>
                            {
                                if (childNode != null)
                                {
                                    parentNode.DecrementSubtreeSize(childNode.Item);
                                }
                            };
                            TraverseUpFromNode(existingNode.ParentNode, decrementCountsAction);

                            throw new ArgumentException("The specified sequence { " + ConvertSequenceToString(sequence) + " } already exists in the trie.", "sequence");
                        }
                        else
                        {
                            // Convert the existing node for the last item into a terminator node
                            Int32 previousSubtreeSize = currentNode.GetSubtreeSize(sequence[i]);
                            SequenceTerminatorTrieNode<T> terminatorNode = existingNode.CloneAsSequenceTerminator();
                            currentNode.RemoveChildNode(sequence[i]);
                            currentNode.AddChildNode(sequence[i], terminatorNode);
                            currentNode.SetSubTreeSize(sequence[i], previousSubtreeSize + 1);
                        }
                    }
                }
                else
                {
                    if (currentNode.ChildNodeExists(sequence[i]) == false)
                    {
                        TrieNode<T> newNode = new TrieNode<T>(sequence[i], currentNode);
                        currentNode.AddChildNode(sequence[i], newNode);
                    }
                    else
                    {
                        currentNode.IncrementSubtreeSize(sequence[i]);
                    }
                }

                currentNode = currentNode.GetChildNode(sequence[i]);
            }
        }

        /// <summary>
        /// Removes the specified sequence of items from the trie.
        /// </summary>
        /// <param name="sequence">The sequence of items to remove.</param>
        /// <exception cref="System.ArgumentException">The specified sequence is empty.</exception>
        /// <exception cref="System.ArgumentException">The specified sequence does not exist in the trie.</exception>
        public void Delete(IList<T> sequence)
        {
            ThrowExceptionIfSequenceCountIsZero(sequence);

            TrieNode<T> currentNode = rootNode;

            for (Int32 i = 0; i < sequence.Count; i++)
            {
                // Need special handling for the last item in the sequence
                if (i == (sequence.Count - 1))
                {
                    if (currentNode.ChildNodeExists(sequence[i]) == false)
                    {
                        throw new ArgumentException("The specified sequence { " + ConvertSequenceToString(sequence) + " } does not exist in the trie.", "sequence");
                    }
                    else
                    {
                        TrieNode<T> lastNode = currentNode.GetChildNode(sequence[i]);
                        if (!(lastNode is SequenceTerminatorTrieNode<T>))
                        {
                            throw new ArgumentException("The specified sequence { " + ConvertSequenceToString(sequence) + " } does not exist in the trie.", "sequence");
                        }
                        else
                        {
                            if (currentNode.GetSubtreeSize(sequence[i]) == 1)
                            {
                                // If the last node has no children it can be removed
                                currentNode.RemoveChildNode(sequence[i]);
                            }
                            else
                            {
                                // Convert the existing terminator node for the last item into a standard trie node
                                Int32 previousSubtreeSize = currentNode.GetSubtreeSize(sequence[i]);
                                TrieNode<T> standardNode = ((SequenceTerminatorTrieNode<T>)lastNode).CloneAsTrieNode();
                                currentNode.RemoveChildNode(sequence[i]);
                                currentNode.AddChildNode(sequence[i], standardNode);
                                currentNode.SetSubTreeSize(sequence[i], previousSubtreeSize - 1);
                            }

                            // Traverse up the trie either removing or decrementing the subtree counts of the parent nodes
                            Action<TrieNode<T>, TrieNode<T>> removeNodesOrDecrementCountsAction = (parentNode, childNode) =>
                            {
                                if (childNode != null)
                                {
                                    if (parentNode.GetSubtreeSize(childNode.Item) > 1)
                                    {
                                        parentNode.DecrementSubtreeSize(childNode.Item);
                                    }
                                    else
                                    {
                                        parentNode.RemoveChildNode(childNode.Item);
                                    }
                                }
                            };
                            TraverseUpFromNode(currentNode, removeNodesOrDecrementCountsAction);
                        }
                    }
                }
                else
                {
                    if (currentNode.ChildNodeExists(sequence[i]) == false)
                    {
                        throw new ArgumentException("The specified sequence { " + ConvertSequenceToString(sequence) + " } does not exist in the trie.", "sequence");
                    }

                    currentNode = currentNode.GetChildNode(sequence[i]);
                }
            }
        }

        /// <summary>
        /// Determines whether the trie contains the specified sequence.
        /// </summary>
        /// <param name="sequence">The sequence to check existence of.</param>
        /// <returns>True if the trie contains the specified sequence, otherwise false.</returns>
        /// <exception cref="System.ArgumentException">The specified sequence is empty.</exception>
        public Boolean Contains(IList<T> sequence)
        {
            ThrowExceptionIfSequenceCountIsZero(sequence);

            TrieNode<T> currentNode = rootNode;

            // TODO: Can (i == (sequence.Count - 1)) be put after (currentNode.ChildNodeExists(sequence[i]) == false) ??

            for (Int32 i = 0; i < sequence.Count; i++)
            {
                if (i == (sequence.Count - 1))
                {
                    if (currentNode.ChildNodeExists(sequence[i]) == false)
                    {
                        return false;
                    }
                    else
                    {
                        TrieNode<T> lastNode = currentNode.GetChildNode(sequence[i]);
                        if (!(lastNode is SequenceTerminatorTrieNode<T>))
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    if (currentNode.ChildNodeExists(sequence[i]) == false)
                    {
                        return false;
                    }

                    currentNode = currentNode.GetChildNode(sequence[i]);
                }
            }

            return false;
        }

        /// <summary>
        /// Returns a list of all sequences in the trie which include the specified prefix sequence.
        /// </summary>
        /// <param name="prefixSequence">The prefix sequence.</param>
        /// <returns>A list of all sequences which include the specified prefix sequence.</returns>
        public List<List<T>> GetAllSequencesWithPrefix(IList<T> prefixSequence)
        {
            List<List<T>> returnList = new List<List<T>>();
            TrieNode<T> lastPrefixSequenceItemNode = null;
            TrieNode<T> currentNode = rootNode;

            // Navigate to the node of the last item in the prefix sequence
            for (Int32 i = 0; i < prefixSequence.Count; i++)
            {
                if (currentNode.ChildNodeExists(prefixSequence[i]) == false)
                {
                    return returnList;
                }
                currentNode = currentNode.GetChildNode(prefixSequence[i]);
            }
            lastPrefixSequenceItemNode = currentNode;

            // Use breadth-first search to traverse the last item node, and all children
            Queue<TrieNode<T>> traversalQueue = new Queue<TrieNode<T>>();
            traversalQueue.Enqueue(currentNode);
            while (traversalQueue.Count > 0)
            {
                currentNode = traversalQueue.Dequeue();
                if (currentNode is SequenceTerminatorTrieNode<T>)
                {
                    returnList.Add(GenerateSequenceToNode(prefixSequence, lastPrefixSequenceItemNode, currentNode));
                }
                foreach(TrieNode<T> currentChildNode in currentNode.ChildNodes)
                {
                    traversalQueue.Enqueue(currentChildNode);
                }
            }

            return returnList;
        }

        /// <summary>
        /// Returns the number of sequences stored in the trie which include the specified prefix sequence.  For example, if the trie contained sequences 'app', 'apple', and 'apples', GetCountOfSequencesWithPrefix("app") would return 3. 
        /// </summary>
        /// <param name="prefixSequence">The prefix sequence.</param>
        /// <returns>The number of sequences which include the specified prefix sequence.</returns>
        /// <exception cref="System.ArgumentException">The specified prefix sequence is empty.</exception>
        public Int32 GetCountOfSequencesWithPrefix(IList<T> prefixSequence)
        {
            if (prefixSequence.Count == 0)
            {
                throw new ArgumentException("The specified prefix sequence is empty.", "prefixSequence");
            }

            TrieNode<T> currentNode = rootNode;

            // TODO: Can (i == (sequence.Count - 1)) be put after (currentNode.ChildNodeExists(sequence[i]) == false) ??

            for (Int32 i = 0; i < prefixSequence.Count; i++)
            {
                if (i == (prefixSequence.Count - 1))
                {
                    if (currentNode.ChildNodeExists(prefixSequence[i]) == false)
                    {
                        return 0;
                    }
                    else
                    {
                        return currentNode.GetSubtreeSize(prefixSequence[i]);
                    }
                }
                else
                {
                    if (currentNode.ChildNodeExists(prefixSequence[i]) == false)
                    {
                        return 0;
                    }

                    currentNode = currentNode.GetChildNode(prefixSequence[i]);
                }
            }

            return 0;
        }

        /// <summary>
        /// Performs breadth-first search of the trie, invoking the specified action at each node.
        /// </summary>
        /// <param name="nodeAction">The action to perform at each node.  Accepts 2 parameters: the node to perform the action on, and a (0-based) integer indicating the depth of the node within the trie.</param>
        public void BreadthFirstSearch(Action<TrieNode<T>, Int32> nodeAction)
        {
            // Holds the next node to process, and the depth of that node in the trie
            Queue<Tuple<TrieNode<T>, Int32>> traversalQueue = new Queue<Tuple<TrieNode<T>, Int32>>();
            traversalQueue.Enqueue(new Tuple<TrieNode<T>, Int32>(rootNode, 0));

            while (traversalQueue.Count > 0)
            {
                TrieNode<T> currentNode = traversalQueue.Peek().Item1;
                Int32 currentDepth = traversalQueue.Dequeue().Item2;
                TrieNode<T> replicatedNode;
                if (currentNode is SequenceTerminatorTrieNode<T>)
                {
                    replicatedNode = ((SequenceTerminatorTrieNode<T>)currentNode).Replicate();
                }
                else
                {
                    replicatedNode = currentNode.Replicate();
                }
                nodeAction.Invoke(replicatedNode, currentDepth);
                foreach (TrieNode<T> currentChildNode in currentNode.ChildNodes)
                {
                    traversalQueue.Enqueue(new Tuple<TrieNode<T>, Int32>(currentChildNode, currentDepth + 1));
                }
            }
        }

        # region Private/Protected Methods

        /// <summary>
        /// Generates the full sequence of items from the root to the item held by parameter 'terminatorNode'.
        /// </summary>
        /// <param name="prefixSequence">Fixed prefix sequence of items.</param>
        /// <param name="lastPrefixSequenceItemNode">The node holding the last item in the prefix sequence.</param>
        /// <param name="terminatorNode">The terminator node of the sequence to generate.</param>
        /// <returns>The sequence.</returns>
        protected List<T> GenerateSequenceToNode(IList<T> prefixSequence, TrieNode<T> lastPrefixSequenceItemNode, TrieNode<T> terminatorNode)
        {
            List<T> returnList = new List<T>(prefixSequence);
            Stack<T> postFixSequence = new Stack<T>();
            TrieNode<T> currentNode = terminatorNode;
            while (currentNode != lastPrefixSequenceItemNode)
            {
                postFixSequence.Push(currentNode.Item);
                currentNode = currentNode.ParentNode;
            }
            while (postFixSequence.Count > 0)
            {
                returnList.Add(postFixSequence.Pop());
            }
            return returnList;
        }

        /// <summary>
        /// Throws a System.ArgumentException if the specified list is empty.
        /// </summary>
        /// <param name="sequence">The list to check.</param>
        protected void ThrowExceptionIfSequenceCountIsZero(IList<T> sequence)
        {
            if (sequence.Count == 0)
            {
                throw new ArgumentException("The specified sequence is empty.", "sequence");
            }
        }

        /// <summary>
        /// Traverses up the trie from the specified node to the root, invoking an action at each node.
        /// </summary>
        /// <param name="startNode">The node to start traversing at.</param>
        /// <param name="nodeAction">The action to perform at each node.  Accepts 2 parameters: the node to perform the action on, and the child of that node in the forward traverse path.</param>
        protected void TraverseUpFromNode(TrieNode<T> startNode, Action<TrieNode<T>, TrieNode<T>> nodeAction)
        {
            TrieNode<T> currentNode = startNode;
            TrieNode<T> childNode = null;

            while (currentNode != null)
            {
                nodeAction.Invoke(currentNode, childNode);
                if (childNode == null)
                {
                    childNode = currentNode;
                }
                else
                {
                    childNode = childNode.ParentNode;
                }
                currentNode = currentNode.ParentNode;
            }
        }

        /// <summary>
        /// Converts the specified sequence of items into a string containing each item's ToString() implementation separated by spaces.
        /// </summary>
        /// <param name="sequence">The sequence to convert.</param>
        /// <returns>The converted sequence.</returns>
        protected String ConvertSequenceToString(IList<T> sequence)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (Int32 i = 0; i < sequence.Count; i++)
            {
                stringBuilder.Append(sequence[i].ToString());
                if (i < (sequence.Count - 1))
                {
                    stringBuilder.Append(" ");
                }
            }

            return stringBuilder.ToString();
        }

        #endregion

        #region Nested Classes

        /// <summary>
        /// The root node of a trie.
        /// </summary>
        /// <typeparam name="T">Specifies the type of item held by the node.</typeparam>
        protected class TrieRootNode<T> : TrieNode<T>
        {
            /// <summary>
            /// Initialises a new instance of the MoreComplexDataStructures.Trie+TrieRootNode class.
            /// </summary>
            public TrieRootNode()
                : base(default(T), null)
            {
            }
        }

        #endregion
    }
}
