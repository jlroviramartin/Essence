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

namespace Essence.Geometry.Core
{
    public abstract class Transform3 : ITransform3
    {
        public abstract Vector3d Transform(Vector3d v);

        public abstract Point3d Transform(Point3d v);

        #region ITransform3

        public virtual IVector3 Transform(IVector3 v)
        {
            return this.Transform(v.ToVector3d());
        }

        public virtual void Transform(IVector3 v, IOpVector3 vout)
        {
            IOpTuple3_Double _vout = vout.AsOpTupleDouble();

            Vector3d aux = this.Transform(v.ToVector3d());
            _vout.Set(aux.X, aux.Y, aux.Z);
        }

        public virtual IPoint3 Transform(IPoint3 p)
        {
            return this.Transform(p.ToPoint3d());
        }

        public virtual void Transform(IPoint3 p, IOpPoint3 pout)
        {
            IOpTuple3_Double _vout = pout.AsOpTupleDouble();

            Vector3d aux = this.Transform(p.ToPoint3d());
            _vout.Set(aux.X, aux.Y, aux.Z);
        }

        public abstract ITransform3 Concat(ITransform3 transform);

        public abstract ITransform3 Inv { get; }

        public abstract bool IsIdentity { get; }

        public abstract void GetMatrix(Matrix4x4d matrix);

        #endregion

        public static Transform3 Identity()
        {
            return new Transform3Matrix(
                1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1);
        }

        public static Transform3 Translate(Vector3d t)
        {
            return Translate(t.X, t.Y, t.Z);
        }

        public static Transform3 Translate(double dx, double dy, double dz)
        {
            return new Transform3Matrix(Matrix4x4dUtils.Translate(dx, dy, dz));
        }

        public static Transform3 Translate(double px, double py, double pz, double px2, double py2, double pz2)
        {
            return Translate(px2 - px, py2 - py, pz2 - pz);
        }

        public static Transform3 Scale(double ex, double ey, double ez)
        {
            return new Transform3Matrix(Matrix4x4dUtils.Scale(ex, ey, ez));
        }

        public static Transform3 Scale(double px, double py, double pz, double ex, double ey, double ez)
        {
            return new Transform3Matrix(Matrix4x4dUtils.Scale(px, py, pz, ex, ey, ez));
        }
    }
}