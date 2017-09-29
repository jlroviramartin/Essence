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
using Essence.Geometry.Core.Byte;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Essence.Geometry.Core.Double;
using Essence.Geometry.Core.Float;
using Essence.Geometry.Core.Int;
using Essence.Util.Math.Double;
using Essence.Util.Math.Float;

namespace Essence.Geometry.Core
{
    [TestClass]
    public class VectorUtilsTest
    {
        [TestMethod]
        public void TestConvert_Double()
        {
            // 2D - vectors

            {
                ITuple2_Float v = VectorUtils.Convert<ITuple2_Float>(new Vector2d(1, 2));
                Assert.IsTrue(v.X == 1 && v.Y == 2);
            }
            {
                ITuple2_Int v = VectorUtils.Convert<ITuple2_Int>(new Vector2d(1, 2));
                Assert.IsTrue(v.X == 1 && v.Y == 2);
            }
            {
                ITuple2_Byte v = VectorUtils.Convert<ITuple2_Byte>(new Vector2d(1, 2));
                Assert.IsTrue(v.X == 1 && v.Y == 2);
            }

            {
                ITuple2_Float v = VectorUtils.Convert<ITuple2_Float>(new BuffVector2d(1, 2));
                Assert.IsTrue(v.X == 1 && v.Y == 2);
            }
            {
                ITuple2_Int v = VectorUtils.Convert<ITuple2_Int>(new BuffVector2d(1, 2));
                Assert.IsTrue(v.X == 1 && v.Y == 2);
            }
            {
                ITuple2_Byte v = VectorUtils.Convert<ITuple2_Byte>(new BuffVector2d(1, 2));
                Assert.IsTrue(v.X == 1 && v.Y == 2);
            }

            // 2D - points

            {
                ITuple2_Float v = VectorUtils.Convert<ITuple2_Float>(new Point2d(1, 2));
                Assert.IsTrue(v.X == 1 && v.Y == 2);
            }
            {
                ITuple2_Int v = VectorUtils.Convert<ITuple2_Int>(new Point2d(1, 2));
                Assert.IsTrue(v.X == 1 && v.Y == 2);
            }
            {
                ITuple2_Byte v = VectorUtils.Convert<ITuple2_Byte>(new Point2d(1, 2));
                Assert.IsTrue(v.X == 1 && v.Y == 2);
            }

            {
                ITuple2_Float v = VectorUtils.Convert<ITuple2_Float>(new BuffPoint2d(1, 2));
                Assert.IsTrue(v.X == 1 && v.Y == 2);
            }
            {
                ITuple2_Int v = VectorUtils.Convert<ITuple2_Int>(new BuffPoint2d(1, 2));
                Assert.IsTrue(v.X == 1 && v.Y == 2);
            }
            {
                ITuple2_Byte v = VectorUtils.Convert<ITuple2_Byte>(new BuffPoint2d(1, 2));
                Assert.IsTrue(v.X == 1 && v.Y == 2);
            }

            // 3D - vectors

            {
                ITuple3_Float v = VectorUtils.Convert<ITuple3_Float>(new Vector3d(1, 2, 3));
                Assert.IsTrue(v.X == 1 && v.Y == 2 && v.Z == 3);
            }
            {
                ITuple3_Byte v = VectorUtils.Convert<ITuple3_Byte>(new Vector3d(1, 2, 3));
                Assert.IsTrue(v.X == 1 && v.Y == 2 && v.Z == 3);
            }

            {
                ITuple3_Float v = VectorUtils.Convert<ITuple3_Float>(new BuffVector3d(1, 2, 3));
                Assert.IsTrue(v.X == 1 && v.Y == 2 && v.Z == 3);
            }
            {
                ITuple3_Byte v = VectorUtils.Convert<ITuple3_Byte>(new BuffVector3d(1, 2, 3));
                Assert.IsTrue(v.X == 1 && v.Y == 2 && v.Z == 3);
            }

            // 3D - points

            {
                ITuple3_Float v = VectorUtils.Convert<ITuple3_Float>(new Point3d(1, 2, 3));
                Assert.IsTrue(v.X == 1 && v.Y == 2 && v.Z == 3);
            }
            {
                ITuple3_Byte v = VectorUtils.Convert<ITuple3_Byte>(new Point3d(1, 2, 3));
                Assert.IsTrue(v.X == 1 && v.Y == 2 && v.Z == 3);
            }

            {
                ITuple3_Float v = VectorUtils.Convert<ITuple3_Float>(new BuffPoint3d(1, 2, 3));
                Assert.IsTrue(v.X == 1 && v.Y == 2 && v.Z == 3);
            }
            {
                ITuple3_Byte v = VectorUtils.Convert<ITuple3_Byte>(new BuffPoint3d(1, 2, 3));
                Assert.IsTrue(v.X == 1 && v.Y == 2 && v.Z == 3);
            }

            // 4D - vectors

            {
                ITuple4_Float v = VectorUtils.Convert<ITuple4_Float>(new Vector4d(1, 2, 3, 4));
                Assert.IsTrue(v.X == 1 && v.Y == 2 && v.Z == 3 && v.W == 4);
            }
            {
                ITuple4_Byte v = VectorUtils.Convert<ITuple4_Byte>(new Vector4d(1, 2, 3, 4));
                Assert.IsTrue(v.X == 1 && v.Y == 2 && v.Z == 3 && v.W == 4);
            }

            {
                ITuple4_Float v = VectorUtils.Convert<ITuple4_Float>(new BuffVector4d(1, 2, 3, 4));
                Assert.IsTrue(v.X == 1 && v.Y == 2 && v.Z == 3 && v.W == 4);
            }
            {
                ITuple4_Byte v = VectorUtils.Convert<ITuple4_Byte>(new BuffVector4d(1, 2, 3, 4));
                Assert.IsTrue(v.X == 1 && v.Y == 2 && v.Z == 3 && v.W == 4);
            }

            // 4D - points

            {
                ITuple4_Float v = VectorUtils.Convert<ITuple4_Float>(new Point4d(1, 2, 3, 4));
                Assert.IsTrue(v.X == 1 && v.Y == 2 && v.Z == 3 && v.W == 4);
            }
            {
                ITuple4_Byte v = VectorUtils.Convert<ITuple4_Byte>(new Point4d(1, 2, 3, 4));
                Assert.IsTrue(v.X == 1 && v.Y == 2 && v.Z == 3 && v.W == 4);
            }

            {
                ITuple4_Float v = VectorUtils.Convert<ITuple4_Float>(new BuffPoint4d(1, 2, 3, 4));
                Assert.IsTrue(v.X == 1 && v.Y == 2 && v.Z == 3 && v.W == 4);
            }
            {
                ITuple4_Byte v = VectorUtils.Convert<ITuple4_Byte>(new BuffPoint4d(1, 2, 3, 4));
                Assert.IsTrue(v.X == 1 && v.Y == 2 && v.Z == 3 && v.W == 4);
            }
        }

