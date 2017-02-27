using Essence.Math.Double.Curves;

namespace Essence.Math.Double.Distances
{
    public sealed class DistPointCircleArc2
    {
        public DistPointCircleArc2()
        {
        }

        public Vec2d Point { get; set; }
        public CircleArc2 Arc { get; set; }

        public Vec2d ClosestPoint { get; private set; }

        public double ClosestAngle { get; private set; }

        public double CalcDistance()
        {
            return System.Math.Sqrt(this.CalcDistance2());
        }

        public double CalcDistance2()
        {
            // Se calcula la distancia al circulo.
            Circle2 circulo = new Circle2(this.Arc.Center, this.Arc.Radius);
            DistPointCircle2 dist = new DistPointCircle2()
            {
                Point = this.Point,
                Circle = circulo
            };
            double d = dist.CalcDistance();
            Vec2d p = dist.ClosestPoint;
            double ca = AngleUtils.Ensure0To2Pi(dist.ClosestAngle); // [0, 2PI)

            // Se comprueba si el punto cae dentro del arco.
            double ai = this.Arc.Angle0;
            double aa = this.Arc.AdvAngle;
            if (AngleUtils.InArc(ai, aa, ca))
            {
                this.ClosestPoint = p;
                this.ClosestAngle = ca;
                return d * d;
            }

            // Se prueban los extremos.
            double d2_0 = vecMath.Distance2(this.Arc.Point0, this.Point);
            double d2_1 = vecMath.Distance2(this.Arc.Point1, this.Point);
            if (d2_0 <= d2_1)
            {
                this.ClosestPoint = this.Arc.Point0;
                this.ClosestAngle = AngleUtils.Ensure0To2Pi(this.Arc.Angle0, true);
                return d2_0;
            }
            else
            {
                this.ClosestPoint = this.Arc.Point1;
                this.ClosestAngle = AngleUtils.Ensure0To2Pi(this.Arc.Angle1, true);
                return d2_1;
            }
        }

        private static readonly VecMath<double, DoubleMath, Vec2d, Vec2dFactory> vecMath = new VecMath<double, DoubleMath, Vec2d, Vec2dFactory>();
    }
}