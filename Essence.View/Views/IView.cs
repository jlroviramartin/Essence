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
using Essence.Util;
using Essence.Util.Events;

namespace Essence.View.Views
{
    public interface IView : INotifyPropertyChangedEx, IDisposableEx
    {
        /// <summary>
        ///     Padre.
        /// </summary>
        IViewContainer Container { get; /*internal*/ set; }

        /// <summary>
        ///     Nombre de la vista.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        ///     Nombre.
        /// </summary>
        string NameUI { get; set; }

        /// <summary>
        ///     Descripción.
        /// </summary>
        string DescriptionUI { get; set; }

        /// <summary>
        ///     Icono.
        /// </summary>
        Icon IconUI { get; set; }

        /// <summary>
        ///     Proveedor de servicios para la vista.
        /// </summary>
        IServiceProvider ServiceProvider { get; set; }

        /*/// <summary>
        /// Indica si la vista es visible.
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        /// Indica si la vista esta habilitada.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Obtiene la posicion de la vista <c>(X, Y)</c> en unidades
        /// del dispositivo.
        /// </summary>
        Point2d Location { get; set; }

        /// <summary>
        /// Obtiene el tamaño de la vista <c>(Ancho, Alto)</c> en unidades
        /// del dispositivo.
        /// </summary>
        Vector2d Size { get; set; }

        /// <summary>
        /// Muestra en primer plano la vista. Si no es visible, la hace visible.
        /// </summary>
        void BringToFront();*/
    }
}