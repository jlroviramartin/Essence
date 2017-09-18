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
using System.Linq;
using Essence.Geometry.Core.Double;

namespace Essence.Geometry.Core
{
    public static class TransformUtils
    {
        public static Vector2d DoTransform(this ITransform2D transform, Vector2d v)
        {
            return transform.Transform(v).ToVector2d();
        }

        public static Point2d DoTransform(this ITransform2D transform, Point2d p)
        {
            return transform.Transform(p).ToPoint2d();
        }

        public static BoundingBox2d DoTransform(this ITransform2D transform, BoundingBox2d bbox)
        {
            return BoundingBox2d.UnionOfPoints(bbox.GetVertices().Select(v => transform.DoTransform(v)));
        }

        public static Matrix2x3d ToMatrix(this ITransform2D transform)
        {
            if (transform is Transform2DIdentity)
            {
                return Matrix2x3d.Identity;
            }
            if (transform is Transform2DMatrix)
            {
                return ((Transform2DMatrix)transform).Matrix;
            }
            throw new NotSupportedException();
        }

        public static Vector3d DoTransform(this ITransform3D transform, Vector3d v)
        {
            return transform.Transform(v).ToVector3d();
        }

        public static Point3d DoTransform(this ITransform3D transform, Point3d p)
        {
            return transform.Transform(p).ToPoint3d();
        }

        public static BoundingBox3d DoTransform(this ITransform3D transform, BoundingBox3d bbox)
        {
            return BoundingBox3d.Union(bbox.GetVertices().Select(v => transform.DoTransform(v)));
        }

        public static Matrix4x4d ToMatrix(this ITransform3D transform)
        {
            if (transform is Transform3DIdentity)
            {
                return Matrix4x4d.Identity;
            }
            if (transform is Transform3DMatrix)
            {
                return ((Transform3DMatrix)transform).Matrix;
            }
            throw new NotSupportedException();
        }
    }
}