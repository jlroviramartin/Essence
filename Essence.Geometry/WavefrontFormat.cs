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

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Linq;
using Essence.Geometry.Core;
using Essence.Geometry.Core.Byte;
using Essence.Geometry.Core.Double;
using Essence.Maths.Double;
using Essence.Util;

namespace Essence.Maths
{
    public class WavefrontFormat : DisposableObject
    {
        public WavefrontFormat(string fileName)
            : this(new FileInfo(fileName))
        {
        }

        public WavefrontFormat(FileInfo file)
            : this(file.Open(FileMode.Create, FileAccess.Write))
        {
        }

        public WavefrontFormat(Stream stream)
            : this(new StreamWriter(stream))
        {
        }

        public WavefrontFormat(StreamWriter streamWriter)
        {
            this.Transform = Transform2D.Identity();
            this.streamWriter = streamWriter;
        }

        // NOTA: Transform3 !!!
        public ITransform2D Transform { get; set; }

        public bool UseObjectGroup { get; set; }

        public void Close()
        {
            this.streamWriter.Close();
        }

        public void BeginObject(string name)
        {
            Contract.Assert(this.currentObject == null);

            this.WritePadding();
            this.streamWriter.Write("o [");
            this.streamWriter.Write(name);
            this.streamWriter.WriteLine("]");

            this.currentObject = name;
        }

        public void BeginGroup(string name)
        {
            Contract.Assert((this.currentObject != null) && (this.currentGroup == null));

            this.WritePadding();
            this.streamWriter.Write("g [");
            this.streamWriter.Write(name);
            this.streamWriter.WriteLine("]");

            this.currentGroup = name;
        }

        public void EndGroup()
        {
            Contract.Assert(this.currentGroup != null);
            this.currentGroup = null;
        }

        public void EndObject()
        {
            Contract.Assert((this.currentObject != null) && (this.currentGroup == null));
            this.currentObject = null;
        }

        public void LoadMaterialLib(string name)
        {
            this.WritePadding();
            this.streamWriter.Write("mtllib ");
            this.streamWriter.WriteLine(name.Replace('\\', '/'));
        }

        public void LoadMaterialLib(FileInfo file)
        {
            this.LoadMaterialLib(file.FullName);
        }

        public void UseMaterial(string name)
        {
            this.WritePadding();
            this.streamWriter.Write("usemtl ");
            this.streamWriter.WriteLine(name);
        }

        public int AddVertex(Point3d v)
        {
            Point2d v2 = this.Transform.DoTransform(new Point2d(v.X, v.Y));
            this.WritePadding();
            this.streamWriter.WriteLine(string.Format(en_US,
                                                      "v {0:F3} {1:F3} {2:F3}",
                                                      v2.X, v2.Y, v.Z));
            return this.vertexIndex++;
        }

        public int AddVertex(Point2d v)
        {
            Point2d v2 = this.Transform.DoTransform(v);
            this.WritePadding();
            this.streamWriter.WriteLine(string.Format(en_US,
                                                      "v {0:F3} {1:F3} {2:F3}",
                                                      v2.X, v2.Y, 0));
            return this.vertexIndex++;
        }

        public int AddTexture(Vector2d tex)
        {
            this.WritePadding();
            this.streamWriter.WriteLine(string.Format(en_US,
                                                      "vt {0:F3} {1:F3}",
                                                      tex.X, tex.Y));
            return this.textureIndex++;
        }

        public int AddNormal(Vector3d n)
        {
            Vector2d n2 = this.Transform.DoTransform(new Vector2d(n.X, n.Y));
            this.WritePadding();
            this.streamWriter.WriteLine(string.Format(en_US,
                                                      "vn  {0:F3} {1:F3} {2:F3}",
                                                      n2.X, n2.Y, n.Z));
            return this.normalIndex++;
        }

        public void AddPoints(IEnumerable<int> indices)
        {
            this.WritePadding();
            this.streamWriter.Write("p");
            foreach (int index in indices)
            {
                this.streamWriter.Write(" ");
                this.streamWriter.Write(index.ToString(en_US));
            }
            this.streamWriter.WriteLine();
        }

