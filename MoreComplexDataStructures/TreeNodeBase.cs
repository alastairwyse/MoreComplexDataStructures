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
    /// Base class for tree nodes.
    /// </summary>
    /// <typeparam name="T">Specifies the type of item held by the node.</typeparam>
    public abstract class TreeNodeBase<T> where T : IComparable<T>
    {
        /// <summary>The item held by the node.</summary>
        protected T item;

        /// <summary>
        /// The item held by the node.
        /// </summary>
        public virtual T Item
        {
            get
            {
                return item;
            }
            set
            {
                item = value;
            }
        }

        /// <summary>
        /// Initialises a new instance of the MoreComplexDataStructures.TreeNodeBase class.
        /// </summary>
        /// <param name="item">The item held by the node.</param>
        protected TreeNodeBase(T item)
        {
            this.item = item;
        }
    }
}
