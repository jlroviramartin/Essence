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
using Essence.Geometry.Core.Double;
using Essence.Util.Math;
using Essence.Util.Math.Double;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Essence.Geometry.Curves;

namespace Essence.Geometry.Geom2D
{
    [TestClass]
    public class PolygonUtilsTest
    {
        [TestMethod]
        public void TestNormalize()
        {
            IList<Point2d> points1 = new[]
            {
                new Point2d(10, 0),
                new Point2d(10, 10),
                new Point2d(0, 10),
                new Point2d(0, 0)
            };
            PolygonUtils.Normalize(points1);
            AssertEqualsList(points1, AsList(new Point2d(0, 0),
                                             new Point2d(10, 0),
                                             new Point2d(10, 10),
                                             new Point2d(0, 10)));

            IList<Point2d> points2 = AsList(new Point2d(10, 10),
                                            new Point2d(0, 10),
                                            new Point2d(0, 0),
                                            new Point2d(10, 0));
            PolygonUtils.Normalize(points2);
            AssertEqualsList(points2, AsList(new Point2d(0, 0),
                                             new Point2d(10, 0),
                                             new Point2d(10, 10),
                                             new Point2d(0, 10)));

            // Puntos repetidos.
            IList<Point2d> points3 = AsList(new Point2d(10, 0), new Point2d(10, 0),
                                            new Point2d(10, 10), new Point2d(10, 10),
                                            new Point2d(0, 10), new Point2d(0, 10),
                                            new Point2d(0, 0), new Point2d(0, 0));
            PolygonUtils.Normalize(points3);
            AssertEqualsList(points3, AsList(new Point2d(0, 0), new Point2d(0, 0),
                                             new Point2d(10, 0), new Point2d(10, 0),
                                             new Point2d(10, 10), new Point2d(10, 10),
                                             new Point2d(0, 10), new Point2d(0, 10)));

            IList<Point2d> points4 = AsList(new Point2d(10, 10), new Point2d(10, 10),
                                            new Point2d(0, 10), new Point2d(0, 10),
                                            new Point2d(0, 0), new Point2d(0, 0),
                                            new Point2d(10, 0), new Point2d(10, 0));
            PolygonUtils.Normalize(points4);
            AssertEqualsList(points4, AsList(new Point2d(0, 0), new Point2d(0, 0),
                                             new Point2d(10, 0), new Point2d(10, 0),
                                             new Point2d(10, 10), new Point2d(10, 10),
                                             new Point2d(0, 10), new Point2d(0, 10)));
        }

        [TestMethod]
        public void TestPointInEdge()
        {
            IList<Point2d> points = AsList(new Point2d(10, 0),
                                           new Point2d(10, 10),
                                           new Point2d(0, 10),
                                           new Point2d(0, 0));

            Assert.IsTrue(PolygonUtils.PointInEdge(points, new Point2d(5, 0), true, MathUtils.EPSILON));
            Assert.IsTrue(PolygonUtils.PointInEdge(points, new Point2d(10, 5), true, MathUtils.EPSILON));
            Assert.IsTrue(PolygonUtils.PointInEdge(points, new Point2d(5, 10), true, MathUtils.EPSILON));
            Assert.IsTrue(PolygonUtils.PointInEdge(points, new Point2d(0, 5), true, MathUtils.EPSILON));

            Assert.IsFalse(PolygonUtils.PointInEdge(points, new Point2d(5, 5), true, MathUtils.EPSILON));
            Assert.IsFalse(PolygonUtils.PointInEdge(points, new Point2d(5, 15), true, MathUtils.EPSILON));
            Assert.IsFalse(PolygonUtils.PointInEdge(points, new Point2d(5, -5), true, MathUtils.EPSILON));

            // Using epsilon.
            Assert.IsFalse(PolygonUtils.PointInEdge(points, new Point2d(5, 5), true, 4.9));
            Assert.IsTrue(PolygonUtils.PointInEdge(points, new Point2d(5, 5), true, 5));
        }

