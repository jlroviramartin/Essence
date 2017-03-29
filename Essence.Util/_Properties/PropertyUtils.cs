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
using Essence.Util.Events;

namespace Essence.Util.Properties
{
    public static class PropertyUtils
    {
        public static void NotifyPropertyChanged(this INotifyPropertyChangedEx_Helper sender, string name, object oldValue, object value)
        {
            sender.NotifyPropertyChanged(new PropertyChangedExEventArgs(name, oldValue, value));
        }

        #region Set classes

        /// <summary>
        ///     Establece la propiedad utilizando el operador <c>!=</c> para comprobar si
        ///     ha cambiado.
        ///     No se puede utilizar en estructuras.
        /// </summary>
        public static void SetCl<T>(this INotifyPropertyChangedEx_Helper @this,
                                    string nombre, T oldValue, T value,
                                    Action<T> setter) where T : class
        {
            if (oldValue != value)
            {
                setter(value);
                @this.NotifyPropertyChanged(new PropertyChangedExEventArgs(nombre, oldValue, value));
            }
        }

        /// <summary>
        ///     Establece la propiedad utilizando el método <c>Equals</c> para comprobar si
        ///     ha cambiado.
        ///     No se puede utilizar en estructuras.
        /// </summary>
        public static void SetEqCl<T>(this INotifyPropertyChangedEx_Helper @this,
                                      string nombre, T oldValue, T value,
                                      Action<T> setter) where T : class
        {
            if (!object.Equals(oldValue, value))
            {
                setter(value);
                @this.NotifyPropertyChanged(new PropertyChangedExEventArgs(nombre, oldValue, value));
            }
        }

        #endregion Set classes

        #region Set structs

        /// <summary>
        ///     Establece la propiedad utilizando el operador <c>!=</c> para comprobar si
        ///     ha cambiado.
        ///     Solo se puede utilizar en <c>bool</c>.
        /// </summary>
        public static void SetSt(this INotifyPropertyChangedEx_Helper @this,
                                 string nombre, bool oldValue, bool value,
                                 Action<bool> setter)
        {
            if (oldValue != value)
            {
                setter(value);
                @this.NotifyPropertyChanged(nombre, oldValue, value);
            }
        }

        /// <summary>
        ///     Establece la propiedad utilizando el operador <c>!=</c> para comprobar si
        ///     ha cambiado.
        ///     Solo se puede utilizar en <c>bool?</c>.
        /// </summary>
        public static void SetSt(this INotifyPropertyChangedEx_Helper @this,
                                 string nombre, bool? oldValue, bool? value,
                                 Action<bool?> setter)
        {
            if (oldValue != value)
            {
                setter(value);
                @this.NotifyPropertyChanged(nombre, oldValue, value);
            }
        }

        /// <summary>
        ///     Establece la propiedad utilizando el operador <c>!=</c> para comprobar si
        ///     ha cambiado.
        ///     Solo se puede utilizar en <c>char</c>.
        /// </summary>
        public static void SetSt(this INotifyPropertyChangedEx_Helper @this,
                                 string nombre, char oldValue, char value,
                                 Action<char> setter)
        {
            if (oldValue != value)
            {
                setter(value);
                @this.NotifyPropertyChanged(nombre, oldValue, value);
            }
        }

        /// <summary>
        ///     Establece la propiedad utilizando el operador <c>!=</c> para comprobar si
        ///     ha cambiado.
        ///     Solo se puede utilizar en <c>char?</c>.
        /// </summary>
        public static void SetSt(this INotifyPropertyChangedEx_Helper @this,
                                 string nombre, char? oldValue, char? value,
                                 Action<char?> setter)
        {
            if (oldValue != value)
            {
                setter(value);
                @this.NotifyPropertyChanged(nombre, oldValue, value);
            }
        }

        /// <summary>
        ///     Establece la propiedad utilizando el operador <c>!=</c> para comprobar si
        ///     ha cambiado.
        ///     Solo se puede utilizar en <c>sbyte</c>.
        /// </summary>
        public static void SetSt(this INotifyPropertyChangedEx_Helper @this,
                                 string nombre, sbyte oldValue, sbyte value,
                                 Action<sbyte> setter)
        {
            if (oldValue != value)
            {
                setter(value);
                @this.NotifyPropertyChanged(nombre, oldValue, value);
            }
        }

