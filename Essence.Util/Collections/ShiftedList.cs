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
    public class ShiftedList<T> : BaseList<T>
    {
        public ShiftedList(IList<T> items, int shiftLeft)
        {
            shiftLeft %= items.Count;
            if (shiftLeft < 0)
            {
                shiftLeft = items.Count + shiftLeft;
            }

            this.items = items;
            this.shift = shiftLeft;
        }

        public int Shift
        {
            get { return this.shift; }
        }

        private readonly IList<T> items;
        private readonly int shift;

        #region List<T>

        public override T this[int index]
        {
            get
            {
                index = index + this.shift;
                if (index >= this.Count)
                {
                    index = index - this.items.Count;
                }
                /*else if (index < 0) {
                index = this.items.Count + index;
                }*/
                return this.items[index];
            }
            set
            {
                index = index + this.shift;
                if (index >= this.Count)
                {
                    index = index - this.items.Count;
                }
                /*else if (index < 0) {
                index = this.items.Count + index;
                }*/
                this.items[index] = value;
            }
        }

        public override int Count
        {
            get { return this.items.Count; }
        }

        #endregion
    }
}