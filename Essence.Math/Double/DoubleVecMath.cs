using System;
using SysMath = System.Math;

namespace Essence.Math.Double
{
    public struct DoubleMath : IMath<double>
    {
        public static readonly DoubleMath Instance = new DoubleMath();

        public double Add(double a, double b)
        {
            return a + b;
        }

        public double Sub(double a, double b)
        {
            return a - b;
        }

        public double Mul(double a, double b)
        {
            return a * b;
        }

        public double Div(double a, double b)
        {
            return a / b;
        }

        public double Neg(double a)
        {
            return -a;
        }

        public double Sqrt(double a)
        {
            return SysMath.Sqrt(a);
        }

        public double Atan2(double y, double x)
        {
            return SysMath.Atan2(y, x);
        }

        public double ToValue<TConvertible>(TConvertible c) where TConvertible : struct, IConvertible
        {
            return c.ToDouble(null);
        }
    }

    public struct Vec2d : IVec2<double, Vec2d>
    {
        public static Vec2d Zero = new Vec2d(0, 0);

        public Vec2d(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        int IVec<double, Vec2d>.Dim
        {
            get { return 2; }
        }

        void IVec<double, Vec2d>.Get(double[] array)
        {
            array[0] = this.X;
            array[1] = this.Y;
        }

        public Vec2d Add(Vec2d v)
        {
            return new Vec2d(this.X + v.X, this.Y + v.Y);
        }

        public Vec2d Sub(Vec2d v)
        {
            return new Vec2d(this.X - v.X, this.Y - v.Y);
        }

        public Vec2d Mul(double v)
        {
            return new Vec2d(this.X * v, this.Y * v);
        }

        public Vec2d Div(double v)
        {
            return new Vec2d(this.X / v, this.Y / v);
        }

        public Vec2d Norm()
        {
            return this.Div(this.Length);
        }

        public double Length2
        {
            get { return this.X * this.X + this.Y * this.Y; }
        }

        public double Length
        {
            get { return SysMath.Sqrt(this.Length2); }
        }

        public double Dot(Vec2d b)
        {
            return this.X * b.X + this.Y * b.Y;
        }

        public double Cross(Vec2d b)
        {
            return this.X * b.Y - this.Y * b.X;
        }

        double IVec1<double, Vec2d>.X
        {
            get { return this.X; }
        }

        double IVec2<double, Vec2d>.Y
        {
            get { return this.Y; }
        }

        public bool EpsilonEquals(Vec2d vec, double error)
        {
            return this.X.EpsilonEquals(vec.X, error) && this.Y.EpsilonEquals(vec.Y, error);
        }

        public readonly double X;
        public readonly double Y;

        public override string ToString()
        {
            return string.Format("{0:F2}; {1:F2}", this.X, this.Y);
        }
    }

    public struct Vec2dFactory : IVec2Factory<double, Vec2d>
    {
        public Vec2d New(double x, double y)
        {
            return new Vec2d(x, y);
        }
    }

    public struct Vec3d : IVec3<double, Vec3d>
    {
        public static Vec3d Zero = new Vec3d(0, 0, 0);

        public Vec3d(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public static Vec3d FromRGB(byte r, byte g, byte b)
        {
            return new Vec3d((double)r / 255, (double)g / 255, (double)b / 255);
        }

        int IVec<double, Vec3d>.Dim
        {
            get { return 3; }
        }

        void IVec<double, Vec3d>.Get(double[] array)
        {
            array[0] = this.X;
            array[1] = this.Y;
            array[2] = this.Z;
        }

        public Vec3d Add(Vec3d v)
        {
            return new Vec3d(this.X + v.X, this.Y + v.Y, this.Z + v.Z);
        }

        public Vec3d Sub(Vec3d v)
        {
            return new Vec3d(this.X - v.X, this.Y - v.Y, this.Z - v.Z);
        }

        public Vec3d Mul(double v)
        {
            return new Vec3d(this.X * v, this.Y * v, this.Z * v);
        }

        public Vec3d Div(double v)
        {
            return new Vec3d(this.X / v, this.Y / v, this.Z / v);
        }

        public Vec3d Norm()
        {
            return this.Div(this.Length);
        }

        public double Length2
        {
            get { return this.X * this.X + this.Y * this.Y + this.Z * this.Z; /* this.Dot(this) */ }
        }

        public double Length
        {
            get { return SysMath.Sqrt(this.Length2); }
        }

        public double Dot(Vec3d b)
        {
            return this.X * b.X + this.Y * b.Y + this.Z * b.Z;
        }

        double IVec2<double, Vec3d>.Cross(Vec3d b)
        {
            return this.Cross(b).Length;
        }

        double IVec1<double, Vec3d>.X
        {
            get { return this.X; }
        }

        double IVec2<double, Vec3d>.Y
        {
            get { return this.Y; }
        }

        double IVec3<double, Vec3d>.Z
        {
            get { return this.Z; }
        }

        public Vec3d Cross(Vec3d v)
        {
            return new Vec3d((this.Y * v.Z) - (this.Z * v.Y),
                             (this.Z * v.X) - (this.X * v.Z),
                             (this.X * v.Y) - (this.Y * v.X));
        }

        public bool EpsilonEquals(Vec3d vec, double error)
        {
            return this.X.EpsilonEquals(vec.X, error) && this.Y.EpsilonEquals(vec.Y, error) && this.Z.EpsilonEquals(vec.Z, error);
        }

        public readonly double X;
        public readonly double Y;
        public readonly double Z;

        public override string ToString()
        {
            return string.Format("{0:F2}; {1:F2}; {2:F2}", this.X, this.Y, this.Z);
        }
    }

