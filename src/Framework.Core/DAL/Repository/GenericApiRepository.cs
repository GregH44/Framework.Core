using Dapper;
using Framework.Core.DAL.Infrastructure;
using Framework.Core.Enums;
using Framework.Core.Exceptions;
using Framework.Core.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace Framework.Core.DAL.Repository
{
    internal sealed class GenericApiRepository<TEntity> : GenericRepository<TEntity>
        where TEntity : class
    {
        internal GenericApiRepository(IDataBaseContext context) : base(context)
        {
        }

        public void Delete(long id)
        {
            try
            {
                context.Database.GetDbConnection().Query(
                    SqlBuilder.Build<TEntity>(GlobalEnums.SqlBuildOperation.Delete, "Identifier"),
                    new
                    {
                        Identifier = id
                    });
            }
            catch (Exception exception)
            {
                throw new GenericRepositoryException($"An error occurred while obtaining an entity [id={id}] !", exception);
            }
        }

        public TEntity Get(long id)
        {
            try
            {
                return dbSet.FromSql(SqlBuilder.Build<TEntity>(GlobalEnums.SqlBuildOperation.Select, "p0"), id).SingleOrDefault();
            }
            catch (Exception exception)
            {
                throw new GenericRepositoryException($"An error occurred while obtaining an entity [id={id}] !", exception);
            }
        }
    }
}
