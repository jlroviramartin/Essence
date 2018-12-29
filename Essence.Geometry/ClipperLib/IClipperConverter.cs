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

namespace ClipperLib
{
    public interface IClipperConverter
    {
        void ZFill(IntPoint bot1, IntPoint top1,
                   IntPoint bot2, IntPoint top2,
                   ref IntPoint pt);

        bool UseZFill { get; }
    }
}