using Framework.Core.DAL.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Sample.DotNetFramework.DataLayer.Infrastructure
{
    public class SampleDbContext : DbContext, IDataBaseContext
    {
        private IConfigurationRoot configuration = null;

        public SampleDbContext(IConfigurationRoot configuration)
        {
            this.configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#warning Améliorer la récupération de la connection string
            var connectionString = configuration["ConnectionStrings:DefaultConnection"];
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigurationModels.Configure(ref modelBuilder);
        }
    }
}
