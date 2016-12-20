using Framework.Core.Service;
using Microsoft.Extensions.DependencyInjection;
using Sample.DotNetFramework.Common.DTO;

namespace Sample.DotNetFramework.MVC6.Configuration
{
    /// <summary>
    /// Classe de configuration des services de l'application
    /// </summary>
    internal static class ConfigureAppServices
    {
        /// <summary>
        /// Méthode d'ajout des services de l'application
        /// </summary>
        /// <param name="services">Collection des services en provenance du startup</param>
        internal static void Configure(ref IServiceCollection services)
        {
            services.AddScoped<GenericService<UserModel>, GenericService<UserModel>>();
            services.AddScoped<IServiceBase<UserModel>, GenericService<UserModel>>();
        }
    }
}
