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
using System.Diagnostics.Contracts;
using System.Text;

namespace Essence.Model
{
    public class TreePath
    {
        /// <summary>
        ///     Separación de los elementos del path.
        /// </summary>
        public const string PATH_SEP = "/";

        /// <summary>
        ///     Crea un path con los elementos <c>items</c>.
        /// </summary>
        /// <param name="items">Elementos del path.</param>
        /// <returns>Path.</returns>
        public static TreePath NewPath(params object[] items)
        {
            Contract.Requires(items != null, "item");

            if (items.Length == 0)
            {
                return null;
            }
            return BuildPath(items, 0, items.Length);
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="item">Elemento del path.</param>
        public TreePath(object item)
            : this(null, item)
        {
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="parent">Path padre.</param>
        /// <param name="item">Elemento del path.</param>
        public TreePath(TreePath parent, object item)
        {
            Contract.Requires(item != null, "item");

            this.parent = parent;
            this.item = item;
        }

        /// <summary>
        ///     Crea un path para el elemento <c>item</c> como
        ///     hijo del path actual.
        /// </summary>
        /// <param name="item">Elemento del path.</param>
        /// <returns>Path hijo.</returns>
        public TreePath NewChild(object item)
        {
            Contract.Requires(item != null, "item");

            return new TreePath(this, item);
        }

        /// <summary>
        ///     Crea un path para el elemento <c>item</c> como
        ///     hermano del path actual.
        /// </summary>
        /// <param name="item">Elemento del path.</param>
        /// <returns>Path hermano.</returns>
        public TreePath NewSibling(object item)
        {
            Contract.Requires(item != null, "item");

            return new TreePath(this.parent, item);
        }

        /// <summary>
        ///     Path padre o <c>null</c> si no tiene.
        /// </summary>
        public TreePath Parent
        {
            get { return this.parent; }
        }

        /// <summary>
        ///     Elemento raiz.
        ///     <example>(a/b/c).Root -> a</example>
        /// </summary>
        public object Root
        {
            get
            {
                if (this.parent != null)
                {
                    return this.parent.Root;
                }
                return this.item;
            }
        }

        /// <summary>
        ///     Obtiene un path de tamaño <c>depth</c>
        ///     a partir del path actual.
        ///     <example>(a/b/c).Subruta(1) -> (c)</example>
        ///     <example>(a/b/c).Subruta(2) -> (b/c)</example>
        ///     <example>(a/b/c).Subruta(3) -> (a/b/c)</example>
        /// </summary>
        /// <param name="depth">Profundidad.</param>
        /// <returns>Path.</returns>
        public TreePath SubPath(int depth)
        {
            Contract.Requires((depth > 0) && ((this.parent != null) || (depth == 1)), "depth");

            if (depth > 1)
            {
                TreePath subpath = this.parent.SubPath(depth - 1);
                if (this.parent == subpath)
                {
                    return this;
                }
                return new TreePath(subpath, this.item);
            }
            else if (depth == 1)
            {
                if (this.parent == null)
                {
                    return this;
                }
                return new TreePath(this.item);
            }

            throw new Exception("Assert fail.");
        }

        /// <summary>
        ///     Elemento del path.
        /// </summary>
        public object Item
        {
            get { return this.item; }
        }

        /// <summary>
        ///     Profundidad del path. Es el número de elementos de la raiz
        ///     al path actual.
        ///     <example>(a).Depth -> 1</example>
        ///     <example>(a/b).Depth -> 2</example>
        ///     <example>(a/b/c).Depth -> 3</example>
        /// </summary>
        public int Depth
        {
            get
            {
                int depth = 1;
                if (this.parent != null)
                {
                    depth += this.parent.Depth;
                }
                return depth;
            }
        }

        /// <summary>
        ///     Copia al path a un array. El array es de izquierda (raiz)
        ///     a derecha (path actual).
        ///     <example>(a/b/c).CopyTo(..) -> [a,b,c]</example>
        /// </summary>
        public void CopyTo(Array array)
        {
            int depth = this.Depth;

            Contract.Requires((array != null) && (array.Length >= depth), "array");

            TreePath curr = this;
            for (int i = 0; i < depth; i++)
            {
                array.SetValue(curr.item, depth - i - 1);
                curr = curr.parent;
            }
        }

        /// <summary>
        ///     Crea un array con el path. El array es de izquierda (raiz)
        ///     a derecha (path actual).
        ///     <example>(a/b/c).ToArray() -> [a,b,c]</example>
        /// </summary>
        public object[] ToArray()
        {
            int depth = this.Depth;

            object[] path = new object[depth];

            TreePath curr = this;
            for (int i = 0; i < depth; i++)
            {
                path[depth - i - 1] = curr.item;
                curr = curr.parent;
            }

            return path;
        }

        public string ToString(string sep)
        {
            StringBuilder buff = new StringBuilder();
            if (this.parent != null)
            {
                buff.Append(this.parent.ToString(sep));
                buff.Append(sep);
            }
            buff.Append(this.item.ToString());
            return buff.ToString();
        }

        #region private

        /// <summary>
        ///     Crea un path a partir del array <c>items</c>
        ///     entre los elementos <c>beg</c> y <c>end</c>.
        /// </summary>
        private static TreePath BuildPath(object[] items, int beg, int end)
        {
            Contract.Requires(items != null, "items");
            Contract.Requires(beg >= 0, "beg");
            Contract.Requires((end > beg) && (end <= items.Length), "end");

            if (beg < end - 1)
            {
                TreePath parent = BuildPath(items, beg, end - 1);
                return new TreePath(parent, items[end - 1]);
            }

            return new TreePath(items[end - 1]);
        }

        /// <summary>Ruta del padre.</summary>
        private readonly TreePath parent;

        /// <summary>Elemento del path.</summary>
        private readonly object item;

        #endregion

        #region Object

        public override bool Equals(object obj)
        {
            TreePath rutaArbol = obj as TreePath;
            if (rutaArbol == null)
            {
                return false;
            }

            if ((this.parent == rutaArbol.parent)
                || ((this.parent != null) && this.parent.Equals(rutaArbol.parent)))
            {
                return (this.item == rutaArbol.item)
                       || ((this.item != null) && this.item.Equals(rutaArbol.item));
            }

            return false;
        }

        public override int GetHashCode()
        {
            int hash = 0;
            if (this.parent != null)
            {
                hash ^= this.parent.GetHashCode();
            }
            hash ^= this.item.GetHashCode();
            return hash;
        }

        public override string ToString()
        {
            return this.ToString(PATH_SEP);
        }

        #endregion
    }
}