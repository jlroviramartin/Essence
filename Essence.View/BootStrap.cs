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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Essence.View.Test
{
    public class BootStrap
    {
        public const string WIN32 = "x86";
        public const string WIN64 = "x64";
        public const string DOTTRACE_PATH = @"C:\Users\joseluis\AppData\Local\JetBrains\Installations\dotTrace06";

        public BootStrap(string applicationName)
        {
            this.applicationName = applicationName;
        }

        public void DefaultEnvironmentVariables()
        {
            string programPath = System.AppDomain.CurrentDomain.BaseDirectory;
            string dataPath = System.AppDomain.CurrentDomain.BaseDirectory;
            string winXxxPath = Path.Combine(programPath, (Environment.Is64BitProcess ? WIN64 : WIN32));

            // Se configura Proj4.
            this.RegisterEnvironmentVariable("PROJ_LIB", Path.Combine(dataPath, "data")); // "SHARE"

            // Antes de referenciar la dll de Gdal, se indican las rutas de acceso.
            this.RegisterEnvironmentVariable("GDAL_DATA", Path.Combine(dataPath, "data"));
            this.RegisterEnvironmentVariable("GEOTIFF_CSV", Path.Combine(dataPath, "data"));
            this.RegisterEnvironmentVariable("GDAL_DRIVER_PATH", Path.Combine(winXxxPath, "gdalplugins"));
        }

        public void Run(Action<string[]> main, string[] args)
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;

            if (!currentDomain.IsDefaultAppDomain())
            {
                // Información de dominio.
                Debug.WriteLine(GetInfo(AppDomain.CurrentDomain));

                main(args);
                return;
            }

            // Información de dominio.
            Debug.WriteLine(GetInfo(currentDomain));

            this.ConfigurarPath3264();

            AppDomainSetup setup = new AppDomainSetup();
            setup.ApplicationName = this.applicationName;
            setup.ApplicationBase = currentDomain.BaseDirectory;
            setup.PrivateBinPath = (Environment.Is64BitProcess ? WIN64 : WIN32);

            // Create a new application domain.
            AppDomain domain = AppDomain.CreateDomain(this.applicationName + ".Domain", currentDomain.Evidence, setup);

#if USO_INTERNO && !DISABLE_DOTTRACE
            domain.AssemblyResolve += MyResolveEventHandler;
            domain.ReflectionOnlyAssemblyResolve += MyReflectionOnlyAssemblyResolveEventHandler;
#endif

            // Información de dominio.
            Debug.WriteLine(GetInfo(domain));

            Assembly executingAssembly = Assembly.GetEntryAssembly();
            //domain.ExecuteAssemblyByName(typeof(BootStrap).Assembly.FullName);
            domain.ExecuteAssemblyByName(executingAssembly.GetName());
        }

        public void RegisterEnvironmentVariable(string name, string value)
        {
            this.environmentVariables.Add(Tuple.Create(name, value));
        }

        #region private

        /// <summary>
        ///     Configura los path para sistemas de 32 y 64 bits.
        /// </summary>
        private void ConfigurarPath3264()
        {
            // Se añade el directorio 'dll' al 'PATH' para el proceso actual.
            string programPath = System.AppDomain.CurrentDomain.BaseDirectory;
            string winXxxPath = Path.Combine(programPath, (Environment.Is64BitProcess ? WIN64 : WIN32));

#if false
            AppDomain domain = AppDomain.CurrentDomain;
            domain.AssemblyResolve += MyResolveEventHandler;
            domain.ReflectionOnlyAssemblyResolve += MyReflectionOnlyAssemblyResolveEventHandler;
#endif

            // Se añade el directorio de inicio del programa al 'PATH' para el proceso actual.
            string path = programPath
                          + Path.PathSeparator + winXxxPath
                          + Path.PathSeparator + Environment.GetEnvironmentVariable("PATH");

            // Se establece el directorio x86 / x64.
            bool ret = SetDllDirectory(winXxxPath);
            Debug.WriteLine("SetDllDirectory " + GetLastWin32ErrorText() + " : return " + ret);
            Debug.WriteLine("GetDllDirectory " + GetDllDirectory());

            // Se establece el path.
            Environment.SetEnvironmentVariable("PATH", path);

            foreach (Tuple<string, string> environmentVariable in this.environmentVariables)
            {
                Environment.SetEnvironmentVariable(environmentVariable.Item1, environmentVariable.Item2);
            }

            // Se configura Gdal.
            //GdalOgrUtils.Config(winXxxPath, dataPath);

            /*try
            {
                Log<BootStrap>.Warn(GDALUtils.GetDriversInfo());
            }
            catch (Exception e)
            {
                Log<BootStrap>.Warn(e);
            }*/
        }

#if USO_INTERNO && !DISABLE_DOTTRACE
        private static Assembly MyReflectionOnlyAssemblyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            //Debug.WriteLine("[ReflectionOnly Assembly Resolve] LoadedAssembly:Name: " + args.Name + " RequestingAssembly: " + args.RequestingAssembly);

            return args.RequestingAssembly;
        }

        private static Assembly MyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            //Debug.WriteLine("[Assembly Resolve] LoadedAssembly:Name: " + args.Name + " RequestingAssembly: " + args.RequestingAssembly);

            // Solo para mi que soy un egoista.. :)
            if (args.Name.StartsWith("JetBrains"))
            {
                int index = args.Name.IndexOf(",");
                if (index >= 0)
                {
                    string assemblyPath = Path.Combine(DOTTRACE_PATH, args.Name.Substring(0, index) + ".dll");
                    if (File.Exists(assemblyPath))
                    {
                        return Assembly.LoadFrom(assemblyPath);
                    }
                }
            }

