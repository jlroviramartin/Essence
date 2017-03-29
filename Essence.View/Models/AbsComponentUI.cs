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

using Essence.Util.Events;
using Essence.Util.Properties;

namespace Essence.View.Models
{
    public abstract class AbsComponentUI : AbsNotifyPropertyChanged, IComponentUI
    {
        public const string NAME = "Name";
        public const string NAME_UI = "NameUI";
        public const string DESCRIPTION_UI = "DescriptionUI";
        public const string ICON_UI = "IconUI";

        #region private

        private string name;

        private string nameUI;
        private string descriptionUI;
        private Icon iconUI;

        #endregion

        #region IComponentUI

        public string Name
        {
            get { return this.name; }
            set { this.Set(NAME, this.name, value, x => this.name = x); }
        }

        public string NameUI
        {
            get { return this.nameUI; }
            set { this.Set(NAME_UI, this.nameUI, value, x => this.nameUI = x); }
        }

        public string DescriptionUI
        {
            get { return this.descriptionUI; }
            set { this.Set(DESCRIPTION_UI, this.descriptionUI, value, x => this.descriptionUI = x); }
        }

        public Icon IconUI
        {
            get { return this.iconUI; }
            set { this.Set(ICON_UI, this.iconUI, value, x => this.iconUI = x); }
        }

        #endregion
    }
}