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

namespace Essence.View.Forms
{
    public class LabelValueToolStrip : ToolStripControlHost
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        public LabelValueToolStrip()
            : base(new LabelValueControl())
        {
            this.CausesValidation = true;
        }

        /// <summary>
        ///     Control interno.
        /// </summary>
        public new LabelValueControl Control
        {
            get { return (LabelValueControl)base.Control; }
        }

        /// <summary>
        ///     Etiqueta.
        /// </summary>
        public string Label
        {
            get { return this.Control.UIThread((c) => c.Label); }
            set { this.Control.UIThread((c) => c.Label = value); }
        }

        /// <summary>
        ///     Ancho del control etiqueta.
        /// </summary>
        public int LabelWidth
        {
            get { return this.Control.UIThread((c) => c.LabelWidth); }
            set { this.Control.UIThread((c) => c.LabelWidth = value); }
        }

        /// <summary>
        ///     Valor.
        /// </summary>
        public string Value
        {
            get { return this.Control.UIThread((c) => c.Value); }
            set { this.Control.UIThread((c) => c.Value = value); }
        }

        /// <summary>
        ///     Ancho del control valor.
        /// </summary>
        public int ValueWidth
        {
            get { return this.Control.UIThread((c) => c.ValueWidth); }
            set { this.Control.UIThread((c) => c.ValueWidth = value); }
        }

        /// <summary>
        ///     Indica que el valor es editable.
        /// </summary>
        public bool ValueIsEditable
        {
            get { return this.Control.UIThread((c) => c.ValueIsEditable); }
            set { this.Control.UIThread((c) => c.ValueIsEditable = value); }
        }

        /// <summary>Lanza un evento indicando que se ha modificado el valor.</summary>
        public event EventHandler ValueModified;

        #region private

        private void ValueControl_LostFocus(object sender, EventArgs args)
        {
            if (this.ValueModified != null)
            {
                this.ValueModified(this, args);
            }
        }

        #endregion

        #region ToolStripControlHost

        protected override void OnSubscribeControlEvents(Control c)
        {
            base.OnSubscribeControlEvents(c);

            // https://msdn.microsoft.com/en-us/library/system.windows.forms.control.enter(v=vs.110).aspx
            //this.Control.ValueControl.GotFocus += this.ValueControl_GotFocus;
            this.Control.ValueControl.LostFocus += this.ValueControl_LostFocus;
            //this.Control.ValueControl.Enter += this.ValueControl_Enter;
            //this.Control.ValueControl.Leave += this.ValueControl_Leave;
            //this.Control.ValueControl.Validating += this.ValueControl_Validating;
        }

        protected override void OnUnsubscribeControlEvents(Control c)
        {
            base.OnUnsubscribeControlEvents(c);

            //this.Control.ValueControl.GotFocus -= this.ValueControl_GotFocus;
            this.Control.ValueControl.LostFocus -= this.ValueControl_LostFocus;
            //this.Control.ValueControl.Enter -= this.ValueControl_Enter;
            //this.Control.ValueControl.Leave -= this.ValueControl_Leave;
            //this.Control.ValueControl.Validating -= this.ValueControl_Validating;
        }

        #endregion
    }
}