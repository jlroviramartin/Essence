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

#region Alias

using REAL = System.Double;

#endregion Alias

namespace Essence.Util.Math.Double
{
    public static class RangeUtils
    {
        public static bool IsValid(REAL aMin, REAL aMax)
        {
            return !(REAL.IsNaN(aMin) || REAL.IsNaN(aMax));
        }

        public static bool IsEmpty(REAL aMin, REAL aMax, REAL epsilon = MathUtils.EPSILON)
        {
            return aMax.EpsilonL(aMin, epsilon);
        }

        public static bool IsZero(REAL aMin, REAL aMax, REAL epsilon = MathUtils.EPSILON)
        {
            return aMax.EpsilonEquals(aMin, epsilon);
        }

        public static bool IsInfinity(REAL aMin, REAL aMax)
        {
            return REAL.IsInfinity(aMin) || REAL.IsInfinity(aMax);
        }

        public static void Union(REAL aMin, REAL aMax, REAL y, out REAL zMin, out REAL zMax)
        {
            // Si [aMin, aMax] es vacio -> [zMin, zMax] = [y, y]
            if (IsEmpty(aMin, aMax))
            {
                zMin = y;
                zMax = y;
                return;
            }

            zMin = System.Math.Min(aMin, y);
            zMax = System.Math.Max(aMax, y);
        }

        public static void Union(REAL aMin, REAL aMax, REAL bMin, REAL bMax, out REAL zMin, out REAL zMax)
        {
            // Si [aMin, aMax] es vacio -> [zMin, zMax] = [bMin, bMax]
            if (IsEmpty(aMin, aMax))
            {
                zMin = bMin;
                zMax = bMax;
                return;
            }

            // Si [bMin, bMax] es vacio -> [zMin, zMax] = [aMin, aMax]
            if (IsEmpty(bMin, bMax))
            {
                // Vacio.
                zMin = aMin;
                zMax = aMax;
                return;
            }

            zMin = System.Math.Min(aMin, bMin);
            zMax = System.Math.Max(aMax, bMax);
        }

        public static void Intersect(REAL aMin, REAL aMax, REAL bMin, REAL bMax, out REAL zMin, out REAL zMax)
        {
            // Si [aMin, aMax] es vacio -> [zMin, zMax] = vacio ( [aMin, aMax] )
            if (IsEmpty(aMin, aMax))
            {
                zMin = aMin;
                zMax = aMax;
                return;
            }

            // Si [bMin, bMax] es vacio -> [zMin, zMax] = vacio ( [bMin, bMax] )
            if (IsEmpty(bMin, bMax))
            {
                zMin = bMin;
                zMax = bMax;
                return;
            }

            zMin = System.Math.Max(aMin, bMin);
            zMax = System.Math.Min(aMax, bMax);
        }

        public static bool IntersectsWith(REAL aMin, REAL aMax, REAL bMin, REAL bMax, REAL epsilon = MathUtils.EPSILON)
        {
            // Si [aMin, aMax] es vacio -> no hay interseccion.
            if (IsEmpty(aMin, aMax))
            {
                return false;
            }

            // Si [bMin, bMax] es vacio -> no hay interseccion.
            if (IsEmpty(bMin, bMax))
            {
                return false;
            }

            return System.Math.Max(aMin, bMin).EpsilonLE(System.Math.Min(aMax, bMax), epsilon);
        }

        public static bool ContainsPoint(REAL aMin, REAL aMax, REAL y, REAL epsilon = MathUtils.EPSILON)
        {
            // Si [aMin, aMax] es vacio -> no lo contiene.
            if (IsEmpty(aMin, aMax))
            {
                return false;
            }

            return y.EpsilonBetweenClosed(aMin, aMax, epsilon);
        }

        public static bool Contains(REAL aMin, REAL aMax, REAL bMin, REAL bMax, REAL epsilon = MathUtils.EPSILON)
        {
            // Si [aMin, aMax] es vacio -> no lo contiene.
            if (IsEmpty(aMin, aMax))
            {
                return false;
            }

            // Si [bMin, bMax] es vacio -> no lo contiene.
            if (IsEmpty(bMin, bMax))
            {
                return false;
            }

            return aMin.EpsilonLE(bMin, epsilon) && bMax.EpsilonLE(aMax, epsilon);
        }

        public static bool TouchPoint(REAL aMin, REAL aMax, REAL y, REAL epsilon = MathUtils.EPSILON)
        {
            // Si [aMin, aMax] es vacio -> no lo toca.
            if (IsEmpty(aMin, aMax))
            {
                return false;
            }

            return aMin.EpsilonEquals(y, epsilon) || aMin.EpsilonEquals(y, epsilon);
        }

        public static bool Touch(REAL aMin, REAL aMax, REAL bMin, REAL bMax, REAL epsilon = MathUtils.EPSILON)
        {
            // Si [aMin, aMax] es vacio -> no lo toca.
            if (IsEmpty(aMin, aMax))
            {
                return false;
            }

            // Si [bMin, bMax] es vacio -> no lo toca.
            if (IsEmpty(bMin, bMax))
            {
                return false;
            }

            return aMin.EpsilonEquals(bMin, epsilon) || aMin.EpsilonEquals(bMax, epsilon) || aMax.EpsilonEquals(bMin, epsilon) || aMax.EpsilonEquals(bMax, epsilon);
        }

        public static bool Equals(REAL aMin, REAL aMax, REAL bMin, REAL bMax)
        {
            if (IsEmpty(aMin, aMax))
            {
                return IsEmpty(bMin, bMax);
            }
            else if (IsEmpty(bMin, bMax))
            {
                return false;
            }

            return (aMin == bMin) || (aMax == bMax);
        }

        public static bool EpsilonEquals(REAL aMin, REAL aMax, REAL bMin, REAL bMax, REAL epsilon = MathUtils.EPSILON)
        {
            if (IsEmpty(aMin, aMax))
            {
                return IsEmpty(bMin, bMax);
            }
            else if (IsEmpty(bMin, bMax))
            {
                return false;
            }

            return aMin.EpsilonEquals(bMin, epsilon) || aMax.EpsilonEquals(bMax, epsilon);
        }
    }
}