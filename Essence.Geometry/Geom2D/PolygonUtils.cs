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
using Essence.Geometry.Geom3D;
using Essence.Util.Collections;
using java.util;

namespace Essence.Geometry.Geom2D
{
    public class PolygonUtils
    {

        /**
         * This method changes the order of the items in a list in order to be the first, the one with minimum lexicographical order (mínimum X, mínimum Y):
         * Point2d.LexComparer. If there are more than one point with equal order, it is choosen as the first point, the left most one.
         * <example>
         * List<Point2d> result = PolygonUtils.Normalize(new[] { new Point2d(10, 0), new Point2d(10, 10), new Point2d(0, 10), new Point2d(0, 0) }).ToList();
         * </example>
         */
        public static void normalize(List<Point2d> points)
        {
            Comparator<Point2d> lexComparer = Point2d.LexComparer.INSTANCE;

            int index = 0;
            Point2d min = points[index];
            for (int i = 1; i < points.Count; i++)
            {
                if (lexComparer.compare(min, points[i)) > 0]
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
                    if (lexComparer.compare(min, points[i)) == 0]
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

            ListUtils.shiftLeft(points, index);
        }

        /**
         * Indica si el punto está en el borde del poligono.
         */
        public static bool pointInEdge(List<Point2d> points, Point2d p, double epsilon)
        {
            double epsilon2 = epsilon * epsilon;

            DistPointSegment2 dist = new DistPointSegment2();
            dist.setPoint(p);

            // loop through all edges of the polygon
            Point2d a = points[points.Count - 1];
            for (int i = 0; i < points.Count; i++)
            {
                // Segmento ab.
                Point2d b = points[i];

                dist.setSegment(new Segment2(a, b));
                if (EpsilonEquals(dist.calcDistance2(), 0, epsilon2))
                {
                    return true;
                }

                a = b;
            }

            return false;
        }

        /**
         * Crossing number test for a point in a polygon. http://geomalgorithms.com/a03-_inclusion.html
         * <ul>
         * <li>non extendedAlgorithm: Points on a right-side boundary edge being outside, and ones on a left-side edge being inside.</li>
         * <li>extendedAlgorithm: This algorithm differenciates between inside/outside/on the polygon.<li>
         * </ul>
         */
        public static PointInPoly pointInPolyEvenOdd(List<Point2d> points, Point2d p, bool extendedAlgorithm, double epsilon)
        {
            // the crossing number counter
            int cn = 0;

            // loop through all edges of the polygon
            for (int i = 0; i < points.Count; i++)
            {
                // Segmento ab.
                Point2d a = points[i];
                Point2d b = points[(i + 1) % points.Count];

                if (extendedAlgorithm)
                {
                    // Horizontal line.
                    if (EpsilonEquals(a.Y, b.Y, epsilon))
                    {
                        if (EpsilonEquals(p.Y, a.Y, epsilon))
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

                            if (epsilonBetweenClosed(p.X, min, max, epsilon))
                            {
                                return PointInPoly.On;
                            }
                        }
                        // No se cuenta.
                        continue;
                    }
                }

                if ((epsilonGE(p.Y, a.Y, epsilon))
                    ? (epsilonL(p.Y, b.Y, epsilon)) // an upward crossing
                    : (epsilonGE(p.Y, b.Y, epsilon))) // a downward crossing
                {
                    // compute the actual edge-ray intersect x-coordinate
                    double vt = (p.Y - a.Y) / (b.Y - a.Y);
                    double x = a.X + vt * (b.X - a.X);

                    if (EpsilonEquals(p.X, x, epsilon))
                    {
                        if (extendedAlgorithm)
                        {
                            return PointInPoly.On;
                        }
                    }
                    else if (epsilonL(p.X, x, epsilon))
                    {
                        // a valid crossing of y=P.Y right of P.X
                        cn++;
                    }
                }
            }
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
        public static PointInPoly pointInPolyNonZero(List<Point2d> points, Point2d p, bool extendedAlgorithm, double epsilon)
        {
            // the winding number counter
            int wn = 0;

            // loop through all edges of the polygon
            for (int i = 0; i < points.Count; i++)
            {
                // Segmento ab.
                Point2d a = points[i];
                Point2d b = points[(i + 1) % points.Count];

                if (extendedAlgorithm)
                {
                    // Horizontal line.
                    if (EpsilonEquals(a.Y, b.Y, epsilon))
                    {
                        if (EpsilonEquals(p.Y, a.Y, epsilon))
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

                            if (epsilonBetweenClosed(p.X, min, max, epsilon))
                            {
                                return PointInPoly.On;
                            }
                        }
                        // No se cuenta.
                        continue;
                    }
                }

                if (epsilonGE(p.Y, a.Y, epsilon))
                {
                    // an upward crossing
                    if (epsilonL(p.Y, b.Y, epsilon))
                    {
                        // P left of edge
                        switch (Point2d.whichSide(a, b, p, epsilon))
                        {
                            case Left:
                                // have a valid up intersect
                                wn++;
                                break;
                            case Middle:
                                if (extendedAlgorithm)
                                {
                                    return PointInPoly.On;
                                }
                        }
                    }
                }
                else // if (p.Y.EpsilonMenor(a.Y, epsilon))
                {
                    // a downward crossing
                    if (epsilonGE(p.Y, b.Y, epsilon))
                    {
                        // P right of edge
                        switch (Point2d.whichSide(a, b, p, epsilon))
                        {
                            case Right:
                                // have a valid down intersect
                                wn--;
                                break;
                            case Middle:
                                if (extendedAlgorithm)
                                {
                                    return PointInPoly.On;
                                }
                        }
                    }
                }
            }
            // == 0 only if P is outside
            return ((wn != 0) ? PointInPoly.Inside : PointInPoly.Outside);
        }

