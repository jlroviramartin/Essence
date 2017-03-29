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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Essence.Geometry.Core.Double;
using Essence.Geometry.Core.Float;
using Essence.Maths.Double;
using Essence.Maths.Double.Curves;
using Essence.Util.Math.Double;
using SysMath = System.Math;

namespace Essence.Maths
{
    public static class WavefrontFormatUtils
    {
        public static MaterialFormat DefaultColors(this MaterialFormat mf)
        {
            mf.AddMaterial(new MaterialFormat.Mat("Black", Color3f.FromRGB(0, 0, 0)));
            mf.AddMaterial(new MaterialFormat.Mat("White", Color3f.FromRGB(255, 255, 255)));
            mf.AddMaterial(new MaterialFormat.Mat("Red", Color3f.FromRGB(255, 0, 0)));
            mf.AddMaterial(new MaterialFormat.Mat("Lime", Color3f.FromRGB(0, 255, 0)));
            mf.AddMaterial(new MaterialFormat.Mat("Blue", Color3f.FromRGB(0, 0, 255)));
            mf.AddMaterial(new MaterialFormat.Mat("Yellow", Color3f.FromRGB(255, 255, 0)));
            mf.AddMaterial(new MaterialFormat.Mat("Cyan", Color3f.FromRGB(0, 255, 255))); // Aqua
            mf.AddMaterial(new MaterialFormat.Mat("Magenta", Color3f.FromRGB(255, 0, 255))); // Fuchsia
            mf.AddMaterial(new MaterialFormat.Mat("Silver", Color3f.FromRGB(192, 192, 192)));
            mf.AddMaterial(new MaterialFormat.Mat("Gray", Color3f.FromRGB(128, 128, 128)));
            mf.AddMaterial(new MaterialFormat.Mat("Maroon", Color3f.FromRGB(128, 0, 0)));
            mf.AddMaterial(new MaterialFormat.Mat("Olive", Color3f.FromRGB(128, 128, 0)));
            mf.AddMaterial(new MaterialFormat.Mat("Green", Color3f.FromRGB(0, 128, 0)));
            mf.AddMaterial(new MaterialFormat.Mat("Purple", Color3f.FromRGB(128, 0, 128)));
            mf.AddMaterial(new MaterialFormat.Mat("Teal", Color3f.FromRGB(0, 128, 128)));
            mf.AddMaterial(new MaterialFormat.Mat("Navy", Color3f.FromRGB(0, 0, 128)));
            return mf;
        }

        public static WavefrontFormat DrawClotho(this WavefrontFormat wf,
                                                 bool invertY, double a,
                                                 string clothoColor,
                                                 string dirColor,
                                                 string normalColor,
                                                 string radColor)
        {
            List<int> indices = new List<int>();
            List<Tuple<int, int>> normals = new List<Tuple<int, int>>();
            List<Tuple<int, int>> dirs = new List<Tuple<int, int>>();
            List<Tuple<int, int>> radius = new List<Tuple<int, int>>();

            int c = 100;
            for (int i = -c; i <= c; i++)
            {
                double lmax = ClothoUtils.GetMaxL(a);
                double s = i * lmax / c;

                Point2d xy = ClothoUtils.Clotho(s, invertY, a);

                int v0 = wf.AddVertex(xy);
                indices.Add(v0);

                Vector2d n = ClothoUtils.ClothoLeftNormal(s, invertY, a).Unit;

                int v1 = wf.AddVertex(xy.Add(n));
                normals.Add(Tuple.Create(v0, v1));

                double dir = ClothoUtils.ClothoTangent(s, invertY, a);
                Vector2d d = Vector2d.NewRotate(dir);

                int v2 = wf.AddVertex(xy.Add(d.Mul(5)));
                dirs.Add(Tuple.Create(v0, v2));

                double r = ClothoUtils.ClothoRadious(s, invertY, a);
                if (double.IsInfinity(r))
                {
                    r = SysMath.Sign(r) * 100;
                }

                int v3 = wf.AddVertex(xy.Add(n.Mul(r)));
                radius.Add(Tuple.Create(v0, v3));

                //double dx, dy;
                //MathUtils.DClotho(s, r, a, out dx, out dy);
            }

            wf.UseMaterial(clothoColor);
            wf.AddLines(indices, false);

            wf.UseMaterial(normalColor);
            foreach (Tuple<int, int> normal in normals)
            {
                wf.AddLines(new[] { normal.Item1, normal.Item2 }, false);
            }

            wf.UseMaterial(dirColor);
            foreach (Tuple<int, int> dir in dirs)
            {
                wf.AddLines(new[] { dir.Item1, dir.Item2 }, false);
            }

            wf.UseMaterial(radColor);
            foreach (Tuple<int, int> rr in radius)
            {
                wf.AddLines(new[] { rr.Item1, rr.Item2 }, false);
            }
            return wf;
        }

