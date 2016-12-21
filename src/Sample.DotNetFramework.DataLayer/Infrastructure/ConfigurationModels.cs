using Microsoft.EntityFrameworkCore;
using Sample.DotNetFramework.Common.DTO;

namespace Sample.DotNetFramework.DataLayer.Infrastructure
{
    internal static class ConfigurationModels
    {
        public static void Configure(ref ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>();
            modelBuilder.Entity<SampleModel>();
        }
    }
}
