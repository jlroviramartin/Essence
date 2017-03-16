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
using System.IO;
using System.Text;
using Essence.Util.Properties;

namespace Essence.Util.IO
{
    /// <summary>
    ///     Constructor para el metodo <c>Object.ToString()</c>.
    ///     Permite de forma sencilla, implementar el metodo
    ///     <c>Object.ToString()</c>.
    ///     <example><![CDATA[
    /// public override String ToString()
    /// {
    ///     return new ToStringBuilder(this)
    ///         .Append("Ancho", this.Ancho)
    ///         .Append("Alto", this.Alto)
    ///         .ToString();
    /// }
    /// ]]></example>
    /// </summary>
    public sealed class ToStringBuilder
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        public ToStringBuilder()
        {
            this.builder = new StringBuilder();
            this.writer = new TextWriterEx(new StringWriter(this.builder));
        }

        /// <summary>
        ///     Añade el objeto con valor <c>obj</c> .
        /// </summary>
        /// <param name="format">Formato.</param>
        /// <param name="args">Argumentos.</param>
        /// <param name="provider">Proveedor.</param>
        /// <returns>This.</returns>
        [StringFormatMethod("format")]
        public ToStringBuilder AppendFormat(string format, params object[] args)
        {
            return this.AppendFormat(null, format, args);
        }

        [StringFormatMethod("format")]
        public ToStringBuilder AppendFormat(IFormatProvider provider, string format, params object[] args)
        {
            this.AppendInner(string.Format(provider, format, args), string.Empty, provider);
            return this;
        }

        /// <summary>
        ///     Añade el objeto con valor <c>obj</c> .
        /// </summary>
        /// <param name="obj">Valor.</param>
        /// <param name="format">Formato.</param>
        /// <param name="provider">Proveedor.</param>
        /// <returns>This.</returns>
        public ToStringBuilder Append(object obj)
        {
            return this.Append(null, null, obj);
        }

        public ToStringBuilder Append(string format, object obj)
        {
            return this.Append(null, format, obj);
        }

        public ToStringBuilder Append(IFormatProvider provider, string format, object obj)
        {
            this.AppendInner(obj, format, provider);
            return this;
        }

        /// <summary>
        ///     Añade el objeto con valores <c>objs</c> .
        /// </summary>
        /// <param name="objs">Valores.</param>
        /// <param name="format">Formato.</param>
        /// <param name="provider">Proveedor.</param>
        /// <returns>This.</returns>
        public ToStringBuilder AppendRange<T>(IEnumerable<T> objs)
        {
            return this.AppendRange(null, null, objs);
        }

        public ToStringBuilder AppendRange<T>(string format, IEnumerable<T> objs)
        {
            return this.AppendRange(null, format, objs);
        }

        public ToStringBuilder AppendRange<T>(IFormatProvider provider, string format, IEnumerable<T> objs)
        {
            this.AppendRangeInner(objs, format, provider);
            return this;
        }

        public ToStringBuilder AppendLine()
        {
            this.writer.Write(Environment.NewLine);
            return this;
        }

        public ToStringBuilder Indent()
        {
            this.writer.Indent();
            return this;
        }

        public ToStringBuilder Unindent()
        {
            this.writer.Unindent();
            return this;
        }

        #region private

        /// <summary>
        ///     Añade el objeto con valor <c>obj</c> .
        /// </summary>
        private void AppendInner(object obj, string format, IFormatProvider provider)
        {
            if (obj == null)
            {
                this.writer.Write(ObjetoNull);
            }
            else if (obj is IFormattable)
            {
                this.writer.Write(((IFormattable)obj).ToString(format, provider));
            }
            else
            {
                this.writer.Write(obj);
            }
        }

        /// <summary>
        ///     Añade el objeto con valores <c>objs</c> .
        /// </summary>
        private void AppendRangeInner<T>(IEnumerable<T> objs, string format, IFormatProvider provider)
        {
            if (objs == null)
            {
                this.writer.Write(ObjetoNull);
            }
            else
            {
                this.writer.Write(InicioArray);
                int i = 0;
                foreach (T obj in objs)
                {
                    if (i > 0)
                    {
                        this.writer.Write(SeparadorArray);
                    }
                    this.AppendInner(obj, format, provider);
                    i++;
                }
                this.writer.Write(FinArray);
            }
        }

        /// <summary>Representacion del objeto null.</summary>
        private const string ObjetoNull = "<null>";

        /// <summary>Representacion de inicio de array.</summary>
        private const string InicioArray = "[ ";

        /// <summary>Representacion de separacion entre elementos del array.</summary>
        private const string SeparadorArray = ", ";

        /// <summary>Representacion de fin de array.</summary>
        private const string FinArray = " ]";

        /// <summary>Constructor de strings.</summary>
        private readonly TextWriterEx writer;

        private readonly StringBuilder builder;

        #endregion

        #region Object

        public override string ToString()
        {
            return new StringBuilder()
                .Append(this.builder)
                .ToString();
        }

        #endregion
    }
}