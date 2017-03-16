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

using Essence.Geometry.Core;
using Essence.Geometry.Core.Double;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Essence.Geometry.Test
{
    [TestClass]
    public class TransformMatrix2DTest
    {
        [TestMethod]
        public void Test1()
        {
            ITransform2D transform = new Transform2DMatrix(Matrix3x3dUtils.Translate(10, 10));

            {
                Vector2d v = new Vector2d(5, 5);
                Vector2d ret = transform.DoTransform(v);
                Assert.IsTrue(ret.EpsilonEquals(new Vector2d(5, 5)));
            }

            {
                Point2d v = new Point2d(5, 5);
                Point2d ret = transform.DoTransform(v);
                Assert.IsTrue(ret.EpsilonEquals(new Point2d(15, 15)));
            }

            /*{
                Vector2d v = new Vector2d(5, 5);
                Vector2d ret = transform.Transform<Vector2d, double>(v);
                Assert.IsTrue(ret.EpsilonEquals(new Vector2d(5, 5)));
            }

            {
                Point2d v = new Point2d(5, 5);
                Point2d ret = transform.Transform<Point2d, Vector2d, double>(v);
                Assert.IsTrue(ret.EpsilonEquals(new Point2d(15, 15)));
            }*/
        }
    }
}