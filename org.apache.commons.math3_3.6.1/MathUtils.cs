using System;

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

    public class Locale
    {
        public static readonly Locale US;

        public static Locale getDefault()
        {
            return US;
        }
    }

    /*public class Number
    {
        public Number(IConvertible convertible)
        {
            this.convertible = convertible;
        }

        public static implicit operator int(Number n)
        {
            return n.convertible.ToInt32(null);
        }

        public static implicit operator long(Number n)
        {
            return n.convertible.ToInt64(null);
        }

        public static implicit operator Number(int n)
        {
            return new Number(n);
        }

        public static implicit operator Number(long n)
        {
            return new Number(n);
        }

        private readonly IConvertible convertible;
    }*/
}

namespace org.apache.commons.math3.exception.util
{
    using Number = System.IConvertible;

    public class ExceptionContext
    {
        public ExceptionContext(Exception exception)
        {
            this.exception = exception;
        }

        private Exception exception;

        internal void AddMessage(Localizable pattern, params object[] args)
        {
            throw new NotImplementedException();
        }

        internal void AddMessage(LocalizedFormats pattern, params object[] args)
        {
            throw new NotImplementedException();
        }

        internal void AddMessage(LocalizedFormats evaluations)
        {
            throw new NotImplementedException();
        }

        internal void AddMessage(Localizable specific, Number max, params object[] args)
        {
            throw new NotImplementedException();
        }

        internal void AddMessage(LocalizedFormats specific, Number max, params object[] args)
        {
            throw new NotImplementedException();
        }

        internal string GetMessage()
        {
            throw new NotImplementedException();
        }

        internal string GetLocalizedMessage()
        {
            throw new NotImplementedException();
        }
    }
}