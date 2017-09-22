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
    public sealed class Transform3DIdentity : Transform3D
    {
        public static readonly Transform3DIdentity Instance = new Transform3DIdentity();

        public override Vector3d Transform(Vector3d v)
        {
            return v;
        }

        public override Point3d Transform(Point3d p)
        {
            return p;
        }

        public override ITransform3D Concat(ITransform3D transform)
        {
            return transform;
        }

        public override ITransform3D Inv
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
    }
}