using Framework.Core.Enums;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Tools
{
    internal static class SqlBuilder
    {
        private static ConcurrentDictionary<string, string> queries = new ConcurrentDictionary<string, string>();

        internal static string Build<TEntity>(GlobalEnums.SqlBuildOperation operation, string paramIdentifierName)
            where TEntity : class
        {
            string query = string.Empty;

            switch (operation)
            {
                case GlobalEnums.SqlBuildOperation.Select:
                    query = queries.GetOrAdd(typeof(TEntity).Name + operation.ToString(), BuildSelect<TEntity>() + BuildFrom<TEntity>() + BuildWhere<TEntity>(paramIdentifierName));
                    break;
                case GlobalEnums.SqlBuildOperation.Delete:
                    query = queries.GetOrAdd(typeof(TEntity).Name + operation.ToString(), BuildDelete<TEntity>() + BuildWhere<TEntity>(paramIdentifierName));
                    break;
                default:
                    break;
            }

            return query;
        }

        private static string BuildDelete<TEntity>()
            where TEntity : class
        {
            string query = "Delete From ";
            var entityType = typeof(TEntity);
            var tableAttributes = entityType.GetTypeInfo().GetCustomAttributes<TableAttribute>();

            if (tableAttributes.Any())
            {
                query += $"[{tableAttributes.First().Name}] ";
            }
            else
            {
                query += $"[{entityType.Name}] ";
            }

            return query;
        }

        private static string BuildFrom<TEntity>()
            where TEntity : class
        {
            string query = "From ";
            var entityType = typeof(TEntity);
            var tableAttributes = entityType.GetTypeInfo().GetCustomAttributes<TableAttribute>();

            if (tableAttributes.Any())
            {
                query += $"[{tableAttributes.First().Name}] ";
            }
            else
            {
                query += $"[{entityType.Name}] ";
            }

            return query;
        }

        private static string BuildSelect<TEntity>()
            where TEntity : class
        {
            StringBuilder queryBuilder = new StringBuilder("Select ");
            var entityType = typeof(TEntity);
            var properties = entityType.GetProperties();
            
            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes<ColumnAttribute>();
                
                if (attributes.Count() > 1)
                    throw new InvalidOperationException("The input sequence contains more than one element.");
                
                if (attributes.Any())
                {
                    var column = attributes.First();

                    if (!string.IsNullOrEmpty(column.Name))
                        queryBuilder.Append($"[{column.Name}], ");
                }
                else
                {
                    queryBuilder.Append($"[{property.Name}], ");
                }
            }

            queryBuilder.Remove(queryBuilder.Length - 2, 1);

            return queryBuilder.ToString();
        }

        private static string BuildWhere<TEntity>(string paramIdentifierName)
        {
            string query = "Where ";
            var entityType = typeof(TEntity);

            var propertiesKey = entityType.GetProperties().ToList().Where(prop => prop.GetCustomAttributes<KeyAttribute>().Any());

            if (propertiesKey.Count() > 1)
                throw new InvalidOperationException("The input sequence contains more than one element.");
            else if (propertiesKey.Count() == 0)
                throw new ArgumentException($"The {nameof(KeyAttribute)} is not found for {entityType.Name}.");

            query += $"[{propertiesKey.First().Name}] = @{paramIdentifierName} ";

            return query;
        }
    }
}
