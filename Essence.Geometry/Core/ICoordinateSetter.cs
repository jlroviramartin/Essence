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
    public interface ICoordinateSetter
    {
        void SetCoords(params double[] coords);

        void SetCoords(params float[] coords);

        void SetCoords(params int[] coords);
    }

    public interface ICoordinateSetter2D : ICoordinateSetter
    {
        void SetCoords(double x, double y);

        void SetCoords(float x, float y);

        void SetCoords(int x, int y);
    }

    public interface ICoordinateSetter3D : ICoordinateSetter
    {
        void SetCoords(double x, double y, double z);

        void SetCoords(float x, float y, float z);
    }

    public interface ICoordinateSetter4D : ICoordinateSetter
    {
        void SetCoords(double x, double y, double z, double w);

        void SetCoords(float x, float y, float z, float w);
    }
}