        /**
         * Se asegura que la orientacion de los points sea CCW. No trata poligonos que contengan vertices repetidos.
         */
        public static void ensureCCW(List<Point2d> points)
        {
            if (testOrientation(points) == Orientation.CW)
            {
                Collections.reverse(points);
            }
        }

        /**
         * Se asegura que la orientacion de los points sea CCW. Trata poligonos que contengan vertices repetidos.
         */
        public static void ensureCCWRobust(List<Point2d> points)
        {
            if (testOrientationRobust(points) == Orientation.CW)
            {
                Collections.reverse(points);
            }
        }

        /**
         * Comprueba la orientacion del poligono. No trata poligonos que contengan vertices repetidos.
         * <p>
         * {@link http://www.easywms.com/easywms/?q=en/node/3602}
         * {@link http://paulbourke.net/geometry/clockwise/}
         * {@link http://paulbourke.net/geometry/polygonmesh/}
         */
        public static Orientation testOrientation(List<Point2d> points)
        {
            if (points.Count < 3)
            {
                return Orientation.Degenerate;
            }

            int count = 0;
            for (int i = 0; i < points.Count; i++)
            {
                Point2d p = points[i];
                Point2d pNext = points[(i + 1) % points.Count];
                Point2d pNextNext = points[(i + 2) % points.Count];

                double v = (pNext.Sub(p)).Cross(pNextNext.Sub(pNext));
                if (EpsilonEquals(v, 0))
                {
                }
                else if (v < 0)
                {
                    count--;
                }
                else //if (v > 0)
                {
                    count++;
                }
            }

            if (count == 0)
            {
                return Orientation.Degenerate;
            }
            else if (count > 0)
            {
                return Orientation.CCW;
            }
            else //if (count < 0)
            {
                return Orientation.CW;
            }
        }

        /**
         * Comprueba la orientacion del poligono. Trata poligonos que contengan vertices repetidos.
         * <p>
         * {@link http://www.easywms.com/easywms/?q=en/node/3602}
         * {@link http://paulbourke.net/geometry/clockwise/}
         * {@link http://paulbourke.net/geometry/polygonmesh/}
         */
        public static Orientation testOrientationRobust(List<Point2d> points)
        {
            if (points.Count < 3)
            {
                return Orientation.Degenerate;
            }

            // Primer punto.
            int i = 0;
            Point2d p = points[i];

            // Segundo punto: no puede ser igual al primero.
            int j = i;
            Point2d pNext;
            int c = 0;
            do
            {
                j++;
                pNext = points[j % points.Count];
                c++;
            } while (p.EpsilonEquals(pNext) && (c < points.Count));

            if (c == points.Count)
            {
                return Orientation.Degenerate;
            }

            // Indicadores de giro.
            int count = 0;

            while (i < points.Count)
            {
                // Tercer punto: no puede ser igual al segundo.
                int k = j;
                Point2d pNextNext;
                c = 0;
                do
                {
                    k++;
                    pNextNext = points[k % points.Count];
                    c++;
                } while (pNext.EpsilonEquals(pNextNext) && (c < points.Count));

                double v = (pNext.Sub(p)).Cross(pNextNext.Sub(pNext));
                if (EpsilonEquals(v, 0))
                {
                }
                else if (v < 0)
                {
                    count--;
                }
                else //if (v > 0)
                {
                    count++;
                }

                i = j;
                j = k;
                p = pNext;
                pNext = pNextNext;
            }

            if (count == 0)
            {
                return Orientation.Degenerate;
            }
            else if (count > 0)
            {
                return Orientation.CCW;
            }
            else //if (count < 0)
            {
                return Orientation.CW;
            }
        }

