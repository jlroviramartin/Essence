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

#if !NETSTANDARD
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Essence.Geometry.Core.Byte;

namespace Essence.Geometry.Images
{
    /// <summary>
    /// Imagen con formato B G R A
    ///                    0 1 2 3
    /// </summary>
    public class FastImage2D
    {
        public FastImage2D(Bitmap bitmap, ImageType imageType = ImageType.BGRA)
            : this(bitmap.Width, bitmap.Height, imageType)
        {
            this.CopyFrom(bitmap);
        }

        public FastImage2D(int w = 0, int h = 0, ImageType imageType = ImageType.BGRA)
        {
            this.ImageType = imageType;
            this.Resize(w, h);
        }

        public bool TieneValores
        {
            get { return this.ImageData.Any(t => t > 0); }
        }

        public bool Empty
        {
            get { return this.Width == 0 || this.Height == 0; }
        }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public int BytesPerPixel
        {
            get { return this.ImageType == ImageType.BGRA ? 4 : 3; }
        }

        public ImageType ImageType { get; private set; }

        public PixelFormat WinPixelFormat
        {
            get { return this.ImageType == ImageType.BGRA ? PixelFormat.Format32bppArgb : PixelFormat.Format24bppRgb; }
        }

        public byte[] ImageData
        {
            get { return this.imageData; }
        }

        public void Resize(int w, int h)
        {
            if (this.Width != w || this.Height != h)
            {
                this.Width = w;
                this.Height = h;
                int length = this.Width * this.Height * this.BytesPerPixel;
                if ((this.ImageData == null) || (length != this.ImageData.Length))
                {
                    this.imageData = new byte[length];
                }
            }
        }

        public void CopyTo(Bitmap bitmap)
        {
            Debug.Assert(this.Width == bitmap.Width && this.Height == bitmap.Height);

            BitmapData bData = null;
            try
            {
                bData = bitmap.LockBits(new Rectangle(0, 0, this.Width, this.Height),
                                        ImageLockMode.WriteOnly,
                                        this.WinPixelFormat);

                int stride = this.Width * this.BytesPerPixel;
                if (stride == bData.Stride)
                {
                    Marshal.Copy(this.ImageData, 0, bData.Scan0, this.ImageData.Length);
                }
                else
                {
                    // Copy the locked bytes from memory.
                    for (int r = 0; r < bitmap.Height; r++)
                    {
                        Marshal.Copy(this.ImageData, r * stride,
                                     bData.Scan0 + r * bData.Stride,
                                     stride);
                    }
                }
            }
            finally
            {
                if (bData != null)
                {
                    bitmap.UnlockBits(bData);
                }
            }
        }

        public void CopyFrom(Bitmap bitmap)
        {
            Debug.Assert(this.Width == bitmap.Width && this.Height == bitmap.Height);

            BitmapData bData = null;
            try
            {
                bData = bitmap.LockBits(new Rectangle(0, 0, this.Width, this.Height),
                                        ImageLockMode.ReadOnly,
                                        this.WinPixelFormat);

                int stride = this.Width * this.BytesPerPixel;
                if (stride == bData.Stride)
                {
                    Marshal.Copy(bData.Scan0, this.ImageData, 0, this.ImageData.Length);
                }
                else
                {
                    // Copy the locked bytes from memory.
                    for (int r = 0; r < bitmap.Height; r++)
                    {
                        Marshal.Copy(bData.Scan0 + r * bData.Stride,
                                     this.ImageData, r * stride,
                                     stride);
                    }
                }
            }
            finally
            {
                if (bData != null)
                {
                    bitmap.UnlockBits(bData);
                }
            }
        }

        public void SaveTo(string fileName)
        {
            this.SaveTo(new FileInfo(fileName));
        }

        public void SaveTo(FileInfo file)
        {
            Bitmap bitmap = new Bitmap(this.Width, this.Height, this.WinPixelFormat);
            this.CopyTo(bitmap);
            using (Stream stream = file.Open(FileMode.Create))
            {
                bitmap.Save(stream, ImageFormat.Png);
            }
        }

        public void Clear(Color4b color)
        {
            byte[] bgra = { color.Blue, color.Green, color.Red, color.Alpha };

            int w = this.Width;
            int h = this.Height;
            int pixelSpace = this.BytesPerPixel;
            for (int i = 0, sz = w * h * pixelSpace; i < sz; i += pixelSpace)
            {
                for (int j = 0; j < pixelSpace; j++)
                {
                    this.ImageData[i + j] = bgra[j];
                }
            }
        }

        public Color4b this[int x, int y]
        {
            get
            {
                int i = (x + y * this.Width) * this.BytesPerPixel;
                byte b = this.ImageData[i];
                i++;
                byte g = this.ImageData[i];
                i++;
                byte r = this.ImageData[i];
                i++;
                if (this.ImageType == ImageType.BGRA)
                {
                    byte a = this.ImageData[i];
                    return new Color4b(r, g, b, a);
                }
                else
                {
                    return new Color4b(r, g, b, 0);
                }
            }
            set
            {
                int i = (x + y * this.Width) * this.BytesPerPixel;
                this.ImageData[i] = value.Blue;
                i++;
                this.ImageData[i] = value.Green;
                i++;
                this.ImageData[i] = value.Red;
                i++;
                if (this.ImageType == ImageType.BGRA)
                {
                    this.ImageData[i] = value.Alpha;
                }
            }
        }

        #region private

        private byte[] imageData;
        //private int offset;
        //private int stride;

        #endregion
    }

#if false
        public interface IPixelDescription
        {
            int BytesPerPixel { get; }
            int ChannelsCount { get; }

            int GetBytesPerChannel(int i);
             
            int GetChannel(int i, byte[] imageData, int offset);
            float GetChannelAsFloat(int i, byte[] imageData, int offset);
        }
#endif
}
#endif