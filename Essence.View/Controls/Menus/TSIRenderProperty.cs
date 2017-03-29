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
using Essence.Util.Logs;
using Essence.View.Forms;
using Essence.View.Models;
using Essence.View.Models.Properties;

namespace Essence.View.Controls.Menus
{
    public class TSIRenderProperty : TSIRenderComponentUI<IProperty, LabelValueToolStrip>
    {
        #region private

        /// <summary>
        ///     Asociation entre un <c>ToolStripItem</c> y una <c>IProperty</c>.
        /// </summary>
        private new class Link : TSIRenderComponentUI<IProperty, LabelValueToolStrip>.Link
        {
            /// <summary>
            ///     Crea una asociacion.
            /// </summary>
            public static void Build(LabelValueToolStrip tsItem, IProperty item, TSIContext context)
            {
                new Link(tsItem, item, context);
            }

            private Link(LabelValueToolStrip tsItem, IProperty item, TSIContext context)
                : base(tsItem, item, context)
            {
                this.UpdateControl();
            }

            /// <summary>
            ///     Escucha el evento <c>TSItem.ValueModified</c>.
            /// </summary>
            private void TSItem_ValueModified(object sender, EventArgs args)
            {
                this.actualizando.Do(
                    () =>
                    {
                        this.UpdateProperty();
                        this.UpdateControl();
                    });
            }

            /// <summary>
            ///     Escucha el evento <c>Property.PropertyChanged</c>.
            /// </summary>
            private void Property_PropertyChanged(object sender, PropertyChangedExEventArgs args)
            {
                switch (args.PropertyName)
                {
                    case Property.ENABLED:
                    {
                        this.TSItem.Enabled = this.Item.Enabled;
                        break;
                    }
                    case Property.EDITABLE:
                    {
                        break;
                    }
                    case Property.VALUE:
                    {
                        this.actualizando.Do(
                            () =>
                            {
                                this.UpdateControl();
                            });
                        break;
                    }
                }
            }

            private void UpdateControl()
            {
                try
                {
                    string format = "";
                    IFormatProvider formatProvider = null;
                    if (this.context.PropertiesProvider != null)
                    {
                        FormatProperties props = this.context.PropertiesProvider.FindProperties<FormatProperties>(this.Item) ?? FormatProperties.Empty;
                        format = props.Format;
                        formatProvider = props.FormatProvider;
                    }
                    this.TSItem.Value = this.Item.ToString(format, formatProvider);
                }
                catch (Exception e)
                {
                    Log<TSIRenderProperty>.Warn(e);
                }
            }

            private void UpdateProperty()
            {
                try
                {
                    string format = "";
                    IFormatProvider formatProvider = null;
                    if (this.context.PropertiesProvider != null)
                    {
                        FormatProperties props = this.context.PropertiesProvider.FindProperties<FormatProperties>(this.Item) ?? FormatProperties.Empty;
                        format = props.Format;
                        formatProvider = props.FormatProvider;
                    }
                    this.Item.TryParse(this.TSItem.Value, format, formatProvider);
                }
                catch (Exception e)
                {
                    Log<TSIRenderProperty>.Warn(e);
                }
            }

            /// <summary>Esta variable evita los bucles infinitos.</summary>
            private readonly PreventReentry actualizando = new PreventReentry();

            protected override void OnSubscribeControlEvents(LabelValueToolStrip tsItem)
            {
                base.OnSubscribeControlEvents(tsItem);
                tsItem.ValueModified += this.TSItem_ValueModified;
            }

            protected override void OnUnsubscribeControlEvents(LabelValueToolStrip tsItem)
            {
                base.OnUnsubscribeControlEvents(tsItem);
                tsItem.ValueModified -= this.TSItem_ValueModified;
            }

            protected override void OnSubscribeItemEvents(IProperty item)
            {
                base.OnSubscribeItemEvents(item);
                item.PropertyChanged += this.Property_PropertyChanged;
            }

            protected override void OnUnsubscribeItemEvents(IProperty item)
            {
                base.OnUnsubscribeItemEvents(item);
                item.PropertyChanged -= this.Property_PropertyChanged;
            }
        }

        #endregion

        #region TSIRender<IProperty>

        public override LabelValueToolStrip Render(IProperty property, TSIContext context)
        {
            LabelValueToolStrip tsItem = new LabelValueToolStrip();
            Set(tsItem, property, context);

            tsItem.Enabled = property.Enabled;
            tsItem.ValueIsEditable = property.Editable;

            tsItem.Label = property.NameUI;

            int labelWidth = 50;
            int valueWidth = 50;
            if (context.PropertiesProvider != null)
            {
                PropertyProperties props = context.PropertiesProvider.FindProperties<PropertyProperties>(property) ?? PropertyProperties.Empty;
                labelWidth = props.LabelWidth;
                valueWidth = props.ValueWidth;
            }
            tsItem.LabelWidth = labelWidth;
            tsItem.ValueWidth = valueWidth;

            // Se asocian el ToolStripItem a la propiedad.
            Link.Build(tsItem, property, context);
            return tsItem;
        }

        #endregion
    }
}