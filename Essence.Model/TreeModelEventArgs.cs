using System;
using System.Collections.Generic;

namespace Essence.Model
{
    /// <summary>
    ///     Argumentos para los eventos producidos por el modelo de arbol.
    /// </summary>
    public class TreeModelEventArgs : EventArgs
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="path">Ruta.</param>
        /// <param name="change">Tipo de cambio.</param>
        public TreeModelEventArgs(TreePath path, TreeModelChange change)
            : this(path, (object[])null, (int[])null, change)
        {
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="path">Ruta del padre.</param>
        /// <param name="child">Hijo.</param>
        /// <param name="index">Indice del hijo.</param>
        /// <param name="change">Tipo de cambio.</param>
        public TreeModelEventArgs(TreePath path, object child, int index, TreeModelChange change)
            : this(path, new object[] { child }, new int[] { index }, change)
        {
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="path">Ruta del padre.</param>
        /// <param name="children">Hijos.</param>
        /// <param name="indices">Indices de los hijos.</param>
        /// <param name="change">Tipo de cambio.</param>
        public TreeModelEventArgs(TreePath path, IList<object> children, IList<int> indices, TreeModelChange change)
        {
            //Debug.Assert(children != null, "children");
            //Debug.Assert(indices != null && indices.Length == children.Length, "indices");

            this.Path = path;
            this.Children = children;
            this.Indices = indices;
            this.Change = change;
        }

        /// <summary>
        ///     Ruta del padre.
        /// </summary>
        public TreePath Path { get; }

        /// <summary>
        ///     Hijos afectados.
        /// </summary>
        public IList<object> Children { get; }

        /// <summary>
        ///     Índices de los hijos afectados.
        /// </summary>
        public IList<int> Indices { get; }

        /// <summary>
        ///     Tipo de cambio.
        /// </summary>
        public TreeModelChange Change { get; }
    }
}