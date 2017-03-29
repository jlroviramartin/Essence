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

namespace Essence.Util.Collections
{
    public abstract class AbsList<T> : AbsCollection<T>,
                                       IList<T>,
                                       IList
    {
        public virtual void InsertAll(int index, IEnumerable<T> enumer)
        {
            foreach (T t in enumer)
            {
                this.Insert(index++, t);
            }
        }

        public virtual void RemoveAll(int index, int count)
        {
            for (int i = 0; i < count; i++)
            {
                this.RemoveAt(index);
            }
        }

        #region IList<T>

        public virtual int IndexOf(T item)
        {
            for (int i = 0, count = this.Count; i < count; i++)
            {
                if (this.EqualityComparer.Equals(this[i], item))
                {
                    return i;
                }
            }
            return -1;
        }

        public abstract void Insert(int index, T item);

        public abstract void RemoveAt(int index);

        public abstract T this[int index] { get; set; }

        #endregion

        #region ICollection<T>

        public override bool Contains(T item)
        {
            return this.IndexOf(item) != -1;
        }

        #endregion

        #region IList

        int IList.Add(object value)
        {
            int index = this.IndexOf((T)value);
            this.Add((T)value);
            return index;
        }

        bool IList.Contains(object value)
        {
            return this.Contains((T)value);
        }

        int IList.IndexOf(object value)
        {
            return this.IndexOf((T)value);
        }

        void IList.Insert(int index, object value)
        {
            this.Insert(index, (T)value);
        }

        public virtual bool IsFixedSize
        {
            get { return false; }
        }

        void IList.Remove(object value)
        {
            this.Remove((T)value);
        }

        object IList.this[int index]
        {
            get { return this[index]; }
            set { this[index] = (T)value; }
        }

        #endregion

        #region IEnumerable<T>

        public override IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        #endregion

        #region ICollection<T>

        public override bool Remove(T item)
        {
            int index = this.IndexOf(item);
            if (index < 0)
            {
                return false;
            }

            this.RemoveAt(index);
            return true;
        }

        public override void Clear()
        {
            for (int i = this.Count - 1; i >= 0; i--)
            {
                this.RemoveAt(i);
            }
        }

        #endregion

        #region AbsCollection<T>

        protected override void CollectionAdd(T item)
        {
            this.Insert(this.Count, item);
        }

        #endregion

        #region Inner classes

        private sealed class Enumerator : AbsEnumerator<T>
        {
            public Enumerator(AbsList<T> outer)
            {
                this.outer = outer;
            }

            private readonly AbsList<T> outer;
            private int index = -1;

            public override T Current
            {
                get { return this.outer[this.index]; }
            }

            public override bool MoveNext()
            {
                this.index++;
                return (this.index < this.outer.Count);
            }
        }

        #endregion
    }

    public class BaseList<T> : AbsList<T>
    {
        public override void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public override void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public override T this[int index]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }
}