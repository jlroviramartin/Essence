using System;
using org.apache.commons.math3.util;

namespace org.apache.commons.math3.analysis.exception
{
    public class NonMonotonicSequenceException : Exception
    {
        public NonMonotonicSequenceException(double d, double previous, int index, MathArrays.OrderDirection dir, bool strict)
        {
            throw new NotImplementedException();
        }
    }
}