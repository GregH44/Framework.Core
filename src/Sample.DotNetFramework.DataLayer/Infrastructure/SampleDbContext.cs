using Framework.Core.DAL.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Sample.DotNetFramework.DataLayer.Infrastructure
{
    public class SampleDbContext : DataBaseContext
    {
        public SampleDbContext()
            : base()
        {
        }

        public SampleDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigurationModels.Configure(ref modelBuilder);
        }
    }
}
