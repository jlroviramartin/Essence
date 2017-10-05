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
    public struct BoundingBox2d : IEpsilonEquatable<BoundingBox2d>, IEquatable<BoundingBox2d>, IFormattable
    {
        public const string _XMIN = "XMin";
        public const string _XMAX = "XMax";

        public const string _YMIN = "YMin";
        public const string _YMAX = "YMax";

        /// <summary>This method builds a bounding box using the <code>(x1, y1, z1)</code> and <code>(x2, y2, z2)</code> points.</summary>
        public static BoundingBox2d FromCoords(double x1, double y1, double x2, double y2)
        {
            if (x1 > x2)
            {
                MiscUtils.Swap(ref x1, ref x2);
            }
            if (y1 > y2)
            {
                MiscUtils.Swap(ref y1, ref y2);
            }

            return new BoundingBox2d(x1, x2, y1, y2);
        }

        /// <summary>This method builds a bounding box using the <code>(x, y, z)</code> point and <code>(dx, dy, dz)</code> extent.</summary>
        public static BoundingBox2d FromExtents(double x, double y, double dx, double dy)
        {
            return FromCoords(x, y, x + dx, y + dy);
        }

        /// <summary>Empty rectangle.</summary>
        public static readonly BoundingBox2d Empty = new BoundingBox2d(0, -1, 0, -1);

        /// <summary>Infinite rectangle.</summary>
        public static BoundingBox2d Infinity = new BoundingBox2d(double.NegativeInfinity, double.PositiveInfinity, double.NegativeInfinity, double.PositiveInfinity);

        /// <summary>This method gets the union of <code>bboxes</code>.</summary>
        public static BoundingBox2d Union(params BoundingBox2d[] bboxes)
        {
            return Union((IEnumerable<BoundingBox2d>)bboxes);
        }

        /// <summary>This method gets the union of <code>bboxes</code>.</summary>
        public static BoundingBox2d Union(IEnumerable<BoundingBox2d> bboxes)
        {
            using (IEnumerator<BoundingBox2d> enumer = bboxes.GetEnumerator())
            {
                if (!enumer.MoveNext())
                {
                    return Empty;
                }
                BoundingBox2d ret = enumer.Current;
                while (enumer.MoveNext())
                {
                    ret = ret.Union(enumer.Current);
                }
                return ret;
            }
        }

        /// <summary>This method gets the union of <code>points</code>.</summary>
        public static BoundingBox2d UnionOfPoints(params Point2d[] points)
        {
            return UnionOfPoints((IEnumerable<Point2d>)points);
        }

        /// <summary>This method gets the union of <code>points</code>.</summary>
        public static BoundingBox2d UnionOfPoints(IEnumerable<Point2d> points)
        {
            using (IEnumerator<Point2d> enumer = points.GetEnumerator())
            {
                if (!enumer.MoveNext())
                {
                    return Empty;
                }
                Point2d p = enumer.Current;
                BoundingBox2d ret = new BoundingBox2d(p, p);
                while (enumer.MoveNext())
                {
                    ret = ret.Union(enumer.Current);
                }
                return ret;
            }
        }

        public BoundingBox2d(double xMin, double xMax, double yMin, double yMax)
        {
            this.XMin = xMin;
            this.XMax = xMax;
            this.YMin = yMin;
            this.YMax = yMax;
        }

        public BoundingBox2d(Point2d pMin, Point2d pMax)
            : this(pMin.X, pMax.X, pMin.Y, pMax.Y)
        {
        }

        /// <summary>This method tests is this bounding box is valid.</summary>
        public bool IsValid
        {
            get
            {
                return RangeUtils.IsValid(this.XMin, this.XMax)
                       && RangeUtils.IsValid(this.YMin, this.YMax);
            }
        }

        /// <summary>This method tests is this bounding box is empty.</summary>
        public bool IsEmpty
        {
            get
            {
                return RangeUtils.IsEmpty(this.XMin, this.XMax)
                       || RangeUtils.IsEmpty(this.YMin, this.YMax);
            }
        }

        /// <summary>This method tests is this bounding box is infinity.</summary>
        public bool IsInfinity
        {
            get
            {
                return RangeUtils.IsInfinity(this.XMin, this.XMax)
                       || RangeUtils.IsInfinity(this.YMin, this.YMax);
            }
        }

        public Point2d Origin
        {
            get { return new Point2d(this.XMin, this.YMin); }
        }

        public Vector2d Size
        {
            get { return new Vector2d(this.DX, this.DY); }
        }

        public double DX
        {
            get { return this.XMax - this.XMin; }
        }

        public double DY
        {
            get { return this.YMax - this.YMin; }
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
        public bool Touch(BoundingBox2d rec, double epsilon = MathUtils.EPSILON)
        {
            return RangeUtils.Touch(this.XMin, this.XMax, rec.XMin, rec.XMax, epsilon)
                   || RangeUtils.Touch(this.YMin, this.YMax, rec.YMin, rec.YMax, epsilon);
        }

        /// <summary>This method tests if this bounding box touches the <code>p</code> point.</summary>
        /// <param name="p">Point.</param>
        /// <param name="epsilon">Epsilon error.</param>
        /// <returns>true if the bounding box touch and the point touch.</returns>
        public bool Touch(Point2d p, double epsilon = MathUtils.EPSILON)
        {
            return RangeUtils.TouchPoint(this.XMin, this.XMax, p.X, epsilon)
                   || RangeUtils.TouchPoint(this.YMin, this.YMax, p.Y, epsilon);
        }

        /// <summary>This method tests if this bounding box completely contains the <code>rec</code> bounding box.</summary>
        /// <param name="rec">Rectangulo.</param>
        /// <param name="epsilon">Epsilon error.</param>
        /// <returns>Indica si lo contiene completamente.</returns>
        public bool Contains(BoundingBox2d rec, double epsilon = MathUtils.EPSILON)
        {
            return RangeUtils.Contains(this.XMin, this.XMax, rec.XMin, rec.XMax, epsilon)
                   && RangeUtils.Contains(this.YMin, this.YMax, rec.YMin, rec.YMax, epsilon);
        }

        /// <summary>This method tests if this bounding box completely contains the <code>p</code> point.</summary>
        /// <param name="p">Punto.</param>
        /// <param name="epsilon">Epsilon error.</param>
        /// <returns>Indica si lo contiene completamente.</returns>
        public bool Contains(Point2d p, double epsilon = MathUtils.EPSILON)
        {
            return RangeUtils.ContainsPoint(this.XMin, this.XMax, p.X, epsilon)
                   && RangeUtils.ContainsPoint(this.YMin, this.YMax, p.Y, epsilon);
        }

        public bool IsInterior(Point2d p, double epsilon = MathUtils.EPSILON)
        {
            return this.Contains(p, epsilon);
        }

        /// <summary>
        ///     Indica si existe intersección con el rectangulo indicado.
        /// </summary>
        /// <param name="rec">Rectangulo.</param>
        /// <param name="epsilon">Epsilon error.</param>
        /// <returns>Indica si existe intersección.</returns>
        public bool IntersectsWith(BoundingBox2d rec, double epsilon = MathUtils.EPSILON)
        {
            return RangeUtils.IntersectsWith(this.XMin, this.XMax, rec.XMin, rec.XMax, epsilon)
                   && RangeUtils.IntersectsWith(this.YMin, this.YMax, rec.YMin, rec.YMax, epsilon);
        }

        /// <summary>
        ///     Amplia el recubrimiento para que contenga al punto indicado.
        /// </summary>
        /// <param name="point">Punto.</param>
        public BoundingBox2d Union(Point2d point)
        {
            double rxMin, rxMax;
            RangeUtils.Union(this.XMin, this.XMax, point.X, out rxMin, out rxMax);

            double ryMin, ryMax;
            RangeUtils.Union(this.YMin, this.YMax, point.Y, out ryMin, out ryMax);

            return new BoundingBox2d(rxMin, rxMax, ryMin, ryMax);
        }

        /// <summary>
        ///     Amplia el recubrimiento para que contenga al rectangulo indicado.
        /// </summary>
        /// <param name="rec">Rectangulo.</param>
        public BoundingBox2d Union(BoundingBox2d rec)
        {
            double rxMin, rxMax;
            RangeUtils.Union(this.XMin, this.XMax, rec.XMin, rec.XMax, out rxMin, out rxMax);

            double ryMin, ryMax;
            RangeUtils.Union(this.YMin, this.YMax, rec.YMin, rec.YMax, out ryMin, out ryMax);

            return new BoundingBox2d(rxMin, rxMax, ryMin, ryMax);
        }

        /// <summary>
        ///     Interseccion entre los recubrimientos.
        /// </summary>
        /// <param name="rec">Rectangulo.</param>
        public BoundingBox2d Intersect(BoundingBox2d rec)
        {
            double rxMin, rxMax;
            RangeUtils.Intersect(this.XMin, this.XMax, rec.XMin, rec.XMax, out rxMin, out rxMax);
            if (RangeUtils.IsEmpty(rxMin, rxMax))
            {
                return Empty;
            }

            double ryMin, ryMax;
            RangeUtils.Intersect(this.YMin, this.YMax, rec.YMin, rec.YMax, out ryMin, out ryMax);
            if (RangeUtils.IsEmpty(ryMin, ryMax))
            {
                return Empty;
            }

            return new BoundingBox2d(rxMin, rxMax, ryMin, ryMax);
        }

        /// <summary>
        ///     Amplia el recubrimiento en cada coordenada.
        /// </summary>
        /// <param name="d">Ancho y alto.</param>
        public BoundingBox2d Inflate(double d)
        {
            return this.Inflate(d, d);
        }

        /// <summary>
        ///     Amplia el recubrimiento en cada coordenada, por el vector indicado.
        /// </summary>
        /// <param name="dx">Ancho.</param>
        /// <param name="dy">Alto.</param>
        public BoundingBox2d Inflate(double dx, double dy)
        {
            return new BoundingBox2d(this.XMin - dx, this.XMax + dx, this.YMin - dy, this.YMax + dy);
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
                case 1:
                    return this.YMin;
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
                case 1:
                    return this.YMax;
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        /// <summary>This method gets the vertices sorted by coordinates:
        /// [Y, X] : [min, min], [min, max], [max, min], [max, max]
        /// </summary>
        public Point2d[] GetVertices()
        {
            if (this.IsEmpty)
            {
                return new Point2d[0];
            }
            return new[]
            {
                new Point2d(this.XMin, this.YMin),
                new Point2d(this.XMax, this.YMin),

                new Point2d(this.XMin, this.YMax),
                new Point2d(this.XMax, this.YMax)
            };
        }

        public readonly double XMin;
        public readonly double XMax;

        public readonly double YMin;
        public readonly double YMax;

        #region private

        private bool EpsilonEquals(double xmin, double xmax,
                                   double ymin, double ymax,
                                   double epsilon)
        {
            return RangeUtils.EpsilonEquals(this.XMin, this.XMax, xmin, xmax, epsilon)
                   && RangeUtils.EpsilonEquals(this.YMin, this.YMax, ymin, ymax, epsilon);
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
            if (!(obj is BoundingBox2d))
            {
                return false;
            }

            return this.Equals((BoundingBox2d)obj);
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
                hash = prime * hash + this.YMin.GetHashCode();
                hash = prime * hash + this.YMax.GetHashCode();
            }
            return hash;
        }

        #endregion

        #region IEpsilonEquatable<BoundingBox2d>

        [Pure]
        public bool EpsilonEquals(BoundingBox2d other, double epsilon = MathUtils.EPSILON)
        {
            return this.EpsilonEquals(other.XMin, other.XMax, other.YMin, other.YMax, (double)epsilon);
        }

        #endregion

        #region IEquatable<BoundingBox2d>

        [Pure]
        public bool Equals(BoundingBox2d other)
        {
            return RangeUtils.Equals(this.XMin, this.XMax, other.XMin, other.XMax)
                   && RangeUtils.Equals(this.YMin, this.YMax, other.YMin, other.YMax);
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
                                 "x: [{0} .. {1}], y: [{2} .. {3}]",
                                 this.XMin.ToString(format, provider),
                                 this.XMax.ToString(format, provider),
                                 this.YMin.ToString(format, provider),
                                 this.YMax.ToString(format, provider));
        }

        #endregion
    }
}