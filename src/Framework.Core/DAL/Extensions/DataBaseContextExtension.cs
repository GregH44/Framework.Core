using Dapper;
using Framework.Core.DAL.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Framework.Core.DAL.Extensions
{
    public static class DataBaseContextExtension
    {
        public static void MigrateDatabase(this DatabaseContext context)
        {
            var pendingMigrations = context.Database.GetPendingMigrations();

            CreateDatabaseIfNeeded(context, pendingMigrations);
        }
        
        /// <param name="sqlScriptsPathToDirectory">Path to directory which contains SQL scripts</param>
        public static void MigrateDatabaseAndSeedData(this DatabaseContext context, string sqlScriptsPathToDirectory)
        {
            var pendingMigrations = context.Database.GetPendingMigrations();

            CreateDatabaseIfNeeded(context, pendingMigrations);
            SeedDataIfNeeded(context, sqlScriptsPathToDirectory, pendingMigrations);
        }

        private static void CreateDatabaseIfNeeded(DatabaseContext context, IEnumerable<string> pendingMigrations)
        {
            if (pendingMigrations.Count() > 0)
            {
                context.Database.Migrate();
            }
        }

        private static void SeedDataIfNeeded(DatabaseContext context, string sqlScriptsPathToDirectory, IEnumerable<string> pendingMigrations)
        {
            if (string.IsNullOrEmpty(sqlScriptsPathToDirectory) || !Directory.Exists(sqlScriptsPathToDirectory))
                throw new ArgumentException("The SQL script path is empty or not found.");

            foreach (var pendingMigration in pendingMigrations)
            {
                string fullPath = Path.Combine(sqlScriptsPathToDirectory, pendingMigration + ".sql");

                if (File.Exists(fullPath))
                    context.Database.GetDbConnection().QueryMultiple(File.ReadAllText(fullPath, Encoding.UTF8));
            }
        }
    }
}
