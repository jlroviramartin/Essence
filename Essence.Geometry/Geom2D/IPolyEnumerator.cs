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
    public interface IPolyEnumerator<TPoint>
    {
        bool Next();

        bool Prev();

        int Index { get; }

        TPoint Point { get; }

        bool Equals(IPolyEnumerator<TPoint> other);

        IPolyEnumerator<TPoint> Clone();
    }

    public class PolyEnumerator<TPoint> : IPolyEnumerator<TPoint>
    {
        public PolyEnumerator(IList<TPoint> points, int index = 0)
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

        public TPoint Point
        {
            get { return this.points[this.index]; }
        }

        public bool Equals(IPolyEnumerator<TPoint> other)
        {
            return this.index == other.Index;
        }

        public IPolyEnumerator<TPoint> Clone()
        {
            IPolyEnumerator<TPoint> copy = (IPolyEnumerator<TPoint>)this.MemberwiseClone();
            return copy;
        }

        private readonly IList<TPoint> points;
        private int index;
    }

    public class PolyEnumeratorRobust<TPoint> : IPolyEnumerator<TPoint>
        where TPoint : IEpsilonEquatable<TPoint>
    {
        public PolyEnumeratorRobust(IList<TPoint> points, int index = 0, bool findFirstEqual = false, double epsilon = MathUtils.EPSILON)
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
            TPoint curr = this.points[this.index];
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
            TPoint curr = this.points[this.index];
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

        public TPoint Point
        {
            get { return this.points[this.index]; }
        }

        public bool Equals(IPolyEnumerator<TPoint> other)
        {
            return this.index == other.Index;
        }

        public IPolyEnumerator<TPoint> Clone()
        {
            IPolyEnumerator<TPoint> copy = (IPolyEnumerator<TPoint>)this.MemberwiseClone();
            return copy;
        }

        private bool FindFirstEqual()
        {
            TPoint curr = this.points[this.index];
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

        private readonly IList<TPoint> points;
        private int index;
        private readonly double epsilon;
    }
}