        [TestMethod]
        public void TestPointInPolyEvenOddExtended()
        {
            Circle2 c = new Circle2(new Point2d(0, 0), 5);
            double tmin = c.TMin;
            double tmax = c.TMax;
            double tinc = (tmax - tmin) / 5;
            Point2d p0 = c.GetPosition(tmin);
            Point2d p1 = c.GetPosition(tmin + tinc);
            Point2d p2 = c.GetPosition(tmin + 2 * tinc);
            Point2d p3 = c.GetPosition(tmin + 3 * tinc);
            Point2d p4 = c.GetPosition(tmin + 4 * tinc);

            IList<Point2d> points = AsList(p0, p2, p4, p1, p3);

            PointInPoly pip1 = PolygonUtils.PointInPolyEvenOdd(points, new Point2d(4, 0), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.Inside, pip1);

            PointInPoly pip2 = PolygonUtils.PointInPolyEvenOdd(points, new Point2d(-2, 2), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.Inside, pip2);

            PointInPoly pip3 = PolygonUtils.PointInPolyEvenOdd(points, new Point2d(-2, -2), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.Inside, pip3);

            PointInPoly pip4 = PolygonUtils.PointInPolyEvenOdd(points, new Point2d(0, 0), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.Outside, pip4);

            PointInPoly pip5 = PolygonUtils.PointInPolyEvenOdd(points, new Point2d(10, 0), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.Outside, pip5);

            PointInPoly pip6 = PolygonUtils.PointInPolyEvenOdd(points, new Point2d(4, 3), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.Outside, pip6);

            PointInPoly pip7 = PolygonUtils.PointInPolyEvenOdd(points, new Point2d(4, -3), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.Outside, pip7);

            PointInPoly pip8 = PolygonUtils.PointInPolyEvenOdd(points, new Point2d(1, 1), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.Outside, pip8);

            PointInPoly pip9 = PolygonUtils.PointInPolyEvenOdd(points, new Point2d(1, -1), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.Outside, pip9);

            PointInPoly pip10 = PolygonUtils.PointInPolyEvenOdd(points, p4.Add(p1.Sub(p4).Unit.Mul(0.7)), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.On, pip10);

            PointInPoly pip11 = PolygonUtils.PointInPolyEvenOdd(points, p4.Add(p1.Sub(p4).Unit.Mul(0.3)), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.On, pip11);

            PointInPoly pip12 = PolygonUtils.PointInPolyEvenOdd(points, p1.Add(p3.Sub(p1).Unit.Mul(0.7)), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.On, pip12);

            PointInPoly pip13 = PolygonUtils.PointInPolyEvenOdd(points, p1.Add(p3.Sub(p1).Unit.Mul(0.3)), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.On, pip13);
        }

        [TestMethod]
        public void TestPointInPolyEvenOddExtendedDuplicatePoints()
        {
            Circle2 c = new Circle2(new Point2d(0, 0), 5);
            double tmin = c.TMin;
            double tmax = c.TMax;
            double tinc = (tmax - tmin) / 5;
            Point2d p0 = c.GetPosition(tmin);
            Point2d p1 = c.GetPosition(tmin + tinc);
            Point2d p2 = c.GetPosition(tmin + 2 * tinc);
            Point2d p3 = c.GetPosition(tmin + 3 * tinc);
            Point2d p4 = c.GetPosition(tmin + 4 * tinc);

            IList<Point2d> points = DuplicatePoints(AsList(p0, p2, p4, p1, p3));

            PointInPoly pip1 = PolygonUtils.PointInPolyEvenOdd(points, new Point2d(4, 0), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.Inside, pip1);

            PointInPoly pip2 = PolygonUtils.PointInPolyEvenOdd(points, new Point2d(-2, 2), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.Inside, pip2);

            PointInPoly pip3 = PolygonUtils.PointInPolyEvenOdd(points, new Point2d(-2, -2), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.Inside, pip3);

            PointInPoly pip4 = PolygonUtils.PointInPolyEvenOdd(points, new Point2d(0, 0), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.Outside, pip4);

            PointInPoly pip5 = PolygonUtils.PointInPolyEvenOdd(points, new Point2d(10, 0), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.Outside, pip5);

            PointInPoly pip6 = PolygonUtils.PointInPolyEvenOdd(points, new Point2d(4, 3), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.Outside, pip6);

            PointInPoly pip7 = PolygonUtils.PointInPolyEvenOdd(points, new Point2d(4, -3), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.Outside, pip7);

            PointInPoly pip8 = PolygonUtils.PointInPolyEvenOdd(points, new Point2d(1, 1), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.Outside, pip8);

            PointInPoly pip9 = PolygonUtils.PointInPolyEvenOdd(points, new Point2d(1, -1), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.Outside, pip9);

            PointInPoly pip10 = PolygonUtils.PointInPolyEvenOdd(points, p4.Add(p1.Sub(p4).Unit.Mul(0.7)), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.On, pip10);

            PointInPoly pip11 = PolygonUtils.PointInPolyEvenOdd(points, p4.Add(p1.Sub(p4).Unit.Mul(0.3)), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.On, pip11);

            PointInPoly pip12 = PolygonUtils.PointInPolyEvenOdd(points, p1.Add(p3.Sub(p1).Unit.Mul(0.7)), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.On, pip12);

            PointInPoly pip13 = PolygonUtils.PointInPolyEvenOdd(points, p1.Add(p3.Sub(p1).Unit.Mul(0.3)), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.On, pip13);
        }

