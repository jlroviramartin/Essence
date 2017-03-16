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
using System.Collections.Generic;

namespace Essence.Util.Collections
{
    public class DictionaryOfType<TV> : HierarchyGraphDictionary<Type, TV>
    {
        public DictionaryOfType()
            : base(new HGraph())
        {
        }

        #region Inner classes

        internal class HGraph : IHGraph
        {
            public IEnumerable<Type> GetParents(Type key)
            {
                // class, interface, struct : IA, IB ===> IA, IB
                foreach (Type @interface in key.GetInterfaces())
                {
                    yield return @interface;
                }

                // enum : int ===> int
                if (key.IsEnum)
                {
                    yield return Enum.GetUnderlyingType(key);
                    yield break;
                }

                // struct? ===> struct
                if (key.IsNullable())
                {
                    yield return Nullable.GetUnderlyingType(key);
                    yield break;
                }

                // class, struct : B ===> B
                if (key.BaseType != null)
                {
                    yield return key.BaseType;
                }
            }

            public bool IsRoot(Type key)
            {
                return ((key.BaseType == null) && key.GetInterfaces().IsEmpty());
            }
        }

        #endregion
    }
}