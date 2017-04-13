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
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Essence.Geometry.Core;
using Essence.Geometry.Core.Byte;
using Essence.Geometry.Core.Double;
using Essence.Geometry.Core.Float;
using Essence.Util;
using Essence.Util.Logs;
using Essence.Util.Properties;
using osg;
using osgDB;
using osgGA;
using osgViewer;
using Image = osg.Image;
using Light = Essence.Geometry.Core.Light;

namespace Essence.View.Controls.Osg
{
    public static class OSGUtils
    {
        public static bool UseShaders = true;

        // Se necesita UseOsgEarth
        public static bool UseOsgEarthShaders = true;

        public static bool UseOsgEarth = true;

        #region Nodes

        /// <summary>
        ///     Busca el nodo indicado haciendo un recorrido en anchura.
        /// </summary>
        public static Node FindNode(this Node root, Func<Node, bool> test)
        {
            Queue<Node> stack = new Queue<Node>();
            stack.Enqueue(root);
            while (stack.Count > 0)
            {
                Node n = stack.Dequeue();

                if (test(n))
                {
                    return n;
                }

                Group g = n.asGroup();
                if (g != null)
                {
                    for (uint i = 0; i < g.getNumChildren(); i++)
                    {
                        stack.Enqueue(g.getChild(i));
                    }
                }
            }
            return null;
        }

        /// <summary>
        ///     Busca el nodo indicado utilizando un <c>NodeVisitor</c>.
        /// </summary>
        public static Node FindNode2(this Node root, Func<Node, bool> test)
        {
            FindVisitor visitor = new FindVisitor(test);
            visitor.setTraversalMode(NodeVisitor.TraversalMode.TRAVERSE_ALL_CHILDREN);
            root.accept(visitor);
            return visitor.Found;
        }

        /*public static Viewer GetOsgViewer(this ContextoGrafico contexto)
        {
            return contexto.GetParam<Viewer>("OsgViewer");
        }

        public static Group GetItemsNode(this ContextoGrafico contexto)
        {
            return contexto.GetParam<Group>("ItemsNode");
        }

        public static Group GetRootNode(this ContextoGrafico contexto)
        {
            return contexto.GetParam<Group>("RootNode");
        }*/

        public static Node GroupNodes(IEnumerable<Node> nodes)
        {
            // Enumeracion de los nodos.
            IEnumerator<Node> enumerator = nodes.Where(x => x != null).GetEnumerator();

            // Si no hay nodos, se devuelve null.
            if (!enumerator.MoveNext())
            {
                return null;
            }

            // Si solo hay un nodo, se devuelve el nodo actual.
            Node node = enumerator.Current;
            if (!enumerator.MoveNext())
            {
                return node;
            }

            // Se crea un grupo y se añaden los nodos.
            Group group = new Group();
            group.addChild(node);
            do
            {
                group.addChild(enumerator.Current);
            } while (enumerator.MoveNext());
            return group;
        }

        public static void AddChildren(this Group group, IEnumerable<Node> nodes)
        {
            foreach (Node node in nodes)
            {
                group.addChild(node);
            }
        }

        /// <summary>
        ///     http://snipplr.com/view/30978/osg-wireframe-display/
        /// </summary>
        public static void ShowWireframe(this Node src)
        {
            ShowWireframe(src.getOrCreateStateSet());
        }

        public static void ShowWireframe(this StateSet stateSet)
        {
            //StateSet stateSet = new StateSet();
            //src.setStateSet(stateSet);

            PolygonMode polymode = new PolygonMode();
            polymode.setMode(PolygonMode.Face.FRONT_AND_BACK, PolygonMode.Mode.LINE);
            stateSet.setAttributeAndModes(polymode, StateAttribute.Values.OVERRIDE | StateAttribute.Values.ON);
        }

        public static void DisableShaders(this Node src)
        {
            DisableShaders(src.getOrCreateStateSet());
        }

        public static void DisableShaders(this StateSet stateSet)
        {
            stateSet.setAttribute(new Program(), StateAttribute.Values.OVERRIDE);
        }

        public static void DisableLights(this Node src)
        {
            DisableLights(src.getOrCreateStateSet());
        }

