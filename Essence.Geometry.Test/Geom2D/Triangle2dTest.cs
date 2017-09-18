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
using Essence.Geometry.Core.Double;
using Essence.Util.Math.Double;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using REAL = System.Double;

namespace Essence.Geometry.Geom2D
{
    [TestClass]
    public class Triangle2dTest
    {
        [TestMethod]
        public void TestEvaluatAndProject()
        {
            Action<Triangle2d, double, double> test01 = (tri, u, v) =>
            {
                Point2d p = tri.Evaluate01(u, v);
                double[] uv = tri.Project01(p);
                Assert.IsTrue(u.EpsilonEquals(uv[0]) && v.EpsilonEquals(uv[1]));
                Point2d p2 = tri.Evaluate01(uv[0], uv[1]);
                Assert.IsTrue(p.EpsilonEquals(p2));
            };

            Action<Triangle2d, double, double> testBar = (tri, u, v) =>
            {
                Point2d p = tri.EvaluateBar(u, v);
                double[] uvw = tri.ProjectBar(p);
                Assert.IsTrue(u.EpsilonEquals(uvw[0]) && v.EpsilonEquals(uvw[1]) && (1 - u - v).EpsilonEquals(uvw[2]));
                Point2d p2 = tri.EvaluateBar(uvw[0], uvw[1], uvw[2]);
                Assert.IsTrue(p.EpsilonEquals(p2));
            };

            {
                Triangle2d tri = new Triangle2d(new Point2d(0, 5), new Point2d(5, 0), new Point2d(5, 5));

                test01(tri, 0, 0);
                test01(tri, 0.1, 0.1);
                test01(tri, 0.2, 0.2);
                test01(tri, 0.3, 0.3);
                test01(tri, 0, 0.3);
                test01(tri, 0.3, 0);

                Assert.IsTrue(tri.EvaluateBar(1, 0, 0).EpsilonEquals(tri.P0));
                Assert.IsTrue(tri.EvaluateBar(0, 1, 0).EpsilonEquals(tri.P1));
                Assert.IsTrue(tri.EvaluateBar(0, 0, 1).EpsilonEquals(tri.P2));

                testBar(tri, 0, 0);
                testBar(tri, 1, 0);
                testBar(tri, 0, 1);
                testBar(tri, 0.5, 0);
                testBar(tri, 0.5, 0.5);
                testBar(tri, 1.0 / 3.0, 1.0 / 3.0);
            }
        }

        [TestMethod]
        public void TestArea()
        {
            Assert.IsTrue(new Triangle2d(new Point2d(0, 0), new Point2d(0, 1), new Point2d(0, 0)).Area.EpsilonEquals(0));
            Assert.IsTrue(new Triangle2d(new Point2d(0, 0), new Point2d(0, 0), new Point2d(0, 0)).Area.EpsilonEquals(0));

            Assert.IsTrue(new Triangle2d(new Point2d(0, 0), new Point2d(1, 0), new Point2d(0, 1)).Area.EpsilonEquals(0.5));
            Assert.IsTrue(new Triangle2d(new Point2d(0, 0), new Point2d(0, 1), new Point2d(1, 0)).Area.EpsilonEquals(0.5));
        }

        [TestMethod]
        public void TestIsDegenerate()
        {
            Assert.IsTrue(new Triangle2d(new Point2d(0, 0), new Point2d(0, 1), new Point2d(0, 0)).IsDegenerate);
            Assert.IsTrue(new Triangle2d(new Point2d(0, 0), new Point2d(0, 0), new Point2d(0, 0)).IsDegenerate);

            Assert.IsTrue(!new Triangle2d(new Point2d(0, 0), new Point2d(1, 0), new Point2d(0, 1)).IsDegenerate);
            Assert.IsTrue(!new Triangle2d(new Point2d(0, 0), new Point2d(0, 1), new Point2d(1, 0)).IsDegenerate);
        }

        [TestMethod]
        public void TestOrientation()
        {
            Assert.IsTrue(new Triangle2d(new Point2d(0, 0), new Point2d(0, 1), new Point2d(0, 0)).Orientation == Orientation.Degenerate);
            Assert.IsTrue(new Triangle2d(new Point2d(0, 0), new Point2d(0, 0), new Point2d(0, 0)).Orientation == Orientation.Degenerate);

            Assert.IsTrue(new Triangle2d(new Point2d(0, 0), new Point2d(1, 0), new Point2d(0, 1)).Orientation == Orientation.CCW);
            Assert.IsTrue(new Triangle2d(new Point2d(0, 0), new Point2d(0, 1), new Point2d(1, 0)).Orientation == Orientation.CW);
        }

        [TestMethod]
        public void TestEnsureCCW()
        {
            Assert.IsTrue(new Triangle2d(new Point2d(0, 0), new Point2d(0, 1), new Point2d(0, 0)).EnsureCCW().Orientation == Orientation.Degenerate);
            Assert.IsTrue(new Triangle2d(new Point2d(0, 0), new Point2d(0, 0), new Point2d(0, 0)).EnsureCCW().Orientation == Orientation.Degenerate);

            Assert.IsTrue(new Triangle2d(new Point2d(0, 0), new Point2d(1, 0), new Point2d(0, 1)).EnsureCCW().Orientation == Orientation.CCW);
            Assert.IsTrue(new Triangle2d(new Point2d(0, 0), new Point2d(0, 1), new Point2d(1, 0)).EnsureCCW().Orientation == Orientation.CCW);
        }
    }
}