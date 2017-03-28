using Essence.Util.Events;

namespace Essence.View.Models
{
    public interface IComponentUI : INotifyPropertyChangedEx
    {
        string Name { get; set; }

        string NameUI { get; set; }
        string DescriptionUI { get; set; }
        Icon IconUI { get; set; }
    }
}