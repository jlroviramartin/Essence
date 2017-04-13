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
using System.Linq;

namespace Essence.Util.Services
{
    /// <summary>
    /// Service provider that joins some services providers.
    /// </summary>
    public class JoinServiceProvider : IServiceProvider
    {
        public JoinServiceProvider(params Func<Type, object>[] getServices)
            : this((IEnumerable<Func<Type, object>>)getServices)
        {
        }

        public JoinServiceProvider(IEnumerable<Func<Type, object>> getServices)
        {
            this.getServices = getServices.ToArray();
        }

        #region private

        private readonly Func<Type, object>[] getServices;

        #endregion

        #region IServiceProvider

        public object GetService(Type serviceType)
        {
            return this.getServices.Select(getService => getService(serviceType)).FirstOrDefault(result => result != null);
        }

        #endregion
    }
}