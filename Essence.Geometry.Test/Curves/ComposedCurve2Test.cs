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
using Essence.Util.Math.Double;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Essence.Geometry.Curves
{
    [TestClass]
    public class ComposedCurve2Test
    {
        [TestMethod]
        public void Test1()
        {
            ComposedCurve2 m = new ComposedCurve2(new ICurve2[]
            {
                new Line2(new Point2d(0, 0), new Point2d(10, 0)),
                new Line2(new Point2d(10, 0), new Point2d(10, 10)),
                new Line2(new Point2d(10, 10), new Point2d(0, 10)),
                new Line2(new Point2d(0, 10), new Point2d(0, 0))
            });

            double tl = m.TotalLength;
            Assert.IsTrue(tl.EpsilonEquals(40));
            for (double l = 0; l <= m.TotalLength; l += m.TotalLength / 10)
            {
                Assert.IsTrue(m.GetLength(m.TMin, m.GetT(l)).EpsilonEquals(l));
            }
        }
    }
}