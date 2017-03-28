using System;
using Essence.Util;
using Essence.Util.Events;

namespace Essence.View.Views
{
    public interface IView : INotifyPropertyChangedEx, IDisposableEx
    {
        /// <summary>
        /// Padre.
        /// </summary>
        IViewContainer Container { get; /*internal*/ set; }

        /// <summary>
        /// Nombre de la vista.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Nombre.
        /// </summary>
        string NameUI { get; set; }

        /// <summary>
        /// Descripción.
        /// </summary>
        string DescriptionUI { get; set; }

        /// <summary>
        /// Icono.
        /// </summary>
        Icon IconUI { get; set; }

        /// <summary>
        /// Proveedor de servicios para la vista.
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
