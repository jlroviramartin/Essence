using System;
using System.Collections;

namespace Essence.View.Models.Properties
{
    public class FormatProperties : ViewProperties
    {
        public static FormatProperties Empty
        {
            get { return new FormatProperties(); }
        }

        /// <summary>Formato.</summary>
        public string Format { get; set; }

        /// <summary>Proveedor de formato.</summary>
        public IFormatProvider FormatProvider { get; set; }

        public ICustomFormatter CustomFormatter { get; set; }

        public IEqualityComparer EqualityComparer { get; set; }
    }
}