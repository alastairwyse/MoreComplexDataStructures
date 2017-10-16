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
    /// Unit tests for the TrieNode class.
    /// </summary>
    public class TrieNodeTests
    {
        [SetUp]
        protected void SetUp()
        {
        }

        /// <summary>
        /// Success tests for the ChildNodeExists() method.
        /// </summary>
        [Test]
        public void ChildNodeExists()
        {
            TrieNode<Char> aNode = new TrieNode<Char>('a', null);
            TrieNode<Char> bNode = new TrieNode<Char>('b', aNode);
            aNode.AddChildNode(bNode.Item, bNode);
            TrieNode<Char> cNode = new TrieNode<Char>('c', aNode);
            aNode.AddChildNode(cNode.Item, cNode);

            Boolean result = aNode.ChildNodeExists('b');

            Assert.IsTrue(result);

            result = aNode.ChildNodeExists('c');

            Assert.IsTrue(result);

            result = aNode.ChildNodeExists('d');

            Assert.IsFalse(result);
        }

        /// <summary>
        /// Tests that an exception is thrown if the GetChildNode() method is called with an item for which a child node doesn't exist.
        /// </summary>
        [Test]
        public void GetChildNode_ChildNodeForItemDoesntExist()
        {
            TrieNode<Char> aNode = new TrieNode<Char>('a', null);
            TrieNode<Char> bNode = new TrieNode<Char>('b', aNode);
            aNode.AddChildNode(bNode.Item, bNode);
            TrieNode<Char> cNode = new TrieNode<Char>('c', aNode);
            aNode.AddChildNode(cNode.Item, cNode);

            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                aNode.GetChildNode('d');
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("A child node for the specified item 'd' does not exist."));
            Assert.AreEqual("item", e.ParamName);
        }

        /// <summary>
        /// Success tests for the GetChildNode(), AddChildNode(), and GetSubtreeSize() methods.
        /// </summary>
        [Test]
        public void GetChildNode_AddChildNode_GetSubtreeSize()
        {
            TrieNode<Char> aNode = new TrieNode<Char>('a', null);
            TrieNode<Char> bNode = new TrieNode<Char>('b', aNode);
            aNode.AddChildNode(bNode.Item, bNode);
            TrieNode<Char> cNode = new TrieNode<Char>('c', aNode);
            aNode.AddChildNode(cNode.Item, cNode);

            TrieNode<Char> resultNode = aNode.GetChildNode('b');

            Assert.AreSame(bNode, resultNode);
            Assert.AreEqual(1, aNode.GetSubtreeSize('b'));
            Assert.AreEqual(1, aNode.GetSubtreeSize('c'));
        }

        /// <summary>
        /// Tests that an exception is thrown if the AddChildNode() method is called with an item for which a child node already exists.
        /// </summary>
        [Test]
        public void AddChildNode_ChildNodeWithItemAlreadyExists()
        {
            TrieNode<Char> aNode = new TrieNode<Char>('a', null);
            TrieNode<Char> bNode = new TrieNode<Char>('b', aNode);
            aNode.AddChildNode(bNode.Item, bNode);
            TrieNode<Char> cNode = new TrieNode<Char>('c', aNode);
            aNode.AddChildNode(cNode.Item, cNode);
            TrieNode<Char> bNode2 = new TrieNode<Char>('b', aNode);

            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                aNode.AddChildNode(bNode2.Item, bNode2);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("A child node for the specified item 'b' already exists."));
            Assert.AreEqual("item", e.ParamName);
        }

        /// <summary>
        /// Tests that an exception is thrown if the GetSubtreeSize() method is called with an item for which a child node doesn't exist.
        /// </summary>
        [Test]
        public void GetSubtreeSize_ChildNodeForItemDoesntExist()
        {
            TrieNode<Char> aNode = new TrieNode<Char>('a', null);
            TrieNode<Char> bNode = new TrieNode<Char>('b', aNode);
            aNode.AddChildNode(bNode.Item, bNode);
            TrieNode<Char> cNode = new TrieNode<Char>('c', aNode);
            aNode.AddChildNode(cNode.Item, cNode);

            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                aNode.GetSubtreeSize('d');
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("A child node for the specified item 'd' does not exist."));
            Assert.AreEqual("item", e.ParamName);
        }

        /// <summary>
        /// Tests that an exception is thrown if the RemoveChildNode() method is called with an item for which a child node doesn't exist.
        /// </summary>
        [Test]
        public void RemoveChildNode_ChildNodeForItemDoesntExist()
        {
            TrieNode<Char> aNode = new TrieNode<Char>('a', null);
            TrieNode<Char> bNode = new TrieNode<Char>('b', aNode);
            aNode.AddChildNode(bNode.Item, bNode);
            TrieNode<Char> cNode = new TrieNode<Char>('c', aNode);
            aNode.AddChildNode(cNode.Item, cNode);

            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                aNode.RemoveChildNode('d');
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("A child node for the specified item 'd' does not exist."));
            Assert.AreEqual("item", e.ParamName);
        }

        /// <summary>
        /// Success tests for the RemoveChildNode() method.
        /// </summary>
        [Test]
        public void RemoveChildNode()
        {
            TrieNode<Char> aNode = new TrieNode<Char>('a', null);
            TrieNode<Char> bNode = new TrieNode<Char>('b', aNode);
            aNode.AddChildNode(bNode.Item, bNode);
            TrieNode<Char> cNode = new TrieNode<Char>('c', aNode);
            aNode.AddChildNode(cNode.Item, cNode);

            Assert.IsTrue(aNode.ChildNodeExists('b'));

            aNode.RemoveChildNode('b');

            Assert.IsFalse(aNode.ChildNodeExists('b'));

            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                aNode.GetSubtreeSize('b');
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("A child node for the specified item 'b' does not exist."));
            Assert.AreEqual("item", e.ParamName);
        }

        /// <summary>
        /// Success tests for the RemoveAllChildren() method.
        /// </summary>
        [Test]
        public void RemoveAllChildren()
        {
            TrieNode<Char> aNode = new TrieNode<Char>('a', null);
            TrieNode<Char> bNode = new TrieNode<Char>('b', aNode);
            aNode.AddChildNode(bNode.Item, bNode);
            TrieNode<Char> cNode = new TrieNode<Char>('c', aNode);
            aNode.AddChildNode(cNode.Item, cNode);

            Assert.IsTrue(aNode.ChildNodeExists('b'));
            Assert.IsTrue(aNode.ChildNodeExists('c'));

            aNode.RemoveAllChildren();

            Assert.IsFalse(aNode.ChildNodeExists('b'));
            Assert.IsFalse(aNode.ChildNodeExists('c'));
        }

        /// <summary>
        /// Success tests for the IncrementSubtreeSize(), DecrementSubtreeSize(), and GetSubtreeSize() methods.
        /// </summary>
        [Test]
        public void IncrementSubtreeSize_DecrementSubtreeSize_GetSubtreeSize()
        {
            TrieNode<Char> aNode = new TrieNode<Char>('a', null);
            TrieNode<Char> bNode = new TrieNode<Char>('b', aNode);
            aNode.AddChildNode(bNode.Item, bNode);
            TrieNode<Char> cNode = new TrieNode<Char>('c', aNode);
            aNode.AddChildNode(cNode.Item, cNode);
            TrieNode<Char> dNode = new TrieNode<Char>('d', bNode);
            bNode.AddChildNode(dNode.Item, dNode);

            Assert.AreEqual(1, aNode.GetSubtreeSize('b'));
            Assert.AreEqual(1, aNode.GetSubtreeSize('c'));
            Assert.AreEqual(1, bNode.GetSubtreeSize('d'));

            aNode.IncrementSubtreeSize('b');

            Assert.AreEqual(2, aNode.GetSubtreeSize('b'));
            Assert.AreEqual(1, aNode.GetSubtreeSize('c'));
            Assert.AreEqual(1, bNode.GetSubtreeSize('d'));

            aNode.DecrementSubtreeSize('b');

            Assert.AreEqual(1, aNode.GetSubtreeSize('b'));
            Assert.AreEqual(1, aNode.GetSubtreeSize('c'));
            Assert.AreEqual(1, bNode.GetSubtreeSize('d'));
        }

        /// <summary>
        /// Tests that an exception is thrown if the IncrementSubtreeSize() method is called with an item for which a child node doesn't exist.
        /// </summary>
        [Test]
        public void IncrementSubtreeSize_ChildNodeForItemDoesntExist()
        {
            TrieNode<Char> aNode = new TrieNode<Char>('a', null);
            TrieNode<Char> bNode = new TrieNode<Char>('b', aNode);
            aNode.AddChildNode(bNode.Item, bNode);
            TrieNode<Char> cNode = new TrieNode<Char>('c', aNode);
            aNode.AddChildNode(cNode.Item, cNode);

            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                aNode.IncrementSubtreeSize('d');
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("A child node for the specified item 'd' does not exist."));
            Assert.AreEqual("item", e.ParamName);
        }

        /// <summary>
        /// Tests that an exception is thrown if the DecrementSubtreeSize() method is called with an item for which a child node doesn't exist.
        /// </summary>
        [Test]
        public void DecrementSubtreeSize_ChildNodeForItemDoesntExist()
        {
            TrieNode<Char> aNode = new TrieNode<Char>('a', null);
            TrieNode<Char> bNode = new TrieNode<Char>('b', aNode);
            aNode.AddChildNode(bNode.Item, bNode);
            TrieNode<Char> cNode = new TrieNode<Char>('c', aNode);
            aNode.AddChildNode(cNode.Item, cNode);

            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                aNode.DecrementSubtreeSize('d');
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("A child node for the specified item 'd' does not exist."));
            Assert.AreEqual("item", e.ParamName);
        }

        /// <summary>
        /// Success tests for the SetSubTreeSize() method.
        /// </summary>
        [Test]
        public void SetSubTreeSize()
        {
            TrieNode<Char> aNode = new TrieNode<Char>('a', null);
            TrieNode<Char> bNode = new TrieNode<Char>('b', aNode);
            aNode.AddChildNode(bNode.Item, bNode);
            TrieNode<Char> cNode = new TrieNode<Char>('c', aNode);
            aNode.AddChildNode(cNode.Item, cNode);
            TrieNode<Char> dNode = new TrieNode<Char>('d', bNode);
            bNode.AddChildNode(dNode.Item, dNode);

            Assert.AreEqual(1, aNode.GetSubtreeSize('b'));
            Assert.AreEqual(1, aNode.GetSubtreeSize('c'));
            Assert.AreEqual(1, bNode.GetSubtreeSize('d'));

            aNode.SetSubTreeSize('b', 3);

            Assert.AreEqual(3, aNode.GetSubtreeSize('b'));
            Assert.AreEqual(1, aNode.GetSubtreeSize('c'));
            Assert.AreEqual(1, bNode.GetSubtreeSize('d'));

            aNode.SetSubTreeSize('b', 1);

            Assert.AreEqual(1, aNode.GetSubtreeSize('b'));
            Assert.AreEqual(1, aNode.GetSubtreeSize('c'));
            Assert.AreEqual(1, bNode.GetSubtreeSize('d'));
        }

        /// <summary>
        /// Tests that an exception is thrown if the SetSubTreeSize() method is called with an item for which a child node doesn't exist.
        /// </summary>
        [Test]
        public void SetSubTreeSize_ChildNodeForItemDoesntExist()
        {
            TrieNode<Char> aNode = new TrieNode<Char>('a', null);
            TrieNode<Char> bNode = new TrieNode<Char>('b', aNode);
            aNode.AddChildNode(bNode.Item, bNode);
            TrieNode<Char> cNode = new TrieNode<Char>('c', aNode);
            aNode.AddChildNode(cNode.Item, cNode);

            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                aNode.SetSubTreeSize('d', 3);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("A child node for the specified item 'd' does not exist."));
            Assert.AreEqual("item", e.ParamName);
        }

        /// <summary>
        /// Success tests for the Replicate() method.
        /// </summary>
        [Test]
        public void Replicate()
        {
            TrieNode<Char> aNode = new TrieNode<Char>('a', null);
            TrieNode<Char> bNode = new TrieNode<Char>('b', aNode);
            aNode.AddChildNode(bNode.Item, bNode);
            TrieNode<Char> cNode = new TrieNode<Char>('c', bNode);
            bNode.AddChildNode(cNode.Item, cNode);
            TrieNode<Char> dNode = new TrieNode<Char>('d', bNode);
            bNode.AddChildNode(dNode.Item, dNode);
            bNode.SetSubTreeSize('c', 2);
            bNode.SetSubTreeSize('d', 3);

            TrieNode<Char> replicaNode = bNode.Replicate();

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
        /// Success tests for the CloneAsSequenceTerminator() method.
        /// </summary>
        [Test]
        public void CloneAsSequenceTerminator()
        {
            TrieNode<Char> aNode = new TrieNode<Char>('a', null);
            TrieNode<Char> bNode = new TrieNode<Char>('b', aNode);
            aNode.AddChildNode(bNode.Item, bNode);
            TrieNode<Char> cNode = new TrieNode<Char>('c', bNode);
            bNode.AddChildNode(cNode.Item, cNode);
            TrieNode<Char> dNode = new TrieNode<Char>('d', bNode);
            bNode.AddChildNode(dNode.Item, dNode);
            bNode.SetSubTreeSize('c', 2);
            bNode.SetSubTreeSize('d', 3);

            SequenceTerminatorTrieNode<Char> clonedNode = bNode.CloneAsSequenceTerminator();

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
