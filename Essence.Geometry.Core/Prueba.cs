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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Essence.Geometry.Core
{
    public static class Prueba
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Pruebas de rendimiento");

            int count = 100;
            int count2 = 1000000;

            List<SingleTest> tests = new List<SingleTest>();

            List<IVec> vectors = new List<IVec>();
            for (int i = 0; i < count2; i++)
            {
                vectors.Add(new Vec(i, i, i, i));
            }

            List<Vec> vectors2 = new List<Vec>();
            for (int i = 0; i < count2; i++)
            {
                vectors2.Add(new Vec(i, i, i, i));
            }

            // BuffVec
            tests.Add(new SingleTest(
                          "Test.BuffVec",
                          () =>
                          {
                              BuffVec sum = new BuffVec();
                              for (int i = 0; i < vectors.Count; i++)
                              {
                                  sum.Add(vectors[i]);
                              }
                              return sum;
                          })
            );

            tests.Add(new SingleTest(
                          "Test.BuffVec (2)",
                          () =>
                          {
                              BuffVec sum = new BuffVec();
                              for (int i = 0; i < vectors2.Count; i++)
                              {
                                  sum.Add(vectors2[i]);
                              }
                              return sum;
                          })
            );

            tests.Add(new SingleTest(
                          "Test.BuffVec.Add_2",
                          () =>
                          {
                              BuffVec sum = new BuffVec();
                              for (int i = 0; i < vectors.Count; i++)
                              {
                                  sum.Add_2(vectors[i]);
                              }
                              return sum;
                          })
            );

            tests.Add(new SingleTest(
                          "Test.BuffVec.Add_2 (2) ",
                          () =>
                          {
                              BuffVec sum = new BuffVec();
                              for (int i = 0; i < vectors2.Count; i++)
                              {
                                  sum.Add_2(vectors2[i]);
                              }
                              return sum;
                          })
            );

            // BuffVec2
            // ***** 2a *****
            tests.Add(new SingleTest(
                          "Test.BuffVec2",
                          () =>
                          {
                              BuffVec_v2 sum = new BuffVec_v2();
                              for (int i = 0; i < vectors.Count; i++)
                              {
                                  sum.Add(vectors[i]);
                              }
                              return sum;
                          })
            );

            tests.Add(new SingleTest(
                          "Test.BuffVec2 (2)",
                          () =>
                          {
                              BuffVec_v2 sum = new BuffVec_v2();
                              for (int i = 0; i < vectors2.Count; i++)
                              {
                                  sum.Add(vectors2[i]);
                              }
                              return sum;
                          })
            );

            tests.Add(new SingleTest(
                          "Test.BuffVec2.Add_2",
                          () =>
                          {
                              BuffVec_v2 sum = new BuffVec_v2();
                              for (int i = 0; i < vectors.Count; i++)
                              {
                                  sum.Add_2(vectors[i]);
                              }
                              return sum;
                          })
            );

            tests.Add(new SingleTest(
                          "Test.BuffVec2.Add_2 (2)",
                          () =>
                          {
                              BuffVec_v2 sum = new BuffVec_v2();
                              for (int i = 0; i < vectors2.Count; i++)
                              {
                                  sum.Add_2(vectors2[i]);
                              }
                              return sum;
                          })
            );

            // Vec
            tests.Add(new SingleTest(
                          "Test.Vec",
                          () =>
                          {
                              Vec sum = new Vec();
                              for (int i = 0; i < vectors.Count; i++)
                              {
                                  sum = sum.Add(vectors[i]);
                              }
                              return sum;
                          })
            );

            // ***** Mejor rendimiento *****
            tests.Add(new SingleTest(
                          "Test.Vec (2)",
                          () =>
                          {
                              Vec sum = new Vec();
                              for (int i = 0; i < vectors2.Count; i++)
                              {
                                  sum = sum.Add(vectors2[i]);
                              }
                              return sum;
                          })
            );

            tests.Add(new SingleTest(
                          "Test.Vec.Add_2",
                          () =>
                          {
                              Vec sum = new Vec();
                              for (int i = 0; i < vectors.Count; i++)
                              {
                                  sum = sum.Add_2(vectors[i]);
                              }
                              return sum;
                          })
            );

            tests.Add(new SingleTest(
                          "Test.Vec.Add_2 (2)",
                          () =>
                          {
                              Vec sum = new Vec();
                              for (int i = 0; i < vectors2.Count; i++)
                              {
                                  sum = sum.Add_2(vectors2[i]);
                              }
                              return sum;
                          })
            );

            // Ultimas pruebas
            tests.Add(new SingleTest(
                          "Test.BuffVec2.Add_3",
                          () =>
                          {
                              BuffVec_v2 sum = new BuffVec_v2();
                              for (int i = 0; i < vectors.Count; i++)
                              {
                                  sum.Add_3(vectors[i]);
                              }
                              return sum;
                          })
            );

            tests.Add(new SingleTest(
                          "Test.BuffVec2.Add_3 (2)",
                          () =>
                          {
                              BuffVec_v2 sum = new BuffVec_v2();
                              for (int i = 0; i < vectors2.Count; i++)
                              {
                                  sum.Add_3(vectors2[i]);
                              }
                              return sum;
                          })
            );

            tests.Add(new SingleTest(
                          "Test.BuffVec.Add_3",
                          () =>
                          {
                              BuffVec sum = new BuffVec();
                              for (int i = 0; i < vectors.Count; i++)
                              {
                                  sum.Add_3(vectors[i]);
                              }
                              return sum;
                          })
            );

            tests.Add(new SingleTest(
                          "Test.BuffVec.Add_3 (2)",
                          () =>
                          {
                              BuffVec sum = new BuffVec();
                              for (int i = 0; i < vectors2.Count; i++)
                              {
                                  sum.Add_3(vectors2[i]);
                              }
                              return sum;
                          })
            );

            /*SingleTest aux = tests[9];
            tests.Clear();
            tests.Add(aux);*/

            foreach (SingleTest test in tests)
            {
                Measure(test, count);
            }

            Console.WriteLine("Resultado");

            tests.Sort((a, b) => a.Time.CompareTo(b.Time));
            long minTime = tests[0].Time;
            foreach (SingleTest test in tests)
            {
                Console.WriteLine("{0,-30} {1,10:F3} : {2,10}", test.Name, test.Time / (double)minTime, test.Time);
            }

            Console.ReadKey();
            foreach (object result in tests[0].Results)
            {
                Console.WriteLine("" + result);
            }
        }

        private class SingleTest
        {
            public SingleTest(string name, Func<object> action)
            {
                this.Name = name;
                this.Action = action;
                this.Results = new List<object>();
            }

            public string Name { get; }
            public Func<object> Action { get; }
            public List<object> Results { get; }
            public long Time { get; set; }
        }

        private static void Measure(SingleTest test, int count)
        {
            List<long> times = new List<long>();

            Stopwatch s = new Stopwatch();
            for (int i = 0; i < count; i++)
            {
                Console.Write("#");

                s.Start();
                object result = test.Action();
                s.Stop();
                times.Add(s.ElapsedMilliseconds);

                test.Results.Add(result);
            }
            times.Sort();
            for (int i = 0; i < 3; i++)
            {
                times.Remove(0);
                times.Remove(times.Count - 1);
            }
            long med = times.Sum() / times.Count;
            test.Time = med;

            Console.WriteLine();
        }

        public interface ITup
        {
            void GetCoords<TSetter>(TSetter setter) where TSetter : struct, ISetter;
        }

        public interface ITup_Double : ITup
        {
            double X { get; }
            double Y { get; }
            double Z { get; }
            double W { get; }

            Vec GetVec();
        }

        public interface IOpTup : ITup
        {
        }

        public interface IOpTup_Double : IOpTup, ITup_Double
        {
            void Set(double x, double y, double z, double w);

            new double X { set; }
            new double Y { set; }
            new double Z { set; }
            new double W { set; }
        }

        public interface IVec : ITup
        {
        }

        public interface IOpVec : IVec, IOpTup
        {
            void Add(IVec other);

            void Add_2(IVec other);
        }

        public struct Vec : IVec, ITup_Double
        {
            public Vec(double x, double y, double z, double w)
            {
                this.x = x;
                this.y = y;
                this.z = z;
                this.w = w;
            }

            public double X
            {
                get { return this.x; }
            }

            public double Y
            {
                get { return this.y; }
            }

            public double Z
            {
                get { return this.z; }
            }

            public double W
            {
                get { return this.w; }
            }

            public Vec GetVec()
            {
                return this;
            }

            public void GetCoords<TSetter>(TSetter setter) where TSetter : struct, ISetter
            {
                setter.Set(this.x, this.y, this.z, this.w);
            }

            public Vec Add(Vec other)
            {
                return new Vec(this.x + other.x, this.y + other.y, this.z + other.z, this.w + other.w);
            }

            public Vec Add(IVec other)
            {
                ITup_Double _other = GetDouble(other);
                return new Vec(this.x + _other.X, this.y + _other.Y, this.z + _other.Z, this.w + _other.W);
            }

            public Vec Add_2(IVec other)
            {
                Vec _other = GetDouble(other).GetVec();
                return new Vec(this.x + _other.x, this.y + _other.y, this.z + _other.z, this.w + _other.w);
            }

            private static ITup_Double GetDouble(IVec v)
            {
                ITup_Double aux = v as ITup_Double;
                if (aux != null)
                {
                    return aux;
                }
                return null;
            }

            public readonly double x;
            public readonly double y;
            public readonly double z;
            public readonly double w;
        }

        public sealed class BuffVec : IOpVec, IOpTup_Double, ISetter
        {
            public BuffVec()
                : this(0, 0, 0, 0)
            {
            }

            public BuffVec(double x, double y, double z, double w)
            {
                this.x = x;
                this.y = y;
                this.z = z;
                this.w = w;
            }

            private double x;
            private double y;
            private double z;
            private double w;

            public void GetCoords<TSetter>(TSetter setter) where TSetter : struct, ISetter
            {
                setter.Set(this.x, this.y, this.z, this.w);
            }

            public void Add(IVec other)
            {
                ITup_Double _other = GetDouble(other);
                this.Set(this.x + _other.X, this.y + _other.Y, this.z + _other.Z, this.w + _other.W);
            }

            public void Add_2(IVec other)
            {
                Vec _other = GetDouble(other).GetVec();
                this.Set(this.x + _other.x, this.y + _other.y, this.z + _other.z, this.w + _other.w);
            }

            public void Add_3(IVec other)
            {
                BuffVec_v2 _other = new BuffVec_v2();
                other.GetCoords(_other);
                this.Set(this.x + _other.x, this.y + _other.y, this.z + _other.z, this.w + _other.w);
            }

            public void Set(double x, double y, double z, double w)
            {
                this.x = x;
                this.y = y;
                this.z = z;
                this.w = w;
            }

            public double X
            {
                get { return this.x; }
                set { this.x = value; }
            }

            public double Y
            {
                get { return this.y; }
                set { this.y = value; }
            }

            public double Z
            {
                get { return this.z; }
                set { this.z = value; }
            }

            public double W
            {
                get { return this.w; }
                set { this.w = value; }
            }

            public Vec GetVec()
            {
                return new Vec(this.x, this.y, this.z, this.w);
            }

            private static ITup_Double GetDouble(IVec v)
            {
                ITup_Double aux = v as ITup_Double;
                if (aux != null)
                {
                    return aux;
                }
                return null;
            }
        }

        public struct BuffVec_v2 : IOpVec, IOpTup_Double, ISetter
        {
            public BuffVec_v2(double x, double y, double z, double w)
            {
                this.x = x;
                this.y = y;
                this.z = z;
                this.w = w;
            }

            public double x;
            public double y;
            public double z;
            public double w;

            public void GetCoords<TSetter>(TSetter setter) where TSetter : struct, ISetter
            {
                setter.Set(this.x, this.y, this.z, this.w);
            }

            public void Add(IVec other)
            {
                ITup_Double _other = GetDouble(other);
                this.Set(this.x + _other.X, this.y + _other.Y, this.z + _other.Z, this.w + _other.W);
            }

            public void Add_2(IVec other)
            {
                Vec _other = GetDouble(other).GetVec();
                this.Set(this.x + _other.x, this.y + _other.y, this.z + _other.z, this.w + _other.w);
            }

            public void Add_3(IVec other)
            {
                BuffVec_v2 _other = new BuffVec_v2();
                other.GetCoords(_other);
                this.Set(this.x + _other.x, this.y + _other.y, this.z + _other.z, this.w + _other.w);
            }

            public void Set(double x, double y, double z, double w)
            {
                this.x = x;
                this.y = y;
                this.z = z;
                this.w = w;
            }

            public double X
            {
                get { return this.x; }
                set { this.x = value; }
            }

            public double Y
            {
                get { return this.y; }
                set { this.y = value; }
            }

            public double Z
            {
                get { return this.z; }
                set { this.z = value; }
            }

            public double W
            {
                get { return this.w; }
                set { this.w = value; }
            }

            public Vec GetVec()
            {
                return new Vec(this.x, this.y, this.z, this.w);
            }

            private static ITup_Double GetDouble(IVec v)
            {
                ITup_Double aux = v as ITup_Double;
                if (aux != null)
                {
                    return aux;
                }
                return null;
            }
        }

        public interface ISetter
        {
            void Set(double x, double y, double z, double w);
        }
    }
}