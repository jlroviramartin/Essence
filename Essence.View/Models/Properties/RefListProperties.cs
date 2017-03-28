namespace Essence.View.Models.Properties
{
    public class RefListProperties : ViewProperties
    {
        public static RefListProperties Empty
        {
            get { return new RefListProperties(); }
        }

        /// <summary>Referencia a una lista de donde obtener los elementos.</summary>
        public IRefList RefList { get; set; }
    }
}