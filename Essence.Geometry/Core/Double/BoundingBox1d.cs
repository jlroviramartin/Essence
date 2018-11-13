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
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Essence.Util;
using Essence.Util.Math;
using Essence.Util.Math.Double;

namespace Essence.Geometry.Core.Double
{
    public struct BoundingBox1d : IEpsilonEquatable<BoundingBox1d>, IEquatable<BoundingBox1d>, IFormattable
    {
        public const string _XMIN = "XMin";
        public const string _XMAX = "XMax";

        /// <summary>This method builds a bounding box using the <code>(x1, y1, z1)</code> and <code>(x2, y2, z2)</code> points.</summary>
        public static BoundingBox1d FromCoords(double x1, double x2)
        {
            if (x1 > x2)
            {
                MiscUtils.Swap(ref x1, ref x2);
            }
            return new BoundingBox1d(x1, x2);
        }

        /// <summary>This method builds a bounding box using the <code>(x, y, z)</code> point and <code>(dx, dy, dz)</code> extent.</summary>
        public static BoundingBox1d FromExtents(double x, double dx)
        {
            return FromCoords(x, x + dx);
        }

        /// <summary>Empty rectangle.</summary>
        public static readonly BoundingBox1d Empty = new BoundingBox1d(0, -1);

        /// <summary>Infinite rectangle.</summary>
        public static BoundingBox1d Infinity = new BoundingBox1d(double.NegativeInfinity, double.PositiveInfinity);

        /// <summary>This method gets the union of <code>bboxes</code>.</summary>
        public static BoundingBox1d Union(params BoundingBox1d[] bboxes)
        {
            return Union((IEnumerable<BoundingBox1d>)bboxes);
        }

        /// <summary>This method gets the union of <code>bboxes</code>.</summary>
        public static BoundingBox1d Union(IEnumerable<BoundingBox1d> bboxes)
        {
            using (IEnumerator<BoundingBox1d> enumer = bboxes.GetEnumerator())
            {
                if (!enumer.MoveNext())
                {
                    return Empty;
                }
                BoundingBox1d ret = enumer.Current;
                while (enumer.MoveNext())
                {
                    ret = ret.Union(enumer.Current);
                }
                return ret;
            }
        }

        /// <summary>This method gets the union of <code>points</code>.</summary>
        public static BoundingBox1d UnionOfPoints(params double[] points)
        {
            return UnionOfPoints((IEnumerable<double>)points);
        }

        /// <summary>This method gets the union of <code>points</code>.</summary>
        public static BoundingBox1d UnionOfPoints(IEnumerable<double> points)
        {
            using (IEnumerator<double> enumer = points.GetEnumerator())
            {
                if (!enumer.MoveNext())
                {
                    return Empty;
                }
                double p = enumer.Current;
                BoundingBox1d ret = new BoundingBox1d(p, p);
                while (enumer.MoveNext())
                {
                    ret = ret.Union(enumer.Current);
                }
                return ret;
            }
        }

        public BoundingBox1d(double xMin, double xMax)
        {
            this.XMin = xMin;
            this.XMax = xMax;
        }

        /// <summary>This method tests is this bounding box is valid.</summary>
        public bool IsValid
        {
            get { return RangeUtils.IsValid(this.XMin, this.XMax); }
        }

        /// <summary>This method tests is this bounding box is empty.</summary>
        public bool IsEmpty
        {
            get { return RangeUtils.IsEmpty(this.XMin, this.XMax); }
        }

        /// <summary>This method tests is this bounding box is infinity.</summary>
        public bool IsInfinity
        {
            get { return RangeUtils.IsInfinity(this.XMin, this.XMax); }
        }

        public double Origin
        {
            get { return this.XMin; }
        }

        public double Size
        {
            get { return this.DX; }
        }

        public double DX
        {
            get { return this.XMax - this.XMin; }
        }

        /// <summary>
        /// This method tests if the edges of this bounding box touches the edges of the <code>rec</code> bounding box.
        /// <c><![CDATA[
        ///  +-+----+---+
        ///  | |    |   |
        ///  | +----+   |
        ///  |          |
        ///  +----------+
        /// ]]></c>
        /// </summary>
        /// <param name="rec">Bounding box.</param>
        /// <param name="epsilon">Epsilon error.</param>
        /// <returns>true if both bounding box touch.</returns>
        public bool Touch(BoundingBox1d rec, double epsilon = MathUtils.EPSILON)
        {
            return RangeUtils.Touch(this.XMin, this.XMax, rec.XMin, rec.XMax, epsilon);
        }

        /// <summary>This method tests if this bounding box touches the <code>p</code> point.</summary>
        /// <param name="p">Point.</param>
        /// <param name="epsilon">Epsilon error.</param>
        /// <returns>true if the bounding box touch and the point touch.</returns>
        public bool Touch(double p, double epsilon = MathUtils.EPSILON)
        {
            return RangeUtils.TouchPoint(this.XMin, this.XMax, p, epsilon);
        }

        /// <summary>This method tests if this bounding box completely contains the <code>rec</code> bounding box.</summary>
        /// <param name="rec">Rectangulo.</param>
        /// <param name="epsilon">Epsilon error.</param>
        /// <returns>Indica si lo contiene completamente.</returns>
        public bool Contains(BoundingBox1d rec, double epsilon = MathUtils.EPSILON)
        {
            return RangeUtils.Contains(this.XMin, this.XMax, rec.XMin, rec.XMax, epsilon);
        }

