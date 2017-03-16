#region License

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

#endregion

using System;

namespace Essence.Util
{
    /// <summary>
    ///     Utilidades sobre proveedores de formato <c>IFormatProvider</c>.
    /// </summary>
    public static class ProviderUtils
    {
        /// <summary>
        ///     Obtiene el formato asociado al tipo del parametro generico.
        /// </summary>
        /// <typeparam name="T">Tipo de formato.</typeparam>
        /// <param name="provider">Proveedor.</param>
        /// <returns>Formato.</returns>
        public static T GetFormat<T>(this IFormatProvider provider)
        {
            return (T)provider.GetFormat(typeof (T));
        }
    }
}