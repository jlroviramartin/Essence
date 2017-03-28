using System;
using System.ComponentModel;

namespace Essence.View.Views
{
    public interface IDialogView : IViewContainer
    {
        void Show();

        DialogResult ShowAsDialog();

        /// <summary>
        /// Notifica que se esta cerrando el dialogo.
        /// </summary>
        event EventHandler<CancelEventArgs> Closing;

        /// <summary>
        /// Notifica que se ha cerrado el dialogo.
        /// </summary>
        event EventHandler Closed;
    }

    public enum DialogResult
    {
        Yes, No, Cancel
    }
}