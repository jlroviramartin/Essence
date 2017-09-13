namespace Essence.Geometry.Core
{
    public interface IColorSetter
    {
        /// <summary>
        ///Sets the chanels as float values.
        /// </summary>
        void SetColor(params float[] channels);

        /// <summary>
        ///Sets the chanels as float values.
        /// </summary>
        void SetColor(params byte[] channels);
    }

    public interface IColorSetter3C
    {
        /// <summary>
        ///Sets the chanels as float values.
        /// </summary>
        void SetColor(float r, float g, float b);

        /// <summary>
        ///Sets the chanels as byte values.
        /// </summary>
        void SetColor(byte r, byte g, byte b);
    }

    public interface IColorSetter4C
    {
        /// <summary>
        ///Sets the chanels as float values.
        /// </summary>
        void SetColor(float r, float g, float b, float a);

        /// <summary>
        ///Sets the chanels as float values.
        /// </summary>
        void SetColor(byte r, byte g, byte b, byte a);
    }
}