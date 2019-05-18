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
using NUnit.Framework;
using MoreComplexDataStructures;

namespace MoreComplexDataStructures.UnitTests
{
    /// <summary>
    /// Unit tests for the CharacterTrie class.
    /// </summary>
    public class CharacterTrieTests
    {
        private CharacterTrie testCharacterTrie;

        [SetUp]
        protected void SetUp()
        {
            testCharacterTrie = new CharacterTrie();
        }

        /// <summary>
        /// Success tests for the Count property.
        /// </summary>
        [Test]
        public void Count()
        {
            Assert.AreEqual(0, testCharacterTrie.Count);

            testCharacterTrie.Insert("cop");
            Assert.AreEqual(1, testCharacterTrie.Count);

            testCharacterTrie.Insert("d");
            Assert.AreEqual(2, testCharacterTrie.Count);

            testCharacterTrie.Delete("d");
            Assert.AreEqual(1, testCharacterTrie.Count);

            testCharacterTrie.Delete("cop");
            Assert.AreEqual(0, testCharacterTrie.Count);

            testCharacterTrie.Insert("cop");
            Assert.AreEqual(1, testCharacterTrie.Count);

            testCharacterTrie.Clear();
            Assert.AreEqual(0, testCharacterTrie.Count);
        }

        /// <summary>
        /// Success tests for the constructor which provides a reference to the root node of the trie.
        /// </summary>
        [Test]
        public void Constructor_RootNodeReference()
        {
            TrieNode<Char> rootNode = null;

            testCharacterTrie = new CharacterTrie(out rootNode);
            foreach (String currentTestWord in new List<String>() { "cop", "d", "apple", "apps", "app" })
            {
                testCharacterTrie.Insert(currentTestWord);
            }

            Assert.AreEqual(3, rootNode.ChildNodes.Count());
            Assert.IsTrue(rootNode.ChildNodeExists('a'));
            Assert.IsTrue(rootNode.ChildNodeExists('c'));
            Assert.IsTrue(rootNode.ChildNodeExists('d'));
        }

        /// <summary>
        /// Tests that an external reference to the root node is maintained after the Clear() method is called.
        /// </summary>
        [Test]
        public void Constructor_RootNodeReferenceMaintainedAfterClearIsCalled()
        {
            TrieNode<Char> rootNode = null;

            testCharacterTrie = new CharacterTrie(out rootNode);
            foreach (String currentTestWord in new List<String>() { "cop", "d", "apple", "apps", "app" })
            {
                testCharacterTrie.Insert(currentTestWord);
            }
            testCharacterTrie.Clear();
            testCharacterTrie.Insert("apple");

            Assert.AreEqual(1, rootNode.ChildNodes.Count());
            Assert.IsTrue(rootNode.ChildNodeExists('a'));
        }

        /// <summary>
        /// Tests that an exception is thrown if the Insert() method is called with an empty string.
        /// </summary>
        [Test]
        public void Insert_EmptyString()
        {
            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                testCharacterTrie.Insert("");
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The specified string is empty."));
            Assert.AreEqual("inputString", e.ParamName);
        }

        /// <summary>
        /// Tests that an exception is thrown if the Insert() method is called with a string which already exists in the trie.
        /// </summary>
        [Test]
        public void Insert_StringAlreadyExists()
        {
            testCharacterTrie.Insert("apple");

            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                testCharacterTrie.Insert("apple");
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The specified string 'apple' already exists in the trie."));
            Assert.AreEqual("inputString", e.ParamName);
        }

        /// <summary>
        /// Success tests for the Insert() and GetAllStringsWithPrefix() methods.
        /// </summary>
        [Test]
        public void Insert_GetAllStringsWithPrefix()
        {
            foreach (String currentTestWord in new List<String>() { "cop", "d", "apple", "apps", "app" })
            {
                testCharacterTrie.Insert(currentTestWord);
            }

            Assert.AreEqual(5, testCharacterTrie.Count);
            HashSet<String> allStrings = PutStringsIntoHashSet(testCharacterTrie.GetAllStringsWithPrefix(""));
            Assert.AreEqual(5, allStrings.Count);
            Assert.IsTrue(allStrings.Contains("cop"));
            Assert.IsTrue(allStrings.Contains("d"));
            Assert.IsTrue(allStrings.Contains("apple"));
            Assert.IsTrue(allStrings.Contains("apps"));
            Assert.IsTrue(allStrings.Contains("app"));
        }

        /// <summary>
        /// Tests that an exception is thrown if the Delete() method is called with an empty string.
        /// </summary>
        [Test]
        public void Delete_EmptyString()
        {
            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                testCharacterTrie.Delete("");
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The specified string is empty."));
            Assert.AreEqual("inputString", e.ParamName);
        }

        /// <summary>
        /// Tests that an exception is thrown if the Delete() method is called with a string which which doesn't exist in the trie.
        /// </summary>
        [Test]
        public void Delete_StringDoesntExist()
        {
            testCharacterTrie.Insert("apple");

            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                testCharacterTrie.Delete("apples");
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The specified string 'apples' does not exist in the trie."));
            Assert.AreEqual("inputString", e.ParamName);
        }

        /// <summary>
        /// Tests that an exception is thrown if the Contains() method is called with an empty string.
        /// </summary>
        [Test]
        public void Contains_EmptyString()
        {
            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                testCharacterTrie.Contains("");
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The specified string is empty."));
            Assert.AreEqual("inputString", e.ParamName);
        }

