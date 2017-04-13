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

using System;
using Essence.Geometry.Core.Double;
using Essence.Geometry.Core.Float;
using Essence.Util.Math.Double;

namespace Essence.Geometry.Core
{
    /// <summary>
    ///     Fuente de luz.
    ///     <see cref="http://www.opengl.org/sdk/docs/man/xhtml/glLight.xml" />
    ///     <see cref="http://www.glprogramming.com/red/chapter05.html#name4" />
    /// </summary>
    public sealed partial class Light : IFormattable, ICloneable
    {
        public const string FOCUS = "Focus";
        public const string POSITION = "Position";
        public const string DIRECTION = "Direction";

        public const string AMBIENT = "Ambient";
        public const string DIFFUSE = "Diffuse";
        public const string ESPECULAR = "Especular";

        public const string CONSTANT_ATTENUATION = "ConstantAttenuation";
        public const string LINEAR_ATTENUATION = "LinearAttenuation";
        public const string QUADRATIC_ATTENUATION = "QuadraticAttenuation";

        public const string SPOT_CUTOFF = "SpotCutoff";
        public const string SPOT_EXPONENT = "SpotExponent";

        public static Light NewDireccional(Vector3d dir,
                                           Color4f? ambiente = null,
                                           Color4f? difuso = null,
                                           Color4f? especular = null)
        {
            Light luz = new Light();

            luz.Focus = false;
            luz.Direction = dir;

            luz.Ambient = ambiente ?? new Color4f(0, 0, 0, 1);
            luz.Diffuse = difuso ?? new Color4f(1, 1, 1, 1);
            luz.Especular = especular ?? new Color4f(1, 1, 1, 1);
            return luz;
        }

        public static Light NewPosicional(Point3d p, Vector3d dir,
                                          Color4f? ambiente = null,
                                          Color4f? difuso = null,
                                          Color4f? especular = null)
        {
            Light luz = new Light();

            luz.Focus = true;
            luz.Position = p;
            luz.Direction = dir;

            luz.Ambient = ambiente ?? new Color4f(0, 0, 0, 1);
            luz.Diffuse = difuso ?? new Color4f(1, 1, 1, 1);
            luz.Especular = especular ?? new Color4f(1, 1, 1, 1);

            return luz;
        }

        public static Light NewFoco(Point3d p, Vector3d dir,
                                    Color4f? ambiente = null,
                                    Color4f? difuso = null,
                                    Color4f? especular = null,
                                    float atenuacionConstante = 1,
                                    float atenuacionLinear = 0,
                                    float atenuacionCuadratica = 0,
                                    float corteFoco = 180,
                                    float exponenteFoco = 0)
        {
            Light luz = new Light();

            luz.Focus = true;
            luz.Position = p;
            luz.Direction = dir;

            luz.Ambient = ambiente ?? new Color4f(0, 0, 0, 1);
            luz.Diffuse = difuso ?? new Color4f(1, 1, 1, 1);
            luz.Especular = especular ?? new Color4f(1, 1, 1, 1);

            luz.ConstantAttenuation = 1;
            luz.LinearAttenuation = 0;
            luz.QuadraticAttenuation = 0;

            luz.SpotCutoff = corteFoco;
            luz.SpotExponent = exponenteFoco;
            return luz;
        }

        public Light()
        {
            this.Focus = false;
            this.Position = new Point3d();
            this.Direction = new Vector3d(0, 0, -1);

            this.Ambient = new Color4f(0, 0, 0, 1);
            this.Diffuse = new Color4f(1, 1, 1, 1);
            this.Especular = new Color4f(1, 1, 1, 1);

            this.ConstantAttenuation = 1;
            this.LinearAttenuation = 0;
            this.QuadraticAttenuation = 0;

            this.SpotCutoff = 180;
            this.SpotExponent = 0;
        }

        public bool Focus { get; set; }

        public Point3d Position { get; set; }

