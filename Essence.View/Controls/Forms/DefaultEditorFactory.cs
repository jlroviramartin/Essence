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
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Essence.Util;
using Essence.Util.Collections;
using Essence.Util.Logs;
using Essence.View.Models;
using Essence.View.Models.Properties;
using Factory = System.Func<System.Type, Essence.View.Models.Properties.ViewProperties[], System.IServiceProvider, System.Windows.Forms.Control>;

namespace Essence.View.Controls.Forms
{
    public class DefaultEditorFactory : IEditorFactory
    {
        public static IEditorFactory Instance = new DefaultEditorFactory();

        #region private

        private DefaultEditorFactory()
        {
            this.Initialize();
        }

        private void Initialize()
        {
            this.Register((s, p) => s, (v, f, p) => v, "");

            this.Register((s, p) => short.Parse(s, NumberStyles.Integer, p), (v, f, p) => v.ToString(f, p), "D");
            this.Register((s, p) => int.Parse(s, NumberStyles.Integer, p), (v, f, p) => v.ToString(f, p), "D");
            this.Register((s, p) => long.Parse(s, NumberStyles.Integer, p), (v, f, p) => v.ToString(f, p), "D");

            this.Register((s, p) => ushort.Parse(s, NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingWhite, p), (v, f, p) => v.ToString(f, p), "D");
            this.Register((s, p) => uint.Parse(s, NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingWhite, p), (v, f, p) => v.ToString(f, p), "D");
            this.Register((s, p) => ulong.Parse(s, NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingWhite, p), (v, f, p) => v.ToString(f, p), "D");

            this.Register((s, p) => double.Parse(s, NumberStyles.Float), (v, f, p) => v.ToString(f, p), "F3");
            this.Register((s, p) => float.Parse(s, NumberStyles.Float), (v, f, p) => v.ToString(f, p), "F3");
        }

        private void Register<TValue>(Func<string, IFormatProvider, TValue> parse, Func<TValue, string, IFormatProvider, string> toString, string format)
        {
            this.dictionary.Add(
                typeof(TValue),
                (type, properties, serviceProvider) =>
                {
                    FormatProperties formatProperties = properties.OfType<FormatProperties>().FirstOrDefault();
                    return this.CreateTextBox(formatProperties, parse, toString, format);
                });
        }

        private TextBox CreateTextBox<TValue>(FormatProperties formatProperties,
                                              Func<string, IFormatProvider, TValue> parse,
                                              Func<TValue, string, IFormatProvider, string> toString,
                                              string format)
        {
            TextBox textBox = new TextBox();

            Func<string, TValue> parse2;
            Func<TValue, string> toString2;

            if (formatProperties != null)
            {
                parse2 = (s) => parse(s, formatProperties.FormatProvider);
                toString2 = (v) => toString(v, formatProperties.Format ?? format, formatProperties.FormatProvider);
            }
            else
            {
                parse2 = (s) => parse(s, null);
                toString2 = (v) => toString(v, format, null);
            }

            //TextProperties textProperties = properties.OfType<TextProperties>().FirstOrDefault();

            ControlExchange.Set(textBox, new DelegateFormattableTextBoxExchange<TValue>(parse2, toString2));
            return textBox;
        }

        private ComboBox CreateList(Type valueType,
                                    IRefList refList,
                                    ViewProperties[] properties,
                                    IServiceProvider serviceProvider)
        {
            ComboBox combo = new ComboBox();
            combo.CausesValidation = true;
            combo.Sorted = false;
            combo.FormattingEnabled = true;
            combo.DropDownStyle = ComboBoxStyle.DropDownList;

            FormatProperties formatProperties = properties.OfType<FormatProperties>().FirstOrDefault();
            string format = formatProperties.With(x => x.Format);
            IFormatProvider formatProvider = formatProperties.With(x => x.FormatProvider);
            ICustomFormatter customFormatter = formatProperties.With(x => x.CustomFormatter);

            combo.Format += (sender, args) =>
            {
                if (!object.Equals(args.DesiredType, typeof(string)))
                {
                    Log<DefaultEditorFactory>.Error("Se intenta convertir a un valor diferente de String.");
                    args.Value = null;
                    return;
                }

                // Se formatea.
                try
                {
                    object item = args.ListItem;
                    if (item is IFormattable)
                    {
                        args.Value = ((IFormattable)item).ToString(format, formatProvider);
                    }
                    else if (customFormatter != null)
                    {
                        args.Value = customFormatter.Format(format, item, formatProvider);
                    }
                    else
                    {
                        args.Value = item.ToString();
                    }
                }
                catch (Exception e)
                {
                    Log<DefaultEditorFactory>.Error("Excepcion al formatear.", e);
                    args.Value = null;
                }
            };

            ControlExchange.Set(combo, TypeUtils.NewGeneric<ControlExchange>(typeof(ComboBoxExchange<>), valueType));
            return combo;
        }

        private readonly DictionaryOfType<Factory> dictionary = new DictionaryOfType<Factory>();

        #endregion

        #region IEditorFactory

        public object Create(Type valueType, ViewProperties[] properties, IServiceProvider serviceProvider)
        {
            IRefList refList = properties.OfType<RefListProperties>().Select(rlp => rlp.RefList).FirstOrDefault();
            if (refList != null)
            {
                return this.CreateList(valueType, refList, properties, serviceProvider);
            }

            Factory factory = this.dictionary.GetSafe(valueType);
            if (factory == null)
            {
                return null;
            }
            return factory(valueType, properties, serviceProvider);
        }

        #endregion
    }
}