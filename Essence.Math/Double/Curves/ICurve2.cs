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

namespace Essence.Maths.Double.Curves
{
    public interface ICurve2
    {
        bool IsClosed { get; }

        double TMin { get; }

        double TMax { get; }

        void SetTInterval(double tmin, double tmax);

        #region Position and derivatives

        Vec2d GetPosition(double t);

        Vec2d GetFirstDerivative(double t);

        Vec2d GetSecondDerivative(double t);

        Vec2d GetThirdDerivative(double t);

        #endregion

        #region Differential geometric quantities

        double GetLength(double t0, double t1);

        double TotalLength { get; }

        double GetSpeed(double t);

        double GetCurvature(double t);

        Vec2d GetTangent(double t);

        Vec2d GetLeftNormal(double t);

        void GetFrame(double t, ref Vec2d position, ref Vec2d tangent, ref Vec2d leftNormal);

        #endregion
    }
}