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
    public class TransformEnumerator<TI, TO> : AbsEnumerator<TO>
    {
        public TransformEnumerator(IEnumerator<TI> enumerator,
                                   Func<TI, TO> map)
        {
            this.enumerator = enumerator;
            this.map = map;
        }

        #region private

        private readonly IEnumerator<TI> enumerator;
        private readonly Func<TI, TO> map;

        #endregion

        #region IEnumerator<TO>

        public override TO Current
        {
            get { return this.map(this.enumerator.Current); }
        }

        public override bool MoveNext()
        {
            return this.enumerator.MoveNext();
        }

        #endregion
    }
}