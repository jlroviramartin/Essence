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

namespace Essence.Geometry.Core.Float
{
    public sealed class ColorSetter3f : IColorSetter3
    {
        public Color3f GetColor()
        {
            return new Color3f(this.C1, this.C2, this.C3);
        }

        public float C1 { get; private set; }

        public float C2 { get; private set; }

        public float C3 { get; private set; }

        public void SetColor(params byte[] channels)
        {
            this.SetColor(channels[0], channels[1], channels[2]);
        }

        public void SetColor(params float[] channels)
        {
            this.SetColor(channels[0], channels[1], channels[2]);
        }

        public void SetColor(byte c1, byte c2, byte c3)
        {
            this.C1 = ColorUtils.ToChannelFloat(c1);
            this.C2 = ColorUtils.ToChannelFloat(c2);
            this.C3 = ColorUtils.ToChannelFloat(c3);
        }

        public void SetColor(float c1, float c2, float c3)
        {
            this.C1 = c1;
            this.C2 = c2;
            this.C3 = c3;
        }
    }

    public sealed class ColorSetter4f : IColorSetter4
    {
        public Color4f GetColor()
        {
            return new Color4f(this.C1, this.C2, this.C3, this.C4);
        }

        public float C1 { get; private set; }

        public float C2 { get; private set; }

        public float C3 { get; private set; }

        public float C4 { get; private set; }

        public void SetColor(params byte[] channels)
        {
            this.SetColor(channels[0], channels[1], channels[2], channels[3]);
        }

        public void SetColor(params float[] channels)
        {
            this.SetColor(channels[0], channels[1], channels[2], channels[3]);
        }

        public void SetColor(byte c1, byte c2, byte c3, byte c4)
        {
            this.C1 = ColorUtils.ToChannelFloat(c1);
            this.C2 = ColorUtils.ToChannelFloat(c2);
            this.C3 = ColorUtils.ToChannelFloat(c3);
            this.C4 = ColorUtils.ToChannelFloat(c4);
        }

        public void SetColor(float c1, float c2, float c3, float c4)
        {
            this.C1 = c1;
            this.C2 = c2;
            this.C3 = c3;
            this.C3 = c4;
        }
    }
}