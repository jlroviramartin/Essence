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
using System.Linq;
using System.Windows.Forms;
using Essence.Util.Collections;

namespace Essence.View.Controls.Menus
{
    public sealed class ToolStripItemRender
    {
        public static ToolStripItemRender Instance = new ToolStripItemRender();

        public ToolStripItem Render(object item, TSIContext context)
        {
            Type tipo = ((item != null) ? item.GetType() : null);

            // Si esta en la tsiCache y lo puede renderizar, utiliza la tsiCache (por tipo).
            if ((tipo != null) && this.tsiCache.ContainsKey(tipo))
            {
                TSIRender render = this.tsiCache[tipo];
                if (render.IsRenderable(item))
                {
                    return render.Render(item, context);
                }
            }

            // Se busca entre los tsiRenders.
            foreach (TSIRender render in this.tsiRenders)
            {
                if (render.IsRenderable(item))
                {
                    // Si no esta en la tsiCache, se añade a la tsiCache (por tipo).
                    if ((tipo != null) && !this.tsiCache.ContainsKey(tipo))
                    {
                        this.tsiCache.Add(tipo, render);
                    }
                    return render.Render(item, context);
                }
            }

            return null;
        }

        public IEnumerable<ToolStripItem> RenderRange(TSIContext context, IEnumerable<object> components)
        {
            return components.Select(c => this.Render(c, context));
        }

        #region private

        private ToolStripItemRender()
        {
            this.tsiRenders.Add(new TSIRenderComposedComponentUI());
            this.tsiRenders.Add(new TSIRenderNull());
            this.tsiRenders.Add(new TSIRenderSeparator());
            this.tsiRenders.Add(new TSIRenderLabel());
            //this.tsiRenders.Add(new TSIRenderBooleanAction());
            this.tsiRenders.Add(new TSIRenderComposedAction());
            this.tsiRenders.Add(new TSIRenderAction());
            this.tsiRenders.Add(new TSIRenderProperty());
        }

        /// <summary>ToolStripItem renders.</summary>
        private readonly List<TSIRender> tsiRenders = new List<TSIRender>();

        /// <summary>Cache de ToolStripItem renders por tipo.</summary>
        private readonly DictionaryOfType<TSIRender> tsiCache = new DictionaryOfType<TSIRender>();

        #endregion
    }
}