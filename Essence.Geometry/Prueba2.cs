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

#if false
using System;
using Essence.Geometry.Core.Double;

namespace Essence.Geometry.Prueba2
{
    public class BufferedVector2d
    {
        public void Set(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public void Set(Vector2d v)
        {
            this.Set(v.X, v.Y);
        }

        public void Add(Vector2d v)
        {
            this.Set(this.X + v.X, this.Y + v.Y);
        }

        public void Add(BufferedVector2d v)
        {
            this.Set(this.X + v.X, this.Y + v.Y);
        }

        public void Sub(Vector2d v)
        {
            this.Set(this.X - v.X, this.Y - v.Y);
        }

        public void Sub(BufferedVector2d v)
        {
            this.Set(this.X - v.X, this.Y - v.Y);
        }

        public void Mul(double v)
        {
            this.Set(this.X * v, this.Y * v);
        }

        public void Div(double v)
        {
            this.Set(this.X / v, this.Y / v);
        }

        public void Neg()
        {
            this.Set(-this.X, -this.Y);
        }

        public void Abs()
        {
            this.Set(Math.Abs(this.X), Math.Abs(this.Y));
        }

        public void Lerp(Vector2d v2, double alpha)
        {
            this.Lineal(v2, 1 - alpha, alpha);
        }

        public void Lineal(Vector2d v2, double alpha, double beta)
        {
            this.Set(alpha * this.X + beta * v2.X, alpha * this.Y + beta * v2.Y);
        }

        public double Dot(Vector2d v2)
        {
            return this.X * v2.X + this.Y * v2.Y;
        }

        public double Proy(Vector2d v2)
        {
            return this.Dot(v2) / this.Length;
        }

        public void ProyV(Vector2d v2)
        {
            this.Mul(this.Proy(v2));
        }

        public double X, Y;
    }
}
#endif
