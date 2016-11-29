using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Framework.Core.DAL.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Framework.Core.Exceptions;

namespace Framework.Core.DAL.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class
    {
        protected DataBaseContext context = null;
        protected DbSet<TEntity> dbSet = null;

        public GenericRepository(DataBaseContext context)
        {
            this.context = context;
            dbSet = context.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public void Add(IEnumerable<TEntity> entities)
        {
            dbSet.AddRange(entities);
        }

        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                IEnumerable<TEntity> entities = dbSet.Where(predicate);
                if (entities.Any())
                    Delete(entities);
            }
            catch (ArgumentNullException argumentNullException)
            {
                throw new GenericRepositoryException("Source or predicate is null !", argumentNullException);
            }
            catch (InvalidOperationException invalidOperationException)
            {
                throw new GenericRepositoryException("More than one elements satisfies the condition in predicate !", invalidOperationException);
            }
            catch (Exception exception)
            {
                throw new GenericRepositoryException("An error occurred while deleting entity !", exception);
            }
        }

        public void Delete(TEntity entity)
        {
            dbSet.Remove(entity);
        }

        public void Delete(IEnumerable<TEntity> entities)
        {
            dbSet.RemoveRange(entities);
        }

        public bool Exists(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return dbSet.Any(predicate);
            }
            catch (ArgumentNullException argumentNullException)
            {
                throw new GenericRepositoryException("Source or predicate is null !", argumentNullException);
            }
        }

        public TEntity Get(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            try
            {
                IQueryable<TEntity> query = dbSet;

                if (predicate != null)
                {
                    query = query.Where(predicate);
                }

                if (includeProperties.Any())
                {
                    foreach (var includeProperty in includeProperties)
                    {
                        query = query.Include(includeProperty);
                    }
                }

                return query.AsNoTracking().FirstOrDefault();
            }
            catch (ArgumentNullException argumentNullException)
            {
                throw new GenericRepositoryException("Source or predicate is null !", argumentNullException);
            }
            catch (Exception exception)
            {
                throw new GenericRepositoryException("An error occurred while obtaining an entity !", exception);
            }
        }

        public async Task<IEnumerable<TEntity>> GetList(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int? skip = default(int?), int? take = default(int?), params Expression<Func<TEntity, object>>[] includeProperties)
        {
            try
            {
                IQueryable<TEntity> query = dbSet;

                if (predicate != null)
                {
                    query = query.Where(predicate);
                }

                if (skip.HasValue)
                {
                    query = query.Skip(skip.Value);
                }

                if (take.HasValue)
                {
                    query = query.Take(take.Value);
                }

                if (includeProperties.Any())
                {
                    foreach (var includeProperty in includeProperties)
                    {
                        query = query.Include(includeProperty);
                    }
                }

                if (orderBy != null)
                {
                    return await orderBy(query).AsNoTracking().ToListAsync();
                }
                else
                {
                    return await query.AsNoTracking().ToListAsync();
                }
            }
            catch (ArgumentNullException argumentNullException)
            {
                throw new GenericRepositoryException("Source or predicate is null !", argumentNullException);
            }
            catch (Exception exception)
            {
                throw new GenericRepositoryException("An error occurred while obtaining a list of entities !", exception);
            }
        }

        public int Save()
        {
            try
            {
                return context.SaveChanges();
            }
            catch (Exception exception)
            {
                throw new GenericRepositoryException("An error occurred while inserting/updating/deleting entries !", exception);
            }
        }

        public void Update(TEntity entity)
        {
            dbSet.Update(entity);
        }

        public void Update(IEnumerable<TEntity> entities)
        {
            dbSet.UpdateRange(entities);
        }
    }
}
