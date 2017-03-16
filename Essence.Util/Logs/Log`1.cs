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
using Essence.Util.Properties;

namespace Essence.Util.Logs
{
    /// <summary>
    ///     Clase de ayuda para los logs. En caso de que el tipo no importe, usar Log@lt;object@gt;.
    /// </summary>
    /// <typeparam name="T">Tipo para el que se asocia el log.</typeparam>
    public static class Log<T>
    {
        /// <summary>
        ///     Indica si el log esta activo.
        /// </summary>
        public static bool LogActivo
        {
            get { return logActivo; }
            set { logActivo = value; }
        }

        /// <summary>
        ///     Obtiene/establece el log actual.
        /// </summary>
        public static ILog LogActual
        {
            get { return logActual; }
        }

        /// <summary>
        ///     Indica si el log de debug esta habilitado.
        /// </summary>
        public static bool IsEnabled(LogType type)
        {
            ILog log = GetLog();
            return ((log != null) ? log.IsEnabled(type) : false);
        }

        /// <summary>
        ///     Muestra <c>message</c> como información de debug.
        /// </summary>
        /// <param name="message">Mensaje.</param>
        public static void Write(LogType type, string message)
        {
            ILog log = GetLog();
            if (log != null)
            {
                log.Write(type, message);
            }
        }

        /// <summary>
        ///     Muestra <c>message</c> y la excepción <c>exception</c>
        ///     como información de debug.
        /// </summary>
        /// <param name="message">Mensaje.</param>
        /// <param name="exception">Excepción.</param>
        public static void Write(LogType type, string message, Exception exception)
        {
            ILog log = GetLog();
            if (log != null)
            {
                log.Write(type, message, exception);
            }
        }

        /// <summary>
        ///     Muestra <c>message</c> y la excepción <c>exception</c>
        ///     como información de debug.
        /// </summary>
        /// <param name="exception">Excepción.</param>
        public static void Write(LogType type, Exception exception)
        {
            ILog log = GetLog();
            if (log != null)
            {
                log.Write(type, "", exception);
            }
        }

        /// <summary>
        ///     Muestra el mansaje formateado <c>format</c> con los
        ///     argumentos <c>args</c> como información de debug.
        /// </summary>
        /// <param name="format">Formato.</param>
        /// <param name="args">Argumentos.</param>
        [StringFormatMethod("format")]
        public static void WriteFormat(LogType type, string format, params object[] args)
        {
            ILog log = GetLog();
            if (log != null)
            {
                log.WriteFormat(type, format, args);
            }
        }

        /// <summary>
        ///     Muestra el mansaje formateado <c>format</c> con los
        ///     argumentos <c>args</c> y el proveedor de formato
        ///     <c>provider</c>, como información de debug.
        /// </summary>
        /// <param name="provider">Proveedor.</param>
        /// <param name="format">Formato.</param>
        /// <param name="args">Argumentos.</param>
        [StringFormatMethod("format")]
        public static void WriteFormat(LogType type, IFormatProvider provider, string format, params object[] args)
        {
            ILog log = GetLog();
            if (log != null)
            {
                log.WriteFormat(type, provider, format, args);
            }
        }

        /// <summary>
        ///     Indica si el log de debug esta habilitado.
        /// </summary>
        public static bool IsDebugEnabled
        {
            get
            {
                ILog log = GetLog();
                return ((log != null) ? log.IsDebugEnabled : false);
            }
        }

        /// <summary>
        ///     Muestra <c>message</c> como información de debug.
        /// </summary>
        /// <param name="message">Mensaje.</param>
        public static void Debug(string message)
        {
            ILog log = GetLog();
            if (log != null)
            {
                log.Debug(message);
            }
        }

        /// <summary>
        ///     Muestra <c>message</c> y la excepción <c>exception</c>
        ///     como información de debug.
        /// </summary>
        /// <param name="message">Mensaje.</param>
        /// <param name="exception">Excepción.</param>
        public static void Debug(string message, Exception exception)
        {
            ILog log = GetLog();
            if (log != null)
            {
                log.Debug(message, exception);
            }
        }

        /// <summary>
        ///     Muestra <c>message</c> y la excepción <c>exception</c>
        ///     como información de debug.
        /// </summary>
        /// <param name="exception">Excepción.</param>
        public static void Debug(Exception exception)
        {
            ILog log = GetLog();
            if (log != null)
            {
                log.Debug("", exception);
            }
        }

        /// <summary>
        ///     Muestra el mansaje formateado <c>format</c> con los
        ///     argumentos <c>args</c> como información de debug.
        /// </summary>
        /// <param name="format">Formato.</param>
        /// <param name="args">Argumentos.</param>
        [StringFormatMethod("format")]
        public static void DebugFormat(string format, params object[] args)
        {
            ILog log = GetLog();
            if (log != null)
            {
                log.DebugFormat(format, args);
            }
        }

        /// <summary>
        ///     Muestra el mansaje formateado <c>format</c> con los
        ///     argumentos <c>args</c> y el proveedor de formato
        ///     <c>provider</c>, como información de debug.
        /// </summary>
        /// <param name="provider">Proveedor.</param>
        /// <param name="format">Formato.</param>
        /// <param name="args">Argumentos.</param>
        [StringFormatMethod("format")]
        public static void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            ILog log = GetLog();
            if (log != null)
            {
                log.DebugFormat(provider, format, args);
            }
        }

        /// <summary>
        ///     Indica si el log de error esta habilitado.
        /// </summary>
        public static bool IsErrorEnabled
        {
            get
            {
                ILog log = GetLog();
                return ((log != null) ? log.IsErrorEnabled : false);
            }
        }

        /// <summary>
        ///     Muestra <c>message</c> como información de error.
        /// </summary>
        /// <param name="message">Mensaje</param>
        public static void Error(string message)
        {
            ILog log = GetLog();
            if (log != null)
            {
                log.Error(message);
            }
        }

        /// <summary>
        ///     Muestra <c>message</c> y la excepcion <c>exception</c>
        ///     como información de error.
        /// </summary>
        /// <param name="message">Mensaje.</param>
        /// <param name="exception">Excepción.</param>
        public static void Error(string message, Exception exception)
        {
            ILog log = GetLog();
            if (log != null)
            {
                log.Error(message, exception);
            }
        }

        /// <summary>
        ///     Muestra <c>message</c> y la excepcion <c>exception</c>
        ///     como información de error.
        /// </summary>
        /// <param name="exception">Excepción.</param>
        public static void Error(Exception exception)
        {
            ILog log = GetLog();
            if (log != null)
            {
                log.Error("", exception);
            }
        }

        /// <summary>
        ///     Muestra el mansaje formateado <c>format</c> con los
        ///     argumentos <c>args</c> como información de error.
        /// </summary>
        /// <param name="format">Formato.</param>
        /// <param name="args">Argumentos.</param>
        [StringFormatMethod("format")]
        public static void ErrorFormat(string format, params object[] args)
        {
            ILog log = GetLog();
            if (log != null)
            {
                log.ErrorFormat(format, args);
            }
        }

        /// <summary>
        ///     Muestra el mansaje formateado <c>format</c> con los
        ///     argumentos <c>args</c> y el proveedor de formato
        ///     <c>provider</c>, como información de error.
        /// </summary>
        /// <param name="provider">Proveedor.</param>
        /// <param name="format">Formato.</param>
        /// <param name="args">Argumentos.</param>
        [StringFormatMethod("format")]
        public static void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            ILog log = GetLog();
            if (log != null)
            {
                log.ErrorFormat(provider, format, args);
            }
        }

        /// <summary>
        ///     Indica si el log de error fatal esta habilitado.
        /// </summary>
        public static bool IsFatalEnabled
        {
            get
            {
                ILog log = GetLog();
                return ((log != null) ? log.IsFatalEnabled : false);
            }
        }

        /// <summary>
        ///     Muestra <c>message</c> como información de error fatal.
        /// </summary>
        /// <param name="message">Mensaje</param>
        public static void Fatal(string message)
        {
            ILog log = GetLog();
            if (log != null)
            {
                log.Fatal(message);
            }
        }

        /// <summary>
        ///     Muestra <c>message</c> y la excepcion <c>exception</c>
        ///     como información de error fatal.
        /// </summary>
        /// <param name="message">Mensaje.</param>
        /// <param name="exception">Excepción.</param>
        public static void Fatal(string message, Exception exception)
        {
            ILog log = GetLog();
            if (log != null)
            {
                log.Fatal(message, exception);
            }
        }

        /// <summary>
        ///     Muestra <c>message</c> y la excepcion <c>exception</c>
        ///     como información de error fatal.
        /// </summary>
        /// <param name="message">Mensaje.</param>
        public static void Fatal(Exception exception)
        {
            ILog log = GetLog();
            if (log != null)
            {
                log.Fatal("", exception);
            }
        }

        /// <summary>
        ///     Muestra el mansaje formateado <c>format</c> con los
        ///     argumentos <c>args</c> como información de error fatal.
        /// </summary>
        /// <param name="format">Formato.</param>
        /// <param name="args">Argumentos.</param>
        [StringFormatMethod("format")]
        public static void FatalFormat(string format, params object[] args)
        {
            ILog log = GetLog();
            if (log != null)
            {
                log.FatalFormat(format, args);
            }
        }

        /// <summary>
        ///     Muestra el mansaje formateado <c>format</c> con los
        ///     argumentos <c>args</c> y el proveedor de formato
        ///     <c>provider</c>, como información de error fatal.
        /// </summary>
        /// <param name="provider">Proveedor.</param>
        /// <param name="format">Formato.</param>
        /// <param name="args">Argumentos.</param>
        [StringFormatMethod("format")]
        public static void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            ILog log = GetLog();
            if (log != null)
            {
                log.FatalFormat(provider, format, args);
            }
        }

        /// <summary>
        ///     Indica si el log de debug esta habilitado.
        /// </summary>
        public static bool IsInfoEnabled
        {
            get
            {
                ILog log = GetLog();
                return ((log != null) ? log.IsInfoEnabled : false);
            }
        }

        /// <summary>
        ///     Muestra <c>message</c> como información.
        /// </summary>
        /// <param name="message">Mensaje</param>
        public static void Info(string message)
        {
            ILog log = GetLog();
            if (log != null)
            {
                log.Info(message);
            }
        }

        /// <summary>
        ///     Muestra <c>message</c> y la excepcion <c>exception</c>
        ///     como información.
        /// </summary>
        /// <param name="message">Mensaje.</param>
        /// <param name="exception">Excepción.</param>
        public static void Info(string message, Exception exception)
        {
            ILog log = GetLog();
            if (log != null)
            {
                log.Info(message, exception);
            }
        }

        /// <summary>
        ///     Muestra <c>message</c> y la excepcion <c>exception</c>
        ///     como información.
        /// </summary>
        /// <param name="exception">Excepción.</param>
        public static void Info(Exception exception)
        {
            ILog log = GetLog();
            if (log != null)
            {
                log.Info("", exception);
            }
        }

        /// <summary>
        ///     Muestra el mansaje formateado <c>format</c> con los
        ///     argumentos <c>args</c> como información.
        /// </summary>
        /// <param name="format">Formato.</param>
        /// <param name="args">Argumentos.</param>
        [StringFormatMethod("format")]
        public static void InfoFormat(string format, params object[] args)
        {
            ILog log = GetLog();
            if (log != null)
            {
                log.InfoFormat(format, args);
            }
        }

        /// <summary>
        ///     Muestra el mansaje formateado <c>format</c> con los
        ///     argumentos <c>args</c> y el proveedor de formato
        ///     <c>provider</c>, como información.
        /// </summary>
        /// <param name="provider">Proveedor.</param>
        /// <param name="format">Formato.</param>
        /// <param name="args">Argumentos.</param>
        [StringFormatMethod("format")]
        public static void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            ILog log = GetLog();
            if (log != null)
            {
                log.InfoFormat(provider, format, args);
            }
        }

        /// <summary>
        ///     Indica si el log de advertencia esta habilitado.
        /// </summary>
        public static bool IsWarnEnabled
        {
            get
            {
                ILog log = GetLog();
                return ((log != null) ? log.IsWarnEnabled : false);
            }
        }

        /// <summary>
        ///     Muestra <c>message</c> como información de advertencia.
        /// </summary>
        /// <param name="message">Mensaje</param>
        public static void Warn(string message)
        {
            ILog log = GetLog();
            if (log != null)
            {
                log.Warn(message);
            }
        }

        /// <summary>
        ///     Muestra <c>message</c> y la excepcion <c>exception</c>
        ///     como información de advertencia.
        /// </summary>
        /// <param name="message">Mensaje.</param>
        /// <param name="exception">Excepción.</param>
        public static void Warn(string message, Exception exception)
        {
            ILog log = GetLog();
            if (log != null)
            {
                log.Warn(message, exception);
            }
        }

        /// <summary>
        ///     Muestra <c>message</c> y la excepcion <c>exception</c>
        ///     como información de advertencia.
        /// </summary>
        /// <param name="message">Mensaje.</param>
        public static void Warn(Exception exception)
        {
            ILog log = GetLog();
            if (log != null)
            {
                log.Warn("", exception);
            }
        }

        /// <summary>
        ///     Muestra el mansaje formateado <c>format</c> con los
        ///     argumentos <c>args</c> como información de advertencia.
        /// </summary>
        /// <param name="format">Formato.</param>
        /// <param name="args">Argumentos.</param>
        [StringFormatMethod("format")]
        public static void WarnFormat(string format, params object[] args)
        {
            ILog log = GetLog();
            if (log != null)
            {
                log.WarnFormat(format, args);
            }
        }

        /// <summary>
        ///     Muestra el mansaje formateado <c>format</c> con los
        ///     argumentos <c>args</c> y el proveedor de formato
        ///     <c>provider</c>, como información de advertencia.
        /// </summary>
        /// <param name="provider">Proveedor.</param>
        /// <param name="format">Formato.</param>
        /// <param name="args">Argumentos.</param>
        [StringFormatMethod("format")]
        public static void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            ILog log = GetLog();
            if (log != null)
            {
                log.WarnFormat(provider, format, args);
            }
        }

        #region private

        /// <summary>Indica si el log esta activo.</summary>
        private static bool logActivo = true;

        /// <summary>Log actual.</summary>
        private static readonly ILog logActual = LogUtils.GetLog(typeof (T));

        private static ILog GetLog()
        {
            if (!logActivo)
            {
                return null;
            }
            return logActual;
        }

        #endregion
    }
}