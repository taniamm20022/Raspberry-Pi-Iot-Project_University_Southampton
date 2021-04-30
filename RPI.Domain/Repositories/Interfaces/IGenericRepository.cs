using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using tesr.Data;
using tesr.Data.Interfaces;

namespace CSDI.Data.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity>
        where TEntity : class
    {
        void Add(
            TEntity entity);

        void Update(
            TEntity entity);

        int Delete(
            TEntity entity);

        TEntity GetSingle(
            Expression<Func<TEntity, bool>> predicate,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool asNoTracking = false);

        Task<PaginatedList<TEntity>> GetPagedListAsync(
            IPagingRequest pagingInfo,
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool asNoTracking = false);
      


        /// <inheritdoc />
        Task<PaginatedList<TEntity>> GetPagedListAsync(
            Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            int pageIndex = 0,
            int pageSize = 20,
            SortExpression sortExpressions = null,
            bool asNoTracking = false);
       
}
