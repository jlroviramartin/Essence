/// Apache Commons Math 3.6.1
using System;

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
namespace org.apache.commons.math3.analysis.solvers
{


    using MathInternalError = org.apache.commons.math3.exception.MathInternalError;
    using NoBracketingException = org.apache.commons.math3.exception.NoBracketingException;
    using NullArgumentException = org.apache.commons.math3.exception.NullArgumentException;
    using NumberIsTooSmallException = org.apache.commons.math3.exception.NumberIsTooSmallException;
    using IntegerSequence = org.apache.commons.math3.util.IntegerSequence;
    using MathArrays = org.apache.commons.math3.util.MathArrays;
    using MathUtils = org.apache.commons.math3.util.MathUtils;
    using Precision = org.apache.commons.math3.util.Precision;

    /// <summary>
    /// This class implements a modification of the <a
    /// href="http://mathworld.wolfram.com/BrentsMethod.html"> Brent algorithm</a>.
    /// <para>
    /// The changes with respect to the original Brent algorithm are:
    /// <ul>
    ///   <li>the returned value is chosen in the current interval according
    ///   to user specified <seealso cref="AllowedSolution"/></li>
    ///   <li>the maximal order for the invert polynomial root search is
    ///   user-specified instead of being invert quadratic only</li>
    /// </para>
    /// </ul><para>
    /// The given interval must bracket the root.</para>
    /// </summary>
    /// @param <T> the type of the field elements
    /// @since 3.6 </param>
    public class FieldBracketingNthOrderBrentSolver<T> : BracketedRealFieldUnivariateSolver<T> where T : org.apache.commons.math3.RealFieldElement<T>
    {

       /// <summary>
       /// Maximal aging triggering an attempt to balance the bracketing interval. </summary>
        private const int MAXIMAL_AGING = 2;

        /// <summary>
        /// Field to which the elements belong. </summary>
        private readonly Field<T> field;

        /// <summary>
        /// Maximal order. </summary>
        private readonly int maximalOrder;

        /// <summary>
        /// Function value accuracy. </summary>
        private readonly T functionValueAccuracy;

        /// <summary>
        /// Absolute accuracy. </summary>
        private readonly T absoluteAccuracy;

        /// <summary>
        /// Relative accuracy. </summary>
        private readonly T relativeAccuracy;

        /// <summary>
        /// Evaluations counter. </summary>
        private IntegerSequence.Incrementor evaluations;

        /// <summary>
        /// Construct a solver.
        /// </summary>
        /// <param name="relativeAccuracy"> Relative accuracy. </param>
        /// <param name="absoluteAccuracy"> Absolute accuracy. </param>
        /// <param name="functionValueAccuracy"> Function value accuracy. </param>
        /// <param name="maximalOrder"> maximal order. </param>
        /// <exception cref="NumberIsTooSmallException"> if maximal order is lower than 2 </exception>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public FieldBracketingNthOrderBrentSolver(final T relativeAccuracy, final T absoluteAccuracy, final T functionValueAccuracy, final int maximalOrder) throws org.apache.commons.math3.exception.NumberIsTooSmallException
        public FieldBracketingNthOrderBrentSolver(T relativeAccuracy, T absoluteAccuracy, T functionValueAccuracy, int maximalOrder)
        {
            if (maximalOrder < 2)
            {
                throw new NumberIsTooSmallException(maximalOrder, 2, true);
            }
            this.field = relativeAccuracy.getField();
            this.maximalOrder = maximalOrder;
            this.absoluteAccuracy = absoluteAccuracy;
            this.relativeAccuracy = relativeAccuracy;
            this.functionValueAccuracy = functionValueAccuracy;
            this.evaluations = IntegerSequence.Incrementor.Create();
        }

        /// <summary>
        /// Get the maximal order. </summary>
        /// <returns> maximal order </returns>
        public virtual int GetMaximalOrder()
        {
            return maximalOrder;
        }

        /// <summary>
        /// Get the maximal number of function evaluations.
        /// </summary>
        /// <returns> the maximal number of function evaluations. </returns>
        public virtual int GetMaxEvaluations()
        {
            return evaluations.GetMaximalCount();
        }

        /// <summary>
        /// Get the number of evaluations of the objective function.
        /// The number of evaluations corresponds to the last call to the
        /// {@code optimize} method. It is 0 if the method has not been
        /// called yet.
        /// </summary>
        /// <returns> the number of evaluations of the objective function. </returns>
        public virtual int GetEvaluations()
        {
            return evaluations.GetCount();
        }

