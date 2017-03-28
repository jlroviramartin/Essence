using System.Windows.Forms;
using Label = Essence.View.Models.Label;

namespace Essence.View.Controls.Menus
{
    public class TSIRenderLabel : TSIRender<Label, ToolStripLabel>
    {
        #region TSIRender<LabelAction>

        public override ToolStripLabel Render(Label action, TSIContext context)
        {
            ToolStripLabel tsItem = new ToolStripLabel();

            Set(tsItem, action, context);

            return tsItem;
        }

        #endregion
    }
}