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

using Essence.Geometry.Core.Byte;
using Essence.Geometry.Core.Float;
using ITuple2_Float = Essence.Geometry.Core.Float.ITuple2_Float;
using ITuple2_Byte = Essence.Geometry.Core.Byte.ITuple2_Byte;
using ITuple3_Float = Essence.Geometry.Core.Float.ITuple3_Float;
using ITuple3_Byte = Essence.Geometry.Core.Byte.ITuple3_Byte;
using ITuple4_Float = Essence.Geometry.Core.Float.ITuple4_Float;
using ITuple4_Byte = Essence.Geometry.Core.Byte.ITuple4_Byte;

namespace Essence.Geometry.Core.Double
{
    public static class DoubleConverters
    {
        public static ITuple2_Double AsTupleDouble(this ITuple2 v)
        {
            ITuple2_Double ret = v as ITuple2_Double;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<ITuple2_Double>(v);
        }

        public static IOpTuple2_Double AsOpTupleDouble(this IOpTuple2 v)
        {
            IOpTuple2_Double ret = v as IOpTuple2_Double;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<IOpTuple2_Double>(v);
        }

        public static ITuple3_Double AsTupleDouble(this ITuple3 v)
        {
            ITuple3_Double ret = v as ITuple3_Double;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<ITuple3_Double>(v);
        }

        public static IOpTuple3_Double AsOpTupleDouble(this IOpTuple3 v)
        {
            IOpTuple3_Double ret = v as IOpTuple3_Double;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<IOpTuple3_Double>(v);
        }

        public static ITuple4_Double AsTupleDouble(this ITuple4 v)
        {
            ITuple4_Double ret = v as ITuple4_Double;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<ITuple4_Double>(v);
        }

        public static IOpTuple4_Double AsOpTupleDouble(this IOpTuple4 v)
        {
            IOpTuple4_Double ret = v as IOpTuple4_Double;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<IOpTuple4_Double>(v);
        }

        public static ITuple2_Double AsTupleDouble(this IPoint2 v)
        {
            ITuple2_Double ret = v as ITuple2_Double;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<ITuple2_Double>(v);
        }

        public static ITuple2_Double AsTupleDouble(this IVector2 v)
        {
            ITuple2_Double ret = v as ITuple2_Double;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<ITuple2_Double>(v);
        }

        public static IOpTuple2_Double AsOpTupleDouble(this IOpPoint2 v)
        {
            IOpTuple2_Double ret = v as IOpTuple2_Double;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<IOpTuple2_Double>(v);
        }

        public static IOpTuple2_Double AsOpTupleDouble(this IOpVector2 v)
        {
            IOpTuple2_Double ret = v as IOpTuple2_Double;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<IOpTuple2_Double>(v);
        }

        public static ITuple3_Double AsTupleDouble(this IPoint3 v)
        {
            ITuple3_Double ret = v as ITuple3_Double;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<ITuple3_Double>(v);
        }

        public static ITuple3_Double AsTupleDouble(this IVector3 v)
        {
            ITuple3_Double ret = v as ITuple3_Double;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<ITuple3_Double>(v);
        }

        public static IOpTuple3_Double AsOpTupleDouble(this IOpPoint3 v)
        {
            IOpTuple3_Double ret = v as IOpTuple3_Double;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<IOpTuple3_Double>(v);
        }

        public static IOpTuple3_Double AsOpTupleDouble(this IOpVector3 v)
        {
            IOpTuple3_Double ret = v as IOpTuple3_Double;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<IOpTuple3_Double>(v);
        }

        public static ITuple4_Double AsTupleDouble(this IPoint4 v)
        {
            ITuple4_Double ret = v as ITuple4_Double;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<ITuple4_Double>(v);
        }

        public static ITuple4_Double AsTupleDouble(this IVector4 v)
        {
            ITuple4_Double ret = v as ITuple4_Double;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<ITuple4_Double>(v);
        }

        public static IOpTuple4_Double AsOpTupleDouble(this IOpPoint4 v)
        {
            IOpTuple4_Double ret = v as IOpTuple4_Double;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<IOpTuple4_Double>(v);
        }

        public static IOpTuple4_Double AsOpTupleDouble(this IOpVector4 v)
        {
            IOpTuple4_Double ret = v as IOpTuple4_Double;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<IOpTuple4_Double>(v);
        }
    }
}