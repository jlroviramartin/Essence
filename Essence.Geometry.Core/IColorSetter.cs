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

namespace Essence.Geometry.Core
{
    public interface IColorSetter
    {
        /// <summary>
        /// Sets the chanels as float values.
        /// </summary>
        /// <param name="channels">Channels</param>
        void SetColor(params float[] channels);

        /// <summary>
        /// Sets the chanels as byte values.
        /// </summary>
        /// <param name="channels">Channels</param>
        void SetColor(params byte[] channels);
    }

    public interface IColorSetter3 : IColorSetter
    {
        /// <summary>
        /// Sets the chanels as float values.
        /// </summary>
        /// <param name="c1">Channel 1</param>
        /// <param name="c2">Channel 2</param>
        /// <param name="c3">Channel 3</param>
        void SetColor(float c1, float c2, float c3);

        /// <summary>
        /// Sets the chanels as byte values.
        /// </summary>
        /// <param name="c1">Channel 1</param>
        /// <param name="c2">Channel 2</param>
        /// <param name="c3">Channel 3</param>
        void SetColor(byte c1, byte c2, byte c3);
    }

    public interface IColorSetter4 : IColorSetter
    {
        /// <summary>
        /// Sets the chanels as float values.
        /// </summary>
        /// <param name="c1">Channel 1</param>
        /// <param name="c2">Channel 2</param>
        /// <param name="c3">Channel 3</param>
        /// <param name="c4">Channel 4</param>
        void SetColor(float c1, float c2, float c3, float c4);

        /// <summary>
        /// Sets the chanels as byte values.
        /// </summary>
        /// <param name="c1">Channel 1</param>
        /// <param name="c2">Channel 2</param>
        /// <param name="c3">Channel 3</param>
        /// <param name="c4">Channel 4</param>
        void SetColor(byte c1, byte c2, byte c3, byte c4);
    }
}