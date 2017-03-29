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
using System.Windows.Forms;
using Essence.Util;
using Essence.Util.Events;
using Essence.View.Models;
using Essence.View.Resources;

namespace Essence.View.Controls.Menus
{
    /// <summary>
    ///     Render de <c>ToolStripItem</c>.
    /// </summary>
    public abstract class TSIRender
    {
        #region Miembros publicos

        /// <summary>
        ///     Indica si puede renderizar el objeto.
        /// </summary>
        /// <param name="obj">Objeto.</param>
        /// <returns>Indica si puede renderizarlo.</returns>
        public abstract bool IsRenderable(object obj);

        /// <summary>
        ///     Renderiza el objeto.
        /// </summary>
        /// <param name="obj">Objeto.</param>
        /// <param name="context">Contexto.</param>
        /// <returns>ToolStripItem.</returns>
        public abstract ToolStripItem Render(object obj, TSIContext context);

        #endregion

        protected static void Set(ToolStripItem tsItem, IComponentUI component, TSIContext context)
        {
            tsItem.Tag = component;
            tsItem.Name = component.Name;
            tsItem.Text = component.NameUI;
            tsItem.ToolTipText = component.DescriptionUI;
            tsItem.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            tsItem.AutoToolTip = false;
            tsItem.TextImageRelation = context.TextImageRelation;

            if (component.IconUI != null)
            {
                tsItem.Image = component.IconUI.ToImage(context.CurrentIconSize);
            }
            else if (context.CurrentDisplayStyle == ToolStripItemDisplayStyle.Image)
            {
                tsItem.Image = MainResources.icon_not_found.ToImage(context.CurrentIconSize);
            }

            tsItem.AutoSize = true;
            tsItem.ImageScaling = ToolStripItemImageScaling.None;
        }

        protected static void Set(ToolStripItem tsItem, IAction accion, TSIContext context)
        {
            Set(tsItem, (IComponentUI)accion, context);
            tsItem.Enabled = accion.Enabled;

            // NOTA: la primera vez que se pinta, se actualiza 'Enabled'.
            PaintEventHandler[] update = new PaintEventHandler[1];
            update[0] = (sender, args) =>
            {
                ActionUtils.NotifyUpdateState(accion);
                tsItem.Enabled = accion.Enabled;

                tsItem.Paint -= update[0];
            };
            tsItem.Paint += update[0];
        }

        protected static void Copy(ToolStripItem tsItemSrc, ToolStripItem tsItemDst)
        {
            // Se copia la informacion del elemento por defecto.
            tsItemDst.Tag = tsItemSrc.Tag;
            tsItemDst.Name = tsItemSrc.Name;
            tsItemDst.Height = tsItemSrc.Height;
            tsItemDst.DisplayStyle = tsItemSrc.DisplayStyle;
            tsItemDst.Enabled = tsItemSrc.Enabled;
            tsItemDst.Text = tsItemSrc.Text;
            tsItemDst.AutoToolTip = tsItemSrc.AutoToolTip;
            tsItemDst.ToolTipText = tsItemSrc.ToolTipText;
            tsItemDst.Image = tsItemSrc.Image;
        }
    }

    /// <summary>
    ///     Render.
    /// </summary>
    public abstract class TSIRender<T, TTSItem> : TSIRender
        where TTSItem : ToolStripItem
    {
        public virtual bool IsRenderable(T obj)
        {
            return true;
        }

        /// <summary>
        ///     Renderiza el objeto.
        /// </summary>
        /// <param name="obj">Objeto.</param>
        /// <param name="context">Contexto.</param>
        /// <returns>ToolStripItem.</returns>
        public abstract TTSItem Render(T obj, TSIContext context);

        #region TSIRender

        /// <summary>
        ///     Indica si puede renderizar el objeto indicado.
        /// </summary>
        /// <param name="obj">Objeto.</param>
        /// <returns>Indica si puede renderizar.</returns>
        public sealed override bool IsRenderable(object obj)
        {
            return (obj is T) && this.IsRenderable((T)obj);
        }

        /// <summary>
        ///     Renderiza el objeto indicado.
        /// </summary>
        /// <param name="obj">Objeto.</param>
        /// <param name="context">Contexto.</param>
        /// <returns>Objeto renderizado.</returns>
        public sealed override ToolStripItem Render(object obj, TSIContext context)
        {
            return this.Render((T)obj, context);
        }

        #endregion
    }

