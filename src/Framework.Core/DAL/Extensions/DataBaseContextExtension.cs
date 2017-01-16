using Dapper;
using Framework.Core.DAL.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
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

        /// <param name="sqlScriptsPathToDirectory">Path to directory which contains SQL scripts (Ex : D:\SqlScriptsDirectory)</param>
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

            var connection = context.Database.GetDbConnection();

            foreach (var pendingMigration in pendingMigrations)
            {
                SeedData(sqlScriptsPathToDirectory, connection, pendingMigration);
            }
        }

        private static void SeedData(string sqlScriptsPathToDirectory, DbConnection connection, string pendingMigration)
        {
            string pendingMigrationDirectoryPath = Path.Combine(sqlScriptsPathToDirectory, pendingMigration);

            if (Directory.Exists(pendingMigrationDirectoryPath))
            {
                var sqlScripts = Directory.GetFiles(pendingMigrationDirectoryPath, "*.sql");

                foreach (var sqlScript in sqlScripts)
                {
                    connection.QueryMultiple(File.ReadAllText(sqlScript, Encoding.UTF8));
                }
            }
        }
    }
}
