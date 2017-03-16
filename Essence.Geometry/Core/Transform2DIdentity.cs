#region License

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

#endregion

namespace Essence.Geometry.Core
{
    public sealed class Transform2DIdentity : Transform2D
    {
        public static readonly Transform2DIdentity Instance = new Transform2DIdentity();

        public override IVector2D Transform(IVector2D v)
        {
            return v;
        }

        public override IPoint2D Transform(IPoint2D p)
        {
            return p;
        }

        public override ITransform2D Concat(ITransform2D transform)
        {
            return transform;
        }

        public override ITransform2D Inv
        {
            get { return this; }
        }

        public override bool IsIdentity
        {
            get { return true; }
        }
    }
}