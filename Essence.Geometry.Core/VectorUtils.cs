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
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Essence.Geometry.Core.Double;
using Essence.Geometry.Core.Int;
using Essence.Util;

namespace Essence.Geometry.Core
{
    public delegate bool TryParse<T>(string s, NumberStyles style, IFormatProvider proveedor, out T result);

    /// <summary>
    /// Vector utilities.
    /// </summary>
    public static class VectorUtils
    {
        public static int GetHashCode<T>(T x, T y)
            where T : struct
        {
            // http://www.jarvana.com/jarvana/view/org/apache/lucene/lucene-spatial/2.9.3/lucene-spatial-2.9.3-sources.jar!/org/apache/lucene/spatial/geometry/shape/Vector2D.java
            const int prime = 31;
            int hash = 1;
            unchecked
            {
                hash = prime * hash + x.GetHashCode();
                hash = prime * hash + y.GetHashCode();
            }
            return hash;
        }

        public static int GetHashCode<T>(T x, T y, T z)
            where T : struct
        {
            // http://www.jarvana.com/jarvana/view/org/apache/lucene/lucene-spatial/2.9.3/lucene-spatial-2.9.3-sources.jar!/org/apache/lucene/spatial/geometry/shape/Vector2D.java
            const int prime = 31;
            int hash = 1;
            unchecked
            {
                hash = prime * hash + x.GetHashCode();
                hash = prime * hash + y.GetHashCode();
                hash = prime * hash + z.GetHashCode();
            }
            return hash;
        }

        public static int GetHashCode<T>(T x, T y, T z, T w)
            where T : struct
        {
            // http://www.jarvana.com/jarvana/view/org/apache/lucene/lucene-spatial/2.9.3/lucene-spatial-2.9.3-sources.jar!/org/apache/lucene/spatial/geometry/shape/Vector2D.java
            const int prime = 31;
            int hash = 1;
            unchecked
            {
                hash = prime * hash + x.GetHashCode();
                hash = prime * hash + y.GetHashCode();
                hash = prime * hash + z.GetHashCode();
                hash = prime * hash + w.GetHashCode();
            }
            return hash;
        }

        /// <summary>
        /// Converts the array to a string.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="provider">Provider.</param>
        /// <param name="format">Format.</param>
        /// <param name="vs">Array.</param>
        /// <returns>String.</returns>
        public static string ToString<T>(IFormatProvider provider,
                                         string format,
                                         T[] vs)
            where T : IFormattable
        {
            // Se obtiene la configuracion.
            VectorFormatInfo vfi = null;
            if (provider != null)
            {
                vfi = provider.GetFormat<VectorFormatInfo>();
            }
            if (vfi == null)
            {
                vfi = VectorFormatInfo.CurrentInfo;
            }

            StringBuilder buff = new StringBuilder();
            if (vfi.HasBegEnd)
            {
                buff.Append(vfi.Beg);
            }
            for (int i = 0; i < vs.Length; i++)
            {
                if (i > 0)
                {
                    if (vfi.HasSep)
                    {
                        buff.Append(vfi.Sep);
                    }
                    buff.Append(" ");
                }
                buff.Append(vs[i].ToString(format, provider));
            }
            if (vfi.HasBegEnd)
            {
                buff.Append(vfi.End);
            }
            return buff.ToString();
        }

