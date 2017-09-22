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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Runtime.Serialization;
using Essence.Util.Math;
using Essence.Util.Math.Int;

namespace Essence.Geometry.Core.Int
{
    public struct Point2i : IPoint2D,
                            IEquatable<Point2i>,
                            IFormattable,
                            ISerializable
    {
        /// <summary>Name of the property X.</summary>
        public const string _X = "X";

        /// <summary>Name of the property Y.</summary>
        public const string _Y = "Y";

        /// <summary>Point zero.</summary>
        public static readonly Point2i Zero = new Point2i(0, 0);

        /// <summary>Point one.</summary>
        public static readonly Point2i One = new Point2i(1, 1);

        /// <summary>Point with property X = 1 and others = 0.</summary>
        public static readonly Point2i UX = new Point2i(1, 0);

        /// <summary>Point with property Y = 1 and others = 0.</summary>
        public static readonly Point2i UY = new Point2i(0, 1);

        public Point2i(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public Point2i(IPoint2D p)
        {
            CoordinateSetter2i setter = new CoordinateSetter2i();
            p.GetCoordinates(setter);
            this.X = setter.X;
            this.Y = setter.Y;
        }

        public Point2i(IPoint p)
        {
            IPoint2D p2 = p as IPoint2D;
            if (p2 != null)
            {
                CoordinateSetter2i setter = new CoordinateSetter2i();
                p2.GetCoordinates(setter);
                this.X = setter.X;
                this.Y = setter.Y;
            }
            else
            {
                if (p.Dim < 2)
                {
                    throw new Exception("Punto no valido");
                }
                CoordinateSetter2i setter = new CoordinateSetter2i();
                p.GetCoordinates(setter);
                this.X = setter.X;
                this.Y = setter.Y;
            }
        }

        /// <summary>Property X.</summary>
        public readonly int X;

        /// <summary>Property Y.</summary>
        public readonly int Y;

        #region operators

        /// <summary>
        /// Casting to an array.
        /// </summary>
        public static explicit operator int[](Point2i v)
        {
            return new int[] { v.X, v.Y };
        }

        public static Point2i operator +(Point2i p, Vector2i v)
        {
            return p.Add(v);
        }

        public static Point2i operator -(Point2i p, Vector2i v)
        {
            return p.Sub(v);
        }

        public static Vector2i operator -(Point2i p1, Point2i p2)
        {
            return p1.Sub(p2);
        }

        public static implicit operator Vector2i(Point2i p)
        {
            return new Vector2i(p.X, p.Y);
        }

        #endregion

        [Pure]
        public int Dim
        {
            get { return 2; }
        }

        [Pure]
        public int this[int i]
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

        /// <summary>
        /// Tests if <code>this</code> point is zero (all coordinates are 0).
        /// </summary>
        [Pure]
        public bool IsZero
        {
            get { return this.Equals(0, 0); }
        }

        /// <summary>
        /// Counterclockwise quadrant:
        /// <pre><![CDATA[
        ///       ^
        ///   1   |   0
        ///       |
        /// <-----+-----> p1
        ///       |
        ///   2   |   3
        ///       v
        /// ]]></pre>
        /// </summary>
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

        [Pure]
        public Point2i Add(Vector2i v)
        {
            return new Point2i(this.X + v.X, this.Y + v.Y);
        }

        [Pure]
        public Point2i Sub(Vector2i v)
        {
            return new Point2i(this.X - v.X, this.Y - v.Y);
        }

        [Pure]
        public Vector2i Sub(Point2i p2)
        {
            return new Vector2i(this.X - p2.X, this.Y - p2.Y);
        }

        [Pure]
        public double Distance2To(Point2i p2)
        {
            return MathUtils.Cuad(p2.X - this.X) + MathUtils.Cuad(p2.Y - this.Y);
        }

        [Pure]
        public double DistanceTo(Point2i p2)
        {
            return (double)Math.Sqrt(this.Distance2To(p2));
        }

        [Pure]
        public Point2i Lerp(Point2i p2, double alpha)
        {
            return this.Lineal(p2, 1 - alpha, alpha);
        }

        [Pure]
        public double InvLerp(Point2i p2, Point2i pLerp)
        {
            Vector2i v12 = p2.Sub(this);
            return v12.Proy(pLerp.Sub(this));
        }

        [Pure]
        public Point2i Lineal(Point2i p2, double alpha, double beta)
        {
            return new Point2i((int)(alpha * this.X + beta * p2.X), (int)(alpha * this.Y + beta * p2.Y));
        }

        #region parse

        /// <summary>
        /// Parses the <code>s</code> string using <code>vstyle</code> and <code>nstyle</code> styles.
        /// </summary>
        /// <param name="s">String.</param>
        /// <param name="provider">Provider.</param>
        /// <param name="vstyle">Vector style.</param>
        /// <param name="nstyle">Number style.</param>
        /// <returns>Point.</returns>
        public static Point2i Parse(string s,
                                    IFormatProvider provider = null,
                                    VectorStyles vstyle = VectorStyles.All,
                                    NumberStyles nstyle = NumberStyles.Float | NumberStyles.AllowThousands)
        {
            Point2i result;
            if (!TryParse(s, out result, provider, vstyle, nstyle))
            {
                throw new Exception();
            }
            return result;
        }

        /// <summary>
        /// Tries to parse the <code>s</code> string using <code>vstyle</code> and <code>nstyle</code> styles.
        /// </summary>
        /// <param name="s">String.</param>
        /// <param name="provider">Provider.</param>
        /// <param name="vstyle">Vector style.</param>
        /// <param name="nstyle">Number style.</param>
        /// <param name="result">Point.</param>
        /// <returns><code>True</code> if everything is correct, <code>false</code> otherwise.</returns>
        public static bool TryParse(string s,
                                    out Point2i result,
                                    IFormatProvider provider = null,
                                    VectorStyles vstyle = VectorStyles.All,
                                    NumberStyles nstyle = NumberStyles.Float | NumberStyles.AllowThousands)
        {
            Contract.Requires(s != null);

            int[] ret;
            if (!VectorUtils.TryParse(s, 2, out ret, int.TryParse, provider, vstyle, nstyle))
            {
                result = Zero;
                return false;
            }
            result = new Point2i(ret[0], ret[1]);
            return true;
        }

        #endregion

        #region private

        /// <summary>
        /// Tests if the coordinates of <code>this</code> point are equals to <code>x</code> and <code>y</code>.
        /// </summary>
        [Pure]
        private bool Equals(int x, int y)
        {
            return this.X == x && this.Y == y;
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
            if (!(obj is Point2i))
            {
                return false;
            }

            return this.Equals((Point2i)obj);
        }

        [Pure]
        public override int GetHashCode()
        {
            return VectorUtils.GetHashCode(this.X, this.Y);
        }

        #endregion

        #region IEquatable

        [Pure]
        public bool Equals(Point2i other)
        {
            return other.X == this.X && other.Y == this.Y;
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

            return VectorUtils.ToString(provider, format, (int[])this);
        }

        #endregion

        #region ISerializable

        public Point2i(SerializationInfo info, StreamingContext context)
        {
            this.X = info.GetInt32(_X);
            this.Y = info.GetInt32(_Y);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(_X, this.X);
            info.AddValue(_Y, this.Y);
        }

        #endregion

        #region IPoint

        //[Pure]
        //int Dim { get; }

        void IPoint.GetCoordinates(ICoordinateSetter setter)
        {
            setter.SetCoords(this.X, this.Y);
        }

        [Pure]
        IPoint IPoint.Add(IVector v)
        {
            return this.Add(v.ToVector2i());
        }

        [Pure]
        IPoint IPoint.Sub(IVector v)
        {
            return this.Sub(v.ToVector2i());
        }

        [Pure]
        IVector IPoint.Sub(IPoint p2)
        {
            return this.Sub(p2.ToPoint2i());
        }

        [Pure]
        IPoint IPoint.Lerp(IPoint p2, double alpha)
        {
            return this.Lerp(p2.ToPoint2i(), alpha);
        }

        [Pure]
        double IPoint.InvLerp(IPoint p2, IPoint pLerp)
        {
            return this.InvLerp(p2.ToPoint2i(), pLerp.ToPoint2i());
        }

        [Pure]
        IPoint IPoint.Lineal(IPoint p2, double alpha, double beta)
        {
            return this.Lineal(p2.ToPoint2i(), alpha, beta);
        }

        #endregion

        #region IPoint2D

        void IPoint2D.GetCoordinates(ICoordinateSetter2D setter)
        {
            setter.SetCoords(this.X, this.Y);
        }

        #endregion

        #region IEpsilonEquatable<IPoint>

        [Pure]
        bool IEpsilonEquatable<IPoint>.EpsilonEquals(IPoint other, double epsilon)
        {
            return this.Equals(other.ToPoint2i());
        }

        #endregion

        #region inner classes

        /// <summary>
        /// This class compares points by coordinate (X or Y).
        /// </summary>
        public sealed class CoordComparer : IComparer<Point2i>, IComparer
        {
            public CoordComparer(int coord)
            {
                this.coord = coord;
            }

            private readonly int coord;

            public int Compare(Point2i v1, Point2i v2)
            {
                switch (this.coord)
                {
                    case 0:
                        return v1.X.CompareTo(v2.X);
                    case 1:
                        return v1.Y.CompareTo(v2.Y);
                }
                throw new IndexOutOfRangeException();
            }

            int IComparer.Compare(object o1, object o2)
            {
                Contract.Requires(o1 is Point2i && o2 is Point2i);
                return this.Compare((Point2i)o1, (Point2i)o2);
            }
        }

        /// <summary>
        /// This class lexicographically compares points: it compares X -> Y.
        /// </summary>
        public sealed class LexComparer : IComparer<Point2i>, IComparer
        {
            public int Compare(Point2i v1, Point2i v2)
            {
                int i;
                i = v1.X.CompareTo(v2.X);
                if (i != 0)
                {
                    return i;
                }
                i = v1.Y.CompareTo(v2.Y);
                return i;
            }

            int IComparer.Compare(object o1, object o2)
            {
                Contract.Requires(o1 is Point2i && o2 is Point2i);
                return this.Compare((Point2i)o1, (Point2i)o2);
            }
        }

        #endregion
    }
}