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