        /// <summary>
        /// Success tests for the Contains() method.
        /// </summary>
        [Test]
        public void Contains()
        {
            testCharacterTrie.Insert("cop");
            testCharacterTrie.Insert("d");
            testCharacterTrie.Insert("apple");

            Assert.IsTrue(testCharacterTrie.Contains("cop"));
            Assert.IsTrue(testCharacterTrie.Contains("d"));
            Assert.IsTrue(testCharacterTrie.Contains("apple"));
            Assert.IsFalse(testCharacterTrie.Contains("e"));
            Assert.IsFalse(testCharacterTrie.Contains("cp"));
            Assert.IsFalse(testCharacterTrie.Contains("ao"));
            Assert.IsFalse(testCharacterTrie.Contains("app"));
            Assert.IsFalse(testCharacterTrie.Contains("appl"));
        }

        /// <summary>
        /// Tests that an exception is thrown if the GetCountOfStringsWithPrefix() method is called with an empty string.
        /// </summary>
        [Test]
        public void GetCountOfStringsWithPrefix_EmptyString()
        {
            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                testCharacterTrie.GetCountOfStringsWithPrefix("");
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The specified prefix string is empty."));
            Assert.AreEqual("prefixString", e.ParamName);
        }

        /// <summary>
        /// Success tests for the GetCountOfStringsWithPrefix() method.
        /// </summary>
        [Test]
        public void GetCountOfStringsWithPrefix()
        {
            testCharacterTrie.Insert("cop");
            testCharacterTrie.Insert("d");
            testCharacterTrie.Insert("apple");
            testCharacterTrie.Insert("apples");
            testCharacterTrie.Insert("app");

            Assert.AreEqual(1, testCharacterTrie.GetCountOfStringsWithPrefix("c"));
            Assert.AreEqual(1, testCharacterTrie.GetCountOfStringsWithPrefix("co"));
            Assert.AreEqual(1, testCharacterTrie.GetCountOfStringsWithPrefix("cop"));
            Assert.AreEqual(0, testCharacterTrie.GetCountOfStringsWithPrefix("copy"));
            Assert.AreEqual(1, testCharacterTrie.GetCountOfStringsWithPrefix("d"));
            Assert.AreEqual(0, testCharacterTrie.GetCountOfStringsWithPrefix("do"));
            Assert.AreEqual(3, testCharacterTrie.GetCountOfStringsWithPrefix("a"));
            Assert.AreEqual(3, testCharacterTrie.GetCountOfStringsWithPrefix("ap"));
            Assert.AreEqual(3, testCharacterTrie.GetCountOfStringsWithPrefix("app"));
            Assert.AreEqual(2, testCharacterTrie.GetCountOfStringsWithPrefix("appl"));
            Assert.AreEqual(2, testCharacterTrie.GetCountOfStringsWithPrefix("apple"));
            Assert.AreEqual(1, testCharacterTrie.GetCountOfStringsWithPrefix("apples"));
            Assert.AreEqual(0, testCharacterTrie.GetCountOfStringsWithPrefix("appless"));
            Assert.AreEqual(0, testCharacterTrie.GetCountOfStringsWithPrefix("appli"));
            Assert.AreEqual(0, testCharacterTrie.GetCountOfStringsWithPrefix("applez"));
            Assert.AreEqual(0, testCharacterTrie.GetCountOfStringsWithPrefix("e"));
        }

        /// <summary>
        /// Success tests for the BreadthFirstSearch() method.
        /// </summary>
        [Test]
        public void BreadthFirstSearch()
        {
            testCharacterTrie.Insert("abc");
            testCharacterTrie.Insert("abcd");
            testCharacterTrie.Insert("abe");
            testCharacterTrie.Insert("fg");
            testCharacterTrie.Insert("h");
            var allNodeCharacters = new HashSet<Tuple<Char, Int32>>();
            Int32 nodeCount = 0;
            Action<TrieNode<Char>, Int32> nodeAction = (currentNode, depth) =>
            {
                allNodeCharacters.Add(new Tuple<Char, Int32>(currentNode.Item, depth));
                nodeCount++;
            };

            testCharacterTrie.BreadthFirstSearch(nodeAction);

            Assert.AreEqual(9, nodeCount);
            Assert.AreEqual(9, allNodeCharacters.Count);
            Assert.IsTrue(allNodeCharacters.Contains(new Tuple<Char, Int32>('a', 1)));
            Assert.IsTrue(allNodeCharacters.Contains(new Tuple<Char, Int32>('b', 2)));
            Assert.IsTrue(allNodeCharacters.Contains(new Tuple<Char, Int32>('c', 3)));
            Assert.IsTrue(allNodeCharacters.Contains(new Tuple<Char, Int32>('d', 4)));
            Assert.IsTrue(allNodeCharacters.Contains(new Tuple<Char, Int32>('e', 3)));
            Assert.IsTrue(allNodeCharacters.Contains(new Tuple<Char, Int32>('f', 1)));
            Assert.IsTrue(allNodeCharacters.Contains(new Tuple<Char, Int32>('g', 2)));
            Assert.IsTrue(allNodeCharacters.Contains(new Tuple<Char, Int32>('h', 1)));
        }

        #region Private Methods

        /// <summary>
        /// Puts strings in an inputted enumerable into a HashSet.
        /// </summary>
        /// <param name="inputStrings">The strings.</param>
        /// <returns>A HashSet containing the strings.</returns>
        private HashSet<String> PutStringsIntoHashSet(IEnumerable<String> inputStrings)
        {
            HashSet<String> returnSet = new HashSet<String>();
            foreach (String currentString in inputStrings)
            {
                returnSet.Add(currentString);
            }

            return returnSet;
        }

        #endregion
    }
}
