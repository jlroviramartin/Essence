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

namespace Essence.Geometry.Core
{
    public sealed class Transform2Identity : Transform2
    {
        public static readonly Transform2Identity Instance = new Transform2Identity();

        #region Transform2

        public override Vector2d Transform(Vector2d v)
        {
            return v;
        }

        public override Point2d Transform(Point2d p)
        {
            return p;
        }

        #endregion

        #region ITransform2

        public override IVector2 Transform(IVector2 v)
        {
            return v;
        }

        public override void Transform(IVector2 v, IOpVector2 vout)
        {
            ITuple2_Double _v = v.AsTupleDouble();
            IOpTuple2_Double _vout = vout.AsOpTupleDouble();

            _vout.Set(_v.X, _v.Y);
        }

        public override IPoint2 Transform(IPoint2 p)
        {
            return p;
        }

        public override void Transform(IPoint2 p, IOpPoint2 pout)
        {
            ITuple2_Double _v = p.AsTupleDouble();
            IOpTuple2_Double _vout = pout.AsOpTupleDouble();

            _vout.Set(_v.X, _v.Y);
        }

        public override ITransform2 Concat(ITransform2 transform)
        {
            return transform;
        }

        public override ITransform2 Inv
        {
            get { return this; }
        }

        public override bool IsIdentity
        {
            get { return true; }
        }

        public override void GetMatrix(Matrix3x3d matrix)
        {
            matrix.SetIdentity();
        }

        #endregion
    }
}