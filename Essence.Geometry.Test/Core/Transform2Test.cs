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

using Essence.Geometry.Core.Double;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Essence.Geometry.Core
{
    [TestClass]
    public class Transform2Test
    {
        [TestMethod]
        public void TransformTest()
        {
            TestTransform(new Point2d(10, 10), new Point2d(20, 20),
                          new Point2d(-70, 100), new Point2d(45, -5));
            TestTransform(new Point2d(10, 10), new Point2d(20, 20),
                          new Point2d(0, 0), new Point2d(-10, -10));
        }

        private static void TestTransform(Point2d a, Point2d b, Point2d c, Point2d d)
        {
            Transform2 t = Transform2.Transform(a, b, c, d);

            Assert.IsTrue(c.EpsilonEquals(t.Transform(a), 0.001)
                          && d.EpsilonEquals(t.Transform(b), 0.001)
                          && c.Lerp(d, 0.5).EpsilonEquals(t.Transform(a.Lerp(b, 0.5)), 0.001));
        }
    }
}