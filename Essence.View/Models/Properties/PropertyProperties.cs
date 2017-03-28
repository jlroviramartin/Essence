namespace Essence.View.Models.Properties
{
    public class PropertyProperties : ViewProperties
    {
        public static PropertyProperties Empty
        {
            get { return new PropertyProperties(); }
        }

        public int LabelWidth { get; set; }

        public int ValueWidth { get; set; }
    }
}