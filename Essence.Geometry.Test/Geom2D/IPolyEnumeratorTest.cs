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

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Essence.Geometry.Core.Double;
using Essence.Util.Collections;

namespace Essence.Geometry.Geom2D
{
    [TestClass]
    public class IPolyEnumeratorTest
    {
        [TestMethod]
        public void Test1()
        {
            IList<Point2d> points = new[] { new Point2d(0, 0), new Point2d(10, 0), new Point2d(10, 10), new Point2d(0, 10) };
            PolyEnumerator<Point2d> enumer = new PolyEnumerator<Point2d>(points);
            Assert.AreEqual(enumer.Index, 0);

            enumer.Next();
            Assert.AreEqual(enumer.Index, 1);

            enumer.Next();
            Assert.AreEqual(enumer.Index, 2);

            enumer.Next();
            Assert.AreEqual(enumer.Index, 3);

            enumer.Next();
            Assert.AreEqual(enumer.Index, 0);

            enumer.Prev();
            Assert.AreEqual(enumer.Index, 3);

            enumer.Prev();
            Assert.AreEqual(enumer.Index, 2);

            enumer.Prev();
            Assert.AreEqual(enumer.Index, 1);

            enumer.Prev();
            Assert.AreEqual(enumer.Index, 0);
        }

        [TestMethod]
        public void TestRobust()
        {
            IList<Point2d> points = DuplicatePoints(new[] { new Point2d(0, 0), new Point2d(10, 0), new Point2d(10, 10), new Point2d(0, 10) });
            PolyEnumeratorRobust<Point2d> enumer = new PolyEnumeratorRobust<Point2d>(points);
            Assert.AreEqual(enumer.Index, 0);

            enumer.Next();
            Assert.AreEqual(enumer.Index, 2);

            enumer.Next();
            Assert.AreEqual(enumer.Index, 4);

            enumer.Next();
            Assert.AreEqual(enumer.Index, 6);

            enumer.Next();
            Assert.AreEqual(enumer.Index, 0);

            enumer.Prev();
            Assert.AreEqual(enumer.Index, 6);

            enumer.Prev();
            Assert.AreEqual(enumer.Index, 4);

            enumer.Prev();
            Assert.AreEqual(enumer.Index, 2);

            enumer.Prev();
            Assert.AreEqual(enumer.Index, 0);
        }

        [TestMethod]
        public void TestRobust2()
        {
            IList<Point2d> points = DuplicatePoints(new[] { new Point2d(0, 0), new Point2d(10, 0), new Point2d(10, 10), new Point2d(0, 10) });
            ListUtils.ShiftLeft(points, 1);

            PolyEnumeratorRobust<Point2d> enumer = new PolyEnumeratorRobust<Point2d>(points, 0, true);
            Assert.AreEqual(enumer.Index, 7);

            enumer.Next();
            Assert.AreEqual(enumer.Index, 1);

            enumer.Next();
            Assert.AreEqual(enumer.Index, 3);

            enumer.Next();
            Assert.AreEqual(enumer.Index, 5);

            enumer.Next();
            Assert.AreEqual(enumer.Index, 7);

            enumer.Prev();
            Assert.AreEqual(enumer.Index, 5);

            enumer.Prev();
            Assert.AreEqual(enumer.Index, 3);

            enumer.Prev();
            Assert.AreEqual(enumer.Index, 1);

            enumer.Prev();
            Assert.AreEqual(enumer.Index, 7);
        }

        private static IList<Point2d> DuplicatePoints(IList<Point2d> points)
        {
            List<Point2d> aux = new List<Point2d>();
            foreach (Point2d p in points)
            {
                aux.Add(p);
                aux.Add(p);
            }
            return aux;
        }
    }
}