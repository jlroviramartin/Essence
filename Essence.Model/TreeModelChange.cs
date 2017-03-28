namespace Essence.Model
{
    /// <summary>
    ///     Tipo de cambios que se pueden producir.
    /// </summary>
    public enum TreeModelChange
    {
        /// <summary>
        ///     Se inserta un nodo (o varios nodos hermanos).
        /// </summary>
        InsertNodes,

        /// <summary>
        ///     Se elimina un nodo (o varios nodos hermanos).
        /// </summary>
        RemoveNodes,

        /// <summary>
        ///     Se cambian la estructura de un nodo.
        /// </summary>
        StructureChange,

        /// <summary>
        ///     Se modifica un nodo (o varios nodos hermanos). No se modifica la estructura.
        /// </summary>
        ChangeNodes
    }
}