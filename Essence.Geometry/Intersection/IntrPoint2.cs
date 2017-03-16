#region License

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

#endregion

using Essence.Geometry.Core.Double;
using REAL = System.Double;

namespace Essence.Geometry.Intersection
{
    public class IntrPoint2
    {
        public IntrPoint2(REAL param0, REAL param1, Point2d point)
        {
            this.Param0 = param0;
            this.Param1 = param1;
            this.Point = point;
        }

        /// <summary>Parametro respecto del elemento 0.</summary>
        public readonly REAL Param0;

        /// <summary>Parametro respecto del elemento 1.</summary>
        public readonly REAL Param1;

        /// <summary>Punto.</summary>
        public readonly Point2d Point;
    }
}