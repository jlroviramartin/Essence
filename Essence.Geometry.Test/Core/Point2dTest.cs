using Essence.Geometry.Core.Double;
using Essence.Maths.Double;
using Essence.Util.Math.Double;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Essence.Geometry.Test.Core
{
    [TestClass]
    public class Point2dTest
    {
        [TestMethod]
        public void Test1()
        {
            double angle = Point2d.EvAngle(new Point2d(10, 10), new Point2d(0, 0), new Point2d(20, 0));
            angle = AngleUtils.Ensure0To2Pi(angle);
            Assert.IsTrue(angle.EpsilonEquals(Math.PI / 2));
        }
    }
}