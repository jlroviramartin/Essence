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

using System.Collections.Generic;
using System.Linq;
using Essence.Geometry.Core.Double;

namespace ClipperLib
{
    public class ClipperOffset2d
    {
        public ClipperOffset2d(IClipperConverter2d converter, double miterLimit = 2.0, double arcTolerance = 0.25)
        {
            this.converter = converter;
            this.offset = new ClipperOffset(miterLimit, arcTolerance);
        }

        public void Clear()
        {
            this.offset.Clear();
        }

        public void AddPath(IEnumerable<Point2d> poly, JoinType joinType, EndType endType)
        {
            this.offset.AddPath(poly.Select(p => this.converter.ToIntPoint(p)).ToList(), joinType, endType);
        }

        public void AddPaths<TPoly>(IEnumerable<TPoly> polys, JoinType joinType, EndType endType)
            where TPoly : IEnumerable<Point2d>
        {
            this.offset.AddPaths(polys.Select(poly => poly.Select(p => this.converter.ToIntPoint(p)).ToList()).ToList(), joinType, endType);
        }

        public void Execute(List<List<Point2d>> solution, double delta)
        {
            List<List<IntPoint>> _solution = new List<List<IntPoint>>();
            this.offset.Execute(_solution, delta);
            foreach (List<IntPoint> poly in _solution)
            {
                solution.Add(poly.Select(p => this.converter.FromIntPoint(p)).ToList());
            }
        }

        #region private

        private readonly IClipperConverter2d converter;
        private readonly ClipperOffset offset;

        #endregion
    }
}