        public static void DisableLights(this StateSet stateSet)
        {
            stateSet.setMode((uint)OsgModule.GL_LIGHTING, (uint)StateAttribute.Values.OFF);
        }

        public static void DisableCullFace(this Node src)
        {
            DisableCullFace(src.getOrCreateStateSet());
        }

        public static void DisableCullFace(this StateSet stateSet)
        {
            stateSet.setMode((uint)OsgModule.GL_CULL_FACE, (uint)StateAttribute.Values.OFF);
        }

        public static void SetPolygonMode(this Node srcNode, PolygonMode.Mode mode)
        {
            Contract.Requires(srcNode != null);

            // Force the Node and its children to be wireframe, overrides parents state.
            StateSet state = srcNode.getOrCreateStateSet();
            state.removeAttribute(StateAttribute.Type.POLYGONMODE);
            state.setAttribute(new PolygonMode(PolygonMode.Face.FRONT_AND_BACK, mode));
        }

        public static void SetPolygonOffset(this Node srcNode, float factor, float units)
        {
            Contract.Requires(srcNode != null);

            StateSet state = srcNode.getOrCreateStateSet();
            state.removeAttribute(StateAttribute.Type.POLYGONOFFSET);
            state.setAttribute(new PolygonOffset(factor, units));
        }

        public static void PrintNodes(Node root)
        {
            Debug.WriteLine("----- Nodes -----");
            Debug.WriteLine("{");
            Debug.Indent();
            OSGUtils.PrintNodes(root,
                                node =>
                                {
                                    BoundingSphered bsph = node.computeBound();
                                    return string.Format("+ {0} [ [{1:F2} ; {2:F2} ; {3:F2} ]; - {4:F2} ]",
                                                         node.getName(),
                                                         bsph._center.X, bsph._center.Y, bsph._center.Z,
                                                         bsph._radius);
                                });
            Debug.Unindent();
            Debug.WriteLine("}");
        }

        public static void PrintNodes(Node node, Func<Node, string> toString)
        {
            Debug.WriteLine(toString(node));

            Debug.Indent();
            Group g = node.asGroup();
            if (g != null)
            {
                for (uint i = 0; i < g.getNumChildren(); i++)
                {
                    PrintNodes(g.getChild(i), toString);
                }
            }
            Debug.Unindent();
        }

        #endregion

        public static osg.Light ToLight(this Light light, uint num)
        {
            osg.Light osgLight = new osg.Light(num);

            if (light.Focus)
            {
                osgLight.setPosition(new Vec4f(light.Position.ToVec3f(), 1));

                osgLight.setDirection(light.Direction.ToVec3f());

                osgLight.setConstantAttenuation(light.ConstantAttenuation);
                osgLight.setLinearAttenuation(light.LinearAttenuation);
                osgLight.setQuadraticAttenuation(light.QuadraticAttenuation);

                osgLight.setSpotCutoff(light.SpotCutoff);
                osgLight.setSpotExponent(light.SpotExponent);
            }
            else
            {
                osgLight.setPosition(new Vec4f(light.Direction.ToVec3f(), 0));

                osgLight.setDirection(new Vec3f(0, 0, -1));

                osgLight.setConstantAttenuation(1);
                osgLight.setLinearAttenuation(0);
                osgLight.setQuadraticAttenuation(0);

                osgLight.setSpotCutoff(180);
                osgLight.setSpotExponent(0);
            }

            osgLight.setAmbient(ToVec4f(light.Ambient));
            osgLight.setDiffuse(ToVec4f(light.Diffuse));
            osgLight.setSpecular(ToVec4f(light.Especular));
            return osgLight;
        }

        #region Colors

        public static Vec4f ToVec4f(this Color3b color)
        {
            return new Vec4f(ByteColor.GetSingle(color.Red), ByteColor.GetSingle(color.Green), ByteColor.GetSingle(color.Blue), 1);
        }

        public static Vec4f ToVec4f(this Color4b color)
        {
            return new Vec4f(ByteColor.GetSingle(color.Red), ByteColor.GetSingle(color.Green), ByteColor.GetSingle(color.Blue), ByteColor.GetSingle(color.Alpha));
        }

