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
using org.apache.commons.math3.analysis.exception;
using FastMath = System.Math;

namespace org.apache.commons.math3.analysis.integration
{
    /// <summary>
    /// Implements the <a href="http://mathworld.wolfram.com/Legendre-GaussQuadrature.html">
    /// Legendre-Gauss</a> quadrature formula.
    /// <para>
    /// Legendre-Gauss integrators are efficient integrators that can
    /// accurately integrate functions with few function evaluations. A
    /// Legendre-Gauss integrator using an n-points quadrature formula can
    /// integrate 2n-1 degree polynomials exactly.
    /// </para>
    /// <para>
    /// These integrators evaluate the function on n carefully chosen
    /// abscissas in each step interval (mapped to the canonical [-1,1] interval).
    /// The evaluation abscissas are not evenly spaced and none of them are
    /// at the interval endpoints. This implies the function integrated can be
    /// undefined at integration interval endpoints.
    /// </para>
    /// <para>
    /// The evaluation abscissas x<sub>i</sub> are the roots of the degree n
    /// Legendre polynomial. The weights a<sub>i</sub> of the quadrature formula
    /// integrals from -1 to +1 &int; Li<sup>2</sup> where Li (x) =
    /// &prod; (x-x<sub>k</sub>)/(x<sub>i</sub>-x<sub>k</sub>) for k != i.
    /// </para>
    /// <para>
    /// @version $Id: LegendreGaussIntegrator.java 1455194 2013-03-11 15:45:54Z luc $
    /// @since 1.2
    /// </para>
    /// </summary>
    /// @deprecated As of 3.1 (to be removed in 4.0). Please use
    /// <seealso cref="IterativeLegendreGaussIntegrator"/> instead. 
    //[Obsolete("As of 3.1 (to be removed in 4.0). Please use")]
    public class LegendreGaussIntegrator : BaseAbstractUnivariateIntegrator
    {
        /// <summary>
        /// Abscissas for the 2 points method. </summary>
        private static readonly double[] ABSCISSAS_2 = new double[] { -1.0 / FastMath.Sqrt(3.0), 1.0 / FastMath.Sqrt(3.0) };

        /// <summary>
        /// Weights for the 2 points method. </summary>
        private static readonly double[] WEIGHTS_2 = new double[] { 1.0, 1.0 };

        /// <summary>
        /// Abscissas for the 3 points method. </summary>
        private static readonly double[] ABSCISSAS_3 = new double[] { -FastMath.Sqrt(0.6), 0.0, FastMath.Sqrt(0.6) };

        /// <summary>
        /// Weights for the 3 points method. </summary>
        private static readonly double[] WEIGHTS_3 = new double[] { 5.0 / 9.0, 8.0 / 9.0, 5.0 / 9.0 };

        /// <summary>
        /// Abscissas for the 4 points method. </summary>
        private static readonly double[] ABSCISSAS_4 = new double[] { -FastMath.Sqrt((15.0 + 2.0 * FastMath.Sqrt(30.0)) / 35.0), -FastMath.Sqrt((15.0 - 2.0 * FastMath.Sqrt(30.0)) / 35.0), FastMath.Sqrt((15.0 - 2.0 * FastMath.Sqrt(30.0)) / 35.0), FastMath.Sqrt((15.0 + 2.0 * FastMath.Sqrt(30.0)) / 35.0) };

        /// <summary>
        /// Weights for the 4 points method. </summary>
        private static readonly double[] WEIGHTS_4 = new double[] { (90.0 - 5.0 * FastMath.Sqrt(30.0)) / 180.0, (90.0 + 5.0 * FastMath.Sqrt(30.0)) / 180.0, (90.0 + 5.0 * FastMath.Sqrt(30.0)) / 180.0, (90.0 - 5.0 * FastMath.Sqrt(30.0)) / 180.0 };

        /// <summary>
        /// Abscissas for the 5 points method. </summary>
        private static readonly double[] ABSCISSAS_5 = new double[] { -FastMath.Sqrt((35.0 + 2.0 * FastMath.Sqrt(70.0)) / 63.0), -FastMath.Sqrt((35.0 - 2.0 * FastMath.Sqrt(70.0)) / 63.0), 0.0, FastMath.Sqrt((35.0 - 2.0 * FastMath.Sqrt(70.0)) / 63.0), FastMath.Sqrt((35.0 + 2.0 * FastMath.Sqrt(70.0)) / 63.0) };

        /// <summary>
        /// Weights for the 5 points method. </summary>
        private static readonly double[] WEIGHTS_5 = new double[] { (322.0 - 13.0 * FastMath.Sqrt(70.0)) / 900.0, (322.0 + 13.0 * FastMath.Sqrt(70.0)) / 900.0, 128.0 / 225.0, (322.0 + 13.0 * FastMath.Sqrt(70.0)) / 900.0, (322.0 - 13.0 * FastMath.Sqrt(70.0)) / 900.0 };

        /// <summary>
        /// Abscissas for the current method. </summary>
        private readonly double[] abscissas;

        /// <summary>
        /// Weights for the current method. </summary>
        private readonly double[] weights;

