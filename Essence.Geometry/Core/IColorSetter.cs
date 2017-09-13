namespace Essence.Geometry.Core
{
    public interface IColorSetter
    {
        /// <summary>
        /// Sets the chanels as float values.
        /// </summary>
        /// <param name="channels">Channels</param>
        void SetColor(params float[] channels);

        /// <summary>
        /// Sets the chanels as byte values.
        /// </summary>
        /// <param name="channels">Channels</param>
        void SetColor(params byte[] channels);
    }

    public interface IColorSetter3 : IColorSetter
    {
        /// <summary>
        /// Sets the chanels as float values.
        /// </summary>
        /// <param name="c1">Channel 1</param>
        /// <param name="c2">Channel 2</param>
        /// <param name="c3">Channel 3</param>
        void SetColor(float c1, float c2, float c3);

        /// <summary>
        /// Sets the chanels as byte values.
        /// </summary>
        /// <param name="c1">Channel 1</param>
        /// <param name="c2">Channel 2</param>
        /// <param name="c3">Channel 3</param>
        void SetColor(byte c1, byte c2, byte c3);
    }

    public interface IColorSetter4 : IColorSetter
    {
        /// <summary>
        /// Sets the chanels as float values.
        /// </summary>
        /// <param name="c1">Channel 1</param>
        /// <param name="c2">Channel 2</param>
        /// <param name="c3">Channel 3</param>
        /// <param name="c4">Channel 4</param>
        void SetColor(float c1, float c2, float c3, float c4);

        /// <summary>
        /// Sets the chanels as byte values.
        /// </summary>
        /// <param name="c1">Channel 1</param>
        /// <param name="c2">Channel 2</param>
        /// <param name="c3">Channel 3</param>
        /// <param name="c4">Channel 4</param>
        void SetColor(byte c1, byte c2, byte c3, byte c4);
    }
}