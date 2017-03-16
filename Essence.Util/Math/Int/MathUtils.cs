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
using System.Text;
using INT = System.Int32;

namespace Essence.Util.Math.Int
{
    public static class MathUtils
    {
        public static INT Clamp(this INT v, INT min, INT max)
        {
            if (v < min)
            {
                return min;
            }
            if (v > max)
            {
                return max;
            }
            return v;
        }

        public static INT Cuad(INT d)
        {
            return d * d;
        }

        /// <summary>
        ///     Formatea un valor largo en una cadena de texto binaria.
        /// </summary>
        /// <param name="valor">Valor largo para formatear.</param>
        /// <param name="blancos">Indica si se ponen blancos cada 4 bits.</param>
        /// <returns>Cadena de texto con el valor binario.</returns>
        public static string ToBinaryString(INT valor, bool blancos)
        {
            StringBuilder bitstring = new StringBuilder();
            for (int i = 31; i >= 0; i--)
            {
                string result = ((((valor >> i) & 1) == 1) ? "1" : "0");
                bitstring.Append(result);
                if (blancos)
                {
                    if ((i > 0) && ((i) % 4) == 0)
                    {
                        bitstring.Append(" ");
                    }
                }
            }
            return bitstring.ToString();
        }

        /// <summary>
        ///     Interpolación lineal.
        /// </summary>
        public static INT Lerp(INT x, INT x1, INT y1, INT x2, INT y2)
        {
            if ((x1 == x2) || (y1 == y2))
            {
                return y1;
            }
            return (y1 * (x2 - x) + y2 * (x - x1)) / (x2 - x1);
        }

        /// <summary>
        ///     Interpolación lineal.
        /// </summary>
        public static INT Lerp(INT x, INT x1, INT y1, INT x2, INT y2, out INT result)
        {
            if ((x1 == x2) || (y1 == y2))
            {
                result = 0;
                return y1;
            }
            return System.Math.DivRem((y1 * (x2 - x) + y2 * (x - x1)), (x2 - x1), out result);
        }
    }
}