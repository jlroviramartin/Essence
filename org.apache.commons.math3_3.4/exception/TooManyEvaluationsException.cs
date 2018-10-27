using System;

namespace org.apache.commons.math3.exception
{
    public class TooManyEvaluationsException : MaxCountExceededException
    {
        public TooManyEvaluationsException(int max)
            : base(max)
        {
        }
    }
}