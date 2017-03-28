using System.Collections;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Essence.Util.Collections;
using Essence.View.Controls.Menus;
using Essence.View.Models;
using Essence.View.Views;

namespace Essence.View.Controls
{
    public class MenuViewControl<TControl> : ViewControl<TControl>, IMenuView
        where TControl : MenuStrip, new()
    {
        public MenuViewControl()
        {
            this.components.ListChanged += this.Components_ListChanged;

            this.Control.CausesValidation = true;
        }

        #region private

        private void Components_ListChanged(object sender, ListEventArgs args)
        {
            // Se crea una copia para liberar despues los elementos.
            ArrayList arr = new ArrayList(this.Control.Items);

            // Se vacia la barra de herramientas.
            this.Control.Items.Clear();

            // Se liberan.
            foreach (ToolStripItem tsItem in arr)
            {
                tsItem.Dispose();
            }

            TSIContext context = new TSIContext();
            context.DisplayStyle1 = ToolStripItemDisplayStyle.ImageAndText;
            context.DisplayStyle2 = ToolStripItemDisplayStyle.ImageAndText;
            context.IconSize1 = new Size(16, 16);
            context.IconSize2 = new Size(16, 16);
            context.TextImageRelation = TextImageRelation.ImageAboveText;
            context.View = this;

            this.Control.Items.AddRange(ToolStripItemRender.Instance.RenderRange(context, this.Components).ToArray());
        }

        private readonly EventList<IComponentUI> components = new EventList<IComponentUI>();

        #endregion

        #region IMenuView

        public IEventList<IComponentUI> Components
        {
            get { return this.components; }
        }

        #endregion
    }
}