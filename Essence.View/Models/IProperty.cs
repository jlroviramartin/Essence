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
using Essence.Util.Converters;

namespace Essence.View.Models
{
    public interface IProperty : IComponentUI, IFormattable, IParseable
    {
        /// <summary>
        ///     Proveedor de servicios para la accion.
        /// </summary>
        IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        ///     Obtiene el tipo del valor.
        /// </summary>
        Type Valuetype { get; }

        /// <summary>
        ///     Obtiene/establece el valor de la propiedad.
        /// </summary>
        object Value { get; set; }

        /// <summary>
        ///     Indica si esta habilitada.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        ///     Indica si es editable.
        /// </summary>
        bool Editable { get; set; }
    }
}