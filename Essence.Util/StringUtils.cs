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
using System.Text;
using Essence.Util.Properties;

namespace Essence.Util
{
    /// <summary>
    ///     Utilidades sobre cadenas de texto.
    /// </summary>
    public static class StringUtils
    {
        #region StringBuilder

        [StringFormatMethod("format")]
        public static StringBuilder AppendFormat(StringBuilder builder, string format, params object[] args)
        {
            return builder.Append(string.Format(format, args));
        }

        [StringFormatMethod("format")]
        public static StringBuilder AppendFormatLine(StringBuilder builder, string format, params object[] args)
        {
            return builder.AppendLine(string.Format(format, args));
        }

        public static StringBuilder AppendIndent(this StringBuilder builder, int index, string s = "  ")
        {
            for (int i = 0; i < index; i++)
            {
                builder.Append(s);
            }
            return builder;
        }

        #endregion

        /// <summary>
        ///     Indica si dos cadenas son iguales independientemente de las mayusculas.
        /// </summary>
        public static bool EqualsIgnoreCase(this string s1, string s2)
        {
            if (s1 == null)
            {
                return (s2 == null);
            }
            if (s2 == null)
            {
                return false;
            }
            return s1.ToUpper().Equals(s2.ToUpper());
        }

        /// <summary>
        ///     Añade el codigo <c>"</c> a cada aparicion del caracter <c>"</c> en
        ///     la cadena <c>str</c>. Compatible con <c>Parse</c>.
        /// </summary>
        /// <param name="str">Cadena de texto.</param>
        /// <returns>Cadena resultado.</returns>
        public static string Scape(this string str)
        {
            return Scape(str, '"', new char[] { '"' });
        }

        /// <summary>
        ///     Añade el codigo <c>scape</c> a cada aparicion de los caracteres <c>toScape</c> en
        ///     la cadena <c>str</c>.
        /// </summary>
        /// <param name="str">Cadena de texto.</param>
        /// <param name="scape">Caracter de escape.</param>
        /// <param name="toScape">Caracteres a escapar.</param>
        /// <returns>Cadena resultado.</returns>
        public static string Scape(this string str, char scape, char[] toScape)
        {
            HashSet<char> aux = new HashSet<char>(toScape);
            StringBuilder buff = new StringBuilder();
            foreach (char ch in str)
            {
                if (!aux.Contains(ch))
                {
                    buff.Append(ch);
                }
                else
                {
                    buff.Append(scape).Append(ch);
                }
            }
            return buff.ToString();
        }

        /// <summary>
        ///     Elimina el codigo <c>"</c> de cada aparicion del caracter <c>"</c> de
        ///     la cadena <c>str</c>. Compatible con <c>Parse</c>.
        /// </summary>
        /// <param name="str">Cadena de texto.</param>
        /// <returns>Cadena resultado.</returns>
        public static string Unscape(this string str)
        {
            return Unscape(str, '"', new char[] { '"' });
        }

        /// <summary>
        ///     Elimina el codigo <c>scape</c> de cada aparicion de los caracteres <c>toScape</c> de
        ///     la cadena <c>str</c>.
        /// </summary>
        /// <param name="str">Cadena de texto.</param>
        /// <param name="scape">Caracter de escape.</param>
        /// <param name="toUnscape">Caracteres a escapar.</param>
        /// <returns>Cadena resultado.</returns>
        /// <exception cref="ParserException">
        ///     Si la cadena esta mal construida, lanza la excepcion:
        ///     <c>ParserException</c>.
        /// </exception>
        public static string Unscape(this string str, char scape, char[] toUnscape)
        {
            HashSet<char> aux = new HashSet<char>(toUnscape);
            StringBuilder buff = new StringBuilder();
            for (int i = 0; i < str.Length; i++)
            {
                char ch = str[i];
                if (ch != scape)
                {
                    buff.Append(ch);
                }
                else
                {
                    i++;
                    if (i >= str.Length)
                    {
                        throw new ParserException();
                    }
                    ch = str[i];
                    if (!aux.Contains(ch))
                    {
                        throw new ParserException();
                    }
                    buff.Append(ch);
                }
            }
            return buff.ToString();
        }

        /// <summary>
        ///     Convierte el enumerable <c>enumer</c> en una representación textual.
        /// </summary>
        /// <param name="enumer">Enumerable.</param>
        /// <returns>Representación textual.</returns>
        public static string ToStringEx<T>(this IEnumerable<T> enumer,
                                           string beg = "[ ", string mid = "; ", string end = " ]",
                                           string format = "", IFormatProvider provider = null)
        {
            StringBuilder builder = new StringBuilder();
            if (beg != null)
            {
                builder.Append(beg);
            }
            bool first = true;
            foreach (T obj in enumer)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    if (mid != null)
                    {
                        builder.Append(mid);
                    }
                }

                if (obj is IFormattable)
                {
                    builder.Append(((IFormattable)obj).ToString(format, provider));
                }
                else
                {
                    builder.Append(obj);
                }
            }
            if (end != null)
            {
                builder.Append(end);
            }
            return builder.ToString();
        }

        /// <summary>
        ///     Convierte el enumerable <c>enumer</c> en una representación textual.
        /// </summary>
        /// <param name="enumer">Enumerable.</param>
        /// <returns>Representación textual.</returns>
        public static string ToStringEx<T>(this IEnumerable<T> enumer, Func<T, string> toString,
                                           string beg = "[ ", string mid = "; ", string end = " ]")
        {
            StringBuilder builder = new StringBuilder();
            if (beg != null)
            {
                builder.Append(beg);
            }
            bool first = true;
            foreach (T obj in enumer)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    if (mid != null)
                    {
                        builder.Append(mid);
                    }
                }

                builder.Append(toString(obj));
            }
            if (end != null)
            {
                builder.Append(end);
            }
            return builder.ToString();
        }
    }
}