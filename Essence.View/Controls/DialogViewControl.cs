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
using Essence.View.Views;
using DialogResult = Essence.View.Views.DialogResult;

namespace Essence.View.Controls
{
    public class DialogViewControl<TControl> : ViewContainerControl<TControl>,
                                               IDialogView
        where TControl : Form, new()
    {
        protected virtual void OnClosing(CancelEventArgs args)
        {
            if (this.Closing != null)
            {
                this.Closing(this, args);
            }
        }

        protected virtual void OnClosed(EventArgs args)
        {
            if (this.Closed != null)
            {
                this.Closed(this, args);
            }
        }

        private void Control_Closing(object sender, CancelEventArgs args)
        {
            this.OnClosing(args);
        }

        private void Control_Closed(object sender, EventArgs args)
        {
            this.OnClosed(args);
        }

        #region ViewContainerControl<TControl>

        public override void SetConstraints(IView view, object constraints)
        {
            base.SetConstraints(view, constraints);
            ControlUtils.ApplyConstraints(this, view, constraints);
        }

        #endregion

        #region ViewControl<TControl>

        protected override void AttachControl()
        {
            base.AttachControl();
            this.Control.Closing += this.Control_Closing;
            this.Control.Closed += this.Control_Closed;
        }

        protected override void DeattachControl()
        {
            this.Control.Closing -= this.Control_Closing;
            this.Control.Closed -= this.Control_Closed;
            base.DeattachControl();
        }

        #endregion

        #region IDialogView

        public void Show()
        {
            this.Control.Show();
        }

        public DialogResult ShowAsDialog()
        {
            System.Windows.Forms.DialogResult dialogResult = this.Control.ShowDialog();
            switch (dialogResult)
            {
                case System.Windows.Forms.DialogResult.Yes:
                case System.Windows.Forms.DialogResult.OK:
                    return DialogResult.Yes;
                case System.Windows.Forms.DialogResult.No:
                case System.Windows.Forms.DialogResult.None:
                    return DialogResult.No;
                case System.Windows.Forms.DialogResult.Cancel:
                case System.Windows.Forms.DialogResult.Abort:
                    return DialogResult.Cancel;
                case System.Windows.Forms.DialogResult.Retry:
                case System.Windows.Forms.DialogResult.Ignore:
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        public event EventHandler<CancelEventArgs> Closing;

        public event EventHandler Closed;

        #endregion
    }

    public enum DockConstraints
    {
        None,
        Top,
        Bottom,
        Left,
        Right,
        Fill,
    }
}