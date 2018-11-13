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
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Essence.Geometry.Core.Double;
using Essence.Geometry.Core.Integer;
using Essence.Util;
using Essence.Util.Logs;

namespace Essence.Geometry.Core
{
    public delegate bool TryParse<T>(string s, NumberStyles style, IFormatProvider proveedor, out T result);

    /// <summary>
    /// Utilidades sobre vectores.
    /// </summary>
    public static class VectorUtils
    {
        public static int GetHashCode<T>(T a, T b)
            where T : struct
        {
            // http://www.jarvana.com/jarvana/view/org/apache/lucene/lucene-spatial/2.9.3/lucene-spatial-2.9.3-sources.jar!/org/apache/lucene/spatial/geometry/shape/Vector2D.java
            const int prime = 31;
            int hash = 1;
            unchecked
            {
                hash = prime * hash + a.GetHashCode();
                hash = prime * hash + b.GetHashCode();
            }
            return hash;
        }

        public static int GetHashCode<T>(T a, T b, T c)
            where T : struct
        {
            // http://www.jarvana.com/jarvana/view/org/apache/lucene/lucene-spatial/2.9.3/lucene-spatial-2.9.3-sources.jar!/org/apache/lucene/spatial/geometry/shape/Vector2D.java
            const int prime = 31;
            int hash = 1;
            unchecked
            {
                hash = prime * hash + a.GetHashCode();
                hash = prime * hash + b.GetHashCode();
                hash = prime * hash + c.GetHashCode();
            }
            return hash;
        }

        public static int GetHashCode<T>(T a, T b, T c, T d)
            where T : struct
        {
            // http://www.jarvana.com/jarvana/view/org/apache/lucene/lucene-spatial/2.9.3/lucene-spatial-2.9.3-sources.jar!/org/apache/lucene/spatial/geometry/shape/Vector2D.java
            const int prime = 31;
            int hash = 1;
            unchecked
            {
                hash = prime * hash + a.GetHashCode();
                hash = prime * hash + b.GetHashCode();
                hash = prime * hash + c.GetHashCode();
                hash = prime * hash + d.GetHashCode();
            }
            return hash;
        }

        /// <summary>
        /// Muestra el array como una cadena de texto.
        /// </summary>
        /// <typeparam name="T">Tipo.</typeparam>
        /// <param name="provider">Proveedor.</param>
        /// <param name="format">Formato.</param>
        /// <param name="vs">Array.</param>
        /// <returns>Cadena de texto.</returns>
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
        /// Intenta parsear la cadena de texto segun los estilos indicados y devuelve un array de valores.
        /// </summary>
        /// <param name="provider">Proveedor de formato.</param>
        /// <param name="s">Cadena de texto a parsear.</param>
        /// <param name="count">Numero de elementos tienen que leer. Si es -1, se leen todos.</param>
        /// <param name="vstyle">Estilo de vectores.</param>
        /// <param name="style">Estilo de numeros.</param>
        /// <param name="tryParse">Funcion de parseo.</param>
        /// <param name="result">Array de flotantes.</param>
        /// <returns>Indica si lo ha parseado correctamente.</returns>
        public static bool TryParse<T>(string s, int count,
                                       out T[] result,
                                       TryParse<T> tryParse,
                                       IFormatProvider provider = null,
                                       VectorStyles vstyle = VectorStyles.All,
                                       NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands)
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
                    if (!tryParse(ss[i], style, provider, out ret[i]))
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

        #region Point2d

        public static Point2i Round(this Point2d p)
        {
            return new Point2i((int)Math.Round(p.X), (int)Math.Round(p.Y));
        }

        public static Point2i Ceiling(this Point2d p)
        {
            return new Point2i((int)Math.Ceiling(p.X), (int)Math.Ceiling(p.Y));
        }

        public static Point2i Floor(this Point2d p)
        {
            return new Point2i((int)Math.Floor(p.X), (int)Math.Floor(p.Y));
        }

        #endregion

        #region IVector2/3/4

        public static Vector2d ToVector2d(this IVector2 p)
        {
            if (p is Vector2d)
            {
                return (Vector2d)p;
            }
            return new Vector2d(p);
        }

        public static Vector2i ToVector2i(this IVector2 p)
        {
            if (p is Vector2i)
            {
                return (Vector2i)p;
            }
            return new Vector2i(p);
        }

        public static Vector3d ToVector3d(this IVector3 p)
        {
            if (p is Vector3d)
            {
                return (Vector3d)p;
            }
            return new Vector3d(p);
        }

