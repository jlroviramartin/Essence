using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Essence.Math.Test
{
    public class AssertEx
    {
        public static void AssertException<TE>(Action acc)
            where TE : Exception
        {
            bool exception = false;
            try
            {
                acc();
            }
            catch (TE)
            {
                exception = true;
            }
            Assert.IsTrue(exception);
        }

        public static void AssertException<TE>(Action acc, Func<TE, bool> test)
            where TE : Exception
        {
            bool exception = false;
            try
            {
                acc();
            }
            catch (TE e)
            {
                exception = test(e);
            }
            Assert.IsTrue(exception);
        }
    }
}