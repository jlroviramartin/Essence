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
using System.Runtime.InteropServices;

namespace Essence.Geometry.Core
{
    /// <summary>
    /// Styles of the vectors.
    /// </summary>
    [Flags]
    [Guid("CD1B7003-54E7-4D7C-BA23-A3AF96F84720")]
    public enum VectorStyles
    {
        Empty = 0,
        Sep = 1,
        BegEnd = 2,
        All = Sep | BegEnd
    }
}