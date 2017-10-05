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

namespace Essence.Geometry.Core.Float
{
    public static class FloatConverters
    {
        public static ITuple2_Float AsTupleFloat(this ITuple2 v)
        {
            ITuple2_Float ret = v as ITuple2_Float;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<ITuple2_Float>(v);
        }

        public static IOpTuple2_Float AsOpTupleFloat(this IOpTuple2 v)
        {
            IOpTuple2_Float ret = v as IOpTuple2_Float;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<IOpTuple2_Float>(v);
        }

        public static ITuple3_Float AsTupleFloat(this ITuple3 v)
        {
            ITuple3_Float ret = v as ITuple3_Float;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<ITuple3_Float>(v);
        }

        public static IOpTuple3_Float AsOpTupleFloat(this IOpTuple3 v)
        {
            IOpTuple3_Float ret = v as IOpTuple3_Float;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<IOpTuple3_Float>(v);
        }

        public static ITuple4_Float AsTupleFloat(this ITuple4 v)
        {
            ITuple4_Float ret = v as ITuple4_Float;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<ITuple4_Float>(v);
        }

        public static IOpTuple4_Float AsOpTupleFloat(this IOpTuple4 v)
        {
            IOpTuple4_Float ret = v as IOpTuple4_Float;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<IOpTuple4_Float>(v);
        }

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
    }
}