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

namespace Essence.Util.Collections.Iterators
{
    public struct CircularListIt<TValue>
    {
        public CircularListIt(IList<TValue> list, int i = 0)
        {
            this.list = list;
            this.i = i;
        }

        public static bool operator ==(CircularListIt<TValue> it1, CircularListIt<TValue> it2)
        {
            return it1.i == it2.i;
        }

        public static bool operator !=(CircularListIt<TValue> it1, CircularListIt<TValue> it2)
        {
            return it1.i != it2.i;
        }

        public void Inc(int c)
        {
            int count = this.list.Count;

            this.i += c;
            if (this.i >= count)
            {
                do
                {
                    this.i -= count;
                } while (this.i >= count);
            }
            else if (this.i < 0)
            {
                do
                {
                    this.i += count;
                } while (this.i < 0);
            }
        }

        public void Inc()
        {
            int count = this.list.Count;

            this.i++;
            if (this.i >= count)
            {
                this.i -= count;
            }
            else if (this.i < 0)
            {
                this.i += count;
            }
        }

        public void Dec()
        {
            int count = this.list.Count;

            this.i--;
            if (this.i >= count)
            {
                this.i -= count;
            }
            else if (this.i < 0)
            {
                this.i += count;
            }
        }

        public TValue Get()
        {
            return this.list[this.i];
        }

        #region private

        private readonly IList<TValue> list;
        private int i;

        #endregion

        #region object

        public override bool Equals(object obj)
        {
            if (!(obj is CircularListIt<TValue>))
            {
                return false;
            }
            CircularListIt<TValue> other = (CircularListIt<TValue>)obj;
            return this.i == other.i;
        }

        public override int GetHashCode()
        {
            return this.i.GetHashCode();
        }

        public override string ToString()
        {
            return this.i.ToString();
        }

        #endregion
    }
}