        [TestMethod]
        public void TestPointInPolyNonZeroExtended()
        {
            Circle2 c = new Circle2(new Point2d(0, 0), 5);
            double tmin = c.TMin;
            double tmax = c.TMax;
            double tinc = (tmax - tmin) / 5;
            Point2d p0 = c.GetPosition(tmin);
            Point2d p1 = c.GetPosition(tmin + tinc);
            Point2d p2 = c.GetPosition(tmin + 2 * tinc);
            Point2d p3 = c.GetPosition(tmin + 3 * tinc);
            Point2d p4 = c.GetPosition(tmin + 4 * tinc);

            IList<Point2d> points = AsList(p0, p2, p4, p1, p3);

            PointInPoly pip1 = PolygonUtils.PointInPolyNonZero(points, new Point2d(4, 0), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.Inside, pip1);

            PointInPoly pip2 = PolygonUtils.PointInPolyNonZero(points, new Point2d(-2, 2), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.Inside, pip2);

            PointInPoly pip3 = PolygonUtils.PointInPolyNonZero(points, new Point2d(-2, -2), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.Inside, pip3);

            PointInPoly pip4 = PolygonUtils.PointInPolyNonZero(points, new Point2d(0, 0), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.Inside, pip4);

            PointInPoly pip5 = PolygonUtils.PointInPolyNonZero(points, new Point2d(10, 0), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.Outside, pip5);

            PointInPoly pip6 = PolygonUtils.PointInPolyNonZero(points, new Point2d(4, 3), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.Outside, pip6);

            PointInPoly pip7 = PolygonUtils.PointInPolyNonZero(points, new Point2d(4, -3), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.Outside, pip7);

            PointInPoly pip8 = PolygonUtils.PointInPolyNonZero(points, new Point2d(1, 1), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.Inside, pip8);

            PointInPoly pip9 = PolygonUtils.PointInPolyNonZero(points, new Point2d(1, -1), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.Inside, pip9);

            PointInPoly pip10 = PolygonUtils.PointInPolyNonZero(points, p4.Add(p1.Sub(p4).Unit.Mul(0.7)), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.On, pip10);

            PointInPoly pip11 = PolygonUtils.PointInPolyNonZero(points, p4.Add(p1.Sub(p4).Unit.Mul(0.3)), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.On, pip11);

            PointInPoly pip12 = PolygonUtils.PointInPolyNonZero(points, p1.Add(p3.Sub(p1).Unit.Mul(0.7)), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.On, pip12);

            PointInPoly pip13 = PolygonUtils.PointInPolyNonZero(points, p1.Add(p3.Sub(p1).Unit.Mul(0.3)), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.On, pip13);
        }

        [TestMethod]
        public void TestPointInPolyNonZeroExtendedDuplicatePoints()
        {
            Circle2 c = new Circle2(new Point2d(0, 0), 5);
            double tmin = c.TMin;
            double tmax = c.TMax;
            double tinc = (tmax - tmin) / 5;
            Point2d p0 = c.GetPosition(tmin);
            Point2d p1 = c.GetPosition(tmin + tinc);
            Point2d p2 = c.GetPosition(tmin + 2 * tinc);
            Point2d p3 = c.GetPosition(tmin + 3 * tinc);
            Point2d p4 = c.GetPosition(tmin + 4 * tinc);

            IList<Point2d> points = DuplicatePoints(AsList(p0, p2, p4, p1, p3));

            PointInPoly pip1 = PolygonUtils.PointInPolyNonZero(points, new Point2d(4, 0), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.Inside, pip1);

            PointInPoly pip2 = PolygonUtils.PointInPolyNonZero(points, new Point2d(-2, 2), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.Inside, pip2);

            PointInPoly pip3 = PolygonUtils.PointInPolyNonZero(points, new Point2d(-2, -2), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.Inside, pip3);

            PointInPoly pip4 = PolygonUtils.PointInPolyNonZero(points, new Point2d(0, 0), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.Inside, pip4);

            PointInPoly pip5 = PolygonUtils.PointInPolyNonZero(points, new Point2d(10, 0), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.Outside, pip5);

            PointInPoly pip6 = PolygonUtils.PointInPolyNonZero(points, new Point2d(4, 3), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.Outside, pip6);

            PointInPoly pip7 = PolygonUtils.PointInPolyNonZero(points, new Point2d(4, -3), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.Outside, pip7);

            PointInPoly pip8 = PolygonUtils.PointInPolyNonZero(points, new Point2d(1, 1), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.Inside, pip8);

            PointInPoly pip9 = PolygonUtils.PointInPolyNonZero(points, new Point2d(1, -1), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.Inside, pip9);

            PointInPoly pip10 = PolygonUtils.PointInPolyNonZero(points, p4.Add(p1.Sub(p4).Unit.Mul(0.7)), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.On, pip10);

            PointInPoly pip11 = PolygonUtils.PointInPolyNonZero(points, p4.Add(p1.Sub(p4).Unit.Mul(0.3)), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.On, pip11);

            PointInPoly pip12 = PolygonUtils.PointInPolyNonZero(points, p1.Add(p3.Sub(p1).Unit.Mul(0.7)), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.On, pip12);

            PointInPoly pip13 = PolygonUtils.PointInPolyNonZero(points, p1.Add(p3.Sub(p1).Unit.Mul(0.3)), true, true, MathUtils.EPSILON);
            Assert.AreEqual(PointInPoly.On, pip13);
        }

