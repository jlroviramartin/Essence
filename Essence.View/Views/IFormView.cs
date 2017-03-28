using Essence.View.Models;

namespace Essence.View.Views
{
    public interface IFormView : IView
    {
        IFormModel FormModel { get; set; }
    }
}