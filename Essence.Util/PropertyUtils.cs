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
    ///     Utilidades sobre propiedades.
    /// </summary>
    public static class PropertyUtils
    {
        /// <summary>
        ///     Si la entrada no es <c>null</c>, realiza la evalucion sobre la entrada
        ///     y devuelve el resultado. En otro caso devuelve <c>null</c>. Equivale a:
        ///     <code><![CDATA[
        /// if (o == null)
        /// {
        ///     return null;
        /// }
        /// return evaluator(o);
        /// ]]></code>
        /// </summary>
        /// <typeparam name="TInput">Tipo de entrada.</typeparam>
        /// <typeparam name="TResult">Tipo de resultado.</typeparam>
        /// <param name="o">entrada.</param>
        /// <param name="evaluator">Evaluador para la entrada.</param>
        /// <returns>Resultado de la evaluacion.</returns>
        public static TResult With<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator)
            where TResult : class
            where TInput : class
        {
            if (o == null)
            {
                return null;
            }
            return evaluator(o);
        }

        /// <summary>
        ///     Si la entrada no es <c>null</c>, realiza la evalucion sobre la entrada
        ///     y devuelve el resultado. En otro caso devuelve el valor de fallo. Equivale a:
        ///     <code><![CDATA[
        /// if (o == null)
        /// {
        ///     return failureValue;
        /// }
        /// return evaluator(o);
        /// ]]></code>
        /// </summary>
        /// <typeparam name="TInput">Tipo de entrada.</typeparam>
        /// <typeparam name="TResult">Tipo de resultado.</typeparam>
        /// <param name="o">entrada.</param>
        /// <param name="evaluator">Evaluador para la entrada.</param>
        /// <param name="failureValue">Resultado si null.</param>
        /// <returns>Resultado de la evaluacion.</returns>
        public static TResult With<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator,
                                                    TResult failureValue)
            where TInput : class
        {
            if (o == null)
            {
                return failureValue;
            }
            return evaluator(o);
        }

        public static TInput If<TInput>(this TInput o, Func<TInput, bool> evaluator)
            where TInput : class
        {
            if (o == null)
            {
                return null;
            }
            return evaluator(o) ? o : null;
        }

        public static TInput Unless<TInput>(this TInput o, Func<TInput, bool> evaluator)
            where TInput : class
        {
            if (o == null)
            {
                return null;
            }
            return evaluator(o) ? null : o;
        }

        /// <summary>
        ///     Si la entrada no es <c>null</c>, realiza una accion sobre la
        ///     entrada. Permite encadenar multiples acciones sobre la misma
        ///     entrada. Equivale a:
        ///     <code><![CDATA[
        /// if (o == null)
        /// {
        ///     return null;
        /// }
        /// action(o);
        /// return o;
        /// ]]></code>
        /// </summary>
        /// <typeparam name="TInput">Tipo de entrada.</typeparam>
        /// <typeparam name="TResult">Tipo de resultado.</typeparam>
        /// <param name="o">entrada.</param>
        /// <param name="evaluator">Accion para la entrada.</param>
        /// <returns>Entrada.</returns>
        public static TInput Do<TInput>(this TInput o, Action<TInput> action)
            where TInput : class
        {
            if (o == null)
            {
                return null;
            }
            action(o);
            return o;
        }

        /*public static IElement IsWithin<TContainingType>(this IElement self)
          where TContainingType : class, IElement
        {
            if (self == null)
                return self;
            var owner = self.GetContainingElement<TContainingType>(false);
            return owner == null ? self : null;
        }*/

        public static TResult WithValue<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator)
            where TInput : struct
        {
            return evaluator(o);
        }
    }
}