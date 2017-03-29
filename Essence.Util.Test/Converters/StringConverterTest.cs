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

using System.Globalization;
using Essence.Util.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Essence.Util.Test.Converters
{
    [TestClass]
    public class StringConverterTest
    {
        [TestMethod]
        public void Test1()
        {
            CultureInfo enUs = CultureInfo.InvariantCulture;
            {
                int result;
                bool ret = StringConverter.Instance.TryParse("10", "", enUs, out result);
                Assert.IsTrue(ret && result == 10);
            }
            {
                int result;
                bool ret = StringConverter.Instance.TryParse("", "", enUs, out result);
                Assert.IsTrue(!ret);
            }
            {
                int? result;
                bool ret = StringConverter.Instance.TryParse("10", "", enUs, out result);
                Assert.IsTrue(ret && result == 10);
            }
            {
                int? result;
                bool ret = StringConverter.Instance.TryParse("", "", enUs, out result);
                Assert.IsTrue(ret && result == null);
            }
            {
                long result;
                bool ret = StringConverter.Instance.TryParse("10", "", enUs, out result);
                Assert.IsTrue(ret && result == 10);
            }
            {
                long result;
                bool ret = StringConverter.Instance.TryParse("", "", enUs, out result);
                Assert.IsTrue(!ret);
            }
            {
                long? result;
                bool ret = StringConverter.Instance.TryParse("10", "", enUs, out result);
                Assert.IsTrue(ret && result == 10);
            }
            {
                long? result;
                bool ret = StringConverter.Instance.TryParse("", "", enUs, out result);
                Assert.IsTrue(ret && result == null);
            }
            {
                string result;
                bool ret = StringConverter.Instance.TryParse("10", "", enUs, out result);
                Assert.IsTrue(ret && result == "10");
            }
            {
                string result;
                bool ret = StringConverter.Instance.TryParse("", "", enUs, out result);
                Assert.IsTrue(ret && result == "");
            }
        }
    }
}