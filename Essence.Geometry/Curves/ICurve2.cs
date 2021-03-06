﻿// Copyright 2017 Jose Luis Rovira Martin
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

using Essence.Geometry.Core.Double;

namespace Essence.Geometry.Curves
{
    public interface ICurve2
    {
        bool IsClosed { get; }

        double TMin { get; }

        double TMax { get; }

        void SetTInterval(double tmin, double tmax);

        double GetT(double length, int iterations = 32, double tolerance = 1e-06);

        #region Position and derivatives

        Point2d GetPosition(double t);

        Vector2d GetFirstDerivative(double t);

        Vector2d GetSecondDerivative(double t);

        Vector2d GetThirdDerivative(double t);

        #endregion

        #region Differential geometric quantities

        double TotalLength { get; }

        double GetLength(double t0, double t1);

        double GetSpeed(double t);

        double GetCurvature(double t);

        Vector2d GetTangent(double t);

        Vector2d GetLeftNormal(double t);

        void GetFrame(double t, out Point2d position, out Vector2d tangent, out Vector2d leftNormal);

        #endregion

        BoundingBox2d BoundingBox { get; }
    }
}