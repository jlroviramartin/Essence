using System.Collections.Generic;

namespace Essence.View.Models
{
    public static class ActionUtils
    {
        public static IEnumerable<IAction> Sort(IEnumerable<IAction> actions)
        {
            return actions;
        }

        public static void NotifyUpdateState(IAction accion)
        {
            if (accion is Action)
            {
                ((Action)accion).NotifyUpdateState();
            }
        }
    }
}