        public static WavefrontFormat DrawClotho(this WavefrontFormat wf,
                                                 bool invertY, double a,
                                                 string clothoColor)
        {
            List<int> indices = new List<int>();

            int c = 100;
            for (int i = -c; i <= c; i++)
            {
                double lmax = ClothoUtils.GetMaxL(a);
                double s = i * lmax / c;

                double x, y;
                ClothoUtils.Clotho(s, invertY, a, out x, out y);

                int v0 = wf.AddVertex(new Point2d(x, y));
                indices.Add(v0);

                //double dx, dy;
                //MathUtils.DClotho(s, r, a, out dx, out dy);
            }

            wf.UseMaterial(clothoColor);
            wf.AddLines(indices, false);
            return wf;
        }

        public static WavefrontFormat DrawClothoRadius(this WavefrontFormat wf,
                                                       bool invertY, double a,
                                                       string radiusColor)
        {
            List<int> indices = new List<int>();

            double lmax = ClothoUtils.GetMaxL(a);
            int c = 100;
            for (int i = -c; i <= c; i++)
            {
                double s = i * lmax / c;

                double r = ClothoUtils.ClothoRadious(s, invertY, a);
                if (double.IsInfinity(r))
                {
                    r = SysMath.Sign(r) * 1000;
                }

                int v0 = wf.AddVertex(new Point2d(s, r));
                indices.Add(v0);

                //double dx, dy;
                //MathUtils.DClotho(s, r, a, out dx, out dy);
            }

            wf.UseMaterial(radiusColor);
            wf.AddLines(indices, false);
            return wf;
        }

        public static WavefrontFormat DrawPolyline(this WavefrontFormat wf,
                                                   string color,
                                                   IEnumerable<Point2d> points,
                                                   bool close)
        {
            wf.UseMaterial(color);
            wf.AddLines(points, close);
            return wf;
        }

        public static WavefrontFormat DrawClotho(this WavefrontFormat wf,
                                                 string color,
                                                 ClothoidArc2 arc)
        {
            wf.UseMaterial(color);
            wf.AddLines(MathUtils.For(arc.TMin, arc.TMax, 10).Select(arc.GetPosition), false);
            return wf;
        }

        public static WavefrontFormat DrawLine(this WavefrontFormat wf,
                                               string color,
                                               Point2d point,
                                               Vector2d v,
                                               double ext = 1000)
        {
            wf.UseMaterial(color);
            Point2d p0 = point.Sub(v.Mul(ext));
            Point2d p1 = point.Add(v.Mul(ext));
            wf.AddLines(new Point2d[] { p0, p1 }, false);
            return wf;
        }

        public static WavefrontFormat DrawCurve(this WavefrontFormat wf,
                                                string color,
                                                ICurve2 curve,
                                                int count = 100)
        {
            wf.UseMaterial(color);
            if (curve is ComposedCurve2)
            {
                int i = 0;
                ComposedCurve2 composed = (ComposedCurve2)curve;
                foreach (ICurve2 segment in composed.GetSegments())
                {
                    /*Point2d pt = segment.GetPosition(segment.TMin);
                    wf.DrawFigure(color, WaveFigure.X, pt, 1);
                    wf.DrawString(color, pt, FontFamily.GenericSerif, FontStyle.Regular, 1000, "" + (i++));*/

                    //if (segment is CircleArc2)
                    {
                        wf.AddLines(MathUtils.For(segment.TMin, segment.TMax, count).Select(segment.GetPosition), false);
                    }
                }

                /*if (!curve.IsClosed)
                {
                    Point2d pt = curve.GetPosition(curve.TMax);
                    wf.DrawFigure(color, WaveFigure.X, pt, 1);
                    wf.DrawString(color, pt, FontFamily.GenericSerif, FontStyle.Regular, 1000, "" + (i++));
                }*/
            }
            else
            {
                wf.AddLines(MathUtils.For(curve.TMin, curve.TMax, count).Select(curve.GetPosition), false);
            }
            return wf;
        }

