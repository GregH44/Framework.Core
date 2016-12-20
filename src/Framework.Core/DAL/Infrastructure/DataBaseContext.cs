using Microsoft.EntityFrameworkCore;

namespace Framework.Core.DAL.Infrastructure
{
    public class DataBaseContext : DbContext, IDataBaseContext
    {
        public DataBaseContext()
            : base()
        {
        }

        public DataBaseContext(DbContextOptions<DataBaseContext> options)
            : base(options)
        {
        }

        public DataBaseContext(object options)
            : base((DbContextOptions)options)
        {
        }
    }
}
