using System;
using System.Collections;
using Essence.Util;
using Essence.View.Models;
using Essence.View.Models.Properties;

namespace Essence.View.Attributes
{
    public class FormatAttribute : ViewAttribute
    {
        public FormatAttribute()
        {
        }

        public FormatAttribute(string format)
        {
            this.Format = format;
        }

        public string Format { get; set; }
        public Type FormatProvider { get; set; }
        public Type CustomFormatter { get; set; }
        public Type EqualityComparer { get; set; }

        #region ViewAttribute

        public override ViewProperties GetViewProperties()
        {
            return new FormatProperties()
            {
                Format = this.Format,
                FormatProvider = this.FormatProvider.New<IFormatProvider>(),
                CustomFormatter = this.CustomFormatter.New<ICustomFormatter>(),
                EqualityComparer = this.EqualityComparer.New<IEqualityComparer>()
            };
        }

        #endregion
    }
}