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

using Essence.Util.Math;
using System;
using System.Diagnostics.Contracts;
using SysMath = System.Math;

namespace Essence.Maths.Double
{
    public interface IMath<T>
        where T : struct, IConvertible
    {
        [Pure]
        T Zero { get; }

        [Pure]
        T One { get; }

        [Pure]
        T Add(T a, T b);

        [Pure]
        T Sub(T a, T b);

        [Pure]
        T Mul(T a, T b);

        [Pure]
        T Div(T a, T b);

        [Pure]
        T Neg(T a);

        [Pure]
        T Sqrt(T a);

        [Pure]
        T Atan2(T y, T x);

        T ToValue<TConvertible>(TConvertible c) where TConvertible : struct, IConvertible;
    }

    public interface IVec<T, TVec>
        where T : struct, IConvertible
    {
        [Pure]
        int Dim { get; }

        [Pure]
        void Get(T[] array);

        [Pure]
        TVec Neg();

        [Pure]
        TVec Add(TVec b);

        [Pure]
        TVec Sub(TVec b);

        [Pure]
        TVec Mul(T v);

        [Pure]
        TVec Div(T v);

        [Pure]
        TVec Add(T alpha, TVec b, T beta);

        [Pure]
        TVec Norm();

        [Pure]
        T Length { get; }

        bool EpsilonEquals(TVec vec, double error);
    }

    public interface IVec1<T, TVec> : IVec<T, TVec>
        where T : struct, IConvertible
    {
        [Pure]
        T X { get; }
    }

    public interface IVec2<T, TVec> : IVec1<T, TVec>
        where T : struct, IConvertible
    {
        [Pure]
        T Y { get; }

        /// <summary>
        ///     Operacion producto escalar (dot): this · vector. Tiene en cuenta el signo.
        ///     A · B = |A| * |B| * cos( angulo( A, B ) )
        ///     <see cref="http://en.wikipedia.org/wiki/Dot_product" />
        ///     <pre><![CDATA[
        ///          
        ///  (-)    |   (+)     __ vector
        ///         |          _/|
        ///         |       _/   |
        ///         |    _/      |
        ///         | _/         |
        /// --------+------------+------> this
        /// ]]></pre>
        ///     Si <c> </c> esta normalizado, es la proyeccion de <c>vector</c> sobre <c>this</c>.
        /// </summary>
        [Pure]
        T Dot(TVec v);

        /// <summary>
        ///     Operacion producto vectorial (cross) en 3D: this x vector. Tiene en cuenta el signo.
        ///     A x B = |A| * |B| * sin( angulo( A, B ) )
        ///     <see cref="http://en.wikipedia.org/wiki/Cross_product" />
        ///     <code><![CDATA[
        /// ^ this x vector ( + )
        /// |
        /// |   _
        /// |   /| vector
        /// |  /
        /// | /
        /// +----------> this
        /// ]]></code>
        /// </summary>
        [Pure]
        T Cross(TVec v);
    }

    public interface IVec2Factory<T, TVec>
        where T : struct, IConvertible
        where TVec : IVec2<T, TVec>
    {
        [Pure]
        TVec New(T x, T y);
    }

    public interface IVec3<T, TVec> : IVec2<T, TVec>
        where T : struct, IConvertible
    {
        [Pure]
        T Z { get; }

        /// <summary>
        ///     Operacion producto vectorial (cross) en 3D: this x vector. Tiene en cuenta el signo.
        ///     A x B = |A| * |B| * sin( angulo( A, B ) )
        ///     <see cref="http://en.wikipedia.org/wiki/Cross_product" />
        ///     <code><![CDATA[
        /// ^ this x vector ( + )
        /// |
        /// |   _
        /// |   /| vector
        /// |  /
        /// | /
        /// +----------> this
        /// ]]></code>
        /// </summary>
        [Pure]
        new TVec Cross(TVec v);
    }

