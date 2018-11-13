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
using org.apache.commons.math3.analysis.integration;

namespace Essence.Maths
{
    public class Integrator
    {
        public enum Type
        {
            RombergIntegrator
        }

        public static double Integrate(Func<double, double> f, double t0, double t1, Type type, int maxEval)
        {
            UnivariateIntegrator iintegral;
            switch (type)
            {
                case Type.RombergIntegrator:
                {
                    iintegral = new RombergIntegrator();
                    break;
                }
                default:
                {
                    throw new IndexOutOfRangeException();
                }
            }
            double v = iintegral.Integrate(maxEval, new DelegateUnivariateFunction(f), t0, t1);
            return v;
        }
    }
}