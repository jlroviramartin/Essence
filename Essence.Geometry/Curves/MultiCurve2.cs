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

using System.Diagnostics.Contracts;
using Essence.Geometry.Core.Double;
using Essence.Maths;
using Essence.Maths.Double;
using Essence.Util.Math.Double;
using SysMath = System.Math;
using UnaryFunction = System.Func<double, double>;

namespace Essence.Geometry.Curves
{
    public abstract class MultiCurve2 : ICurve2
    {
        public abstract int SegmentsCount { get; }

        public abstract double GetTMin(int indice);

        public abstract double GetTMax(int indice);

        #region Position and derivatives

        protected abstract Point2d GetPosition(int index, double t);

        protected virtual Vector2d GetFirstDerivative(int index, double t)
        {
            UnaryFunction fdx = Derivative.Central(tt => this.GetPosition(index, tt).X, 1, 5);
            UnaryFunction fdy = Derivative.Central(tt => this.GetPosition(index, tt).Y, 1, 5);
            return new Vector2d(fdx(t), fdy(t));
        }

        protected virtual Vector2d GetSecondDerivative(int index, double t)
        {
            UnaryFunction fdx = Derivative.Central(tt => this.GetPosition(index, tt).X, 2, 5);
            UnaryFunction fdy = Derivative.Central(tt => this.GetPosition(index, tt).Y, 2, 5);
            return new Vector2d(fdx(t), fdy(t));
        }

        protected virtual Vector2d GetThirdDerivative(int index, double t)
        {
            UnaryFunction fdx = Derivative.Central(tt => this.GetPosition(index, tt).X, 3, 5);
            UnaryFunction fdy = Derivative.Central(tt => this.GetPosition(index, tt).Y, 3, 5);
            return new Vector2d(fdx(t), fdy(t));
        }

        #endregion

        #region Differential geometric quantities

        protected virtual double GetLength(int index, double tInSegment0, double tInSegment1)
        {
            Contract.Assert(tInSegment0 <= tInSegment1);

            return Integrator.Integrate(t => this.GetSpeed(index, t), tInSegment0, tInSegment1, Integrator.Type.RombergIntegrator, IntegralMaxEval);
        }

        protected virtual double GetSpeed(int index, double tInSegment)
        {
            return this.GetFirstDerivative(index, tInSegment).Length;
        }

        protected virtual double GetCurvature(int index, double tInSegment)
        {
            Vector2d der1 = this.GetFirstDerivative(index, tInSegment);
            double speed2 = der1.LengthSquared;

            if (speed2.EpsilonEquals(0))
            {
                // Curvature is indeterminate, just return 0.
                return 0;
            }

            Vector2d der2 = this.GetSecondDerivative(index, tInSegment);

            double numer = der1.Dot(der2);
            double denom = SysMath.Pow(speed2, 1.5);
            return numer / denom;
        }

        protected virtual Vector2d GetTangent(int index, double tInSegment)
        {
            return this.GetFirstDerivative(index, tInSegment).Unit;
        }

        protected virtual Vector2d GetLeftNormal(int index, double tInSegment)
        {
            return this.GetTangent(index, tInSegment).PerpLeft;
        }

        protected virtual void GetFrame(int index, double tInSegment, ref Point2d position, ref Vector2d tangent, ref Vector2d normal)
        {
            position = this.GetPosition(index, tInSegment);
            tangent = this.GetTangent(index, tInSegment);
            normal = tangent.PerpLeft;
        }

        #endregion

        protected abstract void FindIndex(double t, out int index, out double tInSegment);

        protected abstract BoundingBox2d GetBoundingBox(int index);

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
        /// Inicializa las longitudes y la longitudes acumuladas.
        /// </summary>
        private void EvaluateLengths()
        {
            int numSegmentos = this.SegmentsCount;

            this.lengths = new double[numSegmentos];
            this.accLengths = new double[numSegmentos];

            // Arc lengths and accumulative arc length of the segments.
            double longitudAcum = 0;
            for (int i = 0; i < numSegmentos; i++)
            {
                double longitud = this.GetLength(i, this.GetTMin(i), this.GetTMax(i));
                longitudAcum += longitud;

                this.lengths[i] = longitud;
                this.accLengths[i] = longitudAcum;
            }
        }

        /// <summary>Longitudes.</summary>
        private double[] lengths;

        /// <summary>Longitudes acumuladas.</summary>
        private double[] accLengths;

        /// <summary>Número máximo de evaluaciones para el cálculo de la integral.</summary>
        private const int IntegralMaxEval = 1000;

        #endregion

        #region ICurve2

        public abstract bool IsClosed { get; }

        public virtual double TMin
        {
            get { return this.GetTMin(0); }
        }

        public virtual double TMax
        {
            get { return this.GetTMax(this.SegmentsCount - 1); }
        }

        public virtual void SetTInterval(double tmin, double tmax)
        {
        }

        public virtual Point2d GetPosition(double t)
        {
            int index;
            double tInSegment;
            this.FindIndex(t, out index, out tInSegment);

            return this.GetPosition(index, tInSegment);
        }

        public virtual Vector2d GetFirstDerivative(double t)
        {
            int index;
            double tInSegment;
            this.FindIndex(t, out index, out tInSegment);

            return this.GetFirstDerivative(index, tInSegment);
        }

        public virtual Vector2d GetSecondDerivative(double t)
        {
            int index;
            double tInSegment;
            this.FindIndex(t, out index, out tInSegment);

            return this.GetSecondDerivative(index, tInSegment);
        }

        public virtual Vector2d GetThirdDerivative(double t)
        {
            int index;
            double tInSegment;
            this.FindIndex(t, out index, out tInSegment);

            return this.GetThirdDerivative(index, tInSegment);
        }

        public virtual double TotalLength
        {
            get
            {
                this.EnsureLengthsEvaluated();
                return this.accLengths[this.accLengths.Length - 1];
            }
        }

        public virtual double GetLength(double t0, double t1)
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
            double tInSegment0, tInSegment1;
            this.FindIndex(t0, out index0, out tInSegment0);
            this.FindIndex(t1, out index1, out tInSegment1);

            double longitud;
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

        public virtual double GetSpeed(double t)
        {
            int index;
            double tInSegment;
            this.FindIndex(t, out index, out tInSegment);

            return this.GetSpeed(index, tInSegment);
        }

        public virtual double GetCurvature(double t)
        {
            int index;
            double tInSegment;
            this.FindIndex(t, out index, out tInSegment);

            return this.GetCurvature(index, tInSegment);
        }

        public virtual Vector2d GetTangent(double t)
        {
            return this.GetFirstDerivative(t).Unit;
        }

        public virtual Vector2d GetLeftNormal(double t)
        {
            int index;
            double tInSegment;
            this.FindIndex(t, out index, out tInSegment);

            return this.GetLeftNormal(index, tInSegment);
        }

        public virtual void GetFrame(double t, ref Point2d position, ref Vector2d tangent, ref Vector2d normal)
        {
            int index;
            double tInSegment;
            this.FindIndex(t, out index, out tInSegment);

            this.GetFrame(index, tInSegment, ref position, ref tangent, ref normal);
        }

        public BoundingBox2d BoundingBox
        {
            get
            {
                BoundingBox2d boundingBox = BoundingBox2d.Empty;
                for (int i = 0; i < this.SegmentsCount; i++)
                    boundingBox = boundingBox.Union(this.GetBoundingBox(i));
                return boundingBox;
            }
        }

        #endregion
    }
}