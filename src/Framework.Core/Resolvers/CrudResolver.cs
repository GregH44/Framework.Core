using System.Collections;
using System.Collections.Generic;

namespace Framework.Core.Resolvers
{
    internal class CrudResolver
    {
        internal static SortedDictionary<string, BitArray> crudOperationsModelTypes = new SortedDictionary<string, BitArray>();

        internal static BitArray GetCrudOperationsFromModelType(string modelName)
        {
            BitArray crudOperationsAllowed;
            crudOperationsModelTypes.TryGetValue(modelName, out crudOperationsAllowed);
            return crudOperationsAllowed;
        }
    }
}
