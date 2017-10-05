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
    public sealed class Transform3Identity : Transform3
    {
        public static readonly Transform3Identity Instance = new Transform3Identity();

        #region Transform3

        public override Vector3d Transform(Vector3d v)
        {
            return v;
        }

        public override Point3d Transform(Point3d p)
        {
            return p;
        }

        #endregion

        #region ITransform3

        public override IVector3 Transform(IVector3 v)
        {
            return v;
        }

        public override void Transform(IVector3 v, IOpVector3 vout)
        {
            ITuple3_Double _v = v.AsTupleDouble();
            IOpTuple3_Double _vout = vout.AsOpTupleDouble();

            _vout.Set(_v.X, _v.Y, _v.Z);
        }

        public override IPoint3 Transform(IPoint3 p)
        {
            return p;
        }

        public override void Transform(IPoint3 p, IOpPoint3 pout)
        {
            ITuple3_Double _v = p.AsTupleDouble();
            IOpTuple3_Double _vout = pout.AsOpTupleDouble();

            _vout.Set(_v.X, _v.Y, _v.Y);
        }

        public override ITransform3 Concat(ITransform3 transform)
        {
            return transform;
        }

        public override ITransform3 Inv
        {
            get { return this; }
        }

        public override bool IsIdentity
        {
            get { return true; }
        }

        public override void GetMatrix(Matrix4x4d matrix)
        {
            matrix.SetIdentity();
        }

        #endregion
    }
}