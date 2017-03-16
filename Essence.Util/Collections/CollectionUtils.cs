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
using System.Collections.Generic;

namespace Essence.Util.Collections
{
    public static class CollectionUtils
    {
        public static void ForEach<T>(this ICollection<T> collection, Action<T> action)
        {
            foreach (T value in collection)
            {
                action(value);
            }
        }

        public static void Set<T>(this ICollection<T> collection, T value)
        {
            collection.Clear();
            collection.Add(value);
        }

        public static void SetAll<T>(this ICollection<T> collection, IEnumerable<T> enumer)
        {
            collection.Clear();
            collection.AddAll(enumer);
        }

        /// <summary>
        ///     Añade todos los elementos de un enumerable a una coleccion, al final.
        /// </summary>
        /// <typeparam name="T">Tipo de elementos de la lista.</typeparam>
        /// <param name="collection">Coleccion.</param>
        /// <param name="enumer">Enumerable.</param>
        public static void AddAll<T>(this ICollection<T> collection, IEnumerable<T> enumer)
        {
            if (collection is List<T>)
            {
                ((List<T>)collection).AddRange(enumer);
            }
            else if (collection is AbsCollection<T>)
            {
                ((AbsCollection<T>)collection).AddAll(enumer);
            }
            else
            {
                foreach (T t in enumer)
                {
                    collection.Add(t);
                }
            }
        }

        /// <summary>
        ///     Elimina todos los elementos indicados de una lista.
        /// </summary>
        /// <typeparam name="T">Tipo de elementos de la lista.</typeparam>
        /// <param name="collection">Coleccion.</param>
        /// <param name="enumer">Enumerable.</param>
        public static void RemoveAll<T>(this ICollection<T> collection, IEnumerable<T> enumer)
        {
            if (collection is AbsCollection<T>)
            {
                ((AbsCollection<T>)collection).RemoveAll(enumer);
            }
            else
            {
                foreach (T t in enumer)
                {
                    collection.Remove(t);
                }
            }
        }

        /// <summary>
        ///     Elimina todos los elementos que cumplan un predicado de una lista.
        /// </summary>
        /// <typeparam name="T">Tipo de elementos de la lista.</typeparam>
        /// <param name="collection">Coleccion.</param>
        /// <param name="predicate">Predicado.</param>
        public static void RemoveAll<T>(this ICollection<T> collection, Func<T, bool> predicate)
        {
            if (collection is List<T>)
            {
                ((List<T>)collection).RemoveAll(predicate);
            }
            else if (collection is AbsCollection<T>)
            {
                ((AbsCollection<T>)collection).RemoveAll(predicate);
            }
            else
            {
                List<T> toRemove = new List<T>();
                foreach (T t in collection)
                {
                    if (predicate(t))
                    {
                        toRemove.Add(t);
                    }
                }
                RemoveAll(collection, toRemove);
            }
        }

        #region Stack<T>

        /// <summary>
        ///     Añade todos los elementos indicados a una pila.
        /// </summary>
        /// <typeparam name="T">Tipo de la pila.</typeparam>
        /// <param name="collection">Pila.</param>
        /// <param name="enumer">Elementos.</param>
        public static void PushAll<T>(this Stack<T> collection, IEnumerable<T> enumer)
        {
            foreach (T obj in enumer)
            {
                collection.Push(obj);
            }
        }

        #endregion Stack<T>

        #region Queue<T>

        /// <summary>
        ///     Añade todos los elementos indicados a una cola.
        /// </summary>
        /// <typeparam name="T">Tipo de la cola.</typeparam>
        /// <param name="collection">Cola.</param>
        /// <param name="enumer">Elementos.</param>
        public static void EnqueueAll<T>(this Queue<T> collection, IEnumerable<T> enumer)
        {
            foreach (T obj in enumer)
            {
                collection.Enqueue(obj);
            }
        }

        #endregion Queue<T>

        public static void RemoveAllKeys<TK, TV>(this IDictionary<TK, TV> dictionary, IEnumerable<TK> keys)
        {
            if (dictionary is AbsDictionary<TK, TV>)
            {
                ((AbsDictionary<TK, TV>)dictionary).RemoveAllKeys(keys);
            }
            else
            {
                foreach (TK k in keys)
                {
                    dictionary.Remove(k);
                }
            }
        }

        #region Dictionary<TK,TV>

        public static TV GetSafe<TK, TV>(this IDictionary<TK, TV> dictionary, TK key, TV defValue = default(TV))
        {
            TV value;
            if (!dictionary.TryGetValue(key, out value))
            {
                return defValue;
            }
            return value;
        }

        public static TV? GetSafeSt<TK, TV>(this IDictionary<TK, TV> dictionary, TK key)
            where TV : struct
        {
            TV value;
            if (!dictionary.TryGetValue(key, out value))
            {
                return null;
            }
            return value;
        }

        public static void AddOrSet<TK, TV>(this IDictionary<TK, TV> dictionary, TK key, TV value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }

        public static void AddOrSetAll<TK, TV>(this IDictionary<TK, TV> dictionary, IEnumerable<KeyValuePair<TK, TV>> enumer)
        {
            foreach (KeyValuePair<TK, TV> kvp in enumer)
            {
                dictionary.AddOrSet(kvp.Key, kvp.Value);
            }
        }

        #endregion Dictionary<TK,TV>
    }
}