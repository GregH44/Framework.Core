using Microsoft.EntityFrameworkCore;

namespace Framework.Core.DAL.Infrastructure
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options)
            : base(options)
        {
        }
    }
}