        /// <summary>
        ///     Contains four floating-point values that specify
        ///     the ambient RGBA intensity of the light.
        ///     Floating-point values are mapped directly.
        ///     Neither integer nor floating-point values are clamped.
        ///     The initial ambient light intensity is (0, 0, 0, 1).
        /// </summary>
        public Color4f Ambient { get; set; }

        /// <summary>
        ///     Contains four floating-point values that specify
        ///     the diffuse RGBA intensity of the light.
        ///     Floating-point values are mapped directly.
        ///     Neither integer nor floating-point values are clamped.
        ///     The initial value
        ///     for GL_LIGHT0 is (1, 1, 1, 1); for other lights, the
        ///     initial value is (0, 0, 0, 1).
        /// </summary>
        public Color4f Diffuse { get; set; }

        /// <summary>
        ///     Contains four floating-point values that specify
        ///     the specular RGBA intensity of the light.
        ///     Floating-point values are mapped directly.
        ///     Neither integer nor floating-point values are clamped.
        ///     The initial value
        ///     for GL_LIGHT0 is (1, 1, 1, 1); for other lights, the
        ///     initial value is (0, 0, 0, 1).
        /// </summary>
        public Color4f Especular { get; set; }

        public Vector3d Direction { get; set; }

        /// <summary>
        ///     Is a floating-point value that specifies
        ///     one of the three light attenuation factors.
        ///     Floating-point values are mapped directly.
        ///     Only nonnegative values are accepted.
        ///     If the light is positional,
        ///     rather than directional,
        ///     its intensity is attenuated by the reciprocal of the sum of the constant
        ///     factor, the linear factor times the distance between the light
        ///     and the vertex being lighted,
        ///     and the quadratic factor times the square of the same distance.
        ///     The initial attenuation factors are (1, 0, 0),
        ///     resulting in no attenuation.
        /// </summary>
        public float ConstantAttenuation { get; set; }

        /// <summary>
        ///     Is a floating-point value that specifies
        ///     one of the three light attenuation factors.
        ///     Floating-point values are mapped directly.
        ///     Only nonnegative values are accepted.
        ///     If the light is positional,
        ///     rather than directional,
        ///     its intensity is attenuated by the reciprocal of the sum of the constant
        ///     factor, the linear factor times the distance between the light
        ///     and the vertex being lighted,
        ///     and the quadratic factor times the square of the same distance.
        ///     The initial attenuation factors are (1, 0, 0),
        ///     resulting in no attenuation.
        /// </summary>
        public float LinearAttenuation { get; set; }

        /// <summary>
        ///     Is a floating-point value that specifies
        ///     one of the three light attenuation factors.
        ///     Floating-point values are mapped directly.
        ///     Only nonnegative values are accepted.
        ///     If the light is positional,
        ///     rather than directional,
        ///     its intensity is attenuated by the reciprocal of the sum of the constant
        ///     factor, the linear factor times the distance between the light
        ///     and the vertex being lighted,
        ///     and the quadratic factor times the square of the same distance.
        ///     The initial attenuation factors are (1, 0, 0),
        ///     resulting in no attenuation.
        /// </summary>
        public float QuadraticAttenuation { get; set; }

        /// <summary>
        ///     Is a floating-point value that specifies the
        ///     maximum spread angle of a light source. Floating-point
        ///     values are mapped directly. Only values in the range [0,90] and
        ///     the special value 180 are accepted. If the angle between the
        ///     direction of the light and the direction from the light to the
        ///     vertex being lighted is greater than the spot cutoff angle, the
        ///     light is completely masked. Otherwise, its intensity is
        ///     controlled by the spot exponent and the attenuation factors. The
        ///     initial spot cutoff is 180, resulting in uniform light distribution.
        /// </summary>
        public float SpotCutoff { get; set; }

