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
using Essence.Geometry.Core.Byte;
using Essence.Geometry.Core.Float;

namespace Essence.Geometry.Core
{
    public static class ColorUtils
    {
        #region IColor

        public static Color3f ToColor3f(this IColor c)
        {
            if (c is Color3f)
            {
                return (Color3f)c;
            }
            IColor3 c3 = c as IColor3;
            if (c3 != null)
            {
                return new Color3f(c3.Red.ToSingle(),
                                   c3.Green.ToSingle(),
                                   c3.Blue.ToSingle());
            }
            else
            {
                if (c.Dim < 3)
                {
                    throw new Exception("Color no valido");
                }
                return new Color3f(c[0].ToSingle(),
                                   c[1].ToSingle(),
                                   c[2].ToSingle());
            }
        }

        public static Color4f ToColor4f(this IColor c)
        {
            if (c is Color4f)
            {
                return (Color4f)c;
            }
            IColor4 c4 = c as IColor4;
            if (c4 != null)
            {
                return new Color4f(c4.Red.ToSingle(),
                                   c4.Green.ToSingle(),
                                   c4.Blue.ToSingle(),
                                   c4.Alpha.ToSingle());
            }
            else
            {
                if (c.Dim < 3)
                {
                    throw new Exception("Color no valido");
                }
                else if (c.Dim == 3)
                {
                    return new Color4f(c[0].ToSingle(),
                                       c[1].ToSingle(),
                                       c[2].ToSingle(),
                                       1);
                }
                else /*if (c.Dim >= 4)*/
                {
                    return new Color4f(c[0].ToSingle(),
                                       c[1].ToSingle(),
                                       c[2].ToSingle(),
                                       c[3].ToSingle());
                }
            }
        }

        public static Color3b ToColor3b(this IColor c)
        {
            if (c is Color3b)
            {
                return (Color3b)c;
            }
            IColor3 c3 = c as IColor3;
            if (c3 != null)
            {
                return new Color3b(c3.Red.ToByte(),
                                   c3.Green.ToByte(),
                                   c3.Blue.ToByte());
            }
            else
            {
                if (c.Dim < 3)
                {
                    throw new Exception("Color no valido");
                }
                return new Color3b(c[0].ToByte(),
                                   c[1].ToByte(),
                                   c[2].ToByte());
            }
        }

        public static Color4b ToColor4b(this IColor c)
        {
            if (c is Color4b)
            {
                return (Color4b)c;
            }
            IColor4 c4 = c as IColor4;
            if (c4 != null)
            {
                return new Color4b(c4.Red.ToByte(),
                                   c4.Green.ToByte(),
                                   c4.Blue.ToByte(),
                                   c4.Alpha.ToByte());
            }
            else
            {
                if (c.Dim < 3)
                {
                    throw new Exception("Color no valido");
                }
                else if (c.Dim == 3)
                {
                    return new Color4b(c[0].ToByte(),
                                       c[1].ToByte(),
                                       c[2].ToByte(),
                                       1);
                }
                else /*if (c.Dim >= 4)*/
                {
                    return new Color4b(c[0].ToByte(),
                                       c[1].ToByte(),
                                       c[2].ToByte(),
                                       c[3].ToByte());
                }
            }
        }

        #endregion

        #region IColor3

        public static Color3f ToColor3f(this IColor3 c)
        {
            if (c is Color3f)
            {
                return (Color3f)c;
            }
            return new Color3f(c.Red.ToSingle(),
                               c.Green.ToSingle(),
                               c.Blue.ToSingle());
        }

        public static Color4f ToColor4f(this IColor3 c)
        {
            if (c is Color4f)
            {
                return (Color4f)c;
            }
            return new Color4f(c.Red.ToSingle(),
                               c.Green.ToSingle(),
                               c.Blue.ToSingle(),
                               1);
        }

        public static Color3b ToColor3b(this IColor3 c)
        {
            if (c is Color3b)
            {
                return (Color3b)c;
            }
            return new Color3b(c.Red.ToByte(),
                               c.Green.ToByte(),
                               c.Blue.ToByte());
        }

        public static Color4b ToColor4b(this IColor3 c)
        {
            if (c is Color4b)
            {
                return (Color4b)c;
            }
            return new Color4b(c.Red.ToByte(),
                               c.Green.ToByte(),
                               c.Blue.ToByte(),
                               1);
        }

        #endregion

        #region IColor4

        public static Color3f ToColor3f(this IColor4 c)
        {
            return new Color3f(c.Red.ToSingle(),
                               c.Green.ToSingle(),
                               c.Blue.ToSingle());
        }

        public static Color4f ToColor4f(this IColor4 c)
        {
            if (c is Color4f)
            {
                return (Color4f)c;
            }
            return new Color4f(c.Red.ToSingle(),
                               c.Green.ToSingle(),
                               c.Blue.ToSingle(),
                               c.Alpha.ToSingle());
        }

        public static Color3b ToColor3b(this IColor4 c)
        {
            return new Color3b(c.Red.ToByte(),
                               c.Green.ToByte(),
                               c.Blue.ToByte());
        }

        public static Color4b ToColor4b(this IColor4 c)
        {
            if (c is Color4b)
            {
                return (Color4b)c;
            }
            return new Color4b(c.Red.ToByte(),
                               c.Green.ToByte(),
                               c.Blue.ToByte(),
                               c.Alpha.ToByte());
        }

        #endregion
    }
}