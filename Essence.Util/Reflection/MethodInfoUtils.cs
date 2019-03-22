using System;
using System.Reflection;

namespace Essence.Util.Reflection
{
    public static class MethodInfoUtils
    {
        public static TDelegate CreateDelegate<TDelegate>(this MethodInfo method) where TDelegate : class
        {
            return Delegate.CreateDelegate(typeof(TDelegate), method) as TDelegate;
        }
    }
}
