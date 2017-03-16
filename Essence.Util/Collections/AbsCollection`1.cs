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
using System.Collections;
using System.Collections.Generic;

namespace Essence.Util.Collections
{
    public abstract class AbsCollection<T> : ICollection<T>,
                                             ICollection
    {
        public virtual void AddAll(IEnumerable<T> enumer)
        {
            foreach (T t in enumer)
            {
                this.Add(t);
            }
        }

        public virtual void RemoveAll(IEnumerable<T> enumer)
        {
            foreach (T t in enumer)
            {
                this.Remove(t);
            }
        }

        public T[] ToArray()
        {
            T[] array = new T[this.Count];
            this.CopyTo(array, 0);
            return array;
        }

        public IEqualityComparer<T> EqualityComparer
        {
            get { return this.comparer; }
            set { this.comparer = value ?? EqualityComparer<T>.Default; }
        }

        protected abstract void CollectionAdd(T item);

        private IEqualityComparer<T> comparer = EqualityComparer<T>.Default;

        #region ICollection<T>

        public void Add(T item)
        {
            this.CollectionAdd(item);
        }

        public virtual void Clear()
        {
            foreach (T item in this.ToArray())
            {
                this.Remove(item);
            }
        }

        public virtual bool Contains(T item)
        {
            foreach (T item2 in this)
            {
                if (this.EqualityComparer.Equals(item2, item))
                {
                    return true;
                }
            }
            return false;
        }

        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            if (arrayIndex > array.Length)
            {
                return;
            }

            int i = arrayIndex;
            foreach (T t in this)
            {
                if (i >= array.Length)
                {
                    break;
                }
                array[i++] = t;
            }
        }

        public virtual int Count
        {
            get
            {
                int count = 0;
                foreach (T item in this)
                {
                    count++;
                }
                return count;
            }
        }

        public virtual bool IsReadOnly
        {
            get { return false; }
        }

        public abstract bool Remove(T item);

        #endregion

        #region ICollection

        void ICollection.CopyTo(Array array, int index)
        {
            if (array is T[])
            {
                this.CopyTo((T[])array, index);
            }
            else
            {
                Array.Copy(this.ToArray(), 0, array, index, this.Count);
            }
        }

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        object ICollection.SyncRoot
        {
            get { throw new NotSupportedException(); }
        }

        #endregion

        #region IEnumerable<T>

        public abstract IEnumerator<T> GetEnumerator();

        #endregion

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }

    public class BaseCollection<T> : AbsCollection<T>
    {
        protected override void CollectionAdd(T item)
        {
            throw new NotImplementedException();
        }

        public override bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public override IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}