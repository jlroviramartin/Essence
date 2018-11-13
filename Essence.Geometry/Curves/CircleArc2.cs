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
using Essence.Geometry.Distances;
using Essence.Maths.Double;
using Essence.Util.Math.Double;
using SysMath = System.Math;

namespace Essence.Geometry.Curves
{
    public class CircleArc2 : BaseCircle2
    {
        /// <summary>
        /// Crea un arco de circunferencia:
        /// Si angle2 > angle1 el arco es CCW.
        /// Si angle2 &lt; angle1 el arco es CW.
        /// </summary>
        public CircleArc2(Point2d center, double radius, double angle1, double angle2)
            : base(center, radius)
        {
            Contract.Assert(System.Math.Abs(angle1) < 2.0 * System.Math.PI);
            Contract.Assert(System.Math.Abs(angle2) < 2.0 * System.Math.PI);
            Contract.Assert(System.Math.Abs(angle2 - angle1) <= 2.0 * System.Math.PI);

            this.Angle1 = angle1;
            this.Angle2 = angle2;

            this.SetTInterval(0, this.TotalLength);
        }

        public bool IsCCW
        {
            get { return this.Angle1 < this.Angle2; }
        }

        /// <summary>
        /// Initial angle.
        /// <![CDATA[ Angle1 in [ 0, 2 * PI ) ]]>
        /// </summary>
        public double Angle1 { get; }

        /// <summary>
        /// Initial angle.
        /// <![CDATA[ Angle2 in [ -2 * PI, 4 * PI ) ó Abs(Angle2 - Angle1) <= 2 * PI]]>
        /// </summary>
        public double Angle2 { get; }

        /// <summary>
        /// Advance angle.
        /// <![CDATA[ AdvAngle in [ -2 * PI, 2 * PI ] ]]>
        /// </summary>
        public double AdvAngle
        {
            get { return this.Angle2 - this.Angle1; }
        }

        public double Project(Point2d punto)
        {
            double distCuad;
            return this.Project(punto, out distCuad);
        }

        public double Project(Point2d punto, out double d2)
        {
            DistPointCircleArc2 distance = new DistPointCircleArc2()
            {
                Point = punto,
                Arc = this
            };

            // Se calcula el punto mas cercano.
            d2 = distance.CalcDistance2();

            double ca = distance.ClosestAngle;
            double ai = this.Angle1;
            double aa = this.AdvAngle;

            ai = AngleUtils.Ensure0To2Pi(ai);

            // Se comprueban los valores.
            Contract.Assert(ca.Between(0, 2 * SysMath.PI));
            Contract.Assert(ai.Between(0, 2 * SysMath.PI));
            Contract.Assert(aa.Between(-2 * SysMath.PI, 2 * SysMath.PI));

            if (aa >= 0)
            {
                if (ca < ai)
                {
                    ca += 2 * SysMath.PI;
                    Contract.Assert(ca >= ai);
                }
            }
            else
            {
                if (ca > ai)
                {
                    ca -= 2 * SysMath.PI;
                    Contract.Assert(ca <= ai);
                }
            }

            // Se calcula la estacion.
            return ((ca - ai) / aa);
        }

        public bool PointInCurve(Point2d p)
        {
            Vector2d vector2d = p - this.Center;
            if (!vector2d.Length.EpsilonEquals(this.Radius, 0.01))
            {
                return false;
            }
            double num1 = AngleUtils.Ensure0To2Pi(vector2d.Angle, false);
            double num2 = AngleUtils.Ensure0To2Pi(this.Angle1, false);
            double num3 = this.Angle2 - this.Angle1;
            if (num3 > 0.0)
            {
                return num1 >= num2 && num1 <= num2 + num3;
            }
            else
            {
                return num1 >= num2 + num3 && num1 <= num2;
            }
        }

        #region private

        #endregion

        #region ICurve2

        public override void SetTInterval(double tmin, double tmax)
        {
            this.tmin = tmin;
            this.tmax = tmax;
            this.ttransform = new Transform1(this.TMin, this.TMax, this.Angle1, this.Angle2);
        }

        public override double TotalLength
        {
            get { return System.Math.Abs(this.Angle2 - this.Angle1) * this.Radius; }
        }

        public override BoundingBox2d BoundingBox
        {
            get { return BoundingBox2d.FromCoords(this.Center.X - this.Radius, this.Center.Y - this.Radius, this.Center.X + this.Radius, this.Center.Y + this.Radius); }
        }

        #endregion
    }
}