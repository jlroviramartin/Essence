/*
 * Licensed to the Apache Software Foundation (ASF) under one or more
 * contributor license agreements.  See the NOTICE file distributed with
 * this work for additional information regarding copyright ownership.
 * The ASF licenses this file to You under the Apache License, Version 2.0
 * (the "License"); you may not use this file except in compliance with
 * the License.  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using org.apache.commons.math3.analysis.exception;
using FastMath = System.Math;

namespace org.apache.commons.math3.analysis.solvers
{
    /// <summary>
    /// Utility routines for <seealso cref="UnivariateSolver"/> objects.
    /// 
    /// @version $Id: UnivariateSolverUtils.java 1579346 2014-03-19 18:43:39Z erans $
    /// </summary>
    public static class UnivariateSolverUtils
    {
        /// <summary>
        /// Convenience method to find a zero of a univariate real function.  A default
        /// solver is used.
        /// </summary>
        /// <param name="function"> Function. </param>
        /// <param name="x0"> Lower bound for the interval. </param>
        /// <param name="x1"> Upper bound for the interval. </param>
        /// <returns> a value where the function is zero. </returns>
        /// <exception cref="NoBracketingException"> if the function has the same sign at the
        /// endpoints. </exception>
        /// <exception cref="NullArgumentException"> if {@code function} is {@code null}. </exception>
        public static double Solve(UnivariateFunction function, double x0, double x1)
        {
            if (function == null)
            {
                throw new ArgumentNullException("LocalizedFormats.FUNCTION");
            }
            UnivariateSolver solver = new BrentSolver();
            return solver.Solve(int.MaxValue, function, x0, x1);
        }

        /// <summary>
        /// Convenience method to find a zero of a univariate real function.  A default
        /// solver is used.
        /// </summary>
        /// <param name="function"> Function. </param>
        /// <param name="x0"> Lower bound for the interval. </param>
        /// <param name="x1"> Upper bound for the interval. </param>
        /// <param name="absoluteAccuracy"> Accuracy to be used by the solver. </param>
        /// <returns> a value where the function is zero. </returns>
        /// <exception cref="NoBracketingException"> if the function has the same sign at the
        /// endpoints. </exception>
        /// <exception cref="NullArgumentException"> if {@code function} is {@code null}. </exception>
        public static double Solve(UnivariateFunction function, double x0, double x1, double absoluteAccuracy)
        {
            if (function == null)
            {
                throw new ArgumentNullException("LocalizedFormats.FUNCTION");
            }
            UnivariateSolver solver = new BrentSolver(absoluteAccuracy);
            return solver.Solve(int.MaxValue, function, x0, x1);
        }

        /// <summary>
        /// Force a root found by a non-bracketing solver to lie on a specified side,
        /// as if the solver was a bracketing one. </summary>
        /// <param name="maxEval"> maximal number of new evaluations of the function
        /// (evaluations already done for finding the root should have already been subtracted
        /// from this number) </param>
        /// <param name="f"> function to solve </param>
        /// <param name="bracketing"> bracketing solver to use for shifting the root </param>
        /// <param name="baseRoot"> original root found by a previous non-bracketing solver </param>
        /// <param name="min"> minimal bound of the search interval </param>
        /// <param name="max"> maximal bound of the search interval </param>
        /// <param name="allowedSolution"> the kind of solutions that the root-finding algorithm may
        /// accept as solutions. </param>
        /// <returns> a root approximation, on the specified side of the exact root </returns>
        /// <exception cref="NoBracketingException"> if the function has the same sign at the
        /// endpoints. </exception>
        public static double ForceSide(int maxEval, UnivariateFunction f, BracketedUnivariateSolver<UnivariateFunction> bracketing, double baseRoot, double min, double max, AllowedSolution allowedSolution)
        {
            if (allowedSolution == AllowedSolution.ANY_SIDE)
            {
                // no further bracketing required
                return baseRoot;
            }

            // find a very small interval bracketing the root
            double step = FastMath.Max(bracketing.AbsoluteAccuracy, FastMath.Abs(baseRoot * bracketing.RelativeAccuracy));
            double xLo = FastMath.Max(min, baseRoot - step);
            double fLo = f.Value(xLo);
            double xHi = FastMath.Min(max, baseRoot + step);
            double fHi = f.Value(xHi);
            int remainingEval = maxEval - 2;
            while (remainingEval > 0)
            {
                if ((fLo >= 0 && fHi <= 0) || (fLo <= 0 && fHi >= 0))
                {
                    // compute the root on the selected side
                    return bracketing.Solve(remainingEval, f, xLo, xHi, baseRoot, allowedSolution);
                }

                // try increasing the interval
                bool changeLo = false;
                bool changeHi = false;
                if (fLo < fHi)
                {
                    // increasing function
                    if (fLo >= 0)
                    {
                        changeLo = true;
                    }
                    else
                    {
                        changeHi = true;
                    }
                }
                else if (fLo > fHi)
                {
                    // decreasing function
                    if (fLo <= 0)
                    {
                        changeLo = true;
                    }
                    else
                    {
                        changeHi = true;
                    }
                }
                else
                {
                    // unknown variation
                    changeLo = true;
                    changeHi = true;
                }

                // update the lower bound
                if (changeLo)
                {
                    xLo = FastMath.Max(min, xLo - step);
                    fLo = f.Value(xLo);
                    remainingEval--;
                }

                // update the higher bound
                if (changeHi)
                {
                    xHi = FastMath.Min(max, xHi + step);
                    fHi = f.Value(xHi);
                    remainingEval--;
                }
            }

            throw new NoBracketingException("LocalizedFormats.FAILED_BRACKETING", xLo, xHi, fLo, fHi, maxEval - remainingEval, maxEval, baseRoot, min, max);
        }

        /// <summary>
        /// This method simply calls {@link #bracket(UnivariateFunction, double, double, double,
        /// double, double, int) bracket(function, initial, lowerBound, upperBound, q, r, maximumIterations)}
        /// with {@code q} and {@code r} set to 1.0 and {@code maximumIterations} set to {@code Integer.MAX_VALUE}.
        /// <strong>Note: </strong> this method can take
        /// <code>Integer.MAX_VALUE</code> iterations to throw a
        /// <code>ConvergenceException.</code>  Unless you are confident that there
        /// is a root between <code>lowerBound</code> and <code>upperBound</code>
        /// near <code>initial,</code> it is better to use
        /// {@link #bracket(UnivariateFunction, double, double, double, double,
        /// double, int) bracket(function, initial, lowerBound, upperBound, q, r, maximumIterations)},
        /// explicitly specifying the maximum number of iterations.</p>
        /// </summary>
        /// <param name="function"> Function. </param>
        /// <param name="initial"> Initial midpoint of interval being expanded to
        /// bracket a root. </param>
        /// <param name="lowerBound"> Lower bound (a is never lower than this value) </param>
        /// <param name="upperBound"> Upper bound (b never is greater than this
        /// value). </param>
        /// <returns> a two-element array holding a and b. </returns>
        /// <exception cref="NoBracketingException"> if a root cannot be bracketted. </exception>
        /// <exception cref="NotStrictlyPositiveException"> if {@code maximumIterations <= 0}. </exception>
        /// <exception cref="NullArgumentException"> if {@code function} is {@code null}. </exception>
        public static double[] Bracket(UnivariateFunction function, double initial, double lowerBound, double upperBound)
        {
            return Bracket(function, initial, lowerBound, upperBound, 1.0, 1.0, int.MaxValue);
        }

        /// <summary>
        /// This method simply calls {@link #bracket(UnivariateFunction, double, double, double,
        /// double, double, int) bracket(function, initial, lowerBound, upperBound, q, r, maximumIterations)}
        /// with {@code q} and {@code r} set to 1.0. </summary>
        /// <param name="function"> Function. </param>
        /// <param name="initial"> Initial midpoint of interval being expanded to
        /// bracket a root. </param>
        /// <param name="lowerBound"> Lower bound (a is never lower than this value). </param>
        /// <param name="upperBound"> Upper bound (b never is greater than this
        /// value). </param>
        /// <param name="maximumIterations"> Maximum number of iterations to perform </param>
        /// <returns> a two element array holding a and b. </returns>
        /// <exception cref="NoBracketingException"> if the algorithm fails to find a and b
        /// satisfying the desired conditions. </exception>
        /// <exception cref="NotStrictlyPositiveException"> if {@code maximumIterations <= 0}. </exception>
        /// <exception cref="NullArgumentException"> if {@code function} is {@code null}. </exception>
        public static double[] Bracket(UnivariateFunction function, double initial, double lowerBound, double upperBound, int maximumIterations)
        {
            return Bracket(function, initial, lowerBound, upperBound, 1.0, 1.0, maximumIterations);
        }

        /// <summary>
        /// This method attempts to find two values a and b satisfying <ul>
        /// <li> {@code lowerBound <= a < initial < b <= upperBound} </li>
        /// <li> {@code f(a) * f(b) <= 0} </li>
        /// </ul>
        /// If {@code f} is continuous on {@code [a,b]}, this means that {@code a}
        /// and {@code b} bracket a root of {@code f}.
        /// <para>
        /// The algorithm checks the sign of \( f(l_k) \) and \( f(u_k) \) for increasing
        /// values of k, where \( l_k = max(lower, initial - \delta_k) \),
        /// \( u_k = min(upper, initial + \delta_k) \), using recurrence
        /// \( \delta_{k+1} = r \delta_k + q, \delta_0 = 0\) and starting search with \( k=1 \).
        /// The algorithm stops when one of the following happens: <ul>
        /// <li> at least one positive and one negative value have been found --  success!</li>
        /// <li> both endpoints have reached their respective limites -- NoBracketingException </li>
        /// <li> {@code maximumIterations} iterations elapse -- NoBracketingException </li></ul></para>
        /// <para>
        /// If different signs are found at first iteration ({@code k=1}), then the returned
        /// interval will be \( [a, b] = [l_1, u_1] \). If different signs are found at a later
        /// iteration ({code k>1}, then the returned interval will be either
        /// \( [a, b] = [l_{k+1}, l_{k}] \) or ( [a, b] = [u_{k}, u_{k+1}] \). A root solver called
        /// with these parameters will therefore start with the smallest bracketing interval known
        /// at this step.
        /// </para>
        /// <para>
        /// Interval expansion rate is tuned by changing the recurrence parameters {@code r} and
        /// {@code q}. When the multiplicative factor {@code r} is set to 1, the sequence is a
        /// simple arithmetic sequence with linear increase. When the multiplicative factor {@code r}
        /// is larger than 1, the sequence has an asymtotically exponential rate. Note than the
        /// additive parameter {@code q} should never be set to zero, otherwise the interval would
        /// degenerate to the single initial point for all values of {@code k}.
        /// </para>
        /// <para>
        /// As a rule of thumb, when the location of the root is expected to be approximately known
        /// within some error margin, {@code r} should be set to 1 and {@code q} should be set to the
        /// order of magnitude of the error margin. When the location of the root is really a wild guess,
        /// then {@code r} should be set to a value larger than 1 (typically 2 to double the interval
        /// length at each iteration) and {@code q} should be set according to half the initial
        /// search interval length.
        /// </para>
        /// <para>
        /// As an example, if we consider the trivial function {@code f(x) = 1 - x} and use
        /// {@code initial = 4}, {@code r = 1}, {@code q = 2}, the algorithm will compute
        /// {@code f(4-2) = f(2) = -1} and {@code f(4+2) = f(6) = -5} for {@code k = 1}, then
        /// {@code f(4-4) = f(0) = +1} and {@code f(4+4) = f(8) = -7} for {@code k = 2}. Then it will
        /// return the interval {@code [0, 2]} as the smallest one known to be bracketing the root.
        /// As shown by this example, the initial value (here {@code 4}) may lie outside of the returned
        /// bracketing interval.
        /// </para> </summary>
        /// <param name="function"> function to check </param>
        /// <param name="initial"> Initial midpoint of interval being expanded to
        /// bracket a root. </param>
        /// <param name="lowerBound"> Lower bound (a is never lower than this value). </param>
        /// <param name="upperBound"> Upper bound (b never is greater than this
        /// value). </param>
        /// <param name="q"> additive offset used to compute bounds sequence (must be strictly positive) </param>
        /// <param name="r"> multiplicative factor used to compute bounds sequence </param>
        /// <param name="maximumIterations"> Maximum number of iterations to perform </param>
        /// <returns> a two element array holding the bracketing values. </returns>
        /// <exception cref="NoBracketingException"> if function cannot be bracketed in the search interval </exception>
        public static double[] Bracket(UnivariateFunction function, double initial, double lowerBound, double upperBound, double q, double r, int maximumIterations)
        {
            if (function == null)
            {
                throw new ArgumentNullException("LocalizedFormats.FUNCTION");
            }
            if (q <= 0)
            {
                throw new NotStrictlyPositiveException(q);
            }
            if (maximumIterations <= 0)
            {
                throw new NotStrictlyPositiveException("LocalizedFormats.INVALID_MAX_ITERATIONS", maximumIterations);
            }
            VerifySequence(lowerBound, initial, upperBound);

            // initialize the recurrence
            double a = initial;
            double b = initial;
            double fa = double.NaN;
            double fb = double.NaN;
            double delta = 0;

            for (int numIterations = 0; (numIterations < maximumIterations) && (a > lowerBound || b > upperBound); ++numIterations)
            {
                double previousA = a;
                double previousFa = fa;
                double previousB = b;
                double previousFb = fb;

                delta = r * delta + q;
                a = FastMath.Max(initial - delta, lowerBound);
                b = FastMath.Min(initial + delta, upperBound);
                fa = function.Value(a);
                fb = function.Value(b);

                if (numIterations == 0)
                {
                    // at first iteration, we don't have a previous interval
                    // we simply compare both sides of the initial interval
                    if (fa * fb <= 0)
                    {
                        // the first interval already brackets a root
                        return new double[] { a, b };
                    }
                }
                else
                {
                    // we have a previous interval with constant sign and expand it,
                    // we expect sign changes to occur at boundaries
                    if (fa * previousFa <= 0)
                    {
                        // sign change detected at near lower bound
                        return new double[] { a, previousA };
                    }
                    else if (fb * previousFb <= 0)
                    {
                        // sign change detected at near upper bound
                        return new double[] { previousB, b };
                    }
                }
            }

            // no bracketing found
            throw new NoBracketingException(a, b, fa, fb);
        }

        /// <summary>
        /// Compute the midpoint of two values.
        /// </summary>
        /// <param name="a"> first value. </param>
        /// <param name="b"> second value. </param>
        /// <returns> the midpoint. </returns>
        public static double Midpoint(double a, double b)
        {
            return (a + b) * 0.5;
        }

        /// <summary>
        /// Check whether the interval bounds bracket a root. That is, if the
        /// values at the endpoints are not equal to zero, then the function takes
        /// opposite signs at the endpoints.
        /// </summary>
        /// <param name="function"> Function. </param>
        /// <param name="lower"> Lower endpoint. </param>
        /// <param name="upper"> Upper endpoint. </param>
        /// <returns> {@code true} if the function values have opposite signs at the
        /// given points. </returns>
        /// <exception cref="NullArgumentException"> if {@code function} is {@code null}. </exception>
        public static bool IsBracketing(UnivariateFunction function, double lower, double upper)
        {
            if (function == null)
            {
                throw new ArgumentNullException("LocalizedFormats.FUNCTION");
            }
            double fLo = function.Value(lower);
            double fHi = function.Value(upper);
            return (fLo >= 0 && fHi <= 0) || (fLo <= 0 && fHi >= 0);
        }

        /// <summary>
        /// Check whether the arguments form a (strictly) increasing sequence.
        /// </summary>
        /// <param name="start"> First number. </param>
        /// <param name="mid"> Second number. </param>
        /// <param name="end"> Third number. </param>
        /// <returns> {@code true} if the arguments form an increasing sequence. </returns>
        public static bool IsSequence(double start, double mid, double end)
        {
            return (start < mid) && (mid < end);
        }

        /// <summary>
        /// Check that the endpoints specify an interval.
        /// </summary>
        /// <param name="lower"> Lower endpoint. </param>
        /// <param name="upper"> Upper endpoint. </param>
        /// <exception cref="NumberIsTooLargeException"> if {@code lower >= upper}. </exception>
        public static void VerifyInterval(double lower, double upper)
        {
            if (lower >= upper)
            {
                throw new NumberIsTooLargeException("LocalizedFormats.ENDPOINTS_NOT_AN_INTERVAL", lower, upper, false);
            }
        }

        /// <summary>
        /// Check that {@code lower < initial < upper}.
        /// </summary>
        /// <param name="lower"> Lower endpoint. </param>
        /// <param name="initial"> Initial value. </param>
        /// <param name="upper"> Upper endpoint. </param>
        /// <exception cref="NumberIsTooLargeException"> if {@code lower >= initial} or
        /// {@code initial >= upper}. </exception>
        public static void VerifySequence(double lower, double initial, double upper)
        {
            VerifyInterval(lower, initial);
            VerifyInterval(initial, upper);
        }

        /// <summary>
        /// Check that the endpoints specify an interval and the end points
        /// bracket a root.
        /// </summary>
        /// <param name="function"> Function. </param>
        /// <param name="lower"> Lower endpoint. </param>
        /// <param name="upper"> Upper endpoint. </param>
        /// <exception cref="NoBracketingException"> if the function has the same sign at the
        /// endpoints. </exception>
        /// <exception cref="NullArgumentException"> if {@code function} is {@code null}. </exception>
        public static void VerifyBracketing(UnivariateFunction function, double lower, double upper)
        {
            if (function == null)
            {
                throw new ArgumentNullException("LocalizedFormats.FUNCTION");
            }
            VerifyInterval(lower, upper);
            if (!IsBracketing(function, lower, upper))
            {
                throw new NoBracketingException(lower, upper, function.Value(lower), function.Value(upper));
            }
        }
    }
}