        /// <summary>This method tests if this bounding box completely contains the <code>p</code> point.</summary>
        /// <param name="p">Punto.</param>
        /// <param name="epsilon">Epsilon error.</param>
        /// <returns>Indica si lo contiene completamente.</returns>
        public bool Contains(double p, double epsilon = MathUtils.EPSILON)
        {
            return RangeUtils.ContainsPoint(this.XMin, this.XMax, p, epsilon);
        }

        public bool IsInterior(double p, double epsilon = MathUtils.EPSILON)
        {
            return this.Contains(p, epsilon);
        }

        /// <summary>
        ///     Indica si existe intersección con el rectangulo indicado.
        /// </summary>
        /// <param name="rec">Rectangulo.</param>
        /// <param name="epsilon">Epsilon error.</param>
        /// <returns>Indica si existe intersección.</returns>
        public bool IntersectsWith(BoundingBox1d rec, double epsilon = MathUtils.EPSILON)
        {
            return RangeUtils.IntersectsWith(this.XMin, this.XMax, rec.XMin, rec.XMax, epsilon);
        }

        /// <summary>
        ///     Amplia el recubrimiento para que contenga al punto indicado.
        /// </summary>
        /// <param name="point">Punto.</param>
        public BoundingBox1d Union(double point)
        {
            double rxMin, rxMax;
            RangeUtils.Union(this.XMin, this.XMax, point, out rxMin, out rxMax);

            return new BoundingBox1d(rxMin, rxMax);
        }

        /// <summary>
        ///     Amplia el recubrimiento para que contenga al rectangulo indicado.
        /// </summary>
        /// <param name="rec">Rectangulo.</param>
        public BoundingBox1d Union(BoundingBox1d rec)
        {
            double rxMin, rxMax;
            RangeUtils.Union(this.XMin, this.XMax, rec.XMin, rec.XMax, out rxMin, out rxMax);

            return new BoundingBox1d(rxMin, rxMax);
        }

        /// <summary>
        ///     Interseccion entre los recubrimientos.
        /// </summary>
        /// <param name="rec">Rectangulo.</param>
        public BoundingBox1d Intersect(BoundingBox1d rec)
        {
            double rxMin, rxMax;
            RangeUtils.Intersect(this.XMin, this.XMax, rec.XMin, rec.XMax, out rxMin, out rxMax);
            if (RangeUtils.IsEmpty(rxMin, rxMax))
            {
                return Empty;
            }

            return new BoundingBox1d(rxMin, rxMax);
        }

        /// <summary>
        ///     Amplia el recubrimiento en cada coordenada, por el vector indicado.
        /// </summary>
        /// <param name="dx">Ancho.</param>
        /// <param name="dy">Alto.</param>
        public BoundingBox1d Inflate(double dx)
        {
            return new BoundingBox1d(this.XMin - dx, this.XMax + dx);
        }

        /// <summary>
        ///     Obtiene la coordenada minima.
        /// </summary>
        /// <param name="i">Indice de la coordenada.</param>
        /// <returns>Coordenada.</returns>
        /// <exception cref="IndexOutOfRangeException">
        ///     Si el indice esta fuera de rango lanza la
        ///     excepción: <c>IndexOutOfRangeException</c>.
        /// </exception>
        public double GetMin(int i)
        {
            switch (i)
            {
                case 0:
                    return this.XMin;
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        ///     Obtiene la coordenada maxima.
        /// </summary>
        /// <param name="i">Indice de la coordenada.</param>
        /// <returns>Coordenada.</returns>
        /// <exception cref="IndexOutOfRangeException">
        ///     Si el indice esta fuera de rango lanza la
        ///     excepción: <c>IndexOutOfRangeException</c>.
        /// </exception>
        public double GetMax(int i)
        {
            switch (i)
            {
                case 0:
                    return this.XMax;
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        public readonly double XMin;
        public readonly double XMax;

        #region private

        private bool EpsilonEquals(double xmin, double xmax,
                                   double epsilon)
        {
            return RangeUtils.EpsilonEquals(this.XMin, this.XMax, xmin, xmax, epsilon);
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
            if (!(obj is BoundingBox1d))
            {
                return false;
            }

            return this.Equals((BoundingBox1d)obj);
        }

        [Pure]
        public override int GetHashCode()
        {
            // http://www.jarvana.com/jarvana/view/org/apache/lucene/lucene-spatial/2.9.3/lucene-spatial-2.9.3-sources.jar!/org/apache/lucene/spatial/geometry/shape/Vector2D.java
            const int prime = 31;
            int hash = 1;
            unchecked
            {
                hash = prime * hash + this.XMin.GetHashCode();
                hash = prime * hash + this.XMax.GetHashCode();
            }
            return hash;
        }

        #endregion

        #region IEpsilonEquatable<BoundingBox1d>

        [Pure]
        public bool EpsilonEquals(BoundingBox1d other, double epsilon = MathUtils.EPSILON)
        {
            return this.EpsilonEquals(other.XMin, other.XMax, (double)epsilon);
        }

        #endregion

        #region IEquatable<BoundingBox1d>

        [Pure]
        public bool Equals(BoundingBox1d other)
        {
            return RangeUtils.Equals(this.XMin, this.XMax, other.XMin, other.XMax);
        }

        #endregion

        #region IFormattable

        public string ToString(string format, IFormatProvider provider)
        {
            if (this.IsEmpty)
            {
                return "<Empty>";
            }

            return string.Format(provider,
                                 "x: [{0} .. {1}]",
                                 this.XMin.ToString(format, provider),
                                 this.XMax.ToString(format, provider));
        }

        #endregion
    }
}