using Dapper;
using Framework.Core.DAL.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Framework.Core.DAL.Extensions
{
    public static class DataBaseContextExtension
    {
        public static void EnsureCreated(this DataBaseContext context)
        {
            CreateDatabaseIfNeeded(context);
        }

        public static void EnsureCreatedAndSeedData(this DataBaseContext context, string sqlScriptPath)
        {
            CreateDatabaseIfNeeded(context);
            SeedData(context, sqlScriptPath);
        }

        private static void CreateDatabaseIfNeeded(DataBaseContext context)
        {
            if (context.Database.GetPendingMigrations().Count() > 0)
            {
                context.Database.Migrate();
            }
        }

        private static void SeedData(DataBaseContext context, string sqlScriptFullPath)
        {
            if (string.IsNullOrEmpty(sqlScriptFullPath) || !File.Exists(sqlScriptFullPath))
                throw new ArgumentException("The SQL script path is empty or not found.");

            context.Database.GetDbConnection().QueryMultiple(File.ReadAllText(sqlScriptFullPath, Encoding.UTF8));
        }
    }
}
