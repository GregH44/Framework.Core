using Framework.Core.DAL.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Sample.DotNetFramework.DataLayer
{
    public class MyDbContext : DatabaseContext
    {
        public MyDbContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
    }
}
