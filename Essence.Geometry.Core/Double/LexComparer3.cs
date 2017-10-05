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