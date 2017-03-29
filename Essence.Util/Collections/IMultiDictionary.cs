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

namespace Essence.Util.Collections
{
    /// <summary>
    ///     Diccionario que asocia una clave con multiples valores (lista de valores).
    /// </summary>
    /// <typeparam name="TKey">Tipo de las claves del diccionario.</typeparam>
    /// <typeparam name="TValue">Tipo de los valores del diccionario.</typeparam>
    /// <typeparam name="TCollection">Tipo de la coleccion de valores del diccionario.</typeparam>
    public interface IMultiDictionary<TKey, in TValue, TCollection> : IDictionary<TKey, TCollection>
        where TCollection : ICollection<TValue>
    {
        /// <summary>
        ///     Indica si contiene al par <c>key</c>, <c>value</c>.
        /// </summary>
        /// <param name="key">Clave.</param>
        /// <param name="value">Valor.</param>
        /// <returns>Indica si los contiene.</returns>
        bool Contains(TKey key, TValue value);

        /// <summary>
        ///     Añade el par <c>key</c>, <c>value</c>. Si existe la clave, añade el
        ///     valor a la lista de valores de la clave.
        /// </summary>
        /// <param name="key">Clave.</param>
        /// <param name="value">Valor.</param>
        void Add(TKey key, TValue value);

        /// <summary>
        ///     Elimina el par <c>key</c>, <c>value</c>.
        /// </summary>
        /// <param name="key">Clave.</param>
        /// <param name="value">Valor.</param>
        bool Remove(TKey key, TValue value);
    }
}