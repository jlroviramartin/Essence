using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace Essence.Util
{
    /// <summary>
    ///     Utilidades sobre tipos.
    /// </summary>
    public static class TypeUtils
    {
        public static T NewGeneric<T>(Type genericTypeDefinition, params Type[] typeArguments)
            where T : class
        {
            Contract.Assert((genericTypeDefinition != null) && genericTypeDefinition.IsGenericTypeDefinition);
            Type genericType = genericTypeDefinition.MakeGenericType(typeArguments);
            Contract.Assert((genericType != null) && typeof (T).IsAssignableFrom(genericType));

            return (T)Activator.CreateInstance(genericType);
        }

        public static T New<T>(this Type type)
            where T : class
        {
            Contract.Assert((type == null) || typeof (T).IsAssignableFrom(type));
            return (type != null) ? (T)Activator.CreateInstance(type) : null;
        }

        public static object New(this Type type)
        {
            return (type != null) ? Activator.CreateInstance(type) : null;
        }

        /// <summary>
        /// Crea una instancia del la clase indicada, completamente vacia.
        /// </summary>
        /// <typeparam name="T">Tipo de la clase a crear instancia.</typeparam>
        /// <returns>Instancia.</returns>
        public static T NewEmpty<T>()
        {
            return (T)FormatterServices.GetSafeUninitializedObject(typeof (T));
        }

        /// <summary>
        /// Crea una instancia del la clase indicada, completamente vacia.
        /// </summary>
        /// <returns>Instancia.</returns>
        public static object NewEmpty(this Type type)
        {
            return FormatterServices.GetSafeUninitializedObject(type);
        }

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

        public static object GetDefault(this Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }

        /// <summary>
        ///     Obtiene el tamaño del tipo. No es aplicable a objetos ni structuras.
        /// </summary>
        /// <param name="primitiveType"></param>
        /// <returns></returns>
        public static int GetSize(this Type primitiveType)
        {
            if (primitiveType.IsEnum)
            {
                return Type.GetTypeCode(primitiveType.GetEnumUnderlyingType()).GetSize();
            }
            return Type.GetTypeCode(primitiveType).GetSize();
        }

        public static int GetSize(this TypeCode typeCode)
        {
            int size;
            typeCodeToSize.TryGetValue(typeCode, out size);
            return size;
        }

        /// <summary>
        ///     Convert a TypeCode ordinal into it's corresponding Type instance.
        ///     <see cref="http://stackoverflow.com/questions/5915740/how-to-convert-a-typecode-to-an-actual-type" />
        /// </summary>
        public static Type ToType(this TypeCode typeCode)
        {
            Type type;
            typeCodeToTypeMap.TryGetValue(typeCode, out type);
            return type;
        }

        #region private

        /// <summary>
        ///     Table that maps TypeCode to it's corresponding Type.
        /// </summary>
        private static readonly Dictionary<TypeCode, Type> typeCodeToTypeMap = new Dictionary<TypeCode, Type>
        {
            { TypeCode.Boolean, typeof (bool) },
            { TypeCode.Byte, typeof (byte) },
            { TypeCode.Char, typeof (char) },
            { TypeCode.DateTime, typeof (DateTime) },
            { TypeCode.DBNull, typeof (DBNull) },
            { TypeCode.Decimal, typeof (decimal) },
            { TypeCode.Double, typeof (double) },
            { TypeCode.Empty, null },
            { TypeCode.Int16, typeof (short) },
            { TypeCode.Int32, typeof (int) },
            { TypeCode.Int64, typeof (long) },
            { TypeCode.Object, typeof (object) },
            { TypeCode.SByte, typeof (sbyte) },
            { TypeCode.Single, typeof (float) },
            { TypeCode.String, typeof (string) },
            { TypeCode.UInt16, typeof (ushort) },
            { TypeCode.UInt32, typeof (uint) },
            { TypeCode.UInt64, typeof (ulong) }
        };

        /// <summary>
        ///     Table that maps TypeCode to it's corresponding Type.
        /// </summary>
        private static readonly Dictionary<TypeCode, int> typeCodeToSize = new Dictionary<TypeCode, int>
        {
            { TypeCode.Boolean, sizeof(bool) },
            { TypeCode.Byte, sizeof(byte) },
            { TypeCode.Char, sizeof(char) },
            //{ TypeCode.DateTime, sizeof (DateTime) },
            //{ TypeCode.DBNull, sizeof (DBNull) },
            { TypeCode.Decimal, sizeof(decimal) },
            { TypeCode.Double, sizeof(double) },
            //{ TypeCode.Empty, null },
            { TypeCode.Int16, sizeof(short) },
            { TypeCode.Int32, sizeof(int) },
            { TypeCode.Int64, sizeof(long) },
            //{ TypeCode.Object, sizeof (object) },
            { TypeCode.SByte, sizeof(sbyte) },
            { TypeCode.Single, sizeof(float) },
            //{ TypeCode.String, sizeof (string) },
            { TypeCode.UInt16, sizeof(ushort) },
            { TypeCode.UInt32, sizeof(uint) },
            { TypeCode.UInt64, sizeof(ulong) }
        };

        #endregion
    }
}