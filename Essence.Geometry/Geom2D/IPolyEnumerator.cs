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
using Essence.Util.Math;
using Essence.Util.Math.Double;

namespace Essence.Geometry.Geom2D
{
    public interface IPolyEnumerator<TPunto>
    {
        bool Next();

        bool Prev();

        int Index { get; }

        TPunto Point { get; }

        bool Equals(IPolyEnumerator<TPunto> other);

        IPolyEnumerator<TPunto> Clone();
    }

    public class PolyEnumerator<TPunto> : IPolyEnumerator<TPunto>
    {
        public PolyEnumerator(IList<TPunto> points, int index = 0)
        {
            this.points = points;
            this.index = index;
        }

        public bool Next()
        {
            this.index++;
            if (this.index >= this.points.Count)
            {
                this.index = 0;
            }
            return true;
        }

        public bool Prev()
        {
            this.index--;
            if (this.index < 0)
            {
                this.index = this.points.Count - 1;
            }
            return true;
        }

        public int Index
        {
            get { return this.index; }
        }

        public TPunto Point
        {
            get { return this.points[this.index]; }
        }

        public bool Equals(IPolyEnumerator<TPunto> other)
        {
            return this.index == other.Index;
        }

        public IPolyEnumerator<TPunto> Clone()
        {
            IPolyEnumerator<TPunto> copy = (IPolyEnumerator<TPunto>)this.MemberwiseClone();
            return copy;
        }

        private readonly IList<TPunto> points;
        private int index;
    }

    public class PolyEnumeratorRobust<TPunto> : IPolyEnumerator<TPunto>
        where TPunto : IEpsilonEquatable<TPunto>
    {
        public PolyEnumeratorRobust(IList<TPunto> points, int index = 0, bool findFirstEqual = false, double epsilon = MathUtils.EPSILON)
        {
            this.points = points;
            this.index = index;
            this.epsilon = epsilon;

            if (findFirstEqual)
            {
                this.FindFirstEqual();
            }
        }

        public bool Next()
        {
            TPunto curr = this.points[this.index];
            int count = 0;
            do
            {
                count++;
                this.index++;
                if (this.index >= this.points.Count)
                {
                    this.index = 0;
                }
            } while ((count < this.points.Count) && this.points[this.index].EpsilonEquals(curr, this.epsilon));
            return count < this.points.Count;
        }

        public bool Prev()
        {
            TPunto curr = this.points[this.index];
            int count = 0;
            do
            {
                count++;
                this.index--;
                if (this.index < 0)
                {
                    this.index = this.points.Count - 1;
                }
            } while ((count < this.points.Count) && this.points[this.index].EpsilonEquals(curr, this.epsilon));
            if (count >= this.points.Count)
            {
                return false;
            }
            return this.FindFirstEqual();
        }

        public int Index
        {
            get { return this.index; }
        }

        public TPunto Point
        {
            get { return this.points[this.index]; }
        }

        public bool Equals(IPolyEnumerator<TPunto> other)
        {
            return this.index == other.Index;
        }

        public IPolyEnumerator<TPunto> Clone()
        {
            IPolyEnumerator<TPunto> copy = (IPolyEnumerator<TPunto>)this.MemberwiseClone();
            return copy;
        }

        private bool FindFirstEqual()
        {
            TPunto curr = this.points[this.index];
            int count = 0;

            int prevIndex = this.index;
            do
            {
                this.index = prevIndex;

                count++;
                prevIndex--;
                if (prevIndex < 0)
                {
                    prevIndex = this.points.Count - 1;
                }
            } while ((count < this.points.Count) && this.points[prevIndex].EpsilonEquals(curr, this.epsilon));
            return count < this.points.Count;
        }

        private readonly IList<TPunto> points;
        private int index;
        private readonly double epsilon;
    }
}