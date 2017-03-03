using System;
using SysMath = System.Math;

namespace Essence.Math.Double.Curves
{
    public class CircleArc2Utils
    {
        /// <summary>
        /// Crea un arco indicando el angulo inicial y el angulo de avance.
        /// El angulo inicial puede ser positivo o negativo e indica el punto donde empieza el
        /// arco.
        /// El angulo de avance indica cuanto avanza el arco.
        ///     Si es positivo, avanza en sentido contrario a las agujas del reloj.
        ///     Si es negativo, avanza en sentido de las agujas del reloj.
        /// </summary>
        /// <param name="center">Centro.</param>
        /// <param name="radius">Radio.</param>
        /// <param name="anguloInicial">Angulo inicial del arco (radianes).</param>
        /// <param name="anguloAvance">Angulo de avance del arco (radianes).</param>
        /// <returns></returns>
        public static CircleArc2 NewArcWithAdvance(Vec2d center, double radius, double anguloInicial, double anguloAvance)
        {
            return new CircleArc2(center, radius, anguloInicial, anguloInicial + anguloAvance);
        }

        /// <summary>
        /// Crea un arco de circunferencia indicando puntoInicial, puntoFinal, centro, radio y sentido de giro.
        /// </summary>
        public static CircleArc2 NewArc(Vec2d pt0, Vec2d pt1, Vec2d center, double radius, bool sentidoGiroHorario)
        {
            // NOTA: si el radio es muy grande, el error que se produce es grande!
            //Constraints.RequiresD((punto1 - centro).Longitud.EpsilonEquals(radio, 1));
            //Constraints.RequiresD((punto2 - centro).Longitud.EpsilonEquals(radio, 1));

            double a1 = vecMath.Angle(pt0.Sub(center));
            double a2 = vecMath.Angle(pt1.Sub(center));

            a1 = AngleUtils.Ensure0To2Pi(a1);
            a2 = AngleUtils.Ensure0To2Pi(a2);

            return !sentidoGiroHorario
                       ? new CircleArc2(center, radius, a1, AngleUtils.EnsureBranch(a2, a1))
                       : new CircleArc2(center, radius, AngleUtils.EnsureBranch(a1, a2), a2);
        }

        /// <summary>
        /// Crea un arco de circunferencia indicando dos puntos y un radio. El arco creado es siempre el que
        /// no contiene el extremo positivo del eje X. 
        /// Cuidado con los radios negativos!!!
        /// </summary>
        public static CircleArc2 TwoPointsRadius(Vec2d pt0, Vec2d pt1, double radius)
        {
            Vec2d centro = EvaluateCenter(pt0, pt1, radius);

            /*Angulo a1 = (punto1 - centro).AnguloV2;
            Angulo a2 = (punto2 - centro).AnguloV2;
            return new CircleArc2(centro, radio, radio, a1.Radianes0A2PI, a2.Radianes0A2PI);*/

            return NewArc(pt0, pt1, centro, SysMath.Abs(radius), radius > 0);
        }

#if false
/// <summary>
/// Numero de puntos para que tenga la precision indicada.
/// </summary>
/// <param name="precision">Precision.</param>
/// <returns>Numero de puntos.</returns>
        public int CalcularNumPuntos(double precision)
        {
            return Utilidades.CalcularNumPuntos(this.radioX, this.radioY,
                                                this.anguloInicial, this.anguloFinal, precision);
        }
#endif