        /// <summary>
        ///     Is a floating-point value that specifies
        ///     the intensity distribution of the light.
        ///     Floating-point values are mapped directly.
        ///     Only values in the range [0,128] are accepted.
        ///     Effective light intensity is attenuated by the cosine of the angle between
        ///     the direction of the light and the direction from the light to the vertex
        ///     being lighted,
        ///     raised to the power of the spot exponent.
        ///     Thus, higher spot exponents result in a more focused light source,
        ///     regardless of the spot cutoff angle (see GL_SPOT_CUTOFF).
        ///     The initial spot exponent is 0,
        ///     resulting in uniform light distribution.
        /// </summary>
        public float SpotExponent { get; set; }

        public bool EpsilonEquals(object obj, double epsilon)
        {
            if (!(obj is Light))
            {
                return false;
            }
            Light luz = (Light)obj;
            return this.Focus == luz.Focus
                   && this.Position.EpsilonEquals(luz.Position, epsilon)
                   && this.Direction.EpsilonEquals(luz.Direction, epsilon)
                   && this.Ambient.EpsilonEquals(luz.Ambient, epsilon)
                   && this.Diffuse.EpsilonEquals(luz.Diffuse, epsilon)
                   && this.Especular.EpsilonEquals(luz.Especular, epsilon)
                   && MathUtils.EpsilonEquals(this.ConstantAttenuation, luz.ConstantAttenuation, epsilon)
                   && MathUtils.EpsilonEquals(this.LinearAttenuation, luz.LinearAttenuation, epsilon)
                   && MathUtils.EpsilonEquals(this.QuadraticAttenuation, luz.QuadraticAttenuation, epsilon)
                   && MathUtils.EpsilonEquals(this.SpotCutoff, luz.SpotCutoff, epsilon)
                   && MathUtils.EpsilonEquals(this.SpotExponent, luz.SpotExponent, epsilon);
        }

        #region ICloneable

        public object Clone()
        {
            return this;
        }

        #endregion

        #region IFormattable

        public string ToString(string formato, IFormatProvider proveedor)
        {
            if (proveedor != null)
            {
                ICustomFormatter formatter = proveedor.GetFormat(this.GetType()) as ICustomFormatter;
                if (formatter != null)
                {
                    return formatter.Format(formato, this, proveedor);
                }
            }

            /*PropertiesMap map = new PropertiesMap();
            map.Add(FOCO, this.Focus, "", proveedor);
            map.Add(POSICION, this.Position, "F3", proveedor);
            map.Add(DIRECCION, this.Direction, "F3", proveedor);
            map.Add(AMBIENTE, this.Ambient, "F3", proveedor);
            map.Add(DIFUSO, this.Diffuse, "F3", proveedor);
            map.Add(ESPECULAR, this.Especular, "F3", proveedor);
            map.Add(ATENUACION_CONSTANTE, this.ConstantAttenuation, "F3", proveedor);
            map.Add(ATENUACION_LINEAR, this.LinearAttenuation, "F3", proveedor);
            map.Add(ATENUACION_CUADRATICA, this.QuadraticAttenuation, "F3", proveedor);
            map.Add(CORTE_FOCO, this.SpotCutoff, "F3", proveedor);
            map.Add(EXPONENTE_FOCO, this.SpotExponent, "F3", proveedor);
            return map.ToString();*/
            return "Light";
        }

        #endregion

        #region Object

        public override bool Equals(object obj)
        {
            return this.EpsilonEquals(obj, 0);
        }

        public override int GetHashCode()
        {
            return (this.Focus.GetHashCode()
                    ^ this.Position.GetHashCode()
                    ^ this.Direction.GetHashCode()
                    ^ this.Ambient.GetHashCode()
                    ^ this.Diffuse.GetHashCode()
                    ^ this.Especular.GetHashCode()
                    ^ this.ConstantAttenuation.GetHashCode()
                    ^ this.LinearAttenuation.GetHashCode()
                    ^ this.QuadraticAttenuation.GetHashCode()
                    ^ this.SpotCutoff.GetHashCode()
                    ^ this.SpotExponent.GetHashCode());
        }

        #endregion
    }
}