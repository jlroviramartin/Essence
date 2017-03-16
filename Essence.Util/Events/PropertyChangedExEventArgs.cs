#region License

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

#endregion

using System.ComponentModel;
using Essence.Util.IO;

namespace Essence.Util.Events
{
    public class PropertyChangedExEventArgs : PropertyChangedEventArgs
    {
        public PropertyChangedExEventArgs(string propertyName, object oldValue, object newValue)
            : base(propertyName)
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }

        public object OldValue { get; }
        public object NewValue { get; }

        public override string ToString()
        {
            return new PropertiesStringBuilder()
                .AppendNamed("PropertyName", this.PropertyName)
                .AppendNamed("OldValue", this.OldValue)
                .AppendNamed("NewValue", this.NewValue)
                .ToString();
        }
    }
}