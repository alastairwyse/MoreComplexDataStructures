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
using NUnit.Framework;
using MoreComplexDataStructures;

namespace MoreComplexDataStructures.UnitTests
{
    /// <summary>
    /// Unit tests for the Trie class.
    /// </summary>
    public class TrieTests
    {
        private Trie<Char> testTrie;

        [SetUp]
        protected void SetUp()
        {
            testTrie = new Trie<Char>();
        }
        
        /// <summary>
        /// Success tests for the Insert() method.
        /// </summary>
        [Test]
        public void Insert()
        {
            // Test insert of several words into an empty trie
            // Then GetAllNodes() should form a list of lists with the following values (x-axis is outer list index, y-axis is inner list index)
            //
            //    0 1 2 3 4 5
            //    -----------
            // 0 |  b p p l e
            //   |     / /
            // 1 |  c o p
            //   |     /
            // 2 |  a p  
            //   |
            testTrie.Insert("bp".ToCharArray());
            testTrie.Insert("cop".ToCharArray());
            testTrie.Insert("apple".ToCharArray());

            List<List<TrieNode<Char>>> allNodes = GetAllNodes(testTrie);

            Assert.AreEqual(1, allNodes[0].Count);
            Assert.AreEqual(3, allNodes[1].Count);
            Assert.AreEqual(3, allNodes[2].Count);
            Assert.AreEqual(2, allNodes[3].Count);
            Assert.AreEqual(1, allNodes[4].Count);
            Assert.AreEqual(1, allNodes[5].Count);
            Assert.AreEqual('b', allNodes[1][0].Item);
            Assert.AreEqual('c', allNodes[1][1].Item);
            Assert.AreEqual('a', allNodes[1][2].Item);
            Assert.AreEqual('p', allNodes[2][0].Item);
            Assert.AreEqual('o', allNodes[2][1].Item);
            Assert.AreEqual('p', allNodes[2][2].Item);
            Assert.AreEqual('p', allNodes[3][0].Item);
            Assert.AreEqual('p', allNodes[3][1].Item);
            Assert.AreEqual('l', allNodes[4][0].Item);
            Assert.AreEqual('e', allNodes[5][0].Item);
            Assert.AreEqual(1, allNodes[0][0].GetSubtreeSize('b'));
            Assert.AreEqual(1, allNodes[0][0].GetSubtreeSize('c'));
            Assert.AreEqual(1, allNodes[0][0].GetSubtreeSize('a'));
            Assert.AreEqual(1, allNodes[1][0].GetSubtreeSize('p'));
            Assert.AreEqual(1, allNodes[1][1].GetSubtreeSize('o'));
            Assert.AreEqual(1, allNodes[1][2].GetSubtreeSize('p'));
            Assert.AreEqual(1, allNodes[2][1].GetSubtreeSize('p'));
            Assert.AreEqual(1, allNodes[2][2].GetSubtreeSize('p'));
            Assert.AreEqual(1, allNodes[3][1].GetSubtreeSize('l'));
            Assert.AreEqual(1, allNodes[4][0].GetSubtreeSize('e'));
            Assert.AreEqual('b', allNodes[2][0].ParentNode.Item);
            Assert.AreEqual('c', allNodes[2][1].ParentNode.Item);
            Assert.AreEqual('a', allNodes[2][2].ParentNode.Item);
            Assert.AreEqual('o', allNodes[3][0].ParentNode.Item);
            Assert.AreEqual('p', allNodes[3][1].ParentNode.Item);
            Assert.AreEqual('p', allNodes[4][0].ParentNode.Item);
            Assert.AreEqual('l', allNodes[5][0].ParentNode.Item);
            Assert.IsTrue(allNodes[0][0].ChildNodeExists('b'));
            Assert.IsTrue(allNodes[0][0].ChildNodeExists('c'));
            Assert.IsTrue(allNodes[0][0].ChildNodeExists('a'));
            Assert.IsTrue(allNodes[1][0].ChildNodeExists('p'));
            Assert.IsTrue(allNodes[1][1].ChildNodeExists('o'));
            Assert.IsTrue(allNodes[1][2].ChildNodeExists('p'));
            Assert.IsTrue(allNodes[2][1].ChildNodeExists('p'));
            Assert.IsTrue(allNodes[2][2].ChildNodeExists('p'));
            Assert.IsTrue(allNodes[3][1].ChildNodeExists('l'));
            Assert.IsTrue(allNodes[4][0].ChildNodeExists('e'));
            Assert.AreEqual(3, allNodes[0][0].ChildNodes.Count<TrieNode<Char>>());
            Assert.AreEqual(1, allNodes[1][0].ChildNodes.Count<TrieNode<Char>>());
            Assert.AreEqual(1, allNodes[1][1].ChildNodes.Count<TrieNode<Char>>());
            Assert.AreEqual(1, allNodes[1][2].ChildNodes.Count<TrieNode<Char>>());
            Assert.AreEqual(0, allNodes[2][0].ChildNodes.Count<TrieNode<Char>>());
            Assert.AreEqual(1, allNodes[2][1].ChildNodes.Count<TrieNode<Char>>());
            Assert.AreEqual(1, allNodes[2][2].ChildNodes.Count<TrieNode<Char>>());
            Assert.AreEqual(0, allNodes[3][0].ChildNodes.Count<TrieNode<Char>>());
            Assert.AreEqual(1, allNodes[3][1].ChildNodes.Count<TrieNode<Char>>());
            Assert.AreEqual(1, allNodes[4][0].ChildNodes.Count<TrieNode<Char>>());
            Assert.AreEqual(0, allNodes[5][0].ChildNodes.Count<TrieNode<Char>>());
            Assert.IsInstanceOf(typeof(SequenceTerminatorTrieNode<Char>), allNodes[2][0]);
            Assert.IsInstanceOf(typeof(SequenceTerminatorTrieNode<Char>), allNodes[3][0]);
            Assert.IsInstanceOf(typeof(SequenceTerminatorTrieNode<Char>), allNodes[5][0]);
            // TODO: Do I need to check non-terminator nodes are actually non-terminator nodes?


            // Add a sequence which is a sub-sequence of an existing one ("app" is a subsequence of "apple")
            testTrie.Insert("app".ToCharArray());

            allNodes = GetAllNodes(testTrie);

            Assert.AreEqual(1, allNodes[0].Count);
            Assert.AreEqual(3, allNodes[1].Count);
            Assert.AreEqual(3, allNodes[2].Count);
            Assert.AreEqual(2, allNodes[3].Count);
            Assert.AreEqual(1, allNodes[4].Count);
            Assert.AreEqual(1, allNodes[5].Count);
            Assert.AreEqual('p', allNodes[3][1].Item);
            Assert.AreEqual('l', allNodes[4][0].Item);
            Assert.AreEqual(2, allNodes[0][0].GetSubtreeSize('a'));
            Assert.AreEqual(2, allNodes[1][2].GetSubtreeSize('p'));
            Assert.AreEqual(2, allNodes[2][2].GetSubtreeSize('p'));
            Assert.AreEqual(1, allNodes[2][1].GetSubtreeSize('p'));
            Assert.AreEqual(1, allNodes[3][1].GetSubtreeSize('l'));
            Assert.AreEqual('p', allNodes[3][1].ParentNode.Item);
            Assert.AreEqual('p', allNodes[4][0].ParentNode.Item);
            Assert.IsTrue(allNodes[2][2].ChildNodeExists('p'));
            Assert.IsTrue(allNodes[3][1].ChildNodeExists('l'));
            Assert.AreEqual(1, allNodes[2][1].ChildNodes.Count<TrieNode<Char>>());
            Assert.AreEqual(1, allNodes[3][1].ChildNodes.Count<TrieNode<Char>>());
            Assert.IsInstanceOf(typeof(SequenceTerminatorTrieNode<Char>), allNodes[3][1]);
            Assert.IsInstanceOf(typeof(SequenceTerminatorTrieNode<Char>), allNodes[5][0]);
        }

