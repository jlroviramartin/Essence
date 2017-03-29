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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Essence.View.Models;
using Essence.View.Models.Properties;

namespace Essence.View.Controls.Forms
{
    public abstract class ControlExchange
    {
        public static ControlExchange Get(Control control)
        {
            return control.Tag as ControlExchange;
        }

        public static void Set(Control control, ControlExchange exchange)
        {
            control.Tag = exchange;
        }

        /// <summary>
        ///     Obtiene/establece el nombre de la propiedad al que mapea dentro del formulario.
        /// </summary>
        public string PopertyName { get; set; }

        /// <summary>
        ///     LLeva el valor del control a la propiedad.
        /// </summary>
        public abstract object GetValue(object control);

        /// <summary>
        ///     LLeva el valor de la propiedad al control.
        /// </summary>
        public abstract void SetValue(object control, object value);

        public abstract void Listen(object control, EventHandler handler);

        public abstract void DoNotListen(object control, EventHandler handler);

        public virtual void ContentChanged(object control, IFormModel formModel, string name, IServiceProvider serviceProvider)
        {
        }

        public virtual void Attach()
        {
        }

        public virtual void Deattach()
        {
        }
    }

    public abstract class ControlExchange<TControl, TValue> : ControlExchange
        where TControl : Control
    {
        public abstract TValue GetValue(TControl control);

        public abstract void SetValue(TControl control, TValue value);

        public virtual void Listen(TControl control, EventHandler handler)
        {
            // NOTA: se utiliza la validacion para llevar el valor del control al formulario.
            control.Validating += new CancelEventHandler(handler);
        }

        public virtual void DoNotListen(TControl control, EventHandler handler)
        {
            control.Validating -= new CancelEventHandler(handler);
        }

        public virtual void ContentChanged(TControl control, IFormModel formModel, string name, IServiceProvider serviceProvider)
        {
        }

        public sealed override object GetValue(object control)
        {
            return this.GetValue((TControl)control);
        }

        public sealed override void SetValue(object control, object value)
        {
            this.SetValue((TControl)control, (TValue)value);
        }

        public sealed override void Listen(object control, EventHandler handler)
        {
            this.Listen((TControl)control, handler);
        }

        public sealed override void DoNotListen(object control, EventHandler handler)
        {
            this.DoNotListen((TControl)control, handler);
        }

        public sealed override void ContentChanged(object control, IFormModel formModel, string name, IServiceProvider serviceProvider)
        {
            this.ContentChanged((TControl)control, formModel, name, serviceProvider);
        }
    }

    public abstract class FormattableTextBoxExchange<TValue> : ControlExchange<TextBox, TValue>
    {
        public override TValue GetValue(TextBox control)
        {
            return this.Parse(control.Text);
        }

        public override void SetValue(TextBox control, TValue value)
        {
            control.Text = this.ToString(value);
        }

        public abstract TValue Parse(string text);

        public abstract string ToString(TValue value);
    }

    public sealed class DelegateFormattableTextBoxExchange<TValue> : FormattableTextBoxExchange<TValue>
    {
        public DelegateFormattableTextBoxExchange(Func<string, TValue> parse, Func<TValue, string> toString)
        {
            this.parse = parse;
            this.toString = toString;
        }

        public override TValue Parse(string text)
        {
            return this.parse(text);
        }

        public override string ToString(TValue value)
        {
            return this.toString(value);
        }

        private readonly Func<string, TValue> parse;
        private readonly Func<TValue, string> toString;
    }

    public sealed class ComboBoxExchange<TValue> : ControlExchange<ComboBox, TValue>
    {
        public ComboBoxExchange()
        {
        }

        public override TValue GetValue(ComboBox control)
        {
            return (TValue)control.SelectedItem;
        }

        public override void SetValue(ComboBox control, TValue value)
        {
            control.SelectedItem = value;
        }

        public override void Listen(ComboBox control, EventHandler handler)
        {
            control.SelectedIndexChanged += handler;
        }

        public override void DoNotListen(ComboBox control, EventHandler handler)
        {
            control.SelectedIndexChanged -= handler;
        }

        public override void ContentChanged(ComboBox editControl, IFormModel formModel, string name, IServiceProvider serviceProvider)
        {
            IEnumerable<ViewProperties> viewProperties = formModel.GetViewProperties(name);
            IRefList refList = viewProperties.OfType<RefListProperties>().Select(rlp => rlp.RefList).FirstOrDefault();

            if (refList != null)
            {
                IEnumerable items;
                if (formModel is IRefFormObject)
                {
                    object formObject = ((IRefFormObject)formModel).FormObject;
                    items = refList.GetItems(serviceProvider, formObject);
                }
                else
                {
                    items = refList.GetItems(serviceProvider, null);
                }
                editControl.Items.Clear();
                editControl.Items.AddRange(items.Cast<object>().ToArray());
            }
        }
    }
}