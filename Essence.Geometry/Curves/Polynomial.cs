using System;
using Essence.Util.Math;
using Essence.Util.Math.Double;
using java.lang;
using Exception = System.Exception;
using Math = System.Math;
using REAL = System.Double;

namespace Essence.Maths.Double.Curves
{
    public class Polynomial : IFormattable, IEpsilonEquatable<Polynomial>, ICloneable
    {
        /// <summary>
        ///     Computes the monomial x^degree.
        /// </summary>
        /// <param name="degree"></param>
        /// <returns></returns>
        public static Polynomial Monomial(int degree, REAL c = 1)
        {
            REAL[] coeffs = new REAL[degree + 1];

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

        public Polynomial(params REAL[] coeffs)
        {
            this.coefficients = coeffs;
        }

        public const string COEFFICIENTS = "coefficients";

        public REAL this[int i]
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
        public REAL Evaluate(REAL x)
        {
            REAL buf = this.coefficients[this.Degree];

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

            REAL n = this.coefficients[this.Degree];
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

            REAL[] coeffs = new REAL[i + 1];

            for (int k = 0; k <= i; k++)
            {
                coeffs[k] = this.coefficients[k];
            }

            this.coefficients = (REAL[])coeffs.Clone();
        }

        /// <summary>
        ///     Differentiates given polynomial p.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Polynomial Derivative()
        {
            REAL[] buf = new REAL[this.Degree];

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
            REAL[] buf = new REAL[this.Degree + 2];
            buf[0] = 0; // this value can be arbitrary, in fact

            for (int i = 1; i < buf.Length; i++)
            {
                REAL c = this.coefficients[i - 1] / i;
                if (!c.EpsilonEquals(0))
                {
                    buf[i] = c;
                }
            }

            return new Polynomial(buf);
        }

        #region private

        private REAL[] coefficients;

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
                REAL a = ((i <= this.Degree) ? this[i] : 0);
                REAL b = ((i <= polynomial.Degree) ? polynomial[i] : 0);
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
                REAL a = ((i <= this.Degree) ? this[i] : 0);
                REAL b = ((i <= polynomial.Degree) ? polynomial[i] : 0);
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
            copia.coefficients = new REAL[this.coefficients.Length];
            for (int i = 0; i <= this.Degree; i++)
            {
                copia.coefficients[i] = this[i];
            }
            return copia;
        }

        #endregion
    }
}