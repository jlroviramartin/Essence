using Essence.Util.Collections;

namespace Essence.View.Models
{
    public class ComposedComponentUI : AbsComponentUI, IComposedComponentUI
    {
        public ComposedComponentUI()
        {
        }

        #region private

        private readonly EventList<IComponentUI> components = new EventList<IComponentUI>();

        #endregion

        #region IComposedComponentUI

        public IEventList<IComponentUI> Components
        {
            get { return this.components; }
        }

        #endregion
    }
}