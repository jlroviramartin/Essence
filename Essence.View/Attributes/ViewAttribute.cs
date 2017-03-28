using System;
using Essence.View.Models;
using Essence.View.Models.Properties;

namespace Essence.View.Attributes
{
    public abstract class ViewAttribute : Attribute
    {
        public abstract ViewProperties GetViewProperties();
    }
}