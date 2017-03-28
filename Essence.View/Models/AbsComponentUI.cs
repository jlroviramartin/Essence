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