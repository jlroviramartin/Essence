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
using ITuple2_Double = Essence.Geometry.Core.Double.ITuple2_Double;
using ITuple2_Float = Essence.Geometry.Core.Float.ITuple2_Float;
using ITuple2_Int = Essence.Geometry.Core.Int.ITuple2_Int;
using ITuple2_Byte = Essence.Geometry.Core.Byte.ITuple2_Byte;
using ITuple3_Double = Essence.Geometry.Core.Double.ITuple3_Double;
using ITuple3_Float = Essence.Geometry.Core.Float.ITuple3_Float;
using ITuple3_Byte = Essence.Geometry.Core.Byte.ITuple3_Byte;
using ITuple4_Double = Essence.Geometry.Core.Double.ITuple4_Double;
using ITuple4_Float = Essence.Geometry.Core.Float.ITuple4_Float;
using ITuple4_Byte = Essence.Geometry.Core.Byte.ITuple4_Byte;

namespace Essence.Geometry.Core.Byte
{
    public static class ByteConvertibles
    {
        public static ITuple3_Byte AsTupleByte(this IColor3 v)
        {
            ITuple3_Byte ret = v as ITuple3_Byte;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<ITuple3_Byte>(v);
        }

        public static ITuple4_Byte AsTupleByte(this IColor4 v)
        {
            ITuple4_Byte ret = v as ITuple4_Byte;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<ITuple4_Byte>(v);
        }

        private static double toColorDouble(byte v)
        {
            return (double)v / (double)byte.MaxValue;
        }

        private static float toColorFloat(byte v)
        {
            return (float)v / (float)byte.MaxValue;
        }

        [ConvertHelper(SourceType1 = typeof(ITuple2_Byte), SourceType2 = typeof(IVector2), DestinationType = typeof(ITuple2_Double))]
        [ConvertHelper(SourceType1 = typeof(ITuple2_Byte), SourceType2 = typeof(IVector2), DestinationType = typeof(ITuple2_Float))]
        [ConvertHelper(SourceType1 = typeof(ITuple2_Byte), SourceType2 = typeof(IVector2), DestinationType = typeof(ITuple2_Int))]
        [ConvertHelper(SourceType1 = typeof(ITuple2_Byte), SourceType2 = typeof(IPoint2), DestinationType = typeof(ITuple2_Double))]
        [ConvertHelper(SourceType1 = typeof(ITuple2_Byte), SourceType2 = typeof(IPoint2), DestinationType = typeof(ITuple2_Float))]
        [ConvertHelper(SourceType1 = typeof(ITuple2_Byte), SourceType2 = typeof(IPoint2), DestinationType = typeof(ITuple2_Int))]
        public sealed class Convertible2 : ITuple2_Double, ITuple2_Float, ITuple2_Int
        {
            public Convertible2(ITuple2_Byte inner)
            {
                this.inner = inner;
            }

            private readonly ITuple2_Byte inner;

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
        }

        [ConvertHelper(SourceType1 = typeof(ITuple3_Byte), SourceType2 = typeof(IVector3), DestinationType = typeof(ITuple3_Double))]
        [ConvertHelper(SourceType1 = typeof(ITuple3_Byte), SourceType2 = typeof(IVector3), DestinationType = typeof(ITuple3_Float))]
        [ConvertHelper(SourceType1 = typeof(ITuple3_Byte), SourceType2 = typeof(IPoint3), DestinationType = typeof(ITuple3_Double))]
        [ConvertHelper(SourceType1 = typeof(ITuple3_Byte), SourceType2 = typeof(IPoint3), DestinationType = typeof(ITuple3_Float))]
        public sealed class Convertible3 : ITuple3_Double, ITuple3_Float
        {
            public Convertible3(ITuple3_Byte inner)
            {
                this.inner = inner;
            }

            private readonly ITuple3_Byte inner;

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

            #region ITuple3_Float

            float ITuple3_Float.X
            {
                get { return (float)this.inner.X; }
            }

            float ITuple3_Float.Y
            {
                get { return (float)this.inner.Y; }
            }

            float ITuple3_Float.Z
            {
                get { return (float)this.inner.Z; }
            }

