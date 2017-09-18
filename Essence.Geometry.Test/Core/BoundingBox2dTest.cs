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

using Essence.Geometry.Core.Double;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Essence.Geometry.Core
{
    [TestClass]
    public class BoundingBox2dTest
    {
        [TestMethod]
        public void TestIntersect()
        {
            {
                BoundingBox2d bbox1 = new BoundingBox2d(0, 10, 0, 10);
                BoundingBox2d bbox2 = new BoundingBox2d(5, 15, 5, 15);
                Assert.IsTrue(bbox1.Intersect(bbox2).EpsilonEquals(new BoundingBox2d(5, 10, 5, 10)));
            }
            {
                BoundingBox2d bbox1 = new BoundingBox2d(0, 10, 0, 10);
                BoundingBox2d bbox2 = new BoundingBox2d(5, 7, 5, 7);
                Assert.IsTrue(bbox1.Intersect(bbox2).EpsilonEquals(new BoundingBox2d(5, 7, 5, 7)));
            }
            {
                BoundingBox2d bbox1 = new BoundingBox2d(0, 10, 0, 10);
                BoundingBox2d bbox2 = new BoundingBox2d(0, 10, 0, 10);
                Assert.IsTrue(bbox1.Intersect(bbox2).EpsilonEquals(new BoundingBox2d(0, 10, 0, 10)));
            }
            {
                BoundingBox2d bbox1 = new BoundingBox2d(0, 10, 0, 10);
                BoundingBox2d bbox2 = new BoundingBox2d(10, 20, 10, 20);
                Assert.IsTrue(bbox1.Intersect(bbox2).EpsilonEquals(new BoundingBox2d(10, 10, 10, 10)));
            }
            {
                BoundingBox2d bbox1 = new BoundingBox2d(0, 10, 0, 10);
                BoundingBox2d bbox2 = BoundingBox2d.Empty;
                Assert.IsTrue(bbox1.Intersect(bbox2).IsEmpty);
            }
            {
                BoundingBox2d bbox1 = BoundingBox2d.Empty;
                BoundingBox2d bbox2 = BoundingBox2d.Empty;
                Assert.IsTrue(bbox1.Intersect(bbox2).IsEmpty);
            }
            {
                BoundingBox2d bbox1 = new BoundingBox2d(0, double.MaxValue, 0, double.MaxValue);
                BoundingBox2d bbox2 = new BoundingBox2d(-double.MaxValue, 10, -double.MaxValue, 10);
                Assert.IsTrue(bbox1.Intersect(bbox2).EpsilonEquals(new BoundingBox2d(0, 10, 0, 10)));
            }
        }

        [TestMethod]
        public void TestUnion()
        {
            {
                BoundingBox2d bbox1 = new BoundingBox2d(0, 10, 0, 10);
                BoundingBox2d bbox2 = new BoundingBox2d(5, 15, 5, 15);
                Assert.IsTrue(bbox1.Union(bbox2).EpsilonEquals(new BoundingBox2d(0, 15, 0, 15)));
            }
            {
                BoundingBox2d bbox1 = new BoundingBox2d(0, 10, 0, 10);
                BoundingBox2d bbox2 = new BoundingBox2d(5, 7, 5, 7);
                Assert.IsTrue(bbox1.Union(bbox2).EpsilonEquals(new BoundingBox2d(0, 10, 0, 10)));
            }
            {
                BoundingBox2d bbox1 = new BoundingBox2d(0, 10, 0, 10);
                BoundingBox2d bbox2 = new BoundingBox2d(0, 10, 0, 10);
                Assert.IsTrue(bbox1.Union(bbox2).EpsilonEquals(new BoundingBox2d(0, 10, 0, 10)));
            }
            {
                BoundingBox2d bbox1 = new BoundingBox2d(0, 10, 0, 10);
                BoundingBox2d bbox2 = new BoundingBox2d(10, 20, 10, 20);
                Assert.IsTrue(bbox1.Union(bbox2).EpsilonEquals(new BoundingBox2d(0, 20, 0, 20)));
            }
            {
                BoundingBox2d bbox1 = new BoundingBox2d(0, 10, 0, 10);
                BoundingBox2d bbox2 = BoundingBox2d.Empty;
                Assert.IsTrue(bbox1.Union(bbox2).EpsilonEquals(new BoundingBox2d(0, 10, 0, 10)));
            }
            {
                BoundingBox2d bbox1 = BoundingBox2d.Empty;
                BoundingBox2d bbox2 = BoundingBox2d.Empty;
                Assert.IsTrue(bbox1.Union(bbox2).IsEmpty);
            }
            {
                BoundingBox2d bbox1 = new BoundingBox2d(0, double.MaxValue, 0, double.MaxValue);
                BoundingBox2d bbox2 = new BoundingBox2d(-double.MaxValue, 10, -double.MaxValue, 10);
                Assert.IsTrue(bbox1.Union(bbox2).EpsilonEquals(new BoundingBox2d(-double.MaxValue, double.MaxValue, -double.MaxValue, double.MaxValue)));
            }
        }

        [TestMethod]
        public void TestContains()
        {
            {
                BoundingBox2d bbox1 = new BoundingBox2d(0, 10, 0, 10);
                BoundingBox2d bbox2 = new BoundingBox2d(5, 15, 5, 15);
                Assert.IsFalse(bbox1.Contains(bbox2));
                Assert.IsFalse(bbox2.Contains(bbox1));
            }
            {
                BoundingBox2d bbox1 = new BoundingBox2d(0, 10, 0, 10);
                BoundingBox2d bbox2 = new BoundingBox2d(5, 7, 5, 7);
                Assert.IsTrue(bbox1.Contains(bbox2));
                Assert.IsFalse(bbox2.Contains(bbox1));
            }
            {
                BoundingBox2d bbox1 = new BoundingBox2d(0, 10, 0, 10);
                BoundingBox2d bbox2 = new BoundingBox2d(0, 10, 0, 10);
                Assert.IsTrue(bbox1.Contains(bbox2));
                Assert.IsTrue(bbox2.Contains(bbox1));
            }
            {
                BoundingBox2d bbox1 = new BoundingBox2d(0, 10, 0, 10);
                BoundingBox2d bbox2 = new BoundingBox2d(10, 20, 10, 20);
                Assert.IsFalse(bbox1.Contains(bbox2));
                Assert.IsFalse(bbox2.Contains(bbox1));
            }
            {
                BoundingBox2d bbox1 = new BoundingBox2d(0, 10, 0, 10);
                BoundingBox2d bbox2 = BoundingBox2d.Empty;
                Assert.IsFalse(bbox1.Contains(bbox2));
                Assert.IsFalse(bbox2.Contains(bbox1));
            }
            {
                BoundingBox2d bbox1 = BoundingBox2d.Empty;
                BoundingBox2d bbox2 = BoundingBox2d.Empty;
                Assert.IsFalse(bbox1.Contains(bbox2));
            }
            {
                BoundingBox2d bbox1 = new BoundingBox2d(0, double.MaxValue, 0, double.MaxValue);
                BoundingBox2d bbox2 = new BoundingBox2d(-double.MaxValue, 10, -double.MaxValue, 10);
                Assert.IsFalse(bbox1.Contains(bbox2));
                Assert.IsFalse(bbox2.Contains(bbox1));
            }
        }

        [TestMethod]
        public void TestIntersectsWith()
        {
            {
                BoundingBox2d bbox1 = new BoundingBox2d(0, 10, 0, 10);
                BoundingBox2d bbox2 = new BoundingBox2d(5, 15, 5, 15);
                Assert.IsTrue(bbox1.IntersectsWith(bbox2));
                Assert.IsTrue(bbox2.IntersectsWith(bbox1));
            }
            {
                BoundingBox2d bbox1 = new BoundingBox2d(0, 10, 0, 10);
                BoundingBox2d bbox2 = new BoundingBox2d(5, 7, 5, 7);
                Assert.IsTrue(bbox1.IntersectsWith(bbox2));
                Assert.IsTrue(bbox2.IntersectsWith(bbox1));
            }
            {
                BoundingBox2d bbox1 = new BoundingBox2d(0, 10, 0, 10);
                BoundingBox2d bbox2 = new BoundingBox2d(0, 10, 0, 10);
                Assert.IsTrue(bbox1.IntersectsWith(bbox2));
                Assert.IsTrue(bbox2.IntersectsWith(bbox1));
            }
            {
                BoundingBox2d bbox1 = new BoundingBox2d(0, 10, 0, 10);
                BoundingBox2d bbox2 = new BoundingBox2d(10, 20, 10, 20);
                Assert.IsTrue(bbox1.IntersectsWith(bbox2));
                Assert.IsTrue(bbox2.IntersectsWith(bbox1));
            }
            {
                BoundingBox2d bbox1 = new BoundingBox2d(0, 10, 0, 10);
                BoundingBox2d bbox2 = BoundingBox2d.Empty;
                Assert.IsFalse(bbox1.IntersectsWith(bbox2));
                Assert.IsFalse(bbox2.IntersectsWith(bbox1));
            }
            {
                BoundingBox2d bbox1 = BoundingBox2d.Empty;
                BoundingBox2d bbox2 = BoundingBox2d.Empty;
                Assert.IsFalse(bbox1.IntersectsWith(bbox2));
            }
            {
                BoundingBox2d bbox1 = new BoundingBox2d(0, double.MaxValue, 0, double.MaxValue);
                BoundingBox2d bbox2 = new BoundingBox2d(-double.MaxValue, 10, -double.MaxValue, 10);
                Assert.IsTrue(bbox1.IntersectsWith(bbox2));
                Assert.IsTrue(bbox2.IntersectsWith(bbox1));
            }
        }

        [TestMethod]
        public void TestInterseTouch()
        {
            {
                BoundingBox2d bbox1 = new BoundingBox2d(0, 10, 0, 10);
                BoundingBox2d bbox2 = new BoundingBox2d(5, 15, 5, 15);
                Assert.IsFalse(bbox1.Touch(bbox2));
                Assert.IsFalse(bbox2.Touch(bbox1));
            }
            {
                BoundingBox2d bbox1 = new BoundingBox2d(0, 10, 0, 10);
                BoundingBox2d bbox2 = new BoundingBox2d(5, 7, 5, 7);
                Assert.IsFalse(bbox1.Touch(bbox2));
                Assert.IsFalse(bbox2.Touch(bbox1));
            }
            {
                BoundingBox2d bbox1 = new BoundingBox2d(0, 10, 0, 10);
                BoundingBox2d bbox2 = new BoundingBox2d(0, 10, 0, 10);
                Assert.IsTrue(bbox1.Touch(bbox2));
                Assert.IsTrue(bbox2.Touch(bbox1));
            }
            {
                BoundingBox2d bbox1 = new BoundingBox2d(0, 10, 0, 10);
                BoundingBox2d bbox2 = new BoundingBox2d(10, 20, 10, 20);
                Assert.IsTrue(bbox1.Touch(bbox2));
                Assert.IsTrue(bbox2.Touch(bbox1));
            }
            {
                BoundingBox2d bbox1 = new BoundingBox2d(0, 10, 0, 10);
                BoundingBox2d bbox2 = BoundingBox2d.Empty;
                Assert.IsFalse(bbox1.Touch(bbox2));
                Assert.IsFalse(bbox2.Touch(bbox1));
            }
            {
                BoundingBox2d bbox1 = BoundingBox2d.Empty;
                BoundingBox2d bbox2 = BoundingBox2d.Empty;
                Assert.IsFalse(bbox1.Touch(bbox2));
            }
            {
                BoundingBox2d bbox1 = new BoundingBox2d(0, double.MaxValue, 0, double.MaxValue);
                BoundingBox2d bbox2 = new BoundingBox2d(-double.MaxValue, 10, -double.MaxValue, 10);
                Assert.IsFalse(bbox1.Touch(bbox2));
                Assert.IsFalse(bbox2.Touch(bbox1));
            }
        }
    }
}