        /// <summary>
        /// Obtiene el centro de la circunferencia que pasa por los 2 puntos, con el radio indicado.
        /// <c><![CDATA[
        ///            _                  _
        ///         _ /                    \c
        ///       _/ O pf                   O
        ///     _/  /                      / \
        ///    /  _/             radio - _/  |
        ///   /   /|  radio +     <--    /|  |
        ///   |  /      -->             /    /
        ///   / /                      /   _/
        ///   |/                      /  _/
        ///   O pi                   O__/
        ///    \                   _/
        /// ]]></c>
        /// <![CDATA[
        ///  p1   a/2  pm   a/2   p2
        ///  x---------+-------->x
        ///   \        |        /
        ///    \       |       /
        ///     \      |      /
        ///      \     |b    /
        ///       \    |    / r
        ///        \   |   /
        ///         \  |  /
        ///          \ | /
        ///           \|/
        ///            pc
        /// ]]>
        /// Teniendo como dato p1, p2 y r, tenemos que obtener el centro del circulo que pasa por
        /// p1 y p2, con radio r.
        /// 
        /// Con el vector 1-2 obtenemos la distancia a.
        /// b es calculado mediante la f�rmula b = raizcua(r * r + a/2 * a/2).
        /// Creamos un vector perpendicular al 1-2 a una distacion a/2 desde p1 y obtenemos
        /// el punto central de la circunferencia.
        /// Con este dato y conociendo el radio ya simplemente calculamos la esquina del rectangulo.
        /// 
        /// Si el radio es positivo, avanza en sentido contrario a las agujas del reloj.
        /// Si el radio es negativo, avanza en sentido de las agujas del reloj.
        /// <![CDATA[
        ///                   + p2
        ///                  /|\
        ///                   |
        ///  /____            |            ____\
        ///  \    \___        |        ___/    /
        ///           \__     |     __/
        ///              \_   |   _/
        ///   radio -      \  |  /   radio +
        ///   (giro         \ | /    (giro horario)
        ///     antihorario) \|/
        ///                   |
        ///                   + p1
        /// ]]>
        /// </summary>
        /// <exception cref="CalculoImposibleException">Indica que no se puede calcular el
        /// centro.</exception>
        public static Vec2d EvaluateCenter(Vec2d pt0, Vec2d pt1, double radius)
        {
            double a = vecMath.Length(pt0, pt1);

            // Punto medio.
            Vec2d pm = vecMath.Interpolate(pt0, pt1, 0.5);

            // Se tratan radios erroneos que no permitan generar un circunferencia.
            double v = radius * radius - a * a / 4;
            if (v.EpsilonEquals(0))
            {
                return pm;
            }
            if (v < 0)
            {
                throw new Exception("CalculoImposible: Radio erroneo.");
            }

            double b = SysMath.Sqrt(v);

            Vec2d vectorm1 = vecMath.PerpRight(pt1.Sub(pt0).Norm()).Mul(b);

            if (radius > 0) // Derecha
            {
                return pm.Add(vectorm1);
            }
            else // Izquierda
            {
                return pm.Sub(vectorm1);
            }
        }

        /// <summary>
        /// Similar a <c>ObtenerCentro</c> pero en este caso devuelve los dos centros, el
        /// negativo y el positivo.
        /// </summary>
        /// <exception cref="CalculoImposibleException">Indica que no se puede calcular el
        /// radio.</exception>
        public static Vec2d[] EvaluateTwoCenters(Vec2d pt0, Vec2d pt1, double radius)
        {
            double a = vecMath.Length(pt0, pt1);

            // Punto medio.
            Vec2d pm = vecMath.Interpolate(pt0, pt1, 0.5);

            // Se tratan radios erroneos que no permitan generar un circunferencia.
            double v = radius * radius - a * a / 4;
            if (v.EpsilonEquals(0))
            {
                return new[] { pm, pm };
            }
            if (v < 0)
            {
                throw new Exception("CalculoImposible: Radio err�neo.");
            }

            double b = SysMath.Sqrt(v);

            Vec2d vectorm1 = vecMath.PerpRight(pt1.Sub(pt0).Norm()).Mul(b);

            if (radius > 0)
            {
                return new[] { pm.Add(vectorm1), pm.Sub(vectorm1) };
            }
            else
            {
                return new[] { pm.Sub(vectorm1), pm.Add(vectorm1) };
            }
        }

#if false
/// <summary>
/// Obtiene el recubrimiento de la circunferencia formada por 2 punto + radio.
/// </summary>
        public static AABoundingBox2 GetRecubrimiento(Vec2d punto1, Vec2d punto2, double radio)
        {
            double r = SysMath.Abs(radio);

            // Calcula el centro.
            Vec2d c = ObtenerCentro(punto1, punto2, radio);

            // Angulo inicial.
            double ai = punto1.Sub(c).AnguloV2.Radianes0A2PI;
            ai %= (2 * SysMath.PI);
            if (ai < 0)
            {
                ai += (2 * SysMath.PI);
            }

            // Angulo final.
            double af = punto2.Sub(c).AnguloV2.Radianes0A2PI;
            af %= (2 * SysMath.PI);
            if (af < 0)
            {
                af += (2 * SysMath.PI);
            }

            // Angulo de avance.
            double aa = af - ai;
            if (radio > 0)
            {
                // Giro a la derecha -> negativo.
                if (aa > 0)
                {
                    aa -= (2 * SysMath.PI);
                }
            }
            else
            {
                // Giro a la izquierda -> positivo.
                if (aa < 0)
                {
                    aa += (2 * SysMath.PI);
                }
            }

            return GetRecubrimiento(c, r, r, ai, aa);
        }
#endif