        /// <summary>
        ///     Establece la propiedad utilizando el operador <c>!=</c> para comprobar si
        ///     ha cambiado.
        ///     Solo se puede utilizar en <c>sbyte?</c>.
        /// </summary>
        public static void SetSt(this INotifyPropertyChangedEx_Helper @this,
                                 string nombre, sbyte? oldValue, sbyte? value,
                                 Action<sbyte?> setter)
        {
            if (oldValue != value)
            {
                setter(value);
                @this.NotifyPropertyChanged(nombre, oldValue, value);
            }
        }

        /// <summary>
        ///     Establece la propiedad utilizando el operador <c>!=</c> para comprobar si
        ///     ha cambiado.
        ///     Solo se puede utilizar en <c>short</c>.
        /// </summary>
        public static void SetSt(this INotifyPropertyChangedEx_Helper @this,
                                 string nombre, short oldValue, short value,
                                 Action<short> setter)
        {
            if (oldValue != value)
            {
                setter(value);
                @this.NotifyPropertyChanged(nombre, oldValue, value);
            }
        }

        /// <summary>
        ///     Establece la propiedad utilizando el operador <c>!=</c> para comprobar si
        ///     ha cambiado.
        ///     Solo se puede utilizar en <c>short?</c>.
        /// </summary>
        public static void SetSt(this INotifyPropertyChangedEx_Helper @this,
                                 string nombre, short? oldValue, short? value,
                                 Action<short?> setter)
        {
            if (oldValue != value)
            {
                setter(value);
                @this.NotifyPropertyChanged(nombre, oldValue, value);
            }
        }

        /// <summary>
        ///     Establece la propiedad utilizando el operador <c>!=</c> para comprobar si
        ///     ha cambiado.
        ///     Solo se puede utilizar en <c>int</c>.
        /// </summary>
        public static void SetSt(this INotifyPropertyChangedEx_Helper @this,
                                 string nombre, int oldValue, int value,
                                 Action<int> setter)
        {
            if (oldValue != value)
            {
                setter(value);
                @this.NotifyPropertyChanged(nombre, oldValue, value);
            }
        }

        /// <summary>
        ///     Establece la propiedad utilizando el operador <c>!=</c> para comprobar si
        ///     ha cambiado.
        ///     Solo se puede utilizar en <c>int?</c>.
        /// </summary>
        public static void SetSt(this INotifyPropertyChangedEx_Helper @this,
                                 string nombre, int? oldValue, int? value,
                                 Action<int?> setter)
        {
            if (oldValue != value)
            {
                setter(value);
                @this.NotifyPropertyChanged(nombre, oldValue, value);
            }
        }

        /// <summary>
        ///     Establece la propiedad utilizando el operador <c>!=</c> para comprobar si
        ///     ha cambiado.
        ///     Solo se puede utilizar en <c>long</c>.
        /// </summary>
        public static void SetSt(this INotifyPropertyChangedEx_Helper @this,
                                 string nombre, long oldValue, long value,
                                 Action<long> setter)
        {
            if (oldValue != value)
            {
                setter(value);
                @this.NotifyPropertyChanged(nombre, oldValue, value);
            }
        }

        /// <summary>
        ///     Establece la propiedad utilizando el operador <c>!=</c> para comprobar si
        ///     ha cambiado.
        ///     Solo se puede utilizar en <c>long?</c>.
        /// </summary>
        public static void SetSt(this INotifyPropertyChangedEx_Helper @this,
                                 string nombre, long? oldValue, long? value,
                                 Action<long?> setter)
        {
            if (oldValue != value)
            {
                setter(value);
                @this.NotifyPropertyChanged(nombre, oldValue, value);
            }
        }

