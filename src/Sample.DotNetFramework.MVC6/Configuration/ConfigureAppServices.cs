using Microsoft.Extensions.DependencyInjection;
using Sample.DotNetFramework.ServicesLayer.Interfaces;
using Sample.DotNetFramework.ServicesLayer.Services;

namespace Sample.DotNetFramework.MVC6.Configuration
{
    /// <summary>
    /// Classe de configuration des services de l'application
    /// </summary>
    public static class ConfigureAppServices
    {
        /// <summary>
        /// Méthode d'ajout des services de l'application
        /// </summary>
        /// <param name="services">Collection des services en provenance du startup</param>
        public static void Configure(ref IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
        }
    }
}