#if false
    // Se añade el directorio 'win32'/'win64' al 'PATH' para el proceso actual.
            string pathProgram = Path.Combine(Application.StartupPath, (Environment.Is64BitProcess ? WIN64 : WIN32));

            try
            {
                int index = args.Name.IndexOf(",");
                if (index >= 0)
                {
                    string assemblyPath = Path.Combine(pathProgram, args.Name.Substring(0, index) + ".dll");
                    if (File.Exists(assemblyPath))
                    {
                        return Assembly.LoadFrom(assemblyPath);
                    }

                    if (args.Name.StartsWith("JetBrains.Profiler"))
                    {
                        assemblyPath = Path.Combine(@"C:\Users\joseluis\AppData\Local\JetBrains\Installations\dotTrace05", args.Name.Substring(0, index) + ".dll");
                        if (File.Exists(assemblyPath))
                        {
                            return Assembly.LoadFrom(assemblyPath);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
#endif

            return args.RequestingAssembly;
        }
#endif

        private readonly string applicationName;
        private List<Tuple<string, string>> environmentVariables = new List<Tuple<string, string>>();

        // http://stackoverflow.com/questions/21710982/how-to-adjust-path-for-dynamically-loaded-native-dlls

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool SetDllDirectory(string lpPathName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr AddDllDirectory(string lpPathName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int GetDllDirectory(int nBufferLength, StringBuilder lpPathName);

        private static string GetDllDirectory()
        {
            StringBuilder tmp = new StringBuilder(10240);
            GetDllDirectory(10240, tmp);
            return tmp.ToString();
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern bool FreeLibrary(IntPtr hModule);

        private static string GetLastWin32ErrorText()
        {
            return new Win32Exception(Marshal.GetLastWin32Error()).Message;
        }

        private static string GetInfo(AppDomain domain)
        {
            StringBuilder buff = new StringBuilder();

            buff.AppendLine("------------------------------------------------------");
            buff.AppendLine("FriendlyName:       " + domain.FriendlyName);
            buff.AppendLine("Id:                 " + domain.Id);
            buff.AppendLine("BaseDirectory:      " + domain.BaseDirectory);
            buff.AppendLine("RelativeSearchPath: " + domain.RelativeSearchPath);
            buff.AppendLine("DynamicDirectory:   " + domain.DynamicDirectory);
            buff.AppendLine("IsFullyTrusted:     " + domain.IsFullyTrusted);
            buff.AppendLine("IsHomogenous:       " + domain.IsHomogenous);
            buff.AppendLine("IsDefaultAppDoma:   " + domain.IsDefaultAppDomain());

            return buff.ToString();
        }

        #endregion
    }
}