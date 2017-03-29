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
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows.Forms;
using Essence.Util.Collections;
using Essence.Util.Logs;
using Essence.View.Forms;
using Essence.View.Models;

namespace Essence.View.Controls
{
    public class FormViewAsTableControl : FormViewControl<FormViewAsTableControl.CustomTableLayoutPanel>
    {
        public FormViewAsTableControl()
        {
            this.ErrorIconPadding = 15;

            CustomTableLayoutPanel control = this.Control;
            control.FormView = this;

            control.CategoryRowStyle = new RowStyle(SizeType.AutoSize);
            control.PropertyRowStyle = new RowStyle(SizeType.AutoSize);

            control.LabelColumnStyle = new ColumnStyle(SizeType.AutoSize);
            control.ControlColumnStyle = new ColumnStyle(SizeType.Percent, 100);
            control.ErrorColumnStyle = new ColumnStyle(SizeType.Absolute, this.ErrorIconPadding);

            control.ShowCategories = true;
        }

        public int ErrorIconPadding { get; set; }

        #region Inner

        /// <summary>
        ///     Control <c>TableLayoutPanel</c> especifico similar a una tabla.
        /// </summary>
        public class CustomTableLayoutPanel : TableLayoutPanel,
                                              ICustomControl
        {
            public CustomTableLayoutPanel()
            {
//#if DEBUG
//                this.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
//#endif

                this.Initialize();
            }

            public FormViewAsTableControl FormView { get; set; }

            public RowStyle CategoryRowStyle { get; set; }
            public RowStyle PropertyRowStyle { get; set; }

            public ColumnStyle LabelColumnStyle { get; set; }
            public ColumnStyle ControlColumnStyle { get; set; }
            public ColumnStyle ErrorColumnStyle { get; set; }

            public bool ShowCategories { get; set; }

            #region private

            private void AddProperties(IDictionary<string, PropertyData> properties, bool showCategories)
            {
                IFormModel formModel = this.FormView.FormModel;

                int maxColumnProperties = 1;
                this.ColumnCount = maxColumnProperties * 3;

                // Se asigna el estilo a las columnas.
                for (int i = 0; i < maxColumnProperties; i++)
                {
                    this.ColumnStyles.Add(new ColumnStyle(this.LabelColumnStyle.SizeType, this.LabelColumnStyle.Width));
                    this.ColumnStyles.Add(new ColumnStyle(this.ControlColumnStyle.SizeType, this.ControlColumnStyle.Width));
                    this.ColumnStyles.Add(new ColumnStyle(this.ErrorColumnStyle.SizeType, this.ErrorColumnStyle.Width));
                }

                // Se rellena la tabla.
                if (!showCategories)
                {
                    foreach (PropertyData data in formModel.GetProperties().Select(name => properties.GetSafe(name)))
                    {
                        this.AddProperty(data);
                    }
                }
                else
                {
                    foreach (string category in formModel.GetCategories())
                    {
                        PropertyData firstOrDefault = formModel.GetProperties(category).Select(name => properties.GetSafe(name)).FirstOrDefault();
                        if (firstOrDefault != null)
                        {
                            this.AddCategory(firstOrDefault.CategoryControl, formModel.GetProperties(category).Select(name => properties.GetSafe(name)));
                        }
                    }
                }
            }

            private void AddCategory(Control categoryLabel, IEnumerable<PropertyData> enumerable)
            {
                this.AddCategory(categoryLabel);

                if (categoryLabel is CollapsibleControl)
                {
                    CollapsibleControl collapsibleControl = (CollapsibleControl)categoryLabel;
                    collapsibleControl.CollapsedOrExpanded += (sender, args) =>
                    {
                        this.SuspendLayout();

                        foreach (PropertyData data in enumerable)
                        {
                            if (data.LabelControl != null)
                            {
                                if (collapsibleControl.Collapse)
                                {
                                    data.LabelControl.Hide();
                                }
                                else
                                {
                                    data.LabelControl.Show();
                                }
                            }
                            if (data.EditorControl != null)
                            {
                                if (collapsibleControl.Collapse)
                                {
                                    data.EditorControl.Hide();
                                }
                                else
                                {
                                    data.EditorControl.Show();
                                }
                            }
                        }

                        this.ResumeLayout(true);
                    };
                }

                foreach (PropertyData data in enumerable)
                {
                    this.AddProperty(data);
                }
            }

            private void AddCategory(Control category)
            {
                // Estilo de la fila de la categoría.
                this.RowStyles.Add(new RowStyle(this.CategoryRowStyle.SizeType, this.CategoryRowStyle.Height));

                int col = 0;
                int row = this.RowCount;

                this.Controls.Add(category, col, row);
                this.SetColumnSpan(category, this.ColumnCount);

                this.RowCount++;

                Contract.Assert(this.RowStyles.Count == this.RowCount);
            }

            private void AddProperty(PropertyData data)
            {
                // Estilo de la fila.
                this.RowStyles.Add(new RowStyle(this.PropertyRowStyle.SizeType, this.PropertyRowStyle.Height));

                try
                {
                    if (data.LabelControl != null)
                    {
                        this.AddProperty(data.Name, data.LabelControl, data.EditorControl);
                    }
                    else
                    {
                        this.AddProperty(data.Name, data.EditorControl);
                    }
                }
                catch (Exception e)
                {
                    Log<CustomTableLayoutPanel>.Error(e);
                }

                Contract.Assert(this.RowStyles.Count == this.RowCount);
            }

            private void AddProperty(string name, Control label, Control editor, bool invertLabelControl = false)
            {
                int row = this.RowCount;
                int col = 0;

                // Se añade la etiqueta y el control (invertidos, si procede).
                if (!invertLabelControl)
                {
                    this.Controls.Add(label, col, row);
                    this.Controls.Add(editor, col + 1, row);
                }
                else
                {
                    this.Controls.Add(editor, col, row);
                    this.Controls.Add(label, col + 1, row);
                }

                // Se actualizan los indices.
                this.indices.AddOrSet(name, new Tuple<int, int>(row, col));

                // Se incrementa el numero de filas/columnas, si procede.
                this.RowCount = Math.Max(this.RowCount, row + 1);
                this.ColumnCount = Math.Max(this.ColumnCount, col + 3); // label + control + error

                /*// Se incrementa el numero de filas, si procede.
                if (string.IsNullOrEmpty(dependsOn))
                {
                    this.RowCount++;
                }*/

                this.editControls.Add(name, editor);
            }

            private void AddProperty(string name, Control editor)
            {
                int row = this.RowCount;
                int col = 0;

                // Se añade el control.
                this.Controls.Add(editor, col, row);

                // Se indica que ocupa 2 posiciones.
                this.SetColumnSpan(editor, 2);

                // Se actualizan los indices.
                this.indices.AddOrSet(name, new Tuple<int, int>(row, col));

                // Se incrementa el numero de filas, si procede.
                this.RowCount = Math.Max(this.RowCount, row + 1);

                this.editControls.Add(name, editor);
            }

            private void Initialize()
            {
                this.RowCount = 0;
                this.ColumnCount = 3;
                this.GrowStyle = TableLayoutPanelGrowStyle.AddRows;

                // Se utiliza DoubleBuffer.
                this.DoubleBuffered = true;
                this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);

                this.UpdateStyles();
            }

            // Mapa de nombres a su indice de (fila, columna).
            private readonly Dictionary<string, Tuple<int, int>> indices = new Dictionary<string, Tuple<int, int>>();

            private readonly Dictionary<string, Control> editControls = new Dictionary<string, Control>();

            #endregion

            #region ICustomControl

            public IEnumerable<Control> GetEditControls()
            {
                return this.editControls.Values;
            }

            public Control GetEditControl(string name)
            {
                return this.editControls.GetSafe(name);
            }

            public void UpdateControls()
            {
                this.editControls.Clear();
                this.Controls.Clear();
                this.AddProperties(this.FormView.BuildControls(this.ShowCategories), this.ShowCategories);
            }

            #endregion
        }

        #endregion
    }
}