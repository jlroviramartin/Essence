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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Essence.Maths.Test
{
    public class AssertEx
    {
        public static void AssertException<TE>(Action acc)
            where TE : Exception
        {
            bool exception = false;
            try
            {
                acc();
            }
            catch (TE)
            {
                exception = true;
            }
            Assert.IsTrue(exception);
        }

        public static void AssertException<TE>(Action acc, Func<TE, bool> test)
            where TE : Exception
        {
            bool exception = false;
            try
            {
                acc();
            }
            catch (TE e)
            {
                exception = test(e);
            }
            Assert.IsTrue(exception);
        }
    }
}