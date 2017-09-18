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
    public class CircularList<T> : BaseList<T>
    {
        public CircularList(IList<T> items)
        {
            this.items = items;
        }

        private readonly IList<T> items;

        #region List<T>

        public override T this[int index]
        {
            get
            {
                index %= this.items.Count;
                if (index < 0)
                {
                    index += this.items.Count;
                }
                return this.items[index];
            }
            set
            {
                index %= this.items.Count;
                if (index < 0)
                {
                    index += this.items.Count;
                }
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