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
using System.Globalization;
using Essence.Util.Collections;

namespace Essence.Util.Converters
{
    public class StringConverter
    {
        public static StringConverter Instance = new StringConverter();

        public void Register<T>(ICustomFormatter customFormatter)
        {
            this.Register(typeof(T), customFormatter);
        }

        public void Register(Type type, ICustomFormatter customFormatter)
        {
            this.formatters.Add(type, customFormatter);
        }

        public void Register<T>(ICustomParser customParser)
        {
            this.Register(typeof(T), customParser);
        }

        public void Register(Type type, ICustomParser customParser)
        {
            this.parsers.Add(type, customParser);
        }

        public bool TryParse<T>(string text, string format, IFormatProvider formatProvider, out T result)
        {
            object oresult;
            if (!this.TryParse(text, format, formatProvider, typeof(T), out oresult))
            {
                result = default(T);
                return false;
            }
            result = (T)oresult;
            return true;
        }

        public bool TryParse(string text, string format, IFormatProvider formatProvider, Type dstType, out object result)
        {
            ICustomParser customparser;
            if (this.parsers.TryGetValue(dstType, out customparser))
            {
                return customparser.TryParse(text, format, formatProvider, out result);
            }
            result = null;
            return false;
        }

        public string ToString<T>(T obj, string format, IFormatProvider formatProvider)
        {
            return this.ToString((object)obj, format, formatProvider);
        }

        public string ToString(object obj, string format, IFormatProvider formatProvider)
        {
            if (obj == null)
            {
                return "";
            }
            ICustomFormatter customFormatter;
            if (this.formatters.TryGetValue(obj.GetType(), out customFormatter))
            {
                return customFormatter.Format(format, obj, formatProvider);
            }
            if (obj is IFormattable)
            {
                return ((IFormattable)obj).ToString(format, formatProvider);
            }
            return obj.ToString();
        }

        #region private

        private StringConverter()
        {
            this.RegisterBoth((string text, string format, IFormatProvider formatProvider, out short result) =>
            {
                NumberStyles numberStyles = NumberStyles.Integer;
                return short.TryParse(text, numberStyles, formatProvider, out result);
            });

            this.RegisterBoth((string text, string format, IFormatProvider formatProvider, out ushort result) =>
            {
                NumberStyles numberStyles = NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;
                return ushort.TryParse(text, numberStyles, formatProvider, out result);
            });

            this.RegisterBoth((string text, string format, IFormatProvider formatProvider, out int result) =>
            {
                NumberStyles numberStyles = NumberStyles.Integer;
                return int.TryParse(text, numberStyles, formatProvider, out result);
            });

            this.RegisterBoth((string text, string format, IFormatProvider formatProvider, out uint result) =>
            {
                NumberStyles numberStyles = NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;
                return uint.TryParse(text, numberStyles, formatProvider, out result);
            });

            this.RegisterBoth((string text, string format, IFormatProvider formatProvider, out long result) =>
            {
                NumberStyles numberStyles = NumberStyles.Integer;
                return long.TryParse(text, numberStyles, formatProvider, out result);
            });

            this.RegisterBoth((string text, string format, IFormatProvider formatProvider, out ulong result) =>
            {
                NumberStyles numberStyles = NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;
                return ulong.TryParse(text, numberStyles, formatProvider, out result);
            });

            this.RegisterBoth((string text, string format, IFormatProvider formatProvider, out float result) =>
            {
                NumberStyles numberStyles = NumberStyles.Integer;
                return float.TryParse(text, numberStyles, formatProvider, out result);
            });

            this.RegisterBoth((string text, string format, IFormatProvider formatProvider, out double result) =>
            {
                NumberStyles numberStyles = NumberStyles.Integer;
                return double.TryParse(text, numberStyles, formatProvider, out result);
            });

            this.Register((string text, string format, IFormatProvider formatProvider, out string result) =>
            {
                result = text;
                return true;
            });
        }

        public void RegisterBoth<T>(TryParse<T> tryParse) where T : struct
        {
            this.Register<T>(tryParse);
            this.RegisterNullable<T>(tryParse);
        }

        public void Register<T>(TryParse<T> tryParse)
        {
            this.Register(typeof(T), new DelegateCustomParser((string text, string format, IFormatProvider formatProvider, out object result) =>
            {
                /*if (string.IsNullOrWhiteSpace(text))
                {
                    result = null;
                    return false;
                }*/
                T tresult;
                if (!tryParse(text, format, formatProvider, out tresult))
                {
                    result = default(T);
                    return false;
                }
                result = tresult;
                return true;
            }));
        }

        public void RegisterNullable<T>(TryParse<T> tryParse)
            where T : struct
        {
            this.Register(typeof(T?), new DelegateCustomParser((string text, string format, IFormatProvider formatProvider, out object result) =>
            {
                if (string.IsNullOrWhiteSpace(text))
                {
                    result = null;
                    return true;
                }
                T tresult;
                if (!tryParse(text, format, formatProvider, out tresult))
                {
                    result = default(T);
                    return false;
                }
                result = tresult;
                return true;
            }));
        }

        private readonly DictionaryOfType<ICustomFormatter> formatters = new DictionaryOfType<ICustomFormatter>();
        private readonly DictionaryOfType<ICustomParser> parsers = new DictionaryOfType<ICustomParser>();

        #endregion

        private sealed class DelegateCustomParser : ICustomParser
        {
            public DelegateCustomParser(TryParse tryParse)
            {
                this.tryParse = tryParse;
            }

            private readonly TryParse tryParse;

            public bool TryParse(string text, string format, IFormatProvider formatProvider, out object result)
            {
                return this.tryParse(text, format, formatProvider, out result);
            }
        }
    }

    public delegate bool TryParse(string text, string format, IFormatProvider formatProvider, out object result);

    public delegate bool TryParse<T>(string text, string format, IFormatProvider formatProvider, out T result);
}