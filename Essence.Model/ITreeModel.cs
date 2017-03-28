using System;
using System.Collections.Generic;

namespace Essence.Model
{
    public interface ITreeModel
    {
        /// <summary>
        ///     Raiz del modelo.
        /// </summary>
        object Root { get; }

        /// <summary>
        ///     Indica si es una hoja el nodo indicado.
        /// </summary>
        /// <param name="node">Nodo.</param>
        /// <returns>Indica si es una hoja.</returns>
        bool IsLeaf(TreePath node);

        /// <summary>
        ///     Obtiene los hijos del nodo <c>parent</c>.
        /// </summary>
        /// <param name="parent">Padre.</param>
        /// <returns>Hijos.</returns>
        IList<object> GetChildren(TreePath parent);

        /// <summary>
        ///     Obtiene el número de hijos del nodo <c>parent</c>.
        /// </summary>
        /// <param name="parent">Padre.</param>
        /// <returns>Número de hijos.</returns>
        int ChildrenCount(TreePath parent);

        /// <summary>
        ///     Obtiene el hijo en la posición <c>index</c>
        ///     del nodo <c>parent</c>.
        /// </summary>
        /// <param name="parent">Padre.</param>
        /// <param name="index">Posición del hijo.</param>
        /// <returns>Hijo.</returns>
        object GetChildAt(TreePath parent, int index);

        /// <summary>
        ///     Obtiene el índice del hijo <c>child</c>
        ///     para el nodo <c>parent</c>.
        /// </summary>
        /// <param name="parent">Padre.</param>
        /// <param name="child">Hijo.</param>
        /// <returns>Índice del hijo.</returns>
        int IndexOfChild(TreePath parent, object child);

        /// <summary>
        ///     Notifica que se ha modificado el modelo.
        ///     Insercion de nodos (estructural):
        ///     Se envia el evento ModeloArbolChanged con los parametros:
        ///     - Change   : InsertNodes
        ///     - Path     : ruta del nodo en donde se han insertado los nuevos nodos hijo (el padre)
        ///     - Children : nodos insertados (los hijos)
        ///     - Indices  : indices de los nodos insertados
        ///     Eliminacion de nodos (estructural):
        ///     Se envia el evento ModeloArbolChanged con los parametros:
        ///     - Change   : RemoveNodes
        ///     - Path     : ruta del nodo de donde se han eliminado los nodos hijo (el padre)
        ///     - Children : nodos eliminados (los hijos)
        ///     - Indices  : indices de los nodos eliminados
        ///     Cambio en la estructura de un nodo (estructural):
        ///     Un nodo ha cambiado su estructura de manera drastica.
        ///     Cuidado con este tipo de cambio.
        ///     Se envia el evento ModeloArbolChanged con los parametros:
        ///     - Change   : StructureChange
        ///     - Path     : ruta del nodo que ha cambiado la estructura de un nodo hijo (el padre)
        ///     - Children : nodo que ha cambiado la estructura
        ///     - Indices  : indice del nodo que ha cambiado la estructura
        ///     Modificacion de nodos (no estructural):
        ///     Se envia el evento ModeloArbolChanged con los parametros:
        ///     - Change   : ChangeNodes
        ///     - Path     : ruta del nodo en donde se han modificado los nodos (el padre)
        ///     - Children : nodos modificados (los hijos)
        ///     - Indices  : indices de los nodos modificados
        ///     Casos especiales:
        ///     (1) Cambio de la raiz (estructural):
        ///     Se envia el evento ModeloArbolChanged con los parametros:
        ///     - Change   : StructureChange
        ///     - Path     : null
        ///     - Children : la raiz
        ///     - Indices  : indice -1
        ///     (2) Modificacion de la raiz (no estructural):
        ///     Se envia el evento ModeloArbolChanged con los parametros:
        ///     - Change   : ChangeNodes
        ///     - Path     : null
        ///     - Children : la raiz
        ///     - Indices  : indice -1
        /// </summary>
        event EventHandler<TreeModelEventArgs> TreeModelChanged;
    }
}