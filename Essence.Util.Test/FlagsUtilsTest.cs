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

namespace Essence.Util
{
    [TestClass]
    public class FlagsUtilsTest
    {
        [TestMethod]
        public void Test1()
        {
            {
                List<ulong> decomposition = new List<ulong>();
                FlagsUtils.DecomposeFlag(0xffffffffffffffff, decomposition);

                Assert.IsTrue(decomposition.Count == 64);
                for (int i = 0; i < 64; i++)
                {
                    Assert.IsTrue(decomposition.Contains((ulong)System.Math.Pow(2, i)));
                }
            }

            {
                List<ulong> decomposition = new List<ulong>();
                FlagsUtils.DecomposeFlag(0x8000000000000000, decomposition);

                Assert.IsTrue(decomposition.Count == 1);
                Assert.IsTrue(decomposition.Contains((ulong)System.Math.Pow(2, 63)));
            }

            {
                List<ulong> decomposition = new List<ulong>();
                FlagsUtils.DecomposeFlag(0x80000000, decomposition);

                Assert.IsTrue(decomposition.Count == 1);
                Assert.IsTrue(decomposition.Contains((ulong)System.Math.Pow(2, 31)));
            }

            {
                List<ulong> decomposition = new List<ulong>();
                FlagsUtils.DecomposeFlag(0x8000, decomposition);

                Assert.IsTrue(decomposition.Count == 1);
                Assert.IsTrue(decomposition.Contains((ulong)System.Math.Pow(2, 15)));
            }

            {
                List<ulong> decomposition = new List<ulong>();
                FlagsUtils.DecomposeFlag(0x80, decomposition);

                Assert.IsTrue(decomposition.Count == 1);
                Assert.IsTrue(decomposition.Contains((ulong)System.Math.Pow(2, 7)));
            }

            {
                List<ulong> decomposition = new List<ulong>();
                FlagsUtils.DecomposeFlag(0x8, decomposition);

                Assert.IsTrue(decomposition.Count == 1);
                Assert.IsTrue(decomposition.Contains((ulong)System.Math.Pow(2, 3)));
            }

            {
                List<ulong> decomposition = new List<ulong>();
                FlagsUtils.DecomposeFlag(0x1, decomposition);

                Assert.IsTrue(decomposition.Count == 1);
                Assert.IsTrue(decomposition.Contains((ulong)System.Math.Pow(2, 0)));
            }

            {
                List<ulong> decomposition = new List<ulong>();
                FlagsUtils.DecomposeFlag(0x0, decomposition);

                Assert.IsTrue(decomposition.Count == 0);
            }

            for (int i = 0; i < 1000; i++)
            {
                List<ulong> expected = new List<ulong>();
                ulong value = this.Prepare(expected);

                List<ulong> decomposition = new List<ulong>();
                FlagsUtils.DecomposeFlag(value, decomposition);

                Assert.IsTrue(decomposition.Count == expected.Count);
                foreach (ulong t in expected)
                {
                    Assert.IsTrue(decomposition.Contains(t));
                }
            }
        }

        private ulong Prepare(List<ulong> result)
        {
            ulong value = 0;
            for (int i = 0; i < 64; i++)
            {
                if (rnd.NextBool())
                {
                    value |= ((ulong)1 << i);
                    result.Add((ulong)System.Math.Pow(2, i));
                }
            }
            return value;
        }

        private Random rnd = new Random();
    }
}