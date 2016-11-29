using Sample.DotNetFramework.MVC6.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace Sample.DotNetFramework.MVC6.Extensions
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