        /// <summary>
        /// Build a Legendre-Gauss integrator with given accuracies and iterations counts. </summary>
        /// <param name="n"> number of points desired (must be between 2 and 5 inclusive) </param>
        /// <param name="relativeAccuracy"> relative accuracy of the result </param>
        /// <param name="absoluteAccuracy"> absolute accuracy of the result </param>
        /// <param name="minimalIterationCount"> minimum number of iterations </param>
        /// <param name="maximalIterationCount"> maximum number of iterations </param>
        /// <exception cref="MathIllegalArgumentException"> if number of points is out of [2; 5] </exception>
        /// <exception cref="NotStrictlyPositiveException"> if minimal number of iterations
        /// is not strictly positive </exception>
        /// <exception cref="NumberIsTooSmallException"> if maximal number of iterations
        /// is lesser than or equal to the minimal number of iterations </exception>
        public LegendreGaussIntegrator(int n, double relativeAccuracy, double absoluteAccuracy, int minimalIterationCount, int maximalIterationCount)
            : base(relativeAccuracy, absoluteAccuracy, minimalIterationCount, maximalIterationCount)
        {
            switch (n)
            {
                case 2:
                    this.abscissas = ABSCISSAS_2;
                    this.weights = WEIGHTS_2;
                    break;
                case 3:
                    this.abscissas = ABSCISSAS_3;
                    this.weights = WEIGHTS_3;
                    break;
                case 4:
                    this.abscissas = ABSCISSAS_4;
                    this.weights = WEIGHTS_4;
                    break;
                case 5:
                    this.abscissas = ABSCISSAS_5;
                    this.weights = WEIGHTS_5;
                    break;
                default:
                    throw new MathIllegalArgumentException("LocalizedFormats.N_POINTS_GAUSS_LEGENDRE_INTEGRATOR_NOT_SUPPORTED", n, 2, 5);
            }
        }

        /// <summary>
        /// Build a Legendre-Gauss integrator with given accuracies. </summary>
        /// <param name="n"> number of points desired (must be between 2 and 5 inclusive) </param>
        /// <param name="relativeAccuracy"> relative accuracy of the result </param>
        /// <param name="absoluteAccuracy"> absolute accuracy of the result </param>
        /// <exception cref="MathIllegalArgumentException"> if number of points is out of [2; 5] </exception>
        public LegendreGaussIntegrator(int n, double relativeAccuracy, double absoluteAccuracy)
            : this(n, relativeAccuracy, absoluteAccuracy, DEFAULT_MIN_ITERATIONS_COUNT, DEFAULT_MAX_ITERATIONS_COUNT)
        {
        }

        /// <summary>
        /// Build a Legendre-Gauss integrator with given iteration counts. </summary>
        /// <param name="n"> number of points desired (must be between 2 and 5 inclusive) </param>
        /// <param name="minimalIterationCount"> minimum number of iterations </param>
        /// <param name="maximalIterationCount"> maximum number of iterations </param>
        /// <exception cref="MathIllegalArgumentException"> if number of points is out of [2; 5] </exception>
        /// <exception cref="NotStrictlyPositiveException"> if minimal number of iterations
        /// is not strictly positive </exception>
        /// <exception cref="NumberIsTooSmallException"> if maximal number of iterations
        /// is lesser than or equal to the minimal number of iterations </exception>
        public LegendreGaussIntegrator(int n, int minimalIterationCount, int maximalIterationCount)
            : this(n, DEFAULT_RELATIVE_ACCURACY, DEFAULT_ABSOLUTE_ACCURACY, minimalIterationCount, maximalIterationCount)
        {
        }

        /// <summary>
        /// {@inheritDoc} </summary>
        protected internal override double DoIntegrate()
        {
            // compute first estimate with a single step
            double oldt = this.Stage(1);

            int n = 2;
            while (true)
            {
                // improve integral with a larger number of steps
                double t = this.Stage(n);

                // estimate error
                double delta = FastMath.Abs(t - oldt);
                double limit = FastMath.Max(this.AbsoluteAccuracy, this.RelativeAccuracy * (FastMath.Abs(oldt) + FastMath.Abs(t)) * 0.5);

                // check convergence
                if ((this.iterations.Count + 1 >= this.MinimalIterationCount) && (delta <= limit))
                {
                    return t;
                }

                // prepare next iteration
                double ratio = FastMath.Min(4, FastMath.Pow(delta / limit, 0.5 / this.abscissas.Length));
                n = FastMath.Max((int)(ratio * n), n + 1);
                oldt = t;
                this.iterations.IncrementCount();
            }
        }

        /// <summary>
        /// Compute the n-th stage integral. </summary>
        /// <param name="n"> number of steps </param>
        /// <returns> the value of n-th stage integral </returns>
        /// <exception cref="TooManyEvaluationsException"> if the maximum number of evaluations
        /// is exceeded. </exception>
        private double Stage(int n)
        {
            // set up the step for the current stage
            double step = (this.Max - this.Min) / n;
            double halfStep = step / 2.0;

            // integrate over all elementary steps
            double midPoint = this.Min + halfStep;
            double sum = 0.0;
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < this.abscissas.Length; ++j)
                {
                    sum += this.weights[j] * this.ComputeObjectiveValue(midPoint + halfStep * this.abscissas[j]);
                }
                midPoint += step;
            }

            return halfStep * sum;
        }
    }
}