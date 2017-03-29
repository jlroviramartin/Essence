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
    /// <summary>
    ///     Argumentos para los eventos producidos por el modelo de lista.
    /// </summary>
    public sealed class ListModelEventArgs : EventArgs
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="item">Elemento afectado.</param>
        /// <param name="index">Indice del elemento afectado.</param>
        /// <param name="change">Tipo de cambio del elemento afectado.</param>
        public ListModelEventArgs(object item, int index, ListModelChange change)
            : this(new object[] { item }, new int[] { index }, change)
        {
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="items">Elementos afectados.</param>
        /// <param name="indices">Indices de los elementos afectados.</param>
        /// <param name="change">Tipo de cambios de los elementos afectados.</param>
        public ListModelEventArgs(IList<object> items, IList<int> indices, ListModelChange change)
        {
            //Debug.Assert(items != null, "items");
            //Debug.Assert(indices != null && indices.Length == items.Length, "indices");

            this.Items = items;
            this.Indices = indices;
            this.Change = change;
        }

        /// <summary>
        ///     Elementos afectados.
        /// </summary>
        public IList<object> Items { get; }

        /// <summary>
        ///     Índices de los elementos afectados.
        /// </summary>
        public IList<int> Indices { get; }

        /// <summary>
        ///     Tipo de cambio de los elementos afectados
        /// </summary>
        public ListModelChange Change { get; }
    }
}