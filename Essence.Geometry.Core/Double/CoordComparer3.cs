using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Essence.Util.Math;
using Essence.Util.Math.Double;

namespace Essence.Geometry.Core.Double
{
    /// <summary>
    /// Coordinate comparer: comparer based on the coordinate (X or Y or Z).
    /// </summary>
    public sealed class CoordComparer3<TTuple> : IComparer<TTuple>, IComparer
        where TTuple : ITuple3_Double, IEpsilonEquatable<TTuple>
    {
        public CoordComparer3(int coord, double epsilon)
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
            }
            throw new IndexOutOfRangeException();
        }

        int IComparer.Compare(object o1, object o2)
        {
            Contract.Requires(o1 is Point3d && o2 is Point3d);
            return this.Compare((TTuple)o1, (TTuple)o2);
        }
    }
}