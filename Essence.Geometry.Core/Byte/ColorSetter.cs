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

namespace Essence.Geometry.Core.Byte
{
    public sealed class ColorSetter3b : IColorSetter3
    {
        public Color3b GetColor()
        {
            return new Color3b(this.C1, this.C2, this.C3);
        }

        public byte C1 { get; private set; }

        public byte C2 { get; private set; }

        public byte C3 { get; private set; }

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
            this.C1 = c1;
            this.C2 = c2;
            this.C3 = c3;
        }

        public void SetColor(float c1, float c2, float c3)
        {
            this.C1 = ColorUtils.ToChannelByte(c1);
            this.C2 = ColorUtils.ToChannelByte(c2);
            this.C3 = ColorUtils.ToChannelByte(c3);
        }
    }

    public sealed class ColorSetter4b : IColorSetter4
    {
        public Color4b GetColor()
        {
            return new Color4b(this.C1, this.C2, this.C3, this.C4);
        }

        public byte C1 { get; private set; }

        public byte C2 { get; private set; }

        public byte C3 { get; private set; }

        public byte C4 { get; private set; }

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
            this.C1 = c1;
            this.C2 = c2;
            this.C3 = c3;
            this.C3 = c4;
        }

        public void SetColor(float c1, float c2, float c3, float c4)
        {
            this.C1 = ColorUtils.ToChannelByte(c1);
            this.C2 = ColorUtils.ToChannelByte(c2);
            this.C3 = ColorUtils.ToChannelByte(c3);
            this.C4 = ColorUtils.ToChannelByte(c4);
        }
    }
}