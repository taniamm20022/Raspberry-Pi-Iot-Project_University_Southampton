using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using tesr.Data.Interfaces;


namespace tesr.Data.Extensions
{
    public static class IQueryablePaginatedListExtensions
    {       
        public static async Task<PaginatedList<TEntity>> ToPaginatedListAsync<TEntity>(
            this IQueryable<TEntity> source,
            IPagingRequest pagingInfo)
        {
            source = source.OrderBy(x=>pagingInfo.SortExpression.Expression);
            return await source.ToPaginatedListAsync(
                pagingInfo.PageIndex, pagingInfo.PageSize,
                pagingInfo.SortExpression);
        }

      
        public static async Task<PaginatedList<TEntity>> ToPaginatedListAsync<TEntity>(
            this IQueryable<TEntity> source,
            int pageIndex,
            int pageSize,
            SortExpression sortExpressions = null)
        {
            var totalCount = await source.CountAsync().ConfigureAwait(false);

            // Check the request page index is in range
            var totalPages = CalculateTotalPages(pageSize, totalCount);
            if (pageIndex >= totalPages)
            {
                pageIndex = 0;
            }

            var items = await source
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync()
                .ConfigureAwait(false);

            return new PaginatedList<TEntity>(items, pageIndex, pageSize, totalCount, sortExpressions);
        }

        private static int CalculateTotalPages(int pageSize, int totalCount)
        {
            throw new NotImplementedException();
        }
    }
}