        public static Vec4ub ToVec4ub(this Color3b color)
        {
            return new Vec4ub(color.Red, color.Green, color.Blue, 255);
        }

        public static Vec4ub ToVec4ub(this Color4b color)
        {
            return new Vec4ub(color.Red, color.Green, color.Blue, color.Alpha);
        }

        public static Vec4f ToVec4f(this Color3f color)
        {
            return new Vec4f(color.Red, color.Green, color.Blue, 1);
        }

        public static Vec4f ToVec4f(this Color4f color)
        {
            return new Vec4f(color.Red, color.Green, color.Blue, color.Alpha);
        }

        public static Vec4ub ToVec4ub(this Color3f color)
        {
            return new Vec4ub(FloatColor.GetByte(color.Red), FloatColor.GetByte(color.Green), FloatColor.GetByte(color.Blue), 255);
        }

        public static Vec4ub ToVec4ub(this Color4f color)
        {
            return new Vec4ub(FloatColor.GetByte(color.Red), FloatColor.GetByte(color.Green), FloatColor.GetByte(color.Blue), FloatColor.GetByte(color.Alpha));
        }

        #endregion

        #region Vectors

        public static Vec2d ToVec2d(this Vector2d v)
        {
            return new Vec2d(v.X, v.Y);
        }

        public static Vec2f ToVec2f(this Vector2d v)
        {
            return new Vec2f((float)v.X, (float)v.Y);
        }

        public static Vec3d ToVec3d(this Vector3d v)
        {
            return new Vec3d(v.X, v.Y, v.Z);
        }

        public static Vec3f ToVec3f(this Vector3d v)
        {
            return new Vec3f((float)v.X, (float)v.Y, (float)v.Z);
        }

        public static Vec4d ToVec4d(this Vector4d v)
        {
            return new Vec4d(v.X, v.Y, v.Z, v.W);
        }

        public static Vec4f ToVec4f(this Vector4d v)
        {
            return new Vec4f((float)v.X, (float)v.Y, (float)v.Z, (float)v.W);
        }

        public static Vector3d ToVector3d(this Vec3d vec)
        {
            return new Vector3d(vec.x(), vec.y(), vec.z());
        }

        public static Vector3d ToVector3d(this Vec3f vec)
        {
            return new Vector3d(vec.x(), vec.y(), vec.z());
        }

        #endregion

        #region Points

        public static Vec2d ToVec2d(this Point2d p)
        {
            return new Vec2d(p.X, p.Y);
        }

        public static Vec2f ToVec2f(this Point2d p)
        {
            return new Vec2f((float)p.X, (float)p.Y);
        }

        public static Vec3d ToVec3d(this Point3d p)
        {
            return new Vec3d(p.X, p.Y, p.Z);
        }

        public static Vec3f ToVec3f(this Point3d p)
        {
            return new Vec3f((float)p.X, (float)p.Y, (float)p.Z);
        }

        public static Vec4d ToVec4d(this Point4d p)
        {
            return new Vec4d(p.X, p.Y, p.Z, p.W);
        }

        public static Vec4f ToVec4f(this Point4d p)
        {
            return new Vec4f((float)p.X, (float)p.Y, (float)p.Z, (float)p.W);
        }

        public static Point3d ToPoint3d(this Vec3d vec)
        {
            return new Point3d(vec.x(), vec.y(), vec.z());
        }

        public static Point3d ToPoint3d(this Vec3f vec)
        {
            return new Point3d(vec.x(), vec.y(), vec.z());
        }

        public static Vec3Array ToVec3Array(params Point3d[] puntos)
        {
            return ToVec3Array((IEnumerable<Point3d>)puntos);
        }

        public static Vec3Array ToVec3Array(IEnumerable<Point3d> puntos)
        {
            Vec3Array array = new Vec3Array();
            foreach (Point3d punto in puntos)
            {
                array.push_back(punto.ToVec3f());
            }
            return array;
        }

        public static Vec3Array ToVec3Array(params float[] puntos)
        {
            return ToVec3Array((IEnumerable<float>)puntos);
        }

