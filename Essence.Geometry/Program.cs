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

#if false
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Essence.Geometry.Core;
using Essence.Geometry.Core.Double;
using SysMath = System.Math;

namespace Essence.Geometry
{
    public class Program
    {
        private static Test NewTest(string name, Action action)
        {
            return new Test(name, action);
        }

        private static Test NewTest(MethodInfo methodInfo, int count)
        {
            TestAttribute attr = methodInfo.GetCustomAttribute<TestAttribute>();
            Action action = () => methodInfo.Invoke(null, new object[] { count });
            return NewTest(attr.Name, action);
        }

        private class Test
        {
            public Test(string name, Action action)
            {
                this.Name = name;
                this.Action = action;
            }

            public string Name;
            public Action Action;
            public long Time;
        }

        public class TestAttribute : Attribute
        {
            public TestAttribute(string name)
            {
                this.Name = name;
            }

            public string Name { get; private set; }
        }

        public static void Main(string[] args)
        {
            //const int count = 100000000;
            const int count = 10000000;

            Test[] tests = typeof (Program)
                .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                .Where(methodInfo => methodInfo.GetCustomAttribute<TestAttribute>() != null)
                .Select(methodInfo => NewTest(methodInfo, count))
                .ToArray();

            for (int i = 0; i < tests.Length; i++)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                Action action = tests[i].Action;
                action();
                sw.Stop();
                tests[i].Time = sw.ElapsedMilliseconds;
            }

            long tmin = tests.Select(x => x.Time).Min();
            foreach (Test t in tests.OrderBy(x => x.Time))
            {
                Console.WriteLine("Tiempo " + t.Name + " : " + t.Time + " " + (t.Time / (double) tmin).ToString("F6"));
            }
        }

        //[Test("VMath.Add(IVector2D,IVector2D,IBufferedVector2D)")]
        private static void Test_Vector2d_VMath_Add(int count)
        {
            Essence.Maths.Prueba.BufferedVector2d aux = new Essence.Maths.Prueba.BufferedVector2d();
            double x = 0, y = 0;
            Essence.Maths.Prueba.Vector2d v1 = new Essence.Maths.Prueba.Vector2d(10, 10);
            Essence.Maths.Prueba.Vector2d v2 = new Essence.Maths.Prueba.Vector2d(10, 10);
            for (int i = 0; i < count; i++)
            {
                Essence.Maths.Prueba.VMath.Add(v1, v2, aux);
                x += aux.X;
                y += aux.Y;
            }
            Console.WriteLine("result : " + x + " ; " + y + " " + aux);
        }

        //[Test("VMath.Add2(IVector2D, IVector2D, IBufferedVector2D)")]
        private static void Test_Vector2d_VMath_Add2(int count)
        {
            Essence.Maths.Prueba.BufferedVector2d aux = new Essence.Maths.Prueba.BufferedVector2d();
            double x = 0, y = 0;
            Essence.Maths.Prueba.Vector2d v1 = new Essence.Maths.Prueba.Vector2d(10, 10);
            Essence.Maths.Prueba.Vector2d v2 = new Essence.Maths.Prueba.Vector2d(10, 10);
            for (int i = 0; i < count; i++)
            {
                Essence.Maths.Prueba.VMath.Add2(v1, v2, aux);
                x += aux.X;
                y += aux.Y;
            }
            Console.WriteLine("result : " + x + " ; " + y + " " + aux);
        }

        //[Test("IVector2D.Add2(IVector2D,IBufferedVector2D)")]
        private static void Test_IVector2D_Add2(int count)
        {
            Essence.Maths.Prueba.BufferedVector2d aux = new Essence.Maths.Prueba.BufferedVector2d();
            double x = 0, y = 0;
            Essence.Maths.Prueba.IVector2D v1 = new Essence.Maths.Prueba.Vector2d(10, 10);
            Essence.Maths.Prueba.IVector2D v2 = new Essence.Maths.Prueba.Vector2d(10, 10);
            for (int i = 0; i < count; i++)
            {
                v1.Add2(v2, aux);
                x += aux.X;
                y += aux.Y;
            }
            Console.WriteLine("result : " + x + " ; " + y + " " + aux);
        }

        //[Test("IVector2D.Add(IVector2D,IBufferedVector2D)")]
        private static void Test_IVector2D_Add(int count)
        {
            Essence.Maths.Prueba.BufferedVector2d aux = new Essence.Maths.Prueba.BufferedVector2d();
            double x = 0, y = 0;
            Essence.Maths.Prueba.IVector2D v1 = new Essence.Maths.Prueba.Vector2d(10, 10);
            Essence.Maths.Prueba.IVector2D v2 = new Essence.Maths.Prueba.Vector2d(10, 10);
            for (int i = 0; i < count; i++)
            {
                v1.Add(v2, aux);
                x += aux.X;
                y += aux.Y;
            }
            Console.WriteLine("result : " + x + " ; " + y + " " + aux);
        }

