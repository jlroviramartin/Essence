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
using Essence.Geometry.Core;
using Essence.Geometry.Core.Double;
using Essence.Geometry.Curves;
using Essence.Geometry.Distances;
using Essence.Geometry.Geom3D;
using Essence.Util.Collections;
using Essence.Util.Math.Double;
using Math = System.Math;

namespace Essence.Geometry.Geom2D
{
    public class PolyChains
    {
        public PolyChains(IList<Point2d> points)
        {
            this.Points = points;
        }

        public readonly IList<Point2d> Points;
        public int LeftMost;
        public readonly IList<PolyChain> Chains = new List<PolyChain>();

        public void AddChain(bool leftToRight, IPolyEnumerator<Point2d> startIndex, IPolyEnumerator<Point2d> endIndex, List<int> inflectionYPoints)
        {
            int count = endIndex.Index - startIndex.Index + 1;
            if (count < 0)
            {
                count += this.Points.Count;
            }
            this.AddChain(leftToRight, startIndex.Index, count, inflectionYPoints);
        }

        public void AddChain(bool leftToRight, int startIndex, int count, List<int> inflectionYPoints)
        {
            PolyChain pchain = new PolyChain()
            {
                LeftToRight = leftToRight,
                StartIndex = startIndex,
                Count = count
            };
            pchain.InflectionYPoints.AddRange(inflectionYPoints);

            if (pchain.InflectionYPoints.Count == 0)
            {
                pchain.InflectionYPoints.Add(startIndex);
                pchain.InflectionYPoints.Add(startIndex + count - 1);
            }
            else
            {
                if (pchain.InflectionYPoints.First() != startIndex)
                {
                    pchain.InflectionYPoints.Add(startIndex);
                }
                if (pchain.InflectionYPoints.Last() != startIndex + count - 1)
                {
                    pchain.InflectionYPoints.Add(startIndex + count - 1);
                }
            }

            this.Chains.Add(pchain);
        }

        public IList<IList<Point2d>> GetMonotoneChains()
        {
            List<IList<Point2d>> result = new List<IList<Point2d>>();
            foreach (PolyChains.PolyChain pchain in this.Chains)
            {
                result.Add(new RangeList<Point2d>(this.Points, pchain.StartIndex, pchain.Count, true));
            }
            return result;
        }

        public class PolyChain
        {
            public bool LeftToRight;
            public int StartIndex;
            public int Count;
            public readonly List<int> InflectionYPoints = new List<int>();
        }
    }

