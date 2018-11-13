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
using Essence.Geometry.Core.Integer;
using Essence.Util;

namespace Essence.Geometry.Core.Int
{
    public struct BoundingBox2i : IEquatable<BoundingBox2i>, IFormattable
    {
        public const string _XMIN = "XMin";
        public const string _XMAX = "XMax";

        public const string _YMIN = "YMin";
        public const string _YMAX = "YMax";

        public static BoundingBox2i FromCoords(int x1, int y1, int x2, int y2)
        {
            if (x1 > x2)
            {
                MiscUtils.Swap(ref x1, ref x2);
            }
            if (y1 > y2)
            {
                MiscUtils.Swap(ref y1, ref y2);
            }

            return new BoundingBox2i(x1, x2, y1, y2);
        }

        public static BoundingBox2i FromExtents(int x, int y, int dx, int dy)
        {
            return FromCoords(x, y, x + dx, y + dy);
        }

        /// <summary>
        /// Rectangulo vacio.
        /// </summary>
        public static readonly BoundingBox2i Empty = new BoundingBox2i(0, -1, 0, -1);

        /// <summary>
        /// Une todos los rectangulos.
        /// </summary>
        public static BoundingBox2i Union(params BoundingBox2i[] bboxes)
        {
            if (bboxes.Length == 0)
            {
                return Empty;
            }
            BoundingBox2i ret = bboxes[0];
            for (int i = 1; i < bboxes.Length; i++)
            {
                ret = ret.Union(bboxes[i]);
            }
            return ret;
        }

        /// <summary>
        /// Une todos los puntos.
        /// </summary>
        public static BoundingBox2i Union(params Point2i[] points)
        {
            if (points.Length == 0)
            {
                return Empty;
            }
            BoundingBox2i ret = new BoundingBox2i(points[0].X, points[0].Y, points[0].X, points[0].Y);
            for (int i = 1; i < points.Length; i++)
            {
                ret = ret.Union(points[i]);
            }
            return ret;
        }

        public BoundingBox2i(int xMin, int xMax, int yMin, int yMax)
        {
            this.XMin = xMin;
            this.XMax = xMax;
            this.YMin = yMin;
            this.YMax = yMax;
        }

