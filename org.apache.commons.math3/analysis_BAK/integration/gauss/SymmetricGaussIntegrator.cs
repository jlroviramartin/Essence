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
    /// This class's implements <seealso cref="#integrate(UnivariateFunction) integrate"/>
    /// method assuming that the integral is symmetric about 0.
    /// This allows to reduce numerical errors.
    /// 
    /// @since 3.3
    /// @version $Id: SymmetricGaussIntegrator.java 1509234 2013-08-01 13:48:57Z erans $
    /// </summary>
    public class SymmetricGaussIntegrator : GaussIntegrator
    {
        /// <summary>
        /// Creates an integrator from the given {@code points} and {@code weights}.
        /// The integration interval is defined by the first and last value of
        /// {@code points} which must be sorted in increasing order.
        /// </summary>
        /// <param name="points"> Integration points. </param>
        /// <param name="weights"> Weights of the corresponding integration nodes. </param>
        /// <exception cref="NonMonotonicSequenceException"> if the {@code points} are not
        /// sorted in increasing order. </exception>
        /// <exception cref="DimensionMismatchException"> if points and weights don't have the same length </exception>
        public SymmetricGaussIntegrator(double[] points, double[] weights)
            : base(points, weights)
        {
        }

        /// <summary>
        /// Creates an integrator from the given pair of points (first element of
        /// the pair) and weights (second element of the pair.
        /// </summary>
        /// <param name="pointsAndWeights"> Integration points and corresponding weights. </param>
        /// <exception cref="NonMonotonicSequenceException"> if the {@code points} are not
        /// sorted in increasing order.
        /// </exception>
        /// <seealso cref= #SymmetricGaussIntegrator(double[], double[]) </seealso>
        public SymmetricGaussIntegrator(Tuple<double[], double[]> pointsAndWeights)
            : this(pointsAndWeights.Item1, pointsAndWeights.Item2)
        {
        }

        /// <summary>
        /// {@inheritDoc}
        /// </summary>
        public override double Integrate(UnivariateFunction f)
        {
            int ruleLength = this.NumberOfPoints;

            if (ruleLength == 1)
            {
                return this.GetWeight(0) * f.Value(0d);
            }

            int iMax = ruleLength / 2;
            double s = 0;
            double c = 0;
            for (int i = 0; i < iMax; i++)
            {
                double p = this.GetPoint(i);
                double w = this.GetWeight(i);

                double f1 = f.Value(p);
                double f2 = f.Value(-p);

                double y = w * (f1 + f2) - c;
                double t = s + y;

                c = (t - s) - y;
                s = t;
            }

            if (ruleLength % 2 != 0)
            {
                double w = this.GetWeight(iMax);

                double y = w * f.Value(0d) - c;
                double t = s + y;

                s = t;
            }

            return s;
        }
    }
}