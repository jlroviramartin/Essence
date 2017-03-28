using Essence.View.Models;
using Essence.Util.Collections;

namespace Essence.View.Views
{
    public interface IMenuView : IView
    {
        /// <summary>
        /// Acciones.
        /// </summary>
        IEventList<IComponentUI> Components { get; }
    }
}