using System;

namespace Essence.View.Models
{
    public interface IAction : IComponentUI
    {
        /// <summary>
        /// Proveedor de servicios para la accion.
        /// </summary>
        IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// Indica si la accion esta habilitada.
        /// Una accion habilitada, es la que se puede ejecutar en un momento
        /// determinado.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Indica si la accion esta activa.
        /// Una accion activa, es la que se esta ejecutando en un momento
        /// determinado. Se utiliza cuando la acción realiza tareas en background.
        /// </summary>
        bool Active { get; set; }

        void Invoke();

        event EventHandler Invoking;
        event EventHandler Invoked;
        event EventHandler<UpdateStateEventArgs> UpdateState;
    }
}