        /// <summary>
        ///     Establece la propiedad utilizando el operador <c>!=</c> para comprobar si
        ///     ha cambiado.
        ///     Solo se puede utilizar en <c>byte</c>.
        /// </summary>
        public static void SetSt(this INotifyPropertyChangedEx_Helper @this,
                                 string nombre, byte oldValue, byte value,
                                 Action<byte> setter)
        {
            if (oldValue != value)
            {
                setter(value);
                @this.NotifyPropertyChanged(nombre, oldValue, value);
            }
        }

        /// <summary>
        ///     Establece la propiedad utilizando el operador <c>!=</c> para comprobar si
        ///     ha cambiado.
        ///     Solo se puede utilizar en <c>byte?</c>.
        /// </summary>
        public static void SetSt(this INotifyPropertyChangedEx_Helper @this,
                                 string nombre, byte? oldValue, byte? value,
                                 Action<byte?> setter)
        {
            if (oldValue != value)
            {
                setter(value);
                @this.NotifyPropertyChanged(nombre, oldValue, value);
            }
        }

        /// <summary>
        ///     Establece la propiedad utilizando el operador <c>!=</c> para comprobar si
        ///     ha cambiado.
        ///     Solo se puede utilizar en <c>ushort</c>.
        /// </summary>
        public static void SetSt(this INotifyPropertyChangedEx_Helper @this,
                                 string nombre, ushort oldValue, ushort value,
                                 Action<ushort> setter)
        {
            if (oldValue != value)
            {
                setter(value);
                @this.NotifyPropertyChanged(nombre, oldValue, value);
            }
        }

        /// <summary>
        ///     Establece la propiedad utilizando el operador <c>!=</c> para comprobar si
        ///     ha cambiado.
        ///     Solo se puede utilizar en <c>ushort?</c>.
        /// </summary>
        public static void SetSt(this INotifyPropertyChangedEx_Helper @this,
                                 string nombre, ushort? oldValue, ushort? value,
                                 Action<ushort?> setter)
        {
            if (oldValue != value)
            {
                setter(value);
                @this.NotifyPropertyChanged(nombre, oldValue, value);
            }
        }

        /// <summary>
        ///     Establece la propiedad utilizando el operador <c>!=</c> para comprobar si
        ///     ha cambiado.
        ///     Solo se puede utilizar en <c>uint</c>.
        /// </summary>
        public static void SetSt(this INotifyPropertyChangedEx_Helper @this,
                                 string nombre, uint oldValue, uint value,
                                 Action<uint> setter)
        {
            if (oldValue != value)
            {
                setter(value);
                @this.NotifyPropertyChanged(nombre, oldValue, value);
            }
        }

        /// <summary>
        ///     Establece la propiedad utilizando el operador <c>!=</c> para comprobar si
        ///     ha cambiado.
        ///     Solo se puede utilizar en <c>uint?</c>.
        /// </summary>
        public static void SetSt(this INotifyPropertyChangedEx_Helper @this,
                                 string nombre, uint? oldValue, uint? value,
                                 Action<uint?> setter)
        {
            if (oldValue != value)
            {
                setter(value);
                @this.NotifyPropertyChanged(nombre, oldValue, value);
            }
        }

        /// <summary>
        ///     Establece la propiedad utilizando el operador <c>!=</c> para comprobar si
        ///     ha cambiado.
        ///     Solo se puede utilizar en <c>ulong</c>.
        /// </summary>
        public static void SetSt(this INotifyPropertyChangedEx_Helper @this,
                                 string nombre, ulong oldValue, ulong value,
                                 Action<ulong> setter)
        {
            if (oldValue != value)
            {
                setter(value);
                @this.NotifyPropertyChanged(nombre, oldValue, value);
            }
        }

        /// <summary>
        ///     Establece la propiedad utilizando el operador <c>!=</c> para comprobar si
        ///     ha cambiado.
        ///     Solo se puede utilizar en <c>ulong?</c>.
        /// </summary>
        public static void SetSt(this INotifyPropertyChangedEx_Helper @this,
                                 string nombre, ulong? oldValue, ulong? value,
                                 Action<ulong?> setter)
        {
            if (oldValue != value)
            {
                setter(value);
                @this.NotifyPropertyChanged(nombre, oldValue, value);
            }
        }

