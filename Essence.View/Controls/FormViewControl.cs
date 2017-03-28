using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Essence.Util;
using Essence.Util.Collections;
using Essence.Util.Events;
using Essence.Util.Logs;
using Essence.View.Controls.Forms;
using Essence.View.Forms;
using Essence.View.Models;
using Essence.View.Models.Properties;
using Essence.View.Views;
using Label = System.Windows.Forms.Label;

namespace Essence.View.Controls
{
    public class FormViewControl<TControl> : ViewControl<TControl>,
                                             IFormView
        where TControl : Control, FormViewControl<TControl>.ICustomControl, new()
    {
        public const string FORM_MODEL = "FormModel";
        public const string PROPERTIES_DESCRIPTION = "PropertiesDescription";

        public FormViewControl()
        {
            this.DefaultEditorFactory = Forms.DefaultEditorFactory.Instance;

            this.CategoryGap = 10;
            this.CategoryAutoSize = true;
            this.CategoryAnchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            this.CategoryDock = DockStyle.None;
            this.CategoryPadding = Padding.Empty;
            this.CategoryMargin = new Padding(0, 0, 0, 15);

            this.LabelAutoEllipsis = false;
            this.LabelTextAlign = ContentAlignment.MiddleLeft;
            this.LabelAutoSize = true;
            this.LabelAnchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            this.LabelDock = DockStyle.None;
            this.LabelPadding = Padding.Empty;
            this.LabelMargin = new Padding(5, 0, 5, 5);

            this.EditorAutoSize = true;
            this.EditorAnchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            this.EditorDock = DockStyle.None;
            this.EditorPadding = Padding.Empty;
            this.EditorMargin = new Padding(0, 0, 5, 5);

            this.ErrorProvider = new ErrorProvider( /*this.Control*/);
        }

        public IEditorFactory DefaultEditorFactory { get; set; }

        public ErrorProvider ErrorProvider { get; set; }

        public void Validate()
        {
            if (this.FormModel != null)
            {
                foreach (Control editControl in this.Control.GetEditControls())
                {
                    this.Control2Form(editControl);
                }
            }
        }

        #region Category layout

        public int CategoryGap { get; set; }
        public bool CategoryAutoSize { get; set; }
        public AnchorStyles CategoryAnchor { get; set; }
        public DockStyle CategoryDock { get; set; }
        public Padding CategoryPadding { get; set; }
        public Padding CategoryMargin { get; set; }

        #endregion

        #region Label layout

        public bool LabelAutoEllipsis { get; set; }
        public ContentAlignment LabelTextAlign { get; set; }
        public bool LabelAutoSize { get; set; }
        public AnchorStyles LabelAnchor { get; set; }
        public DockStyle LabelDock { get; set; }
        public Padding LabelPadding { get; set; }
        public Padding LabelMargin { get; set; }

        #endregion

        #region Editor layout

        public bool EditorAutoSize { get; set; }
        public AnchorStyles EditorAnchor { get; set; }
        public DockStyle EditorDock { get; set; }
        public Padding EditorPadding { get; set; }
        public Padding EditorMargin { get; set; }

        #endregion

        #region protected

        /// <summary>
        ///     Crea la categoria.
        /// </summary>
        protected virtual Control CreateCategory(string nombre)
        {
            CollapsibleControl label = new CollapsibleControl();
            label.Name = nombre + "_label";
            label.Text = nombre;
            label.LeftColor = label.BackColor;
            label.RightColor = label.BackColor;
            label.ShowHeaderSeparator = false;
            label.Gap = this.CategoryGap;
            label.AutoSize = this.CategoryAutoSize;
            label.Anchor = this.CategoryAnchor;
            label.Dock = this.CategoryDock;
            label.Padding = this.CategoryPadding;
            label.Margin = this.CategoryMargin;

            label.Gap = 0;
            label.RightColor = Color.FromArgb(64, 0, 0, 64);
            return label;
        }

        /// <summary>
        ///     Crea la etiqueta.
        /// </summary>
        protected virtual Control CreateLabel(string name)
        {
            IFormModel formModel = this.FormModel;

            PropertyDescription propDescription = formModel.GetDescription(name);

            Label label = new Label();
            label.Name = name + "_label";
            label.Text = propDescription.Label;
            label.AutoEllipsis = this.LabelAutoEllipsis;
            label.TextAlign = this.LabelTextAlign;
            label.AutoSize = this.LabelAutoSize;
            label.Anchor = this.LabelAnchor;
            label.Dock = this.LabelDock;
            label.Padding = this.LabelPadding;
            label.Margin = this.LabelMargin;

            string description = propDescription.Description;
            if (!string.IsNullOrEmpty(description))
            {
                ToolTip toolTip = new ToolTip();
                toolTip.SetToolTip(label, description);
            }

            return label;
        }

        /// <summary>
        ///     Crea el editor.
        /// </summary>
        protected virtual Control CreateEditor(string name, ErrorProvider errorProvider)
        {
            IFormModel formModel = this.FormModel;

            PropertyDescription propDescription = formModel.GetDescription(name);
            Type tipo = formModel.GetPropertyType(name);

            // Se busca la fabrica de editores.
            IEditorFactory editorFactory = propDescription.EditorFactory ?? this.DefaultEditorFactory;
            Contract.Assert(editorFactory != null);

            ViewProperties[] viewProperties = formModel.GetViewProperties(name).ToArray();
            IServiceProvider serviceProvider = this.GetServiceProvider();

            // Se crea el control mediante la fabrica de editores.
            Control editor = (Control)editorFactory.Create(tipo, viewProperties, serviceProvider);

            if (editor == null)
            {
                Log<FormViewControl<TControl>>.ErrorFormat("No se encuentra mapeo para la propiedad de formulario {0} [{1}]", formModel, name);

                editor = new System.Windows.Forms.Label();
                editor.Text = "<ERROR>";
            }

            //editor.Site = mySite;
            editor.Name = name;
            editor.Enabled = true;
            editor.AutoSize = this.EditorAutoSize;
            editor.Anchor = this.EditorAnchor;
            editor.Dock = this.EditorDock;
            editor.Padding = this.EditorPadding;
            editor.Margin = this.EditorMargin;

            string description = propDescription.Description;
            if (!string.IsNullOrEmpty(description))
            {
                ToolTip toolTip = new ToolTip();
                toolTip.SetToolTip(editor, description);
            }

            ControlExchange exchange = ControlExchange.Get(editor);
            if (exchange != null)
            {
                exchange.PopertyName = name;
            }

            return editor;
        }

        protected IDictionary<string, PropertyData> BuildControls(bool showCategories)
        {
            IFormModel formModel = this.FormModel;

            Dictionary<string, Control> categories = new Dictionary<string, Control>();
            Dictionary<string, PropertyData> lines = new Dictionary<string, PropertyData>();

            if (formModel == null)
            {
                return lines;
            }

            if (showCategories)
            {
                foreach (string category in formModel.GetCategories())
                {
                    Contract.Assert(!string.IsNullOrWhiteSpace(category));

                    Control categoryControl = this.CreateCategory(category);
                    categories.Add(category, categoryControl);
                }
            }

            foreach (string name in formModel.GetProperties())
            {
                try
                {
                    PropertyDescription propDescription = formModel.GetDescription(name);

                    PropertyData line = new PropertyData();
                    line.Name = name;

                    // Se crea y configura el editor.
                    Control editorControl = this.CreateEditor(name, this.ErrorProvider);
                    line.EditorControl = editorControl;

                    // Se trata la etiqueta.
                    if (!propDescription.HideLabel)
                    {
                        // Se crea y configura la etiqueta.
                        Control labelControl = this.CreateLabel(name);
                        line.LabelControl = labelControl;
                    }

                    string category = propDescription.Category;
                    line.Category = category;

                    if (showCategories)
                    {
                        Control categoryControl = categories[category];
                        line.CategoryControl = categoryControl;
                    }

                    lines.Add(name, line);
                }
                catch (Exception e)
                {
                    Log<FormViewControl<TControl>>.Error(e);
                }
            }

            return lines;
        }

        protected virtual void UpdateStructure()
        {
            if (this.FormModel == null)
            {
                return;
            }

            TControl control = this.Control;

            foreach (Control editControl in control.GetEditControls())
            {
                ControlExchange exchange = ControlExchange.Get(editControl);
                if (exchange != null)
                {
                    exchange.DoNotListen(editControl, this.Control_Changed);
                }
            }

            this.DeattachEditControls();

            control.UpdateControls();

            this.AttachEditControls();

            foreach (Control editControl in control.GetEditControls())
            {
                ControlExchange exchange = ControlExchange.Get(editControl);
                if (exchange != null)
                {
                    exchange.Listen(editControl, this.Control_Changed);
                }
            }

            this.ContentChanged();
            this.WholeForm2Control();
        }

        #endregion

        #region private

        private void FormModel_ContentChanged(object sender, EventArgs args)
        {
            this.ContentChanged();
            this.WholeForm2Control();
        }

        private void FormModel_StructureChanged(object sender, EventArgs args)
        {
            this.UpdateStructure();
        }

        private void FormModel_PropertyChanged(object sender, PropertyChangedExEventArgs args)
        {
            switch (args.PropertyName)
            {
                case ReflectionFormModel.FORM_TYPE:
                {
                    break;
                }
                case ReflectionFormModel.FORM_OBJECT:
                {
                    break;
                }
            }
            this.Form2Control(args.PropertyName);
        }

        private void Control_Changed(object sender, EventArgs args)
        {
            Control control = (Control)sender;
            this.Control2Form(control);
        }

        private void Control2Form(Control editControl)
        {
            IFormModel formModel = this.FormModel;
            if (formModel == null)
            {
                return;
            }

            ControlExchange exchange = ControlExchange.Get(editControl);
            if (exchange == null)
            {
                return;
            }

            try
            {
                string name = exchange.PopertyName;
                object value = exchange.GetValue(editControl);
                formModel.SetPropertyValue(name, value);

                this.ErrorProvider.SetError(editControl, null);
            }
            catch (Exception e)
            {
                this.ErrorProvider.SetError(editControl, e.Message);
            }
        }

        private void WholeForm2Control()
        {
            IFormModel formModel = this.FormModel;
            if (formModel == null)
            {
                return;
            }

            this.form2Control.Do(
                    () =>
                    {
                        foreach (string name in this.FormModel.GetProperties())
                        {
                            Control editControl = this.Control.GetEditControl(name);
                            if (editControl == null)
                            {
                                continue;
                            }

                            ControlExchange exchange = ControlExchange.Get(editControl);
                            if (exchange == null)
                            {
                                continue;
                            }

                            try
                            {
                                object value = formModel.GetPropertyValue(name);
                                exchange.SetValue(editControl, value);
                            }
                            catch (Exception e)
                            {
                                this.ErrorProvider.SetError(editControl, e.Message);
                            }
                        }
                    });
        }

        private void Form2Control(string name)
        {
            this.form2Control.Do(
                    () =>
                    {
                        IFormModel formModel = this.FormModel;
                        if (formModel == null)
                        {
                            return;
                        }

                        Control editControl = this.Control.GetEditControl(name);
                        if (editControl == null)
                        {
                            return;
                        }

                        ControlExchange exchange = ControlExchange.Get(editControl);
                        if (exchange == null)
                        {
                            return;
                        }

                        try
                        {
                            object value = formModel.GetPropertyValue(name);
                            exchange.SetValue(editControl, value);
                        }
                        catch (Exception e)
                        {
                            this.ErrorProvider.SetError(editControl, e.Message);
                        }
                    });
        }

        private void ContentChanged()
        {
            foreach (string name in this.FormModel.GetProperties())
            {
                Control editControl = this.Control.GetEditControl(name);
                if (editControl == null)
                {
                    continue;
                }

                ControlExchange exchange = ControlExchange.Get(editControl);
                if (exchange == null)
                {
                    continue;
                }

                IServiceProvider serviceProvider = this.GetServiceProvider();
                exchange.ContentChanged(editControl, this.FormModel, name, serviceProvider);
            }
        }

        private void AttachEditControls()
        {
            if (this.FormModel == null)
            {
                return;
            }

            foreach (Control editControl in this.Control.GetEditControls())
            {
                ControlExchange exchange = ControlExchange.Get(editControl);
                if (exchange != null)
                {
                    exchange.Attach();
                }
            }
        }

        private void DeattachEditControls()
        {
            if (this.FormModel == null)
            {
                return;
            }

            foreach (Control editControl in this.Control.GetEditControls())
            {
                ControlExchange exchange = ControlExchange.Get(editControl);
                if (exchange != null)
                {
                    exchange.Deattach();
                }
            }
        }

        /// <summary>
        /// Obtiene el proveedor de servicios para comunicarse con <c>ControlExchange</c>.
        /// </summary>
        private IServiceProvider GetServiceProvider()
        {
            MyServiceProvider serviceProvider = new MyServiceProvider(this);
            return serviceProvider;
        }

        /// <summary>Evita la reentrada.</summary>
        private readonly PreventReentry form2Control = new PreventReentry();

        private IFormModel formModel;

        #endregion

        #region IFormView

        public IFormModel FormModel
        {
            get { return this.formModel; }
            set
            {
                if (this.formModel != value)
                {
                    if (this.formModel != null)
                    {
                        this.formModel.ContentChanged -= this.FormModel_ContentChanged;
                        this.formModel.StructureChanged -= this.FormModel_StructureChanged;
                        this.formModel.PropertyChanged -= this.FormModel_PropertyChanged;
                    }

                    this.DeattachEditControls();

                    IFormModel oldValue = this.formModel;
                    this.formModel = value;

                    this.AttachEditControls();

                    if (this.formModel != null)
                    {
                        this.formModel.ContentChanged += this.FormModel_ContentChanged;
                        this.formModel.StructureChanged += this.FormModel_StructureChanged;
                        this.formModel.PropertyChanged += this.FormModel_PropertyChanged;
                    }

                    this.UpdateStructure();
                    this.OnPropertyChanged(FORM_MODEL, oldValue, value);
                }
            }
        }

        #endregion

        #region Inner classes

        public interface ICustomControl
        {
            IEnumerable<Control> GetEditControls();

            Control GetEditControl(string name);

            void UpdateControls();
        }

        protected sealed class PropertyData
        {
            public string Category { get; set; }

            public string Name { get; set; }

            public Control CategoryControl { get; set; }
            public Control LabelControl { get; set; }
            public Control EditorControl { get; set; }
        }

        public sealed class MyServiceProvider : IServiceProvider
        {
            public MyServiceProvider(IView view)
            {
                this.view = view;
            }

            public void Register(Type serviceType, object value)
            {
                this.map.Add(serviceType, value);
            }

            private readonly IView view;
            private readonly Dictionary<Type, object> map = new Dictionary<Type, object>();

            public object GetService(Type serviceType)
            {
                return this.map.GetSafe(serviceType) ?? this.view.ServiceProvider.GetService(serviceType);
            }
        }

        #endregion
    }
}