        [TestMethod]
        public void TestConvert_Int()
        {
            // 2D - vectors

            {
                ITuple2_Float v = VectorUtils.Convert<ITuple2_Float>(new Vector2i(1, 2));
                Assert.IsTrue(v.X == 1 && v.Y == 2);
            }
            {
                ITuple2_Int v = VectorUtils.Convert<ITuple2_Int>(new Vector2i(1, 2));
                Assert.IsTrue(v.X == 1 && v.Y == 2);
            }
            {
                ITuple2_Byte v = VectorUtils.Convert<ITuple2_Byte>(new Vector2i(1, 2));
                Assert.IsTrue(v.X == 1 && v.Y == 2);
            }

            // 2D - points

            {
                ITuple2_Float v = VectorUtils.Convert<ITuple2_Float>(new Point2i(1, 2));
                Assert.IsTrue(v.X == 1 && v.Y == 2);
            }
            {
                ITuple2_Int v = VectorUtils.Convert<ITuple2_Int>(new Point2i(1, 2));
                Assert.IsTrue(v.X == 1 && v.Y == 2);
            }
            {
                ITuple2_Byte v = VectorUtils.Convert<ITuple2_Byte>(new Point2i(1, 2));
                Assert.IsTrue(v.X == 1 && v.Y == 2);
            }
        }

        [TestMethod]
        public void TestConvert_ColorByte()
        {
            // 2D - vectors

            {
                ITuple3_Double v = VectorUtils.Convert<ITuple3_Double>(new Color3b(0, 128, 255));
                Assert.IsTrue(v.X.EpsilonEquals(0) && v.Y.EpsilonEquals(0.5, 0.01) && v.Z.EpsilonEquals(1));
            }
            {
                ITuple3_Float v = VectorUtils.Convert<ITuple3_Float>(new Color3b(0, 128, 255));
                Assert.IsTrue(v.X.EpsilonEquals(0) && v.Y.EpsilonEquals(0.5f, 0.01f) && v.Z.EpsilonEquals(1));
            }
            /*{
                ITuple3_Int v = VectorUtils.Convert<ITuple3_Int>(new Color3b(0, 128, 255));
                Assert.IsTrue(v.X.EpsilonEquals(0) && v.Y.EpsilonEquals(0.5) && v.Z.EpsilonEquals(1));
            }*/
            {
                ITuple3_Byte v = VectorUtils.Convert<ITuple3_Byte>(new Color3b(0, 128, 255));
                Assert.IsTrue(v.X == 0 && v.Y == 128 && v.Z == 255);
            }

            // 2D - points

            {
                ITuple2_Float v = VectorUtils.Convert<ITuple2_Float>(new Point2i(1, 2));
                Assert.IsTrue(v.X == 1 && v.Y == 2);
            }
            {
                ITuple2_Int v = VectorUtils.Convert<ITuple2_Int>(new Point2i(1, 2));
                Assert.IsTrue(v.X == 1 && v.Y == 2);
            }
            {
                ITuple2_Byte v = VectorUtils.Convert<ITuple2_Byte>(new Point2i(1, 2));
                Assert.IsTrue(v.X == 1 && v.Y == 2);
            }
        }
    }
}