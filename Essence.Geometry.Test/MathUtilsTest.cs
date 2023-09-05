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

using Essence.Geometry.Wave;
using Essence.Maths.Double;
using Essence.Util.Math.Double;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SysMath = System.Math;

namespace Essence.Geometry
{
    [TestClass]
    public partial class MathUtilsTest
    {
        /// <summary>Radio apartir del cual se considera una recta.</summary>
        private const double RRECTA = 1e20;

        [TestMethod]
        public void Test1()
        {
            const double error = 1e-8;

            // desarrollo, r, parametro, x, y, c, normal ¿o direccion?,
            for (int i = 0; i < this.testData1.Length;)
            {
                double l = this.testData1[i++];
                double r = this.testData1[i++];
                double a = this.testData1[i++];
                double x = this.testData1[i++];
                double y = this.testData1[i++];
                double radio = this.testData1[i++];
                double tg = this.testData1[i++];

                bool invertY = ((l < 0 && r > 0) || (l > 0 && r < 0));

                double x2, y2;
                ClothoUtils.Clotho(l, invertY, a, out x2, out y2);
                double radio2 = ClothoUtils.ClothoRadius(l, invertY, a);
                double tg2 = ClothoUtils.ClothoTangent(l, invertY, a);

                Assert.IsTrue(x.EpsilonEquals(x2, error));
                Assert.IsTrue(y.EpsilonEquals(y2, error));
                Assert.IsTrue((double.IsInfinity(radio) && double.IsInfinity(radio2)) || radio.EpsilonEquals(radio2, error));
                Assert.IsTrue(AngleUtils.Ensure0To2Pi(tg).EpsilonEquals(AngleUtils.Ensure0To2Pi(tg2), error));
            }
        }

        [TestMethod]
        public void Test2()
        {
            using (MaterialFormat mf = new MaterialFormat(@"C:\Temp\Default.mtl"))
            {
                mf.DefaultColors();
            }

            using (WavefrontFormat wf = new WavefrontFormat(@"C:\Temp\Radio+.obj"))
            {
                wf.LoadMaterialLib("Default.mtl");
                wf.DrawClotho(false, 10, "Red", "Yellow", "Green", "Magenta");
            }

            using (WavefrontFormat wf = new WavefrontFormat(@"C:\Temp\Radio-.obj"))
            {
                wf.LoadMaterialLib("Default.mtl");
                wf.DrawClotho(true, 10, "Blue", "Yellow", "Green", "Magenta");
            }
        }

        [TestMethod]
        public void Test3()
        {
            int i = 0;
            while (i < this.testData3_sc.Length)
            {
                double v1 = this.testData3_sc[i++];
                double s1 = FresnelUtils.FresnelS(v1);
                double c1 = FresnelUtils.FresnelC(v1);

                double s2 = this.testData3_sc[i++];
                double c2 = this.testData3_sc[i++];

                Assert.IsTrue(s1.EpsilonEquals(s2));
                Assert.IsTrue(c1.EpsilonEquals(c2));
            }
        }

