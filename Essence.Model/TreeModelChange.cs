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

namespace Essence.Model
{
    /// <summary>
    ///     Tipo de cambios que se pueden producir.
    /// </summary>
    public enum TreeModelChange
    {
        /// <summary>
        ///     Se inserta un nodo (o varios nodos hermanos).
        /// </summary>
        InsertNodes,

        /// <summary>
        ///     Se elimina un nodo (o varios nodos hermanos).
        /// </summary>
        RemoveNodes,

        /// <summary>
        ///     Se cambian la estructura de un nodo.
        /// </summary>
        StructureChange,

        /// <summary>
        ///     Se modifica un nodo (o varios nodos hermanos). No se modifica la estructura.
        /// </summary>
        ChangeNodes
    }
}