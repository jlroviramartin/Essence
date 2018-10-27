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
using org.apache.commons.math3.exception;

namespace org.apache.commons.math3.analysis.integration.gauss
{
    /// <summary>
    /// Base class for rules that determines the integration nodes and their
    /// weights.
    /// Subclasses must implement the <seealso cref="#computeRule(int) computeRule"/> method.
    /// </summary>
    /// @param <T> Type of the number used to represent the points and weights of
    /// the quadrature rules.
    /// 
    /// @since 3.1
    /// @version $Id: BaseRuleFactory.java 1455194 2013-03-11 15:45:54Z luc $ </param>
    public abstract class BaseRuleFactory<T> where T : IConvertible
    {
        /// <summary>
        /// List of points and weights, indexed by the order of the rule. </summary>
        private readonly IDictionary<int?, Tuple<T[], T[]>> pointsAndWeights = new SortedDictionary<int?, Tuple<T[], T[]>>();

        /// <summary>
        /// Cache for double-precision rules. </summary>
        private readonly IDictionary<int?, Tuple<double[], double[]>> pointsAndWeightsDouble = new SortedDictionary<int?, Tuple<double[], double[]>>();

        /// <summary>
        /// Gets a copy of the quadrature rule with the given number of integration
        /// points.
        /// </summary>
        /// <param name="numberOfPoints"> Number of integration points. </param>
        /// <returns> a copy of the integration rule. </returns>
        /// <exception cref="NotStrictlyPositiveException"> if {@code numberOfPoints < 1}. </exception>
        /// <exception cref="DimensionMismatchException"> if the elements of the rule pair do not
        /// have the same length. </exception>
        public virtual Tuple<double[], double[]> GetRule(int numberOfPoints)
        {
            if (numberOfPoints <= 0)
            {
                throw new NotStrictlyPositiveException("LocalizedFormats.NUMBER_OF_POINTS", numberOfPoints);
            }

            // Try to obtain the rule from the cache.
            Tuple<double[], double[]> cached = this.pointsAndWeightsDouble[numberOfPoints];

            if (cached == null)
            {
                // Rule not computed yet.

                // Compute the rule.
                Tuple<T[], T[]> rule = this.GetRuleInternal(numberOfPoints);
                cached = ConvertToDouble(rule);

                // Cache it.
                this.pointsAndWeightsDouble[numberOfPoints] = cached;
            }

            // Return a copy.
            return Tuple.Create((double[])cached.Item1.Clone(), (double[])cached.Item2.Clone());
        }

        /// <summary>
        /// Gets a rule.
        /// Synchronization ensures that rules will be computed and added to the
        /// cache at most once.
        /// The returned rule is a reference into the cache.
        /// </summary>
        /// <param name="numberOfPoints"> Order of the rule to be retrieved. </param>
        /// <returns> the points and weights corresponding to the given order. </returns>
        /// <exception cref="DimensionMismatchException"> if the elements of the rule pair do not
        /// have the same length. </exception>
        protected internal virtual Tuple<T[], T[]> GetRuleInternal(int numberOfPoints)
        {
            lock (this)
            {
                Tuple<T[], T[]> rule = this.pointsAndWeights[numberOfPoints];
                if (rule == null)
                {
                    this.AddRule(this.ComputeRule(numberOfPoints));
                    // The rule should be available now.
                    return this.GetRuleInternal(numberOfPoints);
                }
                return rule;
            }
        }

        /// <summary>
        /// Stores a rule.
        /// </summary>
        /// <param name="rule"> Rule to be stored. </param>
        /// <exception cref="DimensionMismatchException"> if the elements of the pair do not
        /// have the same length. </exception>
        protected internal virtual void AddRule(Tuple<T[], T[]> rule)
        {
            if (rule.Item1.Length != rule.Item2.Length)
            {
                throw new DimensionMismatchException(rule.Item1.Length, rule.Item2.Length);
            }

            this.pointsAndWeights[rule.Item1.Length] = rule;
        }

        /// <summary>
        /// Computes the rule for the given order.
        /// </summary>
        /// <param name="numberOfPoints"> Order of the rule to be computed. </param>
        /// <returns> the computed rule. </returns>
        /// <exception cref="DimensionMismatchException"> if the elements of the pair do not
        /// have the same length. </exception>
        protected internal abstract Tuple<T[], T[]> ComputeRule(int numberOfPoints);

        /// <summary>
        /// Converts the from the actual {@code Number} type to {@code double}
        /// </summary>
        /// @param <T> Type of the number used to represent the points and
        /// weights of the quadrature rules. </param>
        /// <param name="rule"> Points and weights. </param>
        /// <returns> points and weights as {@code double}s. </returns>
        private static Tuple<double[], double[]> ConvertToDouble<T>(Tuple<T[], T[]> rule) where T : IConvertible
        {
            T[] pT = rule.Item1;
            T[] wT = rule.Item2;

            int len = pT.Length;
            double[] pD = new double[len];
            double[] wD = new double[len];

            for (int i = 0; i < len; i++)
            {
                pD[i] = (double)pT[i].ToDouble(null);
                wD[i] = (double)wT[i].ToDouble(null);
            }

            return Tuple.Create(pD, wD);
        }
    }
}