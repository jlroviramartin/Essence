// Copyright 2017 Jose Luis Rovira Martin
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Essence.Util.Collections
{
    [TestClass]
    public class CircularListTest
    {
        [TestMethod]
        public void Test1()
        {
            IList<int> aux = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            IList<int> circular = new CircularList<int>(aux);

            Assert.AreEqual(9, circular[-1]);
            Assert.AreEqual(8, circular[-2]);
            Assert.AreEqual(0, circular[-10]);

            Assert.AreEqual(1, circular[1]);
            Assert.AreEqual(2, circular[2]);
            Assert.AreEqual(0, circular[10]);

            for (int i = -100; i < 100; i++)
            {
                int value = i % aux.Count;
                if (value < 0)
                {
                    value += aux.Count;
                }
                Assert.AreEqual(value, circular[i]);
            }
        }
    }
}