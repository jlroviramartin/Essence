using System;
using System.Collections.Generic;

namespace Essence.View.Views
{
    public interface IViewContainer : IView
    {
        /// <summary>
        /// Vistas.
        /// </summary>
        IList<IView> Views { get; }

        /// <summary>
        /// Añade la vista <c>vista</c> con las restricciones <c>restricciones</c>.
        /// </summary>
        /// <param name="view">Vista.</param>
        /// <param name="constraints">Restricciones.</param>
        void AddView(IView view, object constraints);

        /// <summary>
        /// inserta la vista <c>vista</c> con las restricciones <c>restricciones</c>.
        /// </summary>
        /// <param name="index">Indice.</param>
        /// <param name="view">Vista.</param>
        /// <param name="constraints">Restricciones.</param>
        void InsertView(int index, IView view, object constraints);
    }
}