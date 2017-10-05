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

namespace Essence.Geometry.Core.Byte
{
    public static class ByteConverters
    {
        public static ITuple2_Byte AsTupleByte(this ITuple2 v)
        {
            ITuple2_Byte ret = v as ITuple2_Byte;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<ITuple2_Byte>(v);
        }

        public static IOpTuple2_Byte AsOpTupleByte(this IOpTuple2 v)
        {
            IOpTuple2_Byte ret = v as IOpTuple2_Byte;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<IOpTuple2_Byte>(v);
        }

        public static ITuple3_Byte AsTupleByte(this ITuple3 v)
        {
            ITuple3_Byte ret = v as ITuple3_Byte;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<ITuple3_Byte>(v);
        }

        public static IOpTuple3_Byte AsOpTupleByte(this IOpTuple3 v)
        {
            IOpTuple3_Byte ret = v as IOpTuple3_Byte;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<IOpTuple3_Byte>(v);
        }

        public static ITuple4_Byte AsTupleByte(this ITuple4 v)
        {
            ITuple4_Byte ret = v as ITuple4_Byte;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<ITuple4_Byte>(v);
        }

        public static IOpTuple4_Byte AsOpTupleByte(this IOpTuple4 v)
        {
            IOpTuple4_Byte ret = v as IOpTuple4_Byte;
            if (ret != null)
            {
                return ret;
            }
            return VectorUtils.Convert<IOpTuple4_Byte>(v);
        }

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
    }
}