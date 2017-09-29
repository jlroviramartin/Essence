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
using ITuple2_Int = Essence.Geometry.Core.Int.ITuple2_Int;
using ITuple2_Byte = Essence.Geometry.Core.Byte.ITuple2_Byte;
using ITuple3_Float = Essence.Geometry.Core.Float.ITuple3_Float;
using ITuple3_Byte = Essence.Geometry.Core.Byte.ITuple3_Byte;
using ITuple4_Float = Essence.Geometry.Core.Float.ITuple4_Float;
using ITuple4_Byte = Essence.Geometry.Core.Byte.ITuple4_Byte;

namespace Essence.Geometry.Core.Double
{
    public static class DoubleConvertibles
    {
        public static ITuple2_Double AsTupleDouble(this IPoint2 v)
        {
            ITuple2_Double ret = v as ITuple2_Double;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<IPoint2, ITuple2_Double>(v);
        }

        public static ITuple2_Double AsTupleDouble(this IVector2 v)
        {
            ITuple2_Double ret = v as ITuple2_Double;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<IVector2, ITuple2_Double>(v);
        }

        public static ITuple3_Double AsTupleDouble(this IPoint3 v)
        {
            ITuple3_Double ret = v as ITuple3_Double;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<IPoint3, ITuple3_Double>(v);
        }

        public static ITuple3_Double AsTupleDouble(this IVector3 v)
        {
            ITuple3_Double ret = v as ITuple3_Double;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<IVector3, ITuple3_Double>(v);
        }

        public static ITuple4_Double AsTupleDouble(this IPoint4 v)
        {
            ITuple4_Double ret = v as ITuple4_Double;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<IPoint4, ITuple4_Double>(v);
        }

        public static ITuple4_Double AsTupleDouble(this IVector4 v)
        {
            ITuple4_Double ret = v as ITuple4_Double;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<IVector4, ITuple4_Double>(v);
        }

        [ConvertHelper(SourceType1 = typeof(ITuple2_Double), SourceType2 = typeof(IVector2), DestinationType = typeof(ITuple2_Float))]
        [ConvertHelper(SourceType1 = typeof(ITuple2_Double), SourceType2 = typeof(IVector2), DestinationType = typeof(ITuple2_Int))]
        [ConvertHelper(SourceType1 = typeof(ITuple2_Double), SourceType2 = typeof(IVector2), DestinationType = typeof(ITuple2_Byte))]
        [ConvertHelper(SourceType1 = typeof(ITuple2_Double), SourceType2 = typeof(IPoint2), DestinationType = typeof(ITuple2_Float))]
        [ConvertHelper(SourceType1 = typeof(ITuple2_Double), SourceType2 = typeof(IPoint2), DestinationType = typeof(ITuple2_Int))]
        [ConvertHelper(SourceType1 = typeof(ITuple2_Double), SourceType2 = typeof(IPoint2), DestinationType = typeof(ITuple2_Byte))]
        public sealed class Convertible2 : ITuple2_Float, ITuple2_Int, ITuple2_Byte
        {
            public Convertible2(ITuple2_Double inner)
            {
                this.inner = inner;
            }

            private readonly ITuple2_Double inner;

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

        [ConvertHelper(SourceType1 = typeof(ITuple3_Double), SourceType2 = typeof(IVector3), DestinationType = typeof(ITuple3_Float))]
        [ConvertHelper(SourceType1 = typeof(ITuple3_Double), SourceType2 = typeof(IVector3), DestinationType = typeof(ITuple3_Byte))]
        [ConvertHelper(SourceType1 = typeof(ITuple3_Double), SourceType2 = typeof(IPoint3), DestinationType = typeof(ITuple3_Float))]
        [ConvertHelper(SourceType1 = typeof(ITuple3_Double), SourceType2 = typeof(IPoint3), DestinationType = typeof(ITuple3_Byte))]
        public sealed class Convertible3 : ITuple3_Float, ITuple3_Byte
        {
            public Convertible3(ITuple3_Double inner)
            {
                this.inner = inner;
            }

            private readonly ITuple3_Double inner;

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

        [ConvertHelper(SourceType1 = typeof(ITuple4_Double), SourceType2 = typeof(IVector4), DestinationType = typeof(ITuple4_Float))]
        [ConvertHelper(SourceType1 = typeof(ITuple4_Double), SourceType2 = typeof(IVector4), DestinationType = typeof(ITuple4_Byte))]
        [ConvertHelper(SourceType1 = typeof(ITuple4_Double), SourceType2 = typeof(IPoint4), DestinationType = typeof(ITuple4_Float))]
        [ConvertHelper(SourceType1 = typeof(ITuple4_Double), SourceType2 = typeof(IPoint4), DestinationType = typeof(ITuple4_Byte))]
        public sealed class Convertible4 : ITuple4_Float, ITuple4_Byte
        {
            public Convertible4(ITuple4_Double inner)
            {
                this.inner = inner;
            }

            private readonly ITuple4_Double inner;

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
    }
}