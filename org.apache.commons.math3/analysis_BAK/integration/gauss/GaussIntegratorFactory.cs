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

namespace org.apache.commons.math3.analysis.integration.gauss
{
    /// <summary>
    /// Class that provides different ways to compute the nodes and weights to be
    /// used by the <seealso cref="GaussIntegrator Gaussian integration rule"/>.
    /// 
    /// @since 3.1
    /// @version $Id: GaussIntegratorFactory.java 1500601 2013-07-08 08:20:26Z luc $
    /// </summary>
    public class GaussIntegratorFactory
    {
        /// <summary>
        /// Generator of Gauss-Legendre integrators. </summary>
        private readonly BaseRuleFactory<double> legendre = new LegendreRuleFactory();

#if false
    /// <summary>
    /// Generator of Gauss-Legendre integrators. </summary>
        private readonly BaseRuleFactory<decimal> legendreHighPrecision = new LegendreHighPrecisionRuleFactory();
#endif

        /// <summary>
        /// Generator of Gauss-Hermite integrators. </summary>
        private readonly BaseRuleFactory<double> hermite = new HermiteRuleFactory();

        /// <summary>
        /// Creates a Gauss-Legendre integrator of the given order.
        /// The call to the
        /// {@link GaussIntegrator#integrate(org.apache.commons.math3.analysis.UnivariateFunction)
        /// integrate} method will perform an integration on the natural interval
        /// {@code [-1 , 1]}.
        /// </summary>
        /// <param name="numberOfPoints"> Order of the integration rule. </param>
        /// <returns> a Gauss-Legendre integrator. </returns>
        public virtual GaussIntegrator Legendre(int numberOfPoints)
        {
            return new GaussIntegrator(GetRule(this.legendre, numberOfPoints));
        }

        /// <summary>
        /// Creates a Gauss-Legendre integrator of the given order.
        /// The call to the
        /// {@link GaussIntegrator#integrate(org.apache.commons.math3.analysis.UnivariateFunction)
        /// integrate} method will perform an integration on the given interval.
        /// </summary>
        /// <param name="numberOfPoints"> Order of the integration rule. </param>
        /// <param name="lowerBound"> Lower bound of the integration interval. </param>
        /// <param name="upperBound"> Upper bound of the integration interval. </param>
        /// <returns> a Gauss-Legendre integrator. </returns>
        /// <exception cref="NotStrictlyPositiveException"> if number of points is not positive </exception>
        public virtual GaussIntegrator Legendre(int numberOfPoints, double lowerBound, double upperBound)
        {
            return new GaussIntegrator(Transform(GetRule(this.legendre, numberOfPoints), lowerBound, upperBound));
        }

#if false
    /// <summary>
    /// Creates a Gauss-Legendre integrator of the given order.
    /// The call to the
    /// {@link GaussIntegrator#integrate(org.apache.commons.math3.analysis.UnivariateFunction)
    /// integrate} method will perform an integration on the natural interval
    /// {@code [-1 , 1]}.
    /// </summary>
    /// <param name="numberOfPoints"> Order of the integration rule. </param>
    /// <returns> a Gauss-Legendre integrator. </returns>
    /// <exception cref="NotStrictlyPositiveException"> if number of points is not positive </exception>
        public virtual GaussIntegrator LegendreHighPrecision(int numberOfPoints)
        {
            return new GaussIntegrator(GetRule(legendreHighPrecision, numberOfPoints));
        }

        /// <summary>
        /// Creates an integrator of the given order, and whose call to the
        /// {@link GaussIntegrator#integrate(org.apache.commons.math3.analysis.UnivariateFunction)
        /// integrate} method will perform an integration on the given interval.
        /// </summary>
        /// <param name="numberOfPoints"> Order of the integration rule. </param>
        /// <param name="lowerBound"> Lower bound of the integration interval. </param>
        /// <param name="upperBound"> Upper bound of the integration interval. </param>
        /// <returns> a Gauss-Legendre integrator. </returns>
        /// <exception cref="NotStrictlyPositiveException"> if number of points is not positive </exception>
        public virtual GaussIntegrator LegendreHighPrecision(int numberOfPoints, double lowerBound, double upperBound)
        {
            return new GaussIntegrator(Transform(GetRule(legendreHighPrecision, numberOfPoints), lowerBound, upperBound));
        }
#endif

        /// <summary>
        /// Creates a Gauss-Hermite integrator of the given order.
        /// The call to the
        /// {@link SymmetricGaussIntegrator#integrate(org.apache.commons.math3.analysis.UnivariateFunction)
        /// integrate} method will perform a weighted integration on the interval
        /// {@code [-&inf;, +&inf;]}: the computed value is the improper integral of
        /// <code>
        ///  e<sup>-x<sup>2</sup></sup> f(x)
        /// </code>
        /// where {@code f(x)} is the function passed to the
        /// {@link SymmetricGaussIntegrator#integrate(org.apache.commons.math3.analysis.UnivariateFunction)
        /// integrate} method.
        /// </summary>
        /// <param name="numberOfPoints"> Order of the integration rule. </param>
        /// <returns> a Gauss-Hermite integrator. </returns>
        public virtual SymmetricGaussIntegrator Hermite(int numberOfPoints)
        {
            return new SymmetricGaussIntegrator(GetRule(this.hermite, numberOfPoints));
        }

        /// <param name="factory"> Integration rule factory. </param>
        /// <param name="numberOfPoints"> Order of the integration rule. </param>
        /// <returns> the integration nodes and weights. </returns>
        /// <exception cref="NotStrictlyPositiveException"> if number of points is not positive </exception>
        /// <exception cref="DimensionMismatchException"> if the elements of the rule pair do not
        /// have the same length. </exception>
        private static Tuple<double[], double[]> GetRule<T1>(BaseRuleFactory<T1> factory, int numberOfPoints) where T1 : IConvertible
        {
            return factory.GetRule(numberOfPoints);
        }

        /// <summary>
        /// Performs a change of variable so that the integration can be performed
        /// on an arbitrary interval {@code [a, b]}.
        /// It is assumed that the natural interval is {@code [-1, 1]}.
        /// </summary>
        /// <param name="rule"> Original points and weights. </param>
        /// <param name="a"> Lower bound of the integration interval. </param>
        /// <param name="b"> Lower bound of the integration interval. </param>
        /// <returns> the points and weights adapted to the new interval. </returns>
        private static Tuple<double[], double[]> Transform(Tuple<double[], double[]> rule, double a, double b)
        {
            double[] points = rule.Item1;
            double[] weights = rule.Item2;

            // Scaling
            double scale = (b - a) / 2;
            double shift = a + scale;

            for (int i = 0; i < points.Length; i++)
            {
                points[i] = points[i] * scale + shift;
                weights[i] *= scale;
            }

            return new Tuple<double[], double[]>(points, weights);
        }
    }
}