        [TestMethod]
        public void TestPointInPolyEvenOddDefault()
        {
            IList<Point2d> points1 = AsList(new Point2d(0, 0), new Point2d(10, 10), new Point2d(0, 10));
            IList<Point2d> points2 = AsList(new Point2d(0, 0), new Point2d(10, 0), new Point2d(10, 10));

            Assert.AreEqual(PointInPoly.Outside, PolygonUtils.PointInPolyEvenOdd(points1, new Point2d(5, 10), false, true, MathUtils.EPSILON));
            Assert.AreEqual(PointInPoly.Inside, PolygonUtils.PointInPolyEvenOdd(points1, new Point2d(0, 5), false, true, MathUtils.EPSILON));
            Assert.AreEqual(PointInPoly.Outside, PolygonUtils.PointInPolyEvenOdd(points1, new Point2d(5, 5), false, true, MathUtils.EPSILON));

            Assert.AreEqual(PointInPoly.Inside, PolygonUtils.PointInPolyEvenOdd(points2, new Point2d(5, 0), false, true, MathUtils.EPSILON));
            Assert.AreEqual(PointInPoly.Outside, PolygonUtils.PointInPolyEvenOdd(points2, new Point2d(10, 5), false, true, MathUtils.EPSILON));
            Assert.AreEqual(PointInPoly.Inside, PolygonUtils.PointInPolyEvenOdd(points2, new Point2d(5, 5), false, true, MathUtils.EPSILON));

            IList<Point2d> points3 = AsList(new Point2d(0, 0), new Point2d(10, 0), new Point2d(10, 10), new Point2d(0, 10));
            IList<Point2d> points4 = AsList(new Point2d(0, 10), new Point2d(10, 10), new Point2d(10, 20), new Point2d(0, 20));
            IList<Point2d> points5 = AsList(new Point2d(10, 0), new Point2d(20, 0), new Point2d(20, 10), new Point2d(10, 10));
            IList<Point2d> points6 = AsList(new Point2d(10, 10), new Point2d(20, 10), new Point2d(20, 20), new Point2d(10, 20));

            Assert.AreEqual(PointInPoly.Outside, PolygonUtils.PointInPolyEvenOdd(points3, new Point2d(5, 10), false, true, MathUtils.EPSILON));
            Assert.AreEqual(PointInPoly.Inside, PolygonUtils.PointInPolyEvenOdd(points4, new Point2d(5, 10), false, true, MathUtils.EPSILON));

            Assert.AreEqual(PointInPoly.Outside, PolygonUtils.PointInPolyEvenOdd(points5, new Point2d(15, 10), false, true, MathUtils.EPSILON));
            Assert.AreEqual(PointInPoly.Inside, PolygonUtils.PointInPolyEvenOdd(points6, new Point2d(15, 10), false, true, MathUtils.EPSILON));

            Assert.AreEqual(PointInPoly.Outside, PolygonUtils.PointInPolyEvenOdd(points3, new Point2d(10, 5), false, true, MathUtils.EPSILON));
            Assert.AreEqual(PointInPoly.Inside, PolygonUtils.PointInPolyEvenOdd(points5, new Point2d(10, 5), false, true, MathUtils.EPSILON));

            Assert.AreEqual(PointInPoly.Outside, PolygonUtils.PointInPolyEvenOdd(points4, new Point2d(10, 15), false, true, MathUtils.EPSILON));
            Assert.AreEqual(PointInPoly.Inside, PolygonUtils.PointInPolyEvenOdd(points6, new Point2d(10, 15), false, true, MathUtils.EPSILON));

            Assert.AreEqual(PointInPoly.Outside, PolygonUtils.PointInPolyEvenOdd(points3, new Point2d(10, 10), false, true, MathUtils.EPSILON));
            Assert.AreEqual(PointInPoly.Outside, PolygonUtils.PointInPolyEvenOdd(points5, new Point2d(10, 10), false, true, MathUtils.EPSILON));

            Assert.AreEqual(PointInPoly.Outside, PolygonUtils.PointInPolyEvenOdd(points4, new Point2d(10, 10), false, true, MathUtils.EPSILON));
            Assert.AreEqual(PointInPoly.Inside, PolygonUtils.PointInPolyEvenOdd(points6, new Point2d(10, 10), false, true, MathUtils.EPSILON));
        }

