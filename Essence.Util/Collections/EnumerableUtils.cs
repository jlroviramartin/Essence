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
using System.Collections.Generic;

namespace Essence.Util.Collections
{
    public static class EnumerableUtils
    {
        public static IEnumerable<int> For(int i, int e, int inc = 1)
        {
            while (i < e)
            {
                yield return i;
                i += inc;
            }
        }

        /// <summary>
        ///     Para cada elemento en <c>enumer</c> realiza la operacion <c>action</c>.
        /// </summary>
        /// <typeparam name="T">Tipo de los enumerables.</typeparam>
        /// <param name="enumer">Enumerable.</param>
        /// <param name="action">Accion a aplicar.</param>
        public static void ForEach<T>(this IEnumerable<T> enumer, Action<T> action)
        {
            foreach (T t in enumer)
            {
                action(t);
            }
        }

        /// <summary>
        ///     Aplica la accion <c>action</c> hasta que se cumpla <c>pred</c>.
        ///     Indica si ha terminado prematuramente.
        /// </summary>
        /// <typeparam name="T">Tipo de los enumerables.</typeparam>
        /// <param name="enumer">Enumerable.</param>
        /// <param name="pred">Predicado.</param>
        /// <returns>Indica si ha terminado prematuramente.</returns>
        /// <param name="action">Acción a aplicar.</param>
        public static void Until<T>(this IEnumerable<T> enumer, Func<T, bool> pred, Action<T> action)
        {
            foreach (T t in enumer)
            {
                if (pred(t))
                {
                    return;
                }
                action(t);
            }
        }

        /// <summary>
        ///     Aplica la accion <c>action</c> mientras se cumpla <c>pred</c>.
        ///     Indica si ha terminado prematuramente.
        /// </summary>
        /// <typeparam name="T">Tipo de los enumerables.</typeparam>
        /// <param name="enumer">Enumerable.</param>
        /// <param name="pred">Predicado.</param>
        /// <param name="action">Acción a aplicar.</param>
        public static void While<T>(this IEnumerable<T> enumer, Func<T, bool> pred, Action<T> action)
        {
            foreach (T t in enumer)
            {
                if (!pred(t))
                {
                    return;
                }
                action(t);
            }
        }

        /// <summary>
        ///     Busca el primer elemento en <c>enumer</c> que pase el test <c>predicate</c>.
        /// </summary>
        /// <typeparam name="T">Tipo de los enumerables.</typeparam>
        /// <param name="enumer">Enumerable.</param>
        /// <param name="pred">Predicado.</param>
        /// <param name="defT">Valor por defecto.</param>
        /// <returns>Valor encontrado o defT si no ha encontrado nada.</returns>
        public static T Find<T>(this IEnumerable<T> enumer, Func<T, bool> predicate, T defT)
        {
            T t;
            if (!TryFind(enumer, predicate, out t))
            {
                return defT;
            }
            return t;
        }

        /// <summary>
        ///     Busca el primer elemento en <c>enumer</c> que pase el test <c>predicate</c>.
        /// </summary>
        /// <typeparam name="T">Tipo de los enumerables.</typeparam>
        /// <param name="enumer">Enumerable.</param>
        /// <param name="pred">Predicado.</param>
        /// <param name="ret">Valor encontrado.</param>
        /// <returns>Indica si lo ha encontrado.</returns>
        public static bool TryFind<T>(this IEnumerable<T> enumer, Func<T, bool> predicate, out T ret)
        {
            foreach (T t in enumer)
            {
                if (predicate(t))
                {
                    ret = t;
                    return true;
                }
            }
            ret = default(T);
            return false;
        }

        /// <summary>
        ///     Indica si existe al menos un elemento en <c>enumer</c> que pase el test <c>predicate</c>.
        /// </summary>
        /// <typeparam name="T">Tipo de los enumerables.</typeparam>
        /// <param name="enumer">Enumerable.</param>
        /// <param name="pred">Predicado.</param>
        /// <returns>Indica si existe.</returns>
        public static bool Exists<T>(this IEnumerable<T> enumer, Func<T, bool> predicate)
        {
            foreach (T t in enumer)
            {
                if (predicate(t))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        ///     Indica si existe exactamente un elemento en <c>enumer</c> que pase el test <c>predicate</c>.
        /// </summary>
        /// <typeparam name="T">Tipo de los enumerables.</typeparam>
        /// <param name="enumer">Enumerable.</param>
        /// <param name="pred">Predicado.</param>
        /// <returns>Indica si existe solo uno.</returns>
        public static bool ExistsUnique<T>(this IEnumerable<T> enumer, Func<T, bool> pred)
        {
            bool found = false;
            foreach (T t in enumer)
            {
                if (pred(t))
                {
                    if (found)
                    {
                        return false;
                    }
                    found = true;
                }
            }
            return found;
        }

        public static T FirstOrDefault<T>(this IEnumerable<T> enumer, T def)
        {
            IEnumerator<T> enumerator = enumer.GetEnumerator();
            if (enumerator.MoveNext())
            {
                return enumerator.Current;
            }
            return def;
        }

        public static IEnumerable<int> Numerate(int start, int inc, int count)
        {
            while (count > 0)
            {
                yield return start;
                start += inc;
            }
        }
    }
}