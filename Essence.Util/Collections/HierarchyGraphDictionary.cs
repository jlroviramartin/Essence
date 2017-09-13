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
using System.Linq;

namespace Essence.Util.Collections
{
    public class HierarchyGraphDictionary<TK, TV> : AbsDictionary<TK, TV>
    {
        public HierarchyGraphDictionary(IHGraph hgraph, IEqualityComparer<TK> comparer = null)
        {
            this.originalValues = new EventDictionary<TK, TV>(comparer);
            this.originalValues.CollectionChanged += (sender, args) =>
            {
                this.InvalidateCache();
            };

            this.hgraph = hgraph;
        }

        public IEventDictionary<TK, TV> OriginalValues
        {
            get { return this.originalValues; }
        }

        /// <summary>
        ///     Obtiene los valores asociados a la clave (this [index] devuelve solo
        ///     el primer valor).
        /// </summary>
        public IEnumerable<TV> Get(TK key)
        {
            ICollection<TV> values;
            this.FindInCache(key, out values);
            return values;
        }

        #region private

        /// <summary>
        ///     Invalida la cache.
        /// </summary>
        private void InvalidateCache()
        {
            this.cacheValues.Clear();
        }

        /// <summary>
        ///     Actualiza la cache.
        /// </summary>
        private void UpdateCache()
        {
            if ((this.originalValues.Count > 0) && (this.cacheValues.Count == 0))
            {
                foreach (KeyValuePair<TK, TV> kv in this.originalValues)
                {
                    this.cacheValues.Add(kv.Key, new[] { kv.Value });
                }
            }
        }

        /// <summary>
        ///     Busca en la cache (la actualiza si procede).
        /// </summary>
        private bool FindInCache(TK key, out ICollection<TV> values)
        {
            this.UpdateCache();

            if (this.cacheValues.TryGetValue(key, out values))
            {
                return values.Count > 0;
            }

            // Se busca según la jerarquía.
            // Importante: se busca en 'originalValues'! Evitamos que los resultados tengan
            //             valores repetidos.
            List<TV> result = new List<TV>();

            HashSet<TK> processed = new HashSet<TK>();
            Queue<TK> parents = new Queue<TK>();

            processed.Add(key);
            parents.EnqueueAll(this.hgraph.GetParents(key));

            while (parents.Count > 0)
            {
                TK parent = parents.Dequeue();
                if (!processed.Contains(parent))
                {
                    processed.Add(parent);

                    TV value;
                    if (this.originalValues.TryGetValue(parent, out value))
                    {
                        result.Add(value);
                    }
                    else
                    {
                        parents.EnqueueAll(this.hgraph.GetParents(parent));
                    }
                }
            }

            // Solo se guarda en cache las claves con resultado.
            // TODO: MEJOR TODAS!!!
            if (result.Count > 0)
            {
                this.cacheValues.Add(key, result.ToArray());

                values = result;
                return true;
            }

            values = empty;
            return false;
        }

        private static readonly TV[] empty = new TV[0];
        private readonly EventDictionary<TK, TV> originalValues;

        private readonly IHGraph hgraph;
        private readonly Dictionary<TK, ICollection<TV>> cacheValues = new Dictionary<TK, ICollection<TV>>();

        #endregion

        #region IDictionary<TK,TV>

        public override void Add(TK key, TV value)
        {
            this.OriginalValues.Add(key, value);
        }

        public override bool Remove(TK key)
        {
            return this.OriginalValues.Remove(key);
        }

        public override bool TryGetValue(TK key, out TV value)
        {
            ICollection<TV> values;
            if (!this.FindInCache(key, out values))
            {
                value = default(TV);
                return false;
            }
            value = values.First();
            return true;
        }

        #endregion

        #region ICollection<KeyValuePair<TK,TV>>

        public override int Count
        {
            get { return this.OriginalValues.Count; }
        }

        public override void Clear()
        {
            this.OriginalValues.Clear();
        }

        #endregion

        #region ICollection<KeyValuePair<TK,TV>>

        public override IEnumerator<KeyValuePair<TK, TV>> GetEnumerator()
        {
            return this.OriginalValues.GetEnumerator();
        }

        #endregion

        #region Inner classes

        public interface IHGraph
        {
            IEnumerable<TK> GetParents(TK key);
        }

        #endregion
    }
}