        public static WavefrontFormat DrawCurve(this WavefrontFormat wf,
                                                string color,
                                                ICurve1 curve,
                                                int count = 100)
        {
            wf.UseMaterial(color);
            if (curve is ComposedCurve1)
            {
                int i = 0;
                ComposedCurve1 composed = (ComposedCurve1)curve;
                foreach (ICurve1 segment in composed.GetSegments())
                {
                    /*Point2d pt = segment.GetPosition(segment.TMin);
                    wf.DrawFigure(color, WaveFigure.X, pt, 1);
                    wf.DrawString(color, pt, FontFamily.GenericSerif, FontStyle.Regular, 1000, "" + (i++));*/

                    //if (segment is CircleArc2)
                    {
                        wf.AddLines(MathUtils.For(segment.TMin, segment.TMax, count).Select(x => new Point2d(x, segment.GetPosition(x))), false);
                    }
                }

                /*if (!curve.IsClosed)
                {
                    Point2d pt = curve.GetPosition(curve.TMax);
                    wf.DrawFigure(color, WaveFigure.X, pt, 1);
                    wf.DrawString(color, pt, FontFamily.GenericSerif, FontStyle.Regular, 1000, "" + (i++));
                }*/
            }
            else
            {
                wf.AddLines(MathUtils.For(curve.TMin, curve.TMax, count).Select(x => new Point2d(x, curve.GetPosition(x))), false);
            }
            return wf;
        }

        public static WavefrontFormat DrawFigure(this WavefrontFormat wf,
                                                 string color,
                                                 WaveFigure figure,
                                                 Point2d point,
                                                 double size)
        {
            wf.UseMaterial(color);
            switch (figure)
            {
                case WaveFigure.Circle:
                {
                    wf.AddLines(MathUtils.For(0, 2 * SysMath.PI, 20).Select(t => point.Add(Vector2d.NewRotate(t).Mul(size))), true);
                    break;
                }
                case WaveFigure.Rectangle:
                {
                    wf.AddLines(new Point2d[]
                    {
                        point.Add(new Point2d(size, size)),
                        point.Add(new Point2d(-size, size)),
                        point.Add(new Point2d(-size, -size)),
                        point.Add(new Point2d(size, -size)),
                    }, true);
                    break;
                }
                case WaveFigure.Diamond:
                {
                    wf.AddLines(new Point2d[]
                    {
                        point.Add(new Point2d(0, size)),
                        point.Add(new Point2d(-size, 0)),
                        point.Add(new Point2d(0, -size)),
                        point.Add(new Point2d(size, 0)),
                    }, true);
                    break;
                }
                case WaveFigure.Plus:
                {
                    wf.AddLines(new Point2d[]
                    {
                        point.Add(new Point2d(0, size)),
                        point.Add(new Point2d(0, -size)),
                    }, false);
                    wf.AddLines(new Point2d[]
                    {
                        point.Add(new Point2d(size, 0)),
                        point.Add(new Point2d(-size, 0)),
                    }, false);
                    break;
                }
                case WaveFigure.X:
                {
                    wf.AddLines(new Point2d[]
                    {
                        point.Add(new Point2d(size, size)),
                        point.Add(new Point2d(-size, -size)),
                    }, false);
                    wf.AddLines(new Point2d[]
                    {
                        point.Add(new Point2d(-size, size)),
                        point.Add(new Point2d(size, -size)),
                    }, false);
                    break;
                }
                default:
                {
                    throw new Exception();
                }
            }
            return wf;
        }
    }

    public enum WaveFigure
    {
        Circle,
        Rectangle,
        Diamond,
        Plus,
        X
    }
}