        /// <summary>
        /// Indica si es vacio.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return BoundingBox2iUtils.IsEmpty(this.XMin, this.XMax)
                       || BoundingBox2iUtils.IsEmpty(this.YMin, this.YMax);
            }
        }

        public Point2i[] GetVertices()
        {
            if (this.IsEmpty)
            {
                return new Point2i[0];
            }
            return new[] { new Point2i(this.XMin, this.YMin), new Point2i(this.XMax, this.YMin), new Point2i(this.XMax, this.YMax), new Point2i(this.XMin, this.YMax) };
        }

        public Point2i Origin
        {
            get { return new Point2i(this.XMin, this.YMin); }
        }

        public Vector2i Size
        {
            get { return new Vector2i(this.DX, this.DY); }
        }

        public int DX
        {
            get { return this.XMax - this.XMin; }
        }

        public int DY
        {
            get { return this.YMax - this.YMin; }
        }

        /// <summary>
        /// Indica si el rectangulo toca al rectangulo indicado por algun lado desde el interior.
        /// <c><![CDATA[
        ///  +-+----+---+
        ///  | |    |   |
        ///  | +----+   |
        ///  |          |
        ///  +----------+
        /// ]]></c>
        /// </summary>
        /// <param name="rec">Rectangulo.</param>
        /// <returns>Indica si lo toca.</returns>
        public bool Touch(BoundingBox2i rec)
        {
            return BoundingBox2iUtils.Touch(this.XMin, this.XMax, rec.XMin, rec.XMax)
                   || BoundingBox2iUtils.Touch(this.YMin, this.YMax, rec.YMin, rec.YMax);
        }

        /// <summary>
        /// Indica si el rectangulo toca al punto indicado por algun lado.
        /// </summary>
        /// <param name="p">Punto.</param>
        /// <returns>Indica si lo toca.</returns>
        public bool Touch(Point2i p)
        {
            return BoundingBox2iUtils.TouchPoint(this.XMin, this.XMax, p.X)
                   || BoundingBox2iUtils.TouchPoint(this.YMin, this.YMax, p.Y);
        }

        /// <summary>
        /// Indica si contiene completamente al rectangulo indicado.
        /// </summary>
        /// <param name="rec">Rectangulo.</param>
        /// <returns>Indica si lo contiene completamente.</returns>
        public bool Contains(BoundingBox2i rec)
        {
            return BoundingBox2iUtils.Contains(this.XMin, this.XMax, rec.XMin, rec.XMax)
                   && BoundingBox2iUtils.Contains(this.YMin, this.YMax, rec.YMin, rec.YMax);
        }

        /// <summary>
        /// Indica si contiene completamente al punto indicado.
        /// </summary>
        /// <param name="p">Punto.</param>
        /// <returns>Indica si lo contiene completamente.</returns>
        public bool Contains(Point2i p)
        {
            return BoundingBox2iUtils.ContainsPoint(this.XMin, this.XMax, p.X)
                   && BoundingBox2iUtils.ContainsPoint(this.YMin, this.YMax, p.Y);
        }

        /// <summary>
        /// Indica si existe intersección con el rectangulo indicado.
        /// </summary>
        /// <param name="rec">Rectangulo.</param>
        /// <returns>Indica si existe intersección.</returns>
        public bool IntersectsWith(BoundingBox2i rec)
        {
            return BoundingBox2iUtils.IntersectsWith(this.XMin, this.XMax, rec.XMin, rec.XMax)
                   && BoundingBox2iUtils.IntersectsWith(this.YMin, this.YMax, rec.YMin, rec.YMax);
        }

        /// <summary>
        /// Amplia el recubrimiento para que contenga al punto indicado.
        /// </summary>
        /// <param name="point">Punto.</param>
        public BoundingBox2i Union(Point2i point)
        {
            int rxMin, rxMax;
            BoundingBox2iUtils.Union(this.XMin, this.XMax, point.X, out rxMin, out rxMax);

            int ryMin, ryMax;
            BoundingBox2iUtils.Union(this.YMin, this.YMax, point.Y, out ryMin, out ryMax);

            return new BoundingBox2i(rxMin, rxMax, ryMin, ryMax);
        }

        /// <summary>
        /// Amplia el recubrimiento para que contenga al rectangulo indicado.
        /// </summary>
        /// <param name="rec">Rectangulo.</param>
        public BoundingBox2i Union(BoundingBox2i rec)
        {
            int rxMin, rxMax;
            BoundingBox2iUtils.Union(this.XMin, this.XMax, rec.XMin, rec.XMax, out rxMin, out rxMax);

            int ryMin, ryMax;
            BoundingBox2iUtils.Union(this.YMin, this.YMax, rec.YMin, rec.YMax, out ryMin, out ryMax);

            return new BoundingBox2i(rxMin, rxMax, ryMin, ryMax);
        }

        /// <summary>
        /// Interseccion entre los recubrimientos.
        /// </summary>
        /// <param name="rec">Rectangulo.</param>
        public BoundingBox2i Intersect(BoundingBox2i rec)
        {
            int rxMin, rxMax;
            BoundingBox2iUtils.Intersect(this.XMin, this.XMax, rec.XMin, rec.XMax, out rxMin, out rxMax);

            int ryMin, ryMax;
            BoundingBox2iUtils.Intersect(this.YMin, this.YMax, rec.YMin, rec.YMax, out ryMin, out ryMax);

            return new BoundingBox2i(rxMin, rxMax, ryMin, ryMax);
        }

        /// <summary>
        /// Amplia el recubrimiento en cada coordenada.
        /// </summary>
        /// <param name="d">Ancho y alto.</param>
        public BoundingBox2i Inflate(int d)
        {
            return this.Inflate(d, d);
        }

        /// <summary>
        /// Amplia el recubrimiento en cada coordenada, por el vector indicado.
        /// </summary>
        /// <param name="dx">Ancho.</param>
        /// <param name="dy">Alto.</param>
        public BoundingBox2i Inflate(int dx, int dy)
        {
            return new BoundingBox2i(this.XMin - dx, this.XMax + dx, this.YMin - dy, this.YMax + dy);
        }

        /// <summary>
        /// Obtiene la coordenada minima.
        /// </summary>
        /// <param name="i">Indice de la coordenada.</param>
        /// <returns>Coordenada.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// Si el indice esta fuera de rango lanza la
        /// excepción: <c>IndexOutOfRangeException</c>.
        /// </exception>
        public int GetMin(int i)
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
        /// Obtiene la coordenada maxima.
        /// </summary>
        /// <param name="i">Indice de la coordenada.</param>
        /// <returns>Coordenada.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// Si el indice esta fuera de rango lanza la
        /// excepción: <c>IndexOutOfRangeException</c>.
        /// </exception>
        public int GetMax(int i)
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

        public readonly int XMin;
        public readonly int XMax;

        public readonly int YMin;
        public readonly int YMax;

        #region object

        [Pure]
        public override string ToString()
        {
            return this.ToString("F3", null);
        }

        [Pure]
        public override bool Equals(object obj)
        {
            if (!(obj is BoundingBox2i))
            {
                return false;
            }

            return this.Equals((BoundingBox2i)obj);
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

        #region IEquatable<BOUNDINGBOX>

        [Pure]
        public bool Equals(BoundingBox2i other)
        {
            return BoundingBox2iUtils.Equals(this.XMin, this.XMax, other.XMin, other.XMax)
                   && BoundingBox2iUtils.Equals(this.YMin, this.YMax, other.YMin, other.YMax);
        }

        #endregion

        #region IFormattable

        public string ToString(string format, IFormatProvider provider)
        {
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