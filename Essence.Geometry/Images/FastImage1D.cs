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
    public class FastImage1D
    {
        public FastImage1D(Bitmap bitmap, int y = 0, ImageType imageType = ImageType.BGRA)
            : this(bitmap.Width, imageType)
        {
            this.CopyFrom(bitmap, y);
        }

        public FastImage1D(int w = 0, ImageType imageType = ImageType.BGRA)
        {
            this.ImageType = imageType;
            this.Resize(w);
        }

        public bool TieneValores
        {
            get { return this.ImageData.Any(t => t > 0); }
        }

        public bool Empty
        {
            get { return this.Width == 0; }
        }

        public int Width { get; private set; }

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

        public void Resize(int w)
        {
            if (this.Width != w)
            {
                this.Width = w;
                int length = this.Width * this.BytesPerPixel;
                if ((this.ImageData == null) || (length != this.ImageData.Length))
                {
                    this.imageData = new byte[length];
                }
            }
        }

        public void CopyTo(Bitmap bitmap, int y = 0)
        {
            Debug.Assert(this.Width == bitmap.Width && bitmap.Height > 0);

            BitmapData bData = null;
            try
            {
                bData = bitmap.LockBits(new Rectangle(0, y, this.Width, 1),
                                        ImageLockMode.WriteOnly,
                                        PixelFormat.Format32bppArgb);

                int stride = this.Width * this.BytesPerPixel;
                if (stride == bData.Stride)
                {
                    Marshal.Copy(this.ImageData, 0, bData.Scan0, this.ImageData.Length);
                }
                else
                {
                    // Copy the locked bytes from memory.
                    Marshal.Copy(this.ImageData, stride,
                                 bData.Scan0 + bData.Stride,
                                 stride);
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

        public void CopyFrom(Bitmap bitmap, int y = 0)
        {
            Debug.Assert(this.Width == bitmap.Width && bitmap.Height > 0);

            BitmapData bData = null;
            try
            {
                bData = bitmap.LockBits(new Rectangle(0, y, this.Width, 1),
                                        ImageLockMode.ReadOnly,
                                        PixelFormat.Format32bppArgb);

                int stride = this.Width * this.BytesPerPixel;
                if (stride == bData.Stride)
                {
                    Marshal.Copy(bData.Scan0, this.ImageData, 0, this.ImageData.Length);
                }
                else
                {
                    // Copy the locked bytes from memory.
                    Marshal.Copy(bData.Scan0 + bData.Stride,
                                 this.ImageData, stride,
                                 stride);
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
            Bitmap bitmap = new Bitmap(this.Width, 1, this.WinPixelFormat);
            this.CopyTo(bitmap);
            using (Stream stream = file.Open(FileMode.Create))
            {
                bitmap.Save(stream, ImageFormat.Png);
            }
        }

        public Color4b this[int x]
        {
            get
            {
                int i = x * this.BytesPerPixel;
                byte b = this.ImageData[i];
                i++;
                byte g = this.ImageData[i];
                i++;
                byte r = this.ImageData[i];
                i++;
                byte a = this.ImageData[i];
                return new Color4b(r, g, b, a);
            }
            set
            {
                int i = x * this.BytesPerPixel;
                this.ImageData[i] = value.Blue;
                i++;
                this.ImageData[i] = value.Green;
                i++;
                this.ImageData[i] = value.Red;
                i++;
                this.ImageData[i] = value.Alpha;
            }
        }

        #region private

        private byte[] imageData;
        //private int offset;
        //private int stride;

        #endregion
    }
}
#endif
