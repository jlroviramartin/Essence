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