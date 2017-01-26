using Dapper;
using Framework.Core.Enums;
using Framework.Core.Exceptions;
using Framework.Core.Tools;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Framework.Core.DAL.Repository
{
    internal sealed class GenericApiRepository<TEntity> : GenericRepository<TEntity>
        where TEntity : class
    {
        internal GenericApiRepository(DbContext context) : base(context)
        {
        }

        public override void Delete(object keyValue)
        {
            try
            {
                context.Database.GetDbConnection().Query(
                    SqlBuilder.Build<TEntity>(GlobalEnums.SqlBuildOperation.Delete, "Identifier"),
                    new
                    {
                        Identifier = keyValue.GetType()
                    });
            }
            catch (Exception exception)
            {
                throw new GenericRepositoryException($"An error occurred while obtaining an entity [id={keyValue}] !", exception);
            }
        }

        public override TEntity Get(object keyValue)
        {
            try
            {
                return dbSet.FromSql(SqlBuilder.Build<TEntity>(GlobalEnums.SqlBuildOperation.Select, "p0"), keyValue).SingleOrDefault();
            }
            catch (Exception exception)
            {
                throw new GenericRepositoryException($"An error occurred while obtaining an entity [id={keyValue}] !", exception);
            }
        }
    }
}
