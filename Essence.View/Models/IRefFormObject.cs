namespace Essence.View.Models
{
    /// <summary>
    ///     Se utiliza desde las vistas <c>IFormView</c>, para indicar a que objeto hace referencia el modelo
    ///     <c>IFormModel</c>.
    ///     Lo implementa <c>IFormModel</c>.
    /// </summary>
    public interface IRefFormObject
    {
        object FormObject { get; }
    }
}