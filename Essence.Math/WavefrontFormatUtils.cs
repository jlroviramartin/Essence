using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Essence.Math.Double;
using Essence.Math.Double.Curves;
using SysMath = System.Math;

namespace Essence.Math
{
    public static class WavefrontFormatUtils
    {
        public static MaterialFormat DefaultColors(this MaterialFormat mf)
        {
            mf.AddMaterial(new MaterialFormat.Mat("Black", Vec3d.FromRGB(0, 0, 0)));
            mf.AddMaterial(new MaterialFormat.Mat("White", Vec3d.FromRGB(255, 255, 255)));
            mf.AddMaterial(new MaterialFormat.Mat("Red", Vec3d.FromRGB(255, 0, 0)));
            mf.AddMaterial(new MaterialFormat.Mat("Lime", Vec3d.FromRGB(0, 255, 0)));
            mf.AddMaterial(new MaterialFormat.Mat("Blue", Vec3d.FromRGB(0, 0, 255)));
            mf.AddMaterial(new MaterialFormat.Mat("Yellow", Vec3d.FromRGB(255, 255, 0)));
            mf.AddMaterial(new MaterialFormat.Mat("Cyan", Vec3d.FromRGB(0, 255, 255))); // Aqua
            mf.AddMaterial(new MaterialFormat.Mat("Magenta", Vec3d.FromRGB(255, 0, 255))); // Fuchsia
            mf.AddMaterial(new MaterialFormat.Mat("Silver", Vec3d.FromRGB(192, 192, 192)));
            mf.AddMaterial(new MaterialFormat.Mat("Gray", Vec3d.FromRGB(128, 128, 128)));
            mf.AddMaterial(new MaterialFormat.Mat("Maroon", Vec3d.FromRGB(128, 0, 0)));
            mf.AddMaterial(new MaterialFormat.Mat("Olive", Vec3d.FromRGB(128, 128, 0)));
            mf.AddMaterial(new MaterialFormat.Mat("Green", Vec3d.FromRGB(0, 128, 0)));
            mf.AddMaterial(new MaterialFormat.Mat("Purple", Vec3d.FromRGB(128, 0, 128)));
            mf.AddMaterial(new MaterialFormat.Mat("Teal", Vec3d.FromRGB(0, 128, 128)));
            mf.AddMaterial(new MaterialFormat.Mat("Navy", Vec3d.FromRGB(0, 0, 128)));
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
                double lmax = MathUtils.GetMaxL(a);
                double s = i * lmax / c;

                double x, y;
                MathUtils.Clotho(s, invertY, a, out x, out y);

                int v0 = wf.AddVertex(new Vec2d(x, y));
                indices.Add(v0);

                Vec2d n = MathUtils.ClothoLeftNormal(s, invertY, a).Norm();

                int v1 = wf.AddVertex(new Vec2d(x + n.X, y + n.Y));
                normals.Add(Tuple.Create(v0, v1));

                double dir = MathUtils.ClothoTangent(s, invertY, a);
                double dx = SysMath.Cos(dir);
                double dy = SysMath.Sin(dir);
                Vec2d d = new Vec2d(dx, dy).Norm();

                int v2 = wf.AddVertex(new Vec2d(x + 5 * d.X, y + 5 * d.Y));
                dirs.Add(Tuple.Create(v0, v2));

                double r = MathUtils.ClothoRadious(s, invertY, a);
                if (double.IsInfinity(r))
                {
                    r = SysMath.Sign(r) * 100;
                }

                int v3 = wf.AddVertex(new Vec2d(x + r * n.X, y + r * n.Y));
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
                double lmax = MathUtils.GetMaxL(a);
                double s = i * lmax / c;

                double x, y;
                MathUtils.Clotho(s, invertY, a, out x, out y);

                int v0 = wf.AddVertex(new Vec2d(x, y));
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

            double lmax = MathUtils.GetMaxL(a);
            int c = 100;
            for (int i = -c; i <= c; i++)
            {
                double s = i * lmax / c;

                double r = MathUtils.ClothoRadious(s, invertY, a);
                if (double.IsInfinity(r))
                {
                    r = SysMath.Sign(r) * 1000;
                }

                int v0 = wf.AddVertex(new Vec2d(s, r));
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
                                                   IEnumerable<Vec2d> points,
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
                                               Vec2d point,
                                               Vec2d v,
                                               double ext = 1000)
        {
            wf.UseMaterial(color);
            Vec2d p0 = point.Sub(v.Mul(ext));
            Vec2d p1 = point.Add(v.Mul(ext));
            wf.AddLines(new Vec2d[] { p0, p1 }, false);
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
                    /*Vec2d pt = segment.GetPosition(segment.TMin);
                    wf.DrawFigure(color, WaveFigure.X, pt, 1);
                    wf.DrawString(color, pt, FontFamily.GenericSerif, FontStyle.Regular, 1000, "" + (i++));*/

                    //if (segment is CircleArc2)
                    {
                        wf.AddLines(MathUtils.For(segment.TMin, segment.TMax, count).Select(segment.GetPosition), false);
                    }
                }

                /*if (!curve.IsClosed)
                {
                    Vec2d pt = curve.GetPosition(curve.TMax);
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

        public static WavefrontFormat DrawFigure(this WavefrontFormat wf,
                                                 string color,
                                                 WaveFigure figure,
                                                 Vec2d point,
                                                 double size)
        {
            wf.UseMaterial(color);
            switch (figure)
            {
                case WaveFigure.Circle:
                {
                    wf.AddLines(MathUtils.For(0, 2 * SysMath.PI, 20).Select(t => point.Add(vecMath.NewRotate(t).Mul(size))), true);
                    break;
                }
                case WaveFigure.Rectangle:
                {
                    wf.AddLines(new Vec2d[]
                    {
                        point.Add(new Vec2d(size, size)),
                        point.Add(new Vec2d(-size, size)),
                        point.Add(new Vec2d(-size, -size)),
                        point.Add(new Vec2d(size, -size)),
                    }, true);
                    break;
                }
                case WaveFigure.Diamond:
                {
                    wf.AddLines(new Vec2d[]
                    {
                        point.Add(new Vec2d(0, size)),
                        point.Add(new Vec2d(-size, 0)),
                        point.Add(new Vec2d(0, -size)),
                        point.Add(new Vec2d(size, 0)),
                    }, true);
                    break;
                }
                case WaveFigure.Plus:
                {
                    wf.AddLines(new Vec2d[]
                    {
                        point.Add(new Vec2d(0, size)),
                        point.Add(new Vec2d(0, -size)),
                    }, false);
                    wf.AddLines(new Vec2d[]
                    {
                        point.Add(new Vec2d(size, 0)),
                        point.Add(new Vec2d(-size, 0)),
                    }, false);
                    break;
                }
                case WaveFigure.X:
                {
                    wf.AddLines(new Vec2d[]
                    {
                        point.Add(new Vec2d(size, size)),
                        point.Add(new Vec2d(-size, -size)),
                    }, false);
                    wf.AddLines(new Vec2d[]
                    {
                        point.Add(new Vec2d(-size, size)),
                        point.Add(new Vec2d(size, -size)),
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

        private static VecMath<double, DoubleMath, Vec2d, Vec2dFactory> vecMath = VecMath<double, DoubleMath, Vec2d, Vec2dFactory>.Instance;
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