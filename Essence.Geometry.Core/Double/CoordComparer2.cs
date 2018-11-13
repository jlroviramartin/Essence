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
using System.Diagnostics.Contracts;
using Essence.Util.Math;
using Essence.Util.Math.Double;

namespace Essence.Geometry.Core.Double
{
    /// <summary>
    /// Coordinate comparer: comparer based on the coordinate (X or Y).
    /// </summary>
    public sealed class CoordComparer2<TTuple> : IComparer<TTuple>, IComparer
        where TTuple : ITuple2_Double, IEpsilonEquatable<TTuple>
    {
        public CoordComparer2(int coord, double epsilon)
        {
            this.coord = coord;
            this.epsilon = epsilon;
        }

        private readonly int coord;
        private readonly double epsilon;

        public int Compare(TTuple v1, TTuple v2)
        {
            switch (this.coord)
            {
                case 0:
                    return v1.X.EpsilonCompareTo(v2.X, this.epsilon);
                case 1:
                    return v1.Y.EpsilonCompareTo(v2.Y, this.epsilon);
            }
            throw new IndexOutOfRangeException();
        }

        int IComparer.Compare(object o1, object o2)
        {
            Contract.Requires(o1 is Vector2d && o2 is Vector2d);
            return this.Compare((TTuple)o1, (TTuple)o2);
        }
    }
}