        /// <summary>
        /// Get the absolute accuracy. </summary>
        /// <returns> absolute accuracy </returns>
        public virtual T GetAbsoluteAccuracy()
        {
            return absoluteAccuracy;
        }

        /// <summary>
        /// Get the relative accuracy. </summary>
        /// <returns> relative accuracy </returns>
        public virtual T GetRelativeAccuracy()
        {
            return relativeAccuracy;
        }

        /// <summary>
        /// Get the function accuracy. </summary>
        /// <returns> function accuracy </returns>
        public virtual T GetFunctionValueAccuracy()
        {
            return functionValueAccuracy;
        }

        /// <summary>
        /// Solve for a zero in the given interval.
        /// A solver may require that the interval brackets a single zero root.
        /// Solvers that do require bracketing should be able to handle the case
        /// where one of the endpoints is itself a root.
        /// </summary>
        /// <param name="maxEval"> Maximum number of evaluations. </param>
        /// <param name="f"> Function to solve. </param>
        /// <param name="min"> Lower bound for the interval. </param>
        /// <param name="max"> Upper bound for the interval. </param>
        /// <param name="allowedSolution"> The kind of solutions that the root-finding algorithm may
        /// accept as solutions. </param>
        /// <returns> a value where the function is zero. </returns>
        /// <exception cref="NullArgumentException"> if f is null. </exception>
        /// <exception cref="NoBracketingException"> if root cannot be bracketed </exception>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public T solve(final int maxEval, final org.apache.commons.math3.analysis.RealFieldUnivariateFunction<T> f, final T min, final T max, final AllowedSolution allowedSolution) throws org.apache.commons.math3.exception.NullArgumentException, org.apache.commons.math3.exception.NoBracketingException
        public virtual T Solve(int maxEval, RealFieldUnivariateFunction<T> f, T min, T max, AllowedSolution allowedSolution)
        {
            return Solve(maxEval, f, min, max, min.add(max).divide(2), allowedSolution);
        }

