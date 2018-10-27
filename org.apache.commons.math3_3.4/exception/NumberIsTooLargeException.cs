using System;

namespace org.apache.commons.math3.exception
{
    public class NumberIsTooLargeException : MathIllegalNumberException
    {
        public NumberIsTooLargeException(string str, double lower, double upper, bool b)
        {
        }

        public NumberIsTooLargeException(int maximalIterationCount, int midpointMaxIterationsCount, bool upper)
        {
        }
    }
}