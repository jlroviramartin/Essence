// Copyright 2017 Jose Luis Rovira Martin
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using Essence.Geometry.Core.Double;
using Essence.Util.Collections;
using Essence.Util.Math.Double;
using org.apache.commons.math3.analysis;
using org.apache.commons.math3.analysis.solvers;

namespace Essence.Maths
{
    public class Solver
    {
        public enum Type
        {
            Brent,
            Bisection,
            Secant,
            RegulaFalsi,
            Ridders,
        }

        private static readonly Vector2d VX = new Vector2d(1.0, 0.0);
        private static readonly Vector2d VY = new Vector2d(0.0, 1.0);
        public const double DEFAULT_ABSOLUTE_ACCURACY = 1E-06;

        public static double Solve(Func<double, double> f,
                                   double min, double max,
                                   Type solverType = Type.Brent,
                                   double absoluteAccuracy = DEFAULT_ABSOLUTE_ACCURACY,
                                   int maxEval = int.MaxValue)
        {
            return GetSolver(solverType, absoluteAccuracy).Solve(maxEval, new DelegateUnivariateFunction(f), min, max);
        }

        public static void SolveTangent(Func<int, Func<double, double>> fx, Func<int, Func<double, double>> fy,
                                        Vector2d dir,
                                        double tmin, double tmax,
                                        int maxzeros, IList<double> zeros,
                                        Type solver = Type.Brent,
                                        IList<double> mins = null, IList<double> maxs = null)
        {
            Func<int, Func<double, double>> nthf =
                !dir.EpsilonEquals(VX)
                    ? (!dir.EpsilonEquals(VY)
                        ? (Func<int, Func<double, double>>)(i =>
                        {
                            Func<double, double> ffx = fx(i + 1);
                            Func<double, double> ffy = fy(i + 1);
                            return (t => dir.X * ffy(t) - dir.Y * ffx(t));
                        })
                        : (i =>
                        {
                            Func<double, double> ffx = fx(i + 1);
                            return (t => -ffx(t));
                        }))
                    : (i =>
                    {
                        Func<double, double> ffy = fy(i + 1);
                        return (t => ffy(t));
                    });
            zeros = zeros ?? new List<double>();
            SolveMulti(nthf, tmin, tmax, maxzeros, zeros, solver);
            if (mins == null || maxs == null)
            {
                return;
            }
            Func<double, double> func = nthf(1);
            foreach (double zero in zeros)
            {
                if (func(zero) < 0)
                {
                    maxs.Add(zero);
                }
                else
                {
                    mins.Add(zero);
                }
            }
        }

        public static void SolveMulti(Func<int, Func<double, double>> nthf,
                                      double xmin, double xmax,
                                      int maxzeros, IList<double> zeros,
                                      Type solver = Type.Brent,
                                      int maxEval = 1000)
        {
            Queue<double> collection = new Queue<double>();
            collection.Enqueue(xmin);
            collection.Enqueue(xmax);
            Func<double, double>[] funcArray = new Func<double, double>[maxzeros + 1];

            for (int index = 0; index <= maxzeros; ++index)
            {
                funcArray[index] = nthf(index);
            }

            for (int index = maxzeros; index >= 1; --index)
            {
                List<double> doubleList = new List<double>();
                Func<double, double> f = funcArray[index];
                double num1 = collection.Dequeue();
                double v = f(num1);
                doubleList.Add(num1);
                while (collection.Count > 0)
                {
                    double max = collection.Dequeue();
                    double num2 = f(max);
                    if (v.EpsilonSign() * num2.EpsilonSign() < 0)
                    {
                        double num3 = Solve(f, num1, max, solver, DEFAULT_ABSOLUTE_ACCURACY, maxEval);
                        doubleList.Add(num3);
                    }
                    if (num2.EpsilonEquals(0))
                    {
                        doubleList.Add(max);
                    }
                    num1 = max;
                    v = num2;
                }
                if (!doubleList[doubleList.Count - 1].EpsilonEquals(num1))
                {
                    doubleList.Add(num1);
                }
                collection.Clear();
                collection.EnqueueAll(doubleList);
            }
            Func<double, double> f1 = nthf(0);
            if (collection.Count > 0)
            {
                double min = collection.Dequeue();
                double num1 = f1(min);

                if (num1.EpsilonEquals(0))
                {
                    zeros.Add(min);
                }

                while (collection.Count > 0)
                {
                    double max = collection.Dequeue();
                    double num2 = f1(max);
                    if (num1.EpsilonSign() * num2.EpsilonSign() < 0)
                    {
                        double num3 = Solve(f1, min, max, solver, DEFAULT_ABSOLUTE_ACCURACY, maxEval);
                        zeros.Add(num3);
                    }

                    if (num2.EpsilonEquals(0))
                    {
                        zeros.Add(max);
                    }

                    min = max;
                    num1 = num2;
                }
            }
            int num = 1;
            while (num < zeros.Count)
            {
                num++;
            }
        }

