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
using System.Linq;
using System.Windows.Forms;
using Essence.Util.Collections;
using Essence.Util.Events;
using Essence.View.Models;
using Action = Essence.View.Models.Action;

namespace Essence.View.Controls.Menus
{
    public class TSIRenderComposedAction : TSIRenderComponentUI<IComposedAction, ToolStripDropDownItem>
    {
        #region private

        /// <summary>
        ///     Asociacion entre un <c>ToolStripItem</c> y una <c>IAccion</c>.
        /// </summary>
        private new class Link : TSIRenderComponentUI<IComposedAction, ToolStripDropDownItem>.Link
        {
            /// <summary>
            ///     Crea una asociacion.
            /// </summary>
            public static void Build(ToolStripDropDownItem tsItem, IComposedAction item, TSIContext context)
            {
                new Link(tsItem, item, context);
            }

            private Link(ToolStripDropDownItem tsItem, IComposedAction item, TSIContext context)
                : base(tsItem, item, context)
            {
                this.TSItem.DropDown.Name = tsItem.Name;
                this.UpdateActions();
            }

            /// <summary>
            ///     Escucha el evento <c>TSItem.Click</c>.
            /// </summary>
            private void TSItem_Click(object sender, EventArgs args)
            {
                this.Item.Invoke();
            }

            /// <summary>
            ///     Escucha el evento <c>IComposedAction.PropertyChanged</c>.
            /// </summary>
            private void Action_PropertyChanged(object source, PropertyChangedExEventArgs args)
            {
                switch (args.PropertyName)
                {
                    case Action.ENABLED:
                    {
                        this.TSItem.Enabled = this.Item.Enabled;
                        break;
                    }
                }
            }

            /// <summary>
            ///     Escucha el evento <c>IComposedAction.Accions.ListChanged</c>.
            /// </summary>
            private void Actions_ListChanged(object sender, ListEventArgs args)
            {
                this.UpdateActions();
            }

            private void UpdateActions()
            {
                ArrayList aux = new ArrayList(this.TSItem.DropDownItems);

                this.context.Level++;
                this.TSItem.DropDownItems.Clear();
                this.TSItem.DropDownItems.AddRange(ToolStripItemRender.Instance.RenderRange(this.context, this.Item.Actions).ToArray());
                this.context.Level--;

                foreach (ToolStripItem tsi in aux)
                {
                    tsi.Dispose();
                }
            }

            protected override void OnSubscribeControlEvents(ToolStripDropDownItem tsItem)
            {
                base.OnSubscribeControlEvents(tsItem);
                tsItem.Click += this.TSItem_Click;
                //tsItem.DropDownItemClicked += this.TSSplitButton_DropDownItemClicked;
            }

            protected override void OnUnsubscribeControlEvents(ToolStripDropDownItem tsItem)
            {
                ArrayList aux = new ArrayList(tsItem.DropDownItems);
                tsItem.DropDownItems.Clear();
                foreach (ToolStripItem tsi in aux)
                {
                    tsi.Dispose();
                }

                base.OnUnsubscribeControlEvents(tsItem);
                tsItem.Click -= this.TSItem_Click;
                //tsItem.DropDownItemClicked -= this.TSSplitButton_DropDownItemClicked;
            }

            protected override void OnSubscribeItemEvents(IComposedAction item)
            {
                base.OnSubscribeItemEvents(item);
                item.PropertyChanged += this.Action_PropertyChanged;
                item.Actions.ListChanged += this.Actions_ListChanged;
            }

            protected override void OnUnsubscribeItemEvents(IComposedAction item)
            {
                base.OnUnsubscribeItemEvents(item);
                item.PropertyChanged -= this.Action_PropertyChanged;
                item.Actions.ListChanged -= this.Actions_ListChanged;
            }
        }

        #endregion

        #region TSIRender<IComposedAction>

        public override ToolStripDropDownItem Render(IComposedAction action, TSIContext context)
        {
            ToolStripDropDownItem tsDropDownItem = new ToolStripDropDownButton();

            Set(tsDropDownItem, action, context);
            tsDropDownItem.DisplayStyle = context.CurrentDisplayStyle;

            // Se asocian el ToolStripItem a la accion.
            Link.Build(tsDropDownItem, action, context);
            return tsDropDownItem;
        }

        #endregion
    }
}