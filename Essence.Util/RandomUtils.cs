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
    public static class RandomUtils
    {
        /// <summary>
        /// Next random boolean value.
        /// </summary>
        /// <param name="random">Random number generator.</param>
        /// <returns>Next boolean.</returns>
        public static bool NextBool(this Random random)
        {
            return (random.Next(0, 2) == 1);
        }
    }
}