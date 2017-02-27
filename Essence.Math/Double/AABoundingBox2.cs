namespace Essence.Math.Double
{
    public struct AABoundingBox2
    {
        public AABoundingBox2(Vec2d p0, Vec2d p1)
        {
            if (p0.X <= p1.X)
            {
                this.xMin = p0.X;
                this.xMax = p1.X;
            }
            else
            {
                this.xMin = p1.X;
                this.xMax = p0.X;
            }
            if (p0.Y <= p1.Y)
            {
                this.yMin = p0.Y;
                this.yMax = p1.Y;
            }
            else
            {
                this.yMin = p1.Y;
                this.yMax = p0.Y;
            }
        }

        public AABoundingBox2(double xmin, double ymin, double xmax, double ymax)
        {
            this.xMin = xmin;
            this.yMin = ymin;
            this.xMax = xmax;
            this.yMax = ymax;
        }

        public double XMin
        {
            get { return this.xMin; }
        }

        public double YMin
        {
            get { return this.yMin; }
        }

        public double XMax
        {
            get { return this.xMax; }
        }

        public double YMax
        {
            get { return this.yMax; }
        }

        private readonly double xMin;
        private readonly double yMin;
        private readonly double xMax;
        private readonly double yMax;
    }
}