using Framework.Core.Constants;
using Framework.Core.Enums;
using System;
using System.Collections;
using System.Linq;

namespace Framework.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CrudOperationsAttribute : Attribute
    {
        public readonly BitArray crudOperationsAllowed;

        /// <param name="crudOperationsAllowed">
        /// <see cref="GlobalConstants.CrudOperations"/>
        /// </param>
        public CrudOperationsAttribute(bool Create = true, bool Receive = true, bool Update = true, bool Delete = true)
        {
            crudOperationsAllowed = new BitArray(4);
            crudOperationsAllowed.Set(0, Create);
            crudOperationsAllowed.Set(0, Receive);
            crudOperationsAllowed.Set(0, Update);
            crudOperationsAllowed.Set(0, Delete);
        }
    }
}
