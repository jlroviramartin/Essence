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

using Essence.Geometry.Core.Double;
using SysMath = System.Math;

namespace Essence.Geometry.Curves
{
    public class Circle2 : BaseCircle2
    {
        public Circle2(Point2d center, double radius)
            : base(center, radius)
        {
            this.SetTInterval(0, this.TotalLength);
        }

        #region Curve2

        public override bool IsClosed
        {
            get { return true; }
        }

        public override double TotalLength
        {
            get { return 2 * SysMath.PI * this.Radius; }
        }

        public override BoundingBox2d BoundingBox
        {
            get { return BoundingBox2d.FromCoords(this.Center.X - this.Radius, this.Center.Y - this.Radius, this.Center.X + this.Radius, this.Center.Y + this.Radius); }
        }

        #endregion
    }
}