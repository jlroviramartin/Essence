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
using System.IO;

namespace Essence.Util.Logs
{
    /// <summary>
    ///     Factoría de logs que encapsula a <c>log4net.LogFactory</c>.
    /// </summary>
    public sealed class Log4NetLogFactory : ILogFactory
    {
        /// <summary>
        ///     Instancia única.
        /// </summary>
        public static readonly Log4NetLogFactory Instance;

        #region private

        /// <summary>
        ///     Constructor de clase.
        /// </summary>
        static Log4NetLogFactory()
        {
            try
            {
                //Log4NetUtils.InitLog4Net();
                InitLog4NetConfig();
                Instance = new Log4NetLogFactory();
            }
            catch (Exception)
            {
                Instance = null;
            }
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        private Log4NetLogFactory()
        {
        }

        /// <summary>
        ///     Se configura segun el fichero <c>log4net.xml</c>.
        /// </summary>
        private static void InitLog4NetXml()
        {
            try
            {
                // Carga el fichero.
                FileStream fs = File.Open(DEFAULT_LOG4NET_PATH, FileMode.Open);
                log4net.Config.XmlConfigurator.Configure(fs);
            }
            catch (Exception)
            {
                Console.Error.WriteLine("Fichero '{0}' no encontrado.", DEFAULT_LOG4NET_PATH);
            }
        }

        /// <summary>
        ///     Se configura segun el fichero <c>log4net.config</c>.
        /// </summary>
        private static void InitLog4NetConfig()
        {
            try
            {
                // Carga la configuracion.
                log4net.Config.XmlConfigurator.Configure();
            }
            catch (Exception)
            {
                Console.Error.WriteLine("Error al inicializar la configuración de log4net.");
            }
        }

        /// <summary>Path por defecto del fichero 'log4net.xml'.</summary>
        private const string DEFAULT_LOG4NET_PATH = "Properties\\log4net.xml";

        #endregion

        #region ILogFactory

        public ILog CreateLog(Type type)
        {
            return new Log4netLog(log4net.LogManager.GetLogger(type));
        }

        #endregion
    }
}