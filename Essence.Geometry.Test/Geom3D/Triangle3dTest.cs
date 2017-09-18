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

namespace Essence.Geometry.Geom3D
{
    [TestClass]
    public class Triangle3dTest
    {
        [TestMethod]
        public void TestEvaluatAndProject()
        {
            Action<Triangle3d, double, double> test01 = (tri, u, v) =>
            {
                Point3d p = tri.Evaluate01(u, v);
                double[] uv = tri.Project01(p);
                Assert.IsTrue(u.EpsilonEquals(uv[0]) && v.EpsilonEquals(uv[1]));
                Point3d p2 = tri.Evaluate01(uv[0], uv[1]);
                Assert.IsTrue(p.EpsilonEquals(p2));
            };

            Action<Triangle3d, double, double> testBar = (tri, u, v) =>
            {
                Point3d p = tri.EvaluateBar(u, v);
                double[] uvw = tri.ProjectBar(p);
                Assert.IsTrue(u.EpsilonEquals(uvw[0]) && v.EpsilonEquals(uvw[1]) && (1 - u - v).EpsilonEquals(uvw[2]));
                Point3d p2 = tri.EvaluateBar(uvw[0], uvw[1], uvw[2]);
                Assert.IsTrue(p.EpsilonEquals(p2));
            };

            /*Action<Triangle3d,REAL, REAL, REAL> testTri = (tri, x, y, z) =>
            {
                POINT p = tri.EvaluateTri(x, y, z);
                REAL[] xyz = tri.ProjectTri(p);
                Assert.IsTrue(x.EpsilonEquals(xyz[0]) && y.EpsilonEquals(xyz[1]) && z.EpsilonEquals(xyz[2]));
                POINT p2 = tri.EvaluateTri(xyz[0], xyz[1], xyz[2]);
                Assert.IsTrue(p.EpsilonEquals(p2));
            };*/

            {
                Triangle3d tri = new Triangle3d(new Point3d(1, 0, 0), new Point3d(0, 1, 0), new Point3d(0, 0, 1));
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
            Assert.IsTrue(new Triangle3d(new Point3d(0, 0, 0), new Point3d(0, 1, 0), new Point3d(0, 0, 0)).Area.EpsilonEquals(0));
            Assert.IsTrue(new Triangle3d(new Point3d(0, 0, 0), new Point3d(0, 0, 0), new Point3d(0, 0, 0)).Area.EpsilonEquals(0));

            Assert.IsTrue(new Triangle3d(new Point3d(0, 0, 0), new Point3d(1, 0, 0), new Point3d(0, 1, 0)).Area.EpsilonEquals(0.5));
            Assert.IsTrue(new Triangle3d(new Point3d(0, 0, 0), new Point3d(0, 1, 0), new Point3d(0, 0, 1)).Area.EpsilonEquals(0.5));

            Assert.IsTrue(new Triangle3d(new Point3d(1, 0, 0), new Point3d(0, 1, 0), new Point3d(0, 0, 1)).Area.EpsilonEquals(0.866, 1e-3));
            Assert.IsTrue(new Triangle3d(new Point3d(0, 4, 4), new Point3d(2, -6, -5), new Point3d(-3, -5, 6)).Area.EpsilonEquals(57.083, 1e-3));
        }

        [TestMethod]
        public void TestIsDegenerate()
        {
            Assert.IsTrue(new Triangle3d(new Point3d(0, 0, 0), new Point3d(0, 1, 0), new Point3d(0, 0, 0)).IsDegenerate);
            Assert.IsTrue(new Triangle3d(new Point3d(0, 0, 0), new Point3d(0, 0, 0), new Point3d(0, 0, 0)).IsDegenerate);

            Assert.IsTrue(!new Triangle3d(new Point3d(0, 0, 0), new Point3d(1, 0, 0), new Point3d(0, 1, 0)).IsDegenerate);
            Assert.IsTrue(!new Triangle3d(new Point3d(0, 0, 0), new Point3d(0, 1, 0), new Point3d(0, 0, 1)).IsDegenerate);

            Assert.IsTrue(!new Triangle3d(new Point3d(1, 0, 0), new Point3d(0, 1, 0), new Point3d(0, 0, 1)).IsDegenerate);
            Assert.IsTrue(!new Triangle3d(new Point3d(0, 4, 4), new Point3d(2, -6, -5), new Point3d(-3, -5, 6)).IsDegenerate);
        }

        [TestMethod]
        public void TestNormal()
        {
            Assert.IsTrue(new Triangle3d(new Point3d(10, 0, 0), new Point3d(0, 10, 0), new Point3d(0, 0, 10)).Normal.EpsilonEquals(new Vector3d(1, 1, 1).Unit));
            Assert.IsTrue(new Triangle3d(new Point3d(0, 0, 0), new Point3d(10, 0, 0), new Point3d(0, 10, 0)).Normal.EpsilonEquals(new Vector3d(0, 0, 1).Unit));
        }
    }
}