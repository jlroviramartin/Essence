// Copyright 2017 Jose Luis Rovira Martin
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Essence.Geometry.Core.Double
{
    public sealed class CoordinateSetter2d : ICoordinateSetter2D
    {
        public Point2d GetPoint()
        {
            return new Point2d(this.X, this.Y);
        }

        public Vector2d GetVector()
        {
            return new Vector2d(this.X, this.Y);
        }

        public double X { get; private set; }

        public double Y { get; private set; }

        public void SetCoords(params double[] coords)
        {
            this.SetCoords(coords[0], coords[1]);
        }

        public void SetCoords(params float[] coords)
        {
            this.SetCoords(coords[0], coords[1]);
        }

        public void SetCoords(params int[] coords)
        {
            this.SetCoords(coords[0], coords[1]);
        }

        public void SetCoords(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public void SetCoords(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public void SetCoords(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }
    }

    public sealed class CoordinateSetter3d : ICoordinateSetter3D
    {
        public Point3d GetPoint()
        {
            return new Point3d(this.X, this.Y, this.Z);
        }

        public Vector3d GetVector()
        {
            return new Vector3d(this.X, this.Y, this.Z);
        }

        public double X { get; private set; }

        public double Y { get; private set; }

        public double Z { get; private set; }

        public void SetCoords(params double[] coords)
        {
            this.SetCoords(coords[0], coords[1], coords[2]);
        }

        public void SetCoords(params float[] coords)
        {
            this.SetCoords(coords[0], coords[1], coords[2]);
        }

        public void SetCoords(params int[] coords)
        {
            this.SetCoords((double)coords[0], (double)coords[1], (double)coords[2]);
        }

        public void SetCoords(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public void SetCoords(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
    }

    public sealed class CoordinateSetter4d : ICoordinateSetter4D
    {
        public Point4d GetPoint()
        {
            return new Point4d(this.X, this.Y, this.Z, this.W);
        }

        public Vector4d GetVector()
        {
            return new Vector4d(this.X, this.Y, this.Z, this.W);
        }

        public double X { get; private set; }

        public double Y { get; private set; }

        public double Z { get; private set; }

        public double W { get; private set; }

        public void SetCoords(params double[] coords)
        {
            this.SetCoords(coords[0], coords[1], coords[2], coords[3]);
        }

        public void SetCoords(params float[] coords)
        {
            this.SetCoords(coords[0], coords[1], coords[2], coords[3]);
        }

        public void SetCoords(params int[] coords)
        {
            this.SetCoords((double)coords[0], (double)coords[1], (double)coords[2], (double)coords[3]);
        }

        public void SetCoords(double x, double y, double z, double w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        public void SetCoords(float x, float y, float z, float w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }
    }
}