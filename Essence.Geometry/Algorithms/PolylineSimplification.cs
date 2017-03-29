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
using Essence.Geometry.Core.Double;
using Essence.Util.Math.Double;
using REAL = System.Double;

namespace Essence.Geometry.Algorithms
{
    /// <summary>
    ///     <see cref="http://psimpl.sourceforge.net/" />
    ///     <see cref="http://www.codeproject.com/Articles/114797/Polyline-Simplification" />
    /// </summary>
    public static class PolylineSimplification
    {
        #region NthPoint

        public static void NthPoint(this IEnumerable<Point2d> points, uint n, Action<Point2d> result)
        {
            if (n < 2)
            {
                foreach (Point2d p in points)
                {
                    result(p);
                }
                return;
            }

            IEnumerator<Point2d> enumer = points.GetEnumerator();
            if (!enumer.MoveNext())
            {
                return;
            }

            Point2d current = enumer.Current;

            // El primer punto siempre se añade.
            result(current);

            if (enumer.MoveNext())
            {
                int c = 1;

                Point2d next = enumer.Current;
                bool lastInResult;
                do
                {
                    lastInResult = (c % n) == 0;
                    c++;

                    if (lastInResult)
                    {
                        current = next;
                        result(current);
                    }
                } while (enumer.MoveNext());

                // El ultimo punto siempre se añade.
                if (!lastInResult)
                {
                    result(next);
                }
            }
        }

        public static IEnumerable<Point2d> NthPoint(this IEnumerable<Point2d> points, uint n)
        {
            if (n < 2)
            {
                return points;
            }

            return NthPointSafe(points, n);
        }

        private static IEnumerable<Point2d> NthPointSafe(this IEnumerable<Point2d> points, uint n)
        {
            IEnumerator<Point2d> enumer = points.GetEnumerator();
            if (!enumer.MoveNext())
            {
                yield break;
            }

            Point2d current = enumer.Current;

            // El primer punto siempre se añade.
            yield return current;

            if (enumer.MoveNext())
            {
                int c = 1;

                Point2d next = enumer.Current;
                bool lastInResult;
                do
                {
                    lastInResult = (c % n) == 0;
                    c++;

                    if (lastInResult)
                    {
                        current = next;
                        yield return current;
                    }
                } while (enumer.MoveNext());

                // El ultimo punto siempre se añade.
                if (!lastInResult)
                {
                    yield return next;
                }
            }
        }

        #endregion NthPoint

        #region RadialDistance

        public static void RadialDistance(this IEnumerable<Point2d> points, double tol, Action<Point2d> result)
        {
            double tol2 = tol * tol;

            if (tol2.EpsilonEquals(0))
            {
                foreach (Point2d p in points)
                {
                    result(p);
                }
                return;
            }

            IEnumerator<Point2d> enumer = points.GetEnumerator();
            if (!enumer.MoveNext())
            {
                return;
            }

            Point2d current = enumer.Current;

            // El primer punto siempre se añade.
            result(current);

            if (enumer.MoveNext())
            {
                Point2d next = enumer.Current;
                bool lastInResult;
                do
                {
                    lastInResult = (current.Distance2To(next) >= tol2);
                    if (lastInResult)
                    {
                        current = next;
                        result(current);
                    }
                } while (enumer.MoveNext());

                // El ultimo punto siempre se añade.
                if (!lastInResult)
                {
                    result(next);
                }
            }
        }

        public static IEnumerable<Point2d> RadialDistance(this IEnumerable<Point2d> points, double tol)
        {
            double tol2 = tol * tol;

            if (tol2.EpsilonEquals(0))
            {
                return points;
            }

            return RadialDistanceSafe(points, tol2);
        }

        private static IEnumerable<Point2d> RadialDistanceSafe(this IEnumerable<Point2d> points, double tol2)
        {
            IEnumerator<Point2d> enumer = points.GetEnumerator();
            if (!enumer.MoveNext())
            {
                yield break;
            }

            Point2d current = enumer.Current;

            // El primer punto siempre se añade.
            yield return current;

            if (enumer.MoveNext())
            {
                Point2d next = enumer.Current;
                bool lastInResult;
                do
                {
                    lastInResult = (current.Distance2To(next) >= tol2);
                    if (lastInResult)
                    {
                        current = next;
                        yield return current;
                    }
                } while (enumer.MoveNext());

                // El ultimo punto siempre se añade.
                if (!lastInResult)
                {
                    yield return next;
                }
            }
        }

        #endregion RadialDistance

        #region PerpendicularDistance

