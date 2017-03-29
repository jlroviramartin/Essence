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