    public abstract class TSIRenderComponentUI<T, TTSItem> : TSIRender<T, TTSItem>
        where TTSItem : ToolStripItem
        where T : class, IComponentUI
    {
        /// <summary>
        ///     Asociation entre un <c>ToolStripItem</c> y una <c>IProperty</c>.
        /// </summary>
        protected abstract class Link : DisposableObject
        {
            protected Link(TTSItem tsItem, T item, TSIContext context)
            {
                this.context = (TSIContext)context.Clone();
                this.TSItem = tsItem;
                this.Item = item;
            }

            protected virtual void OnSubscribeControlEvents(TTSItem tsItem)
            {
                tsItem.Disposed += this.TSItem_Disposed;
            }

            protected virtual void OnUnsubscribeControlEvents(TTSItem tsItem)
            {
                tsItem.Disposed -= this.TSItem_Disposed;
            }

            protected virtual void OnSubscribeItemEvents(T item)
            {
                item.PropertyChanged += this.Property_PropertyChanged;
            }

            protected virtual void OnUnsubscribeItemEvents(T item)
            {
                item.PropertyChanged -= this.Property_PropertyChanged;
            }

            /// <summary>
            ///     Escucha el evento <c>TSItem.Disposed</c>.
            /// </summary>
            private void TSItem_Disposed(object sender, EventArgs args)
            {
                this.Dispose();
            }

            /// <summary>
            ///     Escucha el evento <c>Property.PropertyChanged</c>.
            /// </summary>
            private void Property_PropertyChanged(object sender, PropertyChangedExEventArgs args)
            {
                switch (args.PropertyName)
                {
                    case AbsComponentUI.NAME_UI:
                    case AbsComponentUI.DESCRIPTION_UI:
                    case AbsComponentUI.ICON_UI:
                    {
                        this.tsItem.Text = this.Item.NameUI;
                        this.tsItem.ToolTipText = this.Item.DescriptionUI;
                        if (this.Item.IconUI != null)
                        {
                            this.tsItem.Image = this.Item.IconUI.ToImage(this.context.CurrentIconSize);
                        }
                        else if (this.context.CurrentDisplayStyle == ToolStripItemDisplayStyle.Image)
                        {
                            this.tsItem.Image = MainResources.icon_not_found.ToImage(this.context.CurrentIconSize);
                        }
                        break;
                    }
                }
            }

            /// <summary>
            ///     Etiqueta+Valor ToolStripItem.
            /// </summary>
            protected TTSItem TSItem
            {
                get { return this.tsItem; }
                set
                {
                    if (this.tsItem != value)
                    {
                        if (this.tsItem != null)
                        {
                            this.OnUnsubscribeControlEvents(this.tsItem);
                        }
                        this.tsItem = value;
                        if (this.tsItem != null)
                        {
                            this.OnSubscribeControlEvents(this.tsItem);
                        }
                    }
                }
            }

            /// <summary>
            ///     Propiedad.
            /// </summary>
            protected T Item
            {
                get { return this.item; }
                set
                {
                    if (this.item != value)
                    {
                        if (this.item != null)
                        {
                            this.OnUnsubscribeItemEvents(this.item);
                        }
                        this.item = value;
                        if (this.item != null)
                        {
                            this.OnSubscribeItemEvents(this.item);
                        }
                    }
                }
            }

            /// <summary>ToolStrip Etiqueta-Valor.</summary>
            private TTSItem tsItem;

            /// <summary>Propiedad.</summary>
            private T item;

            /// <summary>Copia del contexto.</summary>
            protected TSIContext context;

            #region DisposableObject

            protected override void DisposeOfManagedResources()
            {
                this.TSItem = null;
                this.Item = null;

                base.DisposeOfManagedResources();
            }

            #endregion
        }
    }
}