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