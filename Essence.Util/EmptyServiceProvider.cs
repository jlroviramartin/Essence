using System;

namespace Essence.Util
{
    public sealed class EmptyServiceProvider : IServiceProvider
    {
        public static readonly IServiceProvider Instance = new EmptyServiceProvider();

        private EmptyServiceProvider()
        {
        }

        #region IServiceProvider

        public object GetService(Type serviceType)
        {
            return null;
        }

        #endregion
    }
}