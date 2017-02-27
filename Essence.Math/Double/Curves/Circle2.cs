using SysMath = System.Math;

namespace Essence.Math.Double.Curves
{
    public class Circle2 : BaseCircle2
    {
        public Circle2(Vec2d center, double radius)
            : base(center, radius)
        {
        }

        #region Curve2

        public override bool IsClosed
        {
            get { return true; }
        }

        public override double TotalLength
        {
            get { return 2 * SysMath.PI * this.Radius; }
        }

        #endregion

        #region private

        #endregion
    }
}