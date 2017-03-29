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
using System.Linq;

namespace Essence.Util.Collections
{
    public abstract class AbsDictionary<TK, TV> : AbsCollection<KeyValuePair<TK, TV>>,
                                                  IDictionary<TK, TV>,
                                                  IDictionary
    {
        public virtual void RemoveAllKeys(IEnumerable<TK> enumer)
        {
        }

        public sealed override void RemoveAll(IEnumerable<KeyValuePair<TK, TV>> enumer)
        {
            this.RemoveAllKeys(enumer.Select(kvp => kvp.Key));
        }

        #region IDictionary<TK,TV>

        public virtual ICollection<TK> Keys
        {
            get { return new CollectionKeys(this); }
        }

        public virtual ICollection<TV> Values
        {
            get { return new CollectionValues(this); }
        }

        public abstract void Add(TK key, TV value);

        public abstract bool Remove(TK key);

        public virtual bool ContainsKey(TK key)
        {
            TV value;
            return this.TryGetValue(key, out value);
        }

        public abstract bool TryGetValue(TK key, out TV value);

        public virtual TV this[TK key]
        {
            get
            {
                TV value;
                if (!this.TryGetValue(key, out value))
                {
                    throw new KeyNotFoundException();
                }
                return value;
            }
            set
            {
                TV oldValue;
                if (!this.TryGetValue(key, out oldValue))
                {
                    throw new KeyNotFoundException();
                }
                this.Remove(key);
                this.Add(key, value);
            }
        }

        #endregion

        #region IDictionary

        void IDictionary.Add(object key, object value)
        {
            this.Add((TK)key, (TV)value);
        }

        bool IDictionary.Contains(object key)
        {
            return this.ContainsKey((TK)key);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return (IDictionaryEnumerator)this.GetEnumerator();
        }

        public virtual bool IsFixedSize
        {
            get { return false; }
        }

        ICollection IDictionary.Keys
        {
            get { return (ICollection)this.Keys; }
        }

        void IDictionary.Remove(object key)
        {
            this.Remove((TK)key);
        }

        ICollection IDictionary.Values
        {
            get { return (ICollection)this.Values; }
        }

        object IDictionary.this[object key]
        {
            get { return this[(TK)key]; }
            set { this[(TK)key] = (TV)value; }
        }

        #endregion

        #region ICollection<T>

        protected sealed override void CollectionAdd(KeyValuePair<TK, TV> item)
        {
            this.Add(item.Key, item.Value);
        }

        public sealed override bool Contains(KeyValuePair<TK, TV> item)
        {
            return this.ContainsKey(item.Key);
        }

        public sealed override bool Remove(KeyValuePair<TK, TV> item)
        {
            return this.Remove(item.Key);
        }

        public override void Clear()
        {
            TK[] array = this.Keys.ToArray();
            foreach (TK key in array)
            {
                this.Remove(key);
            }
        }

        #endregion

        #region Inner classes

        /// <summary>
        ///     Coleccion de claves para el diccionario.
        /// </summary>
        private sealed class CollectionKeys : AbsCollection<TK>
        {
            public CollectionKeys(AbsDictionary<TK, TV> outer)
            {
                this.outer = outer;
            }

            private readonly AbsDictionary<TK, TV> outer;

            public override bool Contains(TK item)
            {
                TV value;
                return this.outer.TryGetValue(item, out value);
            }

            public override int Count
            {
                get { return this.outer.Count; }
            }

            protected override void CollectionAdd(TK item)
            {
                throw new NotImplementedException();
            }

            public override bool Remove(TK item)
            {
                return this.outer.Remove(item);
            }

            public override void Clear()
            {
                this.outer.Clear();
            }

            public override IEnumerator<TK> GetEnumerator()
            {
                return new EnumeratorKeys(this.outer);
            }
        }

        /// <summary>
        ///     Coleccion de valores para el diccionario.
        /// </summary>
        private sealed class CollectionValues : AbsCollection<TV>
        {
            public CollectionValues(AbsDictionary<TK, TV> outer)
            {
                this.outer = outer;
            }

            private readonly AbsDictionary<TK, TV> outer;

            public override bool Contains(TV item)
            {
                foreach (TV v in this)
                {
                    if (object.Equals(v, item))
                    {
                        return true;
                    }
                }
                return false;
            }

            public override int Count
            {
                get { return this.outer.Count; }
            }

            protected override void CollectionAdd(TV item)
            {
                throw new NotImplementedException();
            }

            public override bool Remove(TV item)
            {
                throw new NotImplementedException();
            }

            public override void Clear()
            {
                throw new NotImplementedException();
            }

            public override IEnumerator<TV> GetEnumerator()
            {
                return new EnumeratorValues(this.outer);
            }
        }

        /// <summary>
        ///     Enumerador de claves para el diccionario.
        /// </summary>
        private sealed class EnumeratorKeys : AbsEnumerator<TK>
        {
            public EnumeratorKeys(AbsDictionary<TK, TV> outer)
            {
                this.outer = outer;
                this.enumer = this.outer.GetEnumerator();
            }

            private readonly AbsDictionary<TK, TV> outer;
            private readonly IEnumerator<KeyValuePair<TK, TV>> enumer;

            public override TK Current
            {
                get { return this.enumer.Current.Key; }
            }

            public override bool MoveNext()
            {
                return this.enumer.MoveNext();
            }
        }

        /// <summary>
        ///     Enumerador de valores para el diccionario.
        /// </summary>
        private sealed class EnumeratorValues : AbsEnumerator<TV>
        {
            public EnumeratorValues(AbsDictionary<TK, TV> outer)
            {
                this.outer = outer;
                this.enumer = this.outer.GetEnumerator();
            }

            private readonly AbsDictionary<TK, TV> outer;
            private readonly IEnumerator<KeyValuePair<TK, TV>> enumer;

            public override TV Current
            {
                get { return this.enumer.Current.Value; }
            }

            public override bool MoveNext()
            {
                return this.enumer.MoveNext();
            }
        }

        #endregion
    }

    public class BaseDictionary<TK, TV> : AbsDictionary<TK, TV>
    {
        #region IDictionary<TK,TV>

        public override void Add(TK key, TV value)
        {
            throw new NotSupportedException();
        }

        public override bool Remove(TK key)
        {
            throw new NotSupportedException();
        }

        public override bool TryGetValue(TK key, out TV value)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region ICollection<KeyValuePair<TK,TV>>

        public override int Count
        {
            get { throw new NotSupportedException(); }
        }

        #endregion

        #region ICollection<KeyValuePair<TK,TV>>

        public override IEnumerator<KeyValuePair<TK, TV>> GetEnumerator()
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}