        public static Vec3Array ToVec3Array(IEnumerable<float> puntos)
        {
            Vec3Array array = new Vec3Array();
            using (IEnumerator<float> enumer = puntos.GetEnumerator())
            {
                while (true)
                {
                    if (!enumer.MoveNext())
                    {
                        return array;
                    }
                    float x = enumer.Current;
                    if (!enumer.MoveNext())
                    {
                        return array;
                    }
                    float y = enumer.Current;
                    if (!enumer.MoveNext())
                    {
                        return array;
                    }
                    float z = enumer.Current;
                    array.push_back(new Vec3f(x, y, z));
                }
            }
        }

        #endregion

        #region Quaternions

        public static Quat ToQuat(this Quaternion quat)
        {
            return new Quat(quat.X, quat.Y, quat.Z, quat.W);
        }

        public static Quaternion ToQuaternion(this Quat quat)
        {
            return new Quaternion(quat.w(), quat.x(), quat.y(), quat.z());
        }

        #endregion

        #region BoundingBoxes

        public static BoundingBox3d ToBoundingBox3d(this BoundingBoxd box)
        {
            if (!box.valid())
            {
                return BoundingBox3d.Empty;
            }
            return new BoundingBox3d(box._min.X, box._max.X, box._min.Y, box._max.Y, box._min.Z, box._max.Z);
        }

        public static BoundingBox3d ToBoundingBox3d(this BoundingBoxf box)
        {
            if (!box.valid())
            {
                return BoundingBox3d.Empty;
            }
            return new BoundingBox3d(box._min.X, box._max.X, box._min.Y, box._max.Y, box._min.Z, box._max.Z);
        }

        public static Box ToBox(this BoundingBox3d cub)
        {
            return new Box(cub.Origin.ToVec3f(), (float)cub.DX, (float)cub.DY, (float)cub.DZ);
        }

        #endregion

        #region Transforms/Matrices

        public static Matrixd ToMatrixd(this ITransform3D transform)
        {
            if (transform is Transform3DIdentity)
            {
                return Matrixd.identity();
            }
            if (transform is Transform3DMatrix)
            {
                return ((Transform3DMatrix)transform).Matrix.ToMatrixd();
            }
            throw new NotSupportedException();
        }

        public static Matrixd ToMatrixd(this ITransform2D transform)
        {
            if (transform is Transform2DIdentity)
            {
                return Matrixd.identity();
            }
            if (transform is Transform2DMatrix)
            {
                return ((Transform2DMatrix)transform).Matrix.ToMatrixd();
            }
            throw new NotSupportedException();
        }

        /// <summary>
        ///     OSG utiliza una representacion 'row-mayor', OpenGL 'column-mayor' y Clip
        ///     'column-mayor'.
        ///     http://steve.hollasch.net/cgindex/math/matrix/column-vec.html
        ///     http://www.openscenegraph.org/projects/osg/wiki/Support/Maths/MatrixTransformations
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public static Matrixd ToMatrixd(this Matrix4x4d mat)
        {
            return new Matrixd(mat.M00, mat.M10, mat.M20, mat.M30,
                               mat.M01, mat.M11, mat.M21, mat.M31,
                               mat.M02, mat.M12, mat.M22, mat.M32,
                               mat.M03, mat.M13, mat.M23, mat.M33);
        }

        public static Matrixd ToMatrixd(this Matrix3x3d mat)
        {
            return new Matrixd(mat.M00, mat.M10, mat.M20, 0,
                               mat.M01, mat.M11, mat.M21, 0,
                               mat.M02, mat.M12, mat.M22, 0,
                               0, 0, 0, 1);
        }

        public static Matrixd ToMatrixd(this Essence.Geometry.Core.Double.Matrix2x3d mat)
        {
            return new Matrixd(mat.M00, mat.M10, 0, 0,
                               mat.M01, mat.M11, 0, 0,
                               mat.M02, mat.M12, 1, 0,
                               0, 0, 0, 1);
        }

