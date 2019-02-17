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

            // desarrollo, r, parametro, x, y, c, normal ¿o direction?,
            for (int i = 0; i < this.testData1.Length;)
            {
                double s = this.testData1[i++];
                double r = this.testData1[i++];
                double a = this.testData1[i++];
                double x = this.testData1[i++];
                double y = this.testData1[i++];
                double radio = this.testData1[i++];
                double tg = this.testData1[i++];

                // NOTA: Clip toma las clotoides con s < 0 invertidas en Y.
                if (s < 0)
                {
                    y = -y;
                    tg = 2 * SysMath.PI - tg;
                    radio = -radio;
                }

                double x2, y2;
                ClothoUtils.Clotho(s, r < 0, a, out x2, out y2);
                double radio2 = ClothoUtils.ClothoRadious(s, r < 0, a);
                double tg2 = ClothoUtils.ClothoTangent(s, r < 0, a);
                double direction2 = ClothoUtils.ClothoTangent(s, r < 0, a);

                Assert.IsTrue(x.EpsilonEquals(x2, error));
                Assert.IsTrue(y.EpsilonEquals(y2, error));
                Assert.IsTrue((double.IsInfinity(radio) && double.IsInfinity(radio2)) || radio.EpsilonEquals(radio2, error));
                //Assert.IsTrue(AngleUtils.Ensure0To2Pi(normal).EpsilonEquals(AngleUtils.Ensure0To2Pi(normal2), error));

                //Assert.IsTrue(AngleUtils.Ensure0To2Pi(tg).EpsilonEquals(AngleUtils.Ensure0To2Pi(tg2), error));
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
    }
}