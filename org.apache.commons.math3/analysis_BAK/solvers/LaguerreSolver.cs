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
    using FastMath = System.Math;

    /*using Complex = org.apache.commons.math3.complex.Complex;
    using ComplexUtils = org.apache.commons.math3.complex.ComplexUtils;
    using PolynomialFunction = org.apache.commons.math3.analysis.polynomials.PolynomialFunction;
    using NoBracketingException = org.apache.commons.math3.exception.NoBracketingException;
    using NullArgumentException = org.apache.commons.math3.exception.NullArgumentException;
    using NoDataException = org.apache.commons.math3.exception.NoDataException;
    using TooManyEvaluationsException = org.apache.commons.math3.exception.TooManyEvaluationsException;
    using NumberIsTooLargeException = org.apache.commons.math3.exception.NumberIsTooLargeException;
    using LocalizedFormats = org.apache.commons.math3.exception.util.LocalizedFormats;
    using FastMath = org.apache.commons.math3.util.FastMath;*/

    /// <summary>
    /// Implements the <a href="http://mathworld.wolfram.com/LaguerresMethod.html">
    /// Laguerre's Method</a> for root finding of real coefficient polynomials.
    /// For reference, see
    /// <quote>
    ///  <b>A First Course in Numerical Analysis</b>
    ///  ISBN 048641454X, chapter 8.
    /// </quote>
    /// Laguerre's method is global in the sense that it can start with any initial
    /// approximation and be able to solve all roots from that point.
    /// The algorithm requires a bracketing condition.
    /// 
    /// @version $Id: LaguerreSolver.java 1422195 2012-12-15 06:45:18Z psteitz $
    /// @since 1.2
    /// </summary>
    public class LaguerreSolver : AbstractPolynomialSolver
    {
        private bool InstanceFieldsInitialized = false;

        private void InitializeInstanceFields()
        {
            complexSolver = new ComplexSolver(this);
        }

        /// <summary>
        /// Default absolute accuracy. </summary>
        private const double DEFAULT_ABSOLUTE_ACCURACY = 1e-6;
        /// <summary>
        /// Complex solver. </summary>
        private ComplexSolver complexSolver;

        /// <summary>
        /// Construct a solver with default accuracy (1e-6).
        /// </summary>
        public LaguerreSolver() : this(DEFAULT_ABSOLUTE_ACCURACY)
        {
            if (!InstanceFieldsInitialized)
            {
                InitializeInstanceFields();
                InstanceFieldsInitialized = true;
            }
        }
        /// <summary>
        /// Construct a solver.
        /// </summary>
        /// <param name="absoluteAccuracy"> Absolute accuracy. </param>
        public LaguerreSolver(double absoluteAccuracy) : base(absoluteAccuracy)
        {
            if (!InstanceFieldsInitialized)
            {
                InitializeInstanceFields();
                InstanceFieldsInitialized = true;
            }
        }
        /// <summary>
        /// Construct a solver.
        /// </summary>
        /// <param name="relativeAccuracy"> Relative accuracy. </param>
        /// <param name="absoluteAccuracy"> Absolute accuracy. </param>
        public LaguerreSolver(double relativeAccuracy, double absoluteAccuracy) : base(relativeAccuracy, absoluteAccuracy)
        {
            if (!InstanceFieldsInitialized)
            {
                InitializeInstanceFields();
                InstanceFieldsInitialized = true;
            }
        }
        /// <summary>
        /// Construct a solver.
        /// </summary>
        /// <param name="relativeAccuracy"> Relative accuracy. </param>
        /// <param name="absoluteAccuracy"> Absolute accuracy. </param>
        /// <param name="functionValueAccuracy"> Function value accuracy. </param>
        public LaguerreSolver(double relativeAccuracy, double absoluteAccuracy, double functionValueAccuracy) : base(relativeAccuracy, absoluteAccuracy, functionValueAccuracy)
        {
            if (!InstanceFieldsInitialized)
            {
                InitializeInstanceFields();
                InstanceFieldsInitialized = true;
            }
        }

        /// <summary>
        /// {@inheritDoc}
        /// </summary>
        public override double DoSolve()
        {
            double min = Min;
            double max = Max;
            double initial = StartValue;
            double functionValueAccuracy = FunctionValueAccuracy;

            VerifySequence(min, initial, max);

            // Return the initial guess if it is good enough.
            double yInitial = ComputeObjectiveValue(initial);
            if (FastMath.Abs(yInitial) <= functionValueAccuracy)
            {
                return initial;
            }

            // Return the first endpoint if it is good enough.
            double yMin = ComputeObjectiveValue(min);
            if (FastMath.Abs(yMin) <= functionValueAccuracy)
            {
                return min;
            }

            // Reduce interval if min and initial bracket the root.
            if (yInitial * yMin < 0)
            {
                return Laguerre(min, initial, yMin, yInitial);
            }

            // Return the second endpoint if it is good enough.
            double yMax = ComputeObjectiveValue(max);
            if (FastMath.Abs(yMax) <= functionValueAccuracy)
            {
                return max;
            }

            // Reduce interval if initial and max bracket the root.
            if (yInitial * yMax < 0)
            {
                return Laguerre(initial, max, yInitial, yMax);
            }

            throw new NoBracketingException(min, max, yMin, yMax);
        }

        /// <summary>
        /// Find a real root in the given interval.
        /// 
        /// Despite the bracketing condition, the root returned by
        /// <seealso cref="LaguerreSolver.ComplexSolver#solve(Complex[],Complex)"/> may
        /// not be a real zero inside {@code [min, max]}.
        /// For example, <code>p(x) = x<sup>3</sup> + 1,</code>
        /// with {@code min = -2}, {@code max = 2}, {@code initial = 0}.
        /// When it occurs, this code calls
        /// <seealso cref="LaguerreSolver.ComplexSolver#solveAll(Complex[],Complex)"/>
        /// in order to obtain all roots and picks up one real root.
        /// </summary>
        /// <param name="lo"> Lower bound of the search interval. </param>
        /// <param name="hi"> Higher bound of the search interval. </param>
        /// <param name="fLo"> Function value at the lower bound of the search interval. </param>
        /// <param name="fHi"> Function value at the higher bound of the search interval. </param>
        /// <returns> the point at which the function value is zero. </returns>
        /// @deprecated This method should not be part of the public API: It will
        /// be made private in version 4.0. 
        [Obsolete("This method should not be part of the public API: It will")]
        public virtual double Laguerre(double lo, double hi, double fLo, double fHi)
        {
            Complex[] c = ComplexUtils.convertToComplex(Coefficients);

            Complex initial = new Complex(0.5 * (lo + hi), 0);
            Complex z = ComplexSolver.Solve(c, initial);
            if (ComplexSolver.IsRoot(lo, hi, z))
            {
                return z.Real;
            }
            else
            {
                double r = double.NaN;
                // Solve all roots and select the one we are seeking.
                Complex[] root = ComplexSolver.SolveAll(c, initial);
                for (int i = 0; i < root.Length; i++)
                {
                    if (ComplexSolver.IsRoot(lo, hi, root[i]))
                    {
                        r = root[i].Real;
                        break;
                    }
                }
                return r;
            }
        }

        /// <summary>
        /// Find all complex roots for the polynomial with the given
        /// coefficients, starting from the given initial value.
        /// <br/>
        /// Note: This method is not part of the API of <seealso cref="BaseUnivariateSolver"/>.
        /// </summary>
        /// <param name="coefficients"> Polynomial coefficients. </param>
        /// <param name="initial"> Start value. </param>
        /// <returns> the point at which the function value is zero. </returns>
        /// <exception cref="org.apache.commons.math3.exception.TooManyEvaluationsException">
        /// if the maximum number of evaluations is exceeded. </exception>
        /// <exception cref="NullArgumentException"> if the {@code coefficients} is
        /// {@code null}. </exception>
        /// <exception cref="NoDataException"> if the {@code coefficients} array is empty.
        /// @since 3.1 </exception>
        public virtual Complex[] SolveAllComplex(double[] coefficients, double initial)
        {
            Setup(int.MaxValue, new PolynomialFunction(coefficients), double.NegativeInfinity, double.PositiveInfinity, initial);
            return ComplexSolver.SolveAll(ComplexUtils.convertToComplex(coefficients), new Complex(initial, 0d));
        }

        /// <summary>
        /// Find a complex root for the polynomial with the given coefficients,
        /// starting from the given initial value.
        /// <br/>
        /// Note: This method is not part of the API of <seealso cref="BaseUnivariateSolver"/>.
        /// </summary>
        /// <param name="coefficients"> Polynomial coefficients. </param>
        /// <param name="initial"> Start value. </param>
        /// <returns> the point at which the function value is zero. </returns>
        /// <exception cref="org.apache.commons.math3.exception.TooManyEvaluationsException">
        /// if the maximum number of evaluations is exceeded. </exception>
        /// <exception cref="NullArgumentException"> if the {@code coefficients} is
        /// {@code null}. </exception>
        /// <exception cref="NoDataException"> if the {@code coefficients} array is empty.
        /// @since 3.1 </exception>
        public virtual Complex SolveComplex(double[] coefficients, double initial)
        {
            Setup(int.MaxValue, new PolynomialFunction(coefficients), double.NegativeInfinity, double.PositiveInfinity, initial);
            return ComplexSolver.Solve(ComplexUtils.convertToComplex(coefficients), new Complex(initial, 0d));
        }

        /// <summary>
        /// Class for searching all (complex) roots.
        /// </summary>
        private class ComplexSolver
        {
            private readonly LaguerreSolver OuterInstance;

            public ComplexSolver(LaguerreSolver outerInstance)
            {
                this.OuterInstance = outerInstance;
            }

            /// <summary>
            /// Check whether the given complex root is actually a real zero
            /// in the given interval, within the solver tolerance level.
            /// </summary>
            /// <param name="min"> Lower bound for the interval. </param>
            /// <param name="max"> Upper bound for the interval. </param>
            /// <param name="z"> Complex root. </param>
            /// <returns> {@code true} if z is a real zero. </returns>
            public virtual bool IsRoot(double min, double max, Complex z)
            {
                if (outerInstance.IsSequence(min, z.Real, max))
                {
                    double tolerance = FastMath.max(outerInstance.RelativeAccuracy * z.abs(), outerInstance.AbsoluteAccuracy);
                    return (FastMath.abs(z.Imaginary) <= tolerance) || (z.abs() <= outerInstance.FunctionValueAccuracy);
                }
                return false;
            }

            /// <summary>
            /// Find all complex roots for the polynomial with the given
            /// coefficients, starting from the given initial value.
            /// </summary>
            /// <param name="coefficients"> Polynomial coefficients. </param>
            /// <param name="initial"> Start value. </param>
            /// <returns> the point at which the function value is zero. </returns>
            /// <exception cref="org.apache.commons.math3.exception.TooManyEvaluationsException">
            /// if the maximum number of evaluations is exceeded. </exception>
            /// <exception cref="NullArgumentException"> if the {@code coefficients} is
            /// {@code null}. </exception>
            /// <exception cref="NoDataException"> if the {@code coefficients} array is empty. </exception>
            public virtual Complex[] SolveAll(Complex[] coefficients, Complex initial)
            {
                if (coefficients == null)
                {
                    throw new NullArgumentException();
                }
                int n = coefficients.Length - 1;
                if (n == 0)
                {
                    throw new NoDataException(LocalizedFormats.POLYNOMIAL);
                }
                // Coefficients for deflated polynomial.
                Complex[] c = new Complex[n + 1];
                for (int i = 0; i <= n; i++)
                {
                    c[i] = coefficients[i];
                }

                // Solve individual roots successively.
                Complex[] root = new Complex[n];
                for (int i = 0; i < n; i++)
                {
                    Complex[] subarray = new Complex[n - i + 1];
                    Array.Copy(c, 0, subarray, 0, subarray.Length);
                    root[i] = Solve(subarray, initial);
                    // Polynomial deflation using synthetic division.
                    Complex newc = c[n - i];
                    Complex oldc = null;
                    for (int j = n - i - 1; j >= 0; j--)
                    {
                        oldc = c[j];
                        c[j] = newc;
                        newc = oldc.add(newc.multiply(root[i]));
                    }
                }

                return root;
            }

            /// <summary>
            /// Find a complex root for the polynomial with the given coefficients,
            /// starting from the given initial value.
            /// </summary>
            /// <param name="coefficients"> Polynomial coefficients. </param>
            /// <param name="initial"> Start value. </param>
            /// <returns> the point at which the function value is zero. </returns>
            /// <exception cref="org.apache.commons.math3.exception.TooManyEvaluationsException">
            /// if the maximum number of evaluations is exceeded. </exception>
            /// <exception cref="NullArgumentException"> if the {@code coefficients} is
            /// {@code null}. </exception>
            /// <exception cref="NoDataException"> if the {@code coefficients} array is empty. </exception>
            public virtual Complex Solve(Complex[] coefficients, Complex initial)
            {
                if (coefficients == null)
                {
                    throw new NullArgumentException();
                }

                int n = coefficients.Length - 1;
                if (n == 0)
                {
                    throw new NoDataException(LocalizedFormats.POLYNOMIAL);
                }

                double absoluteAccuracy = outerInstance.AbsoluteAccuracy;
                double relativeAccuracy = outerInstance.RelativeAccuracy;
                double functionValueAccuracy = outerInstance.FunctionValueAccuracy;

                Complex nC = new Complex(n, 0);
                Complex n1C = new Complex(n - 1, 0);

                Complex z = initial;
                Complex oldz = new Complex(double.PositiveInfinity, double.PositiveInfinity);
                while (true)
                {
                    // Compute pv (polynomial value), dv (derivative value), and
                    // d2v (second derivative value) simultaneously.
                    Complex pv = coefficients[n];
                    Complex dv = Complex.ZERO;
                    Complex d2v = Complex.ZERO;
                    for (int j = n - 1; j >= 0; j--)
                    {
                        d2v = dv.add(z.multiply(d2v));
                        dv = pv.add(z.multiply(dv));
                        pv = coefficients[j].add(z.multiply(pv));
                    }
                    d2v = d2v.multiply(new Complex(2.0, 0.0));

                    // Check for convergence.
                    double tolerance = FastMath.max(relativeAccuracy * z.abs(), absoluteAccuracy);
                    if ((z.subtract(oldz)).abs() <= tolerance)
                    {
                        return z;
                    }
                    if (pv.abs() <= functionValueAccuracy)
                    {
                        return z;
                    }

                    // Now pv != 0, calculate the new approximation.
                    Complex G = dv.divide(pv);
                    Complex G2 = G.multiply(G);
                    Complex H = G2.subtract(d2v.divide(pv));
                    Complex delta = n1C.multiply((nC.multiply(H)).subtract(G2));
                    // Choose a denominator larger in magnitude.
                    Complex deltaSqrt = delta.sqrt();
                    Complex dplus = G.add(deltaSqrt);
                    Complex dminus = G.subtract(deltaSqrt);
                    Complex denominator = dplus.abs() > dminus.abs() ? dplus : dminus;
                    // Perturb z if denominator is zero, for instance,
                    // p(x) = x^3 + 1, z = 0.
                    if (denominator.Equals(new Complex(0.0, 0.0)))
                    {
                        z = z.add(new Complex(absoluteAccuracy, absoluteAccuracy));
                        oldz = new Complex(double.PositiveInfinity, double.PositiveInfinity);
                    }
                    else
                    {
                        oldz = z;
                        z = z.subtract(nC.divide(denominator));
                    }
                    outerInstance.IncrementEvaluationCount();
                }
            }
        }
    }

}