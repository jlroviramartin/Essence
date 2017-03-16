#region License

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

#endregion

using System;
using Essence.Geometry.Core.Double;
using Essence.Geometry.Geom3D;
using Essence.Util.Math.Double;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using REAL = System.Double;

namespace Essence.Geometry.Test.Geom3D
{
    [TestClass]
    public class Plane3dTest
    {
        [TestMethod]
        public void TestIsOrthonormal()
        {
            // Comprueba el método IsOrthonormal sobre planos no ortonormales.
            {
                // Caso normal.
                Assert.IsTrue(Plane3d.NewNonOrthonormal(new Point3d(1, 1, 1), new Vector3d(1, 0, 0), new Vector3d(0, 1, 0)).IsOrthonormal);
                Assert.IsTrue(Plane3d.NewNonOrthonormal(new Point3d(1, 1, 1), new Vector3d(0, -1, 0), new Vector3d(1, 0, 0)).IsOrthonormal);

                Assert.IsTrue(!Plane3d.NewNonOrthonormal(new Point3d(1, 1, 1), new Vector3d(1, 1, 1), new Vector3d(1, 0, 10)).IsOrthonormal);

                // Planos degenerados.
                Assert.IsTrue(!Plane3d.NewNonOrthonormal(new Point3d(1, 1, 1), new Vector3d(1, 1, 1), new Vector3d(1, 1, 1)).IsOrthonormal);
                Assert.IsTrue(!Plane3d.NewNonOrthonormal(new Point3d(1, 1, 1), new Vector3d(0, 0, 0), new Vector3d(0, 0, 0)).IsOrthonormal);
            }

            // Comprueba el método IsOrthonormal sobre planos ortonormales.
            {
                // Caso normal.
                Assert.IsTrue(Plane3d.NewOrthonormal(new Point3d(1, 1, 1), new Vector3d(10, 0, 0), new Vector3d(0, 10, 90)).IsOrthonormal);
                Assert.IsTrue(Plane3d.NewOrthonormal(new Point3d(1, 1, 1), new Vector3d(5, 89, 2), new Vector3d(9, 23, 7)).IsOrthonormal);
                Assert.IsTrue(Plane3d.NewOrthonormal(new Point3d(1, 1, 1), new Vector3d(1, 1, 1), new Vector3d(1, 0, 10)).IsOrthonormal);

                // Planos degenerados.
                Assert.IsTrue(!Plane3d.NewOrthonormal(new Point3d(1, 1, 1), new Vector3d(1, 1, 1), new Vector3d(1, 1, 1)).IsOrthonormal);
                Assert.IsTrue(!Plane3d.NewOrthonormal(new Point3d(1, 1, 1), new Vector3d(0, 0, 0), new Vector3d(0, 0, 0)).IsOrthonormal);
            }
        }

        [TestMethod]
        public void TestIsDegenerate()
        {
            // Comprueba el método IsOrthonormal sobre planos no ortonormales.
            {
                // Caso normal.
                Assert.IsTrue(!Plane3d.NewNonOrthonormal(new Point3d(1, 1, 1), new Vector3d(1, 0, 0), new Vector3d(0, 1, 0)).IsDegenerate);
                Assert.IsTrue(!Plane3d.NewNonOrthonormal(new Point3d(1, 1, 1), new Vector3d(0, -1, 0), new Vector3d(1, 0, 0)).IsDegenerate);
                Assert.IsTrue(!Plane3d.NewNonOrthonormal(new Point3d(1, 1, 1), new Vector3d(1, 1, 1), new Vector3d(1, 0, 10)).IsDegenerate);

                // Planos degenerados.
                Assert.IsTrue(Plane3d.NewNonOrthonormal(new Point3d(1, 1, 1), new Vector3d(1, 1, 1), new Vector3d(1, 1, 1)).IsDegenerate);
                Assert.IsTrue(Plane3d.NewNonOrthonormal(new Point3d(1, 1, 1), new Vector3d(0, 0, 0), new Vector3d(0, 0, 0)).IsDegenerate);
            }

            // Comprueba el método IsOrthonormal sobre planos ortonormales.
            {
                // Caso normal.
                Assert.IsTrue(!Plane3d.NewOrthonormal(new Point3d(1, 1, 1), new Vector3d(10, 0, 0), new Vector3d(0, 10, 90)).IsDegenerate);
                Assert.IsTrue(!Plane3d.NewOrthonormal(new Point3d(1, 1, 1), new Vector3d(5, 89, 2), new Vector3d(9, 23, 7)).IsDegenerate);
                Assert.IsTrue(!Plane3d.NewOrthonormal(new Point3d(1, 1, 1), new Vector3d(1, 1, 1), new Vector3d(1, 0, 10)).IsDegenerate);

                // Planos degenerados.
                Assert.IsTrue(Plane3d.NewOrthonormal(new Point3d(1, 1, 1), new Vector3d(1, 1, 1), new Vector3d(1, 1, 1)).IsDegenerate);
                Assert.IsTrue(Plane3d.NewOrthonormal(new Point3d(1, 1, 1), new Vector3d(0, 0, 0), new Vector3d(0, 0, 0)).IsDegenerate);
            }
        }