        /// <summary>
        /// Tries to parse the string using <code>vstyle</code> and <code>style</code> styles
        /// and returns an array.
        /// </summary>
        /// <param name="provider">Format providor.</param>
        /// <param name="s">String.</param>
        /// <param name="count">Number of items. If it is -1 the it reads all items.</param>
        /// <param name="vstyle">Vector style.</param>
        /// <param name="nstyle">Number style.</param>
        /// <param name="tryParse">Parser function.</param>
        /// <param name="result">Array.</param>
        /// <returns><code>True</code> if everything is correct, <code>false</code> otherwise.</returns>
        public static bool TryParse<T>(string s, int count,
                                       out T[] result,
                                       TryParse<T> tryParse,
                                       IFormatProvider provider = null,
                                       VectorStyles vstyle = VectorStyles.All,
                                       NumberStyles nstyle = NumberStyles.Float | NumberStyles.AllowThousands)
        {
            try
            {
                // Se obtiene la configuracion.
                VectorFormatInfo vfi = null;
                if (provider != null)
                {
                    vfi = provider.GetFormat<VectorFormatInfo>();
                }
                if (vfi == null)
                {
                    vfi = VectorFormatInfo.CurrentInfo;
                }

                bool usarPrincipioYFin = ((vstyle ^ VectorStyles.BegEnd) != 0) && vfi.HasBegEnd;
                bool usarSeparador = ((vstyle ^ VectorStyles.Sep) != 0) && vfi.HasSep;

                // Se parsea la cadena utilizando expresiones regulares.
                const RegexOptions opciones = RegexOptions.ExplicitCapture
                                              | RegexOptions.IgnoreCase
                                              | RegexOptions.IgnorePatternWhitespace
                                              | RegexOptions.Multiline
                                              | RegexOptions.Compiled;

                string s2;
                if (usarPrincipioYFin)
                {
                    // Se busca el inicio y fin.
                    string patron1 = string.Format(provider,
                                                   @"^\s*(?:{0}\s*)?(?<interior>[^{0}{1}]*)\s*(?:{1}\s*)?$",
                                                   Regex.Escape(vfi.Beg),
                                                   Regex.Escape(vfi.End));
                    Regex rx1 = new Regex(patron1, opciones);
                    Match m1 = rx1.Match(s);
                    if (!m1.Success)
                    {
                        result = null;
                        return false;
                    }
                    s2 = m1.Groups["interior"].Value;
                }
                else
                {
                    // Se busca el inicio y fin.
                    string patron1 = string.Format(provider, @"^\s*(?<interior>.*)\s*$");
                    Regex rx1 = new Regex(patron1, opciones);
                    Match m1 = rx1.Match(s);
                    if (!m1.Success)
                    {
                        result = null;
                        return false;
                    }
                    s2 = m1.Groups["interior"].Value;
                }

                string[] ss;
                if (usarSeparador)
                {
                    // Se buscan los separadores.
                    string patron2 = string.Format(provider, @"\s*{0}\s*",
                                                   Regex.Escape(vfi.Sep));
                    Regex rx2 = new Regex(patron2, opciones);
                    ss = rx2.Split(s2);
                    if (ss.Length != 2)
                    {
                        result = null;
                        return false;
                    }
                }
                else
                {
                    // Se buscan los separadores.
                    string patron2 = string.Format(provider, @"\s+");
                    Regex rx2 = new Regex(patron2, opciones);
                    ss = rx2.Split(s2);
                    if (ss.Length != 2)
                    {
                        result = null;
                        return false;
                    }
                }

                if (count > 0 && ss.Length != count)
                {
                    result = null;
                    return false;
                }

                T[] ret = new T[ss.Length];
                for (int i = 0; i < ss.Length; i++)
                {
                    if (!tryParse(ss[i], nstyle, provider, out ret[i]))
                    {
                        result = null;
                        return false;
                    }
                }
                result = ret;
                return true;
            }
            catch (Exception)
            {
                result = null;
                return false;
            }
        }

        #region Vector2d

        public static Vector2i Round(this Vector2d p)
        {
            return new Vector2i((int)Math.Round(p.X), (int)Math.Round(p.Y));
        }

        public static Vector2i Ceiling(this Vector2d p)
        {
            return new Vector2i((int)Math.Ceiling(p.X), (int)Math.Ceiling(p.Y));
        }

        public static Vector2i Floor(this Vector2d p)
        {
            return new Vector2i((int)Math.Floor(p.X), (int)Math.Floor(p.Y));
        }

        #endregion

        #region IVector

        public static Vector2d ToVector2d(this IVector p)
        {
            if (p is Vector2d)
            {
                return (Vector2d)p;
            }
            return new Vector2d(p);
        }

        public static Vector2i ToVector2i(this IVector p)
        {
            if (p is Vector2i)
            {
                return (Vector2i)p;
            }
            return new Vector2i(p);
        }

        public static Vector3d ToVector3d(this IVector p)
        {
            if (p is Vector3d)
            {
                return (Vector3d)p;
            }
            return new Vector3d(p);
        }

        public static Vector4d ToVector4d(this IVector p)
        {
            if (p is Vector4d)
            {
                return (Vector4d)p;
            }
            return new Vector4d(p);
        }

        #endregion

        #region IVector2D

        public static Vector2d ToVector2d(this IVector2D p)
        {
            if (p is Vector2d)
            {
                return (Vector2d)p;
            }
            return new Vector2d(p);
        }

        public static Vector3d ToVector3d(this IVector3D p)
        {
            if (p is Vector3d)
            {
                return (Vector3d)p;
            }
            return new Vector3d(p);
        }

        public static Vector4d ToVector4d(this IVector4D p)
        {
            if (p is Vector4d)
            {
                return (Vector4d)p;
            }
            return new Vector4d(p);
        }

        #endregion
    }
}