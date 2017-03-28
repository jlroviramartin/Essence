using System;
using Essence.Util;
using Essence.Util.Events;
using Essence.Util.Properties;

namespace Essence.View.Models
{
    public class Action : AbsComponentUI, IAction
    {
        public const string ENABLED = "Enabled";
        public const string ACTIVE = "Active";

        public Action()
        {
            this.Enabled = true;
        }

        public void NotifyUpdateState()
        {
            UpdateStateEventArgs args = new UpdateStateEventArgs();
            args.Enabled = this.Enabled;

            this.OnUpdateState(args);

            this.Enabled = args.Enabled;
        }

        #region protected

        protected virtual void DoInvoke()
        {
        }

        protected virtual void OnInvoking(EventArgs args)
        {
            if (this.Invoking != null)
            {
                this.Invoking(this, args);
            }
        }

        protected virtual void OnInvoked(EventArgs args)
        {
            if (this.Invoked != null)
            {
                this.Invoked(this, args);
            }
        }

        protected virtual void OnUpdateState(UpdateStateEventArgs args)
        {
            if (this.UpdateState != null)
            {
                this.UpdateState(this, args);
            }
        }


        #endregion

        #region private

        private IServiceProvider serviceProvider;

        private bool enabled;
        private bool active;

        #endregion

        #region IAction

        public IServiceProvider ServiceProvider
        {
            get
            {
                if (this.serviceProvider != null)
                {
                    return this.serviceProvider;
                }
                return EmptyServiceProvider.Instance;
            }
            set { this.serviceProvider = value; }
        }

        public bool Enabled
        {
            get { return this.enabled; }
            set { this.Set(ENABLED, this.enabled, value, x => this.enabled = x); }
        }

        public bool Active
        {
            get { return this.active; }
            set { this.Set(ACTIVE, this.active, value, x => this.active = x); }
        }

        public void Invoke()
        {
            this.OnInvoking(EventArgs.Empty);
            if (this.Enabled)
            {
                try
                {
                    this.Active = true;
                    this.DoInvoke();
                }
                finally
                {
                    this.Active = false;
                }
            }
            this.OnInvoked(EventArgs.Empty);
        }

        public event EventHandler Invoking;
        public event EventHandler Invoked;
        public event EventHandler<UpdateStateEventArgs> UpdateState;

        #endregion
    }
}