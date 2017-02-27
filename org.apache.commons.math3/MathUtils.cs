namespace org.apache.commons.math3
{
    internal static class MathUtils
    {
        /// <summary>Error.</summary>
        public const double EPSILON = 1e-09;

        /// <summary>Error al cuadrado (se evita utilizar Math.sqrt).</summary>
        public const double EPSILON_CUAD = EPSILON * EPSILON;

        public static bool EpsilonEquals(this double a, double b, double epsilon = EPSILON)
        {
            //return (SysMath.Abs(b - a) <= epsilon);
            // Resuelve las comparaciones con PositiveInfinity / NegativeInfinity.
            if (b > a)
            {
                return b <= a + epsilon;
            }
            else
            {
                return a <= b + epsilon;
            }
        }
    }
}