        public static void Test()
        {
            double min = -4.0;
            double max = 0.0;
            int maxEval = 1000;
            UnivariateFunction f1 = (UnivariateFunction)new DelegateUnivariateFunction((Func<double, double>)(x => (x + 3.0) * (x - 1.0) * (x - 1.0)));
            DifferentiableUnivariateFunction f2 = (DifferentiableUnivariateFunction)new DelegateDifferentiableUnivariateFunction((Func<double, double>)(x => (x + 3.0) * (x - 1.0) * (x - 1.0)), (Func<double, double>)(x => 2.0 * (x - 1.0) * (x + 3.0) + (x - 1.0) * (x - 1.0)));
            new BisectionSolver().Solve(maxEval, f1, min, max);
            new BrentSolver().Solve(maxEval, f1, min, max);
            new RegulaFalsiSolver().Solve(maxEval, f1, min, max);
            new IllinoisSolver().Solve(maxEval, f1, min, max);
            new PegasusSolver().Solve(maxEval, f1, min, max);
            new RiddersSolver().Solve(maxEval, f1, min, max);
            new SecantSolver().Solve(maxEval, f1, min, max);
            new MullerSolver().Solve(maxEval, f1, min, max);
            new MullerSolver2().Solve(maxEval, f1, min, max);
            new BracketingNthOrderBrentSolver().Solve(maxEval, f1, min, max);
            new NewtonSolver().Solve(maxEval, f2, min, max);
        }

        private static UnivariateSolver GetSolver(Type type, double error = DEFAULT_ABSOLUTE_ACCURACY)
        {
            UnivariateSolver univariateSolver;
            switch (type)
            {
                case Type.Brent:
                    univariateSolver = new BrentSolver(error);
                    break;
                case Type.Bisection:
                    univariateSolver = new BisectionSolver(error);
                    break;
                case Type.Secant:
                    univariateSolver = new SecantSolver(error);
                    break;
                case Type.RegulaFalsi:
                    univariateSolver = new RegulaFalsiSolver(error);
                    break;
                case Type.Ridders:
                    univariateSolver = new RiddersSolver(error);
                    break;
                default:
                    throw new IndexOutOfRangeException();
            }
            return univariateSolver;
        }
    }

#if false
    public class Solver
    {
        public enum Type
        {
            BrentSolver,
            BisectionSolver,
            SecantSolver
        }

        public static double Solve(Func<double, double> f,
                                   double min, double max,
                                   Type solverType,
                                   double absoluteAccuracy,
                                   int maxEval)
        {
            UnivariateSolver solver;
            switch (solverType)
            {
                case Type.BrentSolver:
                {
                    solver = new BrentSolver(absoluteAccuracy);
                    break;
                }
                case Type.BisectionSolver:
                {
                    solver = new BisectionSolver(absoluteAccuracy);
                    break;
                }
                case Type.SecantSolver:
                {
                    solver = new SecantSolver(absoluteAccuracy);
                    break;
                }
                default:
                {
                    throw new IndexOutOfRangeException();
                }
            }
            double v = solver.Solve(maxEval, new DelegateUnivariateFunction(f), min, max);
            return v;
        }
    }
#endif
}