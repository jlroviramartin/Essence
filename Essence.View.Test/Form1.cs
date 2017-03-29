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
using System.ComponentModel;
using System.Windows.Forms;
using Essence.Util.Events;
using Essence.Util.Properties;
using Essence.View.Attributes;
using Essence.View.Controls;
using Essence.View.Models;

namespace Essence.View.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            this.InitializeComponent();

            FormViewAsTableControl formView = new FormViewAsTableControl();

            ReflectionFormModel refFormModel = new ReflectionFormModel();
            refFormModel.FormType = typeof(Prueba);
            refFormModel.FormObject = new Prueba();
            formView.FormModel = refFormModel;

            Control control = ((IRefControl)formView).Control;
            control.Dock = DockStyle.Fill;
            this.Controls.Add(control);
        }
    }

    public class Prueba : AbsNotifyPropertyChanged
    {
        [Browsable(false)]
        public double Dato0 { get; set; }

        [DisplayName("Dato 1")]
        [Description("Este es el dato 1")]
        [Format("D5")]
        public int Dato1
        {
            get { return this.dato1; }
            set { this.Set("Dato1", this.dato1, value, v => this.dato1 = v); }
        }

        private int dato1;

        [DisplayName("Dato 2")]
        [Description("Este es el dato 2")]
        [Format("F2")]
        public double Dato2
        {
            get { return this.dato2; }
            set { this.Set("Dato2", this.dato2, value, v => this.dato2 = v); }
        }

        private double dato2;

        [DisplayName("Dato 3")]
        [Description("Este es el dato 3")]
        [Category("Advanced")]
        public string Dato3
        {
            get { return this.dato3; }
            set { this.Set("Dato3", this.dato3, value, v => this.dato3 = v); }
        }

        private string dato3;

        [DisplayName("Dato 4")]
        [Description("Este es el dato 4")]
        [Category("Advanced")]
        public Guid Dato4
        {
            get { return this.dato4; }
            set { this.Set("Dato4", this.dato4, value, v => this.dato4 = v); }
        }

        private Guid dato4;

        [DisplayName("Dato 5")]
        [Description("Este es el dato 5")]
        [Category("UltraAdvanced")]
        [RefList(typeof(GetItemsDato5))]
        public string Dato5
        {
            get { return this.dato5; }
            set { this.Set("Dato5", this.dato5, value, v => this.dato5 = v); }
        }

        private string dato5 = "A";

        [DisplayName("Dato 6")]
        [Description("Este es el dato 6")]
        [Category("UltraAdvanced")]
        [Format("F2")]
        [RefList(typeof(GetItemsDato6))]
        public double Dato6
        {
            get { return this.dato6; }
            set { this.Set("Dato6", this.dato6, value, v => this.dato6 = v); }
        }

        private double dato6;

        private IEnumerable<double> ItemsDato6
        {
            get
            {
                return new double[]
                {
                    0, 1, 2, 3, 4, 5
                };
            }
        }

        private class GetItemsDato5 : AbsStaticRefList<string>
        {
            public override IEnumerable<string> GetItems(IServiceProvider proveedor)
            {
                return new[]
                {
                    "A", "B", "C", "D", "E"
                };
            }
        }

        private class GetItemsDato6 : AbsRefList<Prueba, double>
        {
            public override IEnumerable<double> GetItems(IServiceProvider proveedor, Prueba prueba)
            {
                return prueba.ItemsDato6;
            }
        }
    }
}