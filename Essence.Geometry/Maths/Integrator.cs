using System;
using org.apache.commons.math3.analysis.integration;

namespace Essence.Maths
{
    public class Integrator
    {
        public enum Type
        {
            RombergIntegrator
        }

        public static double Integrate(Func<double, double> f, double t0, double t1, Type type, int maxEval)
        {
            UnivariateIntegrator iintegral;
            switch (type)
            {
                case Type.RombergIntegrator:
                {
                    iintegral = new RombergIntegrator();
                    break;
                }
                default:
                {
                    throw new IndexOutOfRangeException();
                }
            }
            double v = iintegral.Integrate(maxEval, new DelegateUnivariateFunction(f), t0, t1);
            return v;
        }
    }
}