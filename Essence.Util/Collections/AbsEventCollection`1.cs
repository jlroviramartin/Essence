#region License

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

#endregion

using System;
using System.Collections.Generic;
using Essence.Util.Events;

namespace Essence.Util.Collections
{
    public abstract class AbsEventCollection<T> : AbsCollection<T>,
                                                  IEventCollection<T>,
                                                  IEventCollection
    {
        /// <summary>
        ///     Permite establecer quien sera el 'sender' para los eventos de las colecciones.
        /// </summary>
        public object Container { get; set; }

        #region Operaciones sin eventos

        protected abstract void AddImpl(T item);

        protected abstract bool RemoveImpl(T item);

        protected virtual void AddAllImpl(IEnumerable<T> enumer)
        {
            foreach (T item in enumer)
            {
                this.AddImpl(item);
            }
        }

        protected virtual void RemoveAllImpl(IEnumerable<T> enumer)
        {
            foreach (T item in enumer)
            {
                this.RemoveImpl(item);
            }
        }

        protected virtual void ClearImpl()
        {
            foreach (T item in this.ToArray())
            {
                this.RemoveImpl(item);
            }
        }

        #endregion

        /// <summary>
        ///     Se emite el evento <c>IEventCollection.CollectionChanged</c>.
        /// </summary>
        /// <param name="args">Argumentos.</param>
        protected virtual void OnCollectionChanged(CollectionEventArgs args)
        {
            if (this.CollectionChanged != null)
            {
                args.Who = this.Container;
                this.CollectionChanged(this, args);
            }
        }

        /// <summary>
        ///     Indica si tiene listeners.
        /// </summary>
        protected bool ContainsListeners
        {
            get { return this.CollectionChanged != null; }
        }

        protected void Modify(Action action, Func<CollectionEventArgs> fargs)
        {
            if (!this.ContainsListeners)
            {
                action();
                return;
            }

            CollectionEventArgs args = fargs();
            action();
            this.OnCollectionChanged(args);
        }

        protected TRet Modify<TRet>(Func<TRet> action, Func<CollectionEventArgs> fargs)
        {
            if (!this.ContainsListeners)
            {
                return action();
            }

            CollectionEventArgs args = fargs();
            TRet ret = action();
            this.OnCollectionChanged(args);
            return ret;
        }

        #region IEventCollection<T> / IEventCollection

        public event EventHandler_v2<CollectionEventArgs> CollectionChanged;

        #endregion

        #region AbsCollection<T>

        public sealed override void AddAll(IEnumerable<T> enumer)
        {
            this.Modify(
                () => this.AddAllImpl(enumer),
                () => CollectionEventArgs.NewEventAddRange(enumer));
        }

        public sealed override void RemoveAll(IEnumerable<T> enumer)
        {
            this.Modify(
                () => this.RemoveAllImpl(enumer),
                () => CollectionEventArgs.NewEventRemoveRange(enumer));
        }

        #endregion

        #region ICollection<T>

        protected sealed override void CollectionAdd(T item)
        {
            this.Modify(
                () => this.AddImpl(item),
                () => CollectionEventArgs.NewEventAddRange(new[] { item }));
        }

        public sealed override bool Remove(T item)
        {
            return this.Modify(
                () => this.RemoveImpl(item),
                () => CollectionEventArgs.NewEventRemoveRange(new[] { item }));
        }

        public sealed override void Clear()
        {
            this.Modify(
                () => this.ClearImpl(),
                () => CollectionEventArgs.NewEventClear(this));
        }

        #endregion
    }
}