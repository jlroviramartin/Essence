using System.Diagnostics.Contracts;
using Essence.Math.Double.Distances;
using SysMath = System.Math;

namespace Essence.Math.Double.Curves
{
    public class CircleArc2 : BaseCircle2
    {
        public CircleArc2(Vec2d center, double radius, double angle0, double angle1)
            : base(center, radius)
        {
            Contract.Assert(angle0 >= 0 && angle0 < 2 * SysMath.PI);
            Contract.Assert(SysMath.Abs(angle1 - angle0) <= 2 * SysMath.PI);

            this.angle0 = angle0;
            this.angle1 = angle1;

            this.SetTInterval(0, this.TotalLength);
        }

        /// <summary>
        /// Initial angle.
        /// <![CDATA[ Angle0 in [ 0, 2 * PI ) ]]>
        /// </summary>
        public double Angle0
        {
            get { return this.angle0; }
        }

        public double Angle1
        {
            get { return this.angle1; }
        }

        /// <summary>
        /// Advance angle.
        /// <![CDATA[ AdvAngle in [ -2 * PI, 2 * PI ] ]]>
        /// </summary>
        public double AdvAngle
        {
            get { return this.angle1 - this.angle0; }
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
            double ai = this.Angle0;
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

        public override double TotalLength
        {
            get { return AngleUtils.Diff(this.Angle0, this.Angle1) * this.Radius; }
        }

        #endregion

        #region private

        private readonly double angle0;
        private readonly double angle1;

        #endregion
    }
}