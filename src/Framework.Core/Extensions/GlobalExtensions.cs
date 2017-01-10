using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Framework.Core.Extensions
{
    internal static class GlobalExtensions
    {
        internal static string DeleteSuffix(this string modelName, string modelSuffix)
        {
            if (!string.IsNullOrEmpty(modelSuffix) && modelName.EndsWith(modelSuffix))
            {
                var indexSuffix = modelName.IndexOf(modelSuffix);
                modelName = modelName.Remove(indexSuffix);
            }

            return modelName;
        }

        internal static void AddServicesAsScoped(
            this IServiceCollection services,
            MethodInfo addScopedMethod,
            string modelName,
            IEnumerable<Type> serviceInterfaces,
            IEnumerable<Type> serviceClasses)
        {
            var serviceInterface = serviceInterfaces.SingleOrDefault(svc => svc.Name == $"I{modelName}Service");
            var serviceClass = serviceClasses.SingleOrDefault(svc => svc.Name == $"{modelName}Service");

            if (serviceInterface != null && serviceClass != null)
            {
                var addScopedGenericMethod = addScopedMethod.MakeGenericMethod(serviceInterface, serviceClass);
                var delegateToCall = addScopedGenericMethod.CreateDelegate(Expression.GetDelegateType((from parameter in addScopedGenericMethod.GetParameters() select parameter.ParameterType)
                                .Concat(new[] { addScopedGenericMethod.ReturnType })
                                .ToArray()));
                delegateToCall.DynamicInvoke(services);
            }
        }
    }
}
