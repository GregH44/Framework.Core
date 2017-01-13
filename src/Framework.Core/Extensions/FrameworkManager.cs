using Framework.Core.Attributes;
using Framework.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Core
{
    public static class FrameworkManager
    {
        private static IConfigurationRoot configuration = ConfigurationManager.Configuration;

        public static void Initialize(this IServiceCollection services)
        {
            InitializeFilters(services);
            ConfigureFramework.Initialize(services);
        }

        private static void InitializeFilters(IServiceCollection services)
        {
            // Exception handler service
            services.AddScoped<ExceptionHandlerAttribute>();
            // CorrelationId manager service
            services.AddScoped<CorrelationIdAttribute>();
        }
    }
}
