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
using System.Text;
using INT = System.Int32;

namespace Essence.Util.Math.Int
{
    public static class MathUtils
    {
        public static int Clamp(this int v, int min, int max)
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

        public static int Cuad(int d)
        {
            return d * d;
        }

        /// <summary>
        /// Formatea un valor largo en una cadena de texto binaria.
        /// </summary>
        /// <param name="valor">Valor largo para formatear.</param>
        /// <param name="blancos">Indica si se ponen blancos cada 4 bits.</param>
        /// <returns>Cadena de texto con el valor binario.</returns>
        public static string ToBinaryString(int valor, bool blancos)
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
        /// Interpolación lineal.
        /// </summary>
        public static int Lerp(int x, int x1, int y1, int x2, int y2)
        {
            if ((x1 == x2) || (y1 == y2))
            {
                return y1;
            }
            return (y1 * (x2 - x) + y2 * (x - x1)) / (x2 - x1);
        }

        /// <summary>
        /// Interpolación lineal.
        /// </summary>
        public static int Lerp(int x, int x1, int y1, int x2, int y2, out int result)
        {
            if ((x1 == x2) || (y1 == y2))
            {
                result = 0;
                return y1;
            }
            return System.Math.DivRem((y1 * (x2 - x) + y2 * (x - x1)), (x2 - x1), out result);
        }

        public static int Pow(int x, int y)
        {
            switch (y)
            {
                case 0:
                {
                    return 1;
                }
                case 1:
                {
                    return x;
                }
                case 2:
                {
                    return x * x;
                }
                case 3:
                {
                    return x * x * x;
                }
                case 4:
                {
                    int xx = x * x;
                    return xx * xx;
                }
            }
            int result = x;
            for (int i = 1; i < y; i++)
            {
                result *= x;
            }
            return result;
        }
    }
}