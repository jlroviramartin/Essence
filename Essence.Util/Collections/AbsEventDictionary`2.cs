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
using Essence.Util.Events;

namespace Essence.Util.Collections
{
    public abstract class AbsEventDictionary<TK, TV> : AbsDictionary<TK, TV>,
                                                       IEventDictionary<TK, TV>,
                                                       IEventDictionary,
                                                       IEventCollection<KeyValuePair<TK, TV>>,
                                                       IEventCollection
    {
        /// <summary>
        /// Permite establecer quien sera el 'sender' para los eventos de las colecciones.
        /// </summary>
        public object Container { get; set; }

        #region Operaciones sin eventos

        protected abstract void AddImpl(TK key, TV value);

        protected abstract bool RemoveImpl(TK key);

        protected abstract void SetImpl(TK key, TV value);

        protected virtual void AddAllImpl(IEnumerable<KeyValuePair<TK, TV>> enumer)
        {
            foreach (KeyValuePair<TK, TV> kv in enumer)
            {
                this.AddImpl(kv.Key, kv.Value);
            }
        }

        protected virtual void RemoveAllImpl(IEnumerable<TK> enumer)
        {
            foreach (TK k in enumer)
            {
                this.RemoveImpl(k);
            }
        }

        protected virtual void ClearImpl()
        {
            foreach (KeyValuePair<TK, TV> kv in this.ToArray())
            {
                this.RemoveImpl(kv.Key);
            }
        }

        #endregion

        /// <summary>
        /// Se emite el evento <c>IEventCollection&lt;KeyValuePair&lt;TK,TV>>.CollectionChanged</c>.
        /// </summary>
        /// <param name="args">Argumentos.</param>
        protected virtual void OnDictionaryChanged(DictionaryEventArgs args)
        {
            if (this.DictionaryChanged != null)
            {
                args.Who = this.Container;
                this.DictionaryChanged(this, args);
            }
        }

        /// <summary>
        /// Indica si tiene listeners.
        /// </summary>
        protected bool ContainsListeners
        {
            get { return (this.DictionaryChanged != null); }
        }

        protected void Modify(Action action, Func<DictionaryEventArgs> fargs)
        {
            if (!this.ContainsListeners)
            {
                action();
                return;
            }

            DictionaryEventArgs args = fargs();
            action();
            if (!args.IsEmpty)
            {
                this.OnDictionaryChanged(args);
            }
        }

        protected bool ModifyIf(Func<bool> action, Func<DictionaryEventArgs> fargs)
        {
            if (!this.ContainsListeners)
            {
                return action();
            }

            DictionaryEventArgs args = fargs();
            if (!action())
            {
                return false;
            }
            if (!args.IsEmpty)
            {
                this.OnDictionaryChanged(args);
            }
            return true;
        }

        #region IEventDictionary<TK,TV> / IEventDictionary

        public event EventHandler_v2<DictionaryEventArgs> DictionaryChanged;

        #endregion

        #region IEventCollection<KeyValuePair<TK,TV>> / IEventCollection

        public event EventHandler_v2<CollectionEventArgs> CollectionChanged
        {
            add { this.DictionaryChanged += value; }
            remove { this.DictionaryChanged -= value; }
        }

        #endregion

        #region AbsDictionary<TK,TV>

        public sealed override void RemoveAllKeys(IEnumerable<TK> enumer)
        {
            this.Modify(
                () => this.RemoveAllImpl(enumer),
                () =>
                {
                    List<TK> keys = new List<TK>();
                    List<TV> values = new List<TV>();
                    foreach (TK k in enumer)
                    {
                        TV v;
                        if (this.TryGetValue(k, out v))
                        {
                            keys.Add(k);
                            values.Add(v);
                        }
                    }
                    return DictionaryEventArgs.NewEventRemoveRange(keys, values);
                });
        }

        #endregion

        #region AbsCollection<KeyValuePair<TK,TV>>

        public sealed override void AddAll(IEnumerable<KeyValuePair<TK, TV>> enumer)
        {
            this.Modify(
                () => this.AddAllImpl(enumer),
                () =>
                {
                    List<TK> keys = new List<TK>();
                    List<TV> values = new List<TV>();
                    foreach (KeyValuePair<TK, TV> kvp in enumer)
                    {
                        if (!this.ContainsKey(kvp.Key))
                        {
                            keys.Add(kvp.Key);
                            values.Add(kvp.Value);
                        }
                    }
                    return DictionaryEventArgs.NewEventAddRange(keys, values);
                });
        }

        #endregion

        #region IDictionary<TK,TV>

        public sealed override void Add(TK key, TV value)
        {
            this.Modify(
                () => this.AddImpl(key, value),
                () => DictionaryEventArgs.NewEventAddRange(new[] { key }, new[] { value }));
        }

        public sealed override bool Remove(TK key)
        {
            return this.ModifyIf(
                () => this.RemoveImpl(key),
                () =>
                {
                    TV value;
                    if (!this.TryGetValue(key, out value))
                    {
                        return DictionaryEventArgs.NewEventRemoveRange(new TK[0], new TV[0]);
                    }
                    return DictionaryEventArgs.NewEventRemoveRange(new[] { key }, new[] { value });
                });
        }

        public override TV this[TK key]
        {
            set
            {
                this.Modify(
                    () => this.SetImpl(key, value),
                    () => DictionaryEventArgs.NewEventChange(key, value, this[key]));
            }
        }

        #endregion

        #region ICollection<KeyValuePair<TK,TV>>

        public sealed override void Clear()
        {
            this.Modify(
                () => this.ClearImpl(),
                () => DictionaryEventArgs.NewEventClear(this.Keys, this.Values));
        }

        #endregion
    }
}