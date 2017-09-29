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
using Essence.Util.Math.Double;

namespace Essence.Geometry.Core
{
    public static class ColorUtils
    {
        public static float ToChannelFloat(byte c)
        {
            return ((float)c) / byte.MaxValue;
        }

        public static byte ToChannelByte(float c)
        {
            return (byte)MathUtils.Clamp(c * byte.MaxValue, 0, 255);
        }

        #region IColor3

        public static Color3f ToColor3f(this IColor3 c)
        {
            if (c is Color3f)
            {
                return (Color3f)c;
            }
            return new Color3f(c);
        }

        public static Color4f ToColor4f(this IColor3 c, float alpha = 0)
        {
            return new Color4f(c, alpha);
        }

        public static Color3b ToColor3b(this IColor3 c)
        {
            if (c is Color3b)
            {
                return (Color3b)c;
            }
            return new Color3b(c);
        }

        public static Color4b ToColor4b(this IColor3 c, byte alpha = 0)
        {
            return new Color4b(c, alpha);
        }

        #endregion

        #region IColor4

        /*public static Color3f ToColor3f(this IColor4 c)
        {
            return new Color3f(c);
        }*/

        public static Color4f ToColor4f(this IColor4 c)
        {
            if (c is Color4f)
            {
                return (Color4f)c;
            }
            return new Color4f(c);
        }

        /*public static Color3b ToColor3b(this IColor4 c)
        {
            return new Color3b(c);
        }*/

        public static Color4b ToColor4b(this IColor4 c)
        {
            if (c is Color4b)
            {
                return (Color4b)c;
            }
            return new Color4b(c);
        }

        #endregion
    }
}