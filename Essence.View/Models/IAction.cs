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

namespace Essence.View.Models
{
    public interface IAction : IComponentUI
    {
        /// <summary>
        ///     Proveedor de servicios para la accion.
        /// </summary>
        IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        ///     Indica si la accion esta habilitada.
        ///     Una accion habilitada, es la que se puede ejecutar en un momento
        ///     determinado.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        ///     Indica si la accion esta activa.
        ///     Una accion activa, es la que se esta ejecutando en un momento
        ///     determinado. Se utiliza cuando la acción realiza tareas en background.
        /// </summary>
        bool Active { get; set; }

        void Invoke();

        event EventHandler Invoking;
        event EventHandler Invoked;
        event EventHandler<UpdateStateEventArgs> UpdateState;
    }
}