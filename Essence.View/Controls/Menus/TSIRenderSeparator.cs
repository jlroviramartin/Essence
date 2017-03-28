using System.Windows.Forms;
using Essence.View.Models;

namespace Essence.View.Controls.Menus
{
    public class TSIRenderSeparator : TSIRender<Separator, ToolStripSeparator>
    {
        #region TSIRender<SeparatorAction>

        public override ToolStripSeparator Render(Separator accion, TSIContext context)
        {
            ToolStripSeparator tsItem = new ToolStripSeparator();
            return tsItem;
        }

        #endregion
    }
}