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
using Essence.Util.Math;
using Essence.Util.Math.Double;
using java.lang;
using Exception = System.Exception;
using Math = System.Math;

namespace Essence.Geometry.Curves
{
    public class Polynomial : IFormattable, IEpsilonEquatable<Polynomial>, ICloneable
    {
        /// <summary>
        ///     Computes the monomial x^degree.
        /// </summary>
        /// <param name="degree"></param>
        /// <returns></returns>
        public static Polynomial Monomial(int degree, double c = 1)
        {
            double[] coeffs = new double[degree + 1];

            for (int i = 0; i < degree; i++)
            {
                coeffs[i] = 0;
            }

            coeffs[degree] = c;

            return new Polynomial(coeffs);
        }

        public static Polynomial[] GetStandardBase(int dim)
        {
            if (dim < 1)
            {
                throw new Exception("Dimension expected to be greater than zero.");
            }

            Polynomial[] buf = new Polynomial[dim];

            for (int i = 0; i < dim; i++)
            {
                buf[i] = Monomial(i);
            }

            return buf;
        }

        public Polynomial(params double[] coeffs)
        {
            this.coefficients = coeffs;
        }

        public const string COEFFICIENTS = "coefficients";

        public double this[int i]
        {
            get { return this.coefficients[i]; }
        }

        /// <summary>
        ///     Degree of the polynomial.
        /// </summary>
        public int Degree
        {
            get { return this.coefficients.Length - 1; }
        }

        /// <summary>
        ///     Checks if given polynomial is zero.
        /// </summary>
        /// <returns></returns>
        public bool IsZero()
        {
            for (int i = 0; i < this.coefficients.Length; i++)
            {
                if (!this.coefficients[i].EpsilonEquals(0))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        ///     Evaluates polynomial by using the horner scheme.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public double Evaluate(double x)
        {
            double buf = this.coefficients[this.Degree];

            for (int i = this.Degree - 1; i >= 0; i--)
            {
                buf = this.coefficients[i] + x * buf;
            }

            return buf;
        }

        /// <summary>
        ///     Normalizes the polynomial, e.i. divides each coefficient by the
        ///     coefficient of a_n the greatest term if a_n != 1.
        /// </summary>
        public void Normalize()
        {
            this.Clean();

            double n = this.coefficients[this.Degree];
            if (!n.EpsilonEquals(1))
            {
                n = 1 / n;
                for (int k = 0; k <= this.Degree; k++)
                {
                    this.coefficients[k] *= n;
                }
            }
        }

        /// <summary>
        ///     Removes unnecessary zero terms.
        /// </summary>
        public void Clean()
        {
            int i;
            for (i = this.Degree; (i >= 0) && this.coefficients[i].EpsilonEquals(0); i--)
            {
            }

            double[] coeffs = new double[i + 1];

            for (int k = 0; k <= i; k++)
            {
                coeffs[k] = this.coefficients[k];
            }

            this.coefficients = (double[])coeffs.Clone();
        }

        /// <summary>
        ///     Differentiates given polynomial p.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Polynomial Derivative()
        {
            double[] buf = new double[this.Degree];

            for (int i = 0; i < buf.Length; i++)
            {
                double c = (i + 1) * this.coefficients[i + 1];
                if (!c.EpsilonEquals(0))
                {
                    buf[i] = c;
                }
            }

            return new Polynomial(buf);
        }

        /// <summary>
        ///     Integrates given polynomial p.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Polynomial Integral()
        {
            double[] buf = new double[this.Degree + 2];
            buf[0] = 0; // this value can be arbitrary, in fact

            for (int i = 1; i < buf.Length; i++)
            {
                double c = this.coefficients[i - 1] / i;
                if (!c.EpsilonEquals(0))
                {
                    buf[i] = c;
                }
            }

            return new Polynomial(buf);
        }

        #region private

        private double[] coefficients;

        #endregion

        #region Object

        public override string ToString()
        {
            return this.ToString("F3");
        }

        public override bool Equals(object obj)
        {
            Polynomial polynomial = obj as Polynomial;
            if (polynomial == null)
            {
                return false;
            }
            int degree = Math.Max(this.Degree, polynomial.Degree);
            for (int i = 0; i <= degree; i++)
            {
                double a = ((i <= this.Degree) ? this[i] : 0);
                double b = ((i <= polynomial.Degree) ? polynomial[i] : 0);
                if (!a.Equals(b))
                {
                    return false;
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            // http://www.jarvana.com/jarvana/view/org/apache/lucene/lucene-spatial/2.9.3/lucene-spatial-2.9.3-sources.jar!/org/apache/lucene/spatial/geometry/shape/Vector2D.java
            const int prime = 31;
            int hash = 1;
            unchecked
            {
                for (int i = 0; i <= this.Degree; i++)
                {
                    hash = prime * hash + this[i].GetHashCode();
                }
            }
            return hash;
        }

        #endregion

        #region  IFormattable

        public string ToString(string format, IFormatProvider provider = null)
        {
            if (provider != null)
            {
                ICustomFormatter formatter = provider.GetFormat(this.GetType()) as ICustomFormatter;
                if (formatter != null)
                {
                    return formatter.Format(format, this, provider);
                }
            }

            if (this.IsZero())
            {
                return "0";
            }
            else
            {
                StringBuffer s = new StringBuffer();

                bool first = true;
                for (int i = 0; i < this.Degree + 1; i++)
                {
                    if (!this.coefficients[i].EpsilonEquals(0))
                    {
                        if (!first)
                        {
                            s.append(" + ");
                        }
                        first = false;

                        s.append(this.coefficients[i].ToString(format, provider));

                        if (i == 1)
                        {
                            s.append("x");
                        }
                        else
                        {
                            s.append("x^").append(i.ToString(format, provider));
                        }
                    }
                }

                return s.toString();
            }
        }

        #endregion

        #region IEpsilonEquatable<Polynomial>

        public bool EpsilonEquals(Polynomial polynomial, double epsilon = MathUtils.EPSILON)
        {
            if (polynomial == null)
            {
                return false;
            }
            int degree = Math.Max(this.Degree, polynomial.Degree);
            for (int i = 0; i <= degree; i++)
            {
                double a = ((i <= this.Degree) ? this[i] : 0);
                double b = ((i <= polynomial.Degree) ? polynomial[i] : 0);
                if (!a.EpsilonEquals(b, epsilon))
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region ICloneable

        public object Clone()
        {
            Polynomial copia = (Polynomial)this.MemberwiseClone();
            copia.coefficients = new double[this.coefficients.Length];
            for (int i = 0; i <= this.Degree; i++)
            {
                copia.coefficients[i] = this[i];
            }
            return copia;
        }

        #endregion
    }
}