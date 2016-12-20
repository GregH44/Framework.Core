using System;

namespace Framework.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CrudOperationAttribute : Attribute
    {
        public readonly string crudOperationsAllowed = string.Empty;

        /// <param name="crudOperationsAllowed">Can be CRUD, DURC, CU, DC, crud, etc</param>
        public CrudOperationAttribute(string crudOperationsAllowed)
        {
            this.crudOperationsAllowed = crudOperationsAllowed;
        }
    }
}
