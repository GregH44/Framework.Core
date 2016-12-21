using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Reflection;

namespace Framework.Core.DAL.Infrastructure
{
    public class GenericModelBuilder : ModelBuilder
    {
        private readonly IConfigurationRoot configuration = null;

        public GenericModelBuilder(ConventionSet conventions, IConfigurationRoot configuration)
            : base(conventions)
        {
            this.configuration = configuration;
        }

        public void InitializeDataModels()
        {
            var assemblyModelsName = configuration["Framework:Configuration:Models:Assembly"];
            var namespaceModels = configuration["Framework:Configuration:Models:NamespaceModels"];

            if (string.IsNullOrEmpty(assemblyModelsName) || string.IsNullOrEmpty(namespaceModels))
                throw new ArgumentNullException("The assembly models name or the namespace models are not defined. Please check your appsettings.json.");

            var assembly = Assembly.Load(new AssemblyName(assemblyModelsName));

            if (assembly == null)
                throw new Exception($"Assembly {assemblyModelsName} not found !");

            var modelsTypeInTheNamespace = assembly.ExportedTypes.Where(mod => mod.Namespace.StartsWith(namespaceModels)).OrderBy(o => o.Name);
            
            foreach (var modelType in modelsTypeInTheNamespace)
            {
                Entity(modelType);
            }
        }
    }
}
