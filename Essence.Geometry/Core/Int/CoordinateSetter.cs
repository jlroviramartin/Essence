﻿// Copyright 2017 Jose Luis Rovira Martin
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

namespace Essence.Geometry.Core.Int
{
    public sealed class CoordinateSetter2i : ICoordinateSetter2D
    {
        public Point2i GetPoint()
        {
            return new Point2i(this.x, this.y);
        }

        public Vector2i GetVector()
        {
            return new Vector2i(this.x, this.y);
        }

        public int X
        {
            get { return this.x; }
        }

        public int Y
        {
            get { return this.y; }
        }

        private int x;
        private int y;

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
            this.x = (int)x;
            this.y = (int)y;
        }

        public void SetCoords(float x, float y)
        {
            this.x = (int)x;
            this.y = (int)y;
        }

        public void SetCoords(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}