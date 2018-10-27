using System;
using Essence.Maths.Double;
using Essence.Maths.Double.Curves;
using org.apache.commons.math3.analysis;
using org.apache.commons.math3.analysis.solvers;

namespace Essence.Maths
{
    public class Solver
    {
        public enum Type
        {
            BrentSolver,
            BisectionSolver,
            SecantSolver
        }

        public static double Solve(Func<double, double> f, double min, double max, Type solverType, double absoluteAccuracy, int maxEval)
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
}