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

namespace Essence.Geometry.Core
{
    /// <summary>
    ///     Informacion de formato para vectores y puntos.
    /// </summary>
    public class VectorFormatInfo : ICloneable, IFormatProvider
    {
        /// <summary>
        /// Gets a read-only VectorFormatInfo object that contains vector formatting information for the current culture.
        /// </summary>
        public static VectorFormatInfo CurrentInfo;

        /// <summary>
        /// Gets a read-only VectorFormatInfo object that contains culture-independent (invariant) vector formatting information.
        /// </summary>
        public static VectorFormatInfo InvariantInfo;

        public VectorFormatInfo()
        {
            this.Beg = "(";
            this.Sep = ";";
            this.End = ")";
        }

        public VectorFormatInfo(string beg, string sep, string end)
        {
            this.Beg = beg;
            this.Sep = sep;
            this.End = end;
        }

        public bool HasBegEnd
        {
            get { return !string.IsNullOrEmpty(this.Beg) && !string.IsNullOrEmpty(this.End); }
        }

        public bool HasSep
        {
            get { return !string.IsNullOrEmpty(this.Sep); }
        }

        /// <summary>
        /// Gets or sets the begin text of a point/vector.
        /// </summary>
        public string Beg { get; set; }

        /// <summary>
        /// Gets or sets the coordinates separator text of a point/vector.
        /// </summary>
        public string Sep { get; set; }

        /// <summary>
        /// Gets or sets the end text of a point/vector.
        /// </summary>
        public string End { get; set; }

        #region privados

        static VectorFormatInfo()
        {
            InvariantInfo = new VectorFormatInfo();
            CurrentInfo = new VectorFormatInfo();
        }

        #endregion

        #region ICloneable

        public object Clone()
        {
            VectorFormatInfo pfi = (VectorFormatInfo)this.MemberwiseClone();
            return pfi;
        }

        #endregion

        #region IFormatProvider

        public object GetFormat(Type formatType)
        {
            if (typeof(VectorFormatInfo).IsAssignableFrom(formatType))
            {
                return this;
            }
            return null;
        }

        #endregion
    }
}