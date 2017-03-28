using System;
using Essence.View.Models;
using Essence.View.Models.Properties;

namespace Essence.View.Attributes
{
    public class RefListAttribute : ViewAttribute
    {
        public RefListAttribute(Type refListType)
        {
            this.RefList = (IRefList)Activator.CreateInstance(refListType);
        }

        public IRefList RefList { get; set; }

        #region ViewAttribute

        public override ViewProperties GetViewProperties()
        {
            return new RefListProperties()
            {
                RefList = this.RefList
            };
        }

        #endregion
    }
}