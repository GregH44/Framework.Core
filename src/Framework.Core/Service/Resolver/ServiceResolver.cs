using Framework.Core.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Framework.Core.Service
{
    public class ServiceResolver
    {
        private static Type genericServiceType = null;
        private static ConcurrentDictionary<string, Type> modelsTypeDictionary = null;
        private static IEnumerable<Type> modelsTypeInTheNamespace = null;
        private static string suffix = null;

        //IList<>
        private string assemblyModels = null;
        private string namespaceModels = null;

        public ServiceResolver(string assemblyModels, string namespaceModels)
            : this(assemblyModels, namespaceModels, string.Empty)
        {
        }

        public ServiceResolver(string assemblyModels, string namespaceModels, string modelSuffix)
        {
            // Assembly Sample.DotNetFramework.Common
            this.assemblyModels = assemblyModels;
            this.namespaceModels = namespaceModels;
            suffix = modelSuffix;
            modelsTypeDictionary = new ConcurrentDictionary<string, Type>();
        }

        public void LoadModels()
        {
            var assembly = Assembly.Load(new AssemblyName(assemblyModels));

            if (assembly == null)
                throw new Exception($"Assembly {assemblyModels} not found !");

            modelsTypeInTheNamespace = assembly.ExportedTypes.Where(mod => mod.Namespace.StartsWith(namespaceModels)).OrderBy(o => o.Name);
            genericServiceType = Assembly.Load(new AssemblyName("Framework.Core")).ExportedTypes.Single(type => type.Name.StartsWith("GenericService"));
        }

        internal static object GetService(string modelName)
        {
            var modelType = GetTypeFromModelName(modelName);
            var genericService = genericServiceType.MakeGenericType(modelType);

            return ServicesProvider.Services.GetService(genericService);
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
