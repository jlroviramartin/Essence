using System.Windows.Forms;

namespace Essence.View.Controls.Menus
{
    public class TSIRenderNull : TSIRender
    {
        #region TSIRender

        public override bool IsRenderable(object obj)
        {
            return (obj == null);
        }

        public override ToolStripItem Render(object obj, TSIContext context)
        {
            ToolStripItem tsItem = new ToolStripMenuItem();
            tsItem.Tag = null;
            tsItem.Name = "<Error>";
            tsItem.DisplayStyle = ToolStripItemDisplayStyle.None;
            tsItem.Text = "<Error>";
            tsItem.AutoToolTip = false;
            tsItem.ToolTipText = "<Error>";
            tsItem.Image = null;
            //tsItem.Click = null;
            return tsItem;
        }

        #endregion
    }
}