        public void AddPoints(IEnumerable<Point3d> points)
        {
            List<int> indices = points.Select(p => this.AddVertex(p)).ToList();
            this.AddPoints(indices);
        }

        public void AddPoints(IEnumerable<Point2d> points)
        {
            List<int> indices = points.Select(p => this.AddVertex(p)).ToList();
            this.AddPoints(indices);
        }

        public void AddLines(IEnumerable<int> indices, bool close = false)
        {
            int? first = null;

            this.WritePadding();
            this.streamWriter.Write("l");
            foreach (int index in indices)
            {
                if (first == null)
                {
                    first = index;
                }

                this.streamWriter.Write(" ");
                this.streamWriter.Write(index.ToString(en_US));
            }
            if (close && (first != null))
            {
                this.streamWriter.Write(" ");
                this.streamWriter.Write(((int)first).ToString(en_US));
            }
            this.streamWriter.WriteLine();
        }

        public void AddLines(IEnumerable<Point3d> points, bool close = false)
        {
            List<int> indices = points.Select(p => this.AddVertex(p)).ToList();
            this.AddLines(indices, close);
        }

        public void AddLines(IEnumerable<Point2d> points, bool close = false)
        {
            List<int> indices = points.Select(p => this.AddVertex(p)).ToList();
            this.AddLines(indices, close);
        }

        public void AddFace(params int[] indices)
        {
            this.AddFace((IEnumerable<int>)indices);
        }

        public void AddFace(IEnumerable<int> indices)
        {
            this.WritePadding();
            this.streamWriter.Write("f");
            foreach (int index in indices)
            {
                this.streamWriter.Write(" ");
                this.streamWriter.Write(index.ToString(en_US));
            }
            this.streamWriter.WriteLine();
        }

        public void AddFace(IEnumerable<Point3d> points)
        {
            List<int> indices = points.Select(p => this.AddVertex(p)).ToList();
            this.AddFace(indices);
        }

        public void AddFace(IEnumerable<Point2d> points)
        {
            List<int> indices = points.Select(p => this.AddVertex(p)).ToList();
            this.AddFace(indices);
        }

        public void AddFace(IEnumerable<Face> faces, FaceMask mask)
        {
            this.WritePadding();
            this.streamWriter.Write("f");
            foreach (Face face in faces)
            {
                this.streamWriter.Write(" ");
                this.streamWriter.Write(face.Vertex.ToString(en_US));
                if (mask == FaceMask.Tex || mask == FaceMask.Normal)
                {
                    this.streamWriter.Write("/");
                    if (mask == FaceMask.Tex)
                    {
                        this.streamWriter.Write(face.Tex.ToString(en_US));
                    }
                    if (mask == FaceMask.Normal)
                    {
                        this.streamWriter.Write("/");
                        this.streamWriter.Write(face.Normal.ToString(en_US));
                    }
                }
            }
            this.streamWriter.WriteLine();
        }

        #region Miembros privados

        /*private CurrentData GetCurrent()
        {
            string name = "";
            if (this.currentObject == null)
            {
                return this.current;
            }

            if (this.currentGroup == null)
            {
                CurrentData currentData;
                if (!objects.TryGetValue(this.currentObject, out currentData))
                {
                    currentData.
                }
            }
        }*/

        private void WritePadding()
        {
            if (this.currentObject != null)
            {
                this.streamWriter.Write("  ");
            }
            if (this.currentGroup != null)
            {
                this.streamWriter.Write("  ");
            }
        }

        private string currentObject;
        private string currentGroup;

        // Informacion de cultura ingles/estados unidos (de donde es el DXF????? pues eso).
        private static readonly CultureInfo en_US = new CultureInfo("en-US");

        private readonly StreamWriter streamWriter;

        private int vertexIndex = 1;
        private int textureIndex = 1;
        private int normalIndex = 1;

        #endregion Miembros privados _______________________________________________________________

        #region Miembros DisposableObject

        protected override void DisposeOfManagedResources()
        {
            this.Close();

            base.DisposeOfManagedResources();
        }

        #endregion Miembros DisposableObject _______________________________________________________

        #region Clases internas

