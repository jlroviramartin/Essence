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

using System;

namespace Essence.Util.Math
{
    public static class ConvertibleUtils
    {
        public static int ToInt16<TConvertible>(this TConvertible c)
            where TConvertible : struct, IConvertible
        {
            return c.ToInt16(null);
        }

        public static int ToInt32<TConvertible>(this TConvertible c)
            where TConvertible : struct, IConvertible
        {
            return c.ToInt32(null);
        }

        public static long ToInt64<TConvertible>(this TConvertible c)
            where TConvertible : struct, IConvertible
        {
            return c.ToInt64(null);
        }

        public static float ToSingle<TConvertible>(this TConvertible c)
            where TConvertible : struct, IConvertible
        {
            return c.ToSingle(null);
        }

        public static double ToDouble<TConvertible>(this TConvertible c)
            where TConvertible : struct, IConvertible
        {
            return c.ToDouble(null);
        }
    }
}