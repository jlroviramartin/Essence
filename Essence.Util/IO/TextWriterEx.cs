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

using System.IO;
using System.Text;

namespace Essence.Util.IO
{
    /// <summary>
    ///     Implementacion de TextWriter que permite añadir texto indentado.
    /// </summary>
    public class TextWriterEx : TextWriter
    {
        public TextWriterEx(TextWriter textWriter)
        {
            this.textWriter = textWriter;
        }

        public string Space
        {
            get { return this.space; }
            set { this.space = value; }
        }

        public void Indent()
        {
            this.indent++;
        }

        public void Unindent()
        {
            this.indent--;
        }

        #region private

        private string space = "\t";
        private int indent;
        private bool doIndent;
        private readonly TextWriter textWriter;

        #endregion

        #region TextWriter

        public override Encoding Encoding
        {
            get { return this.textWriter.Encoding; }
        }

        public override void Write(char ch)
        {
            // Si hay que añadir indentacion, se añade.
            if (this.doIndent)
            {
                this.doIndent = false;
                for (int i = 0; i < this.indent; ++i)
                {
                    this.textWriter.Write(this.space);
                }
            }

            this.textWriter.Write(ch);

            // Si es salto de linea, se indica que el proximo caracter a escribir tiene indentacion.
            if (ch == '\n')
            {
                this.doIndent = true;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.textWriter.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}