        [TestMethod]
        public void TestEvaluatAndProject()
        {
            Action<Plane3d, REAL, REAL> test = (plane, u, v) =>
            {
                Point3d p = plane.Evaluate(u, v);
                REAL[] uv = plane.Project(p);
                Assert.IsTrue(u.EpsilonEquals(uv[0]) && u.EpsilonEquals(uv[1]));
                Point3d p2 = plane.Evaluate(uv[0], uv[1]);
                Assert.IsTrue(p.EpsilonEquals(p2));
            };

            // Comprueba los métodos Evaluate y Project sobre planos ortonormales.
            {
                Plane3d plane = Plane3d.NewOrthonormal(new Point3d(10, 0, 0), new Point3d(0, 10, 0), new Point3d(0, 0, 10));
                test(plane, -10, -10);
                test(plane, -1, -1);
                test(plane, -0.5, -0.5);
                test(plane, 0, 0);
                test(plane, 0.5, 0.5);
                test(plane, 1, 1);
                test(plane, 10, 10);
            }

            // Comprueba los métodos Evaluate y Project sobre planos no ortonormales.
            {
                Plane3d plane = Plane3d.NewNonOrthonormal(new Point3d(10, 0, 0), new Point3d(0, 10, 0), new Point3d(0, 0, 10));
                test(plane, -10, -10);
                test(plane, -1, -1);
                test(plane, -0.5, -0.5);
                test(plane, 0, 0);
                test(plane, 0.5, 0.5);
                test(plane, 1, 1);
                test(plane, 10, 10);
            }
        }

        [TestMethod]
        public void TestWhichSide()
        {
            // Comprueba el método WhichSide sobre planos ortonormales.
            {
                Plane3d plane = Plane3d.NewOrthonormal(new Point3d(10, 0, 0), new Point3d(0, 10, 0), new Point3d(0, 0, 10));
                Assert.IsTrue(plane.WhichSide(new Point3d(0, 0, 0)) == PlaneSide.Back);
                Assert.IsTrue(plane.WhichSide(new Point3d(100, 100, 100)) == PlaneSide.Front);
                Assert.IsTrue(plane.WhichSide(new Point3d(10 / 3.0, 10 / 3.0, 10 / 3.0)) == PlaneSide.Middle);
            }

            // Comprueba el método WhichSide sobre planos no ortonormales.
            {
                Plane3d plane = Plane3d.NewNonOrthonormal(new Point3d(10, 0, 0), new Point3d(0, 10, 0), new Point3d(0, 0, 10));
                Assert.IsTrue(plane.WhichSide(new Point3d(0, 0, 0)) == PlaneSide.Back);
                Assert.IsTrue(plane.WhichSide(new Point3d(100, 100, 100)) == PlaneSide.Front);
                Assert.IsTrue(plane.WhichSide(new Point3d(10 / 3.0, 10 / 3.0, 10 / 3.0)) == PlaneSide.Middle);
            }
        }

        [TestMethod]
        public void TestDistance()
        {
            Func<Plane3d, Point3d, bool> test = (plane, p) =>
            {
                REAL[] uv = plane.Project(p);

                Point3d pEnPlano = plane.Evaluate(uv[0], uv[1]);

                Point3d closestPoint;
                REAL d = plane.Distance(p, out closestPoint);

                Point3d p3 = pEnPlano + plane.Normal * d;

                bool pEqualsP3 = p.EpsilonEquals(p3);
                bool pEnPlanoEqualsClosestPoint = pEnPlano.EpsilonEquals(closestPoint);

                return pEqualsP3 && pEnPlanoEqualsClosestPoint && p.DistanceTo(pEnPlano).EpsilonEquals(Math.Abs(d));
            };

            // Comprueba los metodos Project, Evaluate y DistanceTo sobre planos ortonormales.
            {
                Plane3d plane = Plane3d.NewOrthonormal(new Point3d(10, 0, 0), new Point3d(0, 10, 0), new Point3d(0, 0, 10));
                Assert.IsTrue(test(plane, new Point3d(0, 0, 0)));
                Assert.IsTrue(test(plane, new Point3d(100, 100, 100)));
                Assert.IsTrue(test(plane, new Point3d(5, 5, 5)));
                Assert.IsTrue(test(plane, new Point3d(10 / 3.0, 10 / 3.0, 10 / 3.0)));

                Assert.IsTrue(plane.Distance(new Point3d(0, 0, 0)).EpsilonEquals(-5.773, 1e-3));
                Assert.IsTrue(plane.Distance(new Point3d(100, 100, 100)).EpsilonEquals(167.431, 1e-3));
                Assert.IsTrue(plane.Distance(new Point3d(5, 5, 5)).EpsilonEquals(2.886, 1e-3));
                Assert.IsTrue(plane.Distance(new Point3d(10 / 3.0, 10 / 3.0, 10 / 3.0)).EpsilonEquals(0));
            }

            // Comprueba los metodos Project, Evaluate y DistanceTo sobre planos no ortonormales.
            {
                Plane3d plane = Plane3d.NewNonOrthonormal(new Point3d(10, 0, 0), new Point3d(0, 10, 0), new Point3d(0, 0, 10));
                Assert.IsTrue(test(plane, new Point3d(0, 0, 0)));
                Assert.IsTrue(test(plane, new Point3d(100, 100, 100)));
                Assert.IsTrue(test(plane, new Point3d(5, 5, 5)));
                Assert.IsTrue(test(plane, new Point3d(10 / 3.0, 10 / 3.0, 10 / 3.0)));

                Assert.IsTrue(plane.Distance(new Point3d(0, 0, 0)).EpsilonEquals(-5.773, 1e-3));
                Assert.IsTrue(plane.Distance(new Point3d(100, 100, 100)).EpsilonEquals(167.431, 1e-3));
                Assert.IsTrue(plane.Distance(new Point3d(5, 5, 5)).EpsilonEquals(2.886, 1e-3));
                Assert.IsTrue(plane.Distance(new Point3d(10 / 3.0, 10 / 3.0, 10 / 3.0)).EpsilonEquals(0));
            }
        }
    }
}