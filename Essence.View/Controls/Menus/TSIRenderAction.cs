using System;
using System.Windows.Forms;
using Essence.Util;
using Essence.Util.Events;
using Essence.View.Models;
using Essence.View.Resources;
using Action = Essence.View.Models.Action;

namespace Essence.View.Controls.Menus
{
    public class TSIRenderAction : TSIRenderComponentUI<IAction, ToolStripMenuItem>
    {
        #region private

        /// <summary>
        /// Asociacion entre un <c>ToolStripItem</c> y una <c>IAccion</c>.
        /// </summary>
        private new class Link : TSIRenderComponentUI<IAction, ToolStripMenuItem>.Link
        {
            /// <summary>
            /// Crea una asociacion.
            /// </summary>
            public static void Build(ToolStripMenuItem tsItem, IAction item, TSIContext context)
            {
                new Link(tsItem, item, context);
            }

            private Link(ToolStripMenuItem tsItem, IAction item, TSIContext context)
                : base(tsItem, item, context)
            {
            }

            /// <summary>
            /// Escucha el evento <c>IAction.PropertyChanged</c>.
            /// </summary>
            private void Action_PropertyChanged(object source, PropertyChangedExEventArgs args)
            {
                switch (args.PropertyName)
                {
                    case Essence.View.Models.Action.ENABLED:
                    {
                        this.TSItem.Enabled = this.Item.Enabled;
                        break;
                    }
                }
            }

            /// <summary>
            /// Escucha el evento <c>TSItem.Click</c>.
            /// </summary>
            private void TSItem_Click(object sender, EventArgs args)
            {
                this.Item.Invoke();
            }

            protected override void OnSubscribeControlEvents(ToolStripMenuItem tsItem)
            {
                base.OnSubscribeControlEvents(tsItem);
                tsItem.Click += this.TSItem_Click;
            }

            protected override void OnUnsubscribeControlEvents(ToolStripMenuItem tsItem)
            {
                base.OnUnsubscribeControlEvents(tsItem);
                tsItem.Click -= this.TSItem_Click;
            }

            protected override void OnSubscribeItemEvents(IAction item)
            {
                base.OnSubscribeItemEvents(item);
                item.PropertyChanged += this.Action_PropertyChanged;
            }

            protected override void OnUnsubscribeItemEvents(IAction item)
            {
                base.OnUnsubscribeItemEvents(item);
                item.PropertyChanged -= this.Action_PropertyChanged;
            }
        }

        #endregion

        #region TSIRender<IAccion>

        public override ToolStripMenuItem Render(IAction action, TSIContext context)
        {
            ToolStripMenuItem tsItem = new ToolStripMenuItem();
            tsItem.Tag = action;

            Set(tsItem, action, context);
            tsItem.DisplayStyle = context.CurrentDisplayStyle;

            // Se asocian el ToolStripItem a la accion.
            Link.Build(tsItem, action, context);
            return tsItem;
        }

        #endregion
    }
}