        [Flags]
        public enum FaceMask
        {
            Vertex = 1,
            Tex = 2,
            Normal = 4
        }

        public struct Face
        {
            public int Vertex;
            public int Tex;
            public int Normal;
        }

        #endregion Clases internas _________________________________________________________________
    }

    public class MaterialFormat : DisposableObject
    {
        public MaterialFormat(string fileName)
            : this(new FileInfo(fileName))
        {
        }

        public MaterialFormat(FileInfo file)
            : this(file.Open(FileMode.Create, FileAccess.Write))
        {
        }

        public MaterialFormat(Stream stream)
            : this(new StreamWriter(stream))
        {
        }

        public MaterialFormat(StreamWriter streamWriter)
        {
            this.streamWriter = streamWriter;
        }

        public void Close()
        {
            this.streamWriter.Close();
        }

        public void AddMaterial(Mat mat)
        {
            this.map.Add(mat.Name, mat);

            this.streamWriter.Write(string.Format(en_US, "newmtl {0}", mat.Name));
            this.streamWriter.WriteLine();
            if (mat.Ambient != null)
            {
                IColor3 c = mat.Ambient;
                this.streamWriter.Write(string.Format(en_US, "  Ka {0:F3} {1:F3} {2:F3}", c.Red.ToSingle(), c.Green.ToSingle(), c.Blue.ToSingle()));
                this.streamWriter.WriteLine();
            }
            if (mat.Diffuse != null)
            {
                IColor3 c = mat.Diffuse;
                this.streamWriter.Write(string.Format(en_US, "  Kd {0:F3} {1:F3} {2:F3}", c.Red.ToSingle(), c.Green.ToSingle(), c.Blue.ToSingle()));
                this.streamWriter.WriteLine();
            }
            if (mat.Specular != null)
            {
                IColor3 c = mat.Specular;
                this.streamWriter.Write(string.Format(en_US, "  Ks {0:F3} {1:F3} {2:F3}", c.Red.ToSingle(), c.Green.ToSingle(), c.Blue.ToSingle()));
                this.streamWriter.WriteLine();
            }
            this.streamWriter.Write(string.Format(en_US, "  illum {0}", (int)mat.Model));
            this.streamWriter.WriteLine();
        }

        public bool TryGetValue(string name, out Mat mat)
        {
            return this.map.TryGetValue(name, out mat);
        }

        public bool Contains(string name)
        {
            return this.map.ContainsKey(name);
        }

        public Mat this[string name]
        {
            get { return this.map[name]; }
        }

        #region Miembros privados

        // Informacion de cultura ingles/estados unidos (de donde es el DXF????? pues eso).
        private static readonly CultureInfo en_US = new CultureInfo("en-US");

        private readonly Dictionary<string, Mat> map = new Dictionary<string, Mat>();

        private readonly StreamWriter streamWriter;

        #endregion Miembros privados _______________________________________________________________

        #region Miembros DisposableObject

        protected override void DisposeOfManagedResources()
        {
            this.Close();

            base.DisposeOfManagedResources();
        }

        #endregion Miembros DisposableObject _______________________________________________________

        #region Clases internas

        public class Mat
        {
            public Mat()
            {
            }

            public Mat(string name, IColor3 diffuse)
            {
                this.Name = name;
                this.Ambient = null;
                this.Diffuse = diffuse;
                this.Specular = null;
                this.Model = MaterialFormat.IlluminationModel.ColorOnAmbientOff;
            }

            public string Name;

            public IColor3 Ambient;
            public IColor3 Diffuse;
            public IColor3 Specular;
            public IlluminationModel Model;
        }

        public enum IlluminationModel
        {
            ColorOnAmbientOff = 0,
            ColorOnAmbientOn = 1,
            HighlightOn = 2,
            ReflectionOnRayTraceOn = 3,
            GlassOnRayTraceOn = 4,
            FresnelOnRayTraceOn = 5,
            RefractionOnFresnelOffRayTraceOn = 6,
            RefractionOnFresnelOnRayTraceOn = 7,
            ReflectionOnRayTraceOff = 8,
            GlassOnRayTraceOff = 9,
            CastsShadows = 10
        }

        #endregion Clases internas _________________________________________________________________
    }
}