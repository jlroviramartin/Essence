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
    public class TransformList<TI, TO> : AbsList<TO>
    {
        public TransformList(IList<TI> list,
                             Func<TI, TO> map,
                             Func<TO, TI> reverseMap)
        {
            this.list = list;
            this.map = map;
            this.reverseMap = reverseMap;
        }

        #region private

        private readonly IList<TI> list;
        private readonly Func<TI, TO> map;
        private readonly Func<TO, TI> reverseMap;

        #endregion

        #region IList<TO>

        public override int IndexOf(TO item)
        {
            return this.list.IndexOf(this.reverseMap(item));
        }

        public override void Insert(int index, TO item)
        {
            this.list.Insert(index, this.reverseMap(item));
        }

        public override void RemoveAt(int index)
        {
            this.list.RemoveAt(index);
        }

        public override TO this[int index]
        {
            get { return this.map(this.list[index]); }
            set { this.list[index] = this.reverseMap(value); }
        }

        #endregion

        #region IList

        public override bool IsFixedSize
        {
            get { return (this.list as IList)?.IsFixedSize ?? false; }
        }

        #endregion

        #region ICollection<TO>

        public override void Clear()
        {
            this.list.Clear();
        }

        public override bool Remove(TO item)
        {
            return this.list.Remove(this.reverseMap(item));
        }

        #endregion

        #region IEnumerable<TO>

        public override IEnumerator<TO> GetEnumerator()
        {
            return new TransformEnumerator<TI, TO>(this.list.GetEnumerator(), this.map);
        }

        #endregion
    }
}