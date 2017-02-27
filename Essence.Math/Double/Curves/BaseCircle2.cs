using SysMath = System.Math;

namespace Essence.Math.Double.Curves
{
    public abstract class BaseCircle2 : AbsCurve2
    {
        public BaseCircle2(Vec2d center, double radius)
        {
            this.center = center;
            this.radius = radius;
        }

        public Vec2d Center
        {
            get { return this.center; }
        }

        public double Radius
        {
            get { return this.radius; }
        }

        #region Curve2

        public override double TMin
        {
            get { return this.tmin; }
        }

        public override double TMax
        {
            get { return this.tmax; }
        }

        public override void SetTInterval(double tmin, double tmax)
        {
            this.tmin = tmin;
            this.tmax = tmax;
            this.ttransform = new Transform1(this.tmin, this.tmax, 0, 2 * SysMath.PI);
        }

        public override Vec2d GetPosition(double t)
        {
            /*
             *  Maxima:
             *  s:      t * m + n;
             *  x:      r*cos(r);
             *  y:      r*sin(r);
             *  z:      1;
             *  result: [ x, y, z ];
             *  
             *  result: [r*cos(r),r*sin(r),1]
             */
            double angle = this.GetAngle(t);
            double x = this.Radius * SysMath.Cos(angle);
            double y = this.Radius * SysMath.Sin(angle);
            return this.Center.Add(new Vec2d(x, y));
        }

        public override Vec2d GetFirstDerivative(double t)
        {
            /*
             *  Maxima:
             *  s:      t * m + n;
             *  x:      r*cos(r);
             *  y:      r*sin(r);
             *  z:      1;
             *  result: diff([ x, y, z ], t, 1);
             *  
             *  result: [-m*r*sin(m*t+n),m*r*cos(m*t+n),0]
             */
            double m = this.ttransform.A;

            double angle = this.GetAngle(t);
            double x = -this.Radius * SysMath.Sin(angle);
            double y = this.Radius * SysMath.Cos(angle);
            return new Vec2d(m * x, m * y);
        }

        public override Vec2d GetSecondDerivative(double t)
        {
            /*
             *  Maxima:
             *  s:      t * m + n;
             *  x:      r*cos(r);
             *  y:      r*sin(r);
             *  z:      1;
             *  result: diff([ x, y, z ], t, 2);
             *  
             *  result: [-m^2*r*cos(m*t+n),-m^2*r*sin(m*t+n),0]
             */
            double m = this.ttransform.A;
            double m2 = m * m;

            double angle = this.GetAngle(t);
            double x = -this.Radius * SysMath.Cos(angle);
            double y = -this.Radius * SysMath.Sin(angle);
            return new Vec2d(m2 * x, m2 * y);
        }

        public override Vec2d GetThirdDerivative(double t)
        {
            /*
             *  Maxima:
             *  s:      t * m + n;
             *  x:      r*cos(r);
             *  y:      r*sin(r);
             *  z:      1;
             *  result: diff([ x, y, z ], t, 3);
             *  
             *  result: [m^3*r*sin(m*t+n),-m^3*r*cos(m*t+n),0]
             */
            double m = this.ttransform.A;
            double m3 = m * m * m;

            double angle = this.GetAngle(t);
            double x = this.Radius * SysMath.Sin(angle);
            double y = -this.Radius * SysMath.Cos(angle);
            return new Vec2d(m3 * x, m3 * y);
        }

        public override double GetLength(double t0, double t1)
        {
            double a0 = this.GetAngle(t0);
            double a1 = this.GetAngle(t1);
            return AngleUtils.Diff(a0, a1) * this.Radius;
        }

        #endregion

        #region protected

        protected double GetAngle(double t)
        {
            t = t.Clamp(this.TMin, this.TMax);
            double a = this.ttransform.Get(t);
            return a;
        }

        #endregion

        #region private

        private readonly Vec2d center;
        private readonly double radius;

        private double tmin;
        private double tmax;

        /// <summary>Transformacion que se aplica sobre el parametro.</summary>
        private Transform1 ttransform;

        #endregion
    }
}