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
using Essence.Geometry.Core.Double;
using Essence.Geometry.Core.Int;

namespace Essence.Geometry.Core
{
    public static class PointUtils
    {
        #region Point2d

        public static Point2i Round(this Point2d p)
        {
            return new Point2i((int)Math.Round(p.X), (int)Math.Round(p.Y));
        }

        public static Point2i Ceiling(this Point2d p)
        {
            return new Point2i((int)Math.Ceiling(p.X), (int)Math.Ceiling(p.Y));
        }

        public static Point2i Floor(this Point2d p)
        {
            return new Point2i((int)Math.Floor(p.X), (int)Math.Floor(p.Y));
        }

        #endregion

        #region IVector

        public static Vector2d ToVector2d(this IVector p)
        {
            if (p is Vector2d)
            {
                return (Vector2d)p;
            }
            return new Vector2d(p);
        }

        public static Vector3d ToVector3d(this IVector p)
        {
            if (p is Vector3d)
            {
                return (Vector3d)p;
            }
            return new Vector3d(p);
        }

        public static Vector4d ToVector4d(this IVector p)
        {
            if (p is Vector4d)
            {
                return (Vector4d)p;
            }
            return new Vector4d(p);
        }

        #endregion

        #region IVector2D

        public static Vector2d ToVector2d(this IVector2D p)
        {
            if (p is Vector2d)
            {
                return (Vector2d)p;
            }
            return new Vector2d(p);
        }

        public static Vector3d ToVector3d(this IVector3D p)
        {
            if (p is Vector3d)
            {
                return (Vector3d)p;
            }
            return new Vector3d(p);
        }

        public static Vector4d ToVector4d(this IVector4D p)
        {
            if (p is Vector4d)
            {
                return (Vector4d)p;
            }
            return new Vector4d(p);
        }

        #endregion

        #region IPoint

        public static Point2d ToPoint2d(this IPoint p)
        {
            if (p is Point2d)
            {
                return (Point2d)p;
            }
            return new Point2d(p);
        }

        public static Point2i ToPoint2i(this IPoint p)
        {
            if (p is Point2i)
            {
                return (Point2i)p;
            }
            return new Point2i(p);
        }

        public static Point3d ToPoint3d(this IPoint p)
        {
            if (p is Point3d)
            {
                return (Point3d)p;
            }
            return new Point3d(p);
        }

        public static Point4d ToPoint4d(this IPoint p)
        {
            if (p is Point4d)
            {
                return (Point4d)p;
            }
            return new Point4d(p);
        }

        #endregion

        #region IPoint2D

        public static Point2d ToPoint2d(this IPoint2D p)
        {
            if (p is Point2d)
            {
                return (Point2d)p;
            }
            return new Point2d(p);
        }

        public static Point2i ToPoint2i(this IPoint2D p)
        {
            if (p is Point2i)
            {
                return (Point2i)p;
            }
            return new Point2i(p);
        }

        public static Point3d ToPoint3d(this IPoint3D p)
        {
            if (p is Point3d)
            {
                return (Point3d)p;
            }
            return new Point3d(p);
        }

        public static Point4d ToPoint4d(this IPoint4D p)
        {
            if (p is Point4d)
            {
                return (Point4d)p;
            }
            return new Point4d(p);
        }

        #endregion
    }
}