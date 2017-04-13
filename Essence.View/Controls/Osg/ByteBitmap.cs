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
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Essence.View.Controls.Osg
{
    // https://gist.github.com/tkouba/0b7f8496f1aadcfee2db
    // https://social.msdn.microsoft.com/Forums/vstudio/en-US/9bf9dea5-e21e-4361-a0a6-be331efde835/how-do-you-calculate-the-image-stride-for-a-bitmap
    public sealed class ByteBitmap : AbsByteImage
    {
        public static ByteBitmap New(Bitmap bitmap)
        {
            BitmapData bmpData = null;
            try
            {
                bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                                          ImageLockMode.ReadOnly, bitmap.PixelFormat);

                int pixelSize = System.Drawing.Bitmap.GetPixelFormatSize(bmpData.PixelFormat);

                // Check if bpp (Bits Per Pixel) is 8, 24, or 32
                if (pixelSize != 8 && pixelSize != 24 && pixelSize != 32)
                {
                    throw new ArgumentException("Only 8, 24 and 32 bpp images are supported.");
                }

                IntPtr ptr = bmpData.Scan0;
                int stride = bmpData.Stride;

                int bytes = Math.Abs(stride) * bmpData.Height;
                byte[] array = new byte[bytes];

                // Copy the RGB values into the array.
                Marshal.Copy(ptr, array, 0, array.Length);
                return new ByteBitmap(bitmap, bmpData.Width, bmpData.Height, bmpData.Stride, pixelSize, array);
            }
            finally
            {
                if (bmpData != null)
                {
                    bitmap.UnlockBits(bmpData);
                }
            }
        }

        private ByteBitmap(Bitmap bitmap, int w, int h, int stride, int pixelSize, byte[] array)
            : base(w, h, stride, pixelSize, array)
        {
            this.bitmap = bitmap;
        }

        public override void Update()
        {
            BitmapData bmpData = null;
            try
            {
                bmpData = this.bitmap.LockBits(new Rectangle(0, 0, this.bitmap.Width, this.bitmap.Height),
                                               ImageLockMode.WriteOnly, this.bitmap.PixelFormat);

                IntPtr ptr = bmpData.Scan0;

                // Copy the array into the RGB values.
                Marshal.Copy(this.array, 0, ptr, this.array.Length);
            }
            finally
            {
                if (bmpData != null)
                {
                    this.bitmap.UnlockBits(bmpData);
                }
            }
        }

        private readonly Bitmap bitmap;
    }
}