        [TestMethod]
        public void TestPointInPolyEvenOddDefaultDuplicatePoints()
        {
            IList<Point2d> points1 = DuplicatePoints(AsList(new Point2d(0, 0), new Point2d(10, 10), new Point2d(0, 10)));
            IList<Point2d> points2 = DuplicatePoints(AsList(new Point2d(0, 0), new Point2d(10, 0), new Point2d(10, 10)));

            Assert.AreEqual(PointInPoly.Outside, PolygonUtils.PointInPolyEvenOdd(points1, new Point2d(5, 10), false, true, MathUtils.EPSILON));
            Assert.AreEqual(PointInPoly.Inside, PolygonUtils.PointInPolyEvenOdd(points1, new Point2d(0, 5), false, true, MathUtils.EPSILON));
            Assert.AreEqual(PointInPoly.Outside, PolygonUtils.PointInPolyEvenOdd(points1, new Point2d(5, 5), false, true, MathUtils.EPSILON));

            Assert.AreEqual(PointInPoly.Inside, PolygonUtils.PointInPolyEvenOdd(points2, new Point2d(5, 0), false, true, MathUtils.EPSILON));
            Assert.AreEqual(PointInPoly.Outside, PolygonUtils.PointInPolyEvenOdd(points2, new Point2d(10, 5), false, true, MathUtils.EPSILON));
            Assert.AreEqual(PointInPoly.Inside, PolygonUtils.PointInPolyEvenOdd(points2, new Point2d(5, 5), false, true, MathUtils.EPSILON));

            IList<Point2d> points3 = DuplicatePoints(AsList(new Point2d(0, 0), new Point2d(10, 0), new Point2d(10, 10), new Point2d(0, 10)));
            IList<Point2d> points4 = DuplicatePoints(AsList(new Point2d(0, 10), new Point2d(10, 10), new Point2d(10, 20), new Point2d(0, 20)));
            IList<Point2d> points5 = DuplicatePoints(AsList(new Point2d(10, 0), new Point2d(20, 0), new Point2d(20, 10), new Point2d(10, 10)));
            IList<Point2d> points6 = DuplicatePoints(AsList(new Point2d(10, 10), new Point2d(20, 10), new Point2d(20, 20), new Point2d(10, 20)));

            Assert.AreEqual(PointInPoly.Outside, PolygonUtils.PointInPolyEvenOdd(points3, new Point2d(5, 10), false, true, MathUtils.EPSILON));
            Assert.AreEqual(PointInPoly.Inside, PolygonUtils.PointInPolyEvenOdd(points4, new Point2d(5, 10), false, true, MathUtils.EPSILON));

            Assert.AreEqual(PointInPoly.Outside, PolygonUtils.PointInPolyEvenOdd(points5, new Point2d(15, 10), false, true, MathUtils.EPSILON));
            Assert.AreEqual(PointInPoly.Inside, PolygonUtils.PointInPolyEvenOdd(points6, new Point2d(15, 10), false, true, MathUtils.EPSILON));

            Assert.AreEqual(PointInPoly.Outside, PolygonUtils.PointInPolyEvenOdd(points3, new Point2d(10, 5), false, true, MathUtils.EPSILON));
            Assert.AreEqual(PointInPoly.Inside, PolygonUtils.PointInPolyEvenOdd(points5, new Point2d(10, 5), false, true, MathUtils.EPSILON));

            Assert.AreEqual(PointInPoly.Outside, PolygonUtils.PointInPolyEvenOdd(points4, new Point2d(10, 15), false, true, MathUtils.EPSILON));
            Assert.AreEqual(PointInPoly.Inside, PolygonUtils.PointInPolyEvenOdd(points6, new Point2d(10, 15), false, true, MathUtils.EPSILON));

            Assert.AreEqual(PointInPoly.Outside, PolygonUtils.PointInPolyEvenOdd(points3, new Point2d(10, 10), false, true, MathUtils.EPSILON));
            Assert.AreEqual(PointInPoly.Outside, PolygonUtils.PointInPolyEvenOdd(points5, new Point2d(10, 10), false, true, MathUtils.EPSILON));

            Assert.AreEqual(PointInPoly.Outside, PolygonUtils.PointInPolyEvenOdd(points4, new Point2d(10, 10), false, true, MathUtils.EPSILON));
            Assert.AreEqual(PointInPoly.Inside, PolygonUtils.PointInPolyEvenOdd(points6, new Point2d(10, 10), false, true, MathUtils.EPSILON));
        }

