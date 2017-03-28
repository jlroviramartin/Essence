using System.Diagnostics.Contracts;
using org.apache.commons.math3.analysis.integration;
using UnaryFunction = System.Func<double, double>;
using REAL = System.Double;

namespace Essence.Maths.Double.Curves
{
    public abstract class MultiCurve1 : ICurve1
    {
        public abstract int SegmentsCount { get; }

        public abstract REAL GetTMin(int indice);

        public abstract REAL GetTMax(int indice);

        #region Position and derivatives

        protected abstract REAL GetPosition(int index, REAL t);

        protected virtual REAL GetFirstDerivative(int index, REAL t)
        {
            UnaryFunction fdx = Derivative.Central(tt => this.GetPosition(index, tt), 1, 5);
            return fdx(t);
        }

        protected virtual REAL GetSecondDerivative(int index, REAL t)
        {
            UnaryFunction fdx = Derivative.Central(tt => this.GetPosition(index, tt), 2, 5);
            return fdx(t);
        }

        protected virtual REAL GetThirdDerivative(int index, REAL t)
        {
            UnaryFunction fdx = Derivative.Central(tt => this.GetPosition(index, tt), 3, 5);
            return fdx(t);
        }

        #endregion

        #region Differential geometric quantities

        protected virtual REAL GetLength(int index, REAL tInSegment0, REAL tInSegment1)
        {
            Contract.Assert(tInSegment0 <= tInSegment1);

            RombergIntegrator iintegral = new RombergIntegrator();
            return iintegral.integrate(IntegralMaxEval, new DelegateUnivariateFunction(t => this.GetSpeed(index, t)), tInSegment0, tInSegment1);
        }

        protected virtual REAL GetSpeed(int index, REAL tInSegment)
        {
            return this.GetFirstDerivative(index, tInSegment);
        }

        #endregion

        protected abstract void FindIndex(REAL t, out int index, out REAL tInSegment);

        #region private

        private void EnsureLengthsEvaluated()
        {
            if (this.lengths == null)
            {
                this.EvaluateLengths();
                Contract.Assert((this.lengths != null) && (this.accLengths != null));
            }
        }

        /// <summary>
        ///     Inicializa las longitudes y la longitudes acumuladas.
        /// </summary>
        private void EvaluateLengths()
        {
            int numSegmentos = this.SegmentsCount;

            this.lengths = new REAL[numSegmentos];
            this.accLengths = new REAL[numSegmentos];

            // Arc lengths and accumulative arc length of the segments.
            REAL longitudAcum = 0;
            for (int i = 0; i < numSegmentos; i++)
            {
                REAL longitud = this.GetLength(i, this.GetTMin(i), this.GetTMax(i));
                longitudAcum += longitud;

                this.lengths[i] = longitud;
                this.accLengths[i] = longitudAcum;
            }
        }

        /// <summary>Longitudes.</summary>
        private REAL[] lengths;

        /// <summary>Longitudes acumuladas.</summary>
        private REAL[] accLengths;

        /// <summary>Número máximo de evaluaciones para el cálculo de la integral.</summary>
        private const int IntegralMaxEval = 1000;

        #endregion

        #region ICurve1

        public virtual REAL TMin
        {
            get { return this.GetTMin(0); }
        }

        public virtual REAL TMax
        {
            get { return this.GetTMax(this.SegmentsCount - 1); }
        }

        public virtual void SetTInterval(REAL tmin, REAL tmax)
        {
        }

        public virtual REAL GetPosition(REAL t)
        {
            int index;
            REAL tInSegment;
            this.FindIndex(t, out index, out tInSegment);

            return this.GetPosition(index, tInSegment);
        }

        public virtual REAL GetFirstDerivative(REAL t)
        {
            int index;
            REAL tInSegment;
            this.FindIndex(t, out index, out tInSegment);

            return this.GetFirstDerivative(index, tInSegment);
        }

        public virtual REAL GetSecondDerivative(REAL t)
        {
            int index;
            REAL tInSegment;
            this.FindIndex(t, out index, out tInSegment);

            return this.GetSecondDerivative(index, tInSegment);
        }

        public virtual REAL GetThirdDerivative(REAL t)
        {
            int index;
            REAL tInSegment;
            this.FindIndex(t, out index, out tInSegment);

            return this.GetThirdDerivative(index, tInSegment);
        }

        public virtual REAL TotalLength
        {
            get
            {
                this.EnsureLengthsEvaluated();
                return this.accLengths[this.accLengths.Length - 1];
            }
        }

        public virtual REAL GetLength(REAL t0, REAL t1)
        {
            Contract.Assert(t1 >= t0);

            if (t0 < this.TMin)
            {
                t0 = this.TMin;
            }

            if (t1 > this.TMax)
            {
                t1 = this.TMax;
            }

            this.EnsureLengthsEvaluated();

            int index0, index1;
            REAL tInSegment0, tInSegment1;
            this.FindIndex(t0, out index0, out tInSegment0);
            this.FindIndex(t1, out index1, out tInSegment1);

            REAL longitud;
            if (index0 != index1)
            {
                // Add on partial first segment.
                longitud = this.GetLength(index0, tInSegment0, this.GetTMax(index0));

                // NOTA: mas eficiente utilizar accLengths!
                // Accumulate full-segment lengths.
                for (int i = index0 + 1; i < index1; i++)
                {
                    longitud += this.lengths[i];
                }

                // Add on partial last segment.
                if (index1 < this.SegmentsCount)
                {
                    longitud += this.GetLength(index1, this.GetTMin(index1), tInSegment1);
                }
            }
            else
            {
                longitud = this.GetLength(index0, tInSegment0, tInSegment1);
            }

            return longitud;
        }

        public virtual REAL GetSpeed(REAL t)
        {
            int index;
            REAL tInSegment;
            this.FindIndex(t, out index, out tInSegment);

            return this.GetSpeed(index, tInSegment);
        }

        #endregion
    }
}