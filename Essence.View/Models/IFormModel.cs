using System;
using System.Collections.Generic;
using Essence.Util.Events;
using Essence.View.Models.Properties;

namespace Essence.View.Models
{
    public interface IFormModel : INotifyPropertyChangedEx
    {
        IEnumerable<string> GetProperties();
        IEnumerable<string> GetCategories();
        IEnumerable<string> GetProperties(string category);

        Type GetPropertyType(string name);

        object GetPropertyValue(string name);

        void SetPropertyValue(string name, object value);

        PropertyDescription GetDescription(string name);

        IEnumerable<ViewProperties> GetViewProperties(string name);

        event EventHandler<EventArgs> ContentChanged;
        event EventHandler<EventArgs> StructureChanged;
    }
}