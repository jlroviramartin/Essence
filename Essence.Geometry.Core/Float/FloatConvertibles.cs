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

using ITuple2_Double = Essence.Geometry.Core.Double.ITuple2_Double;
using ITuple2_Int = Essence.Geometry.Core.Int.ITuple2_Int;
using ITuple2_Byte = Essence.Geometry.Core.Byte.ITuple2_Byte;
using ITuple3_Double = Essence.Geometry.Core.Double.ITuple3_Double;
using ITuple3_Byte = Essence.Geometry.Core.Byte.ITuple3_Byte;
using ITuple4_Double = Essence.Geometry.Core.Double.ITuple4_Double;
using ITuple4_Byte = Essence.Geometry.Core.Byte.ITuple4_Byte;

namespace Essence.Geometry.Core.Float
{
    public static class FloatConvertibles
    {
        public static ITuple3_Float AsTupleFloat(this IColor3 v)
        {
            ITuple3_Float ret = v as ITuple3_Float;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<IColor3, ITuple3_Float>(v);
        }

        public static ITuple4_Float AsTupleFloat(this IColor4 v)
        {
            ITuple4_Float ret = v as ITuple4_Float;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<IColor4, ITuple4_Float>(v);
        }

        private static byte ToColorByte(float v)
        {
            return (byte)Essence.Util.Math.Float.MathUtils.Clamp(v * byte.MaxValue, 0, byte.MaxValue);
        }

        [ConvertHelper(SourceType1 = typeof(ITuple2_Float), SourceType2 = typeof(IVector2), DestinationType = typeof(ITuple2_Double))]
        [ConvertHelper(SourceType1 = typeof(ITuple2_Float), SourceType2 = typeof(IVector2), DestinationType = typeof(ITuple2_Int))]
        [ConvertHelper(SourceType1 = typeof(ITuple2_Float), SourceType2 = typeof(IVector2), DestinationType = typeof(ITuple2_Byte))]
        [ConvertHelper(SourceType1 = typeof(ITuple2_Float), SourceType2 = typeof(IPoint2), DestinationType = typeof(ITuple2_Double))]
        [ConvertHelper(SourceType1 = typeof(ITuple2_Float), SourceType2 = typeof(IPoint2), DestinationType = typeof(ITuple2_Int))]
        [ConvertHelper(SourceType1 = typeof(ITuple2_Float), SourceType2 = typeof(IPoint2), DestinationType = typeof(ITuple2_Byte))]
        public sealed class Convertible2 : ITuple2_Double, ITuple2_Int, ITuple2_Byte
        {
            public Convertible2(ITuple2_Float inner)
            {
                this.inner = inner;
            }

            private readonly ITuple2_Float inner;

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

            #region ITuple2_Int

            int ITuple2_Int.X
            {
                get { return (int)this.inner.X; }
            }

