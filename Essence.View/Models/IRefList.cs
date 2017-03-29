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
using System.Collections;
using System.Collections.Generic;

namespace Essence.View.Models
{
    /// <summary>
    ///     Obtiene una colección de elementos.
    /// </summary>
    public interface IRefList
    {
        IEnumerable GetItems(IServiceProvider provider, object source);
    }

    /// <summary>
    ///     Obtiene una colección de elementos <c>TItem</c>.
    /// </summary>
    public interface IRefList<out TItem> : IRefList
    {
        new IEnumerable<TItem> GetItems(IServiceProvider proveedor, object obj);
    }

    /// <summary>
    ///     Obtiene una colección de elementos <c>TItem</c> utilizando datos estaticos.
    /// </summary>
    public abstract class AbsStaticRefList<TItem> : IRefList<TItem>
    {
        /// <summary>
        ///     Obtiene una colección de elementos.
        ///     Método de ayuda indicando que el proveedor es <c>null</c>.
        /// </summary>
        public IEnumerable<TItem> GetItems()
        {
            return this.GetItems(null);
        }

        /// <summary>
        ///     Obtiene una colección de elementos.
        /// </summary>
        public abstract IEnumerable<TItem> GetItems(IServiceProvider proveedor);

        #region IRefList

        IEnumerable IRefList.GetItems(IServiceProvider proveedor, object obj)
        {
            return this.GetItems(proveedor);
        }

        #endregion

        #region IRefList<TItem>

        IEnumerable<TItem> IRefList<TItem>.GetItems(IServiceProvider proveedor, object obj)
        {
            return this.GetItems(proveedor);
        }

        #endregion
    }

    /// <summary>
    ///     Obtiene una lista de elementos <c>TItem</c> utilizando datos relativos al tipo <c>TBase</c>.
    /// </summary>
    public abstract class AbsRefList<TBase, TItem> : IRefList<TItem>
    {
        /// <summary>
        ///     Obtiene una colección de elementos.
        ///     Método de ayuda indicando que el proveedor es <c>null</c>.
        /// </summary>
        public IEnumerable<TItem> GetItems(TBase obj)
        {
            return this.GetItems(null, obj);
        }

        /// <summary>
        ///     Obtiene una colección de elementos.
        /// </summary>
        public abstract IEnumerable<TItem> GetItems(IServiceProvider proveedor, TBase obj);

        #region IRefList

        IEnumerable IRefList.GetItems(IServiceProvider proveedor, object obj)
        {
            return this.GetItems(proveedor, (TBase)obj);
        }

        #endregion

        #region IRefList<TItem>

        IEnumerable<TItem> IRefList<TItem>.GetItems(IServiceProvider proveedor, object obj)
        {
            return this.GetItems(proveedor, (TBase)obj);
        }

        #endregion
    }
}