        public static Vector4d ToVector4d(this IVector4 p)
        {
            if (p is Vector4d)
            {
                return (Vector4d)p;
            }
            return new Vector4d(p);
        }

        #endregion

        #region IPoint2/3/4

        public static Point2d ToPoint2d(this IPoint2 p)
        {
            if (p is Point2d)
            {
                return (Point2d)p;
            }
            return new Point2d(p);
        }

        public static Point2i ToPoint2i(this IPoint2 p)
        {
            if (p is Point2i)
            {
                return (Point2i)p;
            }
            return new Point2i(p);
        }

        public static Point3d ToPoint3d(this IPoint3 p)
        {
            if (p is Point3d)
            {
                return (Point3d)p;
            }
            return new Point3d(p);
        }

        public static Point4d ToPoint4d(this IPoint4 p)
        {
            if (p is Point4d)
            {
                return (Point4d)p;
            }
            return new Point4d(p);
        }

        #endregion

        #region Converters

        public static void Register(Type sourceType, Type destinationtype, Func<object, object> func, string funcDescription, float weight)
        {
            Register(new Converter(sourceType, destinationtype, func, funcDescription, weight));
        }

        public static void Register(Type[] sourceTypes, Type[] destinationtypes, Func<object, object> func, string funcDescription, float weight)
        {
            Register(new Converter(sourceTypes, destinationtypes, func, funcDescription, weight));
        }

        public static void Register<TSource1, TSource2, TDestination>(Func<TSource1, TDestination> func, string funcDescription, float weight)
        {
            Register(new[] { typeof(TSource1), typeof(TSource2) },
                     new[] { typeof(TDestination) },
                     x => func((TSource1)x),
                     funcDescription,
                     weight);
        }

        public static void RegisterReflectionRecursively(Type type)
        {
            try
            {
                foreach (Type nestedType in type.GetNestedTypes())
                {
                    RegisterReflection(nestedType);
                }
            }
            catch (Exception exception)
            {
                Log<LogHelper>.Write(LogType.Info, exception);
            }
        }

        public static void RegisterReflection(Type type)
        {
            try
            {
                const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
                ConstructorInfo constructorInfo = type.GetConstructors(flags).FirstOrDefault(c => c.GetParameters().Length == 1);
                if (constructorInfo == null)
                {
                    return;
                }

                //Type sourceType = constructorInfo.GetParameters()[0].ParameterType;
                //HashSet<Type> destinationTypes = new HashSet<Type>(type.GetInterfaces());

                foreach (ConvertHelperAttribute attr in type.GetCustomAttributes<ConvertHelperAttribute>(false))
                {
                    if (attr.SourceType2 != null)
                    {
                        Register(new[] { attr.SourceType1, attr.SourceType2 },
                                 new[] { attr.DestinationType },
                                 o => constructorInfo.Invoke(new[] { o }),
                                 " [" + type.Name + "]",
                                 attr.Weight);
                    }
                    else
                    {
                        Register(attr.SourceType1,
                                 attr.DestinationType,
                                 o => constructorInfo.Invoke(new[] { o }),
                                 " [" + type.Name + "]",
                                 attr.Weight);
                    }
                }

                /*foreach (ConstructorInfo constructorInfo in type.GetConstructors(BindingFlags.Public).Where(c => c.GetParameters().Length == 0))
                {
                    Type sourceType = constructorInfo.GetParameters()[0].ParameterType;
                }*/
            }
            catch (Exception exception)
            {
                Log<LogHelper>.Write(LogType.Info, exception);
            }
        }

        public static object Convert(Type sourceType, Type destinationType, object source)
        {
            if (destinationType.IsAssignableFrom(sourceType))
            {
                return source;
            }
            return SlowConvert(sourceType, destinationType, source);
        }

        public static TD Convert<TS, TD>(TS source)
        {
            if (source is TD)
            {
                return (TD)(object)source;
            }
            return (TD)SlowConvert(typeof(TS), typeof(TD), source);
        }

        public static TD Convert<TD>(object source)
        {
            if (source is TD)
            {
                return (TD)source;
            }
            return (TD)SlowConvert(source.GetType(), typeof(TD), source);
        }

        #endregion

        #region private

        static VectorUtils()
        {
            Converters.Register();
        }

        private static void Register(Converter converter)
        {
            int index = converters.BinarySearch(converter, ConverterComparer.INSTANCE);
            if (index < 0)
            {
                index = ~index;
            }
            converters.Insert(index, converter);
        }

