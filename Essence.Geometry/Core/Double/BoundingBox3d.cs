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
    public struct BoundingBox3d : IEpsilonEquatable<BoundingBox3d>, IEquatable<BoundingBox3d>, IFormattable
    {
        public const string _XMIN = "XMin";
        public const string _XMAX = "XMax";

        public const string _YMIN = "YMin";
        public const string _YMAX = "YMax";

        public const string _ZMIN = "ZMin";
        public const string _ZMAX = "YMax";

        public static BoundingBox3d FromCoords(double x1, double y1, double z1, double x2, double y2, double z2)
        {
            if (x1 > x2)
            {
                MiscUtils.Swap(ref x1, ref x2);
            }
            if (y1 > y2)
            {
                MiscUtils.Swap(ref y1, ref y2);
            }
            if (z1 > z2)
            {
                MiscUtils.Swap(ref z1, ref z2);
            }

            return new BoundingBox3d(x1, x2, y1, y2, z1, z2);
        }

        public static BoundingBox3d FromExtents(double x, double y, double z, double dx, double dy, double dz)
        {
            return FromCoords(x, y, z, x + dx, y + dy, z + dz);
        }

        /// <summary>
        ///     Rectangulo vacio.
        /// </summary>
        public static readonly BoundingBox3d Empty = new BoundingBox3d(0, -1, 0, -1, 0, -1);

        /// <summary>
        ///     Rectangulo infinito.
        /// </summary>
        public static readonly BoundingBox3d Infinity = new BoundingBox3d(double.NegativeInfinity, double.PositiveInfinity, double.NegativeInfinity, double.PositiveInfinity, double.NegativeInfinity, double.PositiveInfinity);

        /// <summary>
        ///     Une todos los rectangulos.
        /// </summary>
        public static BoundingBox3d Union(params BoundingBox3d[] bboxes)
        {
            return Union((IEnumerable<BoundingBox3d>)bboxes);
        }

        /// <summary>
        ///     Une todos los rectangulos.
        /// </summary>
        public static BoundingBox3d Union(IEnumerable<BoundingBox3d> bboxes)
        {
            using (IEnumerator<BoundingBox3d> enumer = bboxes.GetEnumerator())
            {
                if (!enumer.MoveNext())
                {
                    return Empty;
                }
                BoundingBox3d ret = enumer.Current;
                while (enumer.MoveNext())
                {
                    ret = ret.Union(enumer.Current);
                }
                return ret;
            }
        }

        /// <summary>
        ///     Une todos los puntos.
        /// </summary>
        public static BoundingBox3d Union(params Point3d[] points)
        {
            return Union((IEnumerable<Point3d>)points);
        }

        /// <summary>
        ///     Une todos los puntos.
        /// </summary>
        public static BoundingBox3d Union(IEnumerable<Point3d> points)
        {
            using (IEnumerator<Point3d> enumer = points.GetEnumerator())
            {
                if (!enumer.MoveNext())
                {
                    return Empty;
                }
                Point3d p = enumer.Current;
                BoundingBox3d ret = new BoundingBox3d(p, p);
                while (enumer.MoveNext())
                {
                    ret = ret.Union(enumer.Current);
                }
                return ret;
            }
        }

        public BoundingBox3d(double xMin, double xMax, double yMin, double yMax, double zMin, double zMax)
        {
            this.XMin = xMin;
            this.XMax = xMax;
            this.YMin = yMin;
            this.YMax = yMax;
            this.ZMin = zMin;
            this.ZMax = zMax;
        }

        public BoundingBox3d(Point3d pMin, Point3d pMax)
            : this(pMin.X, pMax.X, pMin.Y, pMax.Y, pMin.Z, pMax.Z)
        {
        }

        /// <summary>
        ///     Indica si es valido.
        /// </summary>
        public bool IsValid
        {
            get
            {
                return RangeUtils.IsValid(this.XMin, this.XMax)
                       && RangeUtils.IsValid(this.YMin, this.YMax)
                       && RangeUtils.IsValid(this.ZMin, this.ZMax);
            }
        }

        /// <summary>
        ///     Indica si es vacio.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return RangeUtils.IsEmpty(this.XMin, this.XMax)
                       || RangeUtils.IsEmpty(this.YMin, this.YMax)
                       || RangeUtils.IsEmpty(this.ZMin, this.ZMax);
            }
        }

        /// <summary>
        ///     Indica si es infinito.
        /// </summary>
        public bool IsInfinity
        {
            get
            {
                return RangeUtils.IsInfinity(this.XMin, this.XMax)
                       || RangeUtils.IsInfinity(this.YMin, this.YMax)
                       || RangeUtils.IsInfinity(this.ZMin, this.ZMax);
            }
        }

        /// <summary>
        /// Devuelve todos los vertices ordenados por coordenada:
        /// [Z, Y, X] : [min, min, min], [min, min, max], [min, max, min], [min, max, max],
        ///             [max, min, min], [max, min, max], [max, max, min], [max, max, max]
        /// </summary>
        public Point3d[] GetVertices()
        {
            if (this.IsEmpty)
            {
                return new Point3d[0];
            }
            return new[]
            {
                new Point3d(this.XMin, this.YMin, this.ZMin),
                new Point3d(this.XMax, this.YMin, this.ZMin),

                new Point3d(this.XMin, this.YMax, this.ZMin),
                new Point3d(this.XMax, this.YMax, this.ZMin),

                new Point3d(this.XMin, this.YMin, this.ZMax),
                new Point3d(this.XMax, this.YMin, this.ZMax),

                new Point3d(this.XMin, this.YMax, this.ZMax),
                new Point3d(this.XMax, this.YMax, this.ZMax)
            };
        }

        public Point3d Origin
        {
            get { return new Point3d(this.XMin, this.YMin, this.ZMin); }
        }

        public Vector3d Size
        {
            get { return new Vector3d(this.DX, this.DY, this.DZ); }
        }

        public double DX
        {
            get { return this.XMax - this.XMin; }
        }

        public double DY
        {
            get { return this.YMax - this.YMin; }
        }

        public double DZ
        {
            get { return this.ZMax - this.ZMin; }
        }

        /// <summary>
        ///     Indica si el rectangulo toca al rectangulo indicado por algun lado desde el interior.
        ///     <c><![CDATA[
        ///  +-+----+---+
        ///  | |    |   |
        ///  | +----+   |
        ///  |          |
        ///  +----------+
        /// ]]></c>
        /// </summary>
        /// <param name="rec">Rectangulo.</param>
        /// <param name="epsilon">Epsilon error.</param>
        /// <returns>Indica si lo toca.</returns>
        public bool Touch(BoundingBox3d rec, double epsilon = MathUtils.EPSILON)
        {
            return RangeUtils.Touch(this.XMin, this.XMax, rec.XMin, rec.XMax, epsilon)
                   || RangeUtils.Touch(this.YMin, this.YMax, rec.YMin, rec.YMax, epsilon)
                   || RangeUtils.Touch(this.ZMin, this.ZMax, rec.ZMin, rec.ZMax, epsilon);
        }

        /// <summary>
        ///     Indica si el rectangulo toca al punto indicado por algun lado.
        /// </summary>
        /// <param name="p">Punto.</param>
        /// <param name="epsilon">Epsilon error.</param>
        /// <returns>Indica si lo toca.</returns>
        public bool Touch(Point3d p, double epsilon = MathUtils.EPSILON)
        {
            return RangeUtils.TouchPoint(this.XMin, this.XMax, p.X, epsilon)
                   || RangeUtils.TouchPoint(this.YMin, this.YMax, p.Y, epsilon)
                   || RangeUtils.TouchPoint(this.ZMin, this.ZMax, p.Z, epsilon);
        }

        /// <summary>
        ///     Indica si contiene completamente al rectangulo indicado.
        /// </summary>
        /// <param name="rec">Rectangulo.</param>
        /// <param name="epsilon">Epsilon error.</param>
        /// <returns>Indica si lo contiene completamente.</returns>
        public bool Contains(BoundingBox3d rec, double epsilon = MathUtils.EPSILON)
        {
            return RangeUtils.Contains(this.XMin, this.XMax, rec.XMin, rec.XMax, epsilon)
                   && RangeUtils.Contains(this.YMin, this.YMax, rec.YMin, rec.YMax, epsilon)
                   && RangeUtils.Contains(this.ZMin, this.ZMax, rec.ZMin, rec.ZMax, epsilon);
        }

        /// <summary>
        ///     Indica si contiene completamente al punto indicado.
        /// </summary>
        /// <param name="p">Punto.</param>
        /// <param name="epsilon">Epsilon error.</param>
        /// <returns>Indica si lo contiene completamente.</returns>
        public bool Contains(Point3d p, double epsilon = MathUtils.EPSILON)
        {
            return RangeUtils.ContainsPoint(this.XMin, this.XMax, p.X, epsilon)
                   && RangeUtils.ContainsPoint(this.YMin, this.YMax, p.Y, epsilon)
                   && RangeUtils.ContainsPoint(this.ZMin, this.ZMax, p.Z, epsilon);
        }

        /// <summary>
        ///     Indica si existe intersección con el rectangulo indicado.
        /// </summary>
        /// <param name="rec">Rectangulo.</param>
        /// <param name="epsilon">Epsilon error.</param>
        /// <returns>Indica si existe intersección.</returns>
        public bool IntersectsWith(BoundingBox3d rec, double epsilon = MathUtils.EPSILON)
        {
            return RangeUtils.IntersectsWith(this.XMin, this.XMax, rec.XMin, rec.XMax, epsilon)
                   && RangeUtils.IntersectsWith(this.YMin, this.YMax, rec.YMin, rec.YMax, epsilon)
                   && RangeUtils.IntersectsWith(this.ZMin, this.ZMax, rec.ZMin, rec.ZMax, epsilon);
        }

        /// <summary>
        ///     Amplia el recubrimiento para que contenga al punto indicado.
        /// </summary>
        /// <param name="point">Punto.</param>
        public BoundingBox3d Union(Point3d point)
        {
            double rxMin, rxMax;
            RangeUtils.Union(this.XMin, this.XMax, point.X, out rxMin, out rxMax);

            double ryMin, ryMax;
            RangeUtils.Union(this.YMin, this.YMax, point.Y, out ryMin, out ryMax);

            double rzMin, rzMax;
            RangeUtils.Union(this.ZMin, this.ZMax, point.Z, out rzMin, out rzMax);

            return new BoundingBox3d(rxMin, rxMax, ryMin, ryMax, rzMin, rzMax);
        }

        /// <summary>
        ///     Amplia el recubrimiento para que contenga al rectangulo indicado.
        /// </summary>
        /// <param name="rec">Rectangulo.</param>
        public BoundingBox3d Union(BoundingBox3d rec)
        {
            double rxMin, rxMax;
            RangeUtils.Union(this.XMin, this.XMax, rec.XMin, rec.XMax, out rxMin, out rxMax);

            double ryMin, ryMax;
            RangeUtils.Union(this.YMin, this.YMax, rec.YMin, rec.YMax, out ryMin, out ryMax);

            double rzMin, rzMax;
            RangeUtils.Union(this.ZMin, this.ZMax, rec.ZMin, rec.ZMax, out rzMin, out rzMax);

            return new BoundingBox3d(rxMin, rxMax, ryMin, ryMax, rzMin, rzMax);
        }

        /// <summary>
        ///     Interseccion entre los recubrimientos.
        /// </summary>
        /// <param name="rec">Rectangulo.</param>
        public BoundingBox3d Intersect(BoundingBox3d rec)
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

            double rzMin, rzMax;
            RangeUtils.Intersect(this.ZMin, this.ZMax, rec.ZMin, rec.ZMax, out rzMin, out rzMax);
            if (RangeUtils.IsEmpty(rzMin, rzMax))
            {
                return Empty;
            }

            return new BoundingBox3d(rxMin, rxMax, ryMin, ryMax, rzMin, rzMax);
        }

        /// <summary>
        ///     Amplia el recubrimiento en cada coordenada.
        /// </summary>
        /// <param name="d">Ancho y alto.</param>
        public BoundingBox3d Inflate(double d)
        {
            return this.Inflate(d, d, d);
        }

        /// <summary>
        ///     Amplia el recubrimiento en cada coordenada, por el vector indicado.
        /// </summary>
        /// <param name="dx">Ancho.</param>
        /// <param name="dy">Alto.</param>
        public BoundingBox3d Inflate(double dx, double dy, double dz)
        {
            return new BoundingBox3d(this.XMin - dx, this.XMax + dx, this.YMin - dy, this.YMax + dy, this.ZMin - dz, this.ZMax + dz);
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
                case 2:
                    return this.ZMin;
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
                case 2:
                    return this.ZMax;
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        public readonly double XMin;
        public readonly double XMax;

        public readonly double YMin;
        public readonly double YMax;

        public readonly double ZMin;
        public readonly double ZMax;

        #region private

        private bool EpsilonEquals(double xmin, double xmax,
                                   double ymin, double ymax,
                                   double zmin, double zmax,
                                   double epsilon)
        {
            return RangeUtils.EpsilonEquals(this.XMin, this.XMax, xmin, xmax, epsilon)
                   && RangeUtils.EpsilonEquals(this.YMin, this.YMax, ymin, ymax, epsilon)
                   && RangeUtils.EpsilonEquals(this.ZMin, this.ZMax, zmin, zmax, epsilon);
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
            if (!(obj is BoundingBox3d))
            {
                return false;
            }

            return this.Equals((BoundingBox3d)obj);
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
                hash = prime * hash + this.ZMin.GetHashCode();
                hash = prime * hash + this.ZMax.GetHashCode();
            }
            return hash;
        }

        #endregion

        #region IEpsilonEquatable<BOUNDINGBOX>

        [Pure]
        public bool EpsilonEquals(BoundingBox3d other, double epsilon = MathUtils.EPSILON)
        {
            return this.EpsilonEquals(other.XMin, other.XMax, other.YMin, other.YMax, other.ZMin, other.ZMax, (double)epsilon);
        }

        #endregion

        #region IEquatable<BOUNDINGBOX>

        [Pure]
        public bool Equals(BoundingBox3d other)
        {
            return RangeUtils.Equals(this.XMin, this.XMax, other.XMin, other.XMax)
                   && RangeUtils.Equals(this.YMin, this.YMax, other.YMin, other.YMax)
                   && RangeUtils.Equals(this.ZMin, this.ZMax, other.ZMin, other.ZMax);
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
                                 "x: [{0} .. {1}], y: [{2} .. {3}], z: [{4} .. {5}]",
                                 this.XMin.ToString(format, provider),
                                 this.XMax.ToString(format, provider),
                                 this.YMin.ToString(format, provider),
                                 this.YMax.ToString(format, provider),
                                 this.ZMin.ToString(format, provider),
                                 this.ZMax.ToString(format, provider));
        }

        #endregion
    }
}