        public static void PerpendicularDistance(this IEnumerable<Point2d> points, double tol, uint repeat, Action<Point2d> result)
        {
            Contract.Requires(repeat > 1);

            int removed;

            for (int i = 1; i < repeat; i++)
            {
                List<Point2d> tempPoly = new List<Point2d>();

                PerpendicularDistance(points, tol, tempPoly.Add, out removed);
                points = tempPoly;

                if (removed == 0)
                {
                    foreach (Point2d p in points)
                    {
                        result(p);
                    }
                    return;
                }
            }

            PerpendicularDistance(points, tol, result, out removed);
        }

        public static void PerpendicularDistance(this IEnumerable<Point2d> points, double tol, Action<Point2d> result, out int removed)
        {
            removed = 0;

            double tol2 = tol * tol; // squared distance tolerance

            // validate input and check if simplification required
            if (tol2.EpsilonEquals(0))
            {
                foreach (Point2d p in points)
                {
                    result(p);
                }
                return;
            }

            IEnumerator<Point2d> enumer = points.GetEnumerator();
            if (!enumer.MoveNext())
            {
                return;
            }

            Point2d p0 = enumer.Current;

            // the first point is always part of the simplification
            result(p0);

            if (!enumer.MoveNext())
            {
                return;
            }

            Point2d p1 = enumer.Current;
            Point2d last = p1;

            while (enumer.MoveNext())
            {
                Point2d p2 = enumer.Current;

                // test p1 against line segment S(p0, p2)
                if (segment_distance2(p0, p2, p1) < tol2)
                {
                    result(p2);
                    last = p2;

                    // move up by two points
                    p0 = p2;

                    // protect against advancing p2 beyond last
                    if (!enumer.MoveNext())
                    {
                        break;
                    }
                    p1 = enumer.Current;

                    removed++;
                }
                else
                {
                    result(p1);
                    last = p1;

                    // move up by one point
                    p0 = p1;
                    p1 = p2;
                }
            }

            // make sure the last point is part of the simplification
            if (!p1.Equals(last))
            {
                result(p1);
            }
        }

        public static IEnumerable<Point2d> PerpendicularDistance(this IEnumerable<Point2d> points, double tol, uint repeat)
        {
            Contract.Requires(repeat > 1);

            for (int i = 1; i < repeat; i++)
            {
                List<Point2d> tempPoly = new List<Point2d>();

                int removed;
                PerpendicularDistance(points, tol, tempPoly.Add, out removed);
                points = tempPoly;

                if (removed == 0)
                {
                    foreach (Point2d p in points)
                    {
                        yield return p;
                    }
                    yield break;
                }
            }

            foreach (Point2d p in PerpendicularDistance(points, tol))
            {
                yield return p;
            }
        }

        public static IEnumerable<Point2d> PerpendicularDistance(this IEnumerable<Point2d> points, double tol)
        {
            double tol2 = tol * tol; // squared distance tolerance

            // validate input and check if simplification required
            if (tol2.EpsilonEquals(0))
            {
                return points;
            }
            return PerpendicularDistanceSafe(points, tol);
        }

        private static IEnumerable<Point2d> PerpendicularDistanceSafe(this IEnumerable<Point2d> points, double tol2)
        {
            IEnumerator<Point2d> enumer = points.GetEnumerator();
            if (!enumer.MoveNext())
            {
                yield break;
            }

            Point2d p0 = enumer.Current;

            // the first point is always part of the simplification
            yield return p0;

            if (!enumer.MoveNext())
            {
                yield break;
            }

            Point2d p1 = enumer.Current;
            Point2d last = p1;

            while (enumer.MoveNext())
            {
                Point2d p2 = enumer.Current;

                // test p1 against line segment S(p0, p2)
                if (segment_distance2(p0, p2, p1) < tol2)
                {
                    yield return p2;
                    last = p2;

                    // move up by two points
                    p0 = p2;

                    // protect against advancing p2 beyond last
                    if (!enumer.MoveNext())
                    {
                        break;
                    }
                    p1 = enumer.Current;
                }
                else
                {
                    yield return p1;
                    last = p1;

                    // move up by one point
                    p0 = p1;
                    p1 = p2;
                }
            }

            // make sure the last point is part of the simplification
            if (!p1.Equals(last))
            {
                yield return p1;
            }
        }

        #endregion PerpendicularDistance

        #region ReumannWitkam

        // Reumann-Witkam - Shifts a strip along the polyline and removes points that
        // fall outside

        public static void ReumannWitkam(this IEnumerable<Point2d> points, double tol, Action<Point2d> result)
        {
            ReumannWitkam(points, false, tol, result);
        }

