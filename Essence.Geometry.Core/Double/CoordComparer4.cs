using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Essence.Util.Math;
using Essence.Util.Math.Double;

namespace Essence.Geometry.Core.Double
{
    /// <summary>
    /// Coordinate comparer: comparer based on the coordinate (X or Y or Z or W).
    /// </summary>
    public sealed class CoordComparer4<TTuple> : IComparer<TTuple>, IComparer
        where TTuple : ITuple4_Double, IEpsilonEquatable<TTuple>
    {
        public CoordComparer4(int coord, double epsilon)
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
                case 2:
                    return v1.Z.EpsilonCompareTo(v2.Z, this.epsilon);
                case 3:
                    return v1.W.EpsilonCompareTo(v2.W, this.epsilon);
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