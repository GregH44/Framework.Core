using Framework.Core.Attributes;
using Framework.Core.Configuration;
using Framework.Core.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Framework.Core
{
    public static class FrameworkManager
    {
        private static IConfigurationRoot configuration = ConfigurationManager.Configuration;
        private static Type genericServiceType = null;
        private static IEnumerable<Type> modelTypesInTheNamespace = null;

        public static void InitializeGenericApi(IServiceCollection services)
        {
            genericServiceType = Assembly.Load(new AssemblyName("Framework.Core")).ExportedTypes.Single(type => type.Name.StartsWith("GenericService"));
            LoadModelTypes();

            InitializeServiceResolver();
            DeclareApiServicesAsScoped(services);

            ServicesProvider.Services = services.BuildServiceProvider();
        }

        private static void DeclareApiServicesAsScoped(IServiceCollection services)
        {
            var addScopedGenericMethod = 
                typeof(ServiceCollectionServiceExtensions)
                .GetMethods()
                .Where(method => method.Name == "AddScoped" && method.GetGenericArguments().Length == 2 && method.GetParameters().Count() == 1)
                .Single();

            if (addScopedGenericMethod == null)
                throw new Exception("The AddScoped method doesn't exists !");

            foreach (var modelType in modelTypesInTheNamespace)
            {
                var genericServiceForDataModel = genericServiceType.MakeGenericType(modelType);
                var addScopedMethod = addScopedGenericMethod.MakeGenericMethod(genericServiceForDataModel, genericServiceForDataModel);
                addScopedMethod.Invoke(services, new object[] { services });
            }
        }

        private static void InitializeServiceResolver()
        {
            new ServiceResolver(
                configuration["Framework:Configuration:Models:ModelSuffix"],
                modelTypesInTheNamespace,
                genericServiceType
                );
        }

        private static void LoadModelTypes()
        {
            var assemblyModels = configuration["Framework:Configuration:Models:Assembly"];
            var namespaceModels = configuration["Framework:Configuration:Models:NamespaceModels"];

            if (string.IsNullOrEmpty(assemblyModels))
                throw new Exception($"Assembly is not configure for models. Please check initialization from \"FrameworkManager\" in your startup class.");

            var assembly = Assembly.Load(new AssemblyName(assemblyModels));

            if (assembly == null)
                throw new Exception($"Assembly {assemblyModels} not found !");

            modelTypesInTheNamespace = assembly.ExportedTypes.Where(mod => mod.Namespace.StartsWith(namespaceModels)).OrderBy(o => o.Name);
        }
    }
}
