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

        public static void InitializeFramework(this IServiceCollection services)
        {
            InitializeFilters(services);
            InitializeServices(services);
        }

        private static void InitializeFilters(IServiceCollection services)
        {
            // Exception handler service
            services.AddScoped<ExceptionHandlerAttribute>();
            // CorrelationId manager service
            services.AddScoped<CorrelationIdAttribute>();
        }
        
        private static void InitializeServices(IServiceCollection services)
        {
            genericServiceType = Assembly.Load(new AssemblyName("Framework.Core")).ExportedTypes.Single(type => type.Name.StartsWith("GenericService"));
            LoadModelTypes();

            InitializeServiceResolver();

            var addScopedGenericMethod =
                typeof(ServiceCollectionServiceExtensions)
                .GetMethods()
                .Where(method => method.Name == "AddScoped" && method.GetGenericArguments().Length == 2 && method.GetParameters().Count() == 1)
                .Single();

            DeclareApiServicesAsScoped(services, addScopedGenericMethod);
            DeclareServicesAsScoped(services, addScopedGenericMethod);
        }

        private static void DeclareApiServicesAsScoped(IServiceCollection services, MethodInfo addScopedGenericMethod)
        {
            var useGenericApi = false;

            if (!bool.TryParse(configuration["Framework:Configuration:UseGenericApi"], out useGenericApi) && !useGenericApi)
                return;

            foreach (var modelType in modelTypesInTheNamespace)
            {
                var genericServiceForDataModel = genericServiceType.MakeGenericType(modelType);
                var addScopedMethod = addScopedGenericMethod.MakeGenericMethod(genericServiceForDataModel, genericServiceForDataModel);
                addScopedMethod.Invoke(services, new object[] { services });
            }
        }

        private static void DeclareServicesAsScoped(IServiceCollection services, MethodInfo addScopedGenericMethod)
        {
            var servicesLayerAssemblyName = configuration["Framework:Configuration:Services:Assembly"];
            if (string.IsNullOrEmpty(servicesLayerAssemblyName))
                return;

            var modelSuffix = configuration["Framework:Configuration:Models:ModelSuffix"];

            var servicesInterfaces = Assembly.Load(new AssemblyName(servicesLayerAssemblyName)).ExportedTypes.Where(type => type.GetTypeInfo().IsInterface);
            var servicesClasses = Assembly.Load(new AssemblyName(servicesLayerAssemblyName)).ExportedTypes.Where(type => type.GetTypeInfo().IsClass);

            foreach (var modelType in modelTypesInTheNamespace)
            {
                var modelName = modelType.Name;

                if (!string.IsNullOrEmpty(modelSuffix) && modelType.Name.EndsWith(modelSuffix))
                {
                    var indexSuffix = modelType.Name.IndexOf(modelSuffix);
                    modelName = modelType.Name.Remove(indexSuffix);
                }

                var serviceInterface = servicesInterfaces.SingleOrDefault(svc => svc.Name == $"I{modelName}Service");
                var serviceClass = servicesClasses.SingleOrDefault(svc => svc.Name == $"{modelName}Service");

                if (serviceInterface != null && serviceClass != null)
                {
                    var addScopedMethod = addScopedGenericMethod.MakeGenericMethod(serviceInterface, serviceClass);
                    addScopedMethod.Invoke(services, new object[] { services });
                }
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
