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
using System.ComponentModel;
using System.Windows.Forms;
using Essence.Util;
using Essence.Util.Events;
using Essence.Util.Properties;
using Essence.View.Views;
using PropertyChangedExEventHandler = Essence.Util.Events.EventHandler_v2<Essence.Util.Events.PropertyChangedExEventArgs>;

namespace Essence.View.Controls
{
    public class ViewControl<TControl> : DisposableObject,
                                         IView,
                                         INotifyPropertyChangedEx,
                                         INotifyPropertyChangedEx_Helper,
                                         IRefControl
        where TControl : Control, new()
    {
        public const string CONTAINER = "Container";
        public const string NAME = "Name";
        public const string NAME_UI = "NameUI";
        public const string DESCRIPTION_UI = "DescriptionUI";
        public const string ICON_UI = "IconUI";
        public const string SERVICE_PROVIDER = "ServiceProvider";

        public ViewControl()
        {
            this.control = new TControl();
            this.DoAttach(true);
        }

        #region protected

        protected TControl Control
        {
            get { return this.control; }
        }

        protected static IView GetView(Control control)
        {
            return control.Tag as IView;
        }

        protected static void SetView(Control control, IView view)
        {
            control.Tag = view;
        }

        protected void DoAttach(bool attach)
        {
            if (attach)
            {
                if (!this.attached)
                {
                    this.attached = true;
                    this.AttachControl();
                }
            }
            else
            {
                if (this.attached)
                {
                    this.attached = false;
                    this.DeattachControl();
                }
            }
        }

        protected virtual void AttachControl()
        {
            SetView(this.Control, this);
        }

        protected virtual void DeattachControl()
        {
            SetView(this.Control, null);
        }

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

        #region private

        private bool attached;

        private IViewContainer container;

        private string name;
        private string nameUI;
        private string descriptionUI;
        private Icon iconUI;
        private IServiceProvider serviceProvider;

        private readonly TControl control;

        #endregion

        #region IView

        public IViewContainer Container
        {
            get { return this.container; }
            set { this.Set(ref this.container, value); }
        }

        public string Name
        {
            get { return this.name; }
            set { this.Set(ref this.name, value); }
        }

        public string NameUI
        {
            get { return this.nameUI; }
            set { this.Set(ref this.nameUI, value); }
        }

        public string DescriptionUI
        {
            get { return this.descriptionUI; }
            set { this.Set(ref this.descriptionUI, value); }
        }

        public Icon IconUI
        {
            get { return this.iconUI; }
            set { this.Set(ref this.iconUI, value); }
        }

        public IServiceProvider ServiceProvider
        {
            get
            {
                if (this.serviceProvider != null)
                {
                    return this.serviceProvider;
                }
                if (this.Container != null)
                {
                    return this.Container.ServiceProvider;
                }
                return EmptyServiceProvider.Instance;
            }
            set { this.serviceProvider = value; }
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

        #region IRefControl

        Control IRefControl.Control
        {
            get { return this.Control; }
        }

        #endregion

        #region DisposableObject

        protected override void DisposeOfManagedResources()
        {
            this.DoAttach(false);
            base.DisposeOfManagedResources();
        }

        #endregion
    }
}