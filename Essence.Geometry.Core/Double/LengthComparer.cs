using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Essence.Util.Math;
using Essence.Util.Math.Double;

namespace Essence.Geometry.Core.Double
{
    /// <summary>
    /// Compares vectors using their length.
    /// </summary>
    public sealed class LengthComparer<TVector> : IComparer<TVector>, IComparer
        where TVector : IVector<TVector>, IEpsilonEquatable<TVector>
    {
        public LengthComparer(double epsilon)
        {
            this.epsilon = epsilon;
        }

        private readonly double epsilon;

        public int Compare(TVector v1, TVector v2)
        {
            return v1.LengthSquared.EpsilonCompareTo(v2.LengthSquared, this.epsilon);
        }

        int IComparer.Compare(object o1, object o2)
        {
            Contract.Requires(o1 is Vector4d && o2 is Vector4d);
            return this.Compare((TVector)o1, (TVector)o2);
        }
    }
}