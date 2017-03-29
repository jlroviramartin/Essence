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
using System.Diagnostics;
using Essence.Geometry.Core.Double;
using Essence.Geometry.Geom2D;
using Essence.Util.Math.Double;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using REAL = System.Double;

namespace Essence.Geometry.Test.Geom2D
{
    [TestClass]
    public class Line2dTest
    {
        [TestMethod]
        public void TestIsOrthonormal()
        {
            // Comprueba el método IsNormal sobre lineas no normales.
            {
                // Caso normal.
                Assert.IsTrue(Line2d.NewNonNormal(new Point2d(1, 1), new Vector2d(1, 0)).IsNormal);
                Assert.IsTrue(Line2d.NewNonNormal(new Point2d(1, 1), new Vector2d(0, -1)).IsNormal);

                Assert.IsTrue(!Line2d.NewNonNormal(new Point2d(1, 1), new Vector2d(1, 1)).IsNormal);

                // Lineas degeneradas.
                Assert.IsTrue(!Line2d.NewNonNormal(new Point2d(1, 1), new Vector2d(0, 0)).IsNormal);
            }

            // Comprueba el método IsNormal sobre lineas normales.
            {
                // Caso normal.
                Assert.IsTrue(Line2d.NewNormal(new Point2d(1, 1), new Vector2d(10, 0)).IsNormal);
                Assert.IsTrue(Line2d.NewNormal(new Point2d(1, 1), new Vector2d(5, 89)).IsNormal);
                Assert.IsTrue(Line2d.NewNormal(new Point2d(1, 1), new Vector2d(1, 1)).IsNormal);

                // Lineas degeneradas.
                Assert.IsTrue(!Line2d.NewNormal(new Point2d(1, 1), new Vector2d(0, 0)).IsNormal);
            }
        }

        [TestMethod]
        public void TestIsDegenerate()
        {
            // Comprueba el método IsNormal sobre lineas no normales.
            {
                // Caso normal.
                Assert.IsTrue(!Line2d.NewNonNormal(new Point2d(1, 1), new Vector2d(1, 0)).IsDegenerate);
                Assert.IsTrue(!Line2d.NewNonNormal(new Point2d(1, 1), new Vector2d(0, -1)).IsDegenerate);
                Assert.IsTrue(!Line2d.NewNonNormal(new Point2d(1, 1), new Vector2d(1, 1)).IsDegenerate);

                // Planos degenerados.
                Assert.IsTrue(Line2d.NewNonNormal(new Point2d(1, 1), new Vector2d(0, 0)).IsDegenerate);
            }

            // Comprueba el método IsNormal sobre lineas normales.
            {
                // Caso normal.
                Assert.IsTrue(!Line2d.NewNormal(new Point2d(1, 1), new Vector2d(10, 0)).IsDegenerate);
                Assert.IsTrue(!Line2d.NewNormal(new Point2d(1, 1), new Vector2d(5, 89)).IsDegenerate);
                Assert.IsTrue(!Line2d.NewNormal(new Point2d(1, 1), new Vector2d(1, 1)).IsDegenerate);

                // Planos degenerados.
                Assert.IsTrue(Line2d.NewNormal(new Point2d(1, 1), new Vector2d(0, 0)).IsDegenerate);
            }
        }

        [TestMethod]
        public void TestEvaluatAndProject()
        {
            Action<Line2d, double> test = (line, u) =>
            {
                Point2d p = line.Evaluate(u);
                double _u = line.Project(p);
                Assert.IsTrue(u.EpsilonEquals(_u));
                Point2d p2 = line.Evaluate(_u);
                Assert.IsTrue(p.EpsilonEquals(p2));
            };

            // Comprueba los métodos Evaluate y Project sobre lineas normalizadas.
            {
                Line2d lin = Line2d.NewNormal(new Point2d(50, 0), new Point2d(0, 50));
                test(lin, -10);
                test(lin, -1);
                test(lin, -0.5);
                test(lin, 0);
                test(lin, 0.5);
                test(lin, 1);
                test(lin, 10);
            }

            // Comprueba los métodos Evaluate y Project sobre lineas no normalizadas.
            {
                Line2d lin = Line2d.NewNonNormal(new Point2d(50, 0), new Point2d(0, 50));
                test(lin, -10);
                test(lin, -1);
                test(lin, -0.5);
                test(lin, 0);
                test(lin, 0.5);
                test(lin, 1);
                test(lin, 10);
            }
        }

        [TestMethod]
        public void TestWhichSide()
        {
            // Comprueba el método WhichSide sobre lineas normalizadas.
            {
                Line2d line = Line2d.NewNormal(new Point2d(50, 0), new Point2d(0, 50));
                Assert.IsTrue(line.WhichSide(new Point2d(0, 0)) == LineSide.Left);
                Assert.IsTrue(line.WhichSide(new Point2d(100, 100)) == LineSide.Right);
                Assert.IsTrue(line.WhichSide(new Point2d(25, 25)) == LineSide.Middle);
            }

            // Comprueba el método WhichSide sobre lineas no normalizadas.
            {
                Line2d line = Line2d.NewNonNormal(new Point2d(50, 0), new Point2d(0, 50));
                Assert.IsTrue(line.WhichSide(new Point2d(0, 0)) == LineSide.Left);
                Assert.IsTrue(line.WhichSide(new Point2d(100, 100)) == LineSide.Right);
                Assert.IsTrue(line.WhichSide(new Point2d(25, 25)) == LineSide.Middle);
            }
            {
                Line2d line = Line2d.NewNormal(new Point2d(0, 5), new Vector2d(1, 1));
                Assert.IsTrue(line.WhichSide(new Point2d(0, 0)) == LineSide.Right);
                Assert.IsTrue(line.WhichSide(new Point2d(0, 5)) == LineSide.Middle);
                Assert.IsTrue(line.WhichSide(new Point2d(0, 10)) == LineSide.Left);
            }
        }

