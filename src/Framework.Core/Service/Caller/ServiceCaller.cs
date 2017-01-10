using Framework.Core.DAL.Infrastructure;
using Framework.Core.Enums;
using Framework.Core.Resolvers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Framework.Core.Service
{
    internal static class ServiceCaller
    {
        private static ConcurrentDictionary<string, MethodInfo> methodsInfo = new ConcurrentDictionary<string, MethodInfo>();

        internal static object Call(DataBaseContext dbContext, GlobalEnums.Api methodName, string modelName)
            => Call(dbContext, methodName, modelName, null, null);

        internal static object Call(DataBaseContext dbContext, GlobalEnums.Api methodName, string modelName, long? id)
            => Call(dbContext, methodName, modelName, id, null);

        internal static object Call(DataBaseContext dbContext, GlobalEnums.Api methodName, string modelName, object values)
            => Call(dbContext, methodName, modelName, null, values);

        private static object Call(DataBaseContext dbContext, GlobalEnums.Api methodName, string modelName, long? id, object values)
        {
            var service = ServiceResolver.GetService(dbContext, modelName);
            object result = null;

            switch (methodName)
            {
                case GlobalEnums.Api.Add:
                    if (values == null) throw new ArgumentNullException($"'{nameof(values)}' cannot be null for {GlobalEnums.Api.Add.ToString()} operation.");
                    result = InvokeMethod(service, GlobalEnums.Api.Add, new object[] { values });
                    break;
                case GlobalEnums.Api.Delete:
                    if (!id.HasValue) throw new ArgumentNullException($"'{nameof(id)}' cannot be null for {GlobalEnums.Api.Delete.ToString()} operation.");
                    result = InvokeMethod(service, GlobalEnums.Api.Delete, new object[] { id.Value });
                    break;
                case GlobalEnums.Api.Get:
                    if (id.HasValue)
                        result = InvokeMethod(service, GlobalEnums.Api.Get, new object[] { id.Value });
                    else
                        result = InvokeMethod(service, GlobalEnums.Api.GetList);
                    break;
                case GlobalEnums.Api.Update:
                    if (values == null) throw new ArgumentNullException($"'{nameof(values)}' cannot be null for {GlobalEnums.Api.Update.ToString()} operation.");
                    result = InvokeMethod(service, GlobalEnums.Api.Update, new object[] { values });
                    break;
                default:
                    throw new TargetException($"The method name {methodName.ToString()} is not found !");
            }

            return result;
        }

        private static object InvokeMethod(object service, GlobalEnums.Api methodName, object[] parameters = null)
        {
            MethodInfo methodToCall = GetMethod(service, methodName, parameters);
            object result = null;

            if (parameters == null)
            {
                parameters = methodToCall.GetParameters().Select(param => new List<object> { param.DefaultValue }).ToArray();
            }

            var delegateToCall = methodToCall.CreateDelegate(Expression.GetDelegateType((from parameter in methodToCall.GetParameters() select parameter.ParameterType)
                                    .Concat(new[] { methodToCall.ReturnType })
                                    .ToArray()), service);

            var serviceResponse = delegateToCall.DynamicInvoke(parameters);

            if (serviceResponse is Task)
            {
                var propertyResult = methodToCall.ReturnType.GetProperty("Result");
                result = propertyResult.GetValue(serviceResponse);
            }
            else
            {
                result = serviceResponse;
            }

            return result;
        }

        private static MethodInfo GetMethod(object service, GlobalEnums.Api methodName, object[] parameters = null)
        {
            MethodInfo method = null;

            if (!methodsInfo.TryGetValue(methodName.ToString(), out method))
            {
                if (parameters != null)
                {
                    method = service.GetType().GetMethod(methodName.ToString(), parameters.Select(param => param.GetType()).ToArray());
                }
                else
                {
                    method = service.GetType().GetMethod(methodName.ToString());
                }

                methodsInfo.TryAdd(methodName.ToString(), method);
            }

            return method;
        }
    }
}
