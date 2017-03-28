using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using Essence.Util;
using Essence.Util.Collections;
using Essence.Util.Events;
using Essence.View.Attributes;
using Essence.View.Models.Properties;

namespace Essence.View.Models
{
    public class ReflectionFormModel : AbsFormModel,
                                       IRefFormObject
    {
        public const string DEFAULT_CATEGORY = "Default";

        public const string FORM_TYPE = "FormType";
        public const string FORM_OBJECT = "FormObject";

        public Type FormType
        {
            get { return this.formType; }
            set
            {
                if (!object.Equals(this.formType, value))
                {
                    Type oldValue = this.formType;
                    this.formType = value;
                    this.OnPropertyChanged(FORM_TYPE, oldValue, value);

                    this.OnStructureChanged(EventArgs.Empty);
                }
            }
        }

        #region private

        private const BindingFlags FLAGS = BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public;

        private PropertyInfo GetPropertyInfo(string name)
        {
            if (this.FormType == null)
            {
                return null;
            }
            return this.FormType.GetProperty(name, FLAGS);
        }

        private PropertyDescriptor GetPropertyDescriptor(string name)
        {
            if (this.FormType == null)
            {
                return null;
            }
            return TypeDescriptor.GetProperties(this.formType)[name];
        }

        private string GetCategory(string name)
        {
            PropertyInfo propertyInfo = this.GetPropertyInfo(name);
            Contract.Assert(propertyInfo != null);
            return propertyInfo.GetCustomAttributes<CategoryAttribute>().Select(a => a.Category).FirstOrDefault() ?? DEFAULT_CATEGORY;
        }

        private bool IsBrowsable(string name)
        {
            PropertyInfo propertyInfo = this.GetPropertyInfo(name);
            Contract.Assert(propertyInfo != null);
            return propertyInfo.GetCustomAttributes<BrowsableAttribute>().Select(a => a.Browsable).FirstOrDefault(true);
        }

        private void FormObject_PropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args is PropertyChangedExEventArgs)
            {
                this.OnPropertyChanged((PropertyChangedExEventArgs)args);
            }
            else
            {
                object value = this.GetPropertyValue(args.PropertyName);
                this.OnPropertyChanged(args.PropertyName, value, value);
            }
        }

        private Type formType;
        private object formObject;

        #endregion

        #region IRefFormObject

        public object FormObject
        {
            get { return this.formObject; }
            set
            {
                Contract.Ensures((value == null) || this.FormType.IsInstanceOfType(value));
                if (this.formObject != value)
                {
                    /*Dictionary<string, object> oldValues = new Dictionary<string, object>();
                    foreach (string name in this.GetProperties())
                    {
                        oldValues.Add(name, this.GetPropertyValue(name));
                    }*/

                    if (this.formObject is INotifyPropertyChanged)
                    {
                        ((INotifyPropertyChanged)this.formObject).PropertyChanged -= this.FormObject_PropertyChanged;
                    }

                    object oldValue = this.formObject;
                    this.formObject = value;

                    if (this.formObject is INotifyPropertyChanged)
                    {
                        ((INotifyPropertyChanged)this.formObject).PropertyChanged += this.FormObject_PropertyChanged;
                    }

                    /*foreach (string name in this.GetProperties())
                    {
                        object foldValue = oldValues.GetSafe(name);
                        object fvalue = this.GetPropertyValue(name);
                        if (!object.Equals(foldValue, fvalue))
                        {
                            this.OnPropertyChanged(name, foldValue, fvalue);
                        }
                    }*/

                    this.OnContentChanged(EventArgs.Empty);
                    this.OnPropertyChanged(FORM_OBJECT, oldValue, value);
                }
            }
        }

        #endregion

        #region IFormModel

        public override IEnumerable<string> GetProperties()
        {
            if (this.FormType == null)
            {
                yield break;
            }
            foreach (PropertyInfo propertyInfo in this.FormType.GetProperties(FLAGS))
            {
                if (propertyInfo.GetCustomAttributes<BrowsableAttribute>().Select(a => a.Browsable).FirstOrDefault(true))
                {
                    yield return propertyInfo.Name;
                }
            }
        }

        public override IEnumerable<string> GetCategories()
        {
            if (this.FormType == null)
            {
                return new string[0];
            }
            HashSet<string> categories = new HashSet<string>();
            categories.Add(DEFAULT_CATEGORY);
            foreach (PropertyInfo propertyInfo in this.FormType.GetProperties(FLAGS))
            {
                if (propertyInfo.GetCustomAttributes<BrowsableAttribute>().Select(a => a.Browsable).FirstOrDefault(true))
                {
                    string category = propertyInfo.GetCustomAttributes<CategoryAttribute>().Select(a => a.Category).FirstOrDefault();
                    if (category != null)
                    {
                        categories.Add(category);
                    }
                }
            }
            return categories;
        }

        public override IEnumerable<string> GetProperties(string category)
        {
            return this.GetProperties().Where(name => object.Equals(category, this.GetCategory(name)));
        }

        public override Type GetPropertyType(string name)
        {
            PropertyInfo propertyInfo = this.GetPropertyInfo(name);
            Contract.Assert(propertyInfo != null);
            return propertyInfo.PropertyType;
        }

        public override object GetPropertyValue(string name)
        {
            if (this.FormObject == null)
            {
                Type propType = this.GetPropertyType(name);
                return propType.GetDefault();
            }

            PropertyInfo propertyInfo = this.FormType.GetProperty(name);
            Contract.Assert(propertyInfo != null);
            return propertyInfo.GetValue(this.FormObject);
        }

        public override void SetPropertyValue(string name, object value)
        {
            if (this.FormObject == null)
            {
                return;
            }

            PropertyInfo propertyInfo = this.FormType.GetProperty(name);
            Contract.Assert(propertyInfo != null);
            propertyInfo.SetValue(this.FormObject, value);
        }

        public override IEnumerable<ViewProperties> GetViewProperties(string name)
        {
            PropertyInfo propertyInfo = this.GetPropertyInfo(name);
            Contract.Assert(propertyInfo != null);
            return propertyInfo.GetCustomAttributes<ViewAttribute>().Select(va => va.GetViewProperties());
        }

        public override PropertyDescription GetDescription(string name)
        {
            PropertyInfo propertyInfo = this.GetPropertyInfo(name);
            Contract.Assert(propertyInfo != null);
            return new PropertyDescription()
            {
                Category = propertyInfo.GetCustomAttributes<CategoryAttribute>().Select(a => a.Category).FirstOrDefault() ?? DEFAULT_CATEGORY,
                Name = name,
                Label = propertyInfo.GetCustomAttributes<DisplayNameAttribute>().Select(a => a.DisplayName).FirstOrDefault() ?? name,
                Description = propertyInfo.GetCustomAttributes<DescriptionAttribute>().Select(a => a.Description).FirstOrDefault() ?? name,
            };
        }

        #endregion
    }
}