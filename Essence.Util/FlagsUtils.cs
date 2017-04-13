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

using System.Collections.Generic;

namespace Essence.Util
{
    public class FlagsUtils
    {
        /// <summary>
        /// Descompose the flag <code>flag</code> in its individual 1 bits.
        /// </summary>
        /// <param name="flag">Flags.</param>
        /// <param name="result">Bits 'on'.</param>
        public static void DecomposeFlag(ulong flag, List<ulong> result)
        {
            DecomposeFlag(flag, 32, 0xffffffff, result);
        }

        #region private

        private static void DecomposeFlag(ulong flag, int bits, ulong mask, List<ulong> result)
        {
            if (bits == 0)
            {
                if ((flag & mask) == 1)
                {
                    result.Add(1);
                }
                return;
            }

            ulong low = (flag & mask);
            if (low != 0)
            {
                DecomposeFlag(low, bits >> 1, mask >> (bits >> 1), result);
            }
            ulong high = ((flag >> bits) & mask);
            if (high != 0)
            {
                List<ulong> aux = new List<ulong>();
                DecomposeFlag(high, bits >> 1, mask >> (bits >> 1), aux);
                foreach (ulong v in aux)
                {
                    result.Add(v << bits);
                }
            }
        }

        #endregion
    }
}