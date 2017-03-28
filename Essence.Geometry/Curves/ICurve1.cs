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
using REAL = System.Double;

namespace Essence.Maths.Double.Curves
{
    public interface ICurve1
    {
        REAL TMin { get; }

        REAL TMax { get; }

        void SetTInterval(REAL tmin, REAL tmax);

        #region Position and derivatives

        REAL GetPosition(REAL t);

        REAL GetFirstDerivative(REAL t);

        REAL GetSecondDerivative(REAL t);

        REAL GetThirdDerivative(REAL t);

        #endregion

        #region Differential geometric quantities

        REAL TotalLength { get; }

        REAL GetLength(REAL t0, REAL t1);

        REAL GetSpeed(REAL t);

        #endregion
    }
}