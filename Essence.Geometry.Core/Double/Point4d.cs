﻿// Copyright 2017 Jose Luis Rovira Martin
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
using Essence.Util.Math.Double;
using SysMath = System.Math;

namespace Essence.Geometry.Core.Double
{
    public struct Point4d : IPoint4D,
                            IEpsilonEquatable<Point4d>,
                            IEquatable<Point4d>,
                            IFormattable,
                            ISerializable
    {
        /// <summary>Name of the property X.</summary>
        public const string _X = "X";

        /// <summary>Name of the property Y.</summary>
        public const string _Y = "Y";

        /// <summary>Name of the property Z.</summary>
        public const string _Z = "Z";

        /// <summary>Name of the property W.</summary>
        public const string _W = "W";

        /// <summary>Point zero.</summary>
        public static readonly Point4d Zero = new Point4d(0, 0, 0, 0);

        /// <summary>Point one.</summary>
        public static readonly Point4d One = new Point4d(1, 1, 1, 1);

        /// <summary>Point with property X = 1 and others = 0.</summary>
        public static readonly Point4d UX = new Point4d(1, 0, 0, 0);

        /// <summary>Point with property Y = 1 and others = 0.</summary>
        public static readonly Point4d UY = new Point4d(0, 1, 0, 0);

        /// <summary>Point with property Z = 1 and others = 0.</summary>
        public static readonly Point4d UZ = new Point4d(0, 0, 1, 0);

        /// <summary>Point with property W = 1 and others = 0.</summary>
        public static readonly Point4d UW = new Point4d(0, 0, 0, 1);

        public Point4d(double x, double y, double z, double w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        public Point4d(IPoint4D p)
        {
            CoordinateSetter4d setter = new CoordinateSetter4d();
            p.GetCoordinates(setter);
            this.X = setter.X;
            this.Y = setter.Y;
            this.Z = setter.Z;
            this.W = setter.W;
        }

        public Point4d(IPoint p)
        {
            IPoint4D p4 = p as IPoint4D;
            if (p4 != null)
            {
                CoordinateSetter4d setter = new CoordinateSetter4d();
                p4.GetCoordinates(setter);
                this.X = setter.X;
                this.Y = setter.Y;
                this.Z = setter.Z;
                this.W = setter.W;
            }
            else
            {
                if (p.Dim < 4)
                {
                    throw new Exception("Punto no valido");
                }
                CoordinateSetter4d setter = new CoordinateSetter4d();
                p.GetCoordinates(setter);
                this.X = setter.X;
                this.Y = setter.Y;
                this.Z = setter.Z;
                this.W = setter.W;
            }
        }

        /// <summary>Property X.</summary>
        public readonly double X;

        /// <summary>Property Y.</summary>
        public readonly double Y;

        /// <summary>Property Z.</summary>
        public readonly double Z;

        /// <summary>Property W.</summary>
        public readonly double W;

        #region operators

        /// <summary>
        /// Casting to an array.
        /// </summary>
        public static explicit operator double[](Point4d v)
        {
            return new[] { v.X, v.Y, v.Z, v.W };
        }

        public static Point4d operator +(Point4d p, Vector4d v)
        {
            return p.Add(v);
        }

        public static Point4d operator -(Point4d p, Vector4d v)
        {
            return p.Sub(v);
        }

        public static Vector4d operator -(Point4d p1, Point4d p2)
        {
            return p1.Sub(p2);
        }

        public static implicit operator Vector4d(Point4d p)
        {
            return new Vector4d(p.X, p.Y, p.Z, p.W);
        }

        #endregion

        [Pure]
        public int Dim
        {
            get { return 4; }
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
                    case 2:
                        return this.Z;
                    case 3:
                        return this.W;
                }
                throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// Tests if <code>this</code> point is valid (not any coordinate is NaN or infinity).
        /// </summary>
        [Pure]
        public bool IsValid
        {
            get { return MathUtils.IsValid(this.X) && MathUtils.IsValid(this.Y) && MathUtils.IsValid(this.Z) && MathUtils.IsValid(this.W); }
        }

        /// <summary>
        /// Tests if <code>this</code> point is NaN (any coordinate is NaN).
        /// </summary>
        [Pure]
        public bool IsNaN
        {
            get { return double.IsNaN(this.X) || double.IsNaN(this.Y) || double.IsNaN(this.Z) || double.IsNaN(this.W); }
        }

        /// <summary>
        /// Tests if <code>this</code> point is infinity (any coordinate is infinity).
        /// </summary>
        [Pure]
        public bool IsInfinity
        {
            get { return double.IsInfinity(this.X) || double.IsInfinity(this.Y) || double.IsInfinity(this.Z) || double.IsInfinity(this.W); }
        }

        /// <summary>
        /// Tests if <code>this</code> point is zero (all coordinates are 0).
        /// </summary>
        [Pure]
        public bool IsZero
        {
            get { return this.EpsilonEquals(0, 0, 0, 0); }
        }

        [Pure]
        public Point4d Add(Vector4d v)
        {
            return new Point4d(this.X + v.X, this.Y + v.Y, this.Z + v.Z, this.W + v.W);
        }

        [Pure]
        public Point4d Sub(Vector4d v)
        {
            return new Point4d(this.X - v.X, this.Y - v.Y, this.Z - v.Z, this.W - v.W);
        }

        [Pure]
        public Vector4d Sub(Point4d p2)
        {
            return new Vector4d(this.X - p2.X, this.Y - p2.Y, this.Z - p2.Z, this.W - p2.W);
        }

        [Pure]
        public double Distance2To(Point4d p2)
        {
            return (MathUtils.Cuad(p2.X - this.X) + MathUtils.Cuad(p2.Y - this.Y) + MathUtils.Cuad(p2.Z - this.Z) + MathUtils.Cuad(p2.W - this.W));
        }

        [Pure]
        public double DistanceTo(Point4d p2)
        {
            return (double)Math.Sqrt(this.Distance2To(p2));
        }

        [Pure]
        public Point4d Lerp(Point4d p2, double alpha)
        {
            return this.Lineal(p2, 1 - alpha, alpha);
        }

        [Pure]
        public double InvLerp(Point4d p2, Point4d p3)
        {
            Vector4d v12 = p2.Sub(this);
            return v12.Proy(p3.Sub(this));
        }

        [Pure]
        public Point4d Lineal(Point4d p2, double alpha, double beta)
        {
            return new Point4d(alpha * this.X + beta * p2.X, alpha * this.Y + beta * p2.Y, alpha * this.Z + beta * p2.Z, alpha * this.W + beta * p2.W);
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
        public static Point4d Parse(string s,
                                    IFormatProvider provider = null,
                                    VectorStyles vstyle = VectorStyles.All,
                                    NumberStyles nstyle = NumberStyles.Float | NumberStyles.AllowThousands)
        {
            Point4d result;
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
                                    out Point4d result,
                                    IFormatProvider provider = null,
                                    VectorStyles vstyle = VectorStyles.All,
                                    NumberStyles nstyle = NumberStyles.Float | NumberStyles.AllowThousands)
        {
            Contract.Requires(s != null);

            double[] ret;
            if (!VectorUtils.TryParse(s, 4, out ret, double.TryParse, provider, vstyle, nstyle))
            {
                result = Zero;
                return false;
            }
            result = new Point4d(ret[0], ret[1], ret[2], ret[3]);
            return true;
        }

        #endregion

        #region protected

        #endregion

        #region private

        /// <summary>
        /// Tests if the coordinates of <code>this</code> point are equals to <code>x</code>, <code>y</code>, <code>z</code> and <code>w</code>.
        /// </summary>
        [Pure]
        private bool EpsilonEquals(double x, double y, double z, double w, double epsilon = MathUtils.ZERO_TOLERANCE)
        {
            return this.X.EpsilonEquals(x, epsilon) && this.Y.EpsilonEquals(y, epsilon) && this.Z.EpsilonEquals(z, epsilon) && this.W.EpsilonEquals(w, epsilon);
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
            if (!(obj is Point4d))
            {
                return false;
            }

            return this.Equals((Point4d)obj);
        }

        [Pure]
        public override int GetHashCode()
        {
            return VectorUtils.GetHashCode(this.X, this.Y, this.Z, this.W);
        }

        #endregion

        #region IEpsilonEquatable

        [Pure]
        public bool EpsilonEquals(Point4d other, double epsilon = MathUtils.EPSILON)
        {
            return this.EpsilonEquals(other.X, other.Y, other.Z, other.W, (double)epsilon);
        }

        #endregion

        #region IEquatable

        [Pure]
        public bool Equals(Point4d other)
        {
            return other.X == this.X && other.Y == this.Y && other.Z == this.Z && other.W == this.W;
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

        public Point4d(SerializationInfo info, StreamingContext context)
        {
            this.X = info.GetDouble(_X);
            this.Y = info.GetDouble(_Y);
            this.Z = info.GetDouble(_Z);
            this.W = info.GetDouble(_W);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(_X, this.X);
            info.AddValue(_Y, this.Y);
            info.AddValue(_Z, this.Z);
            info.AddValue(_W, this.W);
        }

        #endregion

        #region IPoint

        //[Pure]
        //int Dim { get; }

        void IPoint.GetCoordinates(ICoordinateSetter setter)
        {
            setter.SetCoords(this.X, this.Y, this.Z, this.W);
        }

        [Pure]
        IPoint IPoint.Add(IVector v)
        {
            return this.Add(v.ToVector4d());
        }

        [Pure]
        IPoint IPoint.Sub(IVector v)
        {
            return this.Sub(v.ToVector4d());
        }

        [Pure]
        IVector IPoint.Sub(IPoint p2)
        {
            return this.Sub(p2.ToPoint4d());
        }

        [Pure]
        IPoint IPoint.Lerp(IPoint p2, double alpha)
        {
            return this.Lerp(p2.ToPoint4d(), alpha);
        }

        [Pure]
        double IPoint.InvLerp(IPoint p2, IPoint pLerp)
        {
            return this.InvLerp(p2.ToPoint4d(), pLerp.ToPoint4d());
        }

        [Pure]
        IPoint IPoint.Lineal(IPoint p2, double alpha, double beta)
        {
            return this.Lineal(p2.ToPoint4d(), alpha, beta);
        }

        #endregion

        #region IPoint4D

        void IPoint4D.GetCoordinates(ICoordinateSetter4D setter)
        {
            setter.SetCoords(this.X, this.Y, this.Z, this.W);
        }

        #endregion

        #region IEpsilonEquatable<IPoint>

        [Pure]
        bool IEpsilonEquatable<IPoint>.EpsilonEquals(IPoint other, double epsilon)
        {
            return this.EpsilonEquals(other.ToPoint4d(), epsilon);
        }

        #endregion

        #region inner classes

        /// <summary>
        /// This class compares points by coordinate (X or Y or Z or W).
        /// </summary>
        public sealed class CoordComparer : IComparer<Point4d>, IComparer
        {
            public CoordComparer(int coord, double epsilon = MathUtils.EPSILON)
            {
                this.coord = coord;
                this.epsilon = epsilon;
            }

            private readonly int coord;
            private readonly double epsilon;

            public int Compare(Point4d v1, Point4d v2)
            {
                switch (this.coord)
                {
                    case 0:
                        return v1.X.EpsilonCompareTo(v2.X, this.epsilon);
                    case 1:
                        return v1.Y.EpsilonCompareTo(v2.Y, this.epsilon);
                    case 2:
                        return v1.Z.EpsilonCompareTo(v2.Z, this.epsilon);
                    case 3:
                        return v1.W.EpsilonCompareTo(v2.W, this.epsilon);
                }
                throw new IndexOutOfRangeException();
            }

            int IComparer.Compare(object o1, object o2)
            {
                Contract.Requires(o1 is Point4d && o2 is Point4d);
                return this.Compare((Point4d)o1, (Point4d)o2);
            }
        }

        /// <summary>
        /// This class lexicographically compares points: it compares X -> Y -> Z -> W.
        /// </summary>
        public sealed class LexComparer : IComparer<Point4d>, IComparer
        {
            public LexComparer(double epsilon = MathUtils.EPSILON)
            {
                this.epsilon = epsilon;
            }

            private readonly double epsilon;

            public int Compare(Point4d v1, Point4d v2)
            {
                int i;
                i = v1.X.EpsilonCompareTo(v2.X, this.epsilon);
                if (i != 0)
                {
                    return i;
                }
                i = v1.Y.EpsilonCompareTo(v2.Y, this.epsilon);
                if (i != 0)
                {
                    return i;
                }
                i = v1.Z.EpsilonCompareTo(v2.Z, this.epsilon);
                if (i != 0)
                {
                    return i;
                }
                i = v1.W.EpsilonCompareTo(v2.W, this.epsilon);
                return i;
            }

            int IComparer.Compare(object o1, object o2)
            {
                Contract.Requires(o1 is Point4d && o2 is Point4d);
                return this.Compare((Point4d)o1, (Point4d)o2);
            }
        }

        #endregion
    }
}