        /**
         * Orientation de un poligono simple. Da problemas con poligonos que contengan vertices repetidos.
         * <p>
         * {@link http://geometryalgorithms.com/Archive/algorithm_0101/algorithm_0101.htm#orientation2D_polygon()}
         * {@link http://geomalgorithms.com/a01-_area.html#orientation2D_polygon%28%29}
         */
        public static Orientation testOrientation2(List<Point2d> points)
        {
            int n = points.Count;

            if (n < 3)
            {
                return Orientation.Degenerate;
            }

            // first find rightmost lowest vertex of the polygon
            int imin = 0;
            double xmin = points[0].X;
            double ymin = points[0].Y;

            for (int i = 1; i < n; i++)
            {
                Point2d pi = points[i];
                if (pi.Y > ymin)
                {
                    continue;
                }

                if (pi.Y == ymin)
                {
                    // just as low
                    if (pi.X < xmin) // and to left
                    {
                        continue;
                    }
                }
                imin = i; // a new rightmost lowest vertex
                xmin = pi.X;
                ymin = pi.Y;
            }

            // test orientation at this imin vertex
            // ccw <=> the edge leaving is left of the entering edge
            int isig = (imin + 1) % n;
            int iprev = (n + imin - 1) % n;
            switch (Point2d.IsLeft(points[iprev], points[imin], points[isig]))
            {
                default:
                case 0:
                    return Orientation.Degenerate;
                case 1:
                    return Orientation.CCW;
                case -1:
                    return Orientation.CW;
            }
        }

