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

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Essence.Util.Math;
using Essence.Util.Math.Double;

namespace Essence.Geometry.Core.Double
{
    /// <summary>
    /// Lexicographer comparer: 1st compares by X, 2nd by Y and 3rd by Z.
    /// </summary>
    public sealed class LexComparer3<TTuple> : IComparer<TTuple>, IComparer
        where TTuple : ITuple3_Double, IEpsilonEquatable<TTuple>
    {
        public LexComparer3(double epsilon)
        {
            this.epsilon = epsilon;
        }

        private readonly double epsilon;

        public int Compare(TTuple v1, TTuple v2)
        {
            int i;
            i = v1.X.EpsilonCompareTo(v2.X, this.epsilon);
            if (i != 0)
            {
                return i;
            }
            i = v1.Y.EpsilonCompareTo(v2.Y, this.epsilon);
            if (i != 0)
            {
                return i;
            }
            i = v1.Z.EpsilonCompareTo(v2.Z, this.epsilon);
            return i;
        }

        int IComparer.Compare(object o1, object o2)
        {
            Contract.Requires(o1 is Vector4d && o2 is Vector4d);
            return this.Compare((TTuple)o1, (TTuple)o2);
        }
    }
}