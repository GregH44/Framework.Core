using Framework.Core.Attributes;
using Framework.Core.Configuration;
using Framework.Core.Extensions;
using Framework.Core.Resolvers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Framework.Core
{
    public static class FrameworkManager
    {
        private static IConfigurationRoot configuration = ConfigurationManager.Configuration;
        private static SortedDictionary<string, Type> modelTypesInTheNamespace = new SortedDictionary<string, Type>();
        private static SortedDictionary<int, PropertyInfo> keysPropertyInfoModelTypes = new SortedDictionary<int, PropertyInfo>();

        public static void InitializeFramework(this IServiceCollection services)
        {
            InitializeFilters(services);
            InitializeServices(services);
        }

        /// <summary>
        /// This method should be used after the calling extension "InitializeFramework"
        /// </summary>
        public static PropertyInfo GetIdentityPropertyInfoModelTypes(int modelTypeHashCode)
        {
            PropertyInfo property = null;
            keysPropertyInfoModelTypes.TryGetValue(modelTypeHashCode, out property);
            return property;
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
            LoadModelTypes();
            InitializeServiceResolver();
            DeclareServicesAsScoped(services);
        }

        private static void DeclareServicesAsScoped(IServiceCollection services)
        {
            var servicesLayerAssemblyName = configuration["Framework:Configuration:Services:Assembly"];
            if (string.IsNullOrEmpty(servicesLayerAssemblyName))
                return;

            var addScopedMethod =
                typeof(ServiceCollectionServiceExtensions)
                .GetMethods()
                .Where(method => method.Name == "AddScoped" && method.GetGenericArguments().Length == 2 && method.GetParameters().Count() == 1)
                .Single();

            var modelSuffix = configuration["Framework:Configuration:Models:ModelSuffix"];

            var serviceInterfaces = Assembly.Load(new AssemblyName(servicesLayerAssemblyName)).ExportedTypes.Where(type => type.GetTypeInfo().IsInterface);
            var serviceClasses = Assembly.Load(new AssemblyName(servicesLayerAssemblyName)).ExportedTypes.Where(type => type.GetTypeInfo().IsClass);

            foreach (var kvpModelType in modelTypesInTheNamespace)
            {
                var modelName = kvpModelType.Key.DeleteSuffix(modelSuffix);
                services.AddServicesAsScoped(addScopedMethod, modelName, serviceInterfaces, serviceClasses);
            }
        }

        private static void InitializeServiceResolver()
        {
            var genericServiceType = Assembly.Load(new AssemblyName("Framework.Core")).ExportedTypes.Single(type => type.Name.StartsWith("GenericService"));
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
                throw new Exception($"Assembly is not configure for models. Please check your settings file (Ex: appsettings.json).");

            var assembly = Assembly.Load(new AssemblyName(assemblyModels));

            if (assembly == null)
                throw new Exception($"Assembly {assemblyModels} not found !");

            foreach (var model in assembly.ExportedTypes.Where(mod => mod.Namespace.StartsWith(namespaceModels)))
            {
                modelTypesInTheNamespace.Add(model.Name, model);
                SetKeyPropertyInfoFromModelType(model);
                SetCrudOperationsFromModelType(model);
            }
        }

        private static void SetKeyPropertyInfoFromModelType(Type modelType)
        {
            var property = modelType.GetProperties().FirstOrDefault(prop => prop.GetCustomAttributes<KeyAttribute>().Any());

            if (property != null)
                keysPropertyInfoModelTypes.Add(modelType.GetHashCode(), property);
        }

        private static void SetCrudOperationsFromModelType(Type modelType)
        {
            var crudOperationAttribute = modelType.GetTypeInfo().GetCustomAttribute<CrudOperationsAttribute>();

            if (crudOperationAttribute != null && crudOperationAttribute.crudOperationsAllowed.Length > 0)
                CrudResolver.crudOperationsModelTypes.Add(modelType.Name, crudOperationAttribute.crudOperationsAllowed);
        }
    }
}
