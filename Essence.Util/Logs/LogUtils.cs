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
using System.Diagnostics;

namespace Essence.Util.Logs
{
    public static class LogUtils
    {
        static LogUtils()
        {
            // NOTA: en alguna ocasión el código de abajo ha producido errores.
            try
            {
                DefaultFactory = Log4NetLogFactory.Instance;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        /// <summary>
        ///     Factoría por defecto.
        /// </summary>
        public static ILogFactory DefaultFactory
        {
            get { return factory; }
            set { factory = value; }
        }

        /// <summary>
        ///     Crea un acceso al log a partir del tipo <c>type</c>,
        ///     utilizando la fábrica <c>LogUtils.DefaultFactory</c>.
        /// </summary>
        public static ILog GetLog(Type type)
        {
            return DefaultFactory.CreateLog(type);
        }

        /// <summary>
        ///     Crea un acceso al log a partir del tipo <c>type</c>,
        ///     utilizando la fábrica <c>LogUtils.DefaultFactory</c>.
        /// </summary>
        public static ILog GetLog<T>()
        {
            return GetLog(typeof (T));
        }

        #region private

        /// <summary>Factoria.</summary>
        private static ILogFactory factory;

        #endregion
    }
}