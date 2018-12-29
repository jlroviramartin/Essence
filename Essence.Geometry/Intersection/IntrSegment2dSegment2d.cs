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
using Essence.Geometry.Geom2D;
using Essence.Util.Math;
using Essence.Util.Math.Double;
using Essence.Geometry.Core.Double;
using REAL = System.Double;

namespace Essence.Geometry.Intersection
{
    public class IntrSegment2dSegment2d
    {
        public IntrSegment2dSegment2d()
        {
            this.Error = MathUtils.EPSILON;
        }

        /// <summary>Obtiene/establece el error máximo en el calculo de intersecciones.</summary>
        public double Error { get; set; }

        /// <summary>Obtiene/establece el segmento 1º.</summary>
        public Segment2d Item0 { get; set; }

        /// <summary>Obtiene/establece el segmento 2º.</summary>
        public Segment2d Item1 { get; set; }

        public IntersectionType IntersectionType { get; private set; }
        public IntrPoint2[] Intersections { get; private set; }

        private void Clear()
        {
            this.IntersectionType = IntersectionType.EMPTY;
            this.Intersections = new IntrPoint2[0];
        }

        public bool Find()
        {
            double[] parameters = new double[2];
            IntersectionType intersectionType = IntrLine2dLine2d.Classify(this.Item0.P0, this.Item0.Direction,
                                                                          this.Item1.P0, this.Item1.Direction,
                                                                          parameters, this.Error);

            switch (intersectionType)
            {
                case IntersectionType.POINT:
                {
                    // Test whether the line-line intersection is on the segments.
                    if (!parameters[0].EpsilonBetweenClosed(0, this.Item0.Length, this.Error)
                        || !parameters[1].EpsilonBetweenClosed(0, this.Item1.Length, this.Error))
                    {
                        this.Clear();
                        return false;
                    }

                    Point2d pt = this.Item0.Evaluate0L(parameters[0]);

                    this.IntersectionType = IntersectionType.POINT;
                    this.Intersections = new[]
                    {
                        new IntrPoint2(parameters[0], this.Item1.Project0L(pt), pt),
                    };
                    break;
                }
                case IntersectionType.LINE:
                {
                    // Proyectamos en 'Item0' los extremos de 'Item1'.
                    double param0 = this.Item0.Project0L(this.Item1.P0, false);
                    double param1 = this.Item0.Project0L(this.Item1.P1, false);

                    // Si ambos caen fuera, no hay interseccion.
                    if (Math.Max(param0, param1).EpsilonL(0, this.Error) || Math.Min(param0, param1).EpsilonG(this.Item0.Length, this.Error))
                    {
                        this.Clear();
                        return false;
                    }

                    param0 = MathUtils.Clamp(param0, 0, this.Item0.Length);
                    param1 = MathUtils.Clamp(param1, 0, this.Item0.Length);

                    // NOTA: caso especial, coinciden los extremos.
                    if (param0.EpsilonEquals(param1, this.Error))
                    {
                        Point2d pt = this.Item0.Evaluate0L(param0);

                        this.IntersectionType = IntersectionType.POINT;
                        this.Intersections = new[]
                        {
                            new IntrPoint2(param0, this.Item1.Project0L(pt), pt),
                        };
                    }
                    else
                    {
                        Point2d pt0 = this.Item0.Evaluate0L(param0);
                        Point2d pt1 = this.Item0.Evaluate0L(param1);

                        this.IntersectionType = IntersectionType.SEGMENT;
                        this.Intersections = new[]
                        {
                            new IntrPoint2(param0, this.Item1.Project0L(pt0), pt0),
                            new IntrPoint2(param1, this.Item1.Project0L(pt1), pt1),
                        };
                    }
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
    }
}