using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Framework.Core.DAL.Repository
{
    public interface IGenericRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// Add a new entity to DbContext
        /// </summary>
        /// <param name="entity">Entity to add</param>
        void Add(TEntity entity);

        /// <summary>
        /// Add a new entity to DbContext
        /// </summary>
        /// <param name="entity">Entity to add</param>
        void Add(IEnumerable<TEntity> entity);

        /// <summary>
        /// Delete entity based on its primary key value
        /// </summary>
        /// <param name="entity">Entity to delete</param>
        void Delete(object keyValue);

        /// <summary>
        /// Delete entities based on the predicate
        /// </summary>
        /// <param name="predicate">Predicate used to delete entities</param>
        void Delete(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Delete entity based on data model
        /// </summary>
        /// <param name="entity">Entity to delete</param>
        void Delete(TEntity entity);

        /// <summary>
        /// Delete entities based on data model <see cref="TEntity"/>
        /// </summary>
        /// <param name="entities">Entities to delete</param>
        void Delete(IEnumerable<TEntity> entities);

        /// <summary>
        /// Check if an entity exists into the database based on the predicate
        /// </summary>
        /// <param name="predicate">Predicate used to find entities</param>
        /// <returns>True if exists ; Otherwise False.</returns>
        bool Exists(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Get entity by its primary key value
        /// </summary>
        /// <param name="keyValue">The value of the primary key</param>
        /// <returns>Return the entity ; Otherwise null</returns>
        TEntity Get(object keyValue);

        /// <summary>
        /// Get entity that satisfy the predicate. If many, the first will be returned.
        /// </summary>
        /// <param name="predicate">Predicate used to get entity</param>
        /// <returns>Return the entity that satisfy the predicate ; Otherwise null</returns>
        TEntity Get(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);

        /// <summary>
        /// Get entities that satisfy the predicate
        /// </summary>
        /// <param name="predicate">Predicate used to get entities</param>
        /// <param name="orderBy">Define the kind of order</param>
        /// <param name="skip">Skip X items</param>
        /// <param name="take">Take the next X items. If skip is defined, get the next X items after skip.</param>
        /// <param name="includeProperties">Define related entities for each TEntity <seealso cref="TEntity"/>></param>
        /// <returns>Return an asynchronous list that satisfy the predicate</returns>
        Task<IEnumerable<TEntity>> GetList(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            int? skip = null, int? take = null,
            params Expression<Func<TEntity, object>>[] includeProperties);

        /// <summary>
        /// Persist entities stored into the DbContext to database
        /// </summary>
        /// <returns>Return the number of rows affected</returns>
        int Save();

        /// <summary>
        /// Update an entity to DbContext
        /// </summary>
        /// <param name="entity">Entity to update</param>
        void Update(TEntity entity);

        /// <summary>
        /// Update an entity to DbContext
        /// </summary>
        /// <param name="entities">Entities to update</param>
        void Update(IEnumerable<TEntity> entities);
    }
}
