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

using Essence.Util.Math;
using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Runtime.Serialization;
using Essence.Util.Math.Double;

namespace Essence.Geometry.Core.Int
{
    public struct Vector2i : IVector2, ITuple2_Int,
                             IEquatable<Vector2i>,
                             IFormattable,
                             ISerializable
    {
        /// <summary>Name of the property X.</summary>
        public const string _X = "X";

        /// <summary>Name of the property Y.</summary>
        public const string _Y = "Y";

        /// <summary>Vector zero.</summary>
        public static readonly Vector2i Zero = new Vector2i(0, 0);

        /// <summary>Vector one.</summary>
        public static readonly Vector2i One = new Vector2i(1, 1);

        /// <summary>Vector with property X = 1 and others = 0.</summary>
        public static readonly Vector2i UX = new Vector2i(1, 0);

        /// <summary>Vector with property Y = 1 and others = 0.</summary>
        public static readonly Vector2i UY = new Vector2i(0, 1);

        public Vector2i(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public Vector2i(IVector2 v)
        {
            ITuple2_Int _v = v.AsTupleInt();
            this.X = _v.X;
            this.Y = _v.Y;
        }

        /// <summary>Property X.</summary>
        public readonly int X;

        /// <summary>Property Y.</summary>
        public readonly int Y;

        #region operators

        public static explicit operator int[](Vector2i v)
        {
            return new[] { v.X, v.Y };
        }

        public static Vector2i operator -(Vector2i v1)
        {
            return v1.Neg();
        }

        public static Vector2i operator +(Vector2i v1, Vector2i v2)
        {
            return v1.Add(v2);
        }

        public static Vector2i operator -(Vector2i v1, Vector2i v2)
        {
            return v1.Sub(v2);
        }

        public static Vector2i operator *(Vector2i v, int c)
        {
            return v.Mul(c);
        }

        public static Vector2i operator *(int c, Vector2i v)
        {
            return v.Mul(c);
        }

        public static Vector2i operator /(Vector2i v, int c)
        {
            return v.Div(c);
        }

        public static implicit operator Point2i(Vector2i p)
        {
            return new Point2i(p.X, p.Y);
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
        public int Cross(Vector2i v2)
        {
            return this.X * v2.Y - this.Y * v2.X;
        }

        [Pure]
        public Vector2i Add(Vector2i v2)
        {
            return new Vector2i(this.X + v2.X, this.Y + v2.Y);
        }

        [Pure]
        public Vector2i Sub(Vector2i v2)
        {
            return new Vector2i(this.X - v2.X, this.Y - v2.Y);
        }

        [Pure]
        public Vector2i Mul(double c)
        {
            return new Vector2i((int)(this.X * c), (int)(this.Y * c));
        }

        [Pure]
        public Vector2i Mul(int c)
        {
            return new Vector2i(this.X * c, this.Y * c);
        }

        [Pure]
        public Vector2i Div(double c)
        {
            return new Vector2i((int)(this.X / c), (int)(this.Y / c));
        }

        [Pure]
        public Vector2i Div(int c)
        {
            return new Vector2i(this.X / c, this.Y / c);
        }

        [Pure]
        public Vector2i SimpleMul(Vector2i v2)
        {
            return new Vector2i(this.X * v2.X, this.Y * v2.Y);
        }

        [Pure]
        public Vector2i Neg()
        {
            return new Vector2i(-this.X, -this.Y);
        }

        [Pure]
        public Vector2i Abs()
        {
            return new Vector2i(Math.Abs(this.X), Math.Abs(this.Y));
        }

        [Pure]
        public Vector2i Lerp(Vector2i v2, double alpha)
        {
            return this.Lineal(v2, 1 - alpha, alpha);
        }

        [Pure]
        public double InvLerp(Vector2i v2, Vector2i vLerp)
        {
            Vector2i v12 = v2.Sub(this);
            return v12.Proj(vLerp.Sub(this));
        }

        [Pure]
        public Vector2i Lineal(Vector2i v2, double alpha, double beta)
        {
            return new Vector2i((int)(alpha * this.X + beta * v2.X), (int)(alpha * this.Y + beta * v2.Y));
        }

        [Pure]
        public int Dot(Vector2i v2)
        {
            return this.X * v2.X + this.Y * v2.Y;
        }

        [Pure]
        public double Proj(Vector2i v2)
        {
            return this.Dot(v2) / this.Length;
        }

        [Pure]
        public Vector2i ProjV(Vector2i v2)
        {
            return this.Mul(this.Proj(v2));
        }

        #region parse

        public static Vector2i Parse(string s,
                                     IFormatProvider provider = null,
                                     VectorStyles vstyle = VectorStyles.All,
                                     NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands)
        {
            Vector2i result;
            if (!TryParse(s, out result, provider, vstyle, style))
            {
                throw new Exception();
            }
            return result;
        }

        public static bool TryParse(string s,
                                    out Vector2i result,
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
            result = new Vector2i(ret[0], ret[1]);
            return true;
        }

        #endregion

        #region private

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
            return (obj is Vector2i) && this.Equals((Vector2i)obj);
        }

        [Pure]
        public override int GetHashCode()
        {
            return VectorUtils.GetHashCode(this.X, this.Y);
        }

        #endregion

        #region IEpsilonEquatable<IVector2>

        [Pure]
        bool IEpsilonEquatable<IVector2>.EpsilonEquals(IVector2 other, double epsilon)
        {
            ITuple2_Int _other = other.AsTupleInt();
            return this.Equals(_other.X, _other.Y);
        }

        #endregion

        #region IEquatable<Vector2i>

        [Pure]
        public bool Equals(Vector2i other)
        {
            return this.Equals(other.X, other.Y);
        }

        #endregion

        #region IEquatable<IVector2>

        [Pure]
        public bool Equals(IVector2 other)
        {
            ITuple2_Int _other = other.AsTupleInt();
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

            return VectorUtils.ToString(provider, format, (int[])this);
        }

        #endregion

        #region ISerializable

        public Vector2i(SerializationInfo info, StreamingContext context)
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
        bool ITuple<IVector2>.IsValid
        {
            get { return true; }
        }

        [Pure]
        bool ITuple<IVector2>.IsInfinity
        {
            get { return false; }
        }

        [Pure]
        public bool IsZero
        {
            get { return this.Equals(0, 0); }
        }

        #endregion

        #region ITuple2_Int

        int ITuple2_Int.X
        {
            get { return this.X; }
        }

        int ITuple2_Int.Y
        {
            get { return this.Y; }
        }

        #endregion

        #region IVector

        [Pure]
        bool IVector<IVector2>.IsUnit
        {
            get { return this.Length.EpsilonEquals(1); }
        }

        [Pure]
        public double Length
        {
            get { return Math.Sqrt(this.Length2); }
        }

        [Pure]
        public double Length2
        {
            get { return this.Dot(this); }
        }

        [Pure]
        public double LengthL1
        {
            get { return Math.Abs(this.X) + Math.Abs(this.Y); }
        }

        [Pure]
        public double Dot(IVector2 v2)
        {
            ITuple2_Int _v2 = v2.AsTupleInt();
            return this.X * _v2.X + this.Y * _v2.Y;
        }

        [Pure]
        public double Proj(IVector2 v2)
        {
            return this.Dot(v2) / this.Length;
        }

        [Pure]
        public double Cross(IVector2 v2)
        {
            ITuple2_Int _v2 = v2.AsTupleInt();
            return this.X * _v2.Y - this.Y * _v2.X;
        }

        [Pure]
        public double InvLerp(IVector2 v2, IVector2 vLerp)
        {
            return this.InvLerp(v2.ToVector2i(), vLerp.ToVector2i());
        }

        #endregion

        #region IVector2

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