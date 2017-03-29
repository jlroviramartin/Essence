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

using Essence.Util.Collections;

namespace Essence.View.Models
{
    public class ComposedAction : Action, IComposedAction
    {
        public ComposedAction()
        {
        }

        #region private

        private readonly EventList<IAction> actions = new EventList<IAction>();

        #endregion

        #region IComposedAction

        public IEventList<IAction> Actions
        {
            get { return this.actions; }
        }

        #endregion
    }
}