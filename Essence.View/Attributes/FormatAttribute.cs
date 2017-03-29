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
using System.Collections;
using Essence.Util;
using Essence.View.Models;
using Essence.View.Models.Properties;

namespace Essence.View.Attributes
{
    public class FormatAttribute : ViewAttribute
    {
        public FormatAttribute()
        {
        }

        public FormatAttribute(string format)
        {
            this.Format = format;
        }

        public string Format { get; set; }
        public Type FormatProvider { get; set; }
        public Type CustomFormatter { get; set; }
        public Type EqualityComparer { get; set; }

        #region ViewAttribute

        public override ViewProperties GetViewProperties()
        {
            return new FormatProperties()
            {
                Format = this.Format,
                FormatProvider = this.FormatProvider.New<IFormatProvider>(),
                CustomFormatter = this.CustomFormatter.New<ICustomFormatter>(),
                EqualityComparer = this.EqualityComparer.New<IEqualityComparer>()
            };
        }

        #endregion
    }
}