        /// <summary>
        /// Tests that an exception is thrown if the Insert() method is called with a sequence which already exists in the trie.
        /// </summary>
        [Test]
        public void Insert_SequenceAlreadyExists()
        {
            // Insert several words into an empty trie
            // Then GetAllNodes() should form a list of lists with the following values (x-axis is outer list index, y-axis is inner list index)
            //
            //    0 1 2 3 4 5
            //    -----------
            // 0 |  b p p l e
            //   |     / /
            // 1 |  c o p
            //   |     /
            // 2 |  a p  
            //   |
            // 3 |  d
            //
            testTrie.Insert("bp".ToCharArray());
            testTrie.Insert("cop".ToCharArray());
            testTrie.Insert("apple".ToCharArray());
            testTrie.Insert("app".ToCharArray());
            testTrie.Insert("d".ToCharArray());

            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                testTrie.Insert("app".ToCharArray());
            });

            List<List<TrieNode<Char>>> allNodes = GetAllNodes(testTrie);

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The specified sequence { a p p } already exists in the trie."));
            Assert.AreEqual("sequence", e.ParamName);
            Assert.AreEqual(1, allNodes[0].Count);
            Assert.AreEqual(4, allNodes[1].Count);
            Assert.AreEqual(3, allNodes[2].Count);
            Assert.AreEqual(2, allNodes[3].Count);
            Assert.AreEqual(1, allNodes[4].Count);
            Assert.AreEqual(1, allNodes[5].Count);
            Assert.AreEqual('p', allNodes[3][1].Item);
            Assert.AreEqual('l', allNodes[4][0].Item);
            Assert.AreEqual(2, allNodes[0][0].GetSubtreeSize('a'));
            Assert.AreEqual(2, allNodes[1][2].GetSubtreeSize('p'));
            Assert.AreEqual(2, allNodes[2][2].GetSubtreeSize('p'));
            Assert.AreEqual(1, allNodes[2][1].GetSubtreeSize('p'));
            Assert.AreEqual(1, allNodes[3][1].GetSubtreeSize('l'));
            Assert.AreEqual('p', allNodes[3][1].ParentNode.Item);
            Assert.AreEqual('p', allNodes[4][0].ParentNode.Item);
            Assert.IsTrue(allNodes[2][2].ChildNodeExists('p'));
            Assert.IsTrue(allNodes[3][1].ChildNodeExists('l'));
            Assert.AreEqual(1, allNodes[2][1].ChildNodes.Count<TrieNode<Char>>());
            Assert.AreEqual(1, allNodes[3][1].ChildNodes.Count<TrieNode<Char>>());
            Assert.IsInstanceOf(typeof(SequenceTerminatorTrieNode<Char>), allNodes[3][1]);
            Assert.IsInstanceOf(typeof(SequenceTerminatorTrieNode<Char>), allNodes[5][0]);


