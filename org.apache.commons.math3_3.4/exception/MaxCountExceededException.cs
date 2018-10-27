using System;

namespace org.apache.commons.math3.exception
{
    public class MaxCountExceededException : MathIllegalStateException
    {
        public MaxCountExceededException(int max)
        {
        }

        public int Max { get; set; }
    }
}