        [TestMethod]
        public void TestPointInPolyNonZeroDefault()
        {
            IList<Point2d> points1 = AsList(new Point2d(0, 0), new Point2d(10, 10), new Point2d(0, 10));
            IList<Point2d> points2 = AsList(new Point2d(0, 0), new Point2d(10, 0), new Point2d(10, 10));

            Assert.AreEqual(PointInPoly.Outside, PolygonUtils.PointInPolyNonZero(points1, new Point2d(5, 10), false, true, MathUtils.EPSILON));
            Assert.AreEqual(PointInPoly.Inside, PolygonUtils.PointInPolyNonZero(points1, new Point2d(0, 5), false, true, MathUtils.EPSILON));
            Assert.AreEqual(PointInPoly.Outside, PolygonUtils.PointInPolyNonZero(points1, new Point2d(5, 5), false, true, MathUtils.EPSILON));

            Assert.AreEqual(PointInPoly.Inside, PolygonUtils.PointInPolyNonZero(points2, new Point2d(5, 0), false, true, MathUtils.EPSILON));
            Assert.AreEqual(PointInPoly.Outside, PolygonUtils.PointInPolyNonZero(points2, new Point2d(10, 5), false, true, MathUtils.EPSILON));
            Assert.AreEqual(PointInPoly.Inside, PolygonUtils.PointInPolyNonZero(points2, new Point2d(5, 5), false, true, MathUtils.EPSILON));

            IList<Point2d> points3 = AsList(new Point2d(0, 0), new Point2d(10, 0), new Point2d(10, 10), new Point2d(0, 10));
            IList<Point2d> points4 = AsList(new Point2d(0, 10), new Point2d(10, 10), new Point2d(10, 20), new Point2d(0, 20));
            IList<Point2d> points5 = AsList(new Point2d(10, 0), new Point2d(20, 0), new Point2d(20, 10), new Point2d(10, 10));
            IList<Point2d> points6 = AsList(new Point2d(10, 10), new Point2d(20, 10), new Point2d(20, 20), new Point2d(10, 20));

            Assert.AreEqual(PointInPoly.Outside, PolygonUtils.PointInPolyNonZero(points3, new Point2d(5, 10), false, true, MathUtils.EPSILON));
            Assert.AreEqual(PointInPoly.Inside, PolygonUtils.PointInPolyNonZero(points4, new Point2d(5, 10), false, true, MathUtils.EPSILON));

            Assert.AreEqual(PointInPoly.Outside, PolygonUtils.PointInPolyNonZero(points5, new Point2d(15, 10), false, true, MathUtils.EPSILON));
            Assert.AreEqual(PointInPoly.Inside, PolygonUtils.PointInPolyNonZero(points6, new Point2d(15, 10), false, true, MathUtils.EPSILON));

            Assert.AreEqual(PointInPoly.Outside, PolygonUtils.PointInPolyNonZero(points3, new Point2d(10, 5), false, true, MathUtils.EPSILON));
            Assert.AreEqual(PointInPoly.Inside, PolygonUtils.PointInPolyNonZero(points5, new Point2d(10, 5), false, true, MathUtils.EPSILON));

            Assert.AreEqual(PointInPoly.Outside, PolygonUtils.PointInPolyNonZero(points4, new Point2d(10, 15), false, true, MathUtils.EPSILON));
            Assert.AreEqual(PointInPoly.Inside, PolygonUtils.PointInPolyNonZero(points6, new Point2d(10, 15), false, true, MathUtils.EPSILON));

            Assert.AreEqual(PointInPoly.Outside, PolygonUtils.PointInPolyNonZero(points3, new Point2d(10, 10), false, true, MathUtils.EPSILON));
            Assert.AreEqual(PointInPoly.Outside, PolygonUtils.PointInPolyNonZero(points5, new Point2d(10, 10), false, true, MathUtils.EPSILON));

            Assert.AreEqual(PointInPoly.Outside, PolygonUtils.PointInPolyNonZero(points4, new Point2d(10, 10), false, true, MathUtils.EPSILON));
            Assert.AreEqual(PointInPoly.Inside, PolygonUtils.PointInPolyNonZero(points6, new Point2d(10, 10), false, true, MathUtils.EPSILON));
        }

        [TestMethod]
        public void TestTestOrientation()
        {
            IList<Point2d> points1 = AsList(new Point2d(0, 0), new Point2d(10, 0), new Point2d(10, 10), new Point2d(0, 10));
            IList<Point2d> points2 = AsList(new Point2d(0, 0), new Point2d(0, 10), new Point2d(10, 10), new Point2d(10, 0));
            Assert.AreEqual(Orientation.CCW, PolygonUtils.TestOrientation(points1, false));
            Assert.AreEqual(Orientation.CW, PolygonUtils.TestOrientation(points2, false));
        }

        [TestMethod]
        public void TestOrientationRobust()
        {
            IList<Point2d> points1 = AsList(new Point2d(0, 0), new Point2d(10, 0), new Point2d(10, 10), new Point2d(0, 10));
            IList<Point2d> points2 = AsList(new Point2d(0, 0), new Point2d(0, 10), new Point2d(10, 10), new Point2d(10, 0));
            Assert.AreEqual(Orientation.CCW, PolygonUtils.TestOrientation(points1, true));
            Assert.AreEqual(Orientation.CW, PolygonUtils.TestOrientation(points2, true));

            IList<Point2d> points3 = DuplicatePoints(points1);
            IList<Point2d> points4 = DuplicatePoints(points2);
            Assert.AreEqual(Orientation.CCW, PolygonUtils.TestOrientation(points3, true));
            Assert.AreEqual(Orientation.CW, PolygonUtils.TestOrientation(points4, true));
        }

        [TestMethod]
        public void TestSignedArea()
        {
            IList<Point2d> points1 = AsList(new Point2d(0, 0), new Point2d(10, 0), new Point2d(10, 10), new Point2d(0, 10));
            IList<Point2d> points2 = AsList(new Point2d(0, 0), new Point2d(0, 10), new Point2d(10, 10), new Point2d(10, 0));
            Assert.AreEqual(100d, PolygonUtils.SignedArea(points1), MathUtils.EPSILON);
            Assert.AreEqual(-100d, PolygonUtils.SignedArea(points2), MathUtils.EPSILON);
        }

        [TestMethod]
        public void TestSignedArea2()
        {
            IList<Point2d> points1 = AsList(new Point2d(0, 0), new Point2d(10, 0), new Point2d(10, 10), new Point2d(0, 10));
            IList<Point2d> points2 = AsList(new Point2d(0, 0), new Point2d(0, 10), new Point2d(10, 10), new Point2d(10, 0));
            Assert.AreEqual(100d, PolygonUtils.SignedArea2(points1), MathUtils.EPSILON);
            Assert.AreEqual(-100d, PolygonUtils.SignedArea2(points2), MathUtils.EPSILON);
        }

