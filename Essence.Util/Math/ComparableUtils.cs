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

namespace Essence.Util.Math
{
    public static class ComparableUtils
    {
        /// <summary>
        /// Ajusta el valor entre el minimo y el maximo.
        /// <c><![CDATA[
        /// [min, max] : return >= min && return <= max
        /// ]]></c>
        /// </summary>
        /// <typeparam name="T">Tipo del valor.</typeparam>
        /// <param name="value">Valor.</param>
        /// <param name="min">Minimo.</param>
        /// <param name="max">Maximo.</param>
        /// <returns>Valor ajustado.</returns>
        public static T Clamp<T>(this T value, T min, T max)
            where T : IComparable<T>
        {
            T result = value;
            if (value.CompareTo(max) > 0)
            {
                result = max;
            }
            if (value.CompareTo(min) < 0)
            {
                result = min;
            }
            return result;
        }

        /// <summary>
        /// Ajusta el valor entre el minimo y el maximo.
        /// <c><![CDATA[
        /// [min, max] : value >= min && value <= max
        /// ]]></c>
        /// </summary>
        /// <typeparam name="T">Tipo del valor.</typeparam>
        /// <param name="value">Valor.</param>
        /// <param name="min">Minimo.</param>
        /// <param name="max">Maximo.</param>
        public static void Clamp<T>(ref T value, T min, T max)
            where T : IComparable<T>
        {
            if (value.CompareTo(max) > 0)
            {
                value = max;
            }
            if (value.CompareTo(min) < 0)
            {
                value = min;
            }
        }
    }
}