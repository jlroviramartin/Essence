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

using System.Drawing;
using System.Windows.Forms;

namespace Essence.View.Models.Properties
{
    public class SimpleProperties : ViewProperties
    {
        public static SimpleProperties Empty
        {
            get { return new SimpleProperties(); }
        }

        public Color? ForeColor { get; set; }
        public Color? BackColor { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
        public int MinWidth { get; set; }
        public int MinHeight { get; set; }
        public int MaxWidth { get; set; }
        public int MaxHeight { get; set; }
        public bool AutoSize { get; set; }

        public DockStyle? Dock { get; set; }
        public AnchorStyles? Anchor { get; set; }

        public string Text { get; set; }
        public FontStyle FontStyle { get; set; }
        public bool? Visible { get; set; }
        public HorizontalAlignment? Align { get; set; }
    }
}