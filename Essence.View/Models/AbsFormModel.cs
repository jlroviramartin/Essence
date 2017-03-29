// Copyright 2017 Jose Luis Rovira Martin
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

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