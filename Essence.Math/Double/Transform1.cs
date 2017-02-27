namespace Essence.Math.Double
{
    /// <summary>
    /// Representa una transformacion sobre un parametro del tipo t * a + b;
    /// </summary>
    public struct Transform1
    {
        /// <summary>
        /// Transformacion: t * a + b.
        /// </summary>
        public Transform1(double a = 1, double b = 0)
        {
            this.A = a;
            this.B = b;
        }

        /// <summary>
        /// Transformacion de [a0, a1] a [b0, b1].
        ///   b0 + (b1 - b0) * (t - a0) / (a1 - a0)
        ///   b0 + t * (b1 - b0) / (a1 - a0) - a0 *(b1 - b0) / (a1 - a0)
        ///   t * [ (b1 - b0) / (a1 - a0) ] + [ b0 - a0 *(b1 - b0) / (a1 - a0) ]
        ///   a = (b1 - b0) / (a1 - a0)
        ///   b = b0 - a0 *(b1 - b0) / (a1 - a1) = b0 - a0 * a
        /// </summary>
        public Transform1(double a0, double a1, double b0, double b1)
        {
            this.A = (b1 - b0) / (a1 - a0);
            this.B = b0 - a0 * this.A;
        }

        public double Get(double t)
        {
            return t * this.A + this.B;
        }

        public double GetInverse(double s)
        {
            return (s - this.B) / this.A;
        }

        public readonly double A;
        public readonly double B;
    }
}