        [TestMethod]
        public void TestIsConvex()
        {
            IList<Point2d> points1 = AsList(new Point2d(0, 0), new Point2d(10, 0), new Point2d(10, 10), new Point2d(0, 10));
            IList<Point2d> points2 = AsList(new Point2d(0, 0), new Point2d(0, 10), new Point2d(10, 10), new Point2d(10, 0));
            Assert.AreEqual(true, PolygonUtils.IsConvex(points1, false));
            Assert.AreEqual(true, PolygonUtils.IsConvex(points2, false));

            IList<Point2d> points3 = AsList(new Point2d(0, 0), new Point2d(10, 0), new Point2d(10, 10), new Point2d(5, 10), new Point2d(5, 5), new Point2d(0, 5));
            IList<Point2d> points4 = AsList(new Point2d(0, 0), new Point2d(0, 10), new Point2d(10, 10), new Point2d(10, 5), new Point2d(5, 5), new Point2d(5, 0));
            Assert.AreEqual(false, PolygonUtils.IsConvex(points3, false));
            Assert.AreEqual(false, PolygonUtils.IsConvex(points4, false));
        }

        [TestMethod]
        public void TestIsConvexRobust()
        {
            IList<Point2d> points1 = AsList(new Point2d(0, 0), new Point2d(10, 0), new Point2d(10, 10), new Point2d(0, 10));
            IList<Point2d> points2 = AsList(new Point2d(0, 0), new Point2d(0, 10), new Point2d(10, 10), new Point2d(10, 0));
            Assert.AreEqual(true, PolygonUtils.IsConvex(points1, true));
            Assert.AreEqual(true, PolygonUtils.IsConvex(points2, true));

            IList<Point2d> points3 = AsList(new Point2d(0, 0), new Point2d(10, 0), new Point2d(10, 10), new Point2d(5, 10), new Point2d(5, 5), new Point2d(0, 5));
            IList<Point2d> points4 = AsList(new Point2d(0, 0), new Point2d(0, 10), new Point2d(10, 10), new Point2d(10, 5), new Point2d(5, 5), new Point2d(5, 0));
            Assert.AreEqual(false, PolygonUtils.IsConvex(points3, true));
            Assert.AreEqual(false, PolygonUtils.IsConvex(points4, true));

            IList<Point2d> points5 = DuplicatePoints(points1);
            IList<Point2d> points6 = DuplicatePoints(points2);
            Assert.AreEqual(true, PolygonUtils.IsConvex(points5, true));
            Assert.AreEqual(true, PolygonUtils.IsConvex(points6, true));

            IList<Point2d> points7 = DuplicatePoints(points3);
            IList<Point2d> points8 = DuplicatePoints(points4);
            Assert.AreEqual(false, PolygonUtils.IsConvex(points7, true));
            Assert.AreEqual(false, PolygonUtils.IsConvex(points8, true));
        }

        [TestMethod]
        public void TestSort()
        {
            IList<Point2d> points1 = AsList(new Point2d(0, 0), new Point2d(5, 0), new Point2d(10, 0),
                                            new Point2d(10, 10), new Point2d(15, 10), new Point2d(20, 10),
                                            new Point2d(20, 20), new Point2d(25, 20), new Point2d(30, 20));

            AssertEqualsListList(PolygonUtils.Sort2(points1),
                                 new[]
                                 {
                                     new[]
                                     {
                                         new Point2d(0, 0), new Point2d(5, 0), new Point2d(10, 0),
                                         new Point2d(10, 10), new Point2d(15, 10), new Point2d(20, 10),
                                         new Point2d(20, 20), new Point2d(25, 20), new Point2d(30, 20)
                                     },
                                     new[]
                                     {
                                         new Point2d(30, 20), new Point2d(0, 0)
                                     }
                                 });

            IList<Point2d> points2 = AsList(new Point2d(0, 0), new Point2d(5, 0), new Point2d(10, 0),
                                            new Point2d(10, 10), new Point2d(15, 10), new Point2d(20, 10),
                                            new Point2d(20, 20), new Point2d(25, 20), new Point2d(30, 20),
                                            new Point2d(30, 10), new Point2d(35, 10), new Point2d(40, 10),
                                            new Point2d(40, 0), new Point2d(45, 0), new Point2d(50, 0));

            AssertEqualsListList(PolygonUtils.Sort2(points2),
                                 new[]
                                 {
                                     new[]
                                     {
                                         new Point2d(0, 0), new Point2d(5, 0), new Point2d(10, 0),
                                         new Point2d(10, 10), new Point2d(15, 10), new Point2d(20, 10),
                                         new Point2d(20, 20), new Point2d(25, 20), new Point2d(30, 20),
                                         new Point2d(30, 10), new Point2d(35, 10), new Point2d(40, 10),
                                         new Point2d(40, 0), new Point2d(45, 0), new Point2d(50, 0)
                                     },
                                     new[]
                                     {
                                         new Point2d(50, 0), new Point2d(0, 0)
                                     }
                                 });

            IList<Point2d> points3 = AsList(new Point2d(20, 0), new Point2d(30, 10), new Point2d(20, 20), new Point2d(10, 20), new Point2d(0, 10), new Point2d(10, 0));
            AssertEqualsListList(PolygonUtils.Sort2(points3),
                                 new[]
                                 {
                                     new[]
                                     {
                                         new Point2d(0, 10), new Point2d(10, 0), new Point2d(20, 0), new Point2d(30, 10)
                                     },
                                     new[]
                                     {
                                         new Point2d(30, 10), new Point2d(20, 20), new Point2d(10, 20), new Point2d(0, 10)
                                     },
                                 });
        }

