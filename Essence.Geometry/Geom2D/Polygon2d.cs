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
using Essence.Geometry.Core.Double;
using Essence.Geometry.Graphics;
using Essence.Util.Collections;
using Essence.Util.Math.Double;

namespace Essence.Geometry.Geom2D
{
    public class Polygon2d
    {
        #region private

        private readonly IList<Point2d> vertices;

        private BoundingBox2d? boundingBox;

        #endregion

        public Polygon2d(IList<Point2d> vertices)
        {
            this.vertices = vertices;
        }

        public IList<Point2d> Vertices
        {
            get { return this.vertices; }
        }

        public BoundingBox2d BoundingBox
        {
            get
            {
                if (this.boundingBox == null)
                {
                    this.boundingBox = BoundingBox2d.UnionOfPoints(this.vertices);
                }
                return (BoundingBox2d)this.boundingBox;
            }
        }

        public int Count
        {
            get { return this.vertices.Count; }
        }

        public Point2d this[int i]
        {
            get { return this.vertices[i]; }
        }

        public Point2d Evaluate(double t)
        {
            return PolygonUtils.Evaluate2D(this.vertices, true, t);
        }

        /**
         * This method tests if the point is on the edge.
         */
        public bool PointInEdge(Point2d p, double precision = MathUtils.EPSILON)
        {
            if (!this.BoundingBox.IsInterior(p))
            {
                return false;
            }

            return PolygonUtils.PointInEdge(this.Vertices, p, precision);
        }

        /**
         * This method tests if the point is on the polygon.
         */
        public bool PointInPoly(Point2d p, WindingRule windingRule, bool extendedAlgorithm, double epsilon = MathUtils.EPSILON)
        {
            switch (windingRule)
            {
                case WindingRule.EvenOdd:
                {
                    return this.PointInPolyEvenOdd(p, extendedAlgorithm, epsilon);
                }
                case WindingRule.NonZero:
                {
                    return this.PointInPolyNonZero(p, extendedAlgorithm, epsilon);
                }
                default:
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        /**
         * Crossing number test for a point in a polygon.
         */
        public bool PointInPolyEvenOdd(Point2d p, bool extendedAlgorithm, double epsilon = MathUtils.EPSILON)
        {
            if (!this.BoundingBox.IsInterior(p))
            {
                return false;
            }

            return (PolygonUtils.PointInPolyEvenOdd(this.vertices, p, extendedAlgorithm, epsilon) != Essence.Geometry.Geom2D.PointInPoly.Outside);
        }

        /**
         * Winding number test for a point in a polygon.
         */
        public bool PointInPolyNonZero(Point2d p, bool extendedAlgorithm, double epsilon = MathUtils.EPSILON)
        {
            if (!this.BoundingBox.IsInterior(p))
            {
                return false;
            }

            return (PolygonUtils.PointInPolyNonZero(this.vertices, p, extendedAlgorithm, epsilon) != Essence.Geometry.Geom2D.PointInPoly.Outside);
        }

        /**
         * This method test if the polygon is CCW.
         */
        public bool IsCCW()
        {
            return this.TestOrientation() == Orientation.CCW;
        }

        /**
         * This method ensures that polygon is CCW.
         */
        public void EnsureCCW()
        {
            if (this.TestOrientation() == Orientation.CW)
            {
                ListUtils.Reverse(this.vertices);
            }
        }

        /**
         * This method removes the suplicate points of this polygon.
         * <example><pre>
         * Polygon2D poly = new Polygon2D(new[]
         * {
         *     new Point2d(0, 0), new Point2d(0, 0), new Point2d(0, 0),
         *     new Point2d(10, 0), new Point2d(10, 0), new Point2d(10, 0),
         *     new Point2d(10, 10), new Point2d(10, 10), new Point2d(10, 10),
         *     new Point2d(0, 10), new Point2d(0, 10), new Point2d(0, 10),
         *     new Point2d(0, 0), new Point2d(0, 0), new Point2d(0, 0),
         * });
         * poly.RemoveDuplicatePoints();
         * Polygon2D poly = new Polygon2D(new[] { new Point2d(0, 0), new Point2d(10, 0), new Point2d(10, 10), new Point2d(0, 10), });
         * </pre></example>
         */
        public void RemoveDuplicatePoints()
        {
            for (int i = this.Count - 1; i >= 0; i--)
            {
                Point2d p = this[i];
                Point2d pNext = this[(i + 1) % this.Count];

                if (p.EpsilonEquals(pNext))
                {
                    this.vertices.RemoveAt(i);
                }
            }
        }

        /**
         * This method evaluates the orientation of this polygon.
         * <p>
         * {@link http://www.easywms.com/easywms/?q=en/node/3602}
         * {@link http://paulbourke.net/geometry/clockwise/}
         * {@link http://paulbourke.net/geometry/polygonmesh/}
         */
        public Orientation TestOrientation()
        {
            return PolygonUtils.TestOrientation(this.vertices, false);
        }

        /**
         * This method evaluates the area of this polygon.
         */
        public double SignedArea()
        {
            return PolygonUtils.SignedArea(this.vertices);
        }
    }
}