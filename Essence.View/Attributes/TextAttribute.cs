using Essence.View.Models;
using Essence.View.Models.Properties;

namespace Essence.View.Attributes
{
    public class TextAttribute : ViewAttribute
    {
        public bool Label { get; set; }
        public bool Multiline { get; set; }
        public bool Scrollable { get; set; }

        #region ViewAttribute

        public override ViewProperties GetViewProperties()
        {
            return new TextProperties()
            {
                Label = this.Label,
                Multiline = this.Multiline,
                Scrollable = this.Scrollable
            };
        }

        #endregion
    }
}