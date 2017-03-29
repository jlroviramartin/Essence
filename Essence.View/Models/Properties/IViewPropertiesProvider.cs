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
using System.Reflection;
using Essence.View.Attributes;

namespace Essence.View.Models.Properties
{
    public interface IViewPropertiesProvider
    {
        IEnumerable<ViewProperties> GetProperties(object obj);

        IEnumerable<ViewProperties> GetProperties(object obj, string property);

        IEnumerable<ViewProperties> GetProperties(Type type);

        IEnumerable<ViewProperties> GetProperties(Type type, string property);
    }

    public class ReflectionViewPropertiesProvider : IViewPropertiesProvider
    {
        private const BindingFlags FLAGS = BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public;

        public IEnumerable<ViewProperties> GetProperties(object obj)
        {
            if (obj is null)
            {
                return new ViewProperties[0];
            }
            return this.GetProperties(obj.GetType());
        }

        public IEnumerable<ViewProperties> GetProperties(object obj, string property)
        {
            if (obj is null)
            {
                return new ViewProperties[0];
            }
            return this.GetProperties(obj.GetType(), property);
        }

        public IEnumerable<ViewProperties> GetProperties(Type type)
        {
            return type.GetCustomAttributes<ViewAttribute>().Select(va => va.GetViewProperties());
        }

        public IEnumerable<ViewProperties> GetProperties(Type type, string property)
        {
            PropertyInfo propertyInfo = type.GetProperty(property, FLAGS);
            Contract.Assert(propertyInfo != null);
            return propertyInfo.GetCustomAttributes<ViewAttribute>().Select(va => va.GetViewProperties());
        }
    }

    public static class ViewPropertiesProviderUtils
    {
        public static TViewProperties FindProperties<TViewProperties>(this IViewPropertiesProvider provider, object obj)
        {
            return provider.GetProperties(obj).OfType<TViewProperties>().FirstOrDefault();
        }

        public static TViewProperties FindProperties<TViewProperties>(this IViewPropertiesProvider provider, object obj, string property)
        {
            return provider.GetProperties(obj, property).OfType<TViewProperties>().FirstOrDefault();
        }
    }
}