        public static void ReumannWitkam(this IEnumerable<Point2d> points, bool cerrado, double tol, Action<Point2d> result)
        {
            double tol2 = tol * tol; // squared distance tolerance

            // validate input and check if simplification required
            if (tol2.EpsilonEquals(0))
            {
                foreach (Point2d p in points)
                {
                    result(p);
                }
                return;
            }

            IEnumerator<Point2d> enumer = points.GetEnumerator();
            if (!enumer.MoveNext())
            {
                return;
            }

            // define the line L(p0, p1)
            Point2d p0 = enumer.Current; // indicates the current key
            Point2d first = p0;

            // the first point is always part of the simplification
            result(p0);

            if (!enumer.MoveNext())
            {
                return;
            }

            Point2d p1 = enumer.Current; // indicates the next point after p0

            // keep track of two test points
            Point2d pj = p1; // the current test point (pi+1)

            // check each point pj against L(p0, p1)
            while (enumer.MoveNext())
            {
                Point2d pi = pj; // the previous test point
                pj = enumer.Current;

                if (line_distance2(p0, p1, pj) < tol2)
                {
                    continue;
                }

                // found the next key at pi
                result(pi);

                // define new line L(pi, pj)
                p0 = pi;
                p1 = pj;
            }

            if (cerrado)
            {
                Point2d pi = pj; // the previous test point
                pj = first;

                if (line_distance2(p0, p1, pj) < tol2)
                {
                    return;
                }

                // found the next key at pi
                result(pi);
            }
            else
            {
                // the last point is always part of the simplification
                result(pj);
            }
        }

        public static IEnumerable<Point2d> ReumannWitkam(this IEnumerable<Point2d> points, double tol)
        {
            return ReumannWitkam(points, false, tol);
        }

        public static IEnumerable<Point2d> ReumannWitkam(this IEnumerable<Point2d> points, bool cerrado, double tol)
        {
            double tol2 = tol * tol; // squared distance tolerance

            // validate input and check if simplification required
            if (tol2.EpsilonEquals(0))
            {
                return points;
            }
            return ReumannWitkamSafe(points, cerrado, tol2);
        }

        private static IEnumerable<Point2d> ReumannWitkamSafe(this IEnumerable<Point2d> points, bool cerrado, double tol2)
        {
            IEnumerator<Point2d> enumer = points.GetEnumerator();
            if (!enumer.MoveNext())
            {
                yield break;
            }

            // define the line L(p0, p1)
            Point2d p0 = enumer.Current; // indicates the current key
            Point2d first = p0;

            // the first point is always part of the simplification
            yield return p0;

            if (!enumer.MoveNext())
            {
                yield break;
            }

            Point2d p1 = enumer.Current; // indicates the next point after p0

            // keep track of two test points
            Point2d pj = p1; // the current test point (pi+1)

            // check each point pj against L(p0, p1)
            while (enumer.MoveNext())
            {
                Point2d pi = pj; // the previous test point
                pj = enumer.Current;

                if (line_distance2(p0, p1, pj) < tol2)
                {
                    continue;
                }

                // found the next key at pi
                yield return pi;

                // define new line L(pi, pj)
                p0 = pi;
                p1 = pj;
            }

            if (cerrado)
            {
                Point2d pi = pj; // the previous test point
                pj = first;

                if (line_distance2(p0, p1, pj) < tol2)
                {
                    yield break;
                }

                // found the next key at pi
                yield return pi;
            }
            else
            {
                // the last point is always part of the simplification
                yield return pj;
            }
        }

        #endregion ReumannWitkam

        #region Opheim

        // Opheim - A constrained version of Reumann-Witkam

        public static void Opheim(this IEnumerable<Point2d> points, double minTol, double maxTol, Action<Point2d> result)
        {
            double minTol2 = minTol * minTol; // squared minimum distance tolerance
            double maxTol2 = maxTol * maxTol; // squared maximum distance tolerance

            // validate input and check if simplification required
            if (minTol2.EpsilonEquals(0) || maxTol2.EpsilonEquals(0))
            {
                foreach (Point2d p in points)
                {
                    result(p);
                }
                return;
            }

            IEnumerator<Point2d> enumer = points.GetEnumerator();
            if (!enumer.MoveNext())
            {
                return;
            }

            // define the ray R(r0, r1)
            Point2d r0 = enumer.Current; // indicates the current key and start of the ray
            Point2d r1 = enumer.Current; // indicates a point on the ray
            bool rayDefined = false;

            // the first point is always part of the simplification
            result(r0);

            // keep track of two test points

            if (!enumer.MoveNext())
            {
                return;
            }

            Point2d pj = enumer.Current; // the current test point (pi+1)

            while (enumer.MoveNext())
            {
                Point2d pi = pj; // the previous test point
                pj = enumer.Current;

                if (!rayDefined)
                {
                    // discard each point within minimum tolerance
                    if (point_distance2(r0, pj) < minTol2)
                    {
                        continue;
                    }

                    // the last point within minimum tolerance pi defines the ray R(r0, r1)
                    r1 = pi;
                    rayDefined = true;
                }

                // check each point pj against R(r0, r1)
                if (point_distance2(r0, pj) < maxTol2 && ray_distance2(r0, r1, pj) < minTol2)
                {
                    continue;
                }

                // found the next key at pi
                result(pi);

                // define new ray R(pi, pj)
                r0 = pi;
                rayDefined = false;
            }

            // the last point is always part of the simplification
            result(pj);
        }

