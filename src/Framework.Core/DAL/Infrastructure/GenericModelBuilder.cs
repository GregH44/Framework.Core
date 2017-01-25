using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Reflection;

namespace Framework.Core.DAL.Infrastructure
{
    public class GenericModelBuilder
    {
        protected readonly ModelBuilder modelBuilder = null;
        protected readonly IConfigurationRoot configuration = null;

        public GenericModelBuilder(IConfigurationRoot configuration)
        {
            var coreConventionSetBuilder = new CoreConventionSetBuilder();
            var sqlConventionSetBuilder = new SqlServerConventionSetBuilder(new SqlServerTypeMapper(), null, null);
            var conventionSet = sqlConventionSetBuilder.AddConventions(coreConventionSetBuilder.CreateConventionSet());
            modelBuilder = new ModelBuilder(conventionSet);

            this.configuration = configuration;
        }

        public virtual IModel InitializeEntities()
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
                modelBuilder.Entity(modelType);
            }

            return modelBuilder.Model;
        }
    }
}