            e = Assert.Throws<ArgumentException>(delegate
            {
                testTrie.Insert("d".ToCharArray());
            });

            allNodes = GetAllNodes(testTrie);

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The specified sequence { d } already exists in the trie."));
            Assert.AreEqual("sequence", e.ParamName);
            Assert.AreEqual(1, allNodes[0].Count);
            Assert.AreEqual(4, allNodes[1].Count);
            Assert.AreEqual(1, allNodes[0][0].GetSubtreeSize('d'));
        }

        /// <summary>
        /// Tests that an exception is thrown if the Insert() method is called with an empty sequence.
        /// </summary>
        [Test]
        public void Insert_EmptySequence()
        {
            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                testTrie.Insert("".ToCharArray());
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The specified sequence is empty."));
            Assert.AreEqual("sequence", e.ParamName);
        }

        /// <summary>
        /// Tests that an exception is thrown if the Delete() method is called with a sequence which doesn't exist, but a subsequence of that sequence to remove does exist.
        /// </summary>
        [Test]
        public void Delete_SequenceDoesntExistButSubSequenceExists()
        {
            testTrie.Insert("app".ToCharArray());

            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                testTrie.Delete("apple".ToCharArray());
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The specified sequence { a p p l e } does not exist in the trie."));
            Assert.AreEqual("sequence", e.ParamName);
        }

        /// <summary>
        /// Tests that an exception is thrown if the Delete() method is called with a sequence which doesn't exist, but a subsequence of that sequence to remove does exist and is one character shorter than the sequence to remove.
        /// </summary>
        [Test]
        public void Delete_SequenceDoesntExistButSubSequenceOneItemShorterExists()
        {
            testTrie.Insert("app".ToCharArray());

            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                testTrie.Delete("apps".ToCharArray());
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The specified sequence { a p p s } does not exist in the trie."));
            Assert.AreEqual("sequence", e.ParamName);
        }

        /// <summary>
        /// Tests that an exception is thrown if the Delete() method is called with a sequence which doesn't exist, but a super sequence (longer sequence) of that sequence to remove does exist.
        /// </summary>
        [Test]
        public void Delete_SequenceDoesntExistButSuperSequenceExists()
        {
            testTrie.Insert("apple".ToCharArray());

            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                testTrie.Delete("app".ToCharArray());
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The specified sequence { a p p } does not exist in the trie."));
            Assert.AreEqual("sequence", e.ParamName);
        }

        /// <summary>
        /// Tests that an exception is thrown if the Delete() method is called with an empty sequence.
        /// </summary>
        [Test]
        public void Delete_EmptySequence()
        {
            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                testTrie.Delete("".ToCharArray());
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The specified sequence is empty."));
            Assert.AreEqual("sequence", e.ParamName);
        }

        /// <summary>
        /// Success tests for the Delete() method.
        /// </summary>
        [Test]
        public void Delete()
        {
            // Insert several words into an empty trie
            // Then GetAllNodes() should form a list of lists with the following values (x-axis is outer list index, y-axis is inner list index)
            //
            //    0 1 2 3 4 5
            //    -----------
            // 0 |  a p p 
            //   |    
            // 1 |  c o p
            //   |  
            // 2 |  d
            //
            testTrie.Insert("app".ToCharArray());
            testTrie.Insert("cop".ToCharArray());
            testTrie.Insert("d".ToCharArray());

            // Test Delete() operation where all nodes in a sequence are removed
            testTrie.Delete("app".ToCharArray());

            List<List<TrieNode<Char>>> allNodes = GetAllNodes(testTrie);

            Assert.AreEqual(1, allNodes[0].Count);
            Assert.AreEqual(2, allNodes[1].Count);
            Assert.AreEqual(1, allNodes[2].Count);
            Assert.AreEqual(1, allNodes[3].Count);
            Assert.AreEqual(1, allNodes[0][0].GetSubtreeSize('c'));
            Assert.AreEqual(1, allNodes[0][0].GetSubtreeSize('d'));
            Assert.IsTrue(allNodes[0][0].ChildNodeExists('c'));
            Assert.IsTrue(allNodes[0][0].ChildNodeExists('d'));
            Assert.IsTrue(allNodes[1][0].ChildNodeExists('o'));
            Assert.AreEqual(2, allNodes[0][0].ChildNodes.Count<TrieNode<Char>>());
            Assert.AreEqual(1, allNodes[1][0].ChildNodes.Count<TrieNode<Char>>());


            // Test Delete() operation where all nodes have their subtree size decremented (app and apple exist, app is removed)
            testTrie.Insert("apple".ToCharArray());
            testTrie.Insert("app".ToCharArray());

            testTrie.Delete("app".ToCharArray());

            // GetAllNodes() should form a list of lists with the following values (x-axis is outer list index, y-axis is inner list index)
            //
            //    0 1 2 3 4 5
            //    -----------
            // 0 |  a p p l e
            //   |       
            // 1 |  c o p 
            //   |   
            // 2 |  d

            allNodes = GetAllNodes(testTrie);

            Assert.AreEqual(1, allNodes[0].Count);
            Assert.AreEqual(3, allNodes[1].Count);
            Assert.AreEqual(2, allNodes[2].Count);
            Assert.AreEqual(2, allNodes[3].Count);
            Assert.AreEqual(1, allNodes[4].Count);
            Assert.AreEqual(1, allNodes[5].Count);
            Assert.AreEqual('a', allNodes[1][0].Item);
            Assert.AreEqual('p', allNodes[2][0].Item);
            Assert.AreEqual('p', allNodes[3][0].Item);
            Assert.AreEqual('l', allNodes[4][0].Item);
            Assert.AreEqual('e', allNodes[5][0].Item);
            Assert.AreEqual(1, allNodes[0][0].GetSubtreeSize('c'));
            Assert.AreEqual(1, allNodes[0][0].GetSubtreeSize('d'));
            Assert.AreEqual(1, allNodes[0][0].GetSubtreeSize('a'));
            Assert.AreEqual(1, allNodes[1][0].GetSubtreeSize('p'));
            Assert.AreEqual(1, allNodes[2][0].GetSubtreeSize('p'));
            Assert.AreEqual(1, allNodes[3][0].GetSubtreeSize('l'));
            Assert.AreEqual(1, allNodes[4][0].GetSubtreeSize('e'));
            Assert.AreEqual('p', allNodes[3][0].ParentNode.Item);
            Assert.AreEqual('p', allNodes[4][0].ParentNode.Item);
            Assert.IsTrue(allNodes[0][0].ChildNodeExists('c'));
            Assert.IsTrue(allNodes[0][0].ChildNodeExists('d'));
            Assert.IsTrue(allNodes[0][0].ChildNodeExists('a'));
            Assert.IsTrue(allNodes[1][1].ChildNodeExists('o'));
            Assert.IsTrue(allNodes[1][0].ChildNodeExists('p'));
            Assert.IsTrue(allNodes[2][0].ChildNodeExists('p'));
            Assert.IsTrue(allNodes[3][0].ChildNodeExists('l'));
            Assert.IsTrue(allNodes[4][0].ChildNodeExists('e'));
            Assert.AreEqual(1, allNodes[1][0].ChildNodes.Count<TrieNode<Char>>());
            Assert.AreEqual(1, allNodes[2][0].ChildNodes.Count<TrieNode<Char>>());
            Assert.AreEqual(1, allNodes[3][0].ChildNodes.Count<TrieNode<Char>>());
            Assert.IsInstanceOf(typeof(TrieNode<Char>), allNodes[3][0]);
            Assert.IsInstanceOf(typeof(SequenceTerminatorTrieNode<Char>), allNodes[5][0]);


            // Test Delete() operation where nodes in a sequence have a mix of operations... some where their subtree size decremented, and some where the node is removed (ap, app, and apple exist, app is removed)
            testTrie.Insert("ap".ToCharArray());
            testTrie.Insert("app".ToCharArray());

            testTrie.Delete("app".ToCharArray());

            // GetAllNodes() should form a list of lists with the following values (x-axis is outer list index, y-axis is inner list index)
            //
            //    0 1 2 3 4 5
            //    -----------
            // 0 |  a p p l e
            //   |       
            // 1 |  c o p 
            //   |   
            // 2 |  d

            allNodes = GetAllNodes(testTrie);

            Assert.AreEqual(1, allNodes[0].Count);
            Assert.AreEqual(3, allNodes[1].Count);
            Assert.AreEqual(2, allNodes[2].Count);
            Assert.AreEqual(2, allNodes[3].Count);
            Assert.AreEqual(1, allNodes[4].Count);
            Assert.AreEqual(1, allNodes[5].Count);
            Assert.AreEqual('a', allNodes[1][0].Item);
            Assert.AreEqual('p', allNodes[2][0].Item);
            Assert.AreEqual('p', allNodes[3][0].Item);
            Assert.AreEqual('l', allNodes[4][0].Item);
            Assert.AreEqual('e', allNodes[5][0].Item);
            Assert.AreEqual(1, allNodes[0][0].GetSubtreeSize('c'));
            Assert.AreEqual(1, allNodes[0][0].GetSubtreeSize('d'));
            Assert.AreEqual(2, allNodes[0][0].GetSubtreeSize('a'));
            Assert.AreEqual(2, allNodes[1][0].GetSubtreeSize('p'));
            Assert.AreEqual(1, allNodes[2][0].GetSubtreeSize('p'));
            Assert.AreEqual(1, allNodes[3][0].GetSubtreeSize('l'));
            Assert.AreEqual(1, allNodes[4][0].GetSubtreeSize('e'));
            Assert.AreEqual('p', allNodes[3][0].ParentNode.Item);
            Assert.AreEqual('p', allNodes[4][0].ParentNode.Item);
            Assert.IsTrue(allNodes[0][0].ChildNodeExists('c'));
            Assert.IsTrue(allNodes[0][0].ChildNodeExists('d'));
            Assert.IsTrue(allNodes[0][0].ChildNodeExists('a'));
            Assert.IsTrue(allNodes[1][1].ChildNodeExists('o'));
            Assert.IsTrue(allNodes[1][0].ChildNodeExists('p'));
            Assert.IsTrue(allNodes[2][0].ChildNodeExists('p'));
            Assert.IsTrue(allNodes[3][0].ChildNodeExists('l'));
            Assert.IsTrue(allNodes[4][0].ChildNodeExists('e'));
            Assert.AreEqual(1, allNodes[1][0].ChildNodes.Count<TrieNode<Char>>());
            Assert.AreEqual(1, allNodes[2][0].ChildNodes.Count<TrieNode<Char>>());
            Assert.AreEqual(1, allNodes[3][0].ChildNodes.Count<TrieNode<Char>>());
            Assert.IsInstanceOf(typeof(SequenceTerminatorTrieNode<Char>), allNodes[2][0]);
            Assert.IsInstanceOf(typeof(TrieNode<Char>), allNodes[3][0]);
            Assert.IsInstanceOf(typeof(SequenceTerminatorTrieNode<Char>), allNodes[5][0]);
        }
        
        /// <summary>
        /// Tests that an exception is thrown if the Contains() method is called with an empty sequence.
        /// </summary>
        [Test]
        public void Contains_EmptySequence()
        {
            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                testTrie.Contains("".ToCharArray());
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The specified sequence is empty."));
            Assert.AreEqual("sequence", e.ParamName);
        }

        /// <summary>
        /// Success tests for the Contains() method.
        /// </summary>
        [Test]
        public void Contains()
        {
            testTrie.Insert("cop".ToCharArray());
            testTrie.Insert("d".ToCharArray());
            testTrie.Insert("apple".ToCharArray());

            Assert.IsTrue(testTrie.Contains("cop".ToCharArray()));
            Assert.IsTrue(testTrie.Contains("d".ToCharArray()));
            Assert.IsTrue(testTrie.Contains("apple".ToCharArray()));
            Assert.IsFalse(testTrie.Contains("e".ToCharArray()));
            Assert.IsFalse(testTrie.Contains("cp".ToCharArray()));
            Assert.IsFalse(testTrie.Contains("ao".ToCharArray()));
            Assert.IsFalse(testTrie.Contains("app".ToCharArray()));
            Assert.IsFalse(testTrie.Contains("appl".ToCharArray()));
        }
        
        /// <summary>
        /// Tests that an exception is thrown if the GetCountOfSequencesWithPrefix() method is called with an empty sequence.
        /// </summary>
        [Test]
        public void GetCountOfSequencesWithPrefix_EmptySequence()
        {
            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                testTrie.GetCountOfSequencesWithPrefix("".ToCharArray());
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The specified prefix sequence is empty."));
            Assert.AreEqual("prefixSequence", e.ParamName);
        }

        /// <summary>
        /// Success tests for the GetCountOfSequencesWithPrefix() method.
        /// </summary>
        [Test]
        public void GetCountOfSequencesWithPrefix()
        {
            testTrie.Insert("cop".ToCharArray());
            testTrie.Insert("d".ToCharArray());
            testTrie.Insert("apple".ToCharArray());
            testTrie.Insert("apples".ToCharArray());
            testTrie.Insert("app".ToCharArray());

            Assert.AreEqual(1, testTrie.GetCountOfSequencesWithPrefix("c".ToCharArray()));
            Assert.AreEqual(1, testTrie.GetCountOfSequencesWithPrefix("co".ToCharArray()));
            Assert.AreEqual(1, testTrie.GetCountOfSequencesWithPrefix("cop".ToCharArray()));
            Assert.AreEqual(0, testTrie.GetCountOfSequencesWithPrefix("copy".ToCharArray()));
            Assert.AreEqual(1, testTrie.GetCountOfSequencesWithPrefix("d".ToCharArray()));
            Assert.AreEqual(0, testTrie.GetCountOfSequencesWithPrefix("do".ToCharArray()));
            Assert.AreEqual(3, testTrie.GetCountOfSequencesWithPrefix("a".ToCharArray()));
            Assert.AreEqual(3, testTrie.GetCountOfSequencesWithPrefix("ap".ToCharArray()));
            Assert.AreEqual(3, testTrie.GetCountOfSequencesWithPrefix("app".ToCharArray()));
            Assert.AreEqual(2, testTrie.GetCountOfSequencesWithPrefix("appl".ToCharArray()));
            Assert.AreEqual(2, testTrie.GetCountOfSequencesWithPrefix("apple".ToCharArray()));
            Assert.AreEqual(1, testTrie.GetCountOfSequencesWithPrefix("apples".ToCharArray()));
            Assert.AreEqual(0, testTrie.GetCountOfSequencesWithPrefix("appless".ToCharArray()));
            Assert.AreEqual(0, testTrie.GetCountOfSequencesWithPrefix("appli".ToCharArray()));
            Assert.AreEqual(0, testTrie.GetCountOfSequencesWithPrefix("applez".ToCharArray()));
            Assert.AreEqual(0, testTrie.GetCountOfSequencesWithPrefix("e".ToCharArray()));
        }

        /// <summary>
        /// Success tests for the GetAllSequencesWithPrefix() method.
        /// </summary>
        [Test]
        public void GetAllSequencesWithPrefix()
        {
            testTrie.Insert("apple".ToCharArray());
            testTrie.Insert("d".ToCharArray());
            testTrie.Insert("cop".ToCharArray());
            testTrie.Insert("app".ToCharArray());
            testTrie.Insert("apps".ToCharArray());
            testTrie.Insert("apples".ToCharArray());

            HashSet<String> results = PutCharacterListsIntoHashSet(testTrie.GetAllSequencesWithPrefix("e".ToCharArray()));

            Assert.AreEqual(0, results.Count);


            results = PutCharacterListsIntoHashSet(testTrie.GetAllSequencesWithPrefix("".ToCharArray()));

            Assert.AreEqual(6, results.Count);
            Assert.IsTrue(results.Contains("apple"));
            Assert.IsTrue(results.Contains("d"));
            Assert.IsTrue(results.Contains("cop"));
            Assert.IsTrue(results.Contains("app"));
            Assert.IsTrue(results.Contains("apps"));
            Assert.IsTrue(results.Contains("apples"));


            results = PutCharacterListsIntoHashSet(testTrie.GetAllSequencesWithPrefix("a".ToCharArray()));

            Assert.AreEqual(4, results.Count);
            Assert.IsTrue(results.Contains("apple"));
            Assert.IsTrue(results.Contains("app"));
            Assert.IsTrue(results.Contains("apps"));
            Assert.IsTrue(results.Contains("apples"));


            results = PutCharacterListsIntoHashSet(testTrie.GetAllSequencesWithPrefix("ap".ToCharArray()));

            Assert.AreEqual(4, results.Count);
            Assert.IsTrue(results.Contains("apple"));
            Assert.IsTrue(results.Contains("app"));
            Assert.IsTrue(results.Contains("apps"));
            Assert.IsTrue(results.Contains("apples"));


            results = PutCharacterListsIntoHashSet(testTrie.GetAllSequencesWithPrefix("apv".ToCharArray()));

            Assert.AreEqual(0, results.Count);


            results = PutCharacterListsIntoHashSet(testTrie.GetAllSequencesWithPrefix("app".ToCharArray()));

            Assert.AreEqual(4, results.Count);
            Assert.IsTrue(results.Contains("apple"));
            Assert.IsTrue(results.Contains("app"));
            Assert.IsTrue(results.Contains("apps"));
            Assert.IsTrue(results.Contains("apples"));


            results = PutCharacterListsIntoHashSet(testTrie.GetAllSequencesWithPrefix("apps".ToCharArray()));

            Assert.AreEqual(1, results.Count);
            Assert.IsTrue(results.Contains("apps"));


            results = PutCharacterListsIntoHashSet(testTrie.GetAllSequencesWithPrefix("apple".ToCharArray()));

            Assert.AreEqual(2, results.Count);
            Assert.IsTrue(results.Contains("apple"));
            Assert.IsTrue(results.Contains("apples"));


            results = PutCharacterListsIntoHashSet(testTrie.GetAllSequencesWithPrefix("apples".ToCharArray()));

            Assert.AreEqual(1, results.Count);
            Assert.IsTrue(results.Contains("apples"));


            results = PutCharacterListsIntoHashSet(testTrie.GetAllSequencesWithPrefix("appless".ToCharArray()));

            Assert.AreEqual(0, results.Count);
        }

        /// <summary>
        /// Success tests for the Count property.
        /// </summary>
        [Test]
        public void Count()
        {
            Assert.AreEqual(0, testTrie.Count);

            testTrie.Insert("cop".ToCharArray());
            Assert.AreEqual(1, testTrie.Count);

            testTrie.Insert("d".ToCharArray());
            Assert.AreEqual(2, testTrie.Count);

            testTrie.Insert("apple".ToCharArray());
            Assert.AreEqual(3, testTrie.Count);

            testTrie.Insert("apps".ToCharArray());
            Assert.AreEqual(4, testTrie.Count);

            testTrie.Insert("app".ToCharArray());
            Assert.AreEqual(5, testTrie.Count);

            testTrie.Insert("apples".ToCharArray());
            Assert.AreEqual(6, testTrie.Count);

            testTrie.Delete("apples".ToCharArray());
            Assert.AreEqual(5, testTrie.Count);

            testTrie.Delete("app".ToCharArray());
            Assert.AreEqual(4, testTrie.Count);

            testTrie.Delete("apps".ToCharArray());
            Assert.AreEqual(3, testTrie.Count);

            testTrie.Delete("apple".ToCharArray());
            Assert.AreEqual(2, testTrie.Count);

            testTrie.Delete("d".ToCharArray());
            Assert.AreEqual(1, testTrie.Count);

            testTrie.Delete("cop".ToCharArray());
            Assert.AreEqual(0, testTrie.Count);

            testTrie.Insert("cop".ToCharArray());
            Assert.AreEqual(1, testTrie.Count);

            testTrie.Clear();
            Assert.AreEqual(0, testTrie.Count);
        }

        /// <summary>
        /// Success tests for the constructor which provides a reference to the root node of the trie.
        /// </summary>
        [Test]
        public void Constructor_RootNodeReference()
        {
            TrieNode<Char> rootNode = null;

            testTrie = new Trie<Char>(out rootNode);
            foreach (Char[] currentTestWord in new List<Char[]> { "cop".ToCharArray(), "d".ToCharArray(), "apple".ToCharArray(), "apps".ToCharArray(), "app".ToCharArray() })
            {
                testTrie.Insert(currentTestWord);
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

            testTrie = new Trie<Char>(out rootNode);
            foreach (Char[] currentTestWord in new List<Char[]> { "cop".ToCharArray(), "d".ToCharArray(), "apple".ToCharArray(), "apps".ToCharArray(), "app".ToCharArray() })
            {
                testTrie.Insert(currentTestWord);
            }
            testTrie.Clear();
            testTrie.Insert("apple".ToCharArray());

            Assert.AreEqual(1, rootNode.ChildNodes.Count());
            Assert.IsTrue(rootNode.ChildNodeExists('a'));
        }

        #region Private Methods

        /// <summary>
        /// Retrieves a replica of each node in the inputted trie via a breadth-first search, putting all nodes at each depth level into a separate list of nodes.
        /// </summary>
        /// <typeparam name="T">Specifies the type of item held by the trie.</typeparam>
        /// <param name="inputTrie">the trie to retrieve the nodes from.</param>
        /// <returns>A list of lists of nodes.  Each list contains all the nodes at a single (0-based) depth level of the trie.</returns>
        private List<List<TrieNode<T>>> GetAllNodes<T>(Trie<T> inputTrie)
        {
            // TODO: Child nodes in a TrieNode are held in a Dictionary, which does not preserve ordering.  Hence the order of the children of any node which has multiple children is not deterministic.
            //       Need to find a better way to do this.

            List<List<TrieNode<T>>> allNodes = new List<List<TrieNode<T>>>();
            Action<TrieNode<T>, Int32> collectAllNodesAction = (currentNode, depth) =>
            {
                if (depth >= allNodes.Count)
                {
                    allNodes.Add(new List<TrieNode<T>>());
                }
                allNodes[depth].Add(currentNode);
            };
            inputTrie.BreadthFirstSearch(collectAllNodesAction);

            return allNodes;
        }

        /// <summary>
        /// Converts an enumerable of character lists into a HashSet of strings.
        /// </summary>
        /// <param name="inputLists">The enumerable to convert.</param>
        /// <returns>A HashSet containing strings equivalent to the contents of the inputted enumerable.</returns>
        private HashSet<String> PutCharacterListsIntoHashSet(IEnumerable<List<Char>> inputLists)
        {
            HashSet<String> returnSet = new HashSet<String>();
            foreach (List<Char> currentCharacterList in inputLists)
            {
                returnSet.Add(new String(currentCharacterList.ToArray()));
            }

            return returnSet;
        }

        #endregion
    }
}
