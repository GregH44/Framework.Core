using Framework.Core.Constants;
using System;
using System.Collections;

namespace Framework.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CrudOperationsAttribute : Attribute
    {
        public readonly BitArray crudOperationsAllowed;

        /// <param name="crudOperationsAllowed"></param>
        public CrudOperationsAttribute(bool Create = true, bool Receive = true, bool Update = true, bool Delete = true)
        {
            crudOperationsAllowed = new BitArray(4);
            crudOperationsAllowed.Set(GlobalConstants.CrudOperations.Create, Create);
            crudOperationsAllowed.Set(GlobalConstants.CrudOperations.Receive, Receive);
            crudOperationsAllowed.Set(GlobalConstants.CrudOperations.Update, Update);
            crudOperationsAllowed.Set(GlobalConstants.CrudOperations.Delete, Delete);
        }
    }
}
