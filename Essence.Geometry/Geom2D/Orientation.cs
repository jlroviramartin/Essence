﻿// Copyright 2017 Jose Luis Rovira Martin
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

namespace Essence.Geometry.Geom2D
{
    public enum Orientation
    {
        /// <summary>CounterClockWise, CCW ( + )<c><![CDATA[> 0]]></c></summary>
        CCW = 1,

        /// <summary>Degenrate ( 0 )<c><![CDATA[== 0]]></c></summary>
        Degenerate = 0,

        /// <summary>ClockWise, CW ( - )<c><![CDATA[< 0]]></c></summary>
        CW = -1
    }
}