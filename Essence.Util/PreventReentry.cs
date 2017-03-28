using System;

namespace Essence.Util
{
    /// <summary>
    ///     Clase de ayuda para prevenir reentradas en el codigo (llamadas recursivas, StackOverflowException).
    /// </summary>
    public sealed class PreventReentry
    {
        public PreventReentry()
        {
        }

        public void Do(Action action)
        {
            if (this.execAction)
            {
                return;
            }
            try
            {
                this.execAction = true;
                action();
            }
            finally
            {
                this.execAction = false;
            }
        }

        private bool execAction;
    }
}