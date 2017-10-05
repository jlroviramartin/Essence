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

namespace Essence.Geometry.Core.Integer
{
    public static class IntegerConverters
    {
        public static ITuple2_Integer AsTupleInteger(this ITuple2 v)
        {
            ITuple2_Integer ret = v as ITuple2_Integer;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<ITuple2_Integer>(v);
        }

        public static IOpTuple2_Integer AsOpTupleInteger(this IOpTuple2 v)
        {
            IOpTuple2_Integer ret = v as IOpTuple2_Integer;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<IOpTuple2_Integer>(v);
        }

        public static ITuple3_Integer AsTupleInteger(this ITuple3 v)
        {
            ITuple3_Integer ret = v as ITuple3_Integer;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<ITuple3_Integer>(v);
        }

        public static IOpTuple3_Integer AsOpTupleInteger(this IOpTuple3 v)
        {
            IOpTuple3_Integer ret = v as IOpTuple3_Integer;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<IOpTuple3_Integer>(v);
        }

        public static ITuple4_Integer AsTupleInteger(this ITuple4 v)
        {
            ITuple4_Integer ret = v as ITuple4_Integer;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<ITuple4_Integer>(v);
        }

        public static IOpTuple4_Integer AsOpTupleInteger(this IOpTuple4 v)
        {
            IOpTuple4_Integer ret = v as IOpTuple4_Integer;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<IOpTuple4_Integer>(v);
        }

        public static ITuple2_Integer AsTupleInteger(this IPoint2 v)
        {
            ITuple2_Integer ret = v as ITuple2_Integer;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<ITuple2_Integer>(v);
        }

        public static ITuple2_Integer AsTupleInteger(this IVector2 v)
        {
            ITuple2_Integer ret = v as ITuple2_Integer;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<ITuple2_Integer>(v);
        }
    }
}