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
    public class TransformCollection<TI, TO> : AbsCollection<TO>
    {
        public TransformCollection(ICollection<TI> collection,
                                   Func<TI, TO> map,
                                   Func<TO, TI> reverseMap)
        {
            this.collection = collection;
            this.map = map;
            this.reverseMap = reverseMap;
        }

        #region private

        private readonly ICollection<TI> collection;
        private readonly Func<TI, TO> map;
        private readonly Func<TO, TI> reverseMap;

        #endregion

        #region AbsCollection<TO>

        protected override void CollectionAdd(TO item)
        {
            this.collection.Add(this.reverseMap(item));
        }

        #endregion

        #region ICollection<TO>

        public override void Clear()
        {
            this.collection.Clear();
        }

        public override bool Contains(TO item)
        {
            return this.collection.Contains(this.reverseMap(item));
        }

        public override int Count
        {
            get { return this.collection.Count; }
        }

        public override bool IsReadOnly
        {
            get { return this.collection.IsReadOnly; }
        }

        public override bool Remove(TO item)
        {
            return this.collection.Remove(this.reverseMap(item));
        }

        #endregion

        #region IEnumerable<TO>

        public override IEnumerator<TO> GetEnumerator()
        {
            return new TransformEnumerator<TI, TO>(this.collection.GetEnumerator(), this.map);
        }

        #endregion
    }
}