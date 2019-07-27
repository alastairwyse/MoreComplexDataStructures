/*
 * Copyright 2019 Alastair Wyse (https://github.com/alastairwyse/MoreComplexDataStructures/)
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
    /// An implementation of the Trie class, storing characters at each node, and exposing public methods with string parameters.
    /// </summary>
    public class CharacterTrie
    {
        /// <summary>The trie underlying the class.</summary>
        protected Trie<Char> underlyingTree;

        /// <summary>
        /// The total number of strings stored in the Trie.
        /// </summary>
        public Int32 Count
        {
            get { return underlyingTree.Count; }
        }

        /// <summary>
        /// Initialises a new instance of the MoreComplexDataStructures.CharacterTrie class.
        /// </summary>
        public CharacterTrie()
        {
            underlyingTree = new Trie<Char>();
        }

        /// <summary>
        /// Initialises a new instance of the MoreComplexDataStructures.CharacterTrie class.
        /// </summary>
        public CharacterTrie(out TrieNode<Char> rootNode)
        {
            underlyingTree = new Trie<Char>(out rootNode);
        }

        /// <summary>
        /// Removes all strings from the tree.
        /// </summary>
        public void Clear()
        {
            underlyingTree.Clear();
        }

        /// <summary>
        /// Adds the specified string to the trie.
        /// </summary>
        /// <param name="inputString">The string to add.</param>
        /// <exception cref="System.ArgumentException">The specified string is empty.</exception>
        /// <exception cref="System.ArgumentException">The specified string already exists in the trie.</exception>
        public void Insert(String inputString)
        {
            ThrowExceptionIfStringLengthIsZero(inputString);

            try
            {
                underlyingTree.Insert(new StringIListWrapper(inputString));
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException($"The specified string '{inputString}' already exists in the trie.", nameof(inputString), e);
            }
        }

        /// <summary>
        /// Removes the specified string from the trie.
        /// </summary>
        /// <param name="inputString">The string to remove.</param>
        /// <exception cref="System.ArgumentException">The specified string is empty.</exception>
        /// <exception cref="System.ArgumentException">The specified string does not exist in the trie.</exception>
        public void Delete(String inputString)
        {
            ThrowExceptionIfStringLengthIsZero(inputString);

            try
            {
                underlyingTree.Delete(new StringIListWrapper(inputString));
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException($"The specified string '{inputString}' does not exist in the trie.", nameof(inputString), e);
            }
        }

        /// <summary>
        /// Determines whether the trie contains the specified string.
        /// </summary>
        /// <param name="inputString">The string to check existence of.</param>
        /// <returns>True if the trie contains the specified string, otherwise false.</returns>
        /// <exception cref="System.ArgumentException">The specified string is empty.</exception>
        public Boolean Contains(String inputString)
        {
            ThrowExceptionIfStringLengthIsZero(inputString);

            return underlyingTree.Contains(new StringIListWrapper(inputString));
        }

        /// <summary>
        /// Returns a list of all strings in the trie which include the specified prefix string.
        /// </summary>
        /// <param name="prefixString">The prefix string.</param>
        /// <returns>An enumerator containing all strings which include the specified prefix string.</returns>
        public IEnumerable<String> GetAllStringsWithPrefix(String prefixString)
        {
            // TODO: Below return statement is quite inefficient, converting a List<Char> to a Char[] and then to a String... see if there's a more efficient way to do this.

            foreach (List<Char> currentSequence in underlyingTree.GetAllSequencesWithPrefix(new StringIListWrapper(prefixString)))
            {
                yield return new String(currentSequence.ToArray());
            }
        }

        /// <summary>
        /// Returns the number of strings stored in the trie which include the specified prefix string.  For example, if the trie contained strings "app", "apple", and "apples", GetCountOfStringsWithPrefix("app") would return 3. 
        /// </summary>
        /// <param name="prefixString">The prefix string.</param>
        /// <returns>The number of strings which include the specified prefix string.</returns>
        /// <exception cref="System.ArgumentException">The specified prefix string is empty.</exception>
        public Int32 GetCountOfStringsWithPrefix(String prefixString)
        {
            if (prefixString.Length == 0)
            {
                throw new ArgumentException("The specified prefix string is empty.", nameof(prefixString));
            }

            return underlyingTree.GetCountOfSequencesWithPrefix(new StringIListWrapper(prefixString));
        }

        /// <summary>
        /// Performs breadth-first search of the trie, invoking the specified action at each node.
        /// </summary>
        /// <param name="nodeAction">The action to perform at each node.  Accepts 2 parameters: the node to perform the action on, and a (0-based) integer indicating the depth of the node within the trie.</param>
        public void BreadthFirstSearch(Action<TrieNode<Char>, Int32> nodeAction)
        {
            underlyingTree.BreadthFirstSearch(nodeAction);
        }

        #region Private/Protected Methods

        /// <summary>
        /// Throws a System.ArgumentException if the specified string is empty.
        /// </summary>
        /// <param name="inputString">The string to check.</param>
        protected void ThrowExceptionIfStringLengthIsZero(String inputString)
        {
            if (inputString.Length == 0)
            {
                throw new ArgumentException("The specified string is empty.", nameof(inputString));
            }
        }

        #endregion

        #region Nested Classes

        /// <summary>
        /// Wraps the String class, and implements IList&lt;Char&gt; methods required by the underlying Trie class.
        /// </summary>
        protected class StringIListWrapper : IList<Char>
        {
            /// <summary>The string wrapped by the class.</summary>
            protected String wrappedString;

            /// <summary>
            /// Initialises a new instance of the MoreComplexDataStructures.CharacterTrie+StringIListWrapper class.
            /// </summary>
            /// <param name="wrappedString">The string wrapped by the class.</param>
            public StringIListWrapper(String wrappedString)
            {
                this.wrappedString = wrappedString;
            }

            #region IList<Char> methods

            #pragma warning disable 1591
            public int IndexOf(Char item)
            {
                throw new NotImplementedException();
            }

            public void Insert(Int32 index, Char item)
            {
                throw new NotImplementedException();
            }

            public void RemoveAt(Int32 index)
            {
                throw new NotImplementedException();
            }

            public Char this[Int32 index]
            {
                get
                {
                    return wrappedString[index];
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public void Add(Char item)
            {
                throw new NotImplementedException();
            }

            public void Clear()
            {
                throw new NotImplementedException();
            }

            public bool Contains(Char item)
            {
                throw new NotImplementedException();
            }

            public void CopyTo(Char[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            public Int32 Count
            {
                get { return wrappedString.Length; }
            }

            public Boolean IsReadOnly
            {
                get { throw new NotImplementedException(); }
            }

            public Boolean Remove(Char item)
            {
                throw new NotImplementedException();
            }

            public IEnumerator<Char> GetEnumerator()
            {
                return wrappedString.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
            #pragma warning restore 1591

            #endregion
        }

        #endregion
    }
}
