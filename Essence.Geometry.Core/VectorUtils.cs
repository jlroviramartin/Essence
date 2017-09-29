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
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Essence.Geometry.Core.Byte;
using Essence.Geometry.Core.Double;
using Essence.Geometry.Core.Float;
using Essence.Geometry.Core.Int;
using Essence.Util;
using Essence.Util.Logs;

namespace Essence.Geometry.Core
{
    public delegate bool TryParse<T>(string s, NumberStyles style, IFormatProvider proveedor, out T result);

    /// <summary>
    ///     Utilidades sobre vectores.
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
        ///     Muestra el array como una cadena de texto.
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
        ///     Intenta parsear la cadena de texto segun los estilos indicados y devuelve un array de valores.
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

        #region IVector2

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

        #region IPoint2

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

        public static object Convert(Type sourceType, Type destinationType, object source)
        {
            if (destinationType.IsAssignableFrom(sourceType))
            {
                return source;
            }
            return EnsureConvert(sourceType, destinationType, source);
        }

        public static void Register(Type sourceType, Type destinationtype, Func<object, object> func)
        {
            converters.Add(new Converter(sourceType, destinationtype, func));
        }

        public static void Register(Type[] sourceTypes, Type[] destinationtypes, Func<object, object> func)
        {
            converters.Add(new Converter(sourceTypes, destinationtypes, func));
        }

        public static void Register<TSource1, TSource2, TDestination>(Func<TSource1, TDestination> func)
        {
            Register(new[] { typeof(TSource1), typeof(TSource2) },
                     new[] { typeof(TDestination) },
                     x => func((TSource1)x));
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

                foreach (ConvertHelperAttribute attr in type.GetCustomAttributes<ConvertHelperAttribute>())
                {
                    if (attr.SourceType2 != null)
                    {
                        Register(new[] { attr.SourceType1, attr.SourceType2 },
                                 new[] { attr.DestinationType },
                                 o => constructorInfo.Invoke(new[] { o }));
                    }
                    else
                    {
                        Register(attr.SourceType1,
                                 attr.DestinationType,
                                 o => constructorInfo.Invoke(new[] { o }));
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

        public static TD Convert<TS, TD>(TS source)
        {
            if (source is TD)
            {
                return (TD)(object)source;
            }
            return (TD)EnsureConvert(typeof(TS), typeof(TD), source);
        }

        public static TD Convert<TD>(object source)
        {
            if (source is TD)
            {
                return (TD)source;
            }
            return (TD)EnsureConvert(source.GetType(), typeof(TD), source);
        }

        #region private

        static VectorUtils()
        {
            RegisterReflectionRecursively(typeof(FloatConvertibles));
            RegisterReflectionRecursively(typeof(DoubleConvertibles));
            RegisterReflectionRecursively(typeof(IntConvertibles));
            RegisterReflectionRecursively(typeof(ByteConvertibles));
        }

        private static object EnsureConvert(Type sourceType, Type destinationType, object source)
        {
            Converter c;
            if (!convertersCache.TryGetValue(Tuple.Create(sourceType, destinationType), out c))
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
                convertersCache.Add(Tuple.Create(sourceType, destinationType), c);
            }
            return c.Func(source);
        }

        private static readonly Dictionary<Tuple<Type, Type>, Converter> convertersCache = new Dictionary<Tuple<Type, Type>, Converter>();
        private static readonly List<Converter> converters = new List<Converter>();

        private struct Converter
        {
            public Converter(Func<Type, bool> predicateSourceType,
                             Func<Type, bool> predicateDestinationType,
                             Func<object, object> func)
            {
                this.PredicateSourceType = predicateSourceType;
                this.PredicateDestinationType = predicateDestinationType;
                this.Func = func;
            }

            public Converter(Type[] sourceTypes, Type[] destinationTypes, Func<object, object> func)
            {
                this.PredicateSourceType = ts => sourceTypes.All(x => x.IsAssignableFrom(ts));
                this.PredicateDestinationType = td => destinationTypes.All(x => td.IsAssignableFrom(x));
                this.Func = func;
            }

            public Converter(Type sourceType, Type destinationType, Func<object, object> func)
            {
                this.PredicateSourceType = ts => sourceType.IsAssignableFrom(ts);
                this.PredicateDestinationType = td => td.IsAssignableFrom(destinationType);
                this.Func = func;
            }

            public bool IsValidFor(Type sourceType, Type destinationType)
            {
                return (this.PredicateSourceType(sourceType) && this.PredicateDestinationType(destinationType));
            }

            public readonly Func<Type, bool> PredicateSourceType;
            public readonly Func<Type, bool> PredicateDestinationType;
            public readonly Func<object, object> Func;
        }

        private class LogHelper
        {
        }

        #endregion
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ConvertHelperAttribute : Attribute
    {
        public Type SourceType1 { get; set; }
        public Type SourceType2 { get; set; }
        public Type DestinationType { get; set; }
    }
}