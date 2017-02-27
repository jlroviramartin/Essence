using System.Diagnostics.Contracts;
using SysMath = System.Math;

namespace Essence.Math.Double
{
    public static class AngleUtils
    {
        private const double twoPi = 2.0 * SysMath.PI;

        /// <summary>
        /// Comprueba si el angulo indicado esta en el arco indicado.
        /// </summary>
        /// <param name="angle0">Angulo inicial.</param>
        /// <param name="advAngle">Angulo de avance (importa el signo del avance).</param>
        /// <param name="angle">Angulo.</param>
        /// <returns>Indica si el angulo esta dentro del arco indicado.</returns>
        public static bool InArc(double angle0, double advAngle, double angle)
        {
            if (SysMath.Abs(advAngle) >= twoPi)
            {
                return true;
            }

            // Seleccionamos una rama que haga que el angulo del arco sea una funcion continua
            double corte = SysMath.Min(angle0, angle0 + advAngle);

            // Ajustamos el angulo a dicha rama
            angle = EnsureBranch(angle, corte);

            // Como el angulo es continuo, basta comprobar que la distancia al inicio de la rama esta dentro del angulo tendido
            return (angle - corte <= SysMath.Abs(advAngle));
        }

        /// <summary>
        /// Ajusta al intervalo closed [0, 2*PI] o open [0, 2*PI).
        /// </summary>
        public static double Ensure0To2Pi(double rad, bool open = false)
        {
            return EnsureBranch(rad, 0, open);
        }

        /// <summary>
        /// Ajusta al intervalo closed [-PI, PI] o open [-PI, PI).
        /// </summary>
        public static double EnsureMinusPiToPi(double rad, bool open = false)
        {
            return EnsureBranch(rad, -SysMath.PI, open);
        }

        /// <summary>
        /// Ajusta al intervalo closed [cut, cut + 2*PI] o open [cut, cut + 2*PI).
        /// </summary>
        /// <param name="rad">Angulo en radianes.</param>
        /// <param name="cut">Angulo inicial de la rama.</param>
        /// <param name="open">Indica si queremos [corte, corte+2*PI).</param>
        /// <returns>Angulo en radianes entre [corte, corte+2*PI] o [corte, corte+2*PI).</returns>
        public static double EnsureBranch(double rad, double cut, bool open = false)
        {
            double rightEnd = cut + twoPi;

            if (rad < cut || rad > rightEnd)
            {
                rad -= SysMath.Floor((rad - cut) / twoPi) * twoPi;
            }

            // Si es abierto, se trata el extremo derecho.
            if (open)
            {
                if (rad == rightEnd)
                {
                    rad = cut;
                }
                Contract.Assert(rad >= cut && rad < rightEnd);
                return rad;
            }

            Contract.Assert(rad >= cut && rad <= rightEnd);
            return rad;
        }

        /// <summary>
        /// Diferencia entre 2 angulos.
        /// </summary>
        public static double Diff(double a0, double a1)
        {
            //a0 = Ensure0To2Pi(a0, true);
            //a1 = Ensure0To2Pi(a1, true);
            //return SysMath.Abs(a1 - a0);
            return Ensure0To2Pi(a1 - a0, true);
        }
    }
}