        /*[TestMethod]
        public void TestSort()
        {
            IList<Point2d> points1 = new IList<Point2d>(new[]
            {
                new Point2d(0, 0), new Point2d(5, 0), new Point2d(10, 0),
                new Point2d(10, 10), new Point2d(15, 10), new Point2d(20, 10),
                new Point2d(20, 20), new Point2d(25, 20), new Point2d(30, 20)
            });

            AssertEqualsListList(PolygonUtils.Sort(points1),
                                 new[]
                                 {
                                     new[]
                                     {
                                         new Point2d(0, 0), new Point2d(5, 0), new Point2d(10, 0),
                                         new Point2d(10, 10), new Point2d(15, 10), new Point2d(20, 10),
                                         new Point2d(20, 20), new Point2d(25, 20), new Point2d(30, 20)
                                     },
                                     new[]
                                     {
                                         new Point2d(30, 20), new Point2d(0, 0)
                                     }
                                 });

            IList<Point2d> points2 = new IList<Point2d>(new[]
            {
                new Point2d(0, 0), new Point2d(5, 0), new Point2d(10, 0),
                new Point2d(10, 10), new Point2d(15, 10), new Point2d(20, 10),
                new Point2d(20, 20), new Point2d(25, 20), new Point2d(30, 20),
                new Point2d(30, 10), new Point2d(35, 10), new Point2d(40, 10),
                new Point2d(40, 0), new Point2d(45, 0), new Point2d(50, 0),
            });

            AssertEqualsListList(PolygonUtils.Sort(points2),
                                 new[]
                                 {
                                     new[]
                                     {
                                         new Point2d(0, 0), new Point2d(5, 0), new Point2d(10, 0),
                                         new Point2d(10, 10), new Point2d(15, 10), new Point2d(20, 10),
                                         new Point2d(20, 20), new Point2d(25, 20), new Point2d(30, 20)
                                     },
                                     new[]
                                     {
                                         new Point2d(30, 20),
                                         new Point2d(30, 10), new Point2d(35, 10), new Point2d(40, 10),
                                         new Point2d(40, 0), new Point2d(45, 0), new Point2d(50, 0)
                                     },
                                     new[]
                                     {
                                         new Point2d(50, 0), new Point2d(0, 0)
                                     }
                                 });

            IList<Point2d> points3 = new IList<Point2d>(new[]
            {
                new Point2d(20, 0), new Point2d(30, 10), new Point2d(20, 20), new Point2d(10, 20), new Point2d(0, 10), new Point2d(10, 0)
            });
            AssertEqualsListList(PolygonUtils.Sort(points3),
                                 new[]
                                 {
                                     new[]
                                     {
                                         new Point2d(0, 10), new Point2d(10, 0), new Point2d(20, 0)
                                     },
                                     new[]
                                     {
                                         new Point2d(20, 0), new Point2d(30, 10)
                                     },
                                     new[]
                                     {
                                         new Point2d(30, 10), new Point2d(20, 20), new Point2d(10, 20)
                                     },
                                     new[]
                                     {
                                         new Point2d(10, 20), new Point2d(0, 10)
                                     },
                                 });
        }*/

        private static IList<Point2d> AsList(params Point2d[] points)
        {
            return points;
        }

        private static void AssertEqualsListList<T>(IList<IList<T>> list1, IList<IList<T>> list2, double epsilon = MathUtils.EPSILON)
            where T : IEpsilonEquatable<T>
        {
            Assert.IsTrue(list1 != null && list2 != null);
            Assert.IsTrue(list1.Count == list2.Count);
            for (int i = 0; i < list1.Count; i++)
            {
                Assert.IsTrue(list1[i] != null && list2[i] != null);
                Assert.IsTrue(list1[i].Count == list2[i].Count);
                for (int j = 0; j < list1[i].Count; j++)
                {
                    Assert.IsTrue(list1[i][j].EpsilonEquals(list2[i][j], epsilon));
                }
            }
        }

        private static void AssertEqualsList<T>(IList<T> list1, IList<T> list2, double epsilon = MathUtils.EPSILON)
            where T : IEpsilonEquatable<T>
        {
            Assert.IsTrue(list1 != null && list2 != null);
            Assert.IsTrue(list1.Count == list2.Count);
            for (int i = 0; i < list1.Count; i++)
            {
                Assert.IsTrue(list1[i].EpsilonEquals(list2[i], epsilon));
            }
        }

        private static IList<Point2d> DuplicatePoints(IList<Point2d> points)
        {
            IList<Point2d> aux = new List<Point2d>();
            foreach (Point2d p in points)
            {
                aux.Add(p);
                aux.Add(p);
            }
            return aux;
        }
    }
}