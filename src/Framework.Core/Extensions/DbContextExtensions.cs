using Framework.Core.DAL.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Core.Extensions
{
    public static class DbContextExtensions
    {
        public static void AddDbContext(
            this IServiceCollection services,
            string connectionString,
            string assemblyDestinationToGenerateMigrations,
            IModel model)
        {
            services.AddDbContext<DataBaseContext>(optionsAction =>
            {
                optionsAction.UseSqlServer(connectionString, sqlServerOptions => sqlServerOptions.MigrationsAssembly(assemblyDestinationToGenerateMigrations));
                optionsAction.UseModel(model);
            });
        }
    }
}
