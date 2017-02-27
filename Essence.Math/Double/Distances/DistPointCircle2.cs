using Essence.Math.Double.Curves;

namespace Essence.Math.Double.Distances
{
    public sealed class DistPointCircle2
    {
        public DistPointCircle2()
        {
        }

        public Vec2d Point { get; set; }
        public Circle2 Circle { get; set; }
        public bool Solid { get; set; }

        public Vec2d ClosestPoint { get; private set; }

        public double ClosestAngle
        {
            get { return AngleUtils.Ensure0To2Pi(vecMath.Angle(this.ClosestPoint.Sub(this.Circle.Center))); }
        }

        public double CalcDistance()
        {
            double radio = System.Math.Abs(this.Circle.Radius);

            Vec2d diff = this.Point.Sub(this.Circle.Center);
            double len = diff.Length;

            if (this.Solid && (len < radio))
            {
                this.ClosestPoint = this.Point;
                return 0;
            }

            if (len.EpsilonEquals(0))
            {
                // Justo en el centro del circulo..
                this.ClosestPoint = this.Circle.GetPosition(0);
                return radio;
            }

            this.ClosestPoint = this.Circle.Center.Add(diff.Div(len).Mul(radio));
            return System.Math.Abs(len - radio);
        }

        private static readonly VecMath<double, DoubleMath, Vec2d, Vec2dFactory> vecMath = new VecMath<double, DoubleMath, Vec2d, Vec2dFactory>();
    }
}