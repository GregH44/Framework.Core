using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Core.Resolvers
{
    internal class ServiceResolver
    {
        private static Type genericServiceType = null;
        private static SortedDictionary<string, Type> modelsTypeInTheNamespace = null;
        private static string suffix = null;

        internal ServiceResolver(
            string modelSuffix,
            SortedDictionary<string, Type> modelsInTheNamespace,
            Type genericService)
        {
            suffix = modelSuffix;
            modelsTypeInTheNamespace = modelsInTheNamespace;
            genericServiceType = genericService;
        }

        internal static object GetService(DbContext dbContext, string modelName)
        {
            var modelType = GetTypeFromModelName(modelName);
            var genericService = genericServiceType.MakeGenericType(modelType);

            return Activator.CreateInstance(genericService, dbContext);
        }

        internal static Type GetModelType(string modelName)
        {
            return GetTypeFromModelName(modelName);
        }

        private static Type GetTypeFromModelName(string modelName)
        {
            var name = modelName.ToLower() + suffix.ToLower();
            KeyValuePair<string, Type> kvpModelType = modelsTypeInTheNamespace.FirstOrDefault(model => model.Key.ToLower() == name);

            if (kvpModelType.Value == null)
                throw new Exception($"The model type {modelName} is not found into the configure namespace. See your configuration in the startup class !");

            return kvpModelType.Value;
        }
    }
}
