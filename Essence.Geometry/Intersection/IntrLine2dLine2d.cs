#region License

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

#endregion

using Essence.Geometry.Core.Double;
using Essence.Geometry.Geom2D;
using Essence.Util.Math.Double;
using REAL = System.Double;

namespace Essence.Geometry.Intersection
{
    public class IntrLine2dLine2d
    {
        public IntrLine2dLine2d()
        {
            this.Error = MathUtils.EPSILON;
        }

        /// <summary>Obtiene/establece el error máximo en el calculo de intersecciones.</summary>
        public REAL Error { get; set; }

        /// <summary>Obtiene/establece el segmento 1º.</summary>
        public Line2d Item0 { get; set; }

        /// <summary>Obtiene/establece el segmento 2º.</summary>
        public Line2d Item1 { get; set; }

        public IntersectionType IntersectionType { get; private set; }
        public IntrPoint2[] Intersections { get; private set; }

        private void Clear()
        {
            this.IntersectionType = IntersectionType.EMPTY;
            this.Intersections = new IntrPoint2[0];
        }

        public bool Find()
        {
            REAL[] parameter = new REAL[2];
            IntersectionType intersectionType = Classify(this.Item0.Origin, this.Item0.Direction,
                                                         this.Item1.Origin, this.Item1.Direction,
                                                         parameter, this.Error);

            switch (intersectionType)
            {
                case IntersectionType.POINT:
                {
                    Point2d pt = this.Item0.Evaluate(parameter[0]);

                    this.IntersectionType = IntersectionType.POINT;
                    this.Intersections = new[]
                    {
                        new IntrPoint2(parameter[0], this.Item1.Project(pt), pt),
                    };
                    break;
                }
                case IntersectionType.LINE:
                {
                    // Proyectamos en 'Line0' el origen de 'Line1'.
                    REAL param0 = this.Item0.Project(this.Item1.Origin);
                    REAL param1 = param0 + 1;

                    Point2d pt0 = this.Item0.Evaluate(param0);
                    Point2d pt1 = this.Item0.Evaluate(param1);

                    this.IntersectionType = IntersectionType.LINE;
                    this.Intersections = new[]
                    {
                        new IntrPoint2(param0, this.Item1.Project(pt0), pt0),
                        new IntrPoint2(param1, this.Item1.Project(pt1), pt1),
                    };
                    break;
                }
                default:
                {
                    this.Clear();
                    return false;
                }
            }

            return this.IntersectionType != IntersectionType.EMPTY;
        }

        internal static IntersectionType Classify(Point2d p0, Vector2d d0, Point2d p1, Vector2d d1, REAL[] s, REAL error)
        {
            // The intersection of two lines is a solution to P0+s0*D0 = P1+s1*D1.
            // Rewrite this as s0*D0 - s1*D1 = P1 - P0 = Q.  If D0.Dot(Perp(D1)) = 0,
            // the lines are parallel.  Additionally, if Q.Dot(Perp(D1)) = 0, the
            // lines are the same.  If D0.Dot(Perp(D1)) is not zero, then
            //   s0 = Q.Dot(Perp(D1))/D0.Dot(Perp(D1))
            // produces the point of intersection.  Also,
            //   s1 = Q.Dot(Perp(D0))/D0.Dot(Perp(D1))

            Vector2d originDiff = p1 - p0;

            REAL d0DotPerpD1 = d0.DotPerpRight(d1);
            if (!d0DotPerpD1.EpsilonEquals(0.0, error))
            {
                // Lines intersect in a single point.
                if (s != null)
                {
                    REAL invD0DotPerpD1 = 1.0 / d0DotPerpD1;
                    REAL diffDotPerpD0 = originDiff.DotPerpRight(d0);
                    REAL diffDotPerpD1 = originDiff.DotPerpRight(d1);
                    s[0] = diffDotPerpD1 * invD0DotPerpD1;
                    s[1] = diffDotPerpD0 * invD0DotPerpD1;
                }
                return IntersectionType.POINT;
            }

            // Lines are parallel.
            originDiff = originDiff.Unit;

            REAL diffNDotPerpD1 = originDiff.DotPerpRight(d1);
            if (diffNDotPerpD1.EpsilonEquals(0.0, error))
            {
                // Lines are colinear.
                return IntersectionType.LINE;
            }

            // Lines are parallel, but distinct.
            return IntersectionType.EMPTY;
        }
    }
}