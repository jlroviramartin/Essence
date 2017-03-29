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

namespace Essence.View.Views
{
    public interface IViewContainer : IView
    {
        /// <summary>
        ///     Vistas.
        /// </summary>
        IList<IView> Views { get; }

        /// <summary>
        ///     Añade la vista <c>vista</c> con las restricciones <c>restricciones</c>.
        /// </summary>
        /// <param name="view">Vista.</param>
        /// <param name="constraints">Restricciones.</param>
        void AddView(IView view, object constraints);

        /// <summary>
        ///     inserta la vista <c>vista</c> con las restricciones <c>restricciones</c>.
        /// </summary>
        /// <param name="index">Indice.</param>
        /// <param name="view">Vista.</param>
        /// <param name="constraints">Restricciones.</param>
        void InsertView(int index, IView view, object constraints);
    }
}