namespace Essence.Geometry.Core
{
    public interface ICoordinateSetter
    {
        /// <summary>
        /// Sets the coordinates as double values.
        /// </summary>
        void SetCoords(params double[] coords);

        /// <summary>
        /// Sets the coordinates as float values.
        /// </summary>
        void SetCoords(params float[] coords);

        /// <summary>
        /// Sets the coordinates as int values.
        /// </summary>
        void SetCoords(params int[] coords);
    }

    public interface ICoordinateSetter1D : ICoordinateSetter
    {
        /// <summary>
        /// Sets the coordinates as double values.
        /// </summary>
        void SetCoords(double x);

        /// <summary>
        /// Sets the coordinates as float values.
        /// </summary>
        void SetCoords(float x);
    }

    public interface ICoordinateSetter2D : ICoordinateSetter
    {
        /// <summary>
        /// Sets the coordinates as double values.
        /// </summary>
        void SetCoords(double x, double y);

        /// <summary>
        /// Sets the coordinates as float values.
        /// </summary>
        void SetCoords(float x, float y);
    }

    public interface ICoordinateSetter3D : ICoordinateSetter
    {
        /// <summary>
        /// Sets the coordinates as double values.
        /// </summary>
        void SetCoords(double x, double y, double z);


        /// <summary>
        /// Sets the coordinates as float values.
        /// </summary>
        void SetCoords(float x, float y, float z);
    }

    public interface ICoordinateSetter4D : ICoordinateSetter
    {
        /// <summary>
        /// Sets the coordinates as double values.
        /// </summary>
        void SetCoords(double x, double y, double z, double w);


        /// <summary>
        /// Sets the coordinates as float values.
        /// </summary>
        void SetCoords(float x, float y, float z, float w);

        /// <summary>
        /// Sets the coordinates as int values.
        /// </summary>
        void SetCoords(int x, int y, int z, int w);
    }
}