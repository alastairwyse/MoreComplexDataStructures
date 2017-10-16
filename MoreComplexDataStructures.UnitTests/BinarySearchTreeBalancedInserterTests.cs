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
using NMock2;
using MoreComplexDataStructures;

namespace MoreComplexDataStructures.UnitTests
{
    /// <summary>
    /// Unit tests for the BinarySearchTreeBalancedInserter class.
    /// </summary>
    public class BinarySearchTreeBalancedInserterTests
    {
        private Mockery mockery;
        private IBinarySearchTree<Int32> mockBinarySearchTree;
        private BinarySearchTreeBalancedInserter testBinarySearchTreeBalancedInserter;

        [SetUp]
        protected void SetUp()
        {
            mockery = new Mockery();
            mockBinarySearchTree = mockery.NewMock<IBinarySearchTree<Int32>>();
            testBinarySearchTreeBalancedInserter = new BinarySearchTreeBalancedInserter();
        }

        /// <summary>
        /// Tests that an exception is thrown if the Insert() method is called with an 'elements' parameter which is empty.
        /// </summary>
        [Test]
        public void Insert_EmptyElementList()
        {
            ArgumentException e = Assert.Throws<ArgumentException>(delegate
            {
                testBinarySearchTreeBalancedInserter.Insert(mockBinarySearchTree, new Int32[0]);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("The specified array of elements is empty."));
            Assert.AreEqual("elements", e.ParamName);
        }

        /// <summary>
        /// Tests that a wrapping exception is thrown if the Insert() method is called, and an underlying exception occurs when calling the tree's Add() method.
        /// </summary>
        [Test]
        public void Insert_TreeAddException()
        {
            Int32[] elementList = new Int32[] { 1, 2, 3, 4, 5 };
            String mockExceptionText = "Mock add exception.";

            using (mockery.Ordered)
            {
                Expect.Once.On(mockBinarySearchTree).Method("Clear").WithNoArguments();
                Expect.Once.On(mockBinarySearchTree).Method("Add").With(3).Will(Throw.Exception(new Exception(mockExceptionText)));
            }

            Exception e = Assert.Throws<Exception>(delegate
            {
                testBinarySearchTreeBalancedInserter.Insert(mockBinarySearchTree, elementList);
            });

            Assert.That(e.Message, NUnit.Framework.Does.StartWith("Error encountered when adding item '3' to the tree."));
            Assert.That(e.InnerException.Message, NUnit.Framework.Does.StartWith(mockExceptionText));
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// Success tests for the Insert() method.
        /// </summary>
        [Test]
        public void Insert()
        {
            Int32[] elementList = new Int32[] { 0 };

            using (mockery.Ordered)
            {
                Expect.Once.On(mockBinarySearchTree).Method("Clear").WithNoArguments();
                Expect.Once.On(mockBinarySearchTree).Method("Add").With(0);
            }

            testBinarySearchTreeBalancedInserter.Insert(mockBinarySearchTree, elementList);

            mockery.VerifyAllExpectationsHaveBeenMet();


            elementList = new Int32[] { 0, 1 };

            mockery.ClearExpectation(mockBinarySearchTree);
            using (mockery.Ordered)
            {
                Expect.Once.On(mockBinarySearchTree).Method("Clear").WithNoArguments();
                Expect.Once.On(mockBinarySearchTree).Method("Add").With(1);
                Expect.Once.On(mockBinarySearchTree).Method("Add").With(0);
            }

            testBinarySearchTreeBalancedInserter.Insert(mockBinarySearchTree, elementList);

            mockery.VerifyAllExpectationsHaveBeenMet();


            elementList = new Int32[] { 0, 1, 2 };

            mockery.ClearExpectation(mockBinarySearchTree);
            using (mockery.Ordered)
            {
                Expect.Once.On(mockBinarySearchTree).Method("Clear").WithNoArguments();
                Expect.Once.On(mockBinarySearchTree).Method("Add").With(1);
                Expect.Once.On(mockBinarySearchTree).Method("Add").With(0);
                Expect.Once.On(mockBinarySearchTree).Method("Add").With(2);
            }

            testBinarySearchTreeBalancedInserter.Insert(mockBinarySearchTree, elementList);

            mockery.VerifyAllExpectationsHaveBeenMet();


            elementList = new Int32[] { 0, 1, 2, 3 };

            mockery.ClearExpectation(mockBinarySearchTree);
            using (mockery.Ordered)
            {
                Expect.Once.On(mockBinarySearchTree).Method("Clear").WithNoArguments();
                Expect.Once.On(mockBinarySearchTree).Method("Add").With(2);
                Expect.Once.On(mockBinarySearchTree).Method("Add").With(1);
                Expect.Once.On(mockBinarySearchTree).Method("Add").With(0);
                Expect.Once.On(mockBinarySearchTree).Method("Add").With(3);
            }

            testBinarySearchTreeBalancedInserter.Insert(mockBinarySearchTree, elementList);

            mockery.VerifyAllExpectationsHaveBeenMet();


            elementList = new Int32[] { 0, 1, 2, 3, 4 };

            mockery.ClearExpectation(mockBinarySearchTree);
            using (mockery.Ordered)
            {
                Expect.Once.On(mockBinarySearchTree).Method("Clear").WithNoArguments();
                Expect.Once.On(mockBinarySearchTree).Method("Add").With(2);
                Expect.Once.On(mockBinarySearchTree).Method("Add").With(1);
                Expect.Once.On(mockBinarySearchTree).Method("Add").With(0);
                Expect.Once.On(mockBinarySearchTree).Method("Add").With(4);
                Expect.Once.On(mockBinarySearchTree).Method("Add").With(3);
            }

            testBinarySearchTreeBalancedInserter.Insert(mockBinarySearchTree, elementList);

            mockery.VerifyAllExpectationsHaveBeenMet();
        }
    }
}
