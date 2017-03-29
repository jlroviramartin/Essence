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

namespace Essence.Util
{
    /// <summary>
    ///     Objeto que se puede liberar.
    ///     A base class that implements IDisposable.
    ///     By implementing IDisposable, you are announcing that
    ///     instances of this type allocate scarce resources.
    ///     https://msdn.microsoft.com/en-us/library/fs2xkftw.aspx
    ///     https://msdn.microsoft.com/en-us/library/ms244737.aspx
    ///     https://msdn.microsoft.com/en-us/library/b1yfkh5e.aspx
    /// </summary>
    public abstract class DisposableObject : IDisposableEx
    {
        #region protected

        protected DisposableObject()
        {
        }

        /// <summary>
        ///     Libera los recursos manejados (managed resources).
        ///     Utilizar IDisposable.Dispose() para los objetos.
        /// </summary>
        protected virtual void DisposeOfManagedResources()
        {
        }

        /// <summary>
        ///     Libera los recursos no manejados (unmanaged resources).
        /// </summary>
        protected virtual void DisposeOfUnManagedResources()
        {
        }

        #endregion

        #region private

        ~DisposableObject()
        {
            this.Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                // Dispose managed resources.
                this.DisposeOfManagedResources();
            }

            // Dispose unmanaged resources.
            this.DisposeOfUnManagedResources();

            this.disposed = true;
        }

        // Flag: Has Dispose already been called?
        private bool disposed = false;

        #endregion Miembros privados _______________________________________________________________

        #region IDisposableEx

        public bool IsDisposed
        {
            get { return this.disposed; }
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}