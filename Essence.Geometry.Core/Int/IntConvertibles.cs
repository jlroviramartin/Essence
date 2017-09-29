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
using ITuple2_Float = Essence.Geometry.Core.Float.ITuple2_Float;
using ITuple2_Double = Essence.Geometry.Core.Double.ITuple2_Double;
using ITuple2_Int = Essence.Geometry.Core.Int.ITuple2_Int;
using ITuple2_Byte = Essence.Geometry.Core.Byte.ITuple2_Byte;

using Vector2d = Essence.Geometry.Core.Double.Vector2d;
using BuffVector2d = Essence.Geometry.Core.Double.BuffVector2d;
using Point2d = Essence.Geometry.Core.Double.Point2d;
using BuffPoint2d = Essence.Geometry.Core.Double.BuffPoint2d;

namespace Essence.Geometry.Core.Int
{
    public static class IntConvertibles
    {
        public static ITuple2_Int AsTupleInt(this IPoint2 v)
        {
            ITuple2_Int ret = v as ITuple2_Int;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<IPoint2, ITuple2_Int>(v);
        }

        public static ITuple2_Int AsTupleInt(this IVector2 v)
        {
            ITuple2_Int ret = v as ITuple2_Int;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<IVector2, ITuple2_Int>(v);
        }

        [ConvertHelper(SourceType1 = typeof(ITuple2_Int), SourceType2 = typeof(IVector2), DestinationType = typeof(ITuple2_Double))]
        [ConvertHelper(SourceType1 = typeof(ITuple2_Int), SourceType2 = typeof(IVector2), DestinationType = typeof(ITuple2_Float))]
        [ConvertHelper(SourceType1 = typeof(ITuple2_Int), SourceType2 = typeof(IVector2), DestinationType = typeof(ITuple2_Byte))]
        [ConvertHelper(SourceType1 = typeof(ITuple2_Int), SourceType2 = typeof(IPoint2), DestinationType = typeof(ITuple2_Double))]
        [ConvertHelper(SourceType1 = typeof(ITuple2_Int), SourceType2 = typeof(IPoint2), DestinationType = typeof(ITuple2_Float))]
        [ConvertHelper(SourceType1 = typeof(ITuple2_Int), SourceType2 = typeof(IPoint2), DestinationType = typeof(ITuple2_Byte))]
        public sealed class Convertible2 : ITuple2_Double, ITuple2_Float, ITuple2_Byte
        {
            public Convertible2(ITuple2_Int inner)
            {
                this.inner = inner;
            }

            private readonly ITuple2_Int inner;

            #region ITuple2_Double

            double ITuple2_Double.X
            {
                get { return (double)this.inner.X; }
            }

            double ITuple2_Double.Y
            {
                get { return (double)this.inner.Y; }
            }

            #endregion

            #region ITuple2_Float

            float ITuple2_Float.X
            {
                get { return (float)this.inner.X; }
            }

            float ITuple2_Float.Y
            {
                get { return (float)this.inner.Y; }
            }

            #endregion

            #region ITuple2_Byte

            byte ITuple2_Byte.X
            {
                get { return (byte)this.inner.X; }
            }

            byte ITuple2_Byte.Y
            {
                get { return (byte)this.inner.Y; }
            }

            #endregion
        }
    }
}