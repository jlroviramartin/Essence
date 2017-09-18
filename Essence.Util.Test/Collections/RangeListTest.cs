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
    public class RangeListTest
    {
        [TestMethod]
        public void TestNormal()
        {
            IList<int> aux = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            AssertEqualsList(new RangeList<int>(aux, 0, 10),
                             new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });

            AssertEqualsList(new RangeList<int>(aux, 0, 1),
                             new[] { 0 });

            AssertEqualsList(new RangeList<int>(aux, 4, 6),
                             new[] { 4, 5, 6, 7, 8, 9 });

            AssertEqualsList(new RangeList<int>(aux, 7, 3),
                             new[] { 7, 8, 9 });

            try
            {
                new RangeList<int>(aux, 7, 4);
                Assert.Fail();
            }
            catch (Exception exception)
            {
            }
        }

        [TestMethod]
        public void TestCircular()
        {
            IList<int> aux = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            AssertEqualsList(new RangeList<int>(aux, 7, 4, true),
                             new[] { 7, 8, 9, 0 });

            AssertEqualsList(new RangeList<int>(aux, 7, 8, true),
                             new[] { 7, 8, 9, 0, 1, 2, 3, 4 });

            AssertEqualsList(new RangeList<int>(aux, 7, 10, true),
                             new[] { 7, 8, 9, 0, 1, 2, 3, 4, 5, 6 });

            try
            {
                new RangeList<int>(aux, 7, 11, true);
                Assert.Fail();
            }
            catch (Exception exception)
            {
            }
        }

        private static void AssertEqualsList<T>(IList<T> list1, IList<T> list2)
        {
            Assert.IsTrue(list1 != null && list2 != null);
            Assert.IsTrue(list1.Count == list2.Count);
            for (int i = 0; i < list1.Count; i++)
            {
                Assert.IsTrue(Equals(list1[i], list2[i]));
            }
        }
    }
}