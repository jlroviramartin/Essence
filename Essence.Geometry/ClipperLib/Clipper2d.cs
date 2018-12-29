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
using System.Diagnostics;
using System.Linq;
using Essence.Geometry.Core.Double;

namespace ClipperLib
{
    public class Clipper2d
    {
        public Clipper2d(IClipperConverter2d converter)
        {
            this.converter = converter;
            this.clipper = new Clipper(Clipper.ioStrictlySimple);
            if (converter.UseZFill)
            {
                this.clipper.ZFillFunction = this.converter.ZFill;
            }
        }

        public void Clear()
        {
            this.clipper.Clear();
        }

        public bool AddPath(IEnumerable<Point2d> poly, PolyType type, bool closed)
        {
            bool ret = this.clipper.AddPath(poly.Select(p => this.converter.ToIntPoint(p)).ToList(), type, closed);
            if (!ret)
            {
                Debug.WriteLine("Error in clipper.AddPath");
            }
            return ret;
        }

        public bool AddPaths<TPoly>(IEnumerable<TPoly> polys, PolyType type, bool closed)
            where TPoly : IEnumerable<Point2d>
        {
            bool ret = this.clipper.AddPaths(polys.Select(poly => poly.Select(p => this.converter.ToIntPoint(p)).ToList()).ToList(), type, closed);
            if (!ret)
            {
                Debug.WriteLine("Error in clipper.AddPaths");
            }
            return ret;
        }

        public bool Execute(ClipType clipType, List<List<Point2d>> solution,
                            PolyFillType fillType = PolyFillType.pftEvenOdd)
        {
            return this.Execute(clipType, solution, fillType, fillType);
        }

        public bool Execute(ClipType clipType, List<List<Point2d>> solution,
                            PolyFillType subjFillType,
                            PolyFillType clipFillType)
        {
            List<List<IntPoint>> _solution = new List<List<IntPoint>>();
            bool ret = this.clipper.Execute(clipType, _solution, subjFillType, clipFillType);
            if (!ret)
            {
                Debug.WriteLine("Error in clipper.Execute");
            }
            solution.AddRange(_solution.Select(poly => poly.Select(p => this.converter.FromIntPoint(p)).ToList()));
            return ret;
        }

        public void SimplifyPolygon(IEnumerable<Point2d> poly, List<Point2d> solution,
                                    PolyFillType fillType = PolyFillType.pftEvenOdd)
        {
            List<IntPoint> _poly = poly.Select(p => this.converter.ToIntPoint(p)).ToList();
            Clipper.SimplifyPolygon(_poly, fillType);
            solution.AddRange(_poly.Select(p => this.converter.FromIntPoint(p)));
        }

        public void SimplifyPolygons<TPoly>(IEnumerable<TPoly> polys, List<List<Point2d>> solution,
                                            PolyFillType fillType = PolyFillType.pftEvenOdd)
            where TPoly : IEnumerable<Point2d>
        {
            List<List<IntPoint>> _polys = polys.Select(poly => poly.Select(p => this.converter.ToIntPoint(p)).ToList()).ToList();
            Clipper.SimplifyPolygons(_polys, fillType);
            solution.AddRange(_polys.Select(poly => poly.Select(p => this.converter.FromIntPoint(p)).ToList()));
        }

        #region private

        private readonly IClipperConverter2d converter;
        private readonly Clipper clipper;

        #endregion
    }
}