        // NOTA: no tiene sentido que este metodo pertenezca a esta clase. Buscar alternativas.

        /// <summary>
        /// Indica si los puntos estan alineados.
        /// </summary>
        /// <param name="pt0">Punto 1.</param>
        /// <param name="pt1">Punto 2.</param>
        /// <param name="pt2">Punto 3.</param>
        /// <returns>Indica si los puntos estan al
        /// ineados.</returns>
        public static bool AreAligned(Vec2d pt0, Vec2d pt1, Vec2d pt2)
        {
            Vec2d v13 = pt2.Sub(pt0);
            Vec2d v12 = pt1.Sub(pt0);
            double area = v13.Cross(v12) / 2;
            return area.EpsilonEquals(0);
        }

        /// <summary>
        /// Arco que pasa por los puntos <c>pinicial</c>, <c>ppaso</c>, <c>pfinal</c>.
        /// </summary>
        /// <exception cref="PuntosAlineadosException">Indica que los puntos estan
        /// alineados.</exception>
        public static CircleArc2 ThreePoints(Vec2d pinicial, Vec2d ppaso, Vec2d pfinal)
        {
            Vec2d vif = pfinal.Sub(pinicial);
            Vec2d vip = ppaso.Sub(pinicial);
            Vec2d vfp = ppaso.Sub(pfinal);

            double area = vif.Cross(vip) / 2;
            if (area.EpsilonEquals(0))
            {
                throw new Exception("PuntosAlineados");
            }

            double u = vif.Dot(vif);
            double v = vip.Dot(vip);
            double w = vfp.Dot(vfp);

            double radio = SysMath.Sqrt(u * v * w) / (4 * area);

            Vec2d[] pcentros = EvaluateTwoCenters(pinicial, pfinal, radio);

            Vec2d pcentro = pcentros[0];
            if (vecMath.Length(ppaso, pcentro) > (SysMath.Abs(radio) + MathUtils.EPSILON))
            {
                pcentro = pcentros[1];
            }

            Vec2d vIni = pinicial.Sub(pcentro);
            Vec2d vFin = pfinal.Sub(pcentro);
            double alfaIni = vecMath.Angle(new Vec2d(1, 0), vIni);
            double alfaFin = vecMath.Angle(vIni, vFin);

            if (alfaIni < 0)
            {
                alfaIni += 2.0 * SysMath.PI;
            }

            if (alfaFin < 0)
            {
                alfaFin += 2.0 * SysMath.PI;
            }

            if (radio < 0)
            {
                return NewArcWithAdvance(pcentro, SysMath.Abs(radio), alfaIni, alfaFin);
            }
            else
            {
                return NewArcWithAdvance(pcentro, SysMath.Abs(radio), alfaIni, alfaFin - 2.0 * SysMath.PI);
            }
        }

        private static readonly DoubleMath math = DoubleMath.Instance;
        private static readonly VecMath<double, DoubleMath, Vec2d, Vec2dFactory> vecMath = VecMath<double, DoubleMath, Vec2d, Vec2dFactory>.Instance;
    }
}