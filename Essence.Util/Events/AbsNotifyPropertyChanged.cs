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

using System;
using System.ComponentModel;
using PropertyChangedExEventHandler = Essence.Util.Events.EventHandler_v2<Essence.Util.Events.PropertyChangedExEventArgs>;

namespace Essence.Util.Events
{
    public abstract class AbsNotifyPropertyChanged : INotifyPropertyChangedEx,
                                                     INotifyPropertyChangedEx_Helper
    {
        #region protected

        protected void OnPropertyChanged(string name, object oldValue, object value)
        {
            this.OnPropertyChanged(new PropertyChangedExEventArgs(name, oldValue, value));
        }

        protected virtual void OnPropertyChanged(PropertyChangedExEventArgs args)
        {
            PropertyChangedExEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        #endregion

        #region INotifyPropertyChangedEx_Helper

        void INotifyPropertyChangedEx_Helper.NotifyPropertyChanged(PropertyChangedExEventArgs args)
        {
            this.OnPropertyChanged(args);
        }

        #endregion

        #region INotifyPropertyChangedEx

        public event PropertyChangedExEventHandler PropertyChanged;

        #endregion

        #region INotifyPropertyChanged

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { this.PropertyChanged += new PropertyChangedExEventHandler(value); }
            remove
            {
                foreach (Delegate @delegate in this.PropertyChanged.GetInvocationList())
                {
                    PropertyChangedEventHandler aux = @delegate.Target as PropertyChangedEventHandler;
                    if (aux == value)
                    {
                        this.PropertyChanged -= (PropertyChangedExEventHandler)@delegate;
                        return;
                    }
                }
            }
        }

        #endregion
    }
}