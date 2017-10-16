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
    /// Unit tests for the SequenceTerminatorTrieNode class.
    /// </summary>
    public class SequenceTerminatorTrieNodeTests
    {
        [SetUp]
        protected void SetUp()
        {
        }

        /// <summary>
        /// Success tests for the Replicate() method.
        /// </summary>
        [Test]
        public void Replicate()
        {
            SequenceTerminatorTrieNode<Char> aNode = new SequenceTerminatorTrieNode<Char>('a', null);
            SequenceTerminatorTrieNode<Char> bNode = new SequenceTerminatorTrieNode<Char>('b', aNode);
            aNode.AddChildNode(bNode.Item, bNode);
            SequenceTerminatorTrieNode<Char> cNode = new SequenceTerminatorTrieNode<Char>('c', bNode);
            bNode.AddChildNode(cNode.Item, cNode);
            SequenceTerminatorTrieNode<Char> dNode = new SequenceTerminatorTrieNode<Char>('d', bNode);
            bNode.AddChildNode(dNode.Item, dNode);
            bNode.SetSubTreeSize('c', 2);
            bNode.SetSubTreeSize('d', 3);

            SequenceTerminatorTrieNode<Char> replicaNode = bNode.Replicate();

            Assert.AreEqual('b', replicaNode.Item);
            Assert.IsTrue(replicaNode.ChildNodeExists('c'));
            Assert.IsTrue(replicaNode.ChildNodeExists('d'));
            Assert.AreEqual(2, replicaNode.GetSubtreeSize('c'));
            Assert.AreEqual(3, replicaNode.GetSubtreeSize('d'));
            Assert.AreNotSame(bNode, replicaNode);
            Assert.AreNotSame(aNode, replicaNode.ParentNode);
            Assert.AreNotSame(cNode, replicaNode.GetChildNode('c'));
            Assert.AreNotSame(dNode, replicaNode.GetChildNode('d'));


            // Test replicating a node with null parent
            replicaNode = aNode.Replicate();
            Assert.AreEqual('a', replicaNode.Item);
            Assert.IsNull(replicaNode.ParentNode);
            Assert.IsTrue(replicaNode.ChildNodeExists('b'));
            Assert.AreNotSame(bNode, replicaNode.GetChildNode('b'));
        }

        /// <summary>
        /// Success tests for the CloneAsTrieNode() method.
        /// </summary>
        [Test]
        public void CloneAsTrieNode()
        {
            SequenceTerminatorTrieNode<Char> aNode = new SequenceTerminatorTrieNode<Char>('a', null);
            SequenceTerminatorTrieNode<Char> bNode = new SequenceTerminatorTrieNode<Char>('b', aNode);
            aNode.AddChildNode(bNode.Item, bNode);
            SequenceTerminatorTrieNode<Char> cNode = new SequenceTerminatorTrieNode<Char>('c', bNode);
            bNode.AddChildNode(cNode.Item, cNode);
            SequenceTerminatorTrieNode<Char> dNode = new SequenceTerminatorTrieNode<Char>('d', bNode);
            bNode.AddChildNode(dNode.Item, dNode);
            bNode.SetSubTreeSize('c', 2);
            bNode.SetSubTreeSize('d', 3);

            TrieNode<Char> clonedNode = bNode.CloneAsTrieNode();

            Assert.IsNotInstanceOf(typeof(SequenceTerminatorTrieNode<Char>), clonedNode);
            Assert.AreEqual('b', clonedNode.Item);
            Assert.IsTrue(clonedNode.ChildNodeExists('c'));
            Assert.IsTrue(clonedNode.ChildNodeExists('d'));
            Assert.AreEqual(2, clonedNode.GetSubtreeSize('c'));
            Assert.AreEqual(3, clonedNode.GetSubtreeSize('d'));
            Assert.AreNotSame(bNode, clonedNode);
            Assert.AreSame(aNode, clonedNode.ParentNode);
            Assert.AreSame(cNode, clonedNode.GetChildNode('c'));
            Assert.AreSame(dNode, clonedNode.GetChildNode('d'));
        }
    }
}