        /// <summary>
        /// Solve for a zero in the given interval, start at {@code startValue}.
        /// A solver may require that the interval brackets a single zero root.
        /// Solvers that do require bracketing should be able to handle the case
        /// where one of the endpoints is itself a root.
        /// </summary>
        /// <param name="maxEval"> Maximum number of evaluations. </param>
        /// <param name="f"> Function to solve. </param>
        /// <param name="min"> Lower bound for the interval. </param>
        /// <param name="max"> Upper bound for the interval. </param>
        /// <param name="startValue"> Start value to use. </param>
        /// <param name="allowedSolution"> The kind of solutions that the root-finding algorithm may
        /// accept as solutions. </param>
        /// <returns> a value where the function is zero. </returns>
        /// <exception cref="NullArgumentException"> if f is null. </exception>
        /// <exception cref="NoBracketingException"> if root cannot be bracketed </exception>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: public T solve(final int maxEval, final org.apache.commons.math3.analysis.RealFieldUnivariateFunction<T> f, final T min, final T max, final T startValue, final AllowedSolution allowedSolution) throws org.apache.commons.math3.exception.NullArgumentException, org.apache.commons.math3.exception.NoBracketingException
        public virtual T Solve(int maxEval, RealFieldUnivariateFunction<T> f, T min, T max, T startValue, AllowedSolution allowedSolution)
        {

            // Checks.
            MathUtils.CheckNotNull(f);

            // Reset.
            evaluations = evaluations.WithMaximalCount(maxEval).withStart(0);
            T zero = field.GetZero();
            T nan = zero.add(Double.NaN);

            // prepare arrays with the first points
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final T[] x = org.apache.commons.math3.util.MathArrays.buildArray(field, maximalOrder + 1);
            T[] x = MathArrays.BuildArray(field, maximalOrder + 1);
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final T[] y = org.apache.commons.math3.util.MathArrays.buildArray(field, maximalOrder + 1);
            T[] y = MathArrays.BuildArray(field, maximalOrder + 1);
            x[0] = min;
            x[1] = startValue;
            x[2] = max;

            // evaluate initial guess
            evaluations.Increment();
            y[1] = f.Value(x[1]);
            if (Precision.Equals(y[1].getReal(), 0.0, 1))
            {
                // return the initial guess if it is a perfect root.
                return x[1];
            }

            // evaluate first endpoint
            evaluations.Increment();
            y[0] = f.Value(x[0]);
            if (Precision.Equals(y[0].getReal(), 0.0, 1))
            {
                // return the first endpoint if it is a perfect root.
                return x[0];
            }

            int nbPoints;
            int signChangeIndex;
            if (y[0].multiply(y[1]).getReal() < 0)
            {

                // reduce interval if it brackets the root
                nbPoints = 2;
                signChangeIndex = 1;

            }
            else
            {

                // evaluate second endpoint
                evaluations.Increment();
                y[2] = f.Value(x[2]);
                if (Precision.Equals(y[2].getReal(), 0.0, 1))
                {
                    // return the second endpoint if it is a perfect root.
                    return x[2];
                }

                if (y[1].multiply(y[2]).getReal() < 0)
                {
                    // use all computed point as a start sampling array for solving
                    nbPoints = 3;
                    signChangeIndex = 2;
                }
                else
                {
                    throw new NoBracketingException(x[0].getReal(), x[2].getReal(), y[0].getReal(), y[2].getReal());
                }

            }

            // prepare a work array for inverse polynomial interpolation
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final T[] tmpX = org.apache.commons.math3.util.MathArrays.buildArray(field, x.length);
            T[] tmpX = MathArrays.BuildArray(field, x.Length);

            // current tightest bracketing of the root
            T xA = x[signChangeIndex - 1];
            T yA = y[signChangeIndex - 1];
            T absXA = xA.abs();
            T absYA = yA.abs();
            int agingA = 0;
            T xB = x[signChangeIndex];
            T yB = y[signChangeIndex];
            T absXB = xB.abs();
            T absYB = yB.abs();
            int agingB = 0;

            // search loop
            while (true)
            {

                // check convergence of bracketing interval
                T maxX = absXA.subtract(absXB).getReal() < 0 ? absXB : absXA;
                T maxY = absYA.subtract(absYB).getReal() < 0 ? absYB : absYA;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final T xTol = absoluteAccuracy.add(relativeAccuracy.multiply(maxX));
                T xTol = absoluteAccuracy.add(relativeAccuracy.multiply(maxX));
                if (xB.subtract(xA).subtract(xTol).getReal() <= 0 || maxY.subtract(functionValueAccuracy).getReal() < 0)
                {
                    switch (allowedSolution)
                    {
                    case org.apache.commons.math3.analysis.solvers.AllowedSolution.ANY_SIDE:
                        return absYA.subtract(absYB).getReal() < 0 ? xA : xB;
                    case org.apache.commons.math3.analysis.solvers.AllowedSolution.LEFT_SIDE:
                        return xA;
                    case org.apache.commons.math3.analysis.solvers.AllowedSolution.RIGHT_SIDE:
                        return xB;
                    case org.apache.commons.math3.analysis.solvers.AllowedSolution.BELOW_SIDE:
                        return yA.getReal() <= 0 ? xA : xB;
                    case org.apache.commons.math3.analysis.solvers.AllowedSolution.ABOVE_SIDE:
                        return yA.getReal() < 0 ? xB : xA;
                    default :
                        // this should never happen
                        throw new MathInternalError(null);
                    }
                }

                // target for the next evaluation point
                T targetY;
                if (agingA >= MAXIMAL_AGING)
                {
                    // we keep updating the high bracket, try to compensate this
                    targetY = yB.divide(16).negate();
                }
                else if (agingB >= MAXIMAL_AGING)
                {
                    // we keep updating the low bracket, try to compensate this
                    targetY = yA.divide(16).negate();
                }
                else
                {
                    // bracketing is balanced, try to find the root itself
                    targetY = zero;
                }

                // make a few attempts to guess a root,
                T nextX;
                int start = 0;
                int end = nbPoints;
                do
                {

                    // guess a value for current target, using inverse polynomial interpolation
                    Array.Copy(x, start, tmpX, start, end - start);
                    nextX = GuessX(targetY, tmpX, y, start, end);

                    if (!((nextX.subtract(xA).getReal() > 0) && (nextX.subtract(xB).getReal() < 0)))
                    {
                        // the guessed root is not strictly inside of the tightest bracketing interval

                        // the guessed root is either not strictly inside the interval or it
                        // is a NaN (which occurs when some sampling points share the same y)
                        // we try again with a lower interpolation order
                        if (signChangeIndex - start >= end - signChangeIndex)
                        {
                            // we have more points before the sign change, drop the lowest point
                            ++start;
                        }
                        else
                        {
                            // we have more points after sign change, drop the highest point
                            --end;
                        }

                        // we need to do one more attempt
                        nextX = nan;

                    }

                } while (double.IsNaN(nextX.getReal()) && (end - start > 1));

                if (double.IsNaN(nextX.getReal()))
                {
                    // fall back to bisection
                    nextX = xA.add(xB.subtract(xA).divide(2));
                    start = signChangeIndex - 1;
                    end = signChangeIndex;
                }

                // evaluate the function at the guessed root
                evaluations.Increment();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final T nextY = f.value(nextX);
                T nextY = f.Value(nextX);
                if (Precision.Equals(nextY.getReal(), 0.0, 1))
                {
                    // we have found an exact root, since it is not an approximation
                    // we don't need to bother about the allowed solutions setting
                    return nextX;
                }

                if ((nbPoints > 2) && (end - start != nbPoints))
                {

                    // we have been forced to ignore some points to keep bracketing,
                    // they are probably too far from the root, drop them from now on
                    nbPoints = end - start;
                    Array.Copy(x, start, x, 0, nbPoints);
                    Array.Copy(y, start, y, 0, nbPoints);
                    signChangeIndex -= start;

                }
                else if (nbPoints == x.Length)
                {

                    // we have to drop one point in order to insert the new one
                    nbPoints--;

                    // keep the tightest bracketing interval as centered as possible
                    if (signChangeIndex >= (x.Length + 1) / 2)
                    {
                        // we drop the lowest point, we have to shift the arrays and the index
                        Array.Copy(x, 1, x, 0, nbPoints);
                        Array.Copy(y, 1, y, 0, nbPoints);
                        --signChangeIndex;
                    }

                }

                // insert the last computed point
                //(by construction, we know it lies inside the tightest bracketing interval)
                Array.Copy(x, signChangeIndex, x, signChangeIndex + 1, nbPoints - signChangeIndex);
                x[signChangeIndex] = nextX;
                Array.Copy(y, signChangeIndex, y, signChangeIndex + 1, nbPoints - signChangeIndex);
                y[signChangeIndex] = nextY;
                ++nbPoints;

                // update the bracketing interval
                if (nextY.multiply(yA).getReal() <= 0)
                {
                    // the sign change occurs before the inserted point
                    xB = nextX;
                    yB = nextY;
                    absYB = yB.abs();
                    ++agingA;
                    agingB = 0;
                }
                else
                {
                    // the sign change occurs after the inserted point
                    xA = nextX;
                    yA = nextY;
                    absYA = yA.abs();
                    agingA = 0;
                    ++agingB;

                    // update the sign change index
                    signChangeIndex++;

                }

            }

        }

