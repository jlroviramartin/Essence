using System;
using org.apache.commons.math3.analysis;

namespace Essence.Math.Double.Curves
{
    public class DelegateUnivariateFunction : UnivariateFunction
    {
        public DelegateUnivariateFunction(Func<double, double> func)
        {
            this.func = func;
        }

        private readonly Func<double, double> func;


        public double value(double d)
        {
            return this.func(d);
        }
    }
}