using System;
using System.Collections.Generic;
using Essence.Util.Events;
using Essence.View.Models.Properties;

namespace Essence.View.Models
{
    public abstract class AbsFormModel : AbsNotifyPropertyChanged,
                                         IFormModel
    {
        #region protected

        protected void OnContentChanged(EventArgs args)
        {
            if (this.ContentChanged != null)
            {
                this.ContentChanged(this, args);
            }
        }

        protected void OnStructureChanged(EventArgs args)
        {
            if (this.StructureChanged != null)
            {
                this.StructureChanged(this, args);
            }
        }

        #endregion

        #region IFormModel

        public abstract IEnumerable<string> GetProperties();

        public abstract IEnumerable<string> GetCategories();

        public abstract IEnumerable<string> GetProperties(string category);

        public abstract Type GetPropertyType(string name);

        public abstract object GetPropertyValue(string name);

        public abstract void SetPropertyValue(string name, object value);

        public abstract PropertyDescription GetDescription(string name);

        public abstract IEnumerable<ViewProperties> GetViewProperties(string name);

        public event EventHandler<EventArgs> ContentChanged;
        public event EventHandler<EventArgs> StructureChanged;

        #endregion
    }
}