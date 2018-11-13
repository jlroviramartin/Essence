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
using Essence.Util.Math.Int;

namespace Essence.Geometry.Core.Integer
{
    public struct Point2i : IPoint2, ITuple2_Integer,
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

        public Point2i(IPoint2 p)
        {
            ITuple2_Integer _p = p.AsTupleInteger();
            this.X = _p.X;
            this.Y = _p.Y;
        }

        /// <summary>Property X.</summary>
        public readonly int X;

        /// <summary>Property Y.</summary>
        public readonly int Y;

        #region operators

        public static explicit operator int[](Point2i v)
        {
            return new[] { v.X, v.Y };
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
            return Math.Sqrt(this.Distance2To(p2));
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
            return v12.Proj(pLerp.Sub(this));
        }

        [Pure]
        public Point2i Lineal(Point2i p2, double alpha, double beta)
        {
            return new Point2i((int)(alpha * this.X + beta * p2.X), (int)(alpha * this.Y + beta * p2.Y));
        }

        #region parse

        public static Point2i Parse(string s,
                                    IFormatProvider provider = null,
                                    VectorStyles vstyle = VectorStyles.All,
                                    NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands)
        {
            Point2i result;
            if (!TryParse(s, out result, provider, vstyle, style))
            {
                throw new Exception();
            }
            return result;
        }

        public static bool TryParse(string s,
                                    out Point2i result,
                                    IFormatProvider provider = null,
                                    VectorStyles vstyle = VectorStyles.All,
                                    NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands)
        {
            Contract.Requires(s != null);

            int[] ret;
            if (!VectorUtils.TryParse(s, 2, out ret, int.TryParse, provider, vstyle, style))
            {
                result = Zero;
                return false;
            }
            result = new Point2i(ret[0], ret[1]);
            return true;
        }

        #endregion

        #region private

        [Pure]
        private bool Equals(int x, int y)
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
            return (obj is Point2i) && this.Equals((Point2i)obj);
        }

        [Pure]
        public override int GetHashCode()
        {
            return VectorUtils.GetHashCode(this.X, this.Y);
        }

        #endregion

        #region IEpsilonEquatable<IPoint2>

        [Pure]
        bool IEpsilonEquatable<IPoint2>.EpsilonEquals(IPoint2 other, double epsilon)
        {
            ITuple2_Integer _other = other.AsTupleInteger();
            return this.Equals(_other.X, _other.Y);
        }

        #endregion

        #region IEquatable<IPoint2>

        [Pure]
        public bool Equals(IPoint2 other)
        {
            ITuple2_Integer _other = other.AsTupleInteger();
            return this.Equals(_other.X, _other.Y);
        }

        #endregion

        #region IEquatable<Point2i>

        [Pure]
        public bool Equals(Point2i other)
        {
            return this.Equals(other.X, other.Y);
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

        #region ITuple

        [Pure]
        bool ITuple<IPoint2>.IsValid
        {
            get { return true; }
        }

        [Pure]
        bool ITuple<IPoint2>.IsInfinity
        {
            get { return false; }
        }

        [Pure]
        public bool IsZero
        {
            get { return this.Equals(0, 0); }
        }

        #endregion

        #region ITuple2

        public void Get(IOpTuple2 setter)
        {
            IOpTuple2_Integer _setter = setter.AsOpTupleInteger();
            _setter.Set(this.X, this.Y);
        }

        #endregion

        #region ITuple2_Int

        int ITuple2_Integer.X
        {
            get { return this.X; }
        }

        int ITuple2_Integer.Y
        {
            get { return this.Y; }
        }

        #endregion

        #region IPoint

        [Pure]
        public double InvLerp(IPoint2 p2, IPoint2 pLerp)
        {
            return this.InvLerp(p2.ToPoint2i(), pLerp.ToPoint2i());
        }

        #endregion

        #region IPoint

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