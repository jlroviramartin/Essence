using System.Diagnostics.Contracts;
using Essence.Math.Double.Distances;
using SysMath = System.Math;

namespace Essence.Math.Double.Curves
{
    public class CircleArc2 : BaseCircle2
    {
        /// <summary>
        ///     Crea un arco de circunferencia:
        ///     Si angle2 > angle1 el arco es CCW.
        ///     Si angle2 &lt; angle1 el arco es CW.
        /// </summary>
        public CircleArc2(Vec2d center, double radius, double angle1, double angle2)
            : base(center, radius)
        {
            Contract.Assert(angle1 >= 0 && angle1 < 2 * SysMath.PI);
            Contract.Assert(SysMath.Abs(angle2 - angle1) <= 2 * SysMath.PI);

            this.angle1 = angle1;
            this.angle2 = angle2;

            this.SetTInterval(0, this.TotalLength);
        }

        /// <summary>
        ///     Initial angle.
        ///     <![CDATA[ Angle1 in [ 0, 2 * PI ) ]]>
        /// </summary>
        public double Angle1
        {
            get { return this.angle1; }
        }

        /// <summary>
        ///     Initial angle.
        ///     <![CDATA[ Angle2 in [ -2 * PI, 4 * PI ) ó Abs(Angle2 - Angle1) <= 2 * PI]]>
        /// </summary>
        public double Angle2
        {
            get { return this.angle2; }
        }

        /// <summary>
        ///     Advance angle.
        ///     <![CDATA[ AdvAngle in [ -2 * PI, 2 * PI ] ]]>
        /// </summary>
        public double AdvAngle
        {
            get { return this.angle2 - this.angle1; }
        }

        public double Project(Vec2d punto)
        {
            double distCuad;
            return this.Project(punto, out distCuad);
        }

        public double Project(Vec2d punto, out double d2)
        {
            DistPointCircleArc2 distance = new DistPointCircleArc2()
            {
                Point = punto, Arc = this
            };

            // Se calcula el punto mas cercano.
            d2 = distance.CalcDistance2();

            double ca = distance.ClosestAngle;
            double ai = this.Angle1;
            double aa = this.AdvAngle;

            ai = AngleUtils.Ensure0To2Pi(ai);

            // Se comprueban los valores.
            Contract.Assert(ca.Between(0, 2 * SysMath.PI));
            Contract.Assert(ai.Between(0, 2 * SysMath.PI));
            Contract.Assert(aa.Between(-2 * SysMath.PI, 2 * SysMath.PI));

            if (aa >= 0)
            {
                if (ca < ai)
                {
                    ca += 2 * SysMath.PI;
                    Contract.Assert(ca >= ai);
                }
            }
            else
            {
                if (ca > ai)
                {
                    ca -= 2 * SysMath.PI;
                    Contract.Assert(ca <= ai);
                }
            }

            // Se calcula la estacion.
            return ((ca - ai) / aa);
        }

        #region ICurve2

        public override void SetTInterval(double tmin, double tmax)
        {
            this.tmin = tmin;
            this.tmax = tmax;
            this.ttransform = new Transform1(this.TMin, this.TMax, this.Angle1, this.Angle2);
        }

        public override double TotalLength
        {
            get { return AngleUtils.Diff(this.Angle1, this.Angle2) * this.Radius; }
        }

        #endregion

        #region private

        private readonly double angle1;
        private readonly double angle2;

        #endregion
    }
}