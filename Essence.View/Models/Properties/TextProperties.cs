namespace Essence.View.Models.Properties
{
    public class TextProperties : ViewProperties
    {
        public static TextProperties Empty
        {
            get { return new TextProperties(); }
        }

        /// <summary>Indica si es una etiqueta.</summary>
        public bool Label { get; set; }

        /// <summary>Indica si el texto permite multiples lineas.</summary>
        public bool Multiline { get; set; }

        /// <summary>Indica si el texto muestra el Scroll.</summary>
        public bool Scrollable { get; set; }
    }
}