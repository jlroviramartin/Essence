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
    public interface ISelectionModel
    {
        /// <summary>
        ///     Selección actual.
        /// </summary>
        IEnumerable<object> Selection { get; }

        /// <summary>
        ///     Obtiene el número de elementos seleccionados.
        /// </summary>
        /// <returns>Numero de elementos seleccionados.</returns>
        int SelectionCount { get; }

        /// <summary>
        ///     Comprueba si el elemento <c>elemento</c> esta seleccionado.
        /// </summary>
        /// <param name="elemento">Elemento.</param>
        /// <returns>Indica si esta seleccionado.</returns>
        bool IsSelected(object elemento);

        #region Operaciones seleccion

        /// <summary>
        ///     Establece la seleccion a <c>elemento</c>.
        /// </summary>
        /// <param name="item">Elemento.</param>
        void SetSelection(object item);

        /// <summary>
        ///     Establece la seleccion a <c>elementos</c>.
        /// </summary>
        /// <param name="items">Elementos.</param>
        void SetSelectionRange(IEnumerable<object> items);

        /// <summary>
        ///     Añade el elemento <c>elemento</c> a la selección.
        /// </summary>
        /// <param name="item">Elemento a añadir.</param>
        void AddSelection(object item);

        /// <summary>
        ///     Añade los elementos <c>elementos</c> a la selección.
        /// </summary>
        /// <param name="items">Elementos a añadir.</param>
        void AddSelectionRange(IEnumerable<object> items);

        /// <summary>
        ///     Elimina el elemento <c>elemento</c> de la selección.
        /// </summary>
        /// <param name="item">Elemento a eliminar.</param>
        void RemoveSelection(object item);

        /// <summary>
        ///     Elimina los elementos <c>elements</c> de la selección.
        /// </summary>
        /// <param name="items">Elementos a eliminar.</param>
        void RemoveSelectionRange(IEnumerable<object> items);

        /// <summary>
        ///     Elimina todos los elementos de la selección.
        /// </summary>
        void ClearSelection();

        #endregion Operaciones seleccion

        /// <summary>
        ///     Notifica que se ha modificado el modelo.
        /// </summary>
        event EventHandler<SelectionModelEventArgs> SelectionModelChanged;
    }
}