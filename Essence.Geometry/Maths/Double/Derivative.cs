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
using System.Diagnostics.Contracts;
using SysMath = System.Math;
using UnaryFunction = System.Func<double, double>;

namespace Essence.Maths.Double
{
    /// <summary>
    /// http://stackoverflow.com/questions/373186/mathematical-derivation-with-c
    /// http://www.holoborodko.com/pavel/numerical-methods/numerical-derivative/central-differences/
    /// http://www.variousconsequences.com/2009/05/maxima-functions-to-generate-finite.html
    /// http://mathfaculty.fullerton.edu/mathews/n2003/NumericalDiffFormulaeMod.html
    /// </summary>
    public static class Derivative
    {
        public static UnaryFunction Central(UnaryFunction f, int order, int count, double h = 10e-6)
        {
            Contract.Assert(order > 0);
            switch (count)
            {
                case 3:
                    return t => central3p[order - 1](f, t, h);
                case 5:
                    return t => central5p[order - 1](f, t, h);
                case 7:
                    return t => central7p[order - 1](f, t, h);
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        public static UnaryFunction Left(UnaryFunction f, int order, int count, double h = 10e-6)
        {
            Contract.Assert(order > 0);
            switch (count)
            {
                case 2:
                    return t => left2p[order - 1](f, t, h);
                case 3:
                    return t => left3p[order - 1](f, t, h);
                case 4:
                    return t => left4p[order - 1](f, t, h);
                case 5:
                    return t => left5p[order - 1](f, t, h);
                case 6:
                    return t => left6p[order - 1](f, t, h);
                case 7:
                    return t => left7p[order - 1](f, t, h);
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        public static UnaryFunction Right(UnaryFunction f, int order, int count, double h = 10e-6)
        {
            Contract.Assert(order > 0);
            switch (count)
            {
                case 2:
                    return t => right2p[order - 1](f, t, h);
                case 3:
                    return t => right3p[order - 1](f, t, h);
                case 4:
                    return t => right4p[order - 1](f, t, h);
                case 5:
                    return t => right5p[order - 1](f, t, h);
                case 6:
                    return t => right6p[order - 1](f, t, h);
                case 7:
                    return t => right7p[order - 1](f, t, h);
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        #region 2 puntos

        // for i:1 thru 1 do print("(f, x0, h) => ", explicit_fde([-1,0],i));
        private static readonly Func<Func<double, double>, double, double, double>[] left2p =
        {
            (f, x0, h) => -(f(x0 - h) - f(x0)) / h,
        };

        private static readonly Func<double, double>[] left2p_error =
        {
            (h) => SysMath.Abs(h),
        };

        // for i:1 thru 1 do print("(f, x0, h) => ", explicit_fde([0,1],i));
        private static readonly Func<Func<double, double>, double, double, double>[] right2p =
        {
            (f, x0, h) => (f(x0 + h) - f(x0)) / h,
        };

        private static readonly Func<double, double>[] right2p_error =
        {
            (h) => SysMath.Abs(h),
        };

        #endregion 2 puntos

        #region 3 puntos

        // for i:1 thru 2 do print("(f, x0, h) => ", explicit_fde([-1,0,1],i));
        private static readonly Func<Func<double, double>, double, double, double>[] central3p =
        {
            (f, x0, h) => (f(x0 + h) - f(x0 - h)) / (2 * h),
            (f, x0, h) => (f(x0 + h) + f(x0 - h) - 2 * f(x0)) / Exp(h, 2)
        };

        private static readonly Func<double, double>[] central3p_error =
        {
            (h) => SysMath.Abs(Exp(h, 2)),
            (h) => SysMath.Abs(Exp(h, 2)),
        };

        // for i:1 thru 2 do print("(f, x0, h) => ", explicit_fde([-2,-1,0],i));
        private static readonly Func<Func<double, double>, double, double, double>[] left3p =
        {
            (f, x0, h) => -(4 * f(x0 - h) - f(x0 - 2 * h) - 3 * f(x0)) / (2 * h),
            (f, x0, h) => -(2 * f(x0 - h) - f(x0 - 2 * h) - f(x0)) / Exp(h, 2)
        };

        private static readonly Func<double, double>[] left3p_error =
        {
            (h) => SysMath.Abs(Exp(h, 2)),
            (h) => SysMath.Abs(h),
        };

        // for i:1 thru 2 do print("(f, x0, h) => ", explicit_fde([0,1,2],i));
        private static readonly Func<Func<double, double>, double, double, double>[] right3p =
        {
            (f, x0, h) => -(f(x0 + 2 * h) - 4 * f(x0 + h) + 3 * f(x0)) / (2 * h),
            (f, x0, h) => (f(x0 + 2 * h) - 2 * f(x0 + h) + f(x0)) / Exp(h, 2)
        };

        private static readonly Func<double, double>[] right3p_error =
        {
            (h) => SysMath.Abs(Exp(h, 2)),
            (h) => SysMath.Abs(h),
        };

        #endregion 3 puntos

        #region 4 puntos

        // for i:1 thru 3 do print("(f, x0, h) => ", explicit_fde([-3,-2,-1,0],i));
        private static readonly Func<Func<double, double>, double, double, double>[] left4p =
        {
            (f, x0, h) => -(18 * f(x0 - h) - 9 * f(x0 - 2 * h) + 2 * f(x0 - 3 * h) - 11 * f(x0)) / (6 * h),
            (f, x0, h) => -(5 * f(x0 - h) - 4 * f(x0 - 2 * h) + f(x0 - 3 * h) - 2 * f(x0)) / Exp(h, 2),
            (f, x0, h) => -(3 * f(x0 - h) - 3 * f(x0 - 2 * h) + f(x0 - 3 * h) - f(x0)) / Exp(h, 3),
        };

        private static readonly Func<double, double>[] left4p_error =
        {
            (h) => SysMath.Abs(Exp(h, 3)),
            (h) => SysMath.Abs(Exp(h, 2)),
            (h) => SysMath.Abs(h),
        };

        // for i:1 thru 3 do print("(f, x0, h) => ", explicit_fde([0,1,2,3],i));
        private static readonly Func<Func<double, double>, double, double, double>[] right4p =
        {
            (f, x0, h) => (2 * f(x0 + 3 * h) - 9 * f(x0 + 2 * h) + 18 * f(x0 + h) - 11 * f(x0)) / (6 * h),
            (f, x0, h) => -(f(x0 + 3 * h) - 4 * f(x0 + 2 * h) + 5 * f(x0 + h) - 2 * f(x0)) / Exp(h, 2),
            (f, x0, h) => (f(x0 + 3 * h) - 3 * f(x0 + 2 * h) + 3 * f(x0 + h) - f(x0)) / Exp(h, 3)
        };

        private static readonly Func<double, double>[] right4p_error =
        {
            (h) => SysMath.Abs(Exp(h, 3)),
            (h) => SysMath.Abs(Exp(h, 2)),
            (h) => SysMath.Abs(h),
        };

        #endregion 4 puntos

        #region 5 puntos

        // for i:1 thru 4 do print("(f, x0, h) => ", explicit_fde([-2,-1,0,1,2],i));
        private static readonly Func<Func<double, double>, double, double, double>[] central5p =
        {
            (f, x0, h) => -(f(x0 + 2 * h) - 8 * f(x0 + h) + 8 * f(x0 - h) - f(x0 - 2 * h)) / (12 * h),
            (f, x0, h) => -(f(x0 + 2 * h) - 16 * f(x0 + h) - 16 * f(x0 - h) + f(x0 - 2 * h) + 30 * f(x0)) / (12 * Exp(h, 2)),
            (f, x0, h) => (f(x0 + 2 * h) - 2 * f(x0 + h) + 2 * f(x0 - h) - f(x0 - 2 * h)) / (2 * Exp(h, 3)),
            (f, x0, h) => (f(x0 + 2 * h) - 4 * f(x0 + h) - 4 * f(x0 - h) + f(x0 - 2 * h) + 6 * f(x0)) / Exp(h, 4)
        };

        private static readonly Func<double, double>[] central5p_error =
        {
            (h) => SysMath.Abs(Exp(h, 4)),
            (h) => SysMath.Abs(Exp(h, 4)),
            (h) => SysMath.Abs(Exp(h, 2)),
            (h) => SysMath.Abs(Exp(h, 2)),
        };

        // for i:1 thru 4 do print("(f, x0, h) => ", explicit_fde([-4,-3,-2,-1,0],i));
        private static readonly Func<Func<double, double>, double, double, double>[] left5p =
        {
            (f, x0, h) => -(48 * f(x0 - h) - 36 * f(x0 - 2 * h) + 16 * f(x0 - 3 * h) - 3 * f(x0 - 4 * h) - 25 * f(x0)) / (12 * h),
            (f, x0, h) => -(104 * f(x0 - h) - 114 * f(x0 - 2 * h) + 56 * f(x0 - 3 * h) - 11 * f(x0 - 4 * h) - 35 * f(x0)) / (12 * Exp(h, 2)),
            (f, x0, h) => -(18 * f(x0 - h) - 24 * f(x0 - 2 * h) + 14 * f(x0 - 3 * h) - 3 * f(x0 - 4 * h) - 5 * f(x0)) / (2 * Exp(h, 3)),
            (f, x0, h) => -(4 * f(x0 - h) - 6 * f(x0 - 2 * h) + 4 * f(x0 - 3 * h) - f(x0 - 4 * h) - f(x0)) / Exp(h, 4),
        };

        private static readonly Func<double, double>[] left5p_error =
        {
            (h) => SysMath.Abs(Exp(h, 4)),
            (h) => SysMath.Abs(Exp(h, 3)),
            (h) => SysMath.Abs(Exp(h, 2)),
            (h) => SysMath.Abs(h),
        };

        // for i:1 thru 4 do print("(f, x0, h) => ", explicit_fde([0,1,2,3,4],i));
        private static readonly Func<Func<double, double>, double, double, double>[] right5p =
        {
            (f, x0, h) => -(3 * f(x0 + 4 * h) - 16 * f(x0 + 3 * h) + 36 * f(x0 + 2 * h) - 48 * f(x0 + h) + 25 * f(x0)) / (12 * h),
            (f, x0, h) => (11 * f(x0 + 4 * h) - 56 * f(x0 + 3 * h) + 114 * f(x0 + 2 * h) - 104 * f(x0 + h) + 35 * f(x0)) / (12 * Exp(h, 2)),
            (f, x0, h) => -(3 * f(x0 + 4 * h) - 14 * f(x0 + 3 * h) + 24 * f(x0 + 2 * h) - 18 * f(x0 + h) + 5 * f(x0)) / (2 * Exp(h, 3)),
            (f, x0, h) => (f(x0 + 4 * h) - 4 * f(x0 + 3 * h) + 6 * f(x0 + 2 * h) - 4 * f(x0 + h) + f(x0)) / Exp(h, 4),
        };

        private static readonly Func<double, double>[] right5p_error =
        {
            (h) => SysMath.Abs(Exp(h, 4)),
            (h) => SysMath.Abs(Exp(h, 3)),
            (h) => SysMath.Abs(Exp(h, 2)),
            (h) => SysMath.Abs(h),
        };

        #endregion 5 puntos

        #region 6 puntos

        // for i:1 thru 5 do print("(f, x0, h) => ", explicit_fde([-5,-4,-3,-2,-1,0],i));
        private static readonly Func<Func<double, double>, double, double, double>[] left6p =
        {
            (f, x0, h) => -(300 * f(x0 - h) - 300 * f(x0 - 2 * h) + 200 * f(x0 - 3 * h) - 75 * f(x0 - 4 * h) + 12 * f(x0 - 5 * h) - 137 * f(x0)) / (60 * h),
            (f, x0, h) => -(154 * f(x0 - h) - 214 * f(x0 - 2 * h) + 156 * f(x0 - 3 * h) - 61 * f(x0 - 4 * h) + 10 * f(x0 - 5 * h) - 45 * f(x0)) / (12 * Exp(h, 2)),
            (f, x0, h) => -(71 * f(x0 - h) - 118 * f(x0 - 2 * h) + 98 * f(x0 - 3 * h) - 41 * f(x0 - 4 * h) + 7 * f(x0 - 5 * h) - 17 * f(x0)) / (4 * Exp(h, 3)),
            (f, x0, h) => -(14 * f(x0 - h) - 26 * f(x0 - 2 * h) + 24 * f(x0 - 3 * h) - 11 * f(x0 - 4 * h) + 2 * f(x0 - 5 * h) - 3 * f(x0)) / Exp(h, 4),
            (f, x0, h) => -(5 * f(x0 - h) - 10 * f(x0 - 2 * h) + 10 * f(x0 - 3 * h) - 5 * f(x0 - 4 * h) + f(x0 - 5 * h) - f(x0)) / Exp(h, 5),
        };

        private static readonly Func<double, double>[] left6p_error =
        {
            (h) => SysMath.Abs(Exp(h, 5)),
            (h) => SysMath.Abs(Exp(h, 4)),
            (h) => SysMath.Abs(Exp(h, 3)),
            (h) => SysMath.Abs(Exp(h, 2)),
            (h) => SysMath.Abs(h),
        };

        // for i:1 thru 5 do print("(f, x0, h) => ", explicit_fde([0,1,2,3,4,5],i));
        private static readonly Func<Func<double, double>, double, double, double>[] right6p =
        {
            (f, x0, h) => (12 * f(x0 + 5 * h) - 75 * f(x0 + 4 * h) + 200 * f(x0 + 3 * h) - 300 * f(x0 + 2 * h) + 300 * f(x0 + h) - 137 * f(x0)) / (60 * h),
            (f, x0, h) => -(10 * f(x0 + 5 * h) - 61 * f(x0 + 4 * h) + 156 * f(x0 + 3 * h) - 214 * f(x0 + 2 * h) + 154 * f(x0 + h) - 45 * f(x0)) / (12 * Exp(h, 2)),
            (f, x0, h) => (7 * f(x0 + 5 * h) - 41 * f(x0 + 4 * h) + 98 * f(x0 + 3 * h) - 118 * f(x0 + 2 * h) + 71 * f(x0 + h) - 17 * f(x0)) / (4 * Exp(h, 3)),
            (f, x0, h) => -(2 * f(x0 + 5 * h) - 11 * f(x0 + 4 * h) + 24 * f(x0 + 3 * h) - 26 * f(x0 + 2 * h) + 14 * f(x0 + h) - 3 * f(x0)) / Exp(h, 4),
            (f, x0, h) => (f(x0 + 5 * h) - 5 * f(x0 + 4 * h) + 10 * f(x0 + 3 * h) - 10 * f(x0 + 2 * h) + 5 * f(x0 + h) - f(x0)) / Exp(h, 5)
        };

        private static readonly Func<double, double>[] right6p_error =
        {
            (h) => SysMath.Abs(Exp(h, 5)),
            (h) => SysMath.Abs(Exp(h, 4)),
            (h) => SysMath.Abs(Exp(h, 3)),
            (h) => SysMath.Abs(Exp(h, 2)),
            (h) => SysMath.Abs(h),
        };

        #endregion 6 puntos

        #region 7 puntos

        // for i:1 thru 6 do print("(f, x0, h) => ", explicit_fde([-3,-2,-1,0,1,2,3],i));
        private static readonly Func<Func<double, double>, double, double, double>[] central7p =
        {
            (f, x0, h) => (f(x0 + 3 * h) - 9 * f(x0 + 2 * h) + 45 * f(x0 + h) - 45 * f(x0 - h) + 9 * f(x0 - 2 * h) - f(x0 - 3 * h)) / (60 * h),
            (f, x0, h) => (2 * f(x0 + 3 * h) - 27 * f(x0 + 2 * h) + 270 * f(x0 + h) + 270 * f(x0 - h) - 27 * f(x0 - 2 * h) + 2 * f(x0 - 3 * h) - 490 * f(x0)) / (180 * Exp(h, 2)),
            (f, x0, h) => -(f(x0 + 3 * h) - 8 * f(x0 + 2 * h) + 13 * f(x0 + h) - 13 * f(x0 - h) + 8 * f(x0 - 2 * h) - f(x0 - 3 * h)) / (8 * Exp(h, 3)),
            (f, x0, h) => -(f(x0 + 3 * h) - 12 * f(x0 + 2 * h) + 39 * f(x0 + h) + 39 * f(x0 - h) - 12 * f(x0 - 2 * h) + f(x0 - 3 * h) - 56 * f(x0)) / (6 * Exp(h, 4)),
            (f, x0, h) => (f(x0 + 3 * h) - 4 * f(x0 + 2 * h) + 5 * f(x0 + h) - 5 * f(x0 - h) + 4 * f(x0 - 2 * h) - f(x0 - 3 * h)) / (2 * Exp(h, 5)),
            (f, x0, h) => (f(x0 + 3 * h) - 6 * f(x0 + 2 * h) + 15 * f(x0 + h) + 15 * f(x0 - h) - 6 * f(x0 - 2 * h) + f(x0 - 3 * h) - 20 * f(x0)) / Exp(h, 6)
        };

        private static readonly Func<double, double>[] central7p_error =
        {
            (h) => SysMath.Abs(Exp(h, 6)),
            (h) => SysMath.Abs(Exp(h, 6)),
            (h) => SysMath.Abs(Exp(h, 4)),
            (h) => SysMath.Abs(Exp(h, 4)),
            (h) => SysMath.Abs(Exp(h, 2)),
            (h) => SysMath.Abs(Exp(h, 2)),
        };

        // for i:1 thru 6 do print("(f, x0, h) => ", explicit_fde([-6,-5,-4,-3,-2,-1,0],i));
        private static readonly Func<Func<double, double>, double, double, double>[] left7p =
        {
            (f, x0, h) => -(360 * f(x0 - h) - 450 * f(x0 - 2 * h) + 400 * f(x0 - 3 * h) - 225 * f(x0 - 4 * h) + 72 * f(x0 - 5 * h) - 10 * f(x0 - 6 * h) - 147 * f(x0)) / (60 * h),
            (f, x0, h) => -(3132 * f(x0 - h) - 5265 * f(x0 - 2 * h) + 5080 * f(x0 - 3 * h) - 2970 * f(x0 - 4 * h) + 972 * f(x0 - 5 * h) - 137 * f(x0 - 6 * h) - 812 * f(x0)) / (180 * Exp(h, 2)),
            (f, x0, h) => -(232 * f(x0 - h) - 461 * f(x0 - 2 * h) + 496 * f(x0 - 3 * h) - 307 * f(x0 - 4 * h) + 104 * f(x0 - 5 * h) - 15 * f(x0 - 6 * h) - 49 * f(x0)) / (8 * Exp(h, 3)),
            (f, x0, h) => -(186 * f(x0 - h) - 411 * f(x0 - 2 * h) + 484 * f(x0 - 3 * h) - 321 * f(x0 - 4 * h) + 114 * f(x0 - 5 * h) - 17 * f(x0 - 6 * h) - 35 * f(x0)) / (6 * Exp(h, 4)),
            (f, x0, h) => -(40 * f(x0 - h) - 95 * f(x0 - 2 * h) + 120 * f(x0 - 3 * h) - 85 * f(x0 - 4 * h) + 32 * f(x0 - 5 * h) - 5 * f(x0 - 6 * h) - 7 * f(x0)) / (2 * Exp(h, 5)),
            (f, x0, h) => -(6 * f(x0 - h) - 15 * f(x0 - 2 * h) + 20 * f(x0 - 3 * h) - 15 * f(x0 - 4 * h) + 6 * f(x0 - 5 * h) - f(x0 - 6 * h) - f(x0)) / Exp(h, 6),
        };

        private static readonly Func<double, double>[] left7p_error =
        {
            (h) => SysMath.Abs(Exp(h, 6)),
            (h) => SysMath.Abs(Exp(h, 5)),
            (h) => SysMath.Abs(Exp(h, 4)),
            (h) => SysMath.Abs(Exp(h, 3)),
            (h) => SysMath.Abs(Exp(h, 2)),
            (h) => SysMath.Abs(h),
        };

        // for i:1 thru 6 do print("(f, x0, h) => ", explicit_fde([0,1,2,3,4,5,6],i));
        private static readonly Func<Func<double, double>, double, double, double>[] right7p =
        {
            (f, x0, h) => -(10 * f(x0 + 6 * h) - 72 * f(x0 + 5 * h) + 225 * f(x0 + 4 * h) - 400 * f(x0 + 3 * h) + 450 * f(x0 + 2 * h) - 360 * f(x0 + h) + 147 * f(x0)) / (60 * h),
            (f, x0, h) => (137 * f(x0 + 6 * h) - 972 * f(x0 + 5 * h) + 2970 * f(x0 + 4 * h) - 5080 * f(x0 + 3 * h) + 5265 * f(x0 + 2 * h) - 3132 * f(x0 + h) + 812 * f(x0)) / (180 * Exp(h, 2)),
            (f, x0, h) => -(15 * f(x0 + 6 * h) - 104 * f(x0 + 5 * h) + 307 * f(x0 + 4 * h) - 496 * f(x0 + 3 * h) + 461 * f(x0 + 2 * h) - 232 * f(x0 + h) + 49 * f(x0)) / (8 * Exp(h, 3)),
            (f, x0, h) => (17 * f(x0 + 6 * h) - 114 * f(x0 + 5 * h) + 321 * f(x0 + 4 * h) - 484 * f(x0 + 3 * h) + 411 * f(x0 + 2 * h) - 186 * f(x0 + h) + 35 * f(x0)) / (6 * Exp(h, 4)),
            (f, x0, h) => -(5 * f(x0 + 6 * h) - 32 * f(x0 + 5 * h) + 85 * f(x0 + 4 * h) - 120 * f(x0 + 3 * h) + 95 * f(x0 + 2 * h) - 40 * f(x0 + h) + 7 * f(x0)) / (2 * Exp(h, 5)),
            (f, x0, h) => (f(x0 + 6 * h) - 6 * f(x0 + 5 * h) + 15 * f(x0 + 4 * h) - 20 * f(x0 + 3 * h) + 15 * f(x0 + 2 * h) - 6 * f(x0 + h) + f(x0)) / Exp(h, 6),
        };

        private static readonly Func<double, double>[] right7p_error =
        {
            (h) => SysMath.Abs(Exp(h, 6)),
            (h) => SysMath.Abs(Exp(h, 5)),
            (h) => SysMath.Abs(Exp(h, 4)),
            (h) => SysMath.Abs(Exp(h, 3)),
            (h) => SysMath.Abs(Exp(h, 2)),
            (h) => SysMath.Abs(h),
        };

        #endregion 7 puntos

        private static double Exp(double h, int exp)
        {
            switch (exp)
            {
                case 0:
                    return 1;
                case 1:
                    return h;
                case 2:
                    return h * h;
                case 3:
                    return h * h * h;
                case 4:
                {
                    double hh = h * h;
                    return hh * hh;
                }
                case 5:
                {
                    double hh = h * h;
                    return hh * hh * h;
                }
                case 6:
                {
                    double hh = h * h;
                    return hh * hh * hh;
                }
                default:
                {
                    double ret = h;
                    for (int i = 1; i < exp; i++)
                    {
                        ret *= h;
                    }
                    return ret;
                }
            }
        }
    }
}

#if false
        public void Test()
        {
            /*Func<double, double> f = x => x * x * x;
            Func<double, double> df1 = x => 3 * x * x;
            Func<double, double> df2 = x => 6 * x;
            Func<double, double> df3 = x => 6;
            Func<double, double> df4 = x => 0;

            Func<double, double> _df1 = Derivative.Central(f, 1, 5);
            Func<double, double> _df2 = Derivative.Central(f, 2, 5);
            Func<double, double> _df3 = Derivative.Central(f, 3, 5);
            Func<double, double> _df4 = Derivative.Central(f, 4, 5);

            double a = df1(0.5);
            double b = _df1(0.5);

            a = df1(-0.5);
            b = _df1(-0.5);

            a = df2(0.5);
            b = _df2(0.5);

            a = df2(-0.5);
            b = _df2(-0.5);*/

            Bezier2D bez = new Bezier2D(new Punto2d(0, 0), new Punto2d(0, 10), new Punto2d(10, -10), new Punto2d(10, 0));

            // t en [0, 1]
            Func<double, double> f = t => bez.GetPosicion(t).Y;

            // Cache con las derivadas calculadas.
            List<Bezier2D> cache = new List<Bezier2D>();
            cache.Add(bez); // Bezier.

            // Funcion derivada de orden i de la derivada de la bezier.
            Deriva ddbezier = (i) =>
            {
                // Se guardan en cache las derivadas que aún no estén calculadas.
                for (int j = cache.Count - 1; j < i; j++)
                {
                    cache.Add(cache[j].Derivada());
                }

                return (tt) => cache[i].GetPosicion(tt).Y;
            };

            Func<double, double> df1 = t => ddbezier(1)(t);
            Func<double, double> df2 = t => ddbezier(2)(t);
            Func<double, double> df3 = t => ddbezier(3)(t);
            Func<double, double> df4 = t => ddbezier(4)(t);

            /*Bezier2D d1 = bez.Derivada();
            Bezier2D d2 = d1.Derivada();
            Bezier2D d3 = d2.Derivada();
            Bezier2D d4 = d3.Derivada();

            Func<double, double> df1 = t => d1.GetPosicion(t).Y;
            Func<double, double> df2 = t => d2.GetPosicion(t).Y;
            Func<double, double> df3 = t => d3.GetPosicion(t).Y;
            Func<double, double> df4 = t => d4.GetPosicion(t).Y;*/

            /*Func<double, double> df1 = t => bez.GetDerivada1(t).Y;
            Func<double, double> df2 = t => bez.GetDerivada2(t).Y;
            Func<double, double> df3 = t => bez.GetDerivada3(t).Y;
            Func<double, double> df4 = t => 0;*/

            Func<double, double> _df1 = Derivative.Central(f, 1, 5);
            Func<double, double> _df2 = Derivative.Central(f, 2, 5);
            Func<double, double> _df3 = Derivative.Central(f, 3, 5);
            Func<double, double> _df4 = Derivative.Central(f, 4, 5);

            for (double t = 0; t <= 1; t += 0.1)
            {
                Console.WriteLine();
                Console.WriteLine("{0:F3} error: {1:F3} real: {2:F3} calculado: {3:F3}", t, SysMath.Abs(_df1(t) - df1(t)), df1(t), _df1(t));
                Console.WriteLine("{0:F3} error: {1:F3} real: {2:F3} calculado: {3:F3}", t, SysMath.Abs(_df2(t) - df2(t)), df2(t), _df2(t));
                Console.WriteLine("{0:F3} error: {1:F3} real: {2:F3} calculado: {3:F3}", t, SysMath.Abs(_df3(t) - df3(t)), df3(t), _df3(t));
                Console.WriteLine("{0:F3} error: {1:F3} real: {2:F3} calculado: {3:F3}", t, SysMath.Abs(_df4(t) - df4(t)), df4(t), _df4(t));
            }
        }
#endif