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
using Essence.Util.Math.Double;

namespace Essence.Util.Math
{
    public abstract class EpsilonEqualityComparer<T> : IEpsilonEqualityComparer<T>
    {
        public static IEpsilonEqualityComparer<T> Default;

        static EpsilonEqualityComparer()
        {
            if (typeof(IEpsilonEquatable<T>).IsAssignableFrom(typeof(T)))
            {
                Default = Activator.CreateInstance(typeof(EpsilonEquatableComparer<>).MakeGenericType(typeof(T))) as IEpsilonEqualityComparer<T>;
            }
            else if (typeof(float).IsAssignableFrom(typeof(T)))
            {
                Default = new SingleComparer() as IEpsilonEqualityComparer<T>;
            }
            else if (typeof(double).IsAssignableFrom(typeof(T)))
            {
                Default = new DoubleComparer() as IEpsilonEqualityComparer<T>;
            }
            else
            {
                Default = new NonEpsilonEquatableComparer<T>();
            }
        }

        #region IEpsilonEqualityComparer<T>

        public abstract bool EpsilonEquals(T first, T second, double epsilon = MathUtils.EPSILON);

        #endregion

        #region private

        private struct EpsilonEquatableComparer<T2> : IEpsilonEqualityComparer<T2>
            where T2 : IEpsilonEquatable<T2>
        {
            public bool EpsilonEquals(T2 first, T2 second, double epsilon = MathUtils.EPSILON)
            {
                return first.EpsilonEquals(second, epsilon);
            }
        }

        private struct NonEpsilonEquatableComparer<T2> : IEpsilonEqualityComparer<T2>
        {
            public bool EpsilonEquals(T2 first, T2 second, double epsilon = MathUtils.EPSILON)
            {
                return EqualityComparer<T2>.Default.Equals(second);
            }
        }

        private struct SingleComparer : IEpsilonEqualityComparer<float>
        {
            public bool EpsilonEquals(float first, float second, double epsilon = MathUtils.EPSILON)
            {
                return Float.MathUtils.EpsilonEquals(second, (float)epsilon);
            }
        }

        private struct DoubleComparer : IEpsilonEqualityComparer<double>
        {
            public bool EpsilonEquals(double first, double second, double epsilon = MathUtils.EPSILON)
            {
                return Double.MathUtils.EpsilonEquals(second, epsilon);
            }
        }

        #endregion
    }
}