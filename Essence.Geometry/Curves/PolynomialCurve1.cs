using Essence.Util.Math.Double;
using java.lang;
using REAL = System.Double;

namespace Essence.Maths.Double.Curves
{
    public class PolynomialCurve1 : SimpleCurve1
    {
        public PolynomialCurve1(Polynomial poly, REAL t0, REAL t1)
        {
            this.poly = poly;

            this.SetTInterval(t0, t1);
        }

        #region private

        private REAL GetT0L(REAL t)
        {
            t = t.Clamp(this.TMin, this.TMax);
            REAL t01 = this.ttransform.Get(t);
            return t01;
        }

        private Polynomial GetPoly(int i)
        {
            switch (i)
            {
                case 0:
                    return this.poly;
                case 1:
                    if (this.poly1 == null)
                    {
                        this.poly1 = this.GetPoly(0).Derivative();
                    }
                    return this.poly1;
                case 2:
                    if (this.poly2 == null)
                    {
                        this.poly2 = this.GetPoly(1).Derivative();
                    }
                    return this.poly2;
                case 3:
                    if (this.poly3 == null)
                    {
                        this.poly3 = this.GetPoly(2).Derivative();
                    }
                    return this.poly3;
                default:
                    throw new IndexOutOfBoundsException();
            }
        }

        private REAL tmin;
        private REAL tmax;

        /// <summary>Transformacion que se aplica sobre el parametro.</summary>
        private Transform1 ttransform;

        private readonly Polynomial poly;
        private Polynomial poly1;
        private Polynomial poly2;
        private Polynomial poly3;

        #endregion

        #region ICurve1

        public override REAL TMin
        {
            get { return this.tmin; }
        }

        public override REAL TMax
        {
            get { return this.tmax; }
        }

        public override void SetTInterval(REAL tmin, REAL tmax)
        {
            this.tmin = tmin;
            this.tmax = tmax;
            this.ttransform = new Transform1(this.tmin, this.tmax, 0, this.tmax - this.tmin);
        }

        public override REAL GetPosition(REAL t)
        {
            REAL tt = this.GetT0L(t);
            return this.GetPoly(0).Evaluate(tt);
        }

        public override REAL GetFirstDerivative(REAL t)
        {
            REAL tt = this.GetT0L(t);
            return this.GetPoly(1).Evaluate(tt);
        }

        public override REAL GetSecondDerivative(REAL t)
        {
            REAL tt = this.GetT0L(t);
            return this.GetPoly(2).Evaluate(tt);
        }

        public override REAL GetThirdDerivative(REAL t)
        {
            REAL tt = this.GetT0L(t);
            return this.GetPoly(3).Evaluate(tt);
        }

        #endregion
    }
}