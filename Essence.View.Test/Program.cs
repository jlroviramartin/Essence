using System;
using System.Windows.Forms;
using Essence.View.Controls;
using Essence.View.Models;
using Essence.View.Views;
using Action = Essence.View.Models.Action;

namespace Essence.View.Test
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            Property<int> iproperty = new Property<int>();
            iproperty.Name = "Propiedad1";
            iproperty.NameUI = "Propiedad 1";
            iproperty.DescriptionUI = "Propiedad 1";
            iproperty.Value = 50;
            iproperty.Editable = true;
            iproperty.Enabled = true;

            ReflectionFormModel refFormModel = new ReflectionFormModel();
            refFormModel.FormType = typeof(Prueba);
            refFormModel.FormObject = new Prueba();

            ComposedComponentUI menu1 = new ComposedComponentUI();
            menu1.Name = "Menu1";
            menu1.NameUI = "Menú ";
            menu1.DescriptionUI = "Menú 1";
            menu1.Components.Add(new Action() { NameUI = "Acción 1", DescriptionUI = "Acción 1"});
            menu1.Components.Add(new Action() { NameUI = "Acción 2", DescriptionUI = "Acción 2" });

            menu1.Components.Add(iproperty);

            ComposedComponentUI menu2 = new ComposedComponentUI();
            menu2.Name = "Menu2";
            menu2.NameUI = "Menú 2";
            menu2.DescriptionUI = "Menú ";
            menu2.Components.Add(new Action() { NameUI = "Acción 1", DescriptionUI = "Acción 1" });
            menu2.Components.Add(new Action() { NameUI = "Acción 2", DescriptionUI = "Acción 2" });

            menu2.Components.Add(iproperty);

            DialogViewControl<Form> dialog = new DialogViewControl<Form>();

            IFormView formView = new FormViewAsTableControl();
            formView.FormModel = refFormModel;

            IMenuView menuView = new MenuViewControl<MenuStrip>();
            menuView.Components.Add(menu1);
            menuView.Components.Add(menu2);
            menuView.Components.Add(iproperty);

            dialog.AddView(menuView, DockConstraints.Top);
            dialog.AddView(formView, DockConstraints.Fill);

            Application.Run((Form)((IRefControl)dialog).Control);
        }
    }
}