    public static class PolygonUtils
    {
        /**
         * This method removes the duplicate points of this polygon.
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
        public static void RemoveDuplicatePoints(IList<Point2d> points, double epsilon = MathUtils.EPSILON)
        {
            for (int i = points.Count - 1; i >= 0; i--)
            {
                Point2d p = points[i];
                Point2d pNext = points[(i + 1) % points.Count];

                if (p.EpsilonEquals(pNext, epsilon))
                {
                    points.RemoveAt(i);
                }
            }
        }

        public static IList<IList<Point2d>> Sort2(IList<Point2d> points)
        {
            PolyChains pchains = Sort(points);
            return pchains.GetMonotoneChains();
        }

        public static PolyChains Sort(IList<Point2d> points, bool robust = true, double epsilon = MathUtils.EPSILON)
        {
            PolyChains pchains = new PolyChains(points);

            // NOTE: it is not necessary to find the leftmost point. It is enough with finding the start of a chain.
            int leftMost = FindLeftMost(points, epsilon);
            pchains.LeftMost = leftMost;

            IPolyEnumerator<Point2d> eFirst = NewEnumerator(points, leftMost, robust, epsilon);
            IPolyEnumerator<Point2d> e = eFirst.Clone();

            IPolyEnumerator<Point2d> eNext = e.Clone();
            eNext.Next();
            if (eNext.Equals(eFirst))
            {
            }

            IPolyEnumerator<Point2d> first = e.Clone();
            bool? leftRight = null;
            bool? bottomTop = null;
            List<int> inflectionYPoints = new List<int>();

            do
            {
                Point2d p = e.Point;
                Point2d pNext = eNext.Point;
                {
                    bool? nextBT = null;
                    if (pNext.Y.EpsilonG(p.Y))
                    {
                        nextBT = true;
                    }
                    else if (pNext.Y.EpsilonL(p.Y))
                    {
                        nextBT = false;
                    }

                    if (bottomTop == null)
                    {
                        bottomTop = nextBT;
                    }
                    else
                    {
                        if ((nextBT != null) && (bottomTop != nextBT))
                        {
                            inflectionYPoints.Add(e.Index);
                        }
                    }
                }

                {
                    bool? nextLR = null;
                    if (pNext.X.EpsilonG(p.X))
                    {
                        nextLR = true;
                    }
                    else if (pNext.X.EpsilonL(p.X))
                    {
                        nextLR = false;
                    }

                    if (leftRight == null)
                    {
                        leftRight = nextLR;
                    }
                    else
                    {
                        if ((nextLR != null) && (leftRight != nextLR))
                        {
                            pchains.AddChain((bool)leftRight, first, e, inflectionYPoints);

                            first = e.Clone();
                            leftRight = null;
                            bottomTop = null;
                            inflectionYPoints.Clear();
                        }
                    }
                }

                e.Next();
                eNext.Next();
            } while (!e.Equals(eFirst));

            if (!first.Equals(e))
            {
                pchains.AddChain((leftRight ?? true), first, e, inflectionYPoints);
            }
            return pchains;

            /*for (int i = 1; i < points.Count + 1; i++)
            {
                Point2d pprev = points[(leftMost + i - 1) % points.Count];
                Point2d p = points[(leftMost + i) % points.Count];

                {
                    bool? nextBT = null;
                    if (p.Y.EpsilonG(pprev.Y))
                    {
                        nextBT = true;
                    }
                    else if (p.Y.EpsilonL(pprev.Y))
                    {
                        nextBT = false;
                    }

                    if (bottomTop == null)
                    {
                        bottomTop = nextBT;
                    }
                    else
                    {
                        if ((nextBT != null) && (bottomTop != nextBT))
                        {
                            inflectionYPoints.Add(leftMost + i);
                        }
                    }
                }

                {
                    bool? nextLR = null;
                    if (p.X.EpsilonG(pprev.X))
                    {
                        nextLR = true;
                    }
                    else if (p.X.EpsilonL(pprev.X))
                    {
                        nextLR = false;
                    }

                    if (leftRight == null)
                    {
                        leftRight = nextLR;
                    }
                    else
                    {
                        if ((nextLR != null) && (leftRight != nextLR))
                        {
                            pchains.AddChain((bool)leftRight, first, (leftMost + i) - first, inflectionYPoints);

                            // Rollback
                            i--;

                            first = leftMost + i;
                            leftRight = null;
                            bottomTop = null;
                            inflectionYPoints.Clear();
                        }
                    }
                }
            }

            int c = (leftMost + points.Count + 1) - first;
            if (c > 1)
            {
                pchains.AddChain((leftRight ?? true), first, c, inflectionYPoints);
            }
            return pchains;*/
        }

        /**
         * This method changes the order of the items in a list in order to be the first, the one with minimum lexicographical order (mínimum X, mínimum Y):
         * Point2d.LexComparer. If there are more than one point with equal order, it is choosen as the first point, the left most one.
         * <example>
         * IList<Point2d> result = PolygonUtils.Normalize(new[] { new Point2d(10, 0), new Point2d(10, 10), new Point2d(0, 10), new Point2d(0, 0) }).ToList();
         * </example>
         */
        public static void Normalize(IList<Point2d> points, double epsilon = MathUtils.EPSILON)
        {
            int index = FindLeftMost(points, epsilon);
            ListUtils.ShiftLeft(points, index);
        }

