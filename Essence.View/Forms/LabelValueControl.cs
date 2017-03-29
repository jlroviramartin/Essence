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

using System.Windows.Forms;

namespace Essence.View.Forms
{
    public class LabelValueControl : TableLayoutPanel
    {
        /// <summary>
        ///     Contructor.
        /// </summary>
        public LabelValueControl()
        {
            this.InitializeComponent();
            this.CausesValidation = true;
            this.ValueControl.CausesValidation = true;
        }

        /// <summary>
        ///     Control etiqueta.
        /// </summary>
        public Label LabelControl
        {
            get { return this.label; }
        }

        /// <summary>
        ///     Etiqueta.
        /// </summary>
        public string Label
        {
            get { return this.label.UIThread((c) => c.Text); }
            set
            {
                this.label.UIThread((c) => c.Text = value);
                this.textBox.UIThread((c) => c.Name = value);
            }
        }

        /// <summary>
        ///     Ancho del control etiqueta.
        /// </summary>
        public int LabelWidth
        {
            get { return this.label.UIThread((c) => c.Width); }
            set { this.label.UIThread((c) => c.Width = value); }
        }

        /// <summary>
        ///     Control valor.
        /// </summary>
        public TextBox ValueControl
        {
            get { return this.textBox; }
        }

        /// <summary>
        ///     Valor.
        /// </summary>
        public string Value
        {
            get { return this.textBox.UIThread((c) => c.Text); }
            set { this.textBox.UIThread((c) => c.Text = value); }
        }

        /// <summary>
        ///     Ancho del control valor.
        /// </summary>
        public int ValueWidth
        {
            get { return this.textBox.UIThread((c) => c.Width); }
            set { this.textBox.UIThread((c) => c.Width = value); }
        }

        /// <summary>
        ///     Indica que el valor es editable.
        /// </summary>
        public bool ValueIsEditable
        {
            get { return !this.textBox.UIThread((c) => c.ReadOnly); }
            set { this.textBox.UIThread((c) => c.ReadOnly = !value); }
        }

        #region Component

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        #region private

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            Form form = this.FindForm();
            if (form == null)
            {
                return;
            }

            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                form.Validate();
                //form.SelectNextControl(this.ValorControl, true, true, true, true);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Tab)
            {
                if (!e.Shift)
                {
                    form.SelectNextControl(this.ValueControl, true, true, true, true);
                }
                else
                {
                    form.SelectNextControl(this.ValueControl, false, true, true, true);
                }
                e.Handled = true;
            }
        }

        #region Component Designer generated code

        /// <summary>
        ///     Required method for Designer support - do not modify
        ///     the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LabelValueControl));
            this.label = new System.Windows.Forms.Label();
            this.textBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label
            // 
            resources.ApplyResources(this.label, "label");
            this.label.Name = "label";
            // 
            // textBox
            // 
            resources.ApplyResources(this.textBox, "textBox");
            this.textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox.Name = "textBox";
            this.textBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyUp);
            // 
            // EtiquetaValorControl
            // 
            resources.ApplyResources(this, "$this");
            this.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.Controls.Add(this.label, 0, 0);
            this.Controls.Add(this.textBox, 1, 0);
            this.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        /// <summary>
        ///     Required designer variable.
        /// </summary>
        private readonly System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label label;
        private System.Windows.Forms.TextBox textBox;

        #endregion
    }
}