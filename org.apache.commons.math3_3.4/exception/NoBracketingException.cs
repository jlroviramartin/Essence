using System;

namespace org.apache.commons.math3.exception
{
    public class NoBracketingException : MathIllegalArgumentException
    {
        public NoBracketingException(double min, double max, double yMin, double yMax)
            : base()
        {
        }

        public NoBracketingException(string str, double xLo, double yMin, double yMax, double fHi, int i, int maxEval, double baseRoot, double min, double max)
            : base()
        {
        }
    }
}