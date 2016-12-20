using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AllowCRUDAttribute : Attribute
    {
        public readonly List<string> CrudOperationsAllowed;

        /// <param name="crudOperationsAllowed">Can be CRUD, DURC, CU, DC, crud, etc</param>
        public AllowCRUDAttribute(string crudOperationsAllowed)
        {
            CrudOperationsAllowed = new List<string>();
            foreach (var crudOperation in crudOperationsAllowed.ToArray())
            {
                var operation = char.ToLowerInvariant(crudOperation);

                if (operation == char.ToLowerInvariant('C')
                    || operation == char.ToLowerInvariant('R')
                    || operation == char.ToLowerInvariant('U')
                    || operation == char.ToLowerInvariant('D'))
                {
                    CrudOperationsAllowed.Add(crudOperation.ToString());
                }
            }
        }
    }
}
