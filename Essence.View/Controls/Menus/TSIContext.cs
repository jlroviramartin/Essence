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
using System.Drawing;
using System.Windows.Forms;
using Essence.View.Models;
using Essence.View.Models.Properties;
using Essence.View.Views;

namespace Essence.View.Controls.Menus
{
    /// <summary>
    ///     Contexto que se aplica a la hora de renderizar un <c>ToolStripItem</c>.
    /// </summary>
    public class TSIContext : ICloneable
    {
        public TSIContext()
        {
            this.IconSize1 = new Size(32, 32);
            this.IconSize2 = new Size(32, 32);
        }

        /// <summary>Vista asociada.</summary>
        public IView View { get; set; }

        /// <summary>Localizacion.</summary>
        public TSILocalization Localization { get; set; }

        /// <summary>Tamaño de los iconos.</summary>
        public ToolStripItemDisplayStyle CurrentDisplayStyle
        {
            get
            {
                if (this.Level == 0)
                {
                    return this.DisplayStyle1;
                }
                return this.DisplayStyle2;
            }
        }

        /// <summary>Estilo visual de un elemento del menu nivel 1.</summary>
        public ToolStripItemDisplayStyle DisplayStyle1 { get; set; }

        /// <summary>Estilo visual de un elemento del menu nivel 2 y sucesivos.</summary>
        public ToolStripItemDisplayStyle DisplayStyle2 { get; set; }

        /// <summary>Tamaño de los iconos.</summary>
        public Size CurrentIconSize
        {
            get
            {
                if ((this.Level == 0) || this.IconSize2.IsEmpty)
                {
                    return this.IconSize1;
                }
                return ((this.Level == 0) ? this.IconSize1 : this.IconSize2);
            }
        }

        /// <summary>Tamaño de los iconos nivel 1.</summary>
        public Size IconSize1 { get; set; }

        /// <summary>Tamaño de los iconos nivel 2 y sucesivos.</summary>
        public Size IconSize2 { get; set; }

        /// <summary>Relación entre el icono y el texto.</summary>
        public TextImageRelation TextImageRelation { get; set; }

        /// <summary>Nivel actual.</summary>
        public int Level { get; set; }

        public IServiceProvider ServiceProvider { get; set; }

        public IViewPropertiesProvider PropertiesProvider { get; set; }

        #region ICloneable

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #endregion
    }
}