        //[Test("IVector2D.Add(IVector2D) : IVector2D")]
        private static void Test_IVector2D_Add_Func(int count)
        {
            Essence.Maths.Prueba.BufferedVector2d aux = new Essence.Maths.Prueba.BufferedVector2d();
            double x = 0, y = 0;
            Essence.Maths.Prueba.IVector2D v1 = new Essence.Maths.Prueba.Vector2d(10, 10);
            Essence.Maths.Prueba.IVector2D v2 = new Essence.Maths.Prueba.Vector2d(10, 10);
            for (int i = 0; i < count; i++)
            {
                Essence.Maths.Prueba.IVector2D v3 = v1.Add(v2);
                v3.GetCoords(aux);
                x += aux.X;
                y += aux.Y;
            }
            Console.WriteLine("result : " + x + " ; " + y + " ");
        }

        //[Test("Vector2d.Add(Vector2d,IBufferedVector2D)")]
        private static void Test_Vector2d_Add(int count)
        {
            Essence.Maths.Prueba.BufferedVector2d aux = new Essence.Maths.Prueba.BufferedVector2d();
            double x = 0, y = 0;
            Essence.Maths.Prueba.Vector2d v1 = new Essence.Maths.Prueba.Vector2d(10, 10);
            Essence.Maths.Prueba.Vector2d v2 = new Essence.Maths.Prueba.Vector2d(10, 10);
            for (int i = 0; i < count; i++)
            {
                v1.Add(v2, aux);
                x += aux.X;
                y += aux.Y;
            }
            Console.WriteLine("result : " + x + " ; " + y);
        }

        //[Test("Vector2d.Add(Vector2d) : Vector2d")]
        private static void Test_Vector2d_Add_Func(int count)
        {
            double x = 0, y = 0;
            Essence.Maths.Prueba.Vector2d v1 = new Essence.Maths.Prueba.Vector2d(10, 10);
            Essence.Maths.Prueba.Vector2d v2 = new Essence.Maths.Prueba.Vector2d(10, 10);
            Essence.Maths.Prueba.Vector2d v3;
            for (int i = 0; i < count; i++)
            {
                v3 = v1.Add(v2);
                x += v3.x;
                y += v3.y;
            }
            Console.WriteLine("result : " + x + " ; " + y);
        }


        /*[Test("struct IVector2D.Add2(IVector2D,IBufferedVector2D)")]
        private static void Test_IVector2D_struct_Add2(int count)
        {
            Essence.Maths.Prueba.BufferedVector2d aux = new Essence.Maths.Prueba.BufferedVector2d();
            double x = 0, y = 0;
            Essence.Maths.Prueba.IVector2D v1 = new Essence.Maths.Prueba.Vector2d_struct(10, 10);
            Essence.Maths.Prueba.IVector2D v2 = new Essence.Maths.Prueba.Vector2d_struct(10, 10);
            for (int i = 0; i < count; i++)
            {
                v1.Add2(v2, aux);
                x += aux.X;
                y += aux.Y;
            }
            Console.WriteLine("result : " + x + " ; " + y + " " + aux);
        }

        [Test("struct IVector2D.Add(IVector2D,IBufferedVector2D)")]
        private static void Test_IVector2D_struct_Add(int count)
        {
            Essence.Maths.Prueba.BufferedVector2d aux = new Essence.Maths.Prueba.BufferedVector2d();
            double x = 0, y = 0;
            Essence.Maths.Prueba.IVector2D v1 = new Essence.Maths.Prueba.Vector2d_struct(10, 10);
            Essence.Maths.Prueba.IVector2D v2 = new Essence.Maths.Prueba.Vector2d_struct(10, 10);
            for (int i = 0; i < count; i++)
            {
                v1.Add(v2, aux);
                x += aux.X;
                y += aux.Y;
            }
            Console.WriteLine("result : " + x + " ; " + y + " " + aux);
        }

        [Test("struct Vector2d_struct.Add(Vector2d_struct,IBufferedVector2D)")]
        private static void Test_Vector2d_struct_Add(int count)
        {
            Essence.Maths.Prueba.BufferedVector2d aux = new Essence.Maths.Prueba.BufferedVector2d();
            double x = 0, y = 0;
            Essence.Maths.Prueba.Vector2d_struct v1 = new Essence.Maths.Prueba.Vector2d_struct(10, 10);
            Essence.Maths.Prueba.Vector2d_struct v2 = new Essence.Maths.Prueba.Vector2d_struct(10, 10);
            for (int i = 0; i < count; i++)
            {
                v1.Add(v2, aux);
                x += aux.X;
                y += aux.Y;
            }
            Console.WriteLine("result : " + x + " ; " + y);
        }

        [Test("struct Vector2d_struct.Add(Vector2d_struct : Vector2d_struct)")]
        private static void Test_Vector2d_struct_Add_Func(int count)
        {
            double x = 0, y = 0;
            Essence.Maths.Prueba.Vector2d_struct v1 = new Essence.Maths.Prueba.Vector2d_struct(10, 10);
            Essence.Maths.Prueba.Vector2d_struct v2 = new Essence.Maths.Prueba.Vector2d_struct(10, 10);
            Essence.Maths.Prueba.Vector2d_struct v3;
            for (int i = 0; i < count; i++)
            {
                v3 = v1.Add(v2);
                x += v3.x;
                y += v3.y;
            }
            Console.WriteLine("result : " + x + " ; " + y);
        }*/

