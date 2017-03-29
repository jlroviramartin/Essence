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

using System.Linq;
using System.Text;

namespace Essence.Util
{
    /// <summary>
    ///     Construye un filtro para ficheros.
    /// </summary>
    public class FilterBuilder
    {
        public FilterBuilder()
        {
        }

        public FilterBuilder(string description, string extension)
        {
            this.filtro.AppendFormat("{0} (*.{1})|*.{1}", description, extension);
        }

        /// <summary>
        ///     Añade un nuevo filtro. La extension no contiene *.
        /// </summary>
        public FilterBuilder Append(string description, string extension)
        {
            if (this.filtro.Length > 0)
            {
                this.filtro.Append("|");
            }
            this.filtro.AppendFormat("{0} (*.{1})|*.{1}",
                                     description,
                                     BuildExt(extension));
            return this;
        }

        /// <summary>
        ///     Añade un nuevo filtro. La extension no contiene *.
        /// </summary>
        public FilterBuilder Append(string description, string[] extensions)
        {
            if (this.filtro.Length > 0)
            {
                this.filtro.Append("|");
            }
            this.filtro.AppendFormat("{0} ({1})|{1}",
                                     description,
                                     extensions.Select(BuildExt).ToStringEx("", ";", ""));
            return this;
        }

        /// <summary>
        ///     Añade un nuevo filtro. La extension contiene *.
        /// </summary>
        public FilterBuilder Append2(string description, string extensions)
        {
            if (this.filtro.Length > 0)
            {
                this.filtro.Append("|");
            }
            this.filtro.AppendFormat("{0} ({1})|{1}",
                                     description,
                                     extensions);
            return this;
        }

        /// <summary>
        ///     Añade un nuevo filtro. La extension contiene *.
        /// </summary>
        public FilterBuilder Append2(string description, string[] extensions)
        {
            if (this.filtro.Length > 0)
            {
                this.filtro.Append("|");
            }
            this.filtro.AppendFormat("{0} ({1})|{1}",
                                     description,
                                     extensions.ToStringEx("", ";", ""));
            return this;
        }

        public string Filtro()
        {
            return this.filtro.ToString();
        }

        public override string ToString()
        {
            return this.Filtro();
        }

        #region private

        private static string BuildExt(string ext)
        {
            if (string.IsNullOrEmpty(ext))
            {
                return "*.*";
            }
            return "*." + ext;
        }

        /// <summary>Filtro.</summary>
        private readonly StringBuilder filtro = new StringBuilder();

        #endregion
    }
}