using Framework.Core.Configuration;
using Framework.Core.DAL.Infrastructure;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Core.Service
{
    internal class ServiceResolver
    {
        private static Type genericServiceType = null;
        private static ConcurrentDictionary<string, Type> modelsTypeDictionary = null;
        private static IEnumerable<Type> modelsTypeInTheNamespace = null;
        private static string suffix = null;

        internal ServiceResolver(string modelSuffix, IEnumerable<Type> modelsInTheNamespace, Type genericService)
        {
            suffix = modelSuffix;
            modelsTypeInTheNamespace = modelsInTheNamespace;
            genericServiceType = genericService;

            modelsTypeDictionary = new ConcurrentDictionary<string, Type>();
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
            var modelType = modelsTypeInTheNamespace.SingleOrDefault(mod => mod.Name == modelName + suffix);

            if (modelType == null)
                throw new Exception($"The model type {modelName} is not found into the configure namespace. See your configuration in the startup class !");

            return modelsTypeDictionary.GetOrAdd(modelName, modelType);
        }
    }
}
