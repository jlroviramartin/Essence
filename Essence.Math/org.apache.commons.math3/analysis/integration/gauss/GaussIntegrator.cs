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
using org.apache.commons.math3.util;

namespace org.apache.commons.math3.analysis.integration.gauss
{
    /// <summary>
    /// Class that implements the Gaussian rule for
    /// <seealso cref="#integrate(UnivariateFunction) integrating"/> a weighted
    /// function.
    /// 
    /// @since 3.1
    /// @version $Id: GaussIntegrator.java 1500603 2013-07-08 08:31:49Z luc $
    /// </summary>
    public class GaussIntegrator
    {
        /// <summary>
        /// Nodes. </summary>
        private readonly double[] points;

        /// <summary>
        /// Nodes weights. </summary>
        private readonly double[] weights;

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
        public GaussIntegrator(double[] points, double[] weights)
        {
            if (points.Length != weights.Length)
            {
                throw new DimensionMismatchException(points.Length, weights.Length);
            }

            MathArrays.CheckOrder(points, MathArrays.OrderDirection.INCREASING, true, true);

            this.points = (double[])points.Clone();
            this.weights = (double[])weights.Clone();
        }

        /// <summary>
        /// Creates an integrator from the given pair of points (first element of
        /// the pair) and weights (second element of the pair.
        /// </summary>
        /// <param name="pointsAndWeights"> Integration points and corresponding weights. </param>
        /// <exception cref="NonMonotonicSequenceException"> if the {@code points} are not
        /// sorted in increasing order.
        /// </exception>
        /// <seealso cref= #GaussIntegrator(double[], double[]) </seealso>
        public GaussIntegrator(Tuple<double[], double[]> pointsAndWeights)
            : this(pointsAndWeights.Item1, pointsAndWeights.Item2)
        {
        }

        /// <summary>
        /// Returns an estimate of the integral of {@code f(x) * w(x)},
        /// where {@code w} is a weight function that depends on the actual
        /// flavor of the Gauss integration scheme.
        /// The algorithm uses the points and associated weights, as passed
        /// to the <seealso cref="#GaussIntegrator(double[],double[]) constructor"/>.
        /// </summary>
        /// <param name="f"> Function to integrate. </param>
        /// <returns> the integral of the weighted function. </returns>
        public virtual double Integrate(UnivariateFunction f)
        {
            double s = 0;
            double c = 0;
            for (int i = 0; i < this.points.Length; i++)
            {
                double x = this.points[i];
                double w = this.weights[i];
                double y = w * f.Value(x) - c;
                double t = s + y;
                c = (t - s) - y;
                s = t;
            }
            return s;
        }

        /// <returns> the order of the integration rule (the number of integration
        /// points). </returns>
        public virtual int NumberOfPoints
        {
            get { return this.points.Length; }
        }

        /// <summary>
        /// Gets the integration point at the given index.
        /// The index must be in the valid range but no check is performed. </summary>
        /// <param name="index"> index of the integration point </param>
        /// <returns> the integration point. </returns>
        public virtual double GetPoint(int index)
        {
            return this.points[index];
        }

        /// <summary>
        /// Gets the weight of the integration point at the given index.
        /// The index must be in the valid range but no check is performed. </summary>
        /// <param name="index"> index of the integration point </param>
        /// <returns> the weight. </returns>
        public virtual double GetWeight(int index)
        {
            return this.weights[index];
        }
    }
}