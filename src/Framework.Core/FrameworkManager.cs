using Framework.Core.Attributes;
using Framework.Core.Configuration;
using Framework.Core.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Core
{
    public static class FrameworkManager
    {
        public static void InitializeGenericApi(IServiceCollection services)
        {
            var configuration = ConfigurationManager.Configuration;

            new ServiceResolver(
                configuration["Framework:Configuration:Models:Assembly"],
                configuration["Framework:Configuration:Models:NamespaceModels"],
                configuration["Framework:Configuration:Models:ModelSuffix"]
                ).LoadModels();

            ServicesProvider.Services = services.BuildServiceProvider();
        }
    }
}