        //[Test("Fast Vector2d.Add(Vector2d) : Vector2d")]
        private static void Test5_FastVector(int count)
        {
            double x = 0, y = 0;
            Essence.Geometry.Core.Double.Vector2d v1 = new Essence.Geometry.Core.Double.Vector2d(10, 10);
            Essence.Geometry.Core.Double.Vector2d v2 = new Essence.Geometry.Core.Double.Vector2d(10, 10);
            Essence.Geometry.Core.Double.Vector2d v3;
            for (int i = 0; i < count; i++)
            {
                v3 = v1 + v2;
                x += v3.X;
                y += v3.Y;
            }
            Console.WriteLine("result : " + x + " ; " + y + " ");
        }


        [Test("Test lento")]
        private static void Test5_Lento(int count)
        {
            Essence.Maths.Prueba.EvaluadorBezierCubica2D ev = new Essence.Maths.Prueba.EvaluadorBezierCubica2D(
                new Essence.Maths.Prueba.Point2d(0,0),
                new Essence.Maths.Prueba.Point2d(0, 100),
                new Essence.Maths.Prueba.Point2d(100, 100),
                new Essence.Maths.Prueba.Point2d(100, 0),
                count);
            List<Essence.Maths.Prueba.IPoint2D> points = new List<Essence.Maths.Prueba.IPoint2D>(count);
            while (ev.MoveNext())
            {
                points.Add(ev.Current);
            }
            Console.WriteLine(points.Count);
            /*foreach (Essence.Maths.Prueba.IPoint2D point in points)
            {
                Console.WriteLine(point.GetXDouble().ToString("F3") + " ; " + point.GetYDouble().ToString("F3"));
            }*/
        }

        [Test("Test lento2")]
        private static void Test5_Lento2(int count)
        {
            Essence.Maths.Prueba.EvaluadorBezierCubica2D_x ev = new Essence.Maths.Prueba.EvaluadorBezierCubica2D_x(
                new Essence.Maths.Prueba.Point2d(0, 0),
                new Essence.Maths.Prueba.Point2d(0, 100),
                new Essence.Maths.Prueba.Point2d(100, 100),
                new Essence.Maths.Prueba.Point2d(100, 0),
                count);
            List<Essence.Maths.Prueba.IPoint2D> points = new List<Essence.Maths.Prueba.IPoint2D>(count);
            while (ev.MoveNext())
            {
                points.Add(ev.Current);
            }
            Console.WriteLine(points.Count);
            /*foreach (Essence.Maths.Prueba.IPoint2D point in points)
            {
                Console.WriteLine(point.GetXDouble().ToString("F3") + " ; " + point.GetYDouble().ToString("F3"));
            }*/
        }

        [Test("Test rapido")]
        private static void Test5_Rapido(int count)
        {
            Essence.Maths.Prueba.EvaluadorBezierCubica2D_2 ev = new Essence.Maths.Prueba.EvaluadorBezierCubica2D_2(
                new Essence.Geometry.Core.Double.Point2d(0, 0),
                new Essence.Geometry.Core.Double.Point2d(0, 100),
                new Essence.Geometry.Core.Double.Point2d(100, 100),
                new Essence.Geometry.Core.Double.Point2d(100, 0),
                count);
            List<Essence.Geometry.Core.Double.Point2d> points = new List<Essence.Geometry.Core.Double.Point2d>(count);
            while (ev.MoveNext())
            {
                points.Add(ev.Current);
            }
            Console.WriteLine(points.Count);
            /*foreach (Essence.Geometry.Core.Double.Point2d point in points)
            {
                Console.WriteLine(point.X.ToString("F3") + " ; " + point.Y.ToString("F3"));
            }*/
        }
    }

