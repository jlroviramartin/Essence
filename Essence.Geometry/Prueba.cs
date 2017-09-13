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

namespace Essence.Maths.Prueba
{
    public interface IBufferedTuple
    {
        void SetCoords(double[] values);
        void SetCoords(float[] values);
    }

    public interface IBufferedTuple2D : IBufferedTuple
    {
        void SetCoords(double x, double y);
        void SetCoords(float x, float y);
    }

    public interface IBufferedPoint2D : IBufferedTuple2D
    {
    }

    public interface IBufferedVector2D : IBufferedTuple2D
    {
    }

    public interface ITuple
    {
        int Dim { get; }

        void GetCoords(IBufferedTuple buff);
    }

    public interface ITuple2D : ITuple
    {
        void GetCoords(IBufferedTuple2D buff);

        double GetXDouble();
        double GetYDouble();

        float GetXFloat();
        float GetYFloat();
    }

    public interface IPoint2D : ITuple2D
    {
        IVector2D ToVector2D();
    }

    public interface IVector2D : ITuple2D
    {
        IPoint2D ToPoint2D();

        IVector2D Add(IVector2D v);
        IVector2D Sub(IVector2D v);
        IVector2D Mul(double t);

        void Add(IVector2D v, IBufferedVector2D result);
        void Sub(IVector2D v, IBufferedVector2D result);
        void Mul(double t, IBufferedVector2D result);


        void Add2(IVector2D v, IBufferedVector2D result);
    }

    public static class VMath
    {
        public static void Add(IVector2D a, IVector2D b, IBufferedVector2D result)
        {
            BufferedTuple2d aux = new BufferedTuple2d();

            a.GetCoords(aux);
            double x = aux.X;
            double y = aux.Y;

            b.GetCoords(aux);
            x += aux.X;
            y += aux.Y;

            result.SetCoords(x, y);
        }

        public static void Add2(IVector2D a, IVector2D b, IBufferedVector2D result)
        {
            result.SetCoords(a.GetXDouble() + b.GetXDouble(), a.GetYDouble() + b.GetYDouble());
        }
    }

    public class VMathX
    {
        private BufferedTuple2d aux = new BufferedTuple2d();

        public void Add(IVector2D a, IVector2D b, IBufferedVector2D result)
        {
            a.GetCoords(aux);
            double x = aux.X;
            double y = aux.Y;

            b.GetCoords(aux);
            x += aux.X;
            y += aux.Y;

            result.SetCoords(x, y);
        }

        public void Add2(IVector2D a, IVector2D b, IBufferedVector2D result)
        {
            result.SetCoords(a.GetXDouble() + b.GetXDouble(), a.GetYDouble() + b.GetYDouble());
        }
    }

    public class Tuple2d : ITuple2D
    {
        public Tuple2d(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public readonly double x, y;

        public int Dim
        {
            get { return 2; }
        }

        public void GetCoords(IBufferedTuple2D buff)
        {
            buff.SetCoords(this.x, this.y);
        }

        public void GetCoords(IBufferedTuple buff)
        {
            buff.SetCoords(new[] { this.x, this.y });
        }

        public double GetXDouble()
        {
            return this.x;
        }

        public double GetYDouble()
        {
            return this.y;
        }

        public float GetXFloat()
        {
            return (float) this.x;
        }

        public float GetYFloat()
        {
            return (float) this.y;
        }
    }

    public sealed class Vector2d : Tuple2d, IVector2D
    {
        public Vector2d(ITuple2D tuple) : base(tuple.GetXDouble(), tuple.GetYDouble())
        {
        }

        public Vector2d(Tuple2d tuple) : base(tuple.x, tuple.y)
        {
        }

        public Vector2d(double x, double y) : base(x, y)
        {
        }

        public IPoint2D ToPoint2D()
        {
            return new Point2d(this.x, this.y);
        }

        public Vector2d Add(Vector2d v)
        {
            return new Vector2d(this.x + v.x, this.y + v.y);
        }

