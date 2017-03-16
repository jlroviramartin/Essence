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

using Essence.Geometry.Core.Double;
using SysMath = System.Math;

namespace Essence.Geometry.Core
{
    public abstract class Transform2D : ITransform2D
    {
        public abstract IVector2D Transform(IVector2D v);

        public abstract IPoint2D Transform(IPoint2D p);

        public abstract ITransform2D Concat(ITransform2D transform);

        public abstract ITransform2D Inv { get; }

        public abstract bool IsIdentity { get; }

        public static Transform2D Identity()
        {
            return new Transform2DMatrix(
                1, 0, 0,
                0, 1, 0);
        }

        public static Transform2D Rotate(double r)
        {
            double c = SysMath.Cos(r);
            double s = SysMath.Sin(r);
            return new Transform2DMatrix(
                c, -s, 0,
                s, c, 0);
        }

        public static Transform2D Rotate(Vector2d pt, double r)
        {
            return Rotate(pt.X, pt.Y, r);
        }

        public static Transform2D Rotate(double px, double py, double r)
        {
            double c = SysMath.Cos(r);
            double s = SysMath.Sin(r);
            return new Transform2DMatrix(
                c, -s, -px * c + py * s + px,
                s, c, -px * s - py * c + py);
        }

        public static Transform2D Translate(Vector2d t)
        {
            return Translate(t.X, t.Y);
        }

        public static Transform2D Translate(double dx, double dy)
        {
            return new Transform2DMatrix(
                1, 0, dx,
                0, 1, dy);
        }

        public static Transform2D Translate(double px, double py, double px2, double py2)
        {
            return Translate(px2 - px, py2 - py);
        }

        public static Transform2D Scale(double ex, double ey)
        {
            return new Transform2DMatrix(
                ex, 0, 0,
                0, ey, 0);
        }

        public static Transform2D Scale(double px, double py, double ex, double ey)
        {
            return new Transform2DMatrix(
                ex, 0, px - ex * px,
                0, ey, py - ey * py);
        }
    }
}