    public interface IVec3Factory<T, TVec>
        where T : struct, IConvertible
        where TVec : IVec3<T, TVec>
    {
        [Pure]
        TVec New(T x, T y, T z);
    }

    public struct VecMath<T, TMath, TVec, TFac> : IVec2Factory<T, TVec>
        where T : struct, IConvertible
        where TMath : IMath<T>, new()
        where TVec : IVec2<T, TVec>
        where TFac : IVec2Factory<T, TVec>, new()
    {
        public static readonly VecMath<T, TMath, TVec, TFac> Instance = new VecMath<T, TMath, TVec, TFac>();

        [Pure]
        public T Length(TVec a, TVec b)
        {
            return b.Sub(a).Length;
        }

        [Pure]
        public T Project(TVec a, TVec b)
        {
            return a.Norm().Dot(b);
        }

        [Pure]
        public TVec PerpLeft(TVec a)
        {
            return fac.New(math.Neg(a.Y), a.X);
        }

        [Pure]
        public TVec PerpRight(TVec a)
        {
            return fac.New(a.Y, math.Neg(a.X));
        }

        [Pure]
        public T Distance2(TVec p0, TVec p1)
        {
            return p1.Sub(p0).Length;
        }

        [Pure]
        public T Distance(TVec p0, TVec p1)
        {
            return math.Sqrt(this.Distance2(p0, p1));
        }

        /// <summary>
        ///     Calcula el angulo de <c>this</c> respecto del eje X.
        ///     <pre><![CDATA[
        ///   ^           __
        ///   |          _/| this
        ///   |        _/
        ///   |      _/
        ///   |    _/ __
        ///   |  _/   |\ angulo +
        ///   |_/       |
        /// --+------------> X
        /// origen      |
        ///   |   \_  |/  angulo -
        ///   |     \_|--
        ///   |       \_
        ///   |         \_
        ///   |           \|
        ///   v          --| this
        /// ]]></pre>
        /// </summary>
        [Pure]
        /*public T Angle(TVec v)
        {
            return math.Atan2(v.Y, v.X);
        }*/
        public double Angle(TVec v)
        {
            return SysMath.Atan2(v.Y.ToDouble(), v.X.ToDouble());
        }

        [Pure]
        public double Angle(TVec a, TVec b)
        {
            //return (this.Angle(b) - this.Angle(a));
            // http://stackoverflow.com/questions/2150050/finding-signed-angle-between-vectors
            double ax = a.X.ToDouble();
            double ay = a.Y.ToDouble();
            double bx = b.X.ToDouble();
            double by = b.Y.ToDouble();
            return SysMath.Atan2(ax * by - ay * bx, ax * bx + ay * by);
        }

        [Pure]
        public TVec Rotate(TVec a, double angle)
        {
            double s = SysMath.Sin(angle);
            double c = SysMath.Cos(angle);
            double ax = a.X.ToDouble();
            double ay = a.Y.ToDouble();
            return fac.New(math.ToValue(ax * c - ay * s), math.ToValue(ax * s + ay * c));
        }

        [Pure]
        public TVec NewRotate(double angle, double len = 1)
        {
            return fac.New(math.ToValue(len * SysMath.Cos(angle)), math.ToValue(len * SysMath.Sin(angle)));
        }

        [Pure]
        public TVec Interpolate(TVec v0, TVec v1, T alpha)
        {
            return v0.Add(alpha, v1, math.Sub(math.One, alpha));
        }

        private static readonly TMath math = new TMath();
        private static readonly TFac fac = new TFac();

        #region IVec2Factory<T,TVec>

        public TVec New(T x, T y)
        {
            return fac.New(x, y);
        }

        #endregion
    }

    public interface ITransform2<TTransform, T, TVec>
        where TTransform : ITransform2<TTransform, T, TVec>
        where TVec : IVec2<T, TVec>
        where T : struct, IConvertible
    {
        TVec TransformVector(TVec v);

        TVec TransformPoint(TVec v);

        TTransform Mult(TTransform t);
    }
}