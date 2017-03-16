#region License

// Copyright 2017 Jose Luis Rovira Martin
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System;
using System.Collections;
using Essence.Util.Math.Double;
using SysMath = System.Math;
using LenP1P2Dir = System.Tuple<double, Essence.Maths.Double.Vec2d, Essence.Maths.Double.Vec2d, Essence.Maths.Double.Vec2d>;

namespace Essence.Maths.Double.Curves
{
    public class CircleArc2Utils
    {
        /// <summary>
        ///     Crea un arco indicando el angulo inicial y el angulo de avance.
        ///     El angulo inicial puede ser positivo o negativo e indica el punto donde empieza el
        ///     arco.
        ///     El angulo de avance indica cuanto avanza el arco.
        ///     Si es positivo, avanza en sentido contrario a las agujas del reloj.
        ///     Si es negativo, avanza en sentido de las agujas del reloj.
        /// </summary>
        /// <param name="center">Centro.</param>
        /// <param name="radius">Radio.</param>
        /// <param name="angle1">Angulo inicial del arco (radianes).</param>
        /// <param name="advAngle">Angulo de avance del arco (radianes).</param>
        /// <returns></returns>
        public static CircleArc2 NewArcWithAdvance(Vec2d center, double radius, double angle1, double advAngle)
        {
            return new CircleArc2(center, radius, angle1, angle1 + advAngle);
        }

        /// <summary>
        ///     Crea un arco de circunferencia indicando puntoInicial, puntoFinal, centro, radio y sentido de giro (cw).
        /// </summary>
        public static CircleArc2 NewArc(Vec2d pt1, Vec2d pt2, Vec2d center, double radius, bool cw)
        {
            double a1 = vecMath.Angle(pt1.Sub(center));
            double a2 = vecMath.Angle(pt2.Sub(center));

            a1 = AngleUtils.Ensure0To2Pi(a1);
            a2 = AngleUtils.Ensure0To2Pi(a2);

            if (cw)
            {
                if (a2 > a1)
                {
                    a2 -= 2 * SysMath.PI;
                }
            }
            else
            {
                if (a2 < a1)
                {
                    a2 += 2 * SysMath.PI;
                }
            }

            return new CircleArc2(center, radius, a1, a2);
        }

        /// <summary>
        ///     Crea un arco de circunferencia indicando dos puntos y un radio. El arco creado es siempre el que
        ///     no contiene el extremo positivo del eje X.
        ///     Cuidado con los radios negativos!!!
        /// </summary>
        public static CircleArc2 TwoPointsRadius(Vec2d pt0, Vec2d pt1, double radius, bool leftRule)
        {
            Vec2d centro = EvaluateCenter(pt0, pt1, radius, leftRule);
            return NewArc(pt0, pt1, centro, SysMath.Abs(radius), leftRule ? (radius < 0) : (radius > 0));
        }

        /// <summary>
        ///     Obtiene el centro de la circunferencia que pasa por los 2 puntos, con el radio indicado.
        ///     Dependiendo del criterio (leftRule), se interpreta el signo del radio:
        ///     Si leftRule entonces el radio se multiplica por la normal a la izquierda (leftNormal) para obtener el centro de la
        ///     circunferencia.
        ///     Si rightRule (!leftRule) entonces el radio se multiplica por la normal a la derecha (rightNormal) para obtener el
        ///     centro de la circunferencia.
        ///     Clip utiliza un criterio rightRule.
        ///     <p />
        ///     Criterio rightRule:
        ///     <c><![CDATA[
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
        ///     <![CDATA[
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
        ///     Teniendo como dato p1, p2 y r, tenemos que obtener el centro del circulo que pasa por
        ///     p1 y p2, con radio r.
        ///     Con el vector 1-2 obtenemos la distancia a.
        ///     b es calculado mediante la fórmula b = raizcua(r * r + a/2 * a/2).
        ///     Creamos un vector perpendicular al 1-2 a una distacion a/2 desde p1 y obtenemos
        ///     el punto central de la circunferencia.
        ///     Con este dato y conociendo el radio ya simplemente calculamos la esquina del rectangulo.
        ///     Si el radio es positivo, avanza en sentido contrario a las agujas del reloj.
        ///     Si el radio es negativo, avanza en sentido de las agujas del reloj.
        ///     <![CDATA[
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
        /// <exception cref="CalculoImposibleException">
        ///     Indica que no se puede calcular el
        ///     centro.
        /// </exception>
        public static Vec2d EvaluateCenter(Vec2d pt0, Vec2d pt1, double radius, bool leftRule)
        {
            // Vector direccion normalizado y longitud.
            Vec2d dir = pt1.Sub(pt0);
            double a = dir.Length;

            if (a.EpsilonEquals(0))
            {
                throw new Exception("CalculoImposible: Puntos coincidentes.");
            }

            dir = dir.Div(a);

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

            double b = SysMath.Sign(radius) * SysMath.Sqrt(v);

            Vec2d normal;
            if (leftRule)
            {
                // Vector normal a la izquierda.
                normal = vecMath.PerpLeft(dir);
            }
            else
            {
                // Vector normal a la derecha.
                normal = vecMath.PerpRight(dir);
            }

            Vec2d vectorm1 = normal.Mul(b);
            return pm.Add(vectorm1);
        }

        /// <summary>
        ///     Indica si los puntos estan alineados.
        /// </summary>
        /// <param name="pt0">Punto 1.</param>
        /// <param name="pt1">Punto 2.</param>
        /// <param name="pt2">Punto 3.</param>
        /// <returns>
        ///     Indica si los puntos estan al
        ///     ineados.
        /// </returns>
        public static bool AreAligned(Vec2d pt0, Vec2d pt1, Vec2d pt2)
        {
            Vec2d v13 = pt2.Sub(pt0);
            Vec2d v12 = pt1.Sub(pt0);
            double area = v13.Cross(v12) / 2;
            return area.EpsilonEquals(0);
        }

