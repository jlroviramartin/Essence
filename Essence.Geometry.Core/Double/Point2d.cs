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
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Runtime.Serialization;
using Essence.Util.Math;
using Essence.Util.Math.Double;
using SysMath = System.Math;

namespace Essence.Geometry.Core.Double
{
    public struct Point2d : IPoint2, ITuple2_Double,
                            IEpsilonEquatable<Point2d>,
                            IEquatable<Point2d>,
                            IFormattable,
                            ISerializable
    {
        /// <summary>Name of the property X.</summary>
        public const string _X = "X";

        /// <summary>Name of the property Y.</summary>
        public const string _Y = "Y";

        private const double EPSILON = MathUtils.EPSILON;

        /// <summary>Point zero.</summary>
        public static readonly Point2d Zero = new Point2d(0, 0);

        /// <summary>Point one.</summary>
        public static readonly Point2d One = new Point2d(1, 1);

        /// <summary>Point with property X = 1 and others = 0.</summary>
        public static readonly Point2d UX = new Point2d(1, 0);

        /// <summary>Point with property Y = 1 and others = 0.</summary>
        public static Point2d UY = new Point2d(0, 1);

        public Point2d(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public Point2d(IPoint2 p)
        {
            ITuple2_Double _p = p.AsTupleDouble();
            this.X = _p.X;
            this.Y = _p.Y;
        }

        /// <summary>Property X.</summary>
        public readonly double X;

        /// <summary>Property Y.</summary>
        public readonly double Y;

        #region operators

        public static explicit operator double[](Point2d v)
        {
            return new[] { v.X, v.Y };
        }

        public static Point2d operator +(Point2d p, Vector2d v)
        {
            return p.Add(v);
        }

        public static Point2d operator -(Point2d p, Vector2d v)
        {
            return p.Sub(v);
        }

        public static Vector2d operator -(Point2d p1, Point2d p2)
        {
            return p1.Sub(p2);
        }

        public static implicit operator Vector2d(Point2d p)
        {
            return new Vector2d(p.X, p.Y);
        }

        #endregion

        [Pure]
        public int Dim
        {
            get { return 2; }
        }

        [Pure]
        public double this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0:
                        return this.X;
                    case 1:
                        return this.Y;
                }
                throw new IndexOutOfRangeException();
            }
        }

        [Pure]
        public bool IsNaN
        {
            get { return double.IsNaN(this.X) || double.IsNaN(this.Y); }
        }

        [Pure]
        public Point2d Add(Vector2d v)
        {
            return new Point2d(this.X + v.X, this.Y + v.Y);
        }

        [Pure]
        public Point2d Sub(Vector2d v)
        {
            return new Point2d(this.X - v.X, this.Y - v.Y);
        }

        [Pure]
        public Vector2d Sub(Point2d p2)
        {
            return new Vector2d(this.X - p2.X, this.Y - p2.Y);
        }

        [Pure]
        public double Distance2To(Point2d p2)
        {
            return MathUtils.Cuad(p2.X - this.X) + MathUtils.Cuad(p2.Y - this.Y);
        }

        [Pure]
        public double DistanceTo(Point2d p2)
        {
            return Math.Sqrt(this.Distance2To(p2));
        }

        [Pure]
        public Point2d Lerp(Point2d p2, double alpha)
        {
            return this.Lineal(p2, 1 - alpha, alpha);
        }

        [Pure]
        public double InvLerp(Point2d p2, Point2d pLerp)
        {
            Vector2d v12 = p2.Sub(this);
            return v12.Proj(pLerp.Sub(this));
        }

        [Pure]
        public Point2d Lineal(Point2d p2, double alpha, double beta)
        {
            return new Point2d(alpha * this.X + beta * p2.X, alpha * this.Y + beta * p2.Y);
        }

        /// <summary>
        /// Evaluates the angle of <c>p2 - 0</c> with respect to <c>p1 - 0</c>.
        /// Es positivo si el giro es sentido horario [0, PI].
        /// Es negativo si el giro es sentido anti-horario [-PI, 0].
        /// <pre><![CDATA[
        ///               __
        ///              _/| p2
        ///            _/
        ///          _/
        ///        _/ __
        ///      _/   |\ angulo +
        ///    _/       |
        ///   +--------------------> p1
        /// Origin      |
        ///       \_  |/  angulo -
        ///         \_|--
        ///           \_
        ///             \_
        ///               \|
        ///              --| p2
        /// ]]></pre>
        /// </summary>
        /// <param name="o">Origin.</param>
        /// <param name="p1">Base point.</param>
        /// <param name="p2">Other point.</param>
        /// <returns>Angle in radians between [-PI, PI]</returns>
        public static double EvAngle(Point2d o, Point2d p1, Point2d p2)
        {
            return (p1 - o).AngleTo(p2 - o);
        }

        /// <summary>
        /// This method tests if the {@code p} point is at the left, on or at the right of the {@code a, b} line
        /// {@link http://www.softsurfer.com/Archive/algorithm_0103/algorithm_0103.htm}.
        /// <ul>
        ///     <li>+1 if the {@code p} point is at the left of the line</li>
        ///     <li>0 if the {@code p} point is on the line</li>
        ///     <li>-1 if the {@code p} point is at the right of the line</li>
        /// </ul>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="p"></param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        public static int IsLeft(Point2d a, Point2d b, Point2d p, double epsilon)
        {
            return (int)WhichSide(a, b, p, epsilon);
        }

        /// <summary>
        /// This method tests if the {@code p} point is at the left, on or at the right of the {@code a, b} line
        /// {@link http://www.softsurfer.com/Archive/algorithm_0103/algorithm_0103.htm}.
        /// <ul>
        ///     <li>+1 if the {@code p} point is at the left of the line</li>
        ///     <li>0 if the {@code p} point is on the line</li>
        ///     <li>-1 if the {@code p} point is at the right of the line</li>
        /// </ul>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static int IsLeft(Point2d a, Point2d b, Point2d p)
        {
            return IsLeft(a, b, p, EPSILON);
        }

        /// <summary>
        /// This method tests if the {@code p} point is at the left, on or at the right of the {@code a, b} line
        /// {@link http://www.softsurfer.com/Archive/algorithm_0103/algorithm_0103.htm}.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="p"></param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        public static LineSide WhichSide(Point2d a, Point2d b, Point2d p, double epsilon)
        {
            double v = ((b.X - a.X) * (p.Y - a.Y) - (b.Y - a.Y) * (p.X - a.X));
            return (MathUtils.EpsilonEquals(v, 0, epsilon) ? LineSide.Middle : (v < 0 ? LineSide.Right : LineSide.Left));
        }

        /// <summary>
        /// This method tests if the {@code p} point is at the left, on or at the right of the {@code a, b} line
        /// {@link http://www.softsurfer.com/Archive/algorithm_0103/algorithm_0103.htm}.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static LineSide WhichSide(Point2d a, Point2d b, Point2d p)
        {
            return WhichSide(a, b, p, EPSILON);
        }

        #region parse

        public static Point2d Parse(string s,
                                    IFormatProvider provider = null,
                                    VectorStyles vstyle = VectorStyles.All,
                                    NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands)
        {
            Point2d result;
            if (!TryParse(s, out result, provider, vstyle, style))
            {
                throw new Exception();
            }
            return result;
        }

        public static bool TryParse(string s,
                                    out Point2d result,
                                    IFormatProvider provider = null,
                                    VectorStyles vstyle = VectorStyles.All,
                                    NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands)
        {
            Contract.Requires(s != null);

            double[] ret;
            if (!VectorUtils.TryParse(s, 2, out ret, double.TryParse, provider, vstyle, style))
            {
                result = Zero;
                return false;
            }
            result = new Point2d(ret[0], ret[1]);
            return true;
        }

        #endregion

        #region private

        [Pure]
        private bool EpsilonEquals(double x, double y, double epsilon = EPSILON)
        {
            return this.X.EpsilonEquals(x, epsilon) && this.Y.EpsilonEquals(y, epsilon);
        }

        [Pure]
        private bool Equals(double x, double y)
        {
            return (this.X == x) && (this.Y == y);
        }

        #endregion

        #region object

        [Pure]
        public override string ToString()
        {
            return this.ToString("F3", null);
        }

        [Pure]
        public override bool Equals(object obj)
        {
            return (obj is IPoint2) && this.Equals((IPoint2)obj);
        }

        [Pure]
        public override int GetHashCode()
        {
            return VectorUtils.GetHashCode(this.X, this.Y);
        }

        #endregion

        #region IEpsilonEquatable<Point2d>

        [Pure]
        public bool EpsilonEquals(Point2d other, double epsilon = EPSILON)
        {
            return this.EpsilonEquals(other.X, other.Y, epsilon);
        }

        #endregion

        #region IEpsilonEquatable<IPoint2>

        [Pure]
        public bool EpsilonEquals(IPoint2 other, double epsilon = EPSILON)
        {
            ITuple2_Double _other = other.AsTupleDouble();
            return this.EpsilonEquals(_other.X, _other.Y, epsilon);
        }

        #endregion

        #region IEquatable<Point2d>

        [Pure]
        public bool Equals(Point2d other)
        {
            return this.Equals(other.X, other.Y);
        }

        #endregion

        #region IEquatable<IPoint2d>

        [Pure]
        public bool Equals(IPoint2 other)
        {
            ITuple2_Double _other = other.AsTupleDouble();
            return this.Equals(_other.X, _other.Y);
        }

        #endregion

        #region IFormattable

        [Pure]
        public string ToString(string format, IFormatProvider provider)
        {
            if (provider != null)
            {
                ICustomFormatter formatter = provider.GetFormat(this.GetType()) as ICustomFormatter;
                if (formatter != null)
                {
                    return formatter.Format(format, this, provider);
                }
            }

            return VectorUtils.ToString(provider, format, (double[])this);
        }

        #endregion

        #region ISerializable

        public Point2d(SerializationInfo info, StreamingContext context)
        {
            this.X = info.GetDouble(_X);
            this.Y = info.GetDouble(_Y);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(_X, this.X);
            info.AddValue(_Y, this.Y);
        }

        #endregion

        #region ITuple

        [Pure]
        public bool IsValid
        {
            get { return MathUtils.IsValid(this.X) && MathUtils.IsValid(this.Y); }
        }

        [Pure]
        public bool IsInfinity
        {
            get { return double.IsInfinity(this.X) || double.IsInfinity(this.Y); }
        }

        [Pure]
        public bool IsZero
        {
            get { return this.EpsilonEquals(0, 0); }
        }

        #endregion

        #region ITuple2

        public void Get(IOpTuple2 setter)
        {
            IOpTuple2_Double _setter = setter.AsOpTupleDouble();
            _setter.Set(this.X, this.Y);
        }

        #endregion

        #region ITuple2_Double

        double ITuple2_Double.X
        {
            get { return this.X; }
        }

        double ITuple2_Double.Y
        {
            get { return this.Y; }
        }

        #endregion

        #region IPoint

        [Pure]
        public double InvLerp(IPoint2 p2, IPoint2 pLerp)
        {
            BuffVector2d v12 = new BuffVector2d();
            v12.Sub(p2, this);
            BuffVector2d v1Lerp = new BuffVector2d();
            v1Lerp.Sub(pLerp, this);
            return v12.Proj(v1Lerp);
        }

        #endregion

        #region IPoint2

        [Pure]
        public int Quadrant
        {
            get
            {
                return ((this.X >= 0)
                    ? ((this.Y >= 0) ? 0 : 3)
                    : ((this.Y >= 0) ? 1 : 2));
            }
        }

        #endregion
    }
}