            int ITuple2_Int.Y
            {
                get { return (int)this.inner.Y; }
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

        [ConvertHelper(SourceType1 = typeof(ITuple3_Float), SourceType2 = typeof(IVector3), DestinationType = typeof(ITuple3_Double))]
        [ConvertHelper(SourceType1 = typeof(ITuple3_Float), SourceType2 = typeof(IVector3), DestinationType = typeof(ITuple3_Byte))]
        [ConvertHelper(SourceType1 = typeof(ITuple3_Float), SourceType2 = typeof(IPoint3), DestinationType = typeof(ITuple3_Double))]
        [ConvertHelper(SourceType1 = typeof(ITuple3_Float), SourceType2 = typeof(IPoint3), DestinationType = typeof(ITuple3_Byte))]
        public sealed class Convertible3 : ITuple3_Double, ITuple3_Byte
        {
            public Convertible3(ITuple3_Float inner)
            {
                this.inner = inner;
            }

            private readonly ITuple3_Float inner;

            #region ITuple3_Double

            double ITuple3_Double.X
            {
                get { return (double)this.inner.X; }
            }

            double ITuple3_Double.Y
            {
                get { return (double)this.inner.Y; }
            }

            double ITuple3_Double.Z
            {
                get { return (double)this.inner.Z; }
            }

            #endregion

            #region ITuple3_Byte

            byte ITuple3_Byte.X
            {
                get { return (byte)this.inner.X; }
            }

            byte ITuple3_Byte.Y
            {
                get { return (byte)this.inner.Y; }
            }

            byte ITuple3_Byte.Z
            {
                get { return (byte)this.inner.Z; }
            }

            #endregion
        }

        [ConvertHelper(SourceType1 = typeof(ITuple4_Float), SourceType2 = typeof(IVector4), DestinationType = typeof(ITuple4_Double))]
        [ConvertHelper(SourceType1 = typeof(ITuple4_Float), SourceType2 = typeof(IVector4), DestinationType = typeof(ITuple4_Byte))]
        [ConvertHelper(SourceType1 = typeof(ITuple4_Float), SourceType2 = typeof(IPoint4), DestinationType = typeof(ITuple4_Double))]
        [ConvertHelper(SourceType1 = typeof(ITuple4_Float), SourceType2 = typeof(IPoint4), DestinationType = typeof(ITuple4_Byte))]
        public sealed class Convertible4 : ITuple4_Double, ITuple4_Byte
        {
            public Convertible4(ITuple4_Float inner)
            {
                this.inner = inner;
            }

            private readonly ITuple4_Float inner;

            #region ITuple4_Double

            double ITuple4_Double.X
            {
                get { return (double)this.inner.X; }
            }

            double ITuple4_Double.Y
            {
                get { return (double)this.inner.Y; }
            }

            double ITuple4_Double.Z
            {
                get { return (double)this.inner.Z; }
            }

            double ITuple4_Double.W
            {
                get { return (double)this.inner.W; }
            }

            #endregion

            #region ITuple4_Byte

            byte ITuple4_Byte.X
            {
                get { return (byte)this.inner.X; }
            }

            byte ITuple4_Byte.Y
            {
                get { return (byte)this.inner.Y; }
            }

            byte ITuple4_Byte.Z
            {
                get { return (byte)this.inner.Z; }
            }

            byte ITuple4_Byte.W
            {
                get { return (byte)this.inner.W; }
            }

            #endregion
        }

        [ConvertHelper(SourceType1 = typeof(ITuple3_Float), SourceType2 = typeof(IColor3), DestinationType = typeof(ITuple3_Byte))]
        public sealed class ColorConvertible3 : ITuple3_Byte
        {
            public ColorConvertible3(ITuple3_Float inner)
            {
                this.inner = inner;
            }

            private readonly ITuple3_Float inner;

            #region ITuple3_Byte

            byte ITuple3_Byte.X
            {
                get { return ToColorByte(this.inner.X); }
            }

            byte ITuple3_Byte.Y
            {
                get { return ToColorByte(this.inner.Y); }
            }

            byte ITuple3_Byte.Z
            {
                get { return ToColorByte(this.inner.Z); }
            }

            #endregion
        }

        [ConvertHelper(SourceType1 = typeof(ITuple4_Float), SourceType2 = typeof(IColor4), DestinationType = typeof(ITuple4_Byte))]
        public sealed class ColorConvertible4 : ITuple4_Byte
        {
            public ColorConvertible4(ITuple4_Float inner)
            {
                this.inner = inner;
            }

            private readonly ITuple4_Float inner;

            #region ITuple4_Byte

            byte ITuple4_Byte.X
            {
                get { return ToColorByte(this.inner.X); }
            }

            byte ITuple4_Byte.Y
            {
                get { return ToColorByte(this.inner.Y); }
            }

            byte ITuple4_Byte.Z
            {
                get { return ToColorByte(this.inner.Z); }
            }

            byte ITuple4_Byte.W
            {
                get { return ToColorByte(this.inner.W); }
            }

            #endregion
        }
    }
}