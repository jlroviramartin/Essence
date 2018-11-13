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
using System.Collections.Generic;
using System.Linq;
using Essence.Util.Events;

namespace Essence.Util.Collections
{
    public abstract class AbsEventList<T> : AbsList<T>,
                                            IEventList<T>,
                                            IEventList
    {
        /// <summary>
        /// Permite establecer quien sera el 'sender' para los eventos de las colecciones.
        /// </summary>
        public object Container { get; set; }

        #region Operaciones sin eventos

        protected abstract void InsertImpl(int index, T item);

        protected abstract void RemoveAtImpl(int index);

        protected abstract void SetImpl(int index, T item);

        protected virtual void InsertAllImpl(int index, IEnumerable<T> enumer)
        {
            foreach (T t in enumer)
            {
                this.Insert(index++, t);
            }
        }

        protected virtual void RemoveAllImpl(int index, int count)
        {
            for (int i = count - 1; i >= index; i--)
            {
                this.RemoveAt(index);
            }
        }

        protected virtual void RemoveAllImpl(IEnumerable<T> enumer)
        {
            foreach (T item in enumer)
            {
                int index = this.IndexOf(item);
                if (index >= 0)
                {
                    this.RemoveAt(index);
                }
            }
        }

        protected virtual void ClearImpl()
        {
            for (int i = this.Count - 1; i >= 0; i--)
            {
                this.RemoveAtImpl(i);
            }
        }

        #endregion

        /// <summary>
        /// Se emite el evento <c>IEventCollection.CollectionChanged</c>.
        /// </summary>
        /// <param name="args">Argumentos.</param>
        protected virtual void OnListChanged(ListEventArgs args)
        {
            if (this.ListChanged != null)
            {
                args.Who = this.Container;
                this.ListChanged(this, args);
            }
        }

        /// <summary>
        /// Indica si tiene listeners.
        /// </summary>
        protected bool ContainsListeners
        {
            get { return this.ListChanged != null; }
        }

        protected void Modify(Action action, Func<ListEventArgs> fargs)
        {
            if (!this.ContainsListeners)
            {
                action();
                return;
            }

            ListEventArgs args = fargs();
            action();
            this.OnListChanged(args);
        }

        protected bool ModifyIf(Func<bool> action, Func<ListEventArgs> fargs)
        {
            if (!this.ContainsListeners)
            {
                return action();
            }

            ListEventArgs args = fargs();
            if (!action())
            {
                return false;
            }
            if (!args.IsEmpty)
            {
                this.OnListChanged(args);
            }
            return true;
        }

        #region IEventList<T> / IEventList

        public event EventHandler_v2<ListEventArgs> ListChanged;

        #endregion

        #region IEventCollection<T> / IEventCollection

        public event EventHandler_v2<CollectionEventArgs> CollectionChanged
        {
            add { this.ListChanged += value; }
            remove { this.ListChanged -= value; }
        }

        #endregion

        #region AbsList<T>

        public sealed override void InsertAll(int index, IEnumerable<T> enumer)
        {
            this.Modify(
                () => this.InsertAllImpl(index, enumer),
                () => ListEventArgs.NewEventAddRange(index, enumer));
        }

        public sealed override void RemoveAll(int index, int count)
        {
            this.Modify(
                () => this.RemoveAllImpl(index, count),
                () => ListEventArgs.NewEventRemoveRange(index, Enumerable.Range(index, count).Select(i => this[i])));
        }

        #endregion

        #region AbsCollection<T>

        public sealed override void AddAll(IEnumerable<T> enumer)
        {
            this.Modify(
                () => this.InsertAllImpl(this.Count, enumer),
                () => ListEventArgs.NewEventAddRange(this.Count, enumer));
        }

        public sealed override void RemoveAll(IEnumerable<T> enumer)
        {
            this.Modify(
                () => this.RemoveAllImpl(enumer),
                () => ListEventArgs.NewEventRemoveRange(enumer.Select(this.IndexOf), enumer));
        }

        #endregion

        #region IList<T>

        public sealed override void Insert(int index, T item)
        {
            this.Modify(
                () => this.InsertImpl(index, item),
                () => ListEventArgs.NewEventAddRange(new[] { index }, new[] { item }));
        }

        public sealed override void RemoveAt(int index)
        {
            this.Modify(
                () => this.RemoveAtImpl(index),
                () => ListEventArgs.NewEventRemoveRange(new[] { index }, new[] { this[index] }));
        }

        public override T this[int index]
        {
            set
            {
                this.Modify(
                    () => this.SetImpl(index, value),
                    () => ListEventArgs.NewEventChange(index, value, this[index]));
            }
        }

        #endregion

        #region ICollection<T>

        protected sealed override void CollectionAdd(T item)
        {
            this.Insert(this.Count, item);
        }

        public sealed override bool Remove(T item)
        {
            int index = this.IndexOf(item);
            if (index < 0)
            {
                return false;
            }
            this.RemoveAt(index);
            return true;
        }

        public sealed override void Clear()
        {
            this.Modify(
                () => this.ClearImpl(),
                () => ListEventArgs.NewEventClear(this));
        }

        #endregion
    }
}