        public static IEnumerable<Point2d> Opheim(this IEnumerable<Point2d> points, double minTol, double maxTol)
        {
            double minTol2 = minTol * minTol; // squared minimum distance tolerance
            double maxTol2 = maxTol * maxTol; // squared maximum distance tolerance

            // validate input and check if simplification required
            if (minTol2.EpsilonEquals(0) || maxTol2.EpsilonEquals(0))
            {
                return points;
            }
            return OpheimSafe(points, minTol2, maxTol2);
        }

        private static IEnumerable<Point2d> OpheimSafe(this IEnumerable<Point2d> points, double minTol2, double maxTol2)
        {
            IEnumerator<Point2d> enumer = points.GetEnumerator();
            if (!enumer.MoveNext())
            {
                yield break;
            }

            // define the ray R(r0, r1)
            Point2d r0 = enumer.Current; // indicates the current key and start of the ray
            Point2d r1 = enumer.Current; // indicates a point on the ray
            bool rayDefined = false;

            // the first point is always part of the simplification
            yield return r0;

            // keep track of two test points

            if (!enumer.MoveNext())
            {
                yield break;
            }

            Point2d pj = enumer.Current; // the current test point (pi+1)

            while (enumer.MoveNext())
            {
                Point2d pi = pj; // the previous test point
                pj = enumer.Current;

                if (!rayDefined)
                {
                    // discard each point within minimum tolerance
                    if (point_distance2(r0, pj) < minTol2)
                    {
                        continue;
                    }

                    // the last point within minimum tolerance pi defines the ray R(r0, r1)
                    r1 = pi;
                    rayDefined = true;
                }

                // check each point pj against R(r0, r1)
                if (point_distance2(r0, pj) < maxTol2 && ray_distance2(r0, r1, pj) < minTol2)
                {
                    continue;
                }

                // found the next key at pi
                yield return pi;

                // define new ray R(pi, pj)
                r0 = pi;
                rayDefined = false;
            }

            // the last point is always part of the simplification
            yield return pj;
        }

        #endregion Opheim

        public static double ray_distance2(Point2d r1, Point2d r2, Point2d p)
        {
            Vector2d v = r2 - r1; // vector r1 --> r2
            Vector2d w = p - r1; // vector r1 --> p

            double cv = v.Dot(v); // squared length of v
            double cw = w.Dot(v); // project w onto v

            if (cw <= 0)
            {
                // projection of w lies to the left of r1 (not on the ray)
                return point_distance2(p, r1);
            }

            // avoid problems with divisions when value_type is an integer type
            if (cv.EpsilonEquals(0))
            {
                return point_distance2(p, r1);
            }
            double fraction = cw / cv;

            Point2d proj = r1.Lerp(r2, fraction); // p projected onto ray (r1, r2)

            return point_distance2(p, proj);
        }

        public static double line_distance2(Point2d l1, Point2d l2, Point2d p)
        {
            Vector2d v = l2 - l1; // vector l1 --> l2
            Vector2d w = p - l1; // vector l1 --> p

            double cv = v.Dot(v); // squared length of v
            double cw = w.Dot(v); // project w onto v

            // avoid problems with divisions when value_type is an integer type
            if (cv.EpsilonEquals(0))
            {
                return point_distance2(p, l1);
            }
            double fraction = cw / cv;

            Point2d proj = l1.Lerp(l2, fraction); // p projected onto line (l1, l2)

            return point_distance2(p, proj);
        }

        public static double segment_distance2(Point2d s1, Point2d s2, Point2d p)
        {
            Vector2d v = s2 - s1; // vector s1 --> s2
            Vector2d w = p - s1; // vector s1 --> p

            double cw = w.Dot(v); // project w onto v
            if (cw <= 0)
            {
                // projection of w lies to the left of s1
                return point_distance2(p, s1);
            }

            double cv = v.Dot(v); // squared length of v
            if (cv <= cw)
            {
                // projection of w lies to the right of s2
                return point_distance2(p, s2);
            }

            // avoid problems with divisions when value_type is an integer type
            if (cv.EpsilonEquals(0))
            {
                return point_distance2(p, s1);
            }
            double fraction = cw / cv;

            Point2d proj = s1.Lerp(s2, fraction); // p projected onto segement (s1, s2)

            return point_distance2(p, proj);
        }

        public static double point_distance2(Point2d p1, Point2d p2)
        {
            return p1.Distance2To(p2);
        }
    }
}