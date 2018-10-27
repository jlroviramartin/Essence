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

#if !NETSTANDARD
using System.Drawing;
using System.Drawing.Drawing2D;
using System;
using System.Collections.Generic;
using Essence.Maths.Double;

namespace Essence.Maths
{
    public static class WavefrontFormatGDIUtils
    {
        public static void DrawString(this WavefrontFormat wf,
                                      string color,
                                      Vec2d pt,
                                      FontFamily family, FontStyle style, float sizeInPoints,
                                      string text)
        {
            float dpiY = 1;
            float emSize = dpiY * sizeInPoints / 72;
            using (GraphicsPath path = new GraphicsPath())
            {
                //float dpiY = 1;
                path.AddString(text,
                               family, (int)style, emSize,
                               new PointF(0, 0),
                               StringFormat.GenericTypographic);

                float pathheight = path.GetBounds().Height;
                path.Transform(new Matrix(1, 0, 0, -1, 0, pathheight));
                path.Transform(new Matrix(1, 0, 0, 1, (float)pt.X, (float)pt.Y));

                path.Flatten(new Matrix(), 0.01f);

                DrawPath(wf, path);
            }
        }

        public static void DrawString(this WavefrontFormat wf,
                                      string color,
                                      Vec2d pt,
                                      Font font,
                                      string text)
        {
            DrawString(wf, color, pt, font.FontFamily, font.Style, font.SizeInPoints, text);
        }

        private static void DrawPath(this WavefrontFormat wf, GraphicsPath path)
        {
            byte[] types = path.PathData.Types;
            PointF[] points = path.PathData.Points;
            FillMode fillMode = path.FillMode;

            List<Vec2d> _points = new List<Vec2d>();

            for (int i = 0; i < types.Length; i++)
            {
                PathPointType tipo = (PathPointType)types[i] & PathPointType.PathTypeMask;
                switch (tipo)
                {
                    case PathPointType.Start:
                    {
                        _points.Add(new Vec2d(points[i].X, points[i].Y));
                        break;
                    }
                    case PathPointType.Line:
                    {
                        _points.Add(new Vec2d(points[i].X, points[i].Y));
                        break;
                    }
                    case PathPointType.Bezier3:
                    {
                        // NOTA: se pintará una bezier en breve.
                        _points.Add(new Vec2d(points[i].X, points[i].Y));
                        i++;
                        _points.Add(new Vec2d(points[i].X, points[i].Y));
                        i++;
                        _points.Add(new Vec2d(points[i].X, points[i].Y));
                        break;
                    }
                    default:
                    {
                        throw new IndexOutOfRangeException();
                    }
                }

                // El ultimo tipo determina si hay que cerrar.
                bool cerrar = ((PathPointType)types[i] & PathPointType.CloseSubpath) == PathPointType.CloseSubpath;
                if (cerrar)
                {
                    if (_points.Count > 0)
                    {
                        wf.AddLines(_points, true);
                        _points.Clear();
                    }
                }
            }

            if (_points.Count > 0)
            {
                wf.AddLines(_points, false);
                _points.Clear();
            }
        }
    }
}
#endif