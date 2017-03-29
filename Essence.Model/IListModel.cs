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

namespace Essence.Model
{
    public interface IListModel
    {
        /// <summary>
        ///     Obtiene los elementos del modelo.
        /// </summary>
        /// <returns>Elementos.</returns>
        IList<object> GetItems();

        /// <summary>
        ///     Obtiene el número de elementos de modelo.
        /// </summary>
        /// <returns>Número de elementos.</returns>
        int ItemsCount();

        /// <summary>
        ///     Obtiene el elemento en la posición <c>index</c>.
        /// </summary>
        /// <param name="index">Posición.</param>
        /// <returns>Elemento.</returns>
        object GetItem(int index);

        /// <summary>
        ///     Obtiene el índice del elemento <c>item</c>.
        /// </summary>
        /// <param name="item">Elemento.</param>
        /// <returns>Índice del elemento.</returns>
        int IndexOfItem(object item);

        /// <summary>
        ///     Notifica que se ha modificado el modelo.
        ///     Insercion de nodos (estructural):
        ///     Se envia el evento ModeloListaChanged con los parametros:
        ///     - Change  : InsertItems
        ///     - Items   : elementos insertados
        ///     - Indices : indices de los elementos insertados
        ///     Eliminacion de nodos (estructural):
        ///     Se envia el evento ModeloListaChanged con los parametros:
        ///     - Change  : RemoveItems
        ///     - Items   : elementos eliminados
        ///     - Indices : indices de los elementos eliminados
        ///     Modificacion de nodos (no estructural):
        ///     Se envia el evento ModeloListaChanged con los parametros:
        ///     - Change  : ChangeItems
        ///     - Items   : elementos modificados
        ///     - Indices : indices de los elementos modificados
        /// </summary>
        event EventHandler<ListModelEventArgs> ModeloListaChanged;
    }
}