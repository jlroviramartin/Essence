namespace Essence.Math.Int
{
    public static class MathUtils
    {
        public static int Clamp(this int v, int a, int b)
        {
            return (v <= a) ? a : ((v >= b) ? b : v);
        }
    }
}