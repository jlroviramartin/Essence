using System;
using Essence.View.Models;
using Essence.View.Models.Properties;

namespace Essence.View.Controls
{
    public interface IEditorFactory
    {
        object Create(Type valueType, ViewProperties[] properties, IServiceProvider serviceProvider);
    }
}