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

namespace Essence.Util.Logs
{
    /// <summary>
    ///     Log genérico similar a <c>log4net.ILog</c>.
    /// </summary>
    public interface ILog
    {
        bool IsEnabled(LogType type);

        void Write(LogType type, string message);

        void Write(LogType type, string message, Exception exception);

        void WriteFormat(LogType type, string format, params object[] args);

        void WriteFormat(LogType type, IFormatProvider provider, string format, params object[] args);

        /// <summary>
        ///     Indica si el log de debug esta habilitado.
        /// </summary>
        bool IsDebugEnabled { get; }

        /// <summary>
        ///     Muestra <c>message</c> como información de debug.
        /// </summary>
        /// <param name="message">Mensaje.</param>
        void Debug(string message);

        /// <summary>
        ///     Muestra <c>message</c> y la excepción <c>exception</c>
        ///     como información de debug.
        /// </summary>
        /// <param name="message">Mensaje.</param>
        /// <param name="exception">Excepción.</param>
        void Debug(string message, Exception exception);

        /// <summary>
        ///     Muestra el mansaje formateado <c>format</c> con los
        ///     argumentos <c>args</c> como información de debug.
        /// </summary>
        /// <param name="format">Formato.</param>
        /// <param name="args">Argumentos.</param>
        void DebugFormat(string format, params object[] args);

        /// <summary>
        ///     Muestra el mansaje formateado <c>format</c> con los
        ///     argumentos <c>args</c> y el proveedor de formato
        ///     <c>provider</c>, como información de debug.
        /// </summary>
        /// <param name="provider">Proveedor.</param>
        /// <param name="format">Formato.</param>
        /// <param name="args">Argumentos.</param>
        void DebugFormat(IFormatProvider provider, string format, params object[] args);

        /// <summary>
        ///     Indica si el log de error esta habilitado.
        /// </summary>
        bool IsErrorEnabled { get; }

        /// <summary>
        ///     Muestra <c>message</c> como información de error.
        /// </summary>
        /// <param name="message">Mensaje</param>
        void Error(string message);

        /// <summary>
        ///     Muestra <c>message</c> y la excepcion <c>exception</c>
        ///     como información de error.
        /// </summary>
        /// <param name="message">Mensaje.</param>
        /// <param name="exception">Excepción.</param>
        void Error(string message, Exception exception);

        /// <summary>
        ///     Muestra el mansaje formateado <c>format</c> con los
        ///     argumentos <c>args</c> como información de error.
        /// </summary>
        /// <param name="format">Formato.</param>
        /// <param name="args">Argumentos.</param>
        void ErrorFormat(string format, params object[] args);

        /// <summary>
        ///     Muestra el mansaje formateado <c>format</c> con los
        ///     argumentos <c>args</c> y el proveedor de formato
        ///     <c>provider</c>, como información de error.
        /// </summary>
        /// <param name="provider">Proveedor.</param>
        /// <param name="format">Formato.</param>
        /// <param name="args">Argumentos.</param>
        void ErrorFormat(IFormatProvider provider, string format, params object[] args);

        /// <summary>
        ///     Indica si el log de error fatal esta habilitado.
        /// </summary>
        bool IsFatalEnabled { get; }

        /// <summary>
        ///     Muestra <c>message</c> como información de error fatal.
        /// </summary>
        /// <param name="message">Mensaje</param>
        void Fatal(string message);

        /// <summary>
        ///     Muestra <c>message</c> y la excepcion <c>exception</c>
        ///     como información de error fatal.
        /// </summary>
        /// <param name="message">Mensaje.</param>
        /// <param name="exception">Excepción.</param>
        void Fatal(string message, Exception exception);

        /// <summary>
        ///     Muestra el mansaje formateado <c>format</c> con los
        ///     argumentos <c>args</c> como información de error fatal.
        /// </summary>
        /// <param name="format">Formato.</param>
        /// <param name="args">Argumentos.</param>
        void FatalFormat(string format, params object[] args);

        /// <summary>
        ///     Muestra el mansaje formateado <c>format</c> con los
        ///     argumentos <c>args</c> y el proveedor de formato
        ///     <c>provider</c>, como información de error fatal.
        /// </summary>
        /// <param name="provider">Proveedor.</param>
        /// <param name="format">Formato.</param>
        /// <param name="args">Argumentos.</param>
        void FatalFormat(IFormatProvider provider, string format, params object[] args);

        /// <summary>
        ///     Indica si el log de debug esta habilitado.
        /// </summary>
        bool IsInfoEnabled { get; }

        /// <summary>
        ///     Muestra <c>message</c> como información.
        /// </summary>
        /// <param name="message">Mensaje</param>
        void Info(string message);

        /// <summary>
        ///     Muestra <c>message</c> y la excepcion <c>exception</c>
        ///     como información.
        /// </summary>
        /// <param name="message">Mensaje.</param>
        /// <param name="exception">Excepción.</param>
        void Info(string message, Exception exception);

        /// <summary>
        ///     Muestra el mansaje formateado <c>format</c> con los
        ///     argumentos <c>args</c> como información.
        /// </summary>
        /// <param name="format">Formato.</param>
        /// <param name="args">Argumentos.</param>
        void InfoFormat(string format, params object[] args);

        /// <summary>
        ///     Muestra el mansaje formateado <c>format</c> con los
        ///     argumentos <c>args</c> y el proveedor de formato
        ///     <c>provider</c>, como información.
        /// </summary>
        /// <param name="provider">Proveedor.</param>
        /// <param name="format">Formato.</param>
        /// <param name="args">Argumentos.</param>
        void InfoFormat(IFormatProvider provider, string format, params object[] args);

        /// <summary>
        ///     Indica si el log de advertencia esta habilitado.
        /// </summary>
        bool IsWarnEnabled { get; }

        /// <summary>
        ///     Muestra <c>message</c> como información de advertencia.
        /// </summary>
        /// <param name="message">Mensaje</param>
        void Warn(string message);

        /// <summary>
        ///     Muestra <c>message</c> y la excepcion <c>exception</c>
        ///     como información de advertencia.
        /// </summary>
        /// <param name="message">Mensaje.</param>
        /// <param name="exception">Excepción.</param>
        void Warn(string message, Exception exception);

        /// <summary>
        ///     Muestra el mansaje formateado <c>format</c> con los
        ///     argumentos <c>args</c> como información de advertencia.
        /// </summary>
        /// <param name="format">Formato.</param>
        /// <param name="args">Argumentos.</param>
        void WarnFormat(string format, params object[] args);

        /// <summary>
        ///     Muestra el mansaje formateado <c>format</c> con los
        ///     argumentos <c>args</c> y el proveedor de formato
        ///     <c>provider</c>, como información de advertencia.
        /// </summary>
        /// <param name="provider">Proveedor.</param>
        /// <param name="format">Formato.</param>
        /// <param name="args">Argumentos.</param>
        void WarnFormat(IFormatProvider provider, string format, params object[] args);
    }
}