            #endregion
        }

        [ConvertHelper(SourceType1 = typeof(ITuple4_Byte), SourceType2 = typeof(IVector4), DestinationType = typeof(ITuple4_Double))]
        [ConvertHelper(SourceType1 = typeof(ITuple4_Byte), SourceType2 = typeof(IVector4), DestinationType = typeof(ITuple4_Float))]
        [ConvertHelper(SourceType1 = typeof(ITuple4_Byte), SourceType2 = typeof(IPoint4), DestinationType = typeof(ITuple4_Double))]
        [ConvertHelper(SourceType1 = typeof(ITuple4_Byte), SourceType2 = typeof(IPoint4), DestinationType = typeof(ITuple4_Float))]
        public sealed class Convertible4 : ITuple4_Double, ITuple4_Float
        {
            public Convertible4(ITuple4_Byte inner)
            {
                this.inner = inner;
            }

            private readonly ITuple4_Byte inner;

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

            #region ITuple4_Float

            float ITuple4_Float.X
            {
                get { return (float)this.inner.X; }
            }

            float ITuple4_Float.Y
            {
                get { return (float)this.inner.Y; }
            }

            float ITuple4_Float.Z
            {
                get { return (float)this.inner.Z; }
            }

            float ITuple4_Float.W
            {
                get { return (float)this.inner.W; }
            }

            #endregion
        }

        [ConvertHelper(SourceType1 = typeof(ITuple3_Byte), SourceType2 = typeof(IColor3), DestinationType = typeof(ITuple3_Double))]
        [ConvertHelper(SourceType1 = typeof(ITuple3_Byte), SourceType2 = typeof(IColor3), DestinationType = typeof(ITuple3_Float))]
        public sealed class ColorConvertible3 : ITuple3_Double, ITuple3_Float
        {
            public ColorConvertible3(ITuple3_Byte inner)
            {
                this.inner = inner;
            }

            private readonly ITuple3_Byte inner;

            #region ITuple3_Double

            double ITuple3_Double.X
            {
                get { return toColorDouble(this.inner.X); }
            }

            double ITuple3_Double.Y
            {
                get { return toColorDouble(this.inner.Y); }
            }

            double ITuple3_Double.Z
            {
                get { return toColorDouble(this.inner.Z); }
            }

            #endregion

            #region ITuple3_Float

            float ITuple3_Float.X
            {
                get { return toColorFloat(this.inner.X); }
            }

            float ITuple3_Float.Y
            {
                get { return toColorFloat(this.inner.Y); }
            }

            float ITuple3_Float.Z
            {
                get { return toColorFloat(this.inner.Z); }
            }

            #endregion
        }

        [ConvertHelper(SourceType1 = typeof(ITuple4_Byte), SourceType2 = typeof(IColor4), DestinationType = typeof(ITuple4_Double))]
        [ConvertHelper(SourceType1 = typeof(ITuple4_Byte), SourceType2 = typeof(IColor4), DestinationType = typeof(ITuple4_Float))]
        public sealed class ColorConvertible4 : ITuple4_Double, ITuple4_Float
        {
            public ColorConvertible4(ITuple4_Byte inner)
            {
                this.inner = inner;
            }

            private readonly ITuple4_Byte inner;

            #region ITuple4_Double

            double ITuple4_Double.X
            {
                get { return toColorDouble(this.inner.X); }
            }

            double ITuple4_Double.Y
            {
                get { return toColorDouble(this.inner.Y); }
            }

            double ITuple4_Double.Z
            {
                get { return toColorDouble(this.inner.Z); }
            }

            double ITuple4_Double.W
            {
                get { return toColorDouble(this.inner.W); }
            }

            #endregion

            #region ITuple4_Float

            float ITuple4_Float.X
            {
                get { return toColorFloat(this.inner.X); }
            }

            float ITuple4_Float.Y
            {
                get { return toColorFloat(this.inner.Y); }
            }

            float ITuple4_Float.Z
            {
                get { return toColorFloat(this.inner.Z); }
            }

            float ITuple4_Float.W
            {
                get { return toColorFloat(this.inner.W); }
            }

            #endregion
        }
    }
}