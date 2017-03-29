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
    ///     Log genérico. Encapsula a <c>log4net.ILog</c>.
    /// </summary>
    public sealed class Log4netLog : ILog
    {
        #region private

        static Log4netLog()
        {
            //Log4NetUtils.InitLog4Net();
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="log">Log que encapsula.</param>
        internal Log4netLog(log4net.ILog log)
        {
            this.log = log;
        }

        /// <summary>Log que encapsula.</summary>
        private readonly log4net.ILog log;

        #endregion

        #region ILog

        public bool IsEnabled(LogType type)
        {
            switch (type)
            {
                case LogType.Debug:
                    return this.IsDebugEnabled;
                case LogType.Error:
                    return this.IsErrorEnabled;
                case LogType.Fatal:
                    return this.IsFatalEnabled;
                case LogType.Info:
                    return this.IsInfoEnabled;
                case LogType.Warn:
                    return this.IsWarnEnabled;
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        public void Write(LogType type, string message)
        {
            switch (type)
            {
                case LogType.Debug:
                    this.log.Debug(message);
                    break;
                case LogType.Error:
                    this.log.Error(message);
                    break;
                case LogType.Fatal:
                    this.log.Fatal(message);
                    break;
                case LogType.Info:
                    this.log.Info(message);
                    break;
                case LogType.Warn:
                    this.log.Warn(message);
                    break;
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        public void Write(LogType type, string message, Exception exception)
        {
            switch (type)
            {
                case LogType.Debug:
                    this.log.Debug(message, exception);
                    break;
                case LogType.Error:
                    this.log.Error(message, exception);
                    break;
                case LogType.Fatal:
                    this.log.Fatal(message, exception);
                    break;
                case LogType.Info:
                    this.log.Info(message, exception);
                    break;
                case LogType.Warn:
                    this.log.Warn(message, exception);
                    break;
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        public void WriteFormat(LogType type, string format, params object[] args)
        {
            switch (type)
            {
                case LogType.Debug:
                    this.log.DebugFormat(format, args);
                    break;
                case LogType.Error:
                    this.log.ErrorFormat(format, args);
                    break;
                case LogType.Fatal:
                    this.log.FatalFormat(format, args);
                    break;
                case LogType.Info:
                    this.log.InfoFormat(format, args);
                    break;
                case LogType.Warn:
                    this.log.WarnFormat(format, args);
                    break;
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        public void WriteFormat(LogType type, IFormatProvider provider, string format, params object[] args)
        {
            switch (type)
            {
                case LogType.Debug:
                    this.log.DebugFormat(provider, format, args);
                    break;
                case LogType.Error:
                    this.log.ErrorFormat(provider, format, args);
                    break;
                case LogType.Fatal:
                    this.log.FatalFormat(provider, format, args);
                    break;
                case LogType.Info:
                    this.log.InfoFormat(provider, format, args);
                    break;
                case LogType.Warn:
                    this.log.WarnFormat(provider, format, args);
                    break;
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        public bool IsDebugEnabled
        {
            get { return this.log.IsDebugEnabled; }
        }

        public void Debug(string message)
        {
            this.log.Debug(message);
        }

        public void Debug(string message, Exception exception)
        {
            this.log.Debug(message, exception);
        }

        public void DebugFormat(string format, params object[] args)
        {
            this.log.DebugFormat(format, args);
        }

        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            this.log.DebugFormat(provider, format, args);
        }

        public bool IsErrorEnabled
        {
            get { return this.log.IsErrorEnabled; }
        }

        public void Error(string message)
        {
            this.log.Error(message);
        }

        public void Error(string message, Exception exception)
        {
            this.log.Error(message, exception);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            this.log.ErrorFormat(format, args);
        }

        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            this.log.ErrorFormat(provider, format, args);
        }

        public bool IsFatalEnabled
        {
            get { return this.log.IsFatalEnabled; }
        }

        public void Fatal(string message)
        {
            this.log.Fatal(message);
        }

        public void Fatal(string message, Exception exception)
        {
            this.log.Fatal(message, exception);
        }

        public void FatalFormat(string format, params object[] args)
        {
            this.log.FatalFormat(format, args);
        }

        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            this.log.FatalFormat(provider, format, args);
        }

        public bool IsInfoEnabled
        {
            get { return this.log.IsInfoEnabled; }
        }

        public void Info(string message)
        {
            this.log.Info(message);
        }

        public void Info(string message, Exception exception)
        {
            this.log.Info(message, exception);
        }

        public void InfoFormat(string format, params object[] args)
        {
            this.log.InfoFormat(format, args);
        }

        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            this.log.InfoFormat(provider, format, args);
        }

        public bool IsWarnEnabled
        {
            get { return (bool)this.log.IsWarnEnabled; }
        }

        public void Warn(string message)
        {
            this.log.Warn(message);
        }

        public void Warn(string message, Exception exception)
        {
            this.log.Warn(message, exception);
        }

        public void WarnFormat(string format, params object[] args)
        {
            this.log.WarnFormat(format, args);
        }

        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            this.log.WarnFormat(provider, format, args);
        }

        #endregion
    }
}