    public struct Vec3dFactory : IVec3Factory<double, Vec3d>, IVec2Factory<double, Vec3d>
    {
        public Vec3d New(double x, double y, double z)
        {
            return new Vec3d(x, y, z);
        }

        Vec3d IVec2Factory<double, Vec3d>.New(double x, double y)
        {
            return new Vec3d(x, y, 0);
        }
    }

    public abstract class Transform2 : ITransform2<Transform2, double, Vec2d>
    {
        public static Transform2 Identity()
        {
            return new MatrixTransform2(
                1, 0, 0,
                0, 1, 0);
        }

        public static Transform2 Rotate(double r)
        {
            double c = SysMath.Cos(r);
            double s = SysMath.Sin(r);
            return new MatrixTransform2(
                c, -s, 0,
                s, c, 0);
        }

        public static Transform2 Rotate(Vec2d pt, double r)
        {
            return Rotate(pt.X, pt.Y, r);
        }

        public static Transform2 Rotate(double px, double py, double r)
        {
            double c = SysMath.Cos(r);
            double s = SysMath.Sin(r);
            return new MatrixTransform2(
                c, -s, -px * c + py * s + px,
                s, c, -px * s - py * c + py);
        }

        public static Transform2 Translate(Vec2d t)
        {
            return Translate(t.X, t.Y);
        }

        public static Transform2 Translate(double dx, double dy)
        {
            return new MatrixTransform2(
                1, 0, dx,
                0, 1, dy);
        }

        public static Transform2 Translate(double px, double py, double px2, double py2)
        {
            return Translate(px2 - px, py2 - py);
        }

        public static Transform2 Scale(double ex, double ey)
        {
            return new MatrixTransform2(
                ex, 0, 0,
                0, ey, 0);
        }

        public static Transform2 Scale(double px, double py, double ex, double ey)
        {
            return new MatrixTransform2(
                ex, 0, px - ex * px,
                0, ey, py - ey * py);
        }

        public abstract Vec2d TransformVector(Vec2d v);

        public abstract Vec2d TransformPoint(Vec2d v);

        public abstract Transform2 Mult(Transform2 t);
    }

    public class MatrixTransform2 : Transform2
    {
        public MatrixTransform2(double a, double b, double tx, double c, double d, double ty)
        {
            this.a = a;
            this.b = b;
            this.tx = tx;

            this.c = c;
            this.d = d;
            this.ty = ty;
        }

        public MatrixTransform2 Mult(MatrixTransform2 t)
        {
            return new MatrixTransform2(this.a * t.a + this.b * t.c,
                                        this.a * t.b + this.b * t.d,
                                        this.a * t.tx + this.b * t.ty + this.tx,
                                        this.c * t.a + this.d * t.c,
                                        this.c * t.b + this.d * t.d,
                                        this.c * t.tx + this.d * t.ty + this.ty);
        }

        public override Vec2d TransformVector(Vec2d v)
        {
            return new Vec2d(v.X * this.a + v.Y * this.b, v.X * this.c + v.Y * this.d);
        }

        public override Vec2d TransformPoint(Vec2d v)
        {
            return new Vec2d(v.X * this.a + v.Y * this.b + this.tx, v.X * this.c + v.Y * this.d + this.ty);
        }

        public override Transform2 Mult(Transform2 t)
        {
            return this.Mult((MatrixTransform2)t);
        }

        private readonly double a, b, tx;
        private readonly double c, d, ty;
    }
}