        /**
         * This method finds the point with minimum lexicographical order (mínimum X, mínimum Y):
         * Point2d.LexComparer. If there are more than one point with equal order, it is choosen as the first point, the left most one.
         */
        public static int FindLeftMost(IList<Point2d> points, double epsilon = MathUtils.EPSILON)
        {
            IComparer<Point2d> lexComparer = new Point2d.LexComparer(epsilon);

            int index = 0;
            Point2d min = points[index];
            for (int i = 1; i < points.Count; i++)
            {
                if (lexComparer.Compare(min, points[i]) > 0)
                {
                    index = i;
                    min = points[index];
                }
            }
            // Special case: the minimum is the point 0. Test from last to first for
            // equals points.
            if (index == 0)
            {
                for (int i = points.Count - 1; i > 0; i--)
                {
                    if (lexComparer.Compare(min, points[i]) == 0)
                    {
                        index = i;
                        min = points[index];
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return index;
        }

        /**
         * Indica si el punto está en el borde del poligono.
         */
        public static bool PointInEdge(IList<Point2d> points, Point2d p, bool robust = true, double epsilon = MathUtils.EPSILON)
        {
            double epsilon2 = epsilon * epsilon;

            DistPointSegment2 dist = new DistPointSegment2();
            dist.Point = p;

            IPolyEnumerator<Point2d> eFirst = NewEnumerator(points, 0, robust, epsilon);
            IPolyEnumerator<Point2d> e = eFirst.Clone();

            IPolyEnumerator<Point2d> eNext = e.Clone();
            eNext.Next();
            if (eNext.Equals(eFirst))
            {
                return false;
            }

            do
            {
                dist.Segment = new Segment2(e.Point, eNext.Point);
                if (dist.CalcDistance2().EpsilonEquals(0, epsilon2))
                {
                    return true;
                }

                e.Next();
                eNext.Next();
            } while (!e.Equals(eFirst));

            return false;
        }

        /**
         * Crossing number test for a point in a polygon. http://geomalgorithms.com/a03-_inclusion.html
         * <ul>
         * <li>non extendedAlgorithm: Points on a right-side boundary edge being outside, and ones on a left-side edge being inside.</li>
         * <li>extendedAlgorithm: This algorithm differenciates between inside/outside/on the polygon.<li>
         * </ul>
         */
        public static PointInPoly PointInPolyEvenOdd(IList<Point2d> points, Point2d p, bool extendedAlgorithm, bool robust = true, double epsilon = MathUtils.EPSILON)
        {
            // the crossing number counter
            int cn = 0;

            IPolyEnumerator<Point2d> eFirst = NewEnumerator(points, 0, robust, epsilon);
            IPolyEnumerator<Point2d> e = eFirst.Clone();

            IPolyEnumerator<Point2d> eNext = e.Clone();
            eNext.Next();
            if (eNext.Equals(eFirst))
            {
                return PointInPoly.Outside;
            }

            // loop through all edges of the polygon
            do
            {
                // Segmento ab.
                Point2d a = e.Point;
                Point2d b = eNext.Point;

                if (extendedAlgorithm)
                {
                    // Horizontal line.
                    if (a.Y.EpsilonEquals(b.Y, epsilon))
                    {
                        if (p.Y.EpsilonEquals(a.Y, epsilon))
                        {
                            double min, max;
                            if (a.X < b.X)
                            {
                                min = a.X;
                                max = b.X;
                            }
                            else
                            {
                                min = b.X;
                                max = a.X;
                            }

                            if (p.X.EpsilonBetweenClosed(min, max, epsilon))
                            {
                                return PointInPoly.On;
                            }
                        }
                        // No se cuenta.
                        continue;
                    }
                }

                if ((p.Y.EpsilonGE(a.Y, epsilon))
                    ? (p.Y.EpsilonL(b.Y, epsilon)) // an upward crossing
                    : (p.Y.EpsilonGE(b.Y, epsilon))) // a downward crossing
                {
                    // compute the actual edge-ray intersect x-coordinate
                    double vt = (p.Y - a.Y) / (b.Y - a.Y);
                    double x = a.X + vt * (b.X - a.X);

                    if (p.X.EpsilonEquals(x, epsilon))
                    {
                        if (extendedAlgorithm)
                        {
                            return PointInPoly.On;
                        }
                    }
                    else if (p.X.EpsilonL(x, epsilon))
                    {
                        // a valid crossing of y=P.Y right of P.X
                        cn++;
                    }
                }

                e.Next();
                eNext.Next();
            } while (!e.Equals(eFirst));

            // 0 = outside, 1 = inside
            // 0 if even (out), and 1 if odd (in)
            return ((cn & 1) == 1) ? PointInPoly.Inside : PointInPoly.Outside;
        }

        /**
         * Winding number test for a point in a polygon. http://geomalgorithms.com/a03-_inclusion.html
         * <ul>
         * <li>non extendedAlgorithm: Points on a right-side boundary edge being outside, and ones on a left-side edge being inside.</li>
         * <li>extendedAlgorithm: This algorithm differenciates between inside/outside/on the polygon.<li>
         * </ul>
         */
        public static PointInPoly PointInPolyNonZero(IList<Point2d> points, Point2d p, bool extendedAlgorithm, bool robust = true, double epsilon = MathUtils.EPSILON)
        {
            // the winding number counter
            int wn = 0;

            IPolyEnumerator<Point2d> eFirst = NewEnumerator(points, 0, robust, epsilon);
            IPolyEnumerator<Point2d> e = eFirst.Clone();

            IPolyEnumerator<Point2d> eNext = e.Clone();
            eNext.Next();
            if (eNext.Equals(eFirst))
            {
                return PointInPoly.Outside;
            }

            // loop through all edges of the polygon
            do
            {
                // Segmento ab.
                Point2d a = e.Point;
                Point2d b = eNext.Point;

                if (extendedAlgorithm)
                {
                    // Horizontal line.
                    if (a.Y.EpsilonEquals(b.Y, epsilon))
                    {
                        if (p.Y.EpsilonEquals(a.Y, epsilon))
                        {
                            double min, max;
                            if (a.X < b.X)
                            {
                                min = a.X;
                                max = b.X;
                            }
                            else
                            {
                                min = b.X;
                                max = a.X;
                            }

                            if (p.X.EpsilonBetweenClosed(min, max, epsilon))
                            {
                                return PointInPoly.On;
                            }
                        }
                        // No se cuenta.
                        continue;
                    }
                }

                if (p.Y.EpsilonGE(a.Y, epsilon))
                {
                    // an upward crossing
                    if (p.Y.EpsilonL(b.Y, epsilon))
                    {
                        // P left of edge
                        switch (Point2d.WhichSide(a, b, p, epsilon))
                        {
                            case LineSide.Left:
                                // have a valid up intersect
                                wn++;
                                break;
                            case LineSide.Middle:
                                if (extendedAlgorithm)
                                {
                                    return PointInPoly.On;
                                }
                                break;
                        }
                    }
                }
                else // if (p.Y.EpsilonL(a.Y, epsilon))
                {
                    // a downward crossing
                    if (p.Y.EpsilonGE(b.Y, epsilon))
                    {
                        // P right of edge
                        switch (Point2d.WhichSide(a, b, p, epsilon))
                        {
                            case LineSide.Right:
                                // have a valid down intersect
                                wn--;
                                break;
                            case LineSide.Middle:
                                if (extendedAlgorithm)
                                {
                                    return PointInPoly.On;
                                }
                                break;
                        }
                    }
                }

                e.Next();
                eNext.Next();
            } while (!e.Equals(eFirst));

            // == 0 only if P is outside
            return ((wn != 0) ? PointInPoly.Inside : PointInPoly.Outside);
        }

        /// <summary>
        /// Ensures that the orientation is CCW.
        /// </summary>
        /// <param name="points">Poligono.</param>
        /// <param name="robust">Indica si trata poligonos que contengan vertices repetidos.</param>
        public static void EnsureCCW(IList<Point2d> points, bool robust = true, double epsilon = MathUtils.EPSILON)
        {
            if (TestOrientation(points, robust, epsilon) == Orientation.CW)
            {
                ListUtils.Reverse(points);
            }
        }

        /**
         * This method tests the orientation of a simple polygon.
         * <p>
         * {@link http://geometryalgorithms.com/Archive/algorithm_0101/algorithm_0101.htm#orientation2D_polygon()}
         * {@link http://geomalgorithms.com/a01-_area.html#orientation2D_polygon%28%29}
         */
        public static Orientation TestOrientation(IList<Point2d> points, bool robust = true, double epsilon = MathUtils.EPSILON)
        {
            int n = points.Count;
            if (n < 3)
            {
                return Orientation.Degenerate;
            }

            // first find leftmost lowest vertex of the polygon
            int index = FindLeftMost(points, epsilon);
            IPolyEnumerator<Point2d> e = NewEnumerator(points, index, robust, epsilon);

            // test orientation at leftmost vertex
            // ccw <=> the edge leaving is left of the entering edge
            IPolyEnumerator<Point2d> eNext = e.Clone();
            eNext.Next();
            if (eNext.Equals(e))
            {
                return Orientation.Degenerate;
            }

            IPolyEnumerator<Point2d> ePrev = e.Clone();
            ePrev.Prev();
            if (ePrev.Equals(e))
            {
                return Orientation.Degenerate;
            }

            switch (Point2d.WhichSide(ePrev.Point, e.Point, eNext.Point))
            {
                case LineSide.Middle:
                    return Orientation.Degenerate;
                case LineSide.Left:
                    return Orientation.CCW;
                case LineSide.Right:
                    return Orientation.CW;
                default:
                    throw new IndexOutOfRangeException();
            }

            // Another algorithms:
            // http://www.easywms.com/easywms/?q=en/node/3602
            // http://paulbourke.net/geometry/clockwise/
            // http://paulbourke.net/geometry/polygonmesh/
            //
            // http://www.easywms.com/easywms/?q=en/node/3602
            // http://paulbourke.net/geometry/clockwise/
            // http://paulbourke.net/geometry/polygonmesh/
        }

        /**
         * Area del poligono con signo.
         * <p>
         * {@link http://paulbourke.net/geometry/polyarea/}
         *
         * @param points Puntos del poligono.
         * @return Area (con signo).
         */
        public static double SignedArea(IList<Point2d> points, bool robust = true, double epsilon = MathUtils.EPSILON)
        {
            if (points.Count < 3)
            {
                return 0;
            }

            double area = 0;

            IPolyEnumerator<Point2d> eFirst = NewEnumerator(points, 0, robust, epsilon);
            IPolyEnumerator<Point2d> e = eFirst.Clone();

            IPolyEnumerator<Point2d> eNext = e.Clone();
            eNext.Next();
            if (eNext.Equals(eFirst))
            {
                return 0;
            }

            do
            {
                area += e.Point.X * eNext.Point.Y - e.Point.Y * eNext.Point.X;
                e.Next();
                eNext.Next();
            } while (!e.Equals(eFirst));

            return area / 2;
        }

        /**
         * Area (con signo) del poligono simple.
         * <p>
         * {@link http://geometryalgorithms.com/Archive/algorithm_0101/algorithm_0101.htm#area2D_polygon()}
         * {@link http://geomalgorithms.com/a01-_area.html#area2D_polygon%28%29}
         *
         * @param points Puntos del poligono.
         * @return Area (con signo).
         */
        public static double SignedArea2(IList<Point2d> points)
        {
            if (points.Count < 3)
            {
                return 0;
            }

            int n = points.Count;

            double area = 0;
            for (int i = 0; i < n; i++)
            {
                Point2d pSig = points[(i + 1) % n];
                Point2d p = points[i];
                Point2d pPrev = points[(n + i - 1) % n];

                area += p.X * (pSig.Y - pPrev.Y);
            }

            return area / 2;
        }

        /**
         * Indica si un poligo es convexo. Da problemas con poligonos que contengan vertices repetidos.
         */
        public static bool IsConvex(IList<Point2d> points, bool robust = true, double epsilon = MathUtils.EPSILON)
        {
            if (points.Count < 3)
            {
                return false;
            }

            // Indicadores de giro.
            bool leftTurn = false, rightTurn = false;

            IPolyEnumerator<Point2d> eFirst = NewEnumerator(points, 0, robust, epsilon);
            IPolyEnumerator<Point2d> e1 = eFirst.Clone();

            IPolyEnumerator<Point2d> e2 = e1.Clone();
            e2.Next();
            if (e2.Equals(eFirst))
            {
                return false;
            }

            IPolyEnumerator<Point2d> e3 = e2.Clone();
            e3.Next();
            if (e3.Equals(eFirst))
            {
                return false;
            }

            do
            {
                switch (Point2d.WhichSide(e1.Point, e2.Point, e3.Point))
                {
                    case LineSide.Left:
                        leftTurn = true;
                        break;
                    case LineSide.Right:
                        rightTurn = true;
                        break;
                }

                // Si se ha girado a izquierda y derecha, no es convexo.
                if (leftTurn && rightTurn)
                {
                    return false;
                }

                e1.Next();
                e2.Next();
                e3.Next();
            } while (!e1.Equals(eFirst));

            // Si no se ha encontrado giro, caso degenerado.
            if (!leftTurn && !rightTurn)
            {
                return false;
            }

            return true;
        }

        /**
         * Tests if all points are contained on a plane.
         */
        public static bool IsPlanarPolygon(IList<Point3d> points, bool robust = true, double epsilon = MathUtils.EPSILON)
        {
            return Plane(points, robust, epsilon) != null;
        }

        /**
         * Evaluates the plane where all points are contained. If it does not exist, it returns {@code null}.
         */
        public static Plane3d Plane(IList<Point3d> points, bool robust = true, double epsilon = MathUtils.EPSILON)
        {
            if (points.Count < 3)
            {
                // Poligono degenerado.
                return null;
            }

            IPolyEnumerator<Point3d> eFirst = NewEnumerator(points, 0, robust, epsilon);
            IPolyEnumerator<Point3d> e1 = eFirst.Clone();

            IPolyEnumerator<Point3d> e2 = e1.Clone();
            e2.Next();
            if (e2.Equals(eFirst))
            {
                // Poligono degenerado.
                return null;
            }

            IPolyEnumerator<Point3d> e3 = e2.Clone();
            e3.Next();
            while (!e3.Equals(eFirst) && AlignmentPoints(e1.Point, e2.Point, e3.Point))
            {
                e3.Next();
            }
            if (e3.Equals(eFirst))
            {
                // Poligono degenerado.
                return null;
            }

            Plane3d plane = Plane3d.NewOrthonormal(e1.Point, e2.Point, e3.Point);

            IPolyEnumerator<Point3d> e4 = e3.Clone();
            e4.Next();
            while (!e4.Equals(eFirst))
            {
                if (plane.WhichSide(e4.Point) != PlaneSide.Middle)
                {
                    // Se ha encontrado un punto que no esta en el plano.
                    return null;
                }

                e4.Next();
            }

            // Se ha terminado correctamente.
            return plane;
        }

        /**
         * Tests if all points are lined up.
         */
        public static bool AlignmentPoints(Point3d p0, Point3d p1, Point3d p2)
        {
            Vector3d v0 = p1.Sub(p0);
            Vector3d v1 = p2.Sub(p0);
            return v0.Cross(v1).IsZero;
        }

        public static Point2d Evaluate2D(IList<Point2d> points, bool cerrada, double t)
        {
            if (cerrada)
            {
                t = t % points.Count;
                int i = (int)Math.Floor(t);
                int inext = (i + 1) % points.Count;
                double alfa = t - i;
                return points[i].Lerp(points[inext], alfa);
            }
            else
            {
                int i = (int)Math.Floor(t);
                int inext = i + 1;
                double alfa = t - i;
                return points[i].Lerp(points[inext], alfa);
            }
        }

        public static Point3d Evaluate3D(IList<Point3d> points, bool cerrada, double t)
        {
            if (cerrada)
            {
                t = t % points.Count;
                int i = (int)Math.Floor(t);
                int inext = (i + 1) % points.Count;
                double alfa = t - i;
                return points[i].Lerp(points[inext], alfa);
            }
            else
            {
                int i = (int)Math.Floor(t);
                int inext = i + 1;
                double alfa = t - i;
                return points[i].Lerp(points[inext], alfa);
            }
        }

        public static bool SamePlane(Point3d p0, Point3d p1, Point3d p2, Point3d p, double epsilon = MathUtils.ZERO_TOLERANCE)
        {
            double det = (p.Sub(p0)).TripleProduct(p1.Sub(p0), p2.Sub(p0));
            /*double det = new Matriz3x3d(
                    p.X - p0.X, p.Y - p0.Y, p.Z - p0.Z,
                    p1.X - p0.X, p1.Y - p0.Y, p1.Z - p0.Z,
                    p2.X - p0.X, p2.Y - p0.Y, p2.Z - p0.Z).Determinante;*/
            return det.EpsilonZero(epsilon);
        }

        private static IPolyEnumerator<Point2d> NewEnumerator(IList<Point2d> points, int index = 0, bool robust = true, double epsilon = MathUtils.EPSILON)
        {
            return (robust
                ? (IPolyEnumerator<Point2d>)new PolyEnumeratorRobust<Point2d>(points, index, true, epsilon)
                : new PolyEnumerator<Point2d>(points, index));
        }

        private static IPolyEnumerator<Point3d> NewEnumerator(IList<Point3d> points, int index = 0, bool robust = true, double epsilon = MathUtils.EPSILON)
        {
            return (robust
                ? (IPolyEnumerator<Point3d>)new PolyEnumeratorRobust<Point3d>(points, index, true, epsilon)
                : new PolyEnumerator<Point3d>(points, index));
        }
    }
}