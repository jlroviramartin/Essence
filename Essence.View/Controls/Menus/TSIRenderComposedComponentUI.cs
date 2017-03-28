using System.Collections;
using System.Linq;
using System.Windows.Forms;
using Essence.Util.Collections;
using Essence.View.Models;

namespace Essence.View.Controls.Menus
{
    public class TSIRenderComposedComponentUI : TSIRenderComponentUI<IComposedComponentUI, ToolStripDropDownItem>
    {
        #region private

        /// <summary>
        /// Asociacion entre un <c>ToolStripItem</c> y una <c>IAccion</c>.
        /// </summary>
        private new class Link : TSIRenderComponentUI<IComposedComponentUI, ToolStripDropDownItem>.Link
        {
            /// <summary>
            /// Crea una asociacion.
            /// </summary>
            public static void Build(ToolStripDropDownItem tsItem, IComposedComponentUI item, TSIContext context)
            {
                new Link(tsItem, item, context);
            }

            private Link(ToolStripDropDownItem tsItem, IComposedComponentUI item, TSIContext context)
                : base(tsItem, item, context)
            {
                this.TSItem.DropDown.Name = item.Name;
                this.UpdateComponents();
            }

            /// <summary>
            /// Escucha el evento <c>IComposedAction.Accions.ListChanged</c>.
            /// </summary>
            private void Components_ListChanged(object sender, ListEventArgs args)
            {
                this.UpdateComponents();
            }

            private void UpdateComponents()
            {
                ArrayList arr = new ArrayList(this.TSItem.DropDownItems);

                this.context.Level++;
                this.TSItem.DropDownItems.Clear();
                this.TSItem.DropDownItems.AddRange(ToolStripItemRender.Instance.RenderRange(this.context, this.Item.Components).ToArray());
                this.context.Level--;

                foreach (ToolStripItem tsItem in arr)
                {
                    tsItem.Dispose();
                }
            }

            protected override void OnSubscribeControlEvents(ToolStripDropDownItem tsItem)
            {
                base.OnSubscribeControlEvents(tsItem);
            }

            protected override void OnUnsubscribeControlEvents(ToolStripDropDownItem tsItem)
            {
                ArrayList arr = new ArrayList(tsItem.DropDownItems);
                tsItem.DropDownItems.Clear();
                foreach (ToolStripItem aux in arr)
                {
                    aux.Dispose();
                }

                base.OnUnsubscribeControlEvents(tsItem);
            }

            protected override void OnSubscribeItemEvents(IComposedComponentUI item)
            {
                base.OnSubscribeItemEvents(item);
                item.Components.ListChanged += this.Components_ListChanged;
            }

            protected override void OnUnsubscribeItemEvents(IComposedComponentUI item)
            {
                base.OnUnsubscribeItemEvents(item);
                item.Components.ListChanged -= this.Components_ListChanged;
            }
        }

        #endregion

        #region TSIRender<IComposedAction>

        public override ToolStripDropDownItem Render(IComposedComponentUI composed, TSIContext context)
        {
            ToolStripDropDownItem tsDropDownItem = new ToolStripDropDownButton();

            Set(tsDropDownItem, composed, context);
            tsDropDownItem.DisplayStyle = context.CurrentDisplayStyle;

            // Se asocian el ToolStripItem a la accion.
            Link.Build(tsDropDownItem, composed, context);
            return tsDropDownItem;
        }

        #endregion
    }
}