        [TestMethod]
        public void TestWhichSideRectangle()
        {
            {
                Line2d line = Line2d.NewNormal(new Point2d(0, 5), new Vector2d(1, 1));
                Assert.IsTrue(line.WhichSide(BoundingBox2d.FromExtents(0, 0, 1, 1)) == LineSide.Right);
                Assert.IsTrue(line.WhichSide(BoundingBox2d.FromExtents(0, 4, 2, 2)) == LineSide.Middle);
                Assert.IsTrue(line.WhichSide(BoundingBox2d.FromExtents(0, 6, 1, 1)) == LineSide.Middle);
                Assert.IsTrue(line.WhichSide(BoundingBox2d.FromExtents(0, 7, 1, 1)) == LineSide.Left);
            }
        }

        [TestMethod]
        public void TestDistance()
        {
            Func<Line2d, Point2d, bool> test = (line, p) =>
            {
                double u = line.Project(p);

                Point2d pEnPlano = line.Evaluate(u);

                Point2d closestPoint;
                double d = line.Distance(p, out closestPoint);

                Point2d p3 = pEnPlano + line.Normal * d;

                bool pEqualsP3 = p.EpsilonEquals(p3);
                bool pEnPlanoEqualsClosestPoint = pEnPlano.EpsilonEquals(closestPoint);

                return pEqualsP3 && pEnPlanoEqualsClosestPoint && p.DistanceTo(pEnPlano).EpsilonEquals(Math.Abs(d));
            };

            // Comprueba los metodos Project, Evaluate y DistanceTo sobre lineas normalizados.
            {
                Line2d line = Line2d.NewNormal(new Point2d(50, 0), new Point2d(0, 50));
                Assert.IsTrue(test(line, new Point2d(0, 0)));
                Assert.IsTrue(test(line, new Point2d(100, 100)));
                Assert.IsTrue(test(line, new Point2d(25, 25)));

                Assert.IsTrue(line.Distance(new Point2d(0, 0)).EpsilonEquals(-35.355, 1e-3));
                Assert.IsTrue(line.Distance(new Point2d(100, 100)).EpsilonEquals(106.066, 1e-3));
                Assert.IsTrue(line.Distance(new Point2d(25, 25)).EpsilonEquals(0));
            }

            // Comprueba los metodos Project, Evaluate y DistanceTo sobre lineas no normalizados.
            {
                Line2d line = Line2d.NewNonNormal(new Point2d(50, 0), new Point2d(0, 50));
                Assert.IsTrue(test(line, new Point2d(0, 0)));
                Assert.IsTrue(test(line, new Point2d(100, 100)));
                Assert.IsTrue(test(line, new Point2d(25, 25)));

                Assert.IsTrue(line.Distance(new Point2d(0, 0)).EpsilonEquals(-35.355, 1e-3));
                Assert.IsTrue(line.Distance(new Point2d(100, 100)).EpsilonEquals(106.066, 1e-3));
                Assert.IsTrue(line.Distance(new Point2d(25, 25)).EpsilonEquals(0));
            }

            {
                Line2d line = Line2d.NewNormal(new Point2d(0, 5), new Vector2d(1, 1));
                Assert.IsTrue(line.Distance(new Point2d(0, 0)).EpsilonEquals(3.535, 1e-3));
                Assert.IsTrue(line.Distance(new Point2d(0, 5)).EpsilonEquals(0));
                Assert.IsTrue(line.Distance(new Point2d(0, 10)).EpsilonEquals(-3.535, 1e-3));
            }
        }

        //[TestMethod]
        public void TestSpeed()
        {
            Xxx xxx = new Xxx();

            int c = 100000000;

            System.Diagnostics.Stopwatch w1 = Stopwatch.StartNew();
            Vector4d sum1 = Vector4d.Zero;
            for (int i = 0; i < c; i++)
            {
                sum1 += xxx.Value1;
            }
            w1.Stop();

            System.Diagnostics.Stopwatch w2 = Stopwatch.StartNew();
            Vector4d sum2 = Vector4d.Zero;
            for (int i = 0; i < c; i++)
            {
                sum2 += xxx.Value2;
            }
            w2.Stop();

            System.Diagnostics.Stopwatch w3 = Stopwatch.StartNew();
            Vector4d sum3 = Vector4d.Zero;
            for (int i = 0; i < c; i++)
            {
                sum3 += xxx.Value3;
            }
            w3.Stop();

            Console.WriteLine("Tiempo 1: " + w1.ElapsedMilliseconds + " suma: " + sum1);
            Console.WriteLine("Tiempo 2: " + w2.ElapsedMilliseconds + " suma: " + sum2);
            Console.WriteLine("Tiempo 3: " + w3.ElapsedMilliseconds + " suma: " + sum3);
        }

        public class Xxx
        {
            public Vector4d Value1
            {
                get
                {
                    if (this.value1 == null)
                    {
                        this.value1 = Vector4d.One;
                    }
                    return (Vector4d)this.value1;
                }
            }

            private Vector4d? value1;

            public Vector4d Value2
            {
                get
                {
                    if (!this.vvalue2)
                    {
                        this.vvalue2 = true;
                        this.value2 = Vector4d.One;
                    }
                    return this.value2;
                }
            }

            private bool vvalue2 = false;
            private Vector4d value2;

            public Vector4d Value3
            {
                get
                {
                    if (this.vvalue3)
                    {
                        this.vvalue3 = false;
                        this.value3 = Vector4d.One;
                    }
                    return this.value3;
                }
            }

            private bool vvalue3 = true;
            private Vector4d value3;
        }
    }
}