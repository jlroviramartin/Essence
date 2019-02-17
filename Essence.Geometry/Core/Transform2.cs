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
    public abstract class Transform2 : ITransform2
    {
        public abstract Vector2d Transform(Vector2d v);

        public abstract Point2d Transform(Point2d v);

        public Transform2 Concat(Transform2 transform)
        {
            return (Transform2)this.Concat((ITransform2)transform);
        }

        #region ITransform2

        public virtual IVector2 Transform(IVector2 v)
        {
            return this.Transform(v.ToVector2d());
        }

        public virtual void Transform(IVector2 v, IOpVector2 vout)
        {
            IOpTuple2_Double _vout = vout.AsOpTupleDouble();

            Vector2d aux = this.Transform(v.ToVector2d());
            _vout.Set(aux.X, aux.Y);
        }

        public virtual IPoint2 Transform(IPoint2 p)
        {
            return this.Transform(p.ToPoint2d());
        }

        public virtual void Transform(IPoint2 p, IOpPoint2 pout)
        {
            IOpTuple2_Double _vout = pout.AsOpTupleDouble();

            Vector2d aux = this.Transform(p.ToPoint2d());
            _vout.Set(aux.X, aux.Y);
        }

        public abstract ITransform2 Concat(ITransform2 transform);

        public abstract ITransform2 Inv { get; }

        public abstract bool IsIdentity { get; }

        public abstract void GetMatrix(Matrix3x3d matrix);

        #endregion

        public static Transform2 Identity()
        {
            return new Transform2Matrix(
                1, 0, 0,
                0, 1, 0);
        }

        public static Transform2 Rotate(double r)
        {
            double c = SysMath.Cos(r);
            double s = SysMath.Sin(r);
            return new Transform2Matrix(
                c, -s, 0,
                s, c, 0);
        }

        public static Transform2 Rotate(Vector2d pt, double r)
        {
            return Rotate(pt.X, pt.Y, r);
        }

        public static Transform2 Rotate(double px, double py, double r)
        {
            double c = SysMath.Cos(r);
            double s = SysMath.Sin(r);
            return new Transform2Matrix(
                c, -s, -px * c + py * s + px,
                s, c, -px * s - py * c + py);
        }

        public static Transform2 Translate(Vector2d t)
        {
            return Translate(t.X, t.Y);
        }

        public static Transform2 Translate(double dx, double dy)
        {
            return new Transform2Matrix(
                1, 0, dx,
                0, 1, dy);
        }

        public static Transform2 Translate(double px, double py, double px2, double py2)
        {
            return Translate(px2 - px, py2 - py);
        }

        public static Transform2 Scale(double e)
        {
            return Scale(e, e);
        }

        public static Transform2 Scale(double ex, double ey)
        {
            return new Transform2Matrix(
                ex, 0, 0,
                0, ey, 0);
        }

        public static Transform2 Scale(double px, double py, double ex, double ey)
        {
            return new Transform2Matrix(
                ex, 0, px - ex * px,
                0, ey, py - ey * py);
        }

        public static Transform2 MirrorX()
        {
            return new Transform2Matrix(
                -1, 0, 0,
                0, 1, 0);
        }

        public static Transform2 MirrorY()
        {
            return new Transform2Matrix(
                1, 0, 0,
                0, -1, 0);
        }

        public static Transform2 Transform(Point2d source1, Point2d source2, Point2d target1, Point2d target2)
        {
            double sourceLen = source1.DistanceTo(source2);
            double targetLen = target1.DistanceTo(target2);

            Vector2d vsource = source2 - source1;
            Vector2d vtarget = target2 - target1;

            double angle = vsource.AngleTo(vtarget);

            return Transform2.Translate(target1.X, target1.Y)
                             .Concat(Transform2.Scale(targetLen / sourceLen))
                             .Concat(Transform2.Rotate(angle))
                             .Concat(Transform2.Translate(-source1.X, -source1.Y));
        }
    }
}