        /// <summary>
        ///     OSG utiliza una representacion 'row-mayor', OpenGL 'column-mayor' y Clip
        ///     'column-mayor'.
        ///     http://steve.hollasch.net/cgindex/math/matrix/column-vec.html
        ///     http://www.openscenegraph.org/projects/osg/wiki/Support/Maths/MatrixTransformations
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public static Matrix4x4d ToMatrix4x4d(this Matrixd mat)
        {
            return new Matrix4x4d(mat.get(0, 0), mat.get(1, 0), mat.get(2, 0), mat.get(3, 0),
                                  mat.get(0, 1), mat.get(1, 1), mat.get(2, 1), mat.get(3, 1),
                                  mat.get(0, 2), mat.get(1, 2), mat.get(2, 2), mat.get(3, 2),
                                  mat.get(0, 3), mat.get(1, 3), mat.get(2, 3), mat.get(3, 3));
        }

        public static TexMat ToTexMat(this Transform3D transform)
        {
            return new TexMat(transform.ToMatrixd());
        }

        public static TexMat ToTexMat(this Transform2D transform)
        {
            return new TexMat(transform.ToMatrixd());
        }

        public static TexMat ToTexMat(this Matrix4x4d mat)
        {
            return new TexMat(mat.ToMatrixd());
        }

        public static TexMat ToTexMat(this Matrix3x3d mat)
        {
            return new TexMat(mat.ToMatrixd());
        }

        #endregion

        public static Image ToImage(this Bitmap bitmap)
        {
            int pixelSize;
            switch (bitmap.PixelFormat)
            {
                case PixelFormat.Format32bppArgb:
                    pixelSize = 4;
                    break;
                case PixelFormat.Format24bppRgb:
                    pixelSize = 3;
                    break;
                default:
                    throw new NotImplementedException();
            }

            Image image = new Image();
            image.setAllocationMode(Image.AllocationMode.USE_NEW_DELETE);
            uint pixelFormat = ((pixelSize == 4) ? (uint)OsgModule.GL_RGBA : (uint)OsgModule.GL_RGB);
            image.allocateImage(bitmap.Width,
                                bitmap.Height,
                                1,
                                pixelFormat,
                                (uint)OsgModule.GL_UNSIGNED_BYTE);

            using (ByteBitmap src = ByteBitmap.New(bitmap))
            using (ByteImage dst = ByteImage.New(image))
            {
                for (int y = 0; y < dst.H; y++)
                {
                    int srcpix = y * src.Stride;
                    int dstpix = y * dst.Stride;
                    for (int x = 0; x < dst.W; x++)
                    {
                        for (int c = 0; c < pixelSize; c++)
                        {
                            byte s = src[srcpix++];
                            dst[dstpix++] = s;
                        }
                    }
                }
                dst.Update();
            }
            return image;
        }

        public static Bitmap ToBitmap(this Image image)
        {
            int pixelSize = (int)image.getPixelSizeInBits() / 8;
            PixelFormat format = ((pixelSize == 4) ? PixelFormat.Format32bppArgb : PixelFormat.Format24bppRgb);
            Bitmap bitmap = new Bitmap(image.s(), image.t(), format);

            using (ByteImage src = ByteImage.New(image))
            using (ByteBitmap dst = ByteBitmap.New(bitmap))
            {
                for (int y = 0; y < dst.H; y++)
                {
                    int srcpix = y * src.Stride;
                    int dstpix = y * dst.Stride;
                    for (int x = 0; x < dst.W; x++)
                    {
                        for (int c = 0; c < pixelSize; c++)
                        {
                            byte s = src[srcpix++];
                            dst[dstpix++] = s;
                        }
                    }
                }
                dst.Update();
            }
            return bitmap;
        }

        #region enums

