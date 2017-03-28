namespace Essence.Model
{
    /// <summary>
    ///     Tipo de cambios que se pueden producir.
    /// </summary>
    public enum ListModelChange
    {
        /// <summary>
        ///     Se insertan elementos.
        /// </summary>
        InsertItems,

        /// <summary>
        ///     Se eliminan elementos.
        /// </summary>
        RemoveItems,

        /// <summary>
        ///     Se cambian elementos.
        /// </summary>
        ChangeItems
    }
}