using System;
using System.Collections.Generic;

namespace Essence.Model
{
    public interface IListModel
    {
        /// <summary>
        ///     Obtiene los elementos del modelo.
        /// </summary>
        /// <returns>Elementos.</returns>
        IList<object> GetItems();

        /// <summary>
        ///     Obtiene el número de elementos de modelo.
        /// </summary>
        /// <returns>Número de elementos.</returns>
        int ItemsCount();

        /// <summary>
        ///     Obtiene el elemento en la posición <c>index</c>.
        /// </summary>
        /// <param name="index">Posición.</param>
        /// <returns>Elemento.</returns>
        object GetItem(int index);

        /// <summary>
        ///     Obtiene el índice del elemento <c>item</c>.
        /// </summary>
        /// <param name="item">Elemento.</param>
        /// <returns>Índice del elemento.</returns>
        int IndexOfItem(object item);

        /// <summary>
        ///     Notifica que se ha modificado el modelo.
        ///     Insercion de nodos (estructural):
        ///     Se envia el evento ModeloListaChanged con los parametros:
        ///     - Change  : InsertItems
        ///     - Items   : elementos insertados
        ///     - Indices : indices de los elementos insertados
        ///     Eliminacion de nodos (estructural):
        ///     Se envia el evento ModeloListaChanged con los parametros:
        ///     - Change  : RemoveItems
        ///     - Items   : elementos eliminados
        ///     - Indices : indices de los elementos eliminados
        ///     Modificacion de nodos (no estructural):
        ///     Se envia el evento ModeloListaChanged con los parametros:
        ///     - Change  : ChangeItems
        ///     - Items   : elementos modificados
        ///     - Indices : indices de los elementos modificados
        /// </summary>
        event EventHandler<ListModelEventArgs> ModeloListaChanged;
    }
}