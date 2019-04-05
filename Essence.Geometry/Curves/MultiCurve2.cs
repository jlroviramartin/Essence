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

using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
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
        public MultiCurve2(IEnumerable<double> times)
        {
            this.times = times.ToArray();
        }

        public virtual int NumSegments
        {
            get { return this.times.Length - 1; }
        }

        public virtual double GetTMin(int key)
        {
            return this.times[key];
        }

        public virtual double GetTMax(int key)
        {
            return this.times[key + 1];
        }

        #region Position and derivatives

        protected abstract Point2d GetPosition(int key, double dt);

        protected virtual Vector2d GetFirstDerivative(int key, double dt)
        {
            UnaryFunction fdx = Derivative.Central(tt => this.GetPosition(key, tt).X, 1, 5);
            UnaryFunction fdy = Derivative.Central(tt => this.GetPosition(key, tt).Y, 1, 5);
            return new Vector2d(fdx(dt), fdy(dt));
        }

        protected virtual Vector2d GetSecondDerivative(int key, double dt)
        {
            UnaryFunction fdx = Derivative.Central(tt => this.GetPosition(key, tt).X, 2, 5);
            UnaryFunction fdy = Derivative.Central(tt => this.GetPosition(key, tt).Y, 2, 5);
            return new Vector2d(fdx(dt), fdy(dt));
        }

        protected virtual Vector2d GetThirdDerivative(int key, double dt)
        {
            UnaryFunction fdx = Derivative.Central(tt => this.GetPosition(key, tt).X, 3, 5);
            UnaryFunction fdy = Derivative.Central(tt => this.GetPosition(key, tt).Y, 3, 5);
            return new Vector2d(fdx(dt), fdy(dt));
        }

        #endregion

        #region Differential geometric quantities

        protected virtual double GetLength(int key, double dt0, double dt1)
        {
            Contract.Assert(dt0 <= dt1);

            return Integrator.Integrate(t => this.GetSpeed(key, t), dt0, dt1, Integrator.Type.RombergIntegrator, IntegralMaxEval);
        }

        protected virtual double GetSpeed(int key, double dt)
        {
            return this.GetFirstDerivative(key, dt).Length;
        }

        protected virtual double GetCurvature(int key, double dt)
        {
            Vector2d der1 = this.GetFirstDerivative(key, dt);
            double speed2 = der1.LengthSquared;

            if (speed2.EpsilonEquals(0))
            {
                // Curvature is indeterminate, just return 0.
                return 0;
            }

            Vector2d der2 = this.GetSecondDerivative(key, dt);

            double numer = der1.Dot(der2);
            double denom = SysMath.Pow(speed2, 1.5);
            return numer / denom;
        }

        protected virtual Vector2d GetTangent(int key, double dt)
        {
            return this.GetFirstDerivative(key, dt).Unit;
        }

        protected virtual Vector2d GetLeftNormal(int key, double dt)
        {
            return this.GetTangent(key, dt).PerpLeft;
        }

        protected virtual void GetFrame(int key, double dt,
                                        out Point2d position, out Vector2d tangent, out Vector2d normal)
        {
            position = this.GetPosition(key, dt);
            tangent = this.GetTangent(key, dt);
            normal = tangent.PerpLeft;
        }

        #endregion

        protected virtual void GetKeyInfo(double t, out int key, out double dt)
        {
            int numSegments = this.NumSegments;

            if (t <= this.times[0])
            {
                key = 0;
                dt = 0;
            }
            else if (t >= this.times[numSegments])
            {
                key = numSegments - 1;
                dt = this.times[numSegments] - this.times[numSegments - 1];
            }
            else
            {
                for (int i = 0; i < numSegments; ++i)
                {
                    if (t < this.times[i + 1])
                    {
                        key = i;
                        dt = t - this.times[i];
                        return;
                    }
                }

                key = -1;
                dt = -1;
            }
        }

        protected abstract BoundingBox2d GetBoundingBox(int key);

        #region private

        /// <summary>
        /// Ensures that the lengths and accumulated lengths are initialized.
        /// </summary>
        private void EnsureLengthsInitialized()
        {
            if (this.lengths == null)
            {
                this.InitializeLength();
                Contract.Assert((this.lengths != null) && (this.accumLengths != null));
            }
        }

        /// <summary>
        /// Initializes the lengths and accumulated lengths.
        /// </summary>
        private void InitializeLength()
        {
            int numSegments = this.NumSegments;

            this.lengths = new double[numSegments];
            this.accumLengths = new double[numSegments];

            // Arc lengths and accumulative arc length of the segments.
            double accumLength = 0;
            for (int i = 0; i < numSegments; i++)
            {
                double length = this.GetLength(i, 0, this.times[i + 1] - this.times[i]);
                accumLength += length;

                this.lengths[i] = length;
                this.accumLengths[i] = accumLength;
            }
        }

        private readonly double[] times;

        /// <summary>Lengths.</summary>
        private double[] lengths;

        /// <summary>Accumulated lengths.</summary>
        private double[] accumLengths;

        /// <summary>Número máximo de evaluaciones para el cálculo de la integral.</summary>
        private const int IntegralMaxEval = 1000;

        #endregion

        #region ICurve2

        public abstract bool IsClosed { get; }

        public virtual double TMin
        {
            get { return this.times[0]; }
        }

        public virtual double TMax
        {
            get { return this.times[this.NumSegments]; }
        }

        public virtual void SetTInterval(double tmin, double tmax)
        {
        }

        public virtual double GetT(double length, int iterations = 32, double tolerance = 1e-06)
        {
            int numSegments = this.NumSegments;

            this.EnsureLengthsInitialized();

            if (length <= 0)
            {
                return this.TMin;
            }

            if (length >= this.accumLengths[numSegments - 1])
            {
                return this.TMax;
            }

            //Array.BinarySearch(this.accumLengths, longitud);
            int key = 0;
            for (; key < numSegments; key++)
            {
                if (length < this.accumLengths[key])
                {
                    break;
                }
            }

            Debug.Assert(key < numSegments);
            if (key >= numSegments)
            {
                return this.TMax;
            }

            double len0;
            if (key == 0)
            {
                len0 = length;
            }
            else
            {
                len0 = length - this.accumLengths[key - 1];
            }
            double len1 = this.lengths[key];

            return this.GetTMin(key) + CurveUtils.FindTime(dt => this.GetLength(key, 0, dt), dt => this.GetSpeed(key, dt),
                                                          0, this.times[key + 1] - this.times[key],
                                                          len0, len1,
                                                          iterations, tolerance);
        }

        public virtual Point2d GetPosition(double t)
        {
            int key;
            double dt;
            this.GetKeyInfo(t, out key, out dt);

            return this.GetPosition(key, dt);
        }

        public virtual Vector2d GetFirstDerivative(double t)
        {
            int key;
            double dt;
            this.GetKeyInfo(t, out key, out dt);

            return this.GetFirstDerivative(key, dt);
        }

        public virtual Vector2d GetSecondDerivative(double t)
        {
            int key;
            double dt;
            this.GetKeyInfo(t, out key, out dt);

            return this.GetSecondDerivative(key, dt);
        }

        public virtual Vector2d GetThirdDerivative(double t)
        {
            int key;
            double dt;
            this.GetKeyInfo(t, out key, out dt);

            return this.GetThirdDerivative(key, dt);
        }

        public virtual double TotalLength
        {
            get
            {
                this.EnsureLengthsInitialized();
                return this.accumLengths[this.accumLengths.Length - 1];
            }
        }

        public virtual double GetLength(double t0, double t1)
        {
            Debug.Assert(this.TMin <= t0 && t0 <= this.TMax, "Invalid input\n");
            Debug.Assert(this.TMin <= t1 && t1 <= this.TMax, "Invalid input\n");
            Debug.Assert(t0 <= t1, "Invalid input\n");

            if (t0 < this.TMin)
            {
                t0 = this.TMin;
            }

            if (t1 > this.TMax)
            {
                t1 = this.TMax;
            }

            this.EnsureLengthsInitialized();

            int key0, key1;
            double dt0, dt1;
            this.GetKeyInfo(t0, out key0, out dt0);
            this.GetKeyInfo(t1, out key1, out dt1);

            double length;
            if (key0 != key1)
            {
                // Add on partial first segment.
                length = this.GetLength(key0, dt0, this.times[key0 + 1] - this.times[key0]);

                // NOTA: mas eficiente utilizar accLengths!
                // Accumulate full-segment lengths.
                for (int i = key0 + 1; i < key1; i++)
                {
                    length += this.lengths[i];
                }

                // Add on partial last segment.
                //if (key1 < this.NumSegments)
                length += this.GetLength(key1, 0, dt1);
            }
            else
            {
                length = this.GetLength(key0, dt0, dt1);
            }

            return length;
        }

        public virtual double GetSpeed(double t)
        {
            int key;
            double dt;
            this.GetKeyInfo(t, out key, out dt);

            return this.GetSpeed(key, dt);
        }

        public virtual double GetCurvature(double t)
        {
            int key;
            double dt;
            this.GetKeyInfo(t, out key, out dt);

            return this.GetCurvature(key, dt);
        }

        public virtual Vector2d GetTangent(double t)
        {
            return this.GetFirstDerivative(t).Unit;
        }

        public virtual Vector2d GetLeftNormal(double t)
        {
            int key;
            double dt;
            this.GetKeyInfo(t, out key, out dt);

            return this.GetLeftNormal(key, dt);
        }

        public virtual void GetFrame(double t, out Point2d position, out Vector2d tangent, out Vector2d normal)
        {
            int key;
            double dt;
            this.GetKeyInfo(t, out key, out dt);

            this.GetFrame(key, dt, out position, out tangent, out normal);
        }

        public BoundingBox2d BoundingBox
        {
            get
            {
                BoundingBox2d boundingBox = BoundingBox2d.Empty;
                for (int i = 0; i < this.NumSegments; i++)
                {
                    boundingBox = boundingBox.Union(this.GetBoundingBox(i));
                }
                return boundingBox;
            }
        }

        #endregion
    }
}