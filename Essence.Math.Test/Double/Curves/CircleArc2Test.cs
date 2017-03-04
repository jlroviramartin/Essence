using System;
using Essence.Math.Double;
using Essence.Math.Double.Curves;
using Essence.Math.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SysMath = System.Math;


namespace Essence.Math
{
    [TestClass]
    public class CircleArc2Test
    {
        [TestMethod]
        public void Test1()
        {
            // Angulos pequeños

            // Arco superior ida y vuelta.
            TestThreePoints(new Vec2d(10, 10), 10, 3 * SysMath.PI / 4, 2 * SysMath.PI / 4, SysMath.PI / 4);
            TestThreePoints(new Vec2d(10, 10), 10, SysMath.PI / 4, 2 * SysMath.PI / 4, 3 * SysMath.PI / 4);

            // Arco inferior ida y vuelta.
            TestThreePoints(new Vec2d(10, 10), 10, 5 * SysMath.PI / 4, 6 * SysMath.PI / 4, 7 * SysMath.PI / 4);
            TestThreePoints(new Vec2d(10, 10), 10, 7 * SysMath.PI / 4, 6 * SysMath.PI / 4, 5 * SysMath.PI / 4);

            // Angulos grandes

            // Arco superior ida y vuelta.
            TestThreePoints(new Vec2d(10, 10), 10, 5 * SysMath.PI / 4, 2 * SysMath.PI / 4, -SysMath.PI / 4);
            TestThreePoints(new Vec2d(10, 10), 10, 7 * SysMath.PI / 4, 10 * SysMath.PI / 4, 13 * SysMath.PI / 4);

            // Arco inferior ida y vuelta.
            TestThreePoints(new Vec2d(10, 10), 10, 3 * SysMath.PI / 4, 6 * SysMath.PI / 4, 9 * SysMath.PI / 4);
            TestThreePoints(new Vec2d(10, 10), 10, SysMath.PI / 4, -2 * SysMath.PI / 4, -5 * SysMath.PI / 4);

            // Se prueban 3 puntos, 2 iguales.
            AssertEx.AssertException<Exception>(() =>
            {
                CircleArc2Utils.GetCenter(new Vec2d(10, 10), new Vec2d(10, 10), new Vec2d(30, 10));
            });

            // Se prueban 3 puntos alineados.
            AssertEx.AssertException<Exception>(() =>
            {
                CircleArc2Utils.GetCenter(new Vec2d(10, 10), new Vec2d(20, 10), new Vec2d(30, 10));
            });
        }

        private static void TestThreePoints(Vec2d c, double radius, double a1, double am, double a2)
        {
            CircleArc2 arc = new CircleArc2(c, radius, a1, a2);

            Assert.IsTrue(arc.GetAngle(arc.TMin).EpsilonEquals(a1));
            Assert.IsTrue(arc.GetAngle((arc.TMin + arc.TMax) / 2).EpsilonEquals(am));
            Assert.IsTrue(arc.GetAngle(arc.TMax).EpsilonEquals(a2));

            // Se toman 3 puntos y se calcula el centro, que tiene que coincidir con el centro.
            Vec2d p1 = arc.GetPosition(arc.TMin);
            Vec2d p2 = arc.GetPosition((arc.TMin + arc.TMax) / 2);
            Vec2d p3 = arc.GetPosition(arc.TMax);
            Vec2d cc = CircleArc2Utils.GetCenter(p1, p2, p3);
            Assert.IsTrue(arc.Center.EpsilonEquals(cc));

            // Se calcula el arco formado por los 3 puntos, que tiene que coincidir con el arco.
            CircleArc2 arc2 = CircleArc2Utils.ThreePoints(p1, p2, p3);

            Assert.IsTrue(arc.Angle1.EpsilonEquals(arc2.Angle1));
            //Assert.IsTrue(arc.Angle2.EpsilonEquals(arc2.Angle2));
            Assert.IsTrue(arc.AdvAngle.EpsilonEquals(arc2.AdvAngle));
            Assert.IsTrue(arc.Center.EpsilonEquals(arc2.Center));
            Assert.IsTrue(arc.Radius.EpsilonEquals(arc2.Radius));

            Assert.IsTrue(arc.TMin.EpsilonEquals(arc2.TMin));
            Assert.IsTrue(arc.TMax.EpsilonEquals(arc2.TMax));
        }
    }
}