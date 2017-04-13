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
using System.Diagnostics.Contracts;
using Essence.Util;
using Essence.Util.Converters;
using Essence.Util.Properties;

namespace Essence.View.Models
{
    public class Property : AbsComponentUI,
                            IProperty
    {
        public const string VALUE = "Value";
        public const string ENABLED = "Enabled";
        public const string EDITABLE = "Editable";

        public Property(Type valuetype)
        {
            this.valuetype = valuetype;
        }

        #region private

        private IServiceProvider serviceProvider;

        private readonly Type valuetype;
        private object value;

        private bool enabled;
        private bool editable;

        #endregion

        #region IProperty

        public IServiceProvider ServiceProvider
        {
            get
            {
                if (this.serviceProvider != null)
                {
                    return this.serviceProvider;
                }
                return EmptyServiceProvider.Instance;
            }
            set { this.serviceProvider = value; }
        }

        public Type Valuetype
        {
            get { return this.valuetype; }
        }

        public object Value
        {
            get { return this.value; }
            set
            {
                Contract.Assert(this.Valuetype.IsInstanceOfType(value));
                if (!object.Equals(this.value, value))
                {
                    object oldValue = this.value;
                    this.value = value;
                    this.OnPropertyChanged(VALUE, oldValue, value);
                }
            }
        }

        public bool Enabled
        {
            get { return this.enabled; }
            set { this.Set(ref this.enabled, value); }
        }

        public bool Editable
        {
            get { return this.editable; }
            set { this.Set(ref this.editable, value); }
        }

        #endregion

        #region IFormattable

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (this.Value is IFormattable)
            {
                return ((IFormattable)this.Value).ToString(format, formatProvider);
            }
            if (this.Value != null)
            {
                return this.Value.ToString();
            }
            return "";
        }

        #endregion

        #region IParseable

        public bool TryParse(string text, string format, IFormatProvider formatProvider)
        {
            object result;
            if (!StringConverter.Instance.TryParse(text, format, formatProvider, this.Valuetype, out result))
            {
                return false;
            }
            this.Value = result;
            return true;
        }

        #endregion

        #region object

        public override string ToString()
        {
            return this.ToString("", null);
        }

        #endregion
    }

    public class Property<TValue> : Property
    {
        public Property()
            : base(typeof(TValue))
        {
        }

        public new TValue Value
        {
            get { return (TValue)base.Value; }
            set { base.Value = value; }
        }
    }
}