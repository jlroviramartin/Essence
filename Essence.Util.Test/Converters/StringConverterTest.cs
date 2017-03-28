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