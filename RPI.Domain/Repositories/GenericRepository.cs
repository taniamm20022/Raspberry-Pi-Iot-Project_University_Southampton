using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using tesr.Data;
using tesr.Data.Extensions;
using tesr.Data.Interfaces;

namespace CSDI.Data.Repositories.Interfaces
{
    public class
        GenericRepository<TContext, TEntity>
        : IGenericRepository<TEntity>
        where TContext : DbContext where TEntity : class
    {

        private readonly TContext DataContext;
        protected DbSet<TEntity> DbSet;


        #region CTOR
        public GenericRepository(
            TContext dbContext)
        {
            DataContext = dbContext;
            DbSet = DataContext.Set<TEntity>();
        }
        #endregion CTOR



        #region Add/Update/Delete
        public virtual void Add(
            TEntity entity)
        {
            DbSet.Add(entity);
        }


        public virtual void Update(
            TEntity entity)
        {
            DbEntityEntry dbEntityEntry = DataContext.Entry<TEntity>(entity);

            if (dbEntityEntry.State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }

            dbEntityEntry.State = EntityState.Modified;
        }


        public virtual async Task<int> DeleteAsync(
            IEnumerable<TEntity> entities)
        {
            var noDeleted = 0;
            if (entities == null)
            {
                return noDeleted;
            }

            foreach (var entity in entities)
            {
                noDeleted += Delete(entity);
            }

            return noDeleted;
        }


        public virtual int Delete(
            TEntity entity)
        {
            if (DataContext.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }

            DbSet.Remove(entity);
            return 1;
        }

        #endregion Add/Update/Delete



        #region GetSingle
        public virtual TEntity GetSingle(
             Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool asNoTracking = false)
        {
            var query = GetQueryable(predicate, null, include, null, null);
            return query.SingleOrDefault();
        }

        #endregion



        #region Queryable
        public virtual IQueryable<TEntity> AsQueryable(bool asNoTracking = false)
        {
            IQueryable<TEntity> query = DbSet;
            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            return query.AsQueryable();
        }


        protected virtual IQueryable<TEntity> GetQueryable(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            int? skip = null,
            int? take = null)
        {
            IQueryable<TEntity> query = DbSet;
            query = query.AsQueryable();

            query = GetQueryable(query, predicate, orderBy, include, skip, take);
            return query;
        }


        protected static IQueryable<TEntity> GetQueryable(
            IQueryable<TEntity> query,
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include,
            int? skip,
            int? take)
        {
            if (include != null)
            {
                query = include(query);
            }


            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query;
        }
        #endregion Queryable



        #region PagedList Methods
        /// <inheritdoc />
        public virtual async Task<PaginatedList<TEntity>> GetPagedListAsync(
            IPagingRequest pagingInfo,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool asNoTracking = false)
        {
            var query = GetQueryable(predicate, null, include, null, null);
            return await query.ToPaginatedListAsync(pagingInfo);
        }


        /// <inheritdoc />
        public virtual async Task<PaginatedList<TEntity>> GetPagedListAsync(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            int pageIndex = 0,
            int pageSize = 20,
            SortExpression sortExpressions = null,
            bool asNoTracking = false)
        {
            var query = GetQueryable(predicate, orderBy, include, null, null);
            return await query.ToPaginatedListAsync(pageIndex, pageSize, sortExpressions);
        }


        #endregion PagedList Methods



        #region List Methods
        /// <inheritdoc />
        public virtual async Task<IList<TEntity>> GetListAsync(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            int? skip = null,
            int? take = null)
        {
            var query = GetQueryable(predicate, orderBy, include, skip, take);
            return await query.ToListAsync();
        }       
        #endregion List Methods
        


        #region Data Context Entity State Helpers
        public void SetEntityState(
            TEntity entity,
            EntityState state)
        {
            var entry = DataContext.Entry(entity);
            entry.State = state;
        }
        #endregion Data Context Entity State Helpers
    }
}

