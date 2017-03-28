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