        [TestMethod]
        public void Test4()
        {
            Assert.IsFalse(0.0.EpsilonEquals(1));
            Assert.IsTrue(0.0.EpsilonEquals(0.01, 0.1));
            Assert.IsTrue(0.0.EpsilonEquals(0.0));
            Assert.IsTrue(0.0.EpsilonEquals(-0.01, 0.1));
            Assert.IsFalse(0.0.EpsilonEquals(-1));
            Assert.IsFalse(0.0.EpsilonEquals(double.PositiveInfinity));
            Assert.IsFalse(0.0.EpsilonEquals(double.NegativeInfinity));
            Assert.IsTrue(double.PositiveInfinity.EpsilonEquals(double.PositiveInfinity));
            Assert.IsFalse(double.PositiveInfinity.EpsilonEquals(double.NegativeInfinity));
            Assert.IsFalse(double.NegativeInfinity.EpsilonG(double.PositiveInfinity));
            Assert.IsTrue(double.NegativeInfinity.EpsilonEquals(double.NegativeInfinity));

            Assert.IsFalse(0.0.EpsilonG(1));
            Assert.IsFalse(0.0.EpsilonG(0.01, 0.1));
            Assert.IsFalse(0.0.EpsilonG(0.0));
            Assert.IsFalse(0.0.EpsilonG(-0.01, 0.1));
            Assert.IsTrue(0.0.EpsilonG(-1));
            Assert.IsFalse(0.0.EpsilonG(double.PositiveInfinity));
            Assert.IsTrue(0.0.EpsilonG(double.NegativeInfinity));
            Assert.IsFalse(double.PositiveInfinity.EpsilonG(double.PositiveInfinity));
            Assert.IsTrue(double.PositiveInfinity.EpsilonG(double.NegativeInfinity));
            Assert.IsFalse(double.NegativeInfinity.EpsilonG(double.PositiveInfinity));
            Assert.IsFalse(double.NegativeInfinity.EpsilonG(double.NegativeInfinity));

            Assert.IsFalse(0.0.EpsilonGE(1));
            Assert.IsTrue(0.0.EpsilonGE(0.01, 0.1));
            Assert.IsTrue(0.0.EpsilonGE(0.0));
            Assert.IsTrue(0.0.EpsilonGE(-0.01, 0.1));
            Assert.IsTrue(0.0.EpsilonGE(-1));
            Assert.IsFalse(0.0.EpsilonGE(double.PositiveInfinity));
            Assert.IsTrue(0.0.EpsilonGE(double.NegativeInfinity));
            Assert.IsTrue(double.PositiveInfinity.EpsilonGE(double.NegativeInfinity));
            Assert.IsTrue(double.PositiveInfinity.EpsilonGE(double.PositiveInfinity));
            Assert.IsFalse(double.NegativeInfinity.EpsilonGE(double.PositiveInfinity));
            Assert.IsTrue(double.NegativeInfinity.EpsilonGE(double.NegativeInfinity));

            Assert.IsTrue(0.0.EpsilonL(1));
            Assert.IsFalse(0.0.EpsilonL(0.01, 0.1));
            Assert.IsFalse(0.0.EpsilonL(0.0));
            Assert.IsFalse(0.0.EpsilonL(-0.01, 0.1));
            Assert.IsFalse(0.0.EpsilonL(-1));
            Assert.IsTrue(0.0.EpsilonL(double.PositiveInfinity));
            Assert.IsFalse(0.0.EpsilonL(double.NegativeInfinity));
            Assert.IsFalse(double.PositiveInfinity.EpsilonL(double.PositiveInfinity));
            Assert.IsFalse(double.PositiveInfinity.EpsilonL(double.NegativeInfinity));
            Assert.IsTrue(double.NegativeInfinity.EpsilonL(double.PositiveInfinity));
            Assert.IsFalse(double.NegativeInfinity.EpsilonL(double.NegativeInfinity));

            Assert.IsTrue(0.0.EpsilonLE(1));
            Assert.IsTrue(0.0.EpsilonLE(0.01, 0.1));
            Assert.IsTrue(0.0.EpsilonLE(0.0));
            Assert.IsTrue(0.0.EpsilonLE(-0.01, 0.1));
            Assert.IsFalse(0.0.EpsilonLE(-1));
            Assert.IsTrue(0.0.EpsilonLE(double.PositiveInfinity));
            Assert.IsFalse(0.0.EpsilonLE(double.NegativeInfinity));
            Assert.IsTrue(double.PositiveInfinity.EpsilonLE(double.PositiveInfinity));
            Assert.IsFalse(double.PositiveInfinity.EpsilonLE(double.NegativeInfinity));
            Assert.IsTrue(double.NegativeInfinity.EpsilonLE(double.PositiveInfinity));
            Assert.IsTrue(double.NegativeInfinity.EpsilonLE(double.NegativeInfinity));
        }
    }
}