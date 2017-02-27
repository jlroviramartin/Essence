using System;

namespace Essence.Util
{
    public interface IDisposableEx : IDisposable
    {
        bool IsDisposed { get; }
    }
}