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
using Essence.Geometry.Geom2D;
using Essence.Geometry.Intersection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Essence.Geometry.Test.Intersection
{
    [TestClass]
    public class IntrSegment2dSegment2dTest
    {
        [TestMethod]
        public void Test1()
        {
            {
                IntrSegment2dSegment2d intr = new IntrSegment2dSegment2d();
                intr.Item0 = new Segment2d(new Point2d(0, 0), new Point2d(10, 10));
                intr.Item1 = new Segment2d(new Point2d(0, 10), new Point2d(10, 0));
                intr.Find();
                Assert.IsTrue(intr.IntersectionType == IntersectionType.POINT);
                Assert.IsTrue(intr.Intersections.Length == 1 && intr.Intersections[0].Point.EpsilonEquals(new Point2d(5, 5)));
            }
            {
                IntrSegment2dSegment2d intr = new IntrSegment2dSegment2d();
                intr.Item0 = new Segment2d(new Point2d(0, 0), new Point2d(6, 6));
                intr.Item1 = new Segment2d(new Point2d(4, 4), new Point2d(10, 10));
                intr.Find();
                Assert.IsTrue(intr.IntersectionType == IntersectionType.SEGMENT);
                Assert.IsTrue(intr.Intersections.Length == 2 && intr.Intersections[0].Point.EpsilonEquals(new Point2d(4, 4)) && intr.Intersections[1].Point.EpsilonEquals(new Point2d(6, 6)));
            }
            {
                IntrSegment2dSegment2d intr = new IntrSegment2dSegment2d();
                intr.Item0 = new Segment2d(new Point2d(0, 0), new Point2d(5, 5));
                intr.Item1 = new Segment2d(new Point2d(0, 10), new Point2d(10, 0));
                intr.Find();
                Assert.IsTrue(intr.IntersectionType == IntersectionType.POINT);
                Assert.IsTrue(intr.Intersections.Length == 1 && intr.Intersections[0].Point.EpsilonEquals(new Point2d(5, 5)));
            }
        }

        [TestMethod]
        public void Test2()
        {
            {
                IntrSegment2dSegment2d intr = new IntrSegment2dSegment2d();
                intr.Item0 = new Segment2d(new Point2d(0, 0), new Point2d(10, 0));
                intr.Item1 = new Segment2d(new Point2d(0, 0), new Point2d(10, 0));
                intr.Find();
                Assert.IsTrue(intr.IntersectionType == IntersectionType.SEGMENT);
                //Assert.IsTrue(intr.Points.Length == 1 && intr.Points[0].EpsilonEquals(new Point2d(5, 5)));
            }

            {
                IntrSegment2dSegment2d intr = new IntrSegment2dSegment2d();
                intr.Item0 = new Segment2d(new Point2d(0, 0), new Point2d(10, 0));
                intr.Item1 = new Segment2d(new Point2d(2, 0), new Point2d(8, 0));
                intr.Find();
                Assert.IsTrue(intr.IntersectionType == IntersectionType.SEGMENT);
                //Assert.IsTrue(intr.Points.Length == 1 && intr.Points[0].EpsilonEquals(new Point2d(5, 5)));
            }

            {
                IntrSegment2dSegment2d intr = new IntrSegment2dSegment2d();
                intr.Item0 = new Segment2d(new Point2d(0, 0), new Point2d(10, 0));
                intr.Item1 = new Segment2d(new Point2d(-2, 0), new Point2d(12, 0));
                intr.Find();
                Assert.IsTrue(intr.IntersectionType == IntersectionType.SEGMENT);
                //Assert.IsTrue(intr.Points.Length == 1 && intr.Points[0].EpsilonEquals(new Point2d(5, 5)));
            }
            {
                IntrSegment2dSegment2d intr = new IntrSegment2dSegment2d();
                intr.Item0 = new Segment2d(new Point2d(0, 0), new Point2d(10, 0));
                intr.Item1 = new Segment2d(new Point2d(2, 0), new Point2d(12, 0));
                intr.Find();
                Assert.IsTrue(intr.IntersectionType == IntersectionType.SEGMENT);
                //Assert.IsTrue(intr.Points.Length == 1 && intr.Points[0].EpsilonEquals(new Point2d(5, 5)));
            }

            {
                IntrSegment2dSegment2d intr = new IntrSegment2dSegment2d();
                intr.Item0 = new Segment2d(new Point2d(0, 0), new Point2d(10, 0));
                intr.Item1 = new Segment2d(new Point2d(-2, 0), new Point2d(8, 0));
                intr.Find();
                Assert.IsTrue(intr.IntersectionType == IntersectionType.SEGMENT);
                //Assert.IsTrue(intr.Points.Length == 1 && intr.Points[0].EpsilonEquals(new Point2d(5, 5)));
            }

            {
                IntrSegment2dSegment2d intr = new IntrSegment2dSegment2d();
                intr.Item0 = new Segment2d(new Point2d(0, 0), new Point2d(10, 0));
                intr.Item1 = new Segment2d(new Point2d(-2, 0), new Point2d(0, 0));
                intr.Find();
                Assert.IsTrue(intr.IntersectionType == IntersectionType.POINT);
                Assert.IsTrue(intr.Intersections.Length == 1 && intr.Intersections[0].Point.EpsilonEquals(new Point2d(0, 0)));
            }

            {
                IntrSegment2dSegment2d intr = new IntrSegment2dSegment2d();
                intr.Item0 = new Segment2d(new Point2d(0, 0), new Point2d(10, 0));
                intr.Item1 = new Segment2d(new Point2d(10, 0), new Point2d(12, 0));
                intr.Find();
                Assert.IsTrue(intr.IntersectionType == IntersectionType.POINT);
                Assert.IsTrue(intr.Intersections.Length == 1 && intr.Intersections[0].Point.EpsilonEquals(new Point2d(10, 0)));
            }

            {
                IntrSegment2dSegment2d intr = new IntrSegment2dSegment2d();
                intr.Item0 = new Segment2d(new Point2d(0, 0), new Point2d(10, 0));
                intr.Item1 = new Segment2d(new Point2d(-2, 0), new Point2d(-1, 0));
                intr.Find();
                Assert.IsTrue(intr.IntersectionType == IntersectionType.EMPTY);
            }

            {
                IntrSegment2dSegment2d intr = new IntrSegment2dSegment2d();
                intr.Item0 = new Segment2d(new Point2d(0, 0), new Point2d(10, 0));
                intr.Item1 = new Segment2d(new Point2d(11, 0), new Point2d(12, 0));
                intr.Find();
                Assert.IsTrue(intr.IntersectionType == IntersectionType.EMPTY);
            }
        }
    }
}