        /**
         * Area del poligono con signo.
         * <p>
         * {@link http://paulbourke.net/geometry/polyarea/}
         *
         * @param points Puntos del poligono.
         * @return Area (con signo).
         */
        public static double signedArea(List<Point2d> points)
        {
            if (points.Count < 3)
            {
                return 0;
            }

            int n = points.Count;

            double area = 0;
            for (int i = 0; i < n; i++)
            {
                Point2d p = points[i];
                Point2d pNext = points[(i + 1) % n];

                area += p.X * pNext.Y - p.Y * pNext.X;
            }
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
        public static double signedArea2(List<Point2d> points)
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
        public static bool isConvex(List<Point2d> points)
        {
            if (points.Count < 3)
            {
                return false;
            }

            // Indicadores de giro.
            bool leftTurn = false, rightTurn = false;

            for (int i = 0; i < points.Count; i++)
            {
                Point2d p = points[i];
                Point2d pNext = points[(i + 1) % points.Count];
                Point2d pNextNext = points[(i + 2) % points.Count];

                switch (Point2d.WhichSide(p, pNext, pNextNext))
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
            }

            // Si no se ha encontrado giro, caso degenerado.
            if (!leftTurn && !rightTurn)
            {
                return false;
            }

            return true;
        }

        /**
         * Indica si un poligo es convexo. Trata poligonos que contengan vertices repetidos.
         */
        public static bool isConvexRobust(List<Point2d> points)
        {
            if (points.Count < 3)
            {
                return false;
            }

            // Primer punto.
            int i = 0;
            Point2d p = points[i];

            // Segundo punto: no puede ser igual al primero.
            int j = i;
            Point2d pNext;
            int c = 0;
            do
            {
                j++;
                pNext = points[j % points.Count];
                c++;
            } while (p.EpsilonEquals(pNext) && (c < points.Count));

            if (c == points.Count)
            {
                return false;
            }

            // Indicadores de giro.
            bool leftTurn = false, rightTurn = false;

            while (i < points.Count)
            {
                /*// Tercer punto: no puede ser una combinacion lineal del primero y segundo.
                    int k = j;
                    Point2d pNextNext;
                    Lado lado;
                    c = 0;
                    do
                    {
                        k++;
                        pNextNext = points[k % points.Count];
                        lado = Point2d.WhichSide(p, pNext, pNextNext);
                        c++;
                    } while ((lado == Lado.Medio) && (c < points.Count));*/

                // Tercer punto: no puede ser igual al segundo.
                int k = j;
                Point2d pNextNext;
                c = 0;
                do
                {
                    k++;
                    pNextNext = points[k % points.Count];
                    c++;
                } while (pNext.EpsilonEquals(pNextNext) && (c < points.Count));

                switch (Point2d.WhichSide(p, pNext, pNextNext))
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

                i = j;
                j = k;
                p = pNext;
                pNext = pNextNext;
            }

            // Si no se ha encontrado giro, caso degenerado.
            if (!leftTurn && !rightTurn)
            {
                return false;
            }

            return true;
        }

        /**
         * Indica si los points estan en un plano.
         */
        public static bool isPlanarPolygon(List<Point3d> points, double epsilon)
        {
            if (points.Count < 3)
            {
                // Poligono degenerado.
                return false;
            }

            Point3d p0 = points[0];
            Point3d p1 = points[1];
            int i = 2;
            while (p0.EpsilonEquals(p1) && (i < points.Count))
            {
                p1 = points[i];
                i++;
            }

            if (i >= points.Count)
            {
                // Poligono degenerado.
                return false;
            }

            Point3d p2 = points[i];
            i++;
            if (alignmentPoints(p0, p1, p2))
            {
                bool encontrado = false;
                while (i < points.Count)
                {
                    p2 = points[i];
                    i++;

                    if (alignmentPoints(p0, p1, p2))
                    {
                        encontrado = true;
                    }
                }
                if (!encontrado)
                {
                    // Poligono degenerado.
                    return false;
                }
            }

            Plane3d plano = Plane3d.NewOrthonormal(p0, p1, p2);

            // Se comprueba que el resto de los points esten en el plano.
            while (i < points.Count)
            {
                Point3d p3 = points[i];
                i++;
                if (plano.WhichSide(p3) != PlaneSide.Middle)
                {
                    // Se ha encontrado un punto que no esta en el plano.
                    return false;
                }
            }

            // Se ha terminado correctamente.
            return true;
        }

        /**
         * Suponiendo que todos los points estan en un plano, calcula dicho plano.
         */
        public static Plane3d plane(List<Point3d> points)
        {
            if (points.Count < 3)
            {
                // Poligono degenerado.
                return null;
            }

            Point3d p0 = points[0];
            Point3d p1 = points[1];
            int i = 2;
            while (p0.EpsilonEquals(p1) && (i < points.Count))
            {
                p1 = points[i];
                i++;
            }

            if (i >= points.Count)
            {
                // Poligono degenerado.
                return null;
            }

            Point3d p2 = points[i];
            i++;
            if (alignmentPoints(p0, p1, p2))
            {
                bool encontrado = false;
                while (i < points.Count)
                {
                    p2 = points[i];
                    i++;

                    if (alignmentPoints(p0, p1, p2))
                    {
                        encontrado = true;
                    }
                }
                if (!encontrado)
                {
                    // Poligono degenerado.
                    return null;
                }
            }

            return Plane3d.NewOrthonormal(p0, p1, p2);
        }

        /**
         * Indica si los points estan alineados.
         */
        public static bool alignmentPoints(Point3d p0, Point3d p1, Point3d p2)
        {
            Vector3d v0 = p1.Sub(p0);
            Vector3d v1 = p2.Sub(p0);
            return v0.Cross(v1).IsZero;
        }

        public static Point2d evaluate2D(List<Point2d> points, bool cerrada, double t)
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

        public static Point3d evaluate3D(List<Point3d> points, bool cerrada, double t)
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

        public static bool samePlane(Point3d p0, Point3d p1, Point3d p2, Point3d p)
        {
            return samePlane(p0, p1, p2, p, ZERO_TOLERANCE);
        }

        public static bool samePlane(Point3d p0, Point3d p1, Point3d p2, Point3d p, double epsilon)
        {
            double det = (p.Sub(p0)).TripleProduct(p1.Sub(p0), p2.Sub(p0));
            /*double det = new Matriz3x3d(
                    p.X - p0.X, p.Y - p0.Y, p.Z - p0.Z,
                    p1.X - p0.X, p1.Y - p0.Y, p1.Z - p0.Z,
                    p2.X - p0.X, p2.Y - p0.Y, p2.Z - p0.Z).Determinante;*/
            return EpsilonZero(det, epsilon);
        }
    }

}