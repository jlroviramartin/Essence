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
    ///     Utilidades sobre tipos.
    /// </summary>
    public static class TypeUtils
    {
        /// <summary>
        ///     Indica si el valor es <c>null</c> o si es <c>nullable</c> y no tiene valor.
        /// </summary>
        /// <typeparam name="T">Tipo del valor.</typeparam>
        /// <param name="value">Valor.</param>
        /// <returns>Indica si es <c>null</c>.</returns>
        public static bool IsNull<T>(T value)
        {
            return (value == null);
        }

        /// <summary>
        ///     Indica si el tipo admite null (no es ValueType o es Nullable).
        /// </summary>
        /// <param name="type">Tipo.</param>
        /// <returns>Indica si el tipo admite null.</returns>
        public static bool NullAdmitted(this Type type)
        {
            //type.IsClass || type.IsInterface
            return ((!type.IsValueType && !type.IsGenericTypeDefinition) || type.IsNullable());
        }

        /// <summary>
        ///     Indica si es Nullable el tipo indicado.
        /// </summary>
        /// <param name="type">Tipo.</param>
        /// <returns>Indica si es Nullable.</returns>
        public static bool IsNullable(this Type type)
        {
            return (type.IsGenericType && !type.IsGenericTypeDefinition && (typeof (Nullable<>) == type.GetGenericTypeDefinition()));
        }

        /// <summary>
        ///     Indica si el tipo es un enumerado Nullable.
        /// </summary>
        /// <param name="type">Tipo.</param>
        /// <returns>Indica si es un enumerado Nullable.</returns>
        public static bool IsNullableEnum(this Type type)
        {
            return (type.IsNullable() && Nullable.GetUnderlyingType(type).IsEnum);
        }
    }
}