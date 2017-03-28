using System;
using Essence.Util.Converters;

namespace Essence.View.Models
{
    public interface IProperty : IComponentUI, IFormattable, IParseable
    {
        /// <summary>
        /// Proveedor de servicios para la accion.
        /// </summary>
        IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// Obtiene el tipo del valor.
        /// </summary>
        Type Valuetype { get; }

        /// <summary>
        /// Obtiene/establece el valor de la propiedad.
        /// </summary>
        object Value { get; set; }

        /// <summary>
        /// Indica si esta habilitada.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Indica si es editable.
        /// </summary>
        bool Editable { get; set; }
    }
}