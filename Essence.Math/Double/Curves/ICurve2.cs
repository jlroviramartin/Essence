namespace Essence.Math.Double.Curves
{
    public interface ICurve2
    {
        bool IsClosed { get; }

        double TMin { get; }

        double TMax { get; }

        void SetTInterval(double tmin, double tmax);

        #region Position and derivatives

        Vec2d GetPosition(double t);

        Vec2d GetFirstDerivative(double t);

        Vec2d GetSecondDerivative(double t);

        Vec2d GetThirdDerivative(double t);

        #endregion

        #region Differential geometric quantities

        double GetLength(double t0, double t1);

        double TotalLength { get; }

        double GetSpeed(double t);

        double GetCurvature(double t);

        Vec2d GetTangent(double t);

        Vec2d GetLeftNormal(double t);

        void GetFrame(double t, ref Vec2d position, ref Vec2d tangent, ref Vec2d leftNormal);

        #endregion
    }
}