        /// <summary>
        /// Guess an x value by n<sup>th</sup> order inverse polynomial interpolation.
        /// <para>
        /// The x value is guessed by evaluating polynomial Q(y) at y = targetY, where Q
        /// is built such that for all considered points (x<sub>i</sub>, y<sub>i</sub>),
        /// Q(y<sub>i</sub>) = x<sub>i</sub>.
        /// </para> </summary>
        /// <param name="targetY"> target value for y </param>
        /// <param name="x"> reference points abscissas for interpolation,
        /// note that this array <em>is</em> modified during computation </param>
        /// <param name="y"> reference points ordinates for interpolation </param>
        /// <param name="start"> start index of the points to consider (inclusive) </param>
        /// <param name="end"> end index of the points to consider (exclusive) </param>
        /// <returns> guessed root (will be a NaN if two points share the same y) </returns>
//JAVA TO C# CONVERTER WARNING: 'final' parameters are not available in .NET:
//ORIGINAL LINE: private T guessX(final T targetY, final T[] x, final T[] y, final int start, final int end)
        private T GuessX(T targetY, T[] x, T[] y, int start, int end)
        {

            // compute Q Newton coefficients by divided differences
            for (int i = start; i < end - 1; ++i)
            {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int delta = i + 1 - start;
                int delta = i + 1 - start;
                for (int j = end - 1; j > i; --j)
                {
                    x[j] = x[j].subtract(x[j - 1]).divide(y[j].subtract(y[j - delta]));
                }
            }

            // evaluate Q(targetY)
            T x0 = field.GetZero();
            for (int j = end - 1; j >= start; --j)
            {
                x0 = x[j].add(x0.multiply(targetY.subtract(y[j])));
            }

            return x0;

        }

    }

}