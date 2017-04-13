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

using System;
using System.Runtime.InteropServices;
using osg;

namespace Essence.View.Controls.Osg
{
    public sealed class ByteImage : AbsByteImage
    {
        public static ByteImage New(Image image)
        {
            IntPtr ptr = image.data();
            int w = image.s();
            int h = image.t();
            int stride = (int)image.getRowSizeInBytes();

            int pixelSize = (int)image.getPixelSizeInBits() / 8;
            //PixelFormat format = ((pixelSize == 4) ? PixelFormat.Format32bppArgb : PixelFormat.Format24bppRgb);

            int bytes = Math.Abs(stride) * h;
            byte[] array = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, array, 0, bytes);

            return new ByteImage(image, w, h, stride, pixelSize, array);
        }

        private ByteImage(Image image, int w, int h, int stride, int pixelSize, byte[] array)
            : base(w, h, stride, pixelSize, array)
        {
            this.image = image;
        }

        public override void Update()
        {
            // Copy the array into the RGB values.
            Marshal.Copy(this.array, 0, this.image.data(), this.array.Length);
        }

        private readonly Image image;
    }
}