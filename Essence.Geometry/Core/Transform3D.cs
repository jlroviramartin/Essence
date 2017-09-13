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
    public abstract class Transform3D : ITransform3D
    {
        public abstract IVector3D Transform(IVector3D v);

        public abstract IPoint3D Transform(IPoint3D p);

        public abstract ITransform3D Concat(ITransform3D transform);

        public abstract ITransform3D Inv { get; }

        public abstract bool IsIdentity { get; }

        public abstract void GetMatrix(Matrix4x4d matrix);

        public static Transform3D Identity()
        {
            return new Transform3DMatrix(
                1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1);
        }

        public static Transform3D Translate(Vector3d t)
        {
            return Translate(t.X, t.Y, t.Z);
        }

        public static Transform3D Translate(double dx, double dy, double dz)
        {
            return new Transform3DMatrix(Matriz4x4dUtils.Translate(dx, dy, dz));
        }

        public static Transform3D Translate(double px, double py, double pz, double px2, double py2, double pz2)
        {
            return Translate(px2 - px, py2 - py, pz2 - pz);
        }

        public static Transform3D Scale(double ex, double ey, double ez)
        {
            return new Transform3DMatrix(Matriz4x4dUtils.Scale(ex, ey, ez));
        }

        public static Transform3D Scale(double px, double py, double pz, double ex, double ey, double ez)
        {
            return new Transform3DMatrix(Matriz4x4dUtils.Scale(px, py, pz, ex, ey, ez));
        }
    }
}