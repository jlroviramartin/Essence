using System;

namespace org.apache.commons.math3.analysis.exception
{
    public class NotStrictlyPositiveException : NumberIsTooSmallException
    {
        public NotStrictlyPositiveException(string str, double q)
        {
        }

        public NotStrictlyPositiveException(double q)
        {
            throw new NotImplementedException();
        }
    }
}