#if false
    public class Line
    {
        public Line(Point2d p0, double t0, Point2d p1, double t1, double normal, double radio)
        {
            this.p0 = p0;
            this.t0 = t0;
            this.p1 = p1;
            this.t1 = t1;
            this.normal = normal;
            this.radio = radio;

            this.dir = this.p1.Sub(this.p0);
            this.len = this.dir.Length;
            this.dirN = this.dir.Div(this.len);
        }

        public double Evaluate(Point2d pnt)
        {
            double t01 = this.Project01(pnt);
            double t = (t01.EpsilonEquals(0)
                ? this.t0
                : (t01.EpsilonEquals(1)
                    ? this.t1
                    : this.t0 + (this.t1 - this.t0) * t01));
            return t;
        }

        public void PuntoEje(double t, out Point2d pnt, out double normal, out double radio)
        {
            // Se normaliza la estacion.
            double t01 = (t.EpsilonEquals(this.t0)
                ? 0
                : (t.EpsilonEquals(this.t1)
                    ? 1
                    : (t - this.t0) / (this.t1 - this.t0)));

            pnt = this.Evaluate01(t01);
            normal = this.normal;
            radio = this.radio;
        }

        public BoundingBox2d GetEnvelope(double t0, double t1)
        {
            if (t0.EpsilonEquals(this.t0) && t1.EpsilonEquals(this.t1))
            {
                return BoundingBox2d.Union(this.p0, this.p1);
            }

            if (this.t0.EpsilonEquals(this.t1))
            {
                return BoundingBox2d.Union(this.p0, this.p0);
            }

            // Se normaliza la estacion.
            double t01_0 = (t0 - this.t0) / (this.t1 - this.t0);
            double t01_1 = (t1 - this.t0) / (this.t1 - this.t0);

            return BoundingBox2d.Union(
                this.Evaluate01(t01_0),
                this.Evaluate01(t01_1));
        }

        public BoundingBox2d GetEnvelope(double d, double estacion1, double estacion2)
        {
            double t0 = this.t0;
            double t1 = this.t1;

            Vector2d v = this.dirN.PerpRight.Mul(d);

            if (estacion1.EpsilonEquals(this.t0) && estacion2.EpsilonEquals(this.t1))
            {
                return BoundingBox2d.Union(this.p0.Add(v), this.p1.Add(v));
            }

            if (this.t0.EpsilonEquals(this.t1))
            {
                return BoundingBox2d.Union(this.p0.Add(v), this.p0.Add(v));
            }

            // Se normaliza la estacion.
            double t01_0 = (estacion1 - this.t0) / (this.t1 - this.t0);
            double t01_1 = (estacion2 - this.t0) / (this.t1 - this.t0);

            return BoundingBox2d.Union(
                this.Evaluate01(t01_0).Add(v),
                this.Evaluate01(t01_1).Add(v));
        }

        private readonly Point2d p0;
        private readonly double t0;

        private readonly Point2d p1;
        private readonly double t1;

        private readonly Vector2d dir;
        private readonly Vector2d dirN;
        private readonly double len;

        private readonly double normal;
        private readonly double radio;

        public double Project01(Point2d p)
        {
            Vector2d diff = p.Sub(this.p0);
            //return this.dir.Dot(diff) / this.len;
            return this.dirN.Dot(diff);
        }

        private Point2d Evaluate01(double t01)
        {
            //return this.p0.Add(this.dirN.Mul(t01 * this.len));
            return this.p0.Add(this.dir.Mul(t01));
        }
    }

    public class Arc
    {
        public double Evaluate(Point2d pnt)
        {
            CircleArc2 arc = new CircleArc2(this.center, this.radius, this.angle0, this.angle1);
            double d2;
            double t = arc.Project(pnt, out d2);

            // Se desnormaliza la estacion.
            return this.t0 + (this.t1 - this.t0) * t;
        }

        public void PuntoEje(double t, out Point2d pnt, out double normal, out double radio)
        {
            CircleArc2 arc = new CircleArc2(this.center, this.radius, this.angle0, this.angle1);

            if (this.t0.EpsilonEquals(this.t1))
            {
                pnt = arc.GetPosition(this.t0);
                radio = this.radius;
                normal = this.normal1;
                return;
            }

            // Se normaliza la estacion.
            double t01 = (t - this.t0) / (this.t1 - this.t0);

            pnt = arc.GetPosition(t01);
            radio = this.radius;
            normal = this.normal1 + (this.t1 - t) / this.radius;
        }

        private Point2d center;
        private double radius;

        private double angle0;
        private readonly double t0;

        private double angle1; // Si angle1 < angle0 es CW.
        private readonly double t1;

        private readonly double normal1;
    }

    public class Clothoid
    {
    }

    public class EllipseArc2d
    {
        private double angle0;
        private double angle1;

        private double angleW;

        private Vec2d center;

        private readonly double avAngle;

        private double xRadius;
        private double yRadius;
    }

    public class Ellipse2d
    {
        private double angleW;

        private Vec2d center;

        private double xRadius;
        private double yRadius;
    }
#endif
}
#endif
