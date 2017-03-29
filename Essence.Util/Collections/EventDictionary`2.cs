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

using System.Collections.Generic;

namespace Essence.Util.Collections
{
    public class EventDictionary<TK, TV> : AbsEventDictionary<TK, TV>
    {
        public EventDictionary(object container)
            : this()
        {
            this.Container = container;
        }

        public EventDictionary()
        {
            this.inner = new Dictionary<TK, TV>();
        }

        public EventDictionary(IEqualityComparer<TK> comparer)
        {
            this.inner = new Dictionary<TK, TV>(comparer);
        }

        public EventDictionary(IDictionary<TK, TV> dic)
        {
            this.inner = new Dictionary<TK, TV>(dic);
        }

        public EventDictionary(IDictionary<TK, TV> dic, IEqualityComparer<TK> comparer)
        {
            this.inner = new Dictionary<TK, TV>(dic, comparer);
        }

        /// <summary>Interno.</summary>
        private readonly Dictionary<TK, TV> inner;

        #region AbsEventDictionary<TK,TV>

        protected sealed override void AddImpl(TK key, TV value)
        {
            this.inner.Add(key, value);
        }

        protected sealed override bool RemoveImpl(TK key)
        {
            return this.inner.Remove(key);
        }

        protected sealed override void SetImpl(TK key, TV value)
        {
            this.inner[key] = value;
        }

        protected sealed override void AddAllImpl(IEnumerable<KeyValuePair<TK, TV>> enumer)
        {
            this.inner.AddAll(enumer);
        }

        protected sealed override void RemoveAllImpl(IEnumerable<TK> enumer)
        {
            this.inner.RemoveAllKeys(enumer);
        }

        protected sealed override void ClearImpl()
        {
            this.inner.Clear();
        }

        #endregion

        #region IDictionary<TK,TV>

        public override ICollection<TK> Keys
        {
            get { return this.inner.Keys; }
        }

        public override ICollection<TV> Values
        {
            get { return this.inner.Values; }
        }

        public override bool ContainsKey(TK key)
        {
            return this.inner.ContainsKey(key);
        }

        public override bool TryGetValue(TK key, out TV value)
        {
            return this.inner.TryGetValue(key, out value);
        }

        public override TV this[TK key]
        {
            get { return this.inner[key]; }
        }

        #endregion

        #region ICollection<KeyValuePair<TK,TV>>

        public override void CopyTo(KeyValuePair<TK, TV>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TK, TV>>)this.inner).CopyTo(array, arrayIndex);
        }

        public override int Count
        {
            get { return this.inner.Count; }
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        #endregion

        #region IEnumerable<KeyValuePair<TK,TV>>

        public override IEnumerator<KeyValuePair<TK, TV>> GetEnumerator()
        {
            return this.inner.GetEnumerator();
        }

        #endregion
    }
}