        /// <summary>
        ///     Arco que pasa por los puntos <c>pinicial</c>, <c>ppaso</c>, <c>pfinal</c>.
        /// </summary>
        /// <exception cref="PuntosAlineadosException">
        ///     Indica que los puntos estan
        ///     alineados.
        /// </exception>
        public static CircleArc2 ThreePoints(Vec2d p1, Vec2d pp, Vec2d p2)
        {
            Vec2d pcenter = GetCenter(p1, pp, p2);

            double radius = vecMath.Length(pcenter, p1);

            Vec2d v1 = p1.Sub(pcenter);
            Vec2d vp = pp.Sub(pcenter);
            Vec2d v2 = p2.Sub(pcenter);
            double alpha1 = vecMath.Angle(v1);
            double alphap = vecMath.Angle(vp);
            double alpha2 = vecMath.Angle(v2);

            { // Se prueba el sentido CCW, alpha1 < alphap < alpha2
                double a1 = alpha1;
                double ap = alphap;
                double a2 = alpha2;
                while (ap < a1)
                {
                    ap += 2 * SysMath.PI;
                }
                while (a2 < ap)
                {
                    a2 += 2 * SysMath.PI;
                }
                if (a2 - a1 > 2 * SysMath.PI)
                { // Se ha dado mas de una vuelta, solucion no valida.
                }
                else
                {
                    return NewArcWithAdvance(pcenter, radius, AngleUtils.Ensure0To2Pi(a1), a2 - a1);
                }
            }

            { // Se prueba el sentido CW, alpha1 > alphap > alpha2
                double a1 = alpha1;
                double ap = alphap;
                double a2 = alpha2;
                while (ap > a1)
                {
                    ap -= 2 * SysMath.PI;
                }
                while (a2 > ap)
                {
                    a2 -= 2 * SysMath.PI;
                }
                if (a1 - a2 > 2 * SysMath.PI)
                { // Se ha dado mas de una vuelta, solucion no valida.
                }
                else
                {
                    return NewArcWithAdvance(pcenter, radius, AngleUtils.Ensure0To2Pi(a1), a2 - a1);
                }
            }

            throw new Exception();

#if false
            double alpha2 = vecMath.Angle(v1, v2);

            alpha1 = AngleUtils.Ensure0To2Pi(alpha1);

            // Existen 2 soluciones que dependen de angulo de pp:
            //  si alpha2 > 0 : alpha2 y alpha2 - 2*PI
            //  si alpha2 < 0 : alpha2 y alpha2 + 2*PI

            if (alpha2 > 0)
            {
                double alpha3 = vecMath.Angle(pp);
            }
            else
            {
                
            }
            return NewArcWithAdvance(pcenter, radius, alpha1, alpha2);
#endif
        }

        /// <summary>
        ///     Calcula el centro de la circunferencia que pasa por los 3 puntos.
        /// </summary>
        public static Vec2d GetCenter(Vec2d p1, Vec2d p2, Vec2d p3)
        {
            Vec2d v1 = p2.Sub(p1);
            Vec2d v2 = p3.Sub(p1);
            Vec2d v3 = p3.Sub(p2);
            double l1 = v1.Length2;
            double l2 = v2.Length2;
            double l3 = v3.Length2;

            LenP1P2Dir[] vs =
            {
                Tuple.Create(l1, p1, p2, v1),
                Tuple.Create(l2, p1, p3, v2),
                Tuple.Create(l3, p2, p3, v3)
            };
            Array.Sort(vs, Length2Comparar.Instance);

            // Para mejorar el calculo, se toman los segmentos mas alejados.
            Vec2d pm_a = vecMath.Interpolate(vs[1].Item2, vs[1].Item3, 0.5);
            Vec2d d_a = vecMath.PerpLeft(vs[1].Item4);

            Vec2d pm_b = vecMath.Interpolate(vs[2].Item2, vs[2].Item3, 0.5);
            Vec2d d_b = vecMath.PerpLeft(vs[2].Item4);

            // Se resuelve la ecuacion: d_a * t1 + pm_a = d_b * t2 + pm_b
            // Maxima:
            //  e1: d_ax * t1 + pm_ax = d_bx * t2 + pm_bx;
            //  e2: d_ay * t1 + pm_ay = d_by * t2 + pm_by;
            //  algsys ([e1, e2], [t1, t2]);

            double div = (d_a.Y * d_b.X - d_a.X * d_b.Y);
            if (div.EpsilonEquals(0))
            {
                throw new Exception("Cálculo imposible");
            }

            double t1 = (d_b.X * (pm_b.Y - pm_a.Y) - d_b.Y * (pm_b.X - pm_a.X)) / div;
            //double t2 = (d_a.X * (pm_b.Y - pm_a.Y) - d_a.Y * (pm_b.X - pm_a.X)) / div;

            return d_a.Mul(t1).Add(pm_a);
        }

        #region private

        private static readonly DoubleMath math = DoubleMath.Instance;
        private static readonly VecMath<double, DoubleMath, Vec2d, Vec2dFactory> vecMath = VecMath<double, DoubleMath, Vec2d, Vec2dFactory>.Instance;

        private sealed class Length2Comparar : IComparer
        {
            public static readonly Length2Comparar Instance = new Length2Comparar();

            public int Compare(object x, object y)
            {
                return ((LenP1P2Dir)x).Item1.CompareTo(((LenP1P2Dir)y).Item1);
            }
        }

        #endregion
    }
}