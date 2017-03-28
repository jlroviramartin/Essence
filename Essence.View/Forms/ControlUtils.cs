using System;
using System.Windows.Forms;
using Essence.Util.Logs;

namespace Essence.View.Forms
{
    public static class ControlUtils
    {
        /// <summary>
        /// Ejecuta el delegado <c>func</c> en la tarea de interface de usuario del
        /// control <c>control</c>. Espera a que termine.
        /// </summary>
        public static T UIThread<TControl, T>(this TControl control, Func<TControl, T> func)
            where TControl : Control
        {
            if (control.IsHandleCreated && !control.IsDisposed && control.InvokeRequired)
            {
                try
                {
                    return (T)control.Invoke(func, control);
                }
                catch (Exception e)
                {
                    Log<object>.Error(e);
                    throw;
                }
            }
            else
            {
                return func(control);
            }
        }
    }
}