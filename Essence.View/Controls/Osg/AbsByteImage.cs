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

using Essence.Util;

namespace Essence.View.Controls.Osg
{
    public abstract class AbsByteImage : DisposableObject
    {
        protected AbsByteImage(int w, int h, int stride, int pixelSize, byte[] array)
        {
            this.w = w;
            this.h = h;
            this.stride = stride;
            this.pixelSize = pixelSize;
            this.array = array;
        }

        /// <summary>
        ///     Image size in bytes.
        /// </summary>
        public int Size
        {
            get { return this.array.Length; }
        }

        public int W
        {
            get { return this.w; }
        }

        public int H
        {
            get { return this.h; }
        }

        public int Stride
        {
            get { return this.stride; }
        }

        /// <summary>
        ///     Pixel size in bytes.
        /// </summary>
        public int PixelSize
        {
            get { return this.pixelSize; }
        }

        public byte this[int i]
        {
            get { return this.array[i]; }
            set { this.array[i] = value; }
        }

        public byte this[int x, int y]
        {
            get { return this.array[x + y * this.stride]; }
            set { this.array[x + y * this.stride] = value; }
        }

        public abstract void Update();

        private readonly int w, h, stride, pixelSize;
        protected readonly byte[] array;
    }
}