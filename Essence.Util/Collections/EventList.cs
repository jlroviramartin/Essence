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

using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Essence.Util.Collections
{
    public class EventList<T> : AbsEventList<T>
    {
        public EventList(object container)
            : this()
        {
            this.Container = container;
        }

        public EventList()
        {
            this.inner = new List<T>();
        }

        public EventList(IEnumerable<T> inner)
        {
            this.inner = new List<T>(inner);
        }

        /// <summary>Interno.</summary>
        private readonly List<T> inner;

        #region AbsEventList<T>

        protected override void InsertImpl(int index, T item)
        {
            Contract.Requires(index <= this.inner.Count);
            this.inner.Insert(index, item);
        }

        protected override void RemoveAtImpl(int index)
        {
            this.inner.RemoveAt(index);
        }

        protected override void SetImpl(int index, T item)
        {
            this.inner[index] = item;
        }

        protected override void InsertAllImpl(int index, IEnumerable<T> enumer)
        {
            this.inner.InsertAll(index, enumer);
        }

        protected override void RemoveAllImpl(int index, int count)
        {
            this.inner.RemoveAll(index, count);
        }

        protected override void RemoveAllImpl(IEnumerable<T> enumer)
        {
            this.inner.RemoveAll(enumer);
        }

        protected override void ClearImpl()
        {
            this.inner.Clear();
        }

        #endregion

        #region IList<T>

        public override int IndexOf(T item)
        {
            return this.inner.IndexOf(item);
        }

        public override T this[int index]
        {
            get { return this.inner[index]; }
        }

        #endregion

        #region ICollection<T>

        public override bool Contains(T item)
        {
            return this.inner.Contains(item);
        }

        public override void CopyTo(T[] array, int arrayIndex)
        {
            this.inner.CopyTo(array, arrayIndex);
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

        #region IEnumerable<T>

        public override IEnumerator<T> GetEnumerator()
        {
            return this.inner.GetEnumerator();
        }

        #endregion
    }
}