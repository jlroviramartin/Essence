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
using System.IO;
using System.Text;

namespace Essence.Util.IO
{
    /// <summary>
    ///     Constructor para el metodo <c>Object.ToString()</c>.
    ///     Permite de forma sencilla, implementar el metodo
    ///     <c>Object.ToString()</c>.
    ///     <example><![CDATA[
    /// public override String ToString()
    /// {
    ///     return new PropertiesStringBuilder(this)
    ///         .Append("Ancho", this.Ancho)
    ///         .Append("Alto", this.Alto)
    ///         .ToString();
    /// }
    /// ]]></example>
    /// </summary>
    public sealed class PropertiesStringBuilder
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        public PropertiesStringBuilder()
        {
            this.builder = new StringBuilder();
            this.writer = new TextWriterEx(new StringWriter(this.builder));
        }

        /// <summary>
        ///     Añade el objeto con nombre <c>nombre</c> y valor <c>obj</c> .
        /// </summary>
        /// <param name="name">Nombre.</param>
        /// <param name="obj">Valor.</param>
        /// <param name="format">Formato.</param>
        /// <param name="provider">Proveedor.</param>
        /// <returns>This.</returns>
        public PropertiesStringBuilder AppendNamed(string name, object obj)
        {
            return this.AppendNamed(null, null, name, obj);
        }

        public PropertiesStringBuilder AppendNamed(string format, string name, object obj)
        {
            return this.AppendNamed(null, format, name, obj);
        }

        public PropertiesStringBuilder AppendNamed(IFormatProvider provider, string format, string name, object obj)
        {
            if (this.count > 0)
            {
                this.writer.Write(SeparadorObjetos);
            }

            this.writer.Write(name);
            this.writer.Write(SeparadorNombreValor);
            this.AppendInner(obj, format, provider);

            this.count++;
            return this;
        }

        /// <summary>
        ///     Añade el objeto con nombre <c>nombre</c> y valores <c>objs</c> .
        /// </summary>
        /// <param name="name">Nombre.</param>
        /// <param name="objs">Valores.</param>
        /// <param name="format">Formato.</param>
        /// <param name="provider">Proveedor.</param>
        /// <returns>This.</returns>
        public PropertiesStringBuilder AppendNamedRange<T>(string name, IEnumerable<T> objs)
        {
            return this.AppendNamedRange(null, null, name, objs);
        }

        public PropertiesStringBuilder AppendNamedRange<T>(string format, string name, IEnumerable<T> objs)
        {
            return this.AppendNamedRange(null, format, name, objs);
        }

        public PropertiesStringBuilder AppendNamedRange<T>(IFormatProvider provider, string format, string name, IEnumerable<T> objs)
        {
            if (this.count > 0)
            {
                this.writer.Write(SeparadorObjetos);
            }

            this.writer.Write(name);
            this.writer.Write(SeparadorNombreValor);
            this.AppendRangeInner(objs, format, provider);

            this.count++;
            return this;
        }

        public PropertiesStringBuilder AppendLine()
        {
            this.writer.Write(Environment.NewLine);
            return this;
        }

        public PropertiesStringBuilder Indent()
        {
            this.writer.Indent();
            return this;
        }

        public PropertiesStringBuilder Unindent()
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
                this.writer.Write(InicioObjeto);
                this.writer.Write(((IFormattable)obj).ToString(format, provider));
                this.writer.Write(FinObjeto);
            }
            else
            {
                this.writer.Write(InicioObjeto);
                this.writer.Write(obj);
                this.writer.Write(FinObjeto);
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

        /// <summary>Representacion de inicio de string.</summary>
        private const string InicioToString = "[ ";

        /// <summary>Representacion de fin de string.</summary>
        private const string FinToString = " ]";

        /// <summary>Representacion de inicio de objeto.</summary>
        private const string InicioObjeto = "'";

        /// <summary>Representacion de fin de objeto.</summary>
        private const string FinObjeto = "'";

        /// <summary>Representacion de separacion entre objetos.</summary>
        private const string SeparadorObjetos = "; ";

        /// <summary>Representacion de separacion entre nombre y valor.</summary>
        private const string SeparadorNombreValor = " = ";

        /// <summary>Representacion de inicio de array.</summary>
        private const string InicioArray = "[ ";

        /// <summary>Representacion de separacion entre elementos del array.</summary>
        private const string SeparadorArray = ", ";

        /// <summary>Representacion de fin de array.</summary>
        private const string FinArray = " ]";

        /// <summary>Constructor de strings.</summary>
        private readonly TextWriterEx writer;

        private readonly StringBuilder builder;

        /// <summary>Numero de objetos.</summary>
        private int count;

        #endregion

        #region Object

        public override string ToString()
        {
            return new StringBuilder()
                .Append(InicioToString)
                .Append(this.builder)
                .Append(FinToString)
                .ToString();
        }

        #endregion
    }
}