        /// <summary>
        ///     Establece la propiedad utilizando el operador <c>!=</c> para comprobar si
        ///     ha cambiado.
        ///     Solo se puede utilizar en <c>uint</c>.
        /// </summary>
        public static void SetSt(this INotifyPropertyChangedEx_Helper @this,
                                 string nombre, float oldValue, float value,
                                 Action<float> setter)
        {
            if (oldValue != value)
            {
                setter(value);
                @this.NotifyPropertyChanged(nombre, oldValue, value);
            }
        }

        /// <summary>
        ///     Establece la propiedad utilizando el operador <c>!=</c> para comprobar si
        ///     ha cambiado.
        ///     Solo se puede utilizar en <c>uint?</c>.
        /// </summary>
        public static void SetSt(this INotifyPropertyChangedEx_Helper @this,
                                 string nombre, float? oldValue, float? value,
                                 Action<float?> setter)
        {
            if (oldValue != value)
            {
                setter(value);
                @this.NotifyPropertyChanged(nombre, oldValue, value);
            }
        }

        /// <summary>
        ///     Establece la propiedad utilizando el operador <c>!=</c> para comprobar si
        ///     ha cambiado.
        ///     Solo se puede utilizar en <c>uint</c>.
        /// </summary>
        public static void SetSt(this INotifyPropertyChangedEx_Helper @this,
                                 string nombre, double oldValue, double value,
                                 Action<double> setter)
        {
            if (oldValue != value)
            {
                setter(value);
                @this.NotifyPropertyChanged(nombre, oldValue, value);
            }
        }

        /// <summary>
        ///     Establece la propiedad utilizando el operador <c>!=</c> para comprobar si
        ///     ha cambiado.
        ///     Solo se puede utilizar en <c>uint?</c>.
        /// </summary>
        public static void SetSt(this INotifyPropertyChangedEx_Helper @this,
                                 string nombre, double? oldValue, double? value,
                                 Action<double?> setter)
        {
            if (oldValue != value)
            {
                setter(value);
                @this.NotifyPropertyChanged(nombre, oldValue, value);
            }
        }

        /// <summary>
        ///     Establece la propiedad utilizando el método <c>Equals</c> para comprobar si
        ///     ha cambiado.
        ///     Solo se puede utilizar en estructuras.
        /// </summary>
        public static void SetSt<T>(this INotifyPropertyChangedEx_Helper @this,
                                    string nombre, T oldValue, T value,
                                    Action<T> setter) where T : struct
        {
            if (!oldValue.Equals(value))
            {
                setter(value);
                @this.NotifyPropertyChanged(nombre, oldValue, value);
            }
        }

        /// <summary>
        ///     Establece la propiedad utilizando el método <c>Equals</c> para comprobar si
        ///     ha cambiado.
        ///     Solo se puede utilizar en estructuras.
        /// </summary>
        public static void SetSt<T>(this INotifyPropertyChangedEx_Helper @this,
                                    string nombre, T? oldValue, T? value,
                                    Action<T?> setter) where T : struct
        {
            if (!object.Equals(oldValue, value))
            {
                setter(value);
                @this.NotifyPropertyChanged(nombre, oldValue, value);
            }
        }

        #endregion Set structs

        /// <summary>
        /// </summary>
        public static void Set<T>(this INotifyPropertyChangedEx_Helper @this,
                                  string nombre, T oldValue, T value,
                                  Action<T> setter)
        {
            if (typeof(T).IsValueType)
            {
                if (!object.Equals(oldValue, value))
                {
                    setter(value);
                    @this.NotifyPropertyChanged(nombre, oldValue, value);
                }
            }
            else
            {
                if (!object.ReferenceEquals(oldValue, value))
                {
                    setter(value);
                    @this.NotifyPropertyChanged(nombre, oldValue, value);
                }
            }
        }

        /// <summary>
        /// </summary>
        public static void SetEq<T>(this INotifyPropertyChangedEx_Helper @this,
                                    string nombre, T oldValue, T value,
                                    Action<T> setter)
        {
            if (!object.Equals(oldValue, value))
            {
                setter(value);
                @this.NotifyPropertyChanged(nombre, oldValue, value);
            }
        }
    }
}