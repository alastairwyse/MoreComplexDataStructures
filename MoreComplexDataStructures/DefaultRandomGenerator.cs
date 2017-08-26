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
    /// An implementation of the IRandomIntegerGenerator interface, using the .NET System.Random class.
    /// </summary>
    public class DefaultRandomGenerator : IRandomIntegerGenerator
    {
        /// <summary>The underlying random number generator.</summary>
        private Random randomGenerator;

        /// <summary>
        /// Initialises a new instance of the MoreComplexDataStructures.DefaultRandomGenerator class.
        /// </summary>
        public DefaultRandomGenerator()
        {
            randomGenerator = new Random();
        }

        /// <include file='InterfaceDocumentationComments.xml' path='doc/members/member[@name="M:MoreComplexDataStructures.IRandomIntegerGenerator.Next(System.Int32)"]/*'/>
        public Int32 Next(Int32 maxValue)
        {
            return randomGenerator.Next(maxValue);
        }

        /// <include file='InterfaceDocumentationComments.xml' path='doc/members/member[@name="M:MoreComplexDataStructures.IRandomIntegerGenerator.Next(System.Int64)"]/*'/>
        public Int64 Next(Int64 maxValue)
        {
            // Using the method of generating an Int64 suggested in Microsoft's Random class documentation (https://msdn.microsoft.com/en-us/library/system.random(v=vs.110).aspx#Long).
            Int64 returnValue = Convert.ToInt64(randomGenerator.NextDouble() * maxValue);
            // Found from testing that occurrence of 0 and maxValue was about half of all other values, hence map maxValue back to 0 to give even distribution.
            if (returnValue == maxValue)
            {
                return 0;
            }
            return returnValue;
        }
    }
}
