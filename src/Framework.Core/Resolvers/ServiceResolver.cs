using Framework.Core.DAL.Infrastructure;
using System;
using System.Collections.Generic;

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

        internal static object GetService(DataBaseContext dbContext, string modelName)
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
            Type modelType = null;

            if (!modelsTypeInTheNamespace.TryGetValue(modelName + suffix, out modelType))
                throw new Exception($"The model type {modelName} is not found into the configure namespace. See your configuration in the startup class !");

            return modelType;
        }
    }
}
