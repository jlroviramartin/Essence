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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Runtime.Serialization;

namespace Essence.Geometry.Core.Int
{
    public struct Vector2i : IVector2D,
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

        public Vector2i(IVector2D v)
        {
            CoordinateSetter2i setter = new CoordinateSetter2i();
            v.GetCoordinates(setter);
            this.X = setter.X;
            this.Y = setter.Y;
        }

        public Vector2i(IVector v)
        {
            IVector2D v2 = v as IVector2D;
            if (v2 != null)
            {
                CoordinateSetter2i setter = new CoordinateSetter2i();
                v2.GetCoordinates(setter);
                this.X = setter.X;
                this.Y = setter.Y;
            }
            else
            {
                if (v.Dim < 2)
                {
                    throw new Exception("Vector no valido");
                }
                CoordinateSetter2i setter = new CoordinateSetter2i();
                v.GetCoordinates(setter);
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
        public static explicit operator int[](Vector2i v)
        {
            return new int[] { v.X, v.Y };
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

        /// <summary>
        /// Tests if <code>this</code> vector is zero (all coordinates are 0).
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
        public int Cross(Vector2i v2)
        {
            return this.X * v2.Y - this.Y * v2.X;
        }

        [Pure]
        public double Length
        {
            get { return (double)Math.Sqrt(this.LengthSquared); }
        }

        [Pure]
        public double LengthSquared
        {
            get { return this.Dot(this); }
        }

        [Pure]
        public double LengthL1
        {
            get { return Math.Abs(this.X) + Math.Abs(this.Y); }
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
        public Vector2i Mul(int c)
        {
            return new Vector2i(this.X * c, this.Y * c);
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
            return v12.Proy(vLerp.Sub(this));
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
        public double Proy(Vector2i v2)
        {
            return this.Dot(v2) / this.Length;
        }

        [Pure]
        public Vector2i ProyV(Vector2i v2)
        {
            return this.Mul(this.Proy(v2));
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
        public static Vector2i Parse(string s,
                                     IFormatProvider provider = null,
                                     VectorStyles vstyle = VectorStyles.All,
                                     NumberStyles nstyle = NumberStyles.Float | NumberStyles.AllowThousands)
        {
            Vector2i result;
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
        /// <param name="result">Vector.</param>
        /// <returns><code>True</code> if everything is correct, <code>false</code> otherwise.</returns>
        public static bool TryParse(string s,
                                    out Vector2i result,
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
            result = new Vector2i(ret[0], ret[1]);
            return true;
        }

        #endregion

        #region private

        [Pure]
        private Vector2i Unit
        {
            get
            {
                double len = this.Length;
                if (Essence.Util.Math.Double.MathUtils.EpsilonZero(len))
                {
                    return Zero;
                }
                return this.Div(len);
            }
        }

        [Pure]
        private Vector2i Mul(double c)
        {
            return new Vector2i((int)(this.X * c), (int)(this.Y * c));
        }

        [Pure]
        private Vector2i Div(double c)
        {
            return new Vector2i((int)(this.X / c), (int)(this.Y / c));
        }

        /// <summary>
        /// Tests if the coordinates of <code>this</code> vector are equals to <code>x</code> and <code>y</code>.
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
            if (!(obj is Vector2i))
            {
                return false;
            }

            return this.Equals((Vector2i)obj);
        }

        [Pure]
        public override int GetHashCode()
        {
            return VectorUtils.GetHashCode(this.X, this.Y);
        }

        #endregion

        #region IEquatable<Vector2i>

        [Pure]
        public bool Equals(Vector2i other)
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

        #region IVector

        //int Dim { get; }

        void IVector.GetCoordinates(ICoordinateSetter setter)
        {
            setter.SetCoords(this.X, this.Y);
        }

        [Pure]
        IVector IVector.Unit
        {
            get { return this.Unit; }
        }

        //[Pure]
        //REAL Length { get; }

        //[Pure]
        //REAL LengthSquared { get; }

        //[Pure]
        //REAL LengthL1 { get; }

        [Pure]
        IVector IVector.Add(IVector v2)
        {
            return this.Add(v2.ToVector2i());
        }

        [Pure]
        IVector IVector.Sub(IVector v2)
        {
            return this.Sub(v2.ToVector2i());
        }

        [Pure]
        IVector IVector.Mul(double c)
        {
            return this.Mul(c);
        }

        [Pure]
        IVector IVector.Div(double c)
        {
            return this.Div(c);
        }

        [Pure]
        IVector IVector.SimpleMul(IVector v2)
        {
            return this.SimpleMul(v2.ToVector2i());
        }

        [Pure]
        IVector IVector.Neg()
        {
            return this.Neg();
        }

        [Pure]
        IVector IVector.Abs()
        {
            return this.Abs();
        }

        [Pure]
        IVector IVector.Lerp(IVector v2, double alpha)
        {
            return this.Lerp(v2.ToVector2i(), alpha);
        }

        [Pure]
        double IVector.InvLerp(IVector v2, IVector vLerp)
        {
            return this.InvLerp(v2.ToVector2i(), vLerp.ToVector2i());
        }

        [Pure]
        IVector IVector.Lineal(IVector v2, double alpha, double beta)
        {
            return this.Lineal(v2.ToVector2i(), alpha, beta);
        }

        [Pure]
        double IVector.Dot(IVector v2)
        {
            return this.Dot(v2.ToVector2i());
        }

        [Pure]
        double IVector.Proj(IVector v2)

        {
            return this.Proy(v2.ToVector2i());
        }

        [Pure]
        IVector IVector.ProjV(IVector v2)
        {
            return this.ProyV(v2.ToVector2i());
        }

        #endregion

        #region IVector2D

        void IVector2D.GetCoordinates(ICoordinateSetter2D setter)
        {
            setter.SetCoords(this.X, this.Y);
        }

        [Pure]
        double IVector2D.Cross(IVector2D v2)
        {
            return this.Cross(v2.ToVector2i());
        }

        #endregion

        #region IEpsilonEquatable<IVector>

        [Pure]
        bool IEpsilonEquatable<IVector>.EpsilonEquals(IVector other, double epsilon)
        {
            return this.Equals(other.ToVector2i());
        }

        #endregion

        #region inner classes

        /// <summary>
        /// This class compares vectors by coordinate (X or Y).
        /// </summary>
        public sealed class CoordComparer : IComparer<Vector2i>, IComparer
        {
            public CoordComparer(int coord)
            {
                this.coord = coord;
            }

            private readonly int coord;

            public int Compare(Vector2i v1, Vector2i v2)
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
                Contract.Requires(o1 is Vector2i && o2 is Vector2i);
                return this.Compare((Vector2i)o1, (Vector2i)o2);
            }
        }

        /// <summary>
        /// This class lexicographically compares vectors: it compares X -> Y.
        /// </summary>
        public sealed class LexComparer : IComparer<Vector2i>, IComparer
        {
            public int Compare(Vector2i v1, Vector2i v2)
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
                Contract.Requires(o1 is Vector2i && o2 is Vector2i);
                return this.Compare((Vector2i)o1, (Vector2i)o2);
            }
        }

        #endregion
    }
}