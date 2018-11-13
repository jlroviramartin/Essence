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

namespace Essence.Util.Collections
{
    public class HierarchyTreeDictionary<TK, TV> : AbsDictionary<TK, TV>
    {
        public HierarchyTreeDictionary(IHTree htree, IEqualityComparer<TK> comparer = null)
        {
            this.originalValues = new EventDictionary<TK, TV>(comparer);
            this.originalValues.CollectionChanged += (sender, args) =>
            {
                this.InvalidateCache();
            };

            this.htree = htree;
        }

        public IEventDictionary<TK, TV> OriginalValues
        {
            get { return this.originalValues; }
        }

        #region private

        /// <summary>
        /// Invalida la cache.
        /// </summary>
        private void InvalidateCache()
        {
            this.cacheValues.Clear();
        }

        /// <summary>
        /// Actualiza la cache.
        /// </summary>
        private void UpdateCache()
        {
            if ((this.originalValues.Count > 0) && (this.cacheValues.Count == 0))
            {
                foreach (KeyValuePair<TK, TV> kv in this.originalValues)
                {
                    this.cacheValues.Add(kv.Key, kv.Value);
                }
            }
        }

        /// <summary>
        /// Busca en la cache (la actualiza si procede).
        /// </summary>
        private bool FindInCache(TK key, out TV value)
        {
            this.UpdateCache();

            if (this.cacheValues.TryGetValue(key, out value))
            {
                return true;
            }

            // Se busca según la jerarquía.
            TK key2 = this.htree.GetParent(key);
            while (key2 != null)
            {
                if (this.cacheValues.TryGetValue(key2, out value))
                {
                    this.cacheValues.Add(key, value);
                    return true;
                }
                key2 = this.htree.GetParent(key2);
            }

            value = default(TV);
            return false;
        }

        private readonly EventDictionary<TK, TV> originalValues;

        private readonly IHTree htree;
        private readonly Dictionary<TK, TV> cacheValues = new Dictionary<TK, TV>();

        #endregion

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
            return this.FindInCache(key, out value);
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

        #region Inner classes

        public interface IHTree
        {
            TK GetParent(TK key);
        }

        #endregion
    }
}