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
using System.Diagnostics;
using System.Threading;
using Essence.Util.Collections;

namespace Essence.Util.Services
{
    /// <summary>
    /// Servide provider that usesa map to store the services.
    /// </summary>
    public class MapServiceProvider : IServiceProvider
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public MapServiceProvider(IServiceProvider defaultServiceProvider = null)
        {
            this.defaultServiceProvider = defaultServiceProvider;
        }

        public void RegisterGetter(Type serviceType, Func<object> getter)
        {
            this.Register(serviceType, getter);
        }

        /// <summary>
        /// Registra el mapeo de un tipo de servicio al servicio asociado.
        /// </summary>
        /// <param name="serviceType">Tipo de servicio.</param>
        /// <param name="service">Servicio.</param>
        public void Register(Type serviceType, object service)
        {
            this.rwlock.AcquireWriterLock(-1);
            try
            {
                this.services.Add(serviceType, service);
            }
            finally
            {
                // Ensure that the lock is released.
                this.rwlock.ReleaseWriterLock();
            }
        }

        /// <summary>
        /// Registra el mapeo de un tipo de servicio al servicio asociado.
        /// </summary>
        /// <typeparam name="T">Tipo de servicio.</typeparam>
        /// <param name="service">Servicio.</param>
        public void Register<T>(T service)
        {
            this.rwlock.AcquireWriterLock(-1);
            try
            {
                this.services.Add(typeof(T), service);
            }
            finally
            {
                // Ensure that the lock is released.
                this.rwlock.ReleaseWriterLock();
            }
        }

        #region private

        /// <summary>Servicios por defecto en donde buscar.</summary>
        private readonly IServiceProvider defaultServiceProvider;

        /// <summary>Mapa de servicios.</summary>
        private readonly DictionaryOfType<object> services = new DictionaryOfType<object>();

        /// <summary>Defines a lock that supports single writers and multiple readers.</summary>
        private readonly ReaderWriterLock rwlock = new ReaderWriterLock();

        #endregion

        #region IServiceProvider

        public object GetService(Type serviceType)
        {
            Debug.Assert(serviceType != null);

            this.rwlock.AcquireReaderLock(-1);
            try
            {
                object service;
                if (this.services.TryGetValue(serviceType, out service))
                {
                    /*if (service is IConstructor)
                    {
                        return ((IConstructor)service).Invoke();
                    }*/
                    if (service is Func<object>)
                    {
                        Func<object> getter = (Func<object>)service;
                        return getter();
                    }
                    return service;
                }

                if (this.defaultServiceProvider != null)
                {
                    return this.defaultServiceProvider.GetService(serviceType);
                }

                return null;
            }
            finally
            {
                // Ensure that the lock is released.
                this.rwlock.ReleaseReaderLock();
            }
        }

        #endregion
    }
}