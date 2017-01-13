using Framework.Core.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Core.Extensions
{
    public static class MVVMExtensions
    {
        public static TDest ToModel<TDest>(this object model)
        {
            return Mapper.Instance.Map<TDest>(model);
        }

        public static IEnumerable<TDest> ToModels<TDest>(this IEnumerable<object> dataModels)
        {
            if (dataModels == null)
                return null;

            return dataModels.Select(Mapper.Instance.Map<TDest>);
        }
    }
}
