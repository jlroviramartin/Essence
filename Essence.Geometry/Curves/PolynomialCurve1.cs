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
using System.Collections.Generic;
using Essence.Geometry.Core.Double;
using Essence.Maths;
using Essence.Maths.Double;
using Essence.Util.Math.Double;

namespace Essence.Geometry.Curves
{
    public class PolynomialCurve1 : SimpleCurve1
    {
        public PolynomialCurve1(Polynomial poly, double hlen, double t0, double t1)
        {
            this.poly = poly;
            this.hlen = hlen;
            this.SetTInterval(t0, t1);
        }

        #region private

        private double GetT0L(double t)
        {
            t = t.Clamp(this.TMin, this.TMax);
            double t01 = this.ttransform.Get(t);
            return t01;
        }

        private Polynomial GetPoly(int i)
        {
            switch (i)
            {
                case 0:
                    return this.poly;
                case 1:
                    if (this.poly1 == null)
                    {
                        this.poly1 = this.GetPoly(0).Derivative();
                    }
                    return this.poly1;
                case 2:
                    if (this.poly2 == null)
                    {
                        this.poly2 = this.GetPoly(1).Derivative();
                    }
                    return this.poly2;
                case 3:
                    if (this.poly3 == null)
                    {
                        this.poly3 = this.GetPoly(2).Derivative();
                    }
                    return this.poly3;
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        private double tmin;
        private double tmax;

        /// <summary>Transformacion que se aplica sobre el parametro.</summary>
        private Transform1 ttransform;

        private readonly Polynomial poly;
        private readonly double hlen;
        private Polynomial poly1;
        private Polynomial poly2;
        private Polynomial poly3;

        #endregion

        #region ICurve1

        public override double TMin
        {
            get { return this.tmin; }
        }

        public override double TMax
        {
            get { return this.tmax; }
        }

        public override void SetTInterval(double tmin, double tmax)
        {
            this.tmin = tmin;
            this.tmax = tmax;
            this.ttransform = new Transform1(this.tmin, this.tmax, 0, this.hlen);
        }

        public override double GetPosition(double t)
        {
            double tt = this.GetT0L(t);
            return this.GetPoly(0).Evaluate(tt);
        }

        public override double GetFirstDerivative(double t)
        {
            double tt = this.GetT0L(t);
            return this.GetPoly(1).Evaluate(tt);
        }

        public override double GetSecondDerivative(double t)
        {
            double tt = this.GetT0L(t);
            return this.GetPoly(2).Evaluate(tt);
        }

        public override double GetThirdDerivative(double t)
        {
            double tt = this.GetT0L(t);
            return this.GetPoly(3).Evaluate(tt);
        }

        public override BoundingBox1d BoundingBox
        {
            get
            {
                Func<int, Func<double, double>> nthf = (i => this.GetPoly(i + 1).Evaluate);
                double t0L1 = this.GetT0L(this.TMin);
                double t0L2 = this.GetT0L(this.TMax);
                int maxzeros = this.poly.Degree - 1;
                List<double> doubleList = new List<double>();
                Solver.SolveMulti(nthf, t0L1, t0L2, maxzeros, doubleList, Solver.Type.Brent, 1000);
                double xMin = double.MaxValue;
                double xMax = double.MinValue;
                foreach (double x in doubleList)
                {
                    double num = this.GetPoly(0).Evaluate(x);
                    if (num < xMin)
                    {
                        xMin = num;
                    }
                    if (num > xMax)
                    {
                        xMax = num;
                    }
                }
                return new BoundingBox1d(xMin, xMax);
            }
        }

        #endregion
    }
}