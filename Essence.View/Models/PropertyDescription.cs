using Essence.View.Controls;

namespace Essence.View.Models
{
    public class PropertyDescription
    {
        public string Category { get; set; }

        public string Name { get; set; }

        public string Label { get; set; }

        public string Description { get; set; }

        public Icon Icon { get; set; }

        public bool HideLabel { get; set; }

        public IEditorFactory EditorFactory { get; set; }
    }
}