        public static Texture.WrapMode ToWrapMode(this TextureWrapMode mode)
        {
            switch (mode)
            {
                case TextureWrapMode.Clamp:
                    return Texture.WrapMode.CLAMP;
                case TextureWrapMode.ClampToBorder:
                    return Texture.WrapMode.CLAMP_TO_BORDER;
                case TextureWrapMode.ClampToEdge:
                    return Texture.WrapMode.CLAMP_TO_EDGE;
                case TextureWrapMode.Repeat:
                    return Texture.WrapMode.REPEAT;
                case TextureWrapMode.MirroredRepeat:
                    return Texture.WrapMode.MIRROR;
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        public static Texture.FilterMode ToFilterMode(this TextureMinFilter filter)
        {
            switch (filter)
            {
                case TextureMinFilter.Linear:
                    return Texture.FilterMode.LINEAR_MIPMAP_LINEAR;
                case TextureMinFilter.Nearest:
                    return Texture.FilterMode.NEAREST_MIPMAP_NEAREST;
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        public static Texture.FilterMode ToFilterMode(this TextureMagFilter filter)
        {
            switch (filter)
            {
                case TextureMagFilter.Linear:
                    return Texture.FilterMode.LINEAR_MIPMAP_LINEAR;
                case TextureMagFilter.Nearest:
                    return Texture.FilterMode.NEAREST_MIPMAP_NEAREST;
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        public static TextureMode ToTextureMode(TexEnv.Mode mode)
        {
            switch (mode)
            {
                case TexEnv.Mode.ADD:
                    return TextureMode.ADD;
                    break;
                case TexEnv.Mode.BLEND:
                    return TextureMode.BLEND;
                    break;
                case TexEnv.Mode.REPLACE:
                    return TextureMode.REPLACE;
                    break;
                case TexEnv.Mode.MODULATE:
                default:
                    return TextureMode.MODULATE;
                    break;
                case TexEnv.Mode.DECAL:
                    return TextureMode.DECAL;
                    break;
            }
        }

        #endregion

        public static Node ReadEarthNodeFile(string xmlNode)
        {
            string fileName = string.Format("{0}{1}.{2}", Path.GetTempPath(), Guid.NewGuid(), "earth");

            using (Stream stream = File.OpenWrite(fileName))
            using (TextWriter writer = new StreamWriter(stream))
            {
                writer.Write(xmlNode);
            }

            Node node = OsgDBModule.readNodeFile(fileName);

            File.Delete(fileName);

            return node;
        }

        #region shaders

        // load source from a file.
        public static bool LoadShaderSource(Shader shader, string fileName)
        {
            string fqFileName = OsgDBModule.findDataFile(fileName);

            /*if (string.IsNullOrEmpty(fqFileName))
            {
                fqFileName = Path.Combine(AppContext.Instance.StartupPath,
                                          "Resources",
                                          "Shaders",
                                          fileName);
                if (!File.Exists(fqFileName))
                {
                    Log<LogHelper>.Error("File \"" + fileName + "\" not found.");
                    return false;
                }
            }*/

            try
            {
                string source = File.ReadAllText(fqFileName);
                shader.setShaderSource(source);
            }
            catch (Exception)
            {
                Log<LogHelper>.Error("Couldn't load file: " + fqFileName);
                return false;
            }

            /*bool success = shader.loadShaderSourceFromFile(fqFileName);
            if (!success)
            {
                Log<OSGManager>.Error("Couldn't load file: " + fileName);
                return false;
            }*/

            return true;
        }

        public static Shader LoadShaderSource(Shader.Type type, string fileName)
        {
            Shader shader = new Shader(type);
            shader.setName("LoadShaderSource_" + Path.GetFileName(fileName));
            LoadShaderSource(shader, fileName);
            return shader;
        }

        #endregion

        public static void SetDebug()
        {
#if DEBUG
            OsgModule.setNotifyHandler(MyNotifyHandlerDebug.Instance);
            OsgModule.setNotifyLevel(NotifySeverity.WARN); // WARN DEBUG_FP DEBUG_INFO

            // http://trac.openscenegraph.org/projects/osg//wiki/Support/TipsAndTricks
            Environment.SetEnvironmentVariable("OSG_NOTIFY_LEVEL", "WARN"); // "WARN" "DEBUG" "INFO"

            //State.setCheckForGLErrors(State.CheckForGLErrors.ONCE_PER_ATTRIBUTE);
            Environment.SetEnvironmentVariable("OSG_GL_ERROR_CHECKING", "ON");

            Environment.SetEnvironmentVariable("OSG_OPTIMIZER", "OFF");
            Environment.SetEnvironmentVariable("CHECK_GEOMETRY", "ON");

            // https://github.com/gwaldron/osgearth/blob/master/docs/source/references/envvars.rst
            Environment.SetEnvironmentVariable("OSGEARTH_NOTIFY_LEVEL", "WARN"); // DEBUG, INFO, NOTICE, WARN

            //Environment.SetEnvironmentVariable("OSGEARTH_MP_PROFILE", "1"); // 1, 2
            Environment.SetEnvironmentVariable("OSGEARTH_MP_DEBUG", "ON");
#else
//OsgModule.setNotifyHandler(MyNotifyHandlerDebug.Instance);
            OsgModule.setNotifyLevel(NotifySeverity.WARN); // WARN DEBUG_FP

            // http://trac.openscenegraph.org/projects/osg//wiki/Support/TipsAndTricks
            Environment.SetEnvironmentVariable("OSG_NOTIFY_LEVEL", "WARN"); // "WARN" "DEBUG" "INFO"

            // https://github.com/gwaldron/osgearth/blob/master/docs/source/references/envvars.rst
            Environment.SetEnvironmentVariable("OSGEARTH_NOTIFY_LEVEL", "WARN"); // DEBUG, INFO, NOTICE, WARN
#endif
        }

        #region Inner classes

        private class FindVisitor : NodeVisitor
        {
            public FindVisitor(Func<Node, bool> test)
            {
                this.test = test;
            }

            private readonly Func<Node, bool> test;

            public Node Found
            {
                get { return this.found; }
            }

            private Node found;

            public override void apply(Node node)
            {
                if (this.found != null)
                {
                    return;
                }

                if (this.test(node))
                {
                    this.found = node;
                    return;
                }

                base.apply(node);
            }

            public override void apply(Group node)
            {
                if (this.found != null)
                {
                    return;
                }

                base.apply(node);
            }
        }

        private class MyNotifyHandler : NotifyHandler
        {
            public override void notify(NotifySeverity severity, string message)
            {
                switch (severity)
                {
                    case NotifySeverity.FATAL:
                        Log<LogHelper>.Fatal(message);
                        return;
                    case NotifySeverity.WARN:
                        Log<LogHelper>.Warn(message);
                        return;
                    case NotifySeverity.INFO:
                        Log<LogHelper>.Info(message);
                        return;
                    case NotifySeverity.NOTICE:
                        Log<LogHelper>.Info(message);
                        return;
                    case NotifySeverity.DEBUG_FP:
                        Log<LogHelper>.Debug(message);
                        return;
                    case NotifySeverity.DEBUG_INFO:
                        Log<LogHelper>.Debug(message);
                        return;

                    case NotifySeverity.ALWAYS:
                        Log<LogHelper>.Info(message);
                        return;
                }
            }

            //public static readonly NotifyHandler Instance = new MyNotifyHandler();
        }

        private class MyNotifyHandlerDebug : NotifyHandler
        {
            public override void notify(NotifySeverity severity, string message)
            {
                switch (severity)
                {
                    case NotifySeverity.FATAL:
                        Debug.WriteLine("FATAL: " + message);
                        return;
                    case NotifySeverity.WARN:
                        Debug.WriteLine("WARN: " + message);
                        return;
                    case NotifySeverity.INFO:
                        Debug.WriteLine("INFO: " + message);
                        return;
                    case NotifySeverity.NOTICE:
                        Debug.WriteLine("NOTICE: " + message);
                        return;
                    case NotifySeverity.DEBUG_FP:
                        Debug.WriteLine("DEBUG_FP: " + message);
                        return;
                    case NotifySeverity.DEBUG_INFO:
                        Debug.WriteLine("DEBUG_INFO: " + message);
                        return;

                    case NotifySeverity.ALWAYS:
                        Debug.WriteLine("ALWAYS: " + message);
                        return;
                }
            }

            public static readonly MyNotifyHandlerDebug Instance = new MyNotifyHandlerDebug();
        }

        [UsedImplicitly]
        private class LogHelper
        {
        }

        #endregion
    }
}