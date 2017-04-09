using System;

namespace org.apache.commons.math3.analysis.exception
{
    public class MaxCountExceededException : MathIllegalStateException
    {
        public MaxCountExceededException(int max)
        {
        }

        public int Max { get; set; }
    }
}