using Framework.Core.Attributes;
using Framework.Core.Extensions;
using Framework.Core.Resolvers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Framework.Core.Configuration
{
    internal static class ConfigureFramework
    {
        private static SortedDictionary<int, PropertyInfo> keysPropertyInfoModelTypes = new SortedDictionary<int, PropertyInfo>();
        private static SortedDictionary<string, Type> modelTypesInTheNamespace = new SortedDictionary<string, Type>();

        internal static void Initialize(IServiceCollection services)
        {
            InitializeConfiguration(services);
            InitializeModels(services);
        }

        /// <summary>
        /// This method should be used after the calling extension "InitializeFramework"
        /// </summary>
        internal static PropertyInfo GetIdentityPropertyInfoModelTypes(int modelTypeHashCode)
        {
            PropertyInfo property = null;
            keysPropertyInfoModelTypes.TryGetValue(modelTypeHashCode, out property);
            return property;
        }

        private static void InitializeConfiguration(IServiceCollection services)
        {
            ConfigurationManager.Configuration.GetSection("Framework:Configuration:Models").Bind(FrameworkSettings.Configuration.DataModelConfig);
            ConfigurationManager.Configuration.GetSection("Framework:Configuration:ViewModels").Bind(FrameworkSettings.Configuration.ViewModelConfig);
            ConfigurationManager.Configuration.GetSection("Framework:Configuration:Services").Bind(FrameworkSettings.Configuration.ServiceConfig);
        }

        private static void InitializeModels(IServiceCollection services)
        {
            var modelsAssembly = GetModelsAssembly();
            var viewModelsType = GetViewModelsType();

            var dataModels = modelsAssembly.ExportedTypes.Where(mod => mod.Namespace.StartsWith(FrameworkSettings.Configuration.DataModelConfig.NamespaceModels));

            AddDataModelsToCache(dataModels);
            InitializeMVVMAutoMapper(dataModels, viewModelsType);
            InitializeServiceResolver();
            DeclareServicesAsScoped(services);
        }

        private static void AddDataModelsToCache(IEnumerable<Type> dataModelTypes)
        {
            foreach (var model in dataModelTypes)
            {
                LoadModels(model);
            }
        }

        private static void InitializeMVVMAutoMapper(IEnumerable<Type> dataModelTypes, IEnumerable<Type> viewModelTypes)
        {
            if (viewModelTypes.Count() == 0)
                return;

            foreach (var dataModel in dataModelTypes)
            {
                string modelNameWithoutSuffix = dataModel.Name.DeleteSuffix(FrameworkSettings.Configuration.DataModelConfig.Suffix);
                var viewModelType = viewModelTypes.FirstOrDefault(model => model.Name.StartsWith(modelNameWithoutSuffix));

                if (viewModelType != null)
                    AddViewModelsToAutoMapper(dataModel, viewModelType);
            }
        }

        private static void InitializeServiceResolver()
        {
            var genericServiceType = Assembly.Load(new AssemblyName(typeof(FrameworkManager).Namespace)).ExportedTypes.Single(type => type.Name.StartsWith("GenericService"));
            new ServiceResolver(
                FrameworkSettings.Configuration.DataModelConfig.Suffix,
                modelTypesInTheNamespace,
                genericServiceType
                );
        }

        private static void DeclareServicesAsScoped(IServiceCollection services)
        {
            if (string.IsNullOrEmpty(FrameworkSettings.Configuration.ServiceConfig.Assembly))
                return;

            var addScopedMethod =
                typeof(ServiceCollectionServiceExtensions)
                .GetMethods()
                .Where(method => method.Name == "AddScoped" && method.GetGenericArguments().Length == 2 && method.GetParameters().Count() == 1)
                .Single();

            var serviceInterfaces =
                Assembly.Load(
                    new AssemblyName(FrameworkSettings.Configuration.ServiceConfig.Assembly))
                    .ExportedTypes.Where(type => type.GetTypeInfo().IsInterface);
            var serviceClasses =
                Assembly.Load(
                    new AssemblyName(FrameworkSettings.Configuration.ServiceConfig.Assembly))
                    .ExportedTypes.Where(type => type.GetTypeInfo().IsClass);

            foreach (var kvpModelType in modelTypesInTheNamespace)
            {
                var modelName = kvpModelType.Key.DeleteSuffix(FrameworkSettings.Configuration.DataModelConfig.Suffix);
                services.AddServicesAsScoped(addScopedMethod, modelName, serviceInterfaces, serviceClasses);
            }
        }

        private static Assembly GetModelsAssembly()
        {
            var assemblyModels = FrameworkSettings.Configuration.DataModelConfig.Assembly;

            if (string.IsNullOrEmpty(assemblyModels))
                throw new Exception($"Assembly is not define for models. Please check your settings file (Ex: appsettings.json).");

            var assembly = Assembly.Load(new AssemblyName(assemblyModels));

            if (assembly == null)
                throw new Exception($"Assembly {assemblyModels} not found !");

            return assembly;
        }

        private static IEnumerable<Type> GetViewModelsType()
        {
            IEnumerable<Type> viewModels = null;
            var assemblyViewModels = FrameworkSettings.Configuration.ViewModelConfig.Assembly;
            var viewModelSuffix = FrameworkSettings.Configuration.ViewModelConfig.Suffix;

            if (!string.IsNullOrEmpty(assemblyViewModels))
            {
                if (string.IsNullOrEmpty(viewModelSuffix))
                    throw new ArgumentNullException("The view models suffix should have a value if you define an assembly view model. Please check your settings file (Ex: appsettings.json).");

                var assembly = Assembly.Load(new AssemblyName(assemblyViewModels));

                if (assembly == null)
                    throw new Exception($"Assembly {assemblyViewModels} not found !");

                viewModels = assembly.ExportedTypes.Where(mod => mod.Name.EndsWith(viewModelSuffix));
            }
            else
            {
                viewModels = new List<Type>();
            }

            return viewModels;
        }

        private static void LoadModels(Type modelType)
        {
            modelTypesInTheNamespace.Add(modelType.Name, modelType);

            SetKeyPropertyInfoFromModelType(modelType);
            SetCrudOperationsFromModelType(modelType);
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
                CrudResolver.crudOperationsModelTypes.Add(
                    modelType.Name.DeleteSuffix(FrameworkSettings.Configuration.DataModelConfig.Suffix),
                    crudOperationAttribute.crudOperationsAllowed);
        }

        private static void AddViewModelsToAutoMapper(Type dataModelType, Type viewModelType)
        {
            Mapper.modelsToMap.Add(dataModelType, viewModelType);
        }
    }
}
