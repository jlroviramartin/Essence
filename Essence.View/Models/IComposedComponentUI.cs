using Essence.Util.Collections;

namespace Essence.View.Models
{
    public interface IComposedComponentUI : IComponentUI
    {
        IEventList<IComponentUI> Components { get; }
    }
}