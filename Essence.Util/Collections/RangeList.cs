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
    public class RangeList<T> : BaseList<T>
    {
        public RangeList(IList<T> items, int index, int count, bool circular = false)
        {
            if (!circular)
            {
                if (index + count > items.Count)
                {
                    throw new Exception();
                }
            }
            else
            {
                if (count > items.Count)
                {
                    throw new Exception();
                }
            }
            this.items = items;
            this.index = index;
            this.count = count;
        }

        public int Index
        {
            get { return this.index; }
        }

        private readonly IList<T> items;
        private readonly int index;
        private readonly int count;

        #region List<T>

        public override T this[int index]
        {
            get
            {
                index = (index + this.index) % this.items.Count;
                return this.items[index];
            }
            set
            {
                index = (index + this.index) % this.items.Count;
                this.items[index] = value;
            }
        }

        public override int Count
        {
            get { return this.count; }
        }

        #endregion
    }
}