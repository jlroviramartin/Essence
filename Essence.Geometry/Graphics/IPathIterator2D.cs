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

using Essence.Geometry.Core;

namespace Essence.Geometry.Graphics
{
    public interface IPathIterator2D
    {
        WindingRule GetWindingRule();

        bool Next();

        SegmentType GetType();

        void GetP1(ICoordinateSetter2D p1);

        void GetP2(ICoordinateSetter2D p2);

        void GetP3(ICoordinateSetter2D p3);
    }
}