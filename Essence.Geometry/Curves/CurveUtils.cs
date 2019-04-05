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

using Essence.Geometry.Core.Double;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using SysMath = System.Math;

namespace Essence.Geometry.Curves
{
    public static class CurveUtils
    {
        public static ICurve1 Split(ICurve1 curve, double tmin, double tmax)
        {
            return new SplitCurve1(curve, tmin, tmax);
        }

        public static ICurve2 Split(ICurve2 curve, double tmin, double tmax)
        {
            return new SplitCurve2(curve, tmin, tmax);
        }

        public static void TimeSubdivision(ICurve2 curve, int size, IList<Point2d> points)
        {
            Debug.Assert(size >= 2);
            Debug.Assert(points != null);

            double delta = (curve.TMax - curve.TMin) / (size - 1);
            double t = curve.TMin;

            for (int i = 0; i < size; i++)
            {
                points.Add(curve.GetPosition(t));
                t += delta;
            }
        }

        public static void LengthSubdivision(ICurve2 curve, int size, IList<Point2d> puntos)
        {
            Debug.Assert(size >= 2);
            Debug.Assert(puntos != null);

            double delta = curve.TotalLength / (size - 1);
            double longitud = 0;

            for (int i = 0; i < size; i++)
            {
                double t = curve.GetT(longitud);
                puntos.Add(curve.GetPosition(t));
                longitud += delta;
            }
        }

        /// <summary>
        /// This method evaluates the t of a curve based on its length.
        /// </summary>
        /// <param name="flen">Evaluates the length.</param>
        /// <param name="dflen">First derivative of the length.</param>
        /// <param name="lower">Min t.</param>
        /// <param name="upper">Max t.</param>
        /// <param name="length">Length.</param>
        /// <param name="maxLen">Max length.</param>
        /// <param name="iterations">Number of iterations.</param>
        /// <param name="tolerance">Tolerance.</param>
        /// <returns></returns>
        public static double FindTime(Func<double, double> flen, Func<double, double> dflen,
                                      double lower, double upper,
                                      double length, double maxLen,
                                      int iterations, double tolerance)
        {
            if (length <= 0)
            {
                return lower;
            }

            if (length >= maxLen)
            {
                return upper;
            }

            // If L(t) is the length function for t in [tmin,tmax], the derivative is
            // L'(t) = |x'(t)| >= 0 (the magnitude of speed).  Therefore, L(t) is a
            // nondecreasing function (and it is assumed that x'(t) is zero only at
            // isolated points; that is, no degenerate curves allowed).  The second
            // derivative is L"(t).  If L"(t) >= 0 for all t, L(t) is a convex
            // function and Newton's method for root finding is guaranteed to
            // converge.  However, L"(t) can be negative, which can lead to Newton
            // iterates outside the domain [tmin,tmax].  The algorithm here avoids
            // this problem by using a hybrid of Newton's method and bisection.

            // Initial guess for Newton's method.
            double ratio = length / maxLen;
            double t = (1 - ratio) * lower + ratio * upper;

            // Initial root-bounding interval for bisection: lower, upper.

            for (int i = 0; i < iterations; i++)
            {
                double difference = flen(t) - length;
                if (SysMath.Abs(difference) < tolerance)
                {
                    // |L(t)-length| is close enough to zero, report t as the time
                    // at which 'length' is attained.
                    return t;
                }

                // Generate a candidate for Newton's method.
                double tCandidate = t - difference / dflen(t);

                // Update the root-bounding interval and test for containment of the
                // candidate.
                if (difference > 0)
                {
                    upper = t;
                    if (tCandidate <= lower)
                    {
                        // Candidate is outside the root-bounding interval.  Use
                        // bisection instead.
                        t = 0.5 * (upper + lower);
                    }
                    else
                    {
                        // There is no need to compare to 'upper' because the tangent
                        // line has positive slope, guaranteeing that the t-axis
                        // intercept is smaller than 'upper'.
                        t = tCandidate;
                    }
                }
                else
                {
                    lower = t;
                    if (tCandidate >= upper)
                    {
                        // Candidate is outside the root-bounding interval.  Use
                        // bisection instead.
                        t = 0.5 * (upper + lower);
                    }
                    else
                    {
                        // There is no need to compare to 'lower' because the tangent
                        // line has positive slope, guaranteeing that the t-axis
                        // intercept is larger than 'lower'.
                        t = tCandidate;
                    }
                }
            }

            // A root was not found according to the specified number of iterations
            // and tolerance.  You might want to increase iterations or tolerance or
            // integration accuracy.  However, in this application it is likely that
            // the time values are oscillating, due to the limited numerical
            // precision of 32-bit floats.  It is safe to use the last computed time.
            return t;
        }


        private sealed class SplitCurve1 : WrapperCurve1
        {
            public SplitCurve1(ICurve1 curve, double tmin, double tmax)
                : base(curve)
            {
                this.tmin = tmin;
                this.tmax = tmax;
            }

            private readonly double tmin;
            private readonly double tmax;

            public override double TMin
            {
                get { return this.tmin; }
            }

            public override double TMax
            {
                get { return this.tmax; }
            }

            public override double TotalLength
            {
                get { return this.curve.GetLength(this.tmin, this.tmax); }
            }
        }

        private sealed class SplitCurve2 : WrapperCurve2
        {
            public SplitCurve2(ICurve2 curve, double tmin, double tmax)
                : base(curve)
            {
                this.tmin = tmin;
                this.tmax = tmax;
            }

            private readonly double tmin;
            private readonly double tmax;

            public override double TMin
            {
                get { return this.tmin; }
            }

            public override double TMax
            {
                get { return this.tmax; }
            }

            public override double TotalLength
            {
                get { return this.curve.GetLength(this.tmin, this.tmax); }
            }
        }
    }
}