        private static object SlowConvert(Type sourceType, Type destinationType, object source)
        {
            Dictionary<Type, Converter> dc;
            if (!convertersCache.TryGetValue(sourceType, out dc))
            {
                dc = new Dictionary<Type, Converter>();
                convertersCache.Add(sourceType, dc);
            }
            Converter c;
            if (!dc.TryGetValue(destinationType, out c))
            {
                bool found = false;
                foreach (Converter caux in converters)
                {
                    if (caux.IsValidFor(sourceType, destinationType))
                    {
                        c = caux;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    throw new InvalidCastException();
                }
                dc.Add(destinationType, c);
            }
            return c.Func(source);
        }

        private static readonly Dictionary<Type, Dictionary<Type, Converter>> convertersCache = new Dictionary<Type, Dictionary<Type, Converter>>();

        //private static readonly Dictionary<Tuple<Type, Type>, Converter> convertersCache = new Dictionary<Tuple<Type, Type>, Converter>();
        private static readonly List<Converter> converters = new List<Converter>();

        #endregion

        #region Inner classes

        private struct Converter : IComparable<Converter>
        {
            public Converter(Func<Type, bool> predicateSourceType,
                             Func<Type, bool> predicateDestinationType,
                             Func<object, object> func,
                             float weight = 0)
            {
                this.PredicateSourceType = predicateSourceType;
                this.PredicateDestinationType = predicateDestinationType;
                this.Func = func;
                this.weight = weight;

                StringBuilder buff = new StringBuilder();
                buff.AppendFormat("[ {0:F3} ] ..", this.weight);

                this.str = buff.ToString();
            }

            public Converter(Type[] sourceTypes, Type[] destinationTypes,
                             Func<object, object> func,
                             string funcdescription,
                             float weight = 0)
            {
                this.PredicateSourceType = ts =>
                {
                    return sourceTypes.All(x => x.IsAssignableFrom(ts));
                };

                this.PredicateDestinationType = td =>
                {
                    return destinationTypes.Any(x => td.IsAssignableFrom(x));
                };
                this.Func = func;

                this.weight = weight;

                StringBuilder buff = new StringBuilder();
                buff.AppendFormat("[ {0:F3} ]", this.weight);
                buff.Append("[ ");
                for (int i = 0; i < sourceTypes.Length; i++)
                {
                    if (i > 0)
                    {
                        buff.Append(", ");
                    }
                    buff.Append(sourceTypes[i].Name);
                }
                buff.Append(" ]");

                buff.Append(" -> ");

                buff.Append("[ ");
                for (int i = 0; i < destinationTypes.Length; i++)
                {
                    if (i > 0)
                    {
                        buff.Append(", ");
                    }
                    buff.Append(destinationTypes[i].Name);
                }
                buff.Append(" ] ");
                buff.Append(funcdescription);

                this.str = buff.ToString();
            }

            public Converter(Type sourceType, Type destinationType,
                             Func<object, object> func,
                             string funcdescription,
                             float weight = 0)
            {
                this.PredicateSourceType = ts =>
                {
                    return sourceType.IsAssignableFrom(ts);
                };
                this.PredicateDestinationType = td =>
                {
                    return td.IsAssignableFrom(destinationType);
                };
                this.Func = func;

                this.weight = weight;

                StringBuilder buff = new StringBuilder();
                buff.AppendFormat("[ {0:F3} ]", this.weight);

                buff.Append("[ ");
                buff.Append(sourceType.Name);
                buff.Append(" ]");

                buff.Append(" -> ");

                buff.Append("[ ");
                buff.Append(destinationType.Name);
                buff.Append(" ] ");
                buff.Append(funcdescription);

                this.str = buff.ToString();
            }

            public bool IsValidFor(Type sourceType, Type destinationType)
            {
                return (this.PredicateSourceType(sourceType) && this.PredicateDestinationType(destinationType));
            }

            public readonly Func<Type, bool> PredicateSourceType;
            public readonly Func<Type, bool> PredicateDestinationType;
            public readonly Func<object, object> Func;
            private readonly string str;
            private readonly float weight;

            public int CompareTo(Converter other)
            {
                return this.weight.CompareTo(other.weight);
            }

            public override string ToString()
            {
                return this.str;
            }
        }

        private class ConverterComparer : IComparer<Converter>
        {
            public static readonly ConverterComparer INSTANCE = new ConverterComparer();

            public int Compare(Converter x, Converter y)
            {
                return x.CompareTo(y);
            }
        }

        private class LogHelper
        {
        }

        #endregion
    }
}