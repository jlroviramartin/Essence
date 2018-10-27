/// Apache Commons Math 3.6.1
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
namespace org.apache.commons.math3.analysis.integration.gauss
{

    using DimensionMismatchException = org.apache.commons.math3.exception.DimensionMismatchException;
    using NotStrictlyPositiveException = org.apache.commons.math3.exception.NotStrictlyPositiveException;
    //using Pair = org.apache.commons.math3.util.Pair;
    using org.apache.commons.math3.util;
    using Number = System.IConvertible;

    /// <summary>
    /// Class that provides different ways to compute the nodes and weights to be
    /// used by the <seealso cref="GaussIntegrator Gaussian integration rule"/>.
    /// 
    /// @since 3.1
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
            return new GaussIntegrator(GetRule(legendre, numberOfPoints));
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
            return new GaussIntegrator(Transform(GetRule(legendre, numberOfPoints), lowerBound, upperBound));
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
        /// \([-\infty, +\infty]\): the computed value is the improper integral of
        /// \(e^{-x^2}f(x)\)
        /// where \(f(x)\) is the function passed to the
        /// {@link SymmetricGaussIntegrator#integrate(org.apache.commons.math3.analysis.UnivariateFunction)
        /// integrate} method.
        /// </summary>
        /// <param name="numberOfPoints"> Order of the integration rule. </param>
        /// <returns> a Gauss-Hermite integrator. </returns>
        public virtual SymmetricGaussIntegrator Hermite(int numberOfPoints)
        {
            return new SymmetricGaussIntegrator(GetRule(hermite, numberOfPoints));
        }

        /// <param name="factory"> Integration rule factory. </param>
        /// <param name="numberOfPoints"> Order of the integration rule. </param>
        /// <returns> the integration nodes and weights. </returns>
        /// <exception cref="NotStrictlyPositiveException"> if number of points is not positive </exception>
        /// <exception cref="DimensionMismatchException"> if the elements of the rule pair do not
        /// have the same length. </exception>
        private static Pair<double[], double[]> GetRule<T1>(BaseRuleFactory<T1> factory, int numberOfPoints) where T1 : Number
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
        private static Pair<double[], double[]> Transform(Pair<double[], double[]> rule, double a, double b)
        {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] points = rule.getFirst();
            double[] points = rule.GetFirst();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] weights = rule.getSecond();
            double[] weights = rule.GetSecond();

            // Scaling
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double scale = (b - a) / 2;
            double scale = (b - a) / 2;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double shift = a + scale;
            double shift = a + scale;

            for (int i = 0; i < points.Length; i++)
            {
                points[i] = points[i] * scale + shift;
                weights[i] *= scale;
            }

            return new Pair<double[], double[]>(points, weights);
        }
    }

}