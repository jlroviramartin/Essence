/// Apache Commons Math 3.6.1
using System.Collections.Generic;

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

    //using Pair = org.apache.commons.math3.util.Pair;
    using org.apache.commons.math3.util;
    using DimensionMismatchException = org.apache.commons.math3.exception.DimensionMismatchException;
    using NotStrictlyPositiveException = org.apache.commons.math3.exception.NotStrictlyPositiveException;
    using LocalizedFormats = org.apache.commons.math3.exception.util.LocalizedFormats;
    using Number = System.IConvertible;

    /// <summary>
    /// Base class for rules that determines the integration nodes and their
    /// weights.
    /// Subclasses must implement the <seealso cref="#computeRule(int) computeRule"/> method.
    /// </summary>
    /// @param <T> Type of the number used to represent the points and weights of
    /// the quadrature rules.
    /// 
    /// @since 3.1 </param>
    public abstract class BaseRuleFactory<T> where T : Number
    {
        /// <summary>
        /// List of points and weights, indexed by the order of the rule. </summary>
        private readonly IDictionary<int?, Pair<T[], T[]>> pointsAndWeights = new SortedDictionary<int?, Pair<T[], T[]>>();
        /// <summary>
        /// Cache for double-precision rules. </summary>
        private readonly IDictionary<int?, Pair<double[], double[]>> pointsAndWeightsDouble = new SortedDictionary<int?, Pair<double[], double[]>>();

        /// <summary>
        /// Gets a copy of the quadrature rule with the given number of integration
        /// points.
        /// </summary>
        /// <param name="numberOfPoints"> Number of integration points. </param>
        /// <returns> a copy of the integration rule. </returns>
        /// <exception cref="NotStrictlyPositiveException"> if {@code numberOfPoints < 1}. </exception>
        /// <exception cref="DimensionMismatchException"> if the elements of the rule pair do not
        /// have the same length. </exception>
        public virtual Pair<double[], double[]> GetRule(int numberOfPoints)
        {

            if (numberOfPoints <= 0)
            {
                throw new NotStrictlyPositiveException(LocalizedFormats.NUMBER_OF_POINTS, numberOfPoints);
            }

            // Try to obtain the rule from the cache.
            Pair<double[], double[]> cached = pointsAndWeightsDouble[numberOfPoints];

            if (cached == null)
            {
                // Rule not computed yet.

                // Compute the rule.
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final org.apache.commons.math3.util.Pair<T[], T[]> rule = getRuleInternal(numberOfPoints);
                Pair<T[], T[]> rule = GetRuleInternal(numberOfPoints);
                cached = ConvertToDouble(rule);

                // Cache it.
                pointsAndWeightsDouble[numberOfPoints] = cached;
            }

            // Return a copy.
            return new Pair<double[], double[]>((double[])cached.GetFirst().Clone(), (double[])cached.GetSecond().Clone());
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
        protected internal virtual Pair<T[], T[]> GetRuleInternal(int numberOfPoints)
        {
            lock (this)
            {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final org.apache.commons.math3.util.Pair<T[], T[]> rule = pointsAndWeights.get(numberOfPoints);
                Pair<T[], T[]> rule = pointsAndWeights[numberOfPoints];
                if (rule == null)
                {
                    AddRule(ComputeRule(numberOfPoints));
                    // The rule should be available now.
                    return GetRuleInternal(numberOfPoints);
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
        protected internal virtual void AddRule(Pair<T[], T[]> rule)
        {
            if (rule.GetFirst().Length != rule.GetSecond().Length)
            {
                throw new DimensionMismatchException(rule.GetFirst().Length, rule.GetSecond().Length);
            }

            pointsAndWeights[rule.GetFirst().Length] = rule;
        }

        /// <summary>
        /// Computes the rule for the given order.
        /// </summary>
        /// <param name="numberOfPoints"> Order of the rule to be computed. </param>
        /// <returns> the computed rule. </returns>
        /// <exception cref="DimensionMismatchException"> if the elements of the pair do not
        /// have the same length. </exception>
        protected internal abstract Pair<T[], T[]> ComputeRule(int numberOfPoints);

        /// <summary>
        /// Converts the from the actual {@code Number} type to {@code double}
        /// </summary>
        /// @param <T> Type of the number used to represent the points and
        /// weights of the quadrature rules. </param>
        /// <param name="rule"> Points and weights. </param>
        /// <returns> points and weights as {@code double}s. </returns>
        private static Pair<double[], double[]> ConvertToDouble<T>(Pair<T[], T[]> rule) where T : Number
        {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final T[] pT = rule.getFirst();
            T[] pT = rule.GetFirst();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final T[] wT = rule.getSecond();
            T[] wT = rule.GetSecond();

//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int len = pT.length;
            int len = pT.Length;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] pD = new double[len];
            double[] pD = new double[len];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final double[] wD = new double[len];
            double[] wD = new double[len];

            for (int i = 0; i < len; i++)
            {
                pD[i] = pT[i].ToDouble(null);
                wD[i] = wT[i].ToDouble(null);
            }

            return new Pair<double[], double[]>(pD, wD);
        }
    }

}