        public Vector2d Sub(Vector2d v)
        {
            return new Vector2d(this.x - v.x, this.y - v.y);
        }

        public Vector2d Mul_2(double v)
        {
            return new Vector2d(this.x - v, this.y - v);
        }

        public void Add(Vector2d v, IBufferedVector2D result)
        {
            result.SetCoords(this.x + v.x, this.y + v.y);
        }

        public void Sub(Vector2d v, IBufferedVector2D result)
        {
            result.SetCoords(this.x - v.x, this.y - v.y);
        }

        public IVector2D Add(IVector2D v)
        {
            Vector2d v2 = v as Vector2d;
            if (v2 != null)
            {
                return this.Add(v2);
            }
            throw new Exception();
        }

        public IVector2D Sub(IVector2D v)
        {
            Vector2d v2 = v as Vector2d;
            if (v2 != null)
            {
                return this.Sub(v2);
            }
            throw new Exception();
        }

        public IVector2D Mul(double v)
        {
            return this.Mul_2(v);
        }

        public void Add(IVector2D v, IBufferedVector2D result)
        {
            Vector2d v2 = v as Vector2d;
            if (v2 != null)
            {
                this.Add(v2, result);
            }
        }

        public void Sub(IVector2D v, IBufferedVector2D result)
        {
            Vector2d v2 = v as Vector2d;
            if (v2 != null)
            {
                this.Sub(v2, result);
            }
        }

        public void Mul(double v, IBufferedVector2D result)
        {
            result.SetCoords(this.x * v, this.y * v);
        }

        public void Add2(IVector2D v, IBufferedVector2D result)
        {
            result.SetCoords(this.x + v.GetXDouble(), this.y + v.GetYDouble());
        }
    }

    public sealed class Point2d : Tuple2d, IPoint2D
    {
        public Point2d(ITuple2D tuple) : base(tuple.GetXDouble(), tuple.GetYDouble())
        {
        }

        public Point2d(Tuple2d tuple) : base(tuple.x, tuple.y)
        {
        }

        public Point2d(double x, double y) : base(x, y)
        {
        }

        public IVector2D ToVector2D()
        {
            return new Vector2d(this.x, this.y);
        }
    }

    public class BufferedTuple2d : IBufferedTuple2D, ITuple2D
    {
        public BufferedTuple2d()
        {
        }

        public BufferedTuple2d(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public double X;
        public double Y;

        public int Dim
        {
            get { return 2; }
        }

        public void GetCoords(IBufferedTuple2D buff)
        {
            buff.SetCoords(this.X, this.Y);
        }

        public void GetCoords(IBufferedTuple buff)
        {
            buff.SetCoords(new[] { this.X, this.Y });
        }

        public double GetXDouble()
        {
            return this.X;
        }

        public double GetYDouble()
        {
            return this.Y;
        }

        public float GetXFloat()
        {
            return (float)this.X;
        }

        public float GetYFloat()
        {
            return (float)this.Y;
        }

        public void SetCoords(double[] values)
        {
            values[0] = this.X;
            values[1] = this.Y;
        }

        public void SetCoords(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public void SetCoords(float[] values)
        {
            values[0] = (float) this.X;
            values[1] = (float) this.Y;
        }

        public void SetCoords(float x, float y)
        {
            this.SetCoords((double) x, (double) y);
        }
    }

    public class BufferedVector2d : BufferedTuple2d, IBufferedVector2D, IVector2D
    {
        public BufferedVector2d()
        {
        }

        public BufferedVector2d(double x, double y) : base(x, y)
        {
        }

        public IPoint2D ToPoint2D()
        {
            return new BufferedPoint2d(this.X, this.Y);
        }

        public IVector2D Add(IVector2D v)
        {
            BufferedVector2d bv = v as BufferedVector2d;
            if (bv != null)
            {
                return new BufferedVector2d(this.X + bv.X, this.Y + bv.Y);
            }
            Vector2d sv = v as Vector2d;
            if (sv != null)
            {
                return new BufferedVector2d(this.X + sv.x, this.Y + sv.y);
            }
            return new BufferedVector2d(this.X + v.GetXDouble(), this.Y + v.GetYDouble());
        }

        public IVector2D Sub(IVector2D v)
        {
            BufferedVector2d bv = v as BufferedVector2d;
            if (bv != null)
            {
                return new BufferedVector2d(this.X - bv.X, this.Y - bv.Y);
            }
            Vector2d sv = v as Vector2d;
            if (sv != null)
            {
                return new BufferedVector2d(this.X - sv.x, this.Y - sv.y);
            }
            return new BufferedVector2d(this.X - v.GetXDouble(), this.Y - v.GetYDouble());
        }

        public IVector2D Mul(double t)
        {
            return new BufferedVector2d(this.X * t, this.Y * t);
        }

        public void Add(IVector2D v, IBufferedVector2D result)
        {
            BufferedVector2d bv = v as BufferedVector2d;
            if (bv != null)
            {
                result.SetCoords(this.X + bv.X, this.Y + bv.Y);
                return;
            }
            Vector2d sv = v as Vector2d;
            if (sv != null)
            {
                result.SetCoords(this.X + sv.x, this.Y + sv.y);
                return;
            }
            result.SetCoords(this.X + v.GetXDouble(), this.Y + v.GetYDouble());
        }

        public void Sub(IVector2D v, IBufferedVector2D result)
        {
            BufferedVector2d bv = v as BufferedVector2d;
            if (bv != null)
            {
                result.SetCoords(this.X - bv.X, this.Y - bv.Y);
                return;
            }
            Vector2d sv = v as Vector2d;
            if (sv != null)
            {
                result.SetCoords(this.X - sv.x, this.Y - sv.y);
                return;
            }
            result.SetCoords(this.X - v.GetXDouble(), this.Y - v.GetYDouble());
        }

        public void Mul(double t, IBufferedVector2D result)
        {
            result.SetCoords(this.X * t, this.Y * t);
        }

        public void Add2(IVector2D v, IBufferedVector2D result)
        {
            result.SetCoords(this.X + v.GetXDouble(), this.Y + v.GetYDouble());
        }
    }

    public class BufferedPoint2d : BufferedTuple2d, IBufferedVector2D, IPoint2D
    {
        public BufferedPoint2d()
        {
        }

        public BufferedPoint2d(double x, double y) : base(x, y)
        {
        }

        public IVector2D ToVector2D()
        {
            return new BufferedVector2d(this.X, this.Y);
        }
    }


    public class BufferedVector2d_x : BufferedTuple2d
    {
        public BufferedVector2d_x()
        {
        }

        public BufferedVector2d_x(double x, double y) : base(x, y)
        {
        }

        public IPoint2D ToPoint2D()
        {
            return new BufferedPoint2d(this.X, this.Y);
        }

        public void Set(Vector2d v)
        {
            this.X = v.x;
            this.Y = v.y;
        }

        public void Set(BufferedVector2d_x v)
        {
            this.X = v.X;
            this.Y = v.Y;
        }

        public void Add(Vector2d v)
        {
            this.X += v.x;
            this.Y += v.y;
        }

        public void Add(BufferedVector2d_x v)
        {
            this.X += v.X;
            this.Y += v.Y;
        }

        public void Sub(Vector2d v)
        {
            this.X -= v.x;
            this.Y -= v.y;
        }

        public void Sub(BufferedVector2d_x v)
        {
            this.X -= v.X;
            this.Y -= v.Y;
        }

        public void Mul(double v)
        {
            this.X *= v;
            this.Y *= v;
        }

        public void Neg()
        {
            this.X = -this.X;
            this.Y = -this.Y;
        }
    }

    /*public class EvaluadorBezierCubica2D
    {
        public EvaluadorBezierCubica2D(IPoint2D p0, IPoint2D p1, IPoint2D p2, IPoint2D p3, int steps = 100)
        {
            this.p0 = p0.ToVector2D();
            this.p1 = p1.ToVector2D();
            this.p2 = p2.ToVector2D();
            this.p3 = p3.ToVector2D();
            this.steps = steps;
            this.Reset();
        }

        private readonly IVector2D p0;
        private readonly IVector2D p1;
        private readonly IVector2D p2;
        private readonly IVector2D p3;
        private readonly int steps;

        private IVector2D f;
        private IVector2D fd;
        private IVector2D fdd;
        private IVector2D fddd;
        private IVector2D fdd_per_2;
        private IVector2D fddd_per_2;
        private IVector2D fddd_per_6;
        private int loop;

        private IPoint2D current;

        public IPoint2D Current
        {
            get { return this.current; }
        }

        public void Reset()
        {
            float t = 1.0f / (float)this.steps;
            float temp = t * t;

            IVector2D v0 = this.p0;
            IVector2D v1 = this.p1;
            IVector2D v2 = this.p2;
            IVector2D v3 = this.p3;

            // Se inicializa las formulas.

            this.f = v0;

            //this.fd = 3 * (v1 - v0) * t;
            BufferedVector2d buff1 = new BufferedVector2d();
            v1.Sub(v0, buff1);
            buff1.Mul(3 * t, buff1);
            this.fd = buff1;

            //this.fdd_per_2 = 3 * (v0 - 2 * v1 + v2) * temp;
            BufferedVector2d buff2 = new BufferedVector2d();
            v1.Mul(2, buff2);
            v0.Sub(buff2, buff2);
            buff2.Add(v2, buff2);
            buff2.Mul(3 * temp, buff2);
            this.fdd_per_2 = buff2;

            //this.fddd_per_2 = 3 * (3 * (v1 - v2) + v3 - v0) * temp * t;
            BufferedVector2d buff3 = new BufferedVector2d();
            v1.Sub(v2, buff3);
            buff3.Mul(3, buff3);
            buff3.Add(v3, buff3);
            buff3.Sub(v0, buff3);
            buff3.Mul(3 * temp * t, buff3);
            this.fddd_per_2 = buff3;

            //this.fddd = this.fddd_per_2 + this.fddd_per_2;
            BufferedVector2d buff4 = new BufferedVector2d();
            this.fddd_per_2.Add(this.fddd_per_2, buff4);
            this.fddd = buff4;

            //this.fdd = this.fdd_per_2 + this.fdd_per_2;
            BufferedVector2d buff5 = new BufferedVector2d();
            this.fdd_per_2.Add(this.fdd_per_2, buff5);
            this.fdd = buff5;

            //this.fddd_per_6 = this.fddd_per_2 * (1.0f / 3.0f);
            BufferedVector2d buff6 = new BufferedVector2d();
            this.fddd_per_2.Mul(1.0 / 3.0, buff6);
            this.fddd_per_6 = buff6;

            // Se inicia el contador.
            this.loop = 0;

            this.current = null;
        }

        public bool MoveNext()
        {
            if (this.loop > this.steps)
            {
                return false;
            }

            if (this.loop < this.steps)
            {
                this.current = this.f.ToPoint2D();

                // Se calcula el siguiente paso.

                //this.f = this.f + this.fd + this.fdd_per_2 + this.fddd_per_6;
                BufferedVector2d buff1 = new BufferedVector2d();
                this.f.Add(this.fd, buff1);
                buff1.Add(this.fdd_per_2, buff1);
                buff1.Add(this.fddd_per_6, buff1);
                this.f = buff1;

                //this.fd = this.fd + this.fdd + this.fddd_per_2;
                BufferedVector2d buff2 = new BufferedVector2d();
                this.fd.Add(this.fdd, buff2);
                buff2.Add(buff2, buff2);
                this.fd = buff2;

                //this.fdd_per_2 = this.fdd_per_2 + this.fddd_per_2;
                BufferedVector2d buff3 = new BufferedVector2d();
                this.fdd_per_2.Add(this.fddd_per_2, buff3);
                this.fdd_per_2 = buff3;

                //this.fdd = this.fdd + this.fddd;
                BufferedVector2d buff4 = new BufferedVector2d();
                this.fdd.Add(this.fddd, buff4);
                this.fdd = buff4;
            }
            else
            {
                // El ultimo paso es exactamente el punto 3.
                this.current = this.p3.ToPoint2D();
            }

            // Se incrementa el contador.
            this.loop++;
            return true;
        }
    }*/

    public class EvaluadorBezierCubica2D
    {
        public EvaluadorBezierCubica2D(Point2d p0, Point2d p1, Point2d p2, Point2d p3, int steps = 100)
        {
            this.p0 = p0;
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
            this.steps = steps;
            this.Reset();
        }

        private readonly Point2d p0;
        private readonly Point2d p1;
        private readonly Point2d p2;
        private readonly Point2d p3;
        private readonly int steps;

        private Vector2d f;
        private Vector2d fd;
        private Vector2d fdd;
        private Vector2d fddd;
        private Vector2d fdd_per_2;
        private Vector2d fddd_per_2;
        private Vector2d fddd_per_6;
        private int loop;

        private Point2d current;

        public Point2d Current
        {
            get { return this.current; }
        }

        public void Reset()
        {
            double t = 1.0 / (double)this.steps;
            double temp = t * t;

            Vector2d v0 = new Vector2d(this.p0);
            Vector2d v1 = new Vector2d(this.p1);
            Vector2d v2 = new Vector2d(this.p2);
            Vector2d v3 = new Vector2d(this.p3);

            // Se inicializa las formulas.

            this.f = v0;

            //this.fd = 3 * (v1 - v0) * t;
            this.fd = v1.Sub(v0).Mul_2(3 * t);

            //this.fdd_per_2 = 3 * (v0 - 2 * v1 + v2) * temp;
            this.fdd_per_2 = v0.Sub(v1.Mul_2(2)).Add(v2).Mul_2(3 * temp);

            //this.fddd_per_2 = 3 * (3 * (v1 - v2) + v3 - v0) * temp * t;
            this.fddd_per_2 = v1.Sub(v2).Mul_2(3).Add(v3).Sub(v0).Mul_2(3 * temp * t);

            //this.fddd = this.fddd_per_2 + this.fddd_per_2;
            this.fddd = this.fddd_per_2.Add(this.fddd_per_2);

            //this.fdd = this.fdd_per_2 + this.fdd_per_2;
            this.fdd = this.fdd_per_2.Add(this.fdd_per_2);

            //this.fddd_per_6 = this.fddd_per_2 * (1.0 / 3.0);
            this.fddd_per_6 = this.fddd_per_2.Mul_2(1.0 / 3.0);

            // Se inicia el contador.
            this.loop = 0;

            this.current = null;
        }

        public bool MoveNext()
        {
            if (this.loop > this.steps)
            {
                return false;
            }

            if (this.loop < this.steps)
            {
                this.current = new Point2d(this.f);

                // Se calcula el siguiente paso.

                //this.f = this.f + this.fd + this.fdd_per_2 + this.fddd_per_6;
                this.f = this.f.Add(this.fd).Add(this.fdd_per_2).Add(this.fddd_per_6);

                //this.fd = this.fd + this.fdd + this.fddd_per_2;
                this.fd = this.fd.Add(this.fdd).Add(this.fddd_per_2);

                //this.fdd_per_2 = this.fdd_per_2 + this.fddd_per_2;
                this.fdd_per_2 = this.fdd_per_2.Add(this.fddd_per_2);

                //this.fdd = this.fdd + this.fddd;
                this.fdd = this.fdd.Add(this.fddd);
            }
            else
            {
                // El ultimo paso es exactamente el punto 3.
                this.current = new Point2d(this.p3);
            }

            // Se incrementa el contador.
            this.loop++;
            return true;
        }
    }

    public class EvaluadorBezierCubica2D_x
    {
        public EvaluadorBezierCubica2D_x(Point2d p0, Point2d p1, Point2d p2, Point2d p3, int steps = 100)
        {
            this.p0 = p0;
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
            this.steps = steps;
            this.Reset();
        }

        private readonly Point2d p0;
        private readonly Point2d p1;
        private readonly Point2d p2;
        private readonly Point2d p3;
        private readonly int steps;

        private BufferedVector2d_x f = new BufferedVector2d_x();
        private BufferedVector2d_x fd = new BufferedVector2d_x();
        private BufferedVector2d_x fdd = new BufferedVector2d_x();
        private BufferedVector2d_x fddd = new BufferedVector2d_x();
        private BufferedVector2d_x fdd_per_2 = new BufferedVector2d_x();
        private BufferedVector2d_x fddd_per_2 = new BufferedVector2d_x();
        private BufferedVector2d_x fddd_per_6 = new BufferedVector2d_x();
        private int loop;

        private Point2d current;

        public Point2d Current
        {
            get { return this.current; }
        }

        public void Reset()
        {
            double t = 1.0 / (double)this.steps;
            double temp = t * t;

            Vector2d v0 = new Vector2d(this.p0);
            Vector2d v1 = new Vector2d(this.p1);
            Vector2d v2 = new Vector2d(this.p2);
            Vector2d v3 = new Vector2d(this.p3);

            // Se inicializa las formulas.

            this.f.Set(v0);

            //this.fd = 3 * (v1 - v0) * t;
            this.fd.Set(v1);
            this.fd.Sub(v0);
            this.fd.Mul(3 * t);

            //this.fdd_per_2 = 3 * (v0 - 2 * v1 + v2) * temp;
            this.fdd_per_2.Set(v1);
            this.fdd_per_2.Mul(2);
            this.fdd_per_2.Neg();
            this.fdd_per_2.Add(v0);
            this.fdd_per_2.Add(v2);
            this.fdd_per_2.Mul(3 * temp);

            //this.fddd_per_2 = 3 * (3 * (v1 - v2) + v3 - v0) * temp * t;
            this.fddd_per_2.Set(v1);
            this.fddd_per_2.Sub(v2);
            this.fddd_per_2.Mul(3);
            this.fddd_per_2.Add(v3);
            this.fddd_per_2.Sub(v0);
            this.fddd_per_2.Mul(3 * temp * t);

            //this.fddd = this.fddd_per_2 + this.fddd_per_2;
            this.fddd.Set(this.fddd_per_2);
            this.fddd.Mul(2);

            //this.fdd = this.fdd_per_2 + this.fdd_per_2;
            this.fdd.Set(this.fdd_per_2);
            this.fdd.Mul(2);

            //this.fddd_per_6 = this.fddd_per_2 * (1.0 / 3.0);
            this.fddd_per_6.Set(this.fddd_per_2);
            this.fddd_per_6.Mul(1.0 / 3.0);

            // Se inicia el contador.
            this.loop = 0;

            this.current = null;
        }

        public bool MoveNext()
        {
            if (this.loop > this.steps)
            {
                return false;
            }

            if (this.loop < this.steps)
            {
                this.current = new Point2d(this.f);

                // Se calcula el siguiente paso.

                //this.f = this.f + this.fd + this.fdd_per_2 + this.fddd_per_6;
                this.f.Add(this.fd);
                this.f.Add(this.fdd_per_2);
                this.f.Add(this.fddd_per_6);

                //this.fd = this.fd + this.fdd + this.fddd_per_2;
                this.fd.Add(this.fdd);
                this.fd.Add(this.fddd_per_2);

                //this.fdd_per_2 = this.fdd_per_2 + this.fddd_per_2;
                this.fdd_per_2.Add(this.fddd_per_2);

                //this.fdd = this.fdd + this.fddd;
                this.fdd.Add(this.fddd);
            }
            else
            {
                // El ultimo paso es exactamente el punto 3.
                this.current = new Point2d(this.p3);
            }

            // Se incrementa el contador.
            this.loop++;
            return true;
        }
    }

    public sealed class EvaluadorBezierCubica2D_2
    {
        public EvaluadorBezierCubica2D_2(Essence.Geometry.Core.Double.Point2d p0, Essence.Geometry.Core.Double.Point2d p1, Essence.Geometry.Core.Double.Point2d p2, Essence.Geometry.Core.Double.Point2d p3, int steps = 100)
        {
            this.p0 = p0;
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
            this.steps = steps;
            this.Reset();
        }

        private readonly Essence.Geometry.Core.Double.Point2d p0;
        private readonly Essence.Geometry.Core.Double.Point2d p1;
        private readonly Essence.Geometry.Core.Double.Point2d p2;
        private readonly Essence.Geometry.Core.Double.Point2d p3;
        private readonly int steps;

        private Essence.Geometry.Core.Double.Vector2d f;
        private Essence.Geometry.Core.Double.Vector2d fd;
        private Essence.Geometry.Core.Double.Vector2d fdd;
        private Essence.Geometry.Core.Double.Vector2d fddd;
        private Essence.Geometry.Core.Double.Vector2d fdd_per_2;
        private Essence.Geometry.Core.Double.Vector2d fddd_per_2;
        private Essence.Geometry.Core.Double.Vector2d fddd_per_6;
        private int loop;

        private Essence.Geometry.Core.Double.Point2d? current;

        public Essence.Geometry.Core.Double.Point2d Current
        {
            get
            {
                if (this.current == null)
                {
                    throw new InvalidOperationException();
                }
                return (Essence.Geometry.Core.Double.Point2d)this.current;
            }
        }

        public void Reset()
        {
            float t = 1.0f / (float)this.steps;
            float temp = t * t;

            Essence.Geometry.Core.Double.Vector2d v0 = (Essence.Geometry.Core.Double.Vector2d)this.p0;
            Essence.Geometry.Core.Double.Vector2d v1 = (Essence.Geometry.Core.Double.Vector2d)this.p1;
            Essence.Geometry.Core.Double.Vector2d v2 = (Essence.Geometry.Core.Double.Vector2d)this.p2;
            Essence.Geometry.Core.Double.Vector2d v3 = (Essence.Geometry.Core.Double.Vector2d)this.p3;

            // Se inicializa las formulas.
            this.f = v0;
            this.fd = (v1 - v0) * (3 * t);
            this.fdd_per_2 = (v0 - 2 * v1 + v2) * (3 * temp);
            this.fddd_per_2 = (3 * (v1 - v2) + v3 - v0) * (3 * temp * t);
            this.fddd = 2 * this.fddd_per_2;
            this.fdd = 2 * this.fdd_per_2;
            this.fddd_per_6 = this.fddd_per_2 * (1.0f / 3.0f);

            // Se inicia el contador.
            this.loop = 0;

            this.current = null;
        }

        public bool MoveNext()
        {
            if (this.loop > this.steps)
            {
                return false;
            }

            if (this.loop < this.steps)
            {
                this.current = (Essence.Geometry.Core.Double.Point2d)this.f;

                // Se calcula el siguiente paso.
                this.f = this.f + this.fd + this.fdd_per_2 + this.fddd_per_6;
                this.fd = this.fd + this.fdd + this.fddd_per_2;
                this.fdd_per_2 = this.fdd_per_2 + this.fddd_per_2;
                this.fdd = this.fdd + this.fddd;
            }
            else
            {
                // El ultimo paso es exactamente el punto 3.
                this.current = this.p3;
            }

            // Se incrementa el contador.
            this.loop++;
            return true;
        }
    }
}
#endif
