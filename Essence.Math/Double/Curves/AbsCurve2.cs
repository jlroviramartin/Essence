using System;
using SysMath = System.Math;
using UnaryFunction = System.Func<double, double>;

namespace Essence.Math.Double.Curves
{
    public abstract class AbsCurve2 : ICurve2
    {
        public Vec2d Point0
        {
            get { return this.GetPosition(this.TMin); }
        }

        public Vec2d Point1
        {
            get { return this.GetPosition(this.TMax); }
        }

        #region ICurve2

        public virtual bool IsClosed
        {
            get { return false; }
        }

        public virtual double TMin
        {
            get { return 0; }
        }

        public virtual double TMax
        {
            get { return 1; }
        }

        public virtual void SetTInterval(double tmin, double tmax)
        {
        }

        public abstract Vec2d GetPosition(double t);

        public virtual Vec2d GetFirstDerivative(double t)
        {
            UnaryFunction fdx = Derivative.Central(tt => this.GetPosition(tt).X, 1, 5);
            UnaryFunction fdy = Derivative.Central(tt => this.GetPosition(tt).Y, 1, 5);
            return new Vec2d(fdx(t), fdy(t));
        }

        public virtual Vec2d GetSecondDerivative(double t)
        {
            UnaryFunction fdx = Derivative.Central(tt => this.GetPosition(tt).X, 2, 5);
            UnaryFunction fdy = Derivative.Central(tt => this.GetPosition(tt).Y, 2, 5);
            return new Vec2d(fdx(t), fdy(t));
        }

        public virtual Vec2d GetThirdDerivative(double t)
        {
            // NOTA: no esta
            UnaryFunction fdx = Derivative.Central(tt => this.GetPosition(tt).X, 3, 5);
            UnaryFunction fdy = Derivative.Central(tt => this.GetPosition(tt).Y, 3, 5);
            return new Vec2d(fdx(t), fdy(t));
        }

        public virtual double GetSpeed(double t)
        {
            return this.GetFirstDerivative(t).Length;
        }

        public virtual double GetLength(double t0, double t1)
        {
            throw new NotImplementedException();
        }

        public virtual double TotalLength
        {
            get { return this.GetLength(this.TMin, this.TMax); }
        }

        public virtual Vec2d GetTangent(double t)
        {
            return this.GetFirstDerivative(t).Norm();
        }

        public virtual Vec2d GetLeftNormal(double t)
        {
            return vecMath.PerpLeft(this.GetTangent(t));
        }

        public virtual void GetFrame(double t, ref Vec2d position, ref Vec2d tangent, ref Vec2d normal)
        {
            position = this.GetPosition(t);
            tangent = this.GetTangent(t);
            normal = vecMath.PerpLeft(tangent);
        }

        public virtual double GetCurvature(double t)
        {
            Vec2d der1 = this.GetFirstDerivative(t);
            double speed2 = der1.Length2;

            if (speed2.EpsilonEquals(0))
            {
                // Curvature is indeterminate, just return 0.
                return 0;
            }

            Vec2d der2 = this.GetSecondDerivative(t);

            double numer = der1.Dot(der2);
            double denom = SysMath.Pow(speed2, 1.5);
            return numer / denom;
        }

        #endregion

        #region private

        private static readonly DoubleMath math = DoubleMath.Instance;
        private static readonly VecMath<double, DoubleMath, Vec2d, Vec2dFactory> vecMath = VecMath<double, DoubleMath, Vec2d, Vec2dFactory>.Instance;

        #endregion
    }
}