using Framework.Core.DAL.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Sample.DotNetFramework.Common.DTO;

namespace Sample.DotNetFramework.DataLayer
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>();
            modelBuilder.Entity<SampleModel>();
        }
    }
}
