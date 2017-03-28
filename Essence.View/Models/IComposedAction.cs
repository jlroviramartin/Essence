using Essence.Util.Collections;

namespace Essence.View.Models
{
    public interface IComposedAction : IAction
    {
        IEventList<IAction> Actions { get; }
    }
}