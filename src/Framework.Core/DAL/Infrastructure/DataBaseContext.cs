using Microsoft.EntityFrameworkCore;

namespace Framework.Core.DAL.Infrastructure
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext()
            : base()
        {
        }

        public DataBaseContext(DbContextOptions options)
            : base(options)
        {
        }
    }
}
