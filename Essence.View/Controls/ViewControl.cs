﻿using System;
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
            set { this.Set(CONTAINER, this.container, value, x => this.container = x); }
        }

        public string Name
        {
            get { return this.name; }
            set { this.Set(NAME, this.name, value, x => this.name = x); }
        }

        public string NameUI
        {
            get { return this.nameUI; }
            set { this.Set(NAME_UI, this.nameUI, value, x => this.nameUI = x); }
        }

        public string DescriptionUI
        {
            get { return this.descriptionUI; }
            set { this.Set(DESCRIPTION_UI, this.descriptionUI, value, x => this.descriptionUI = x); }
        }

        public Icon IconUI
        {
            get { return this.iconUI; }
            set { this.Set(ICON_UI, this.iconUI, value, x => this.iconUI = x); }
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