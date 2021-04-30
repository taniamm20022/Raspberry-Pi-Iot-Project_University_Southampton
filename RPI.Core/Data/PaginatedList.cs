using System;
using System.Collections.Generic;
using System.Linq;


namespace tesr.Data
{
    public class PaginatedList<T> : PagingRequest, IPaginatedList<T>
    {
        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>The items.</value>
        public List<T> Items { get; }



        #region CTORs
    
        public PaginatedList(
            IEnumerable<T> items,
            int pageIndex,
            int pageSize,
            int totalCount,
            SortExpression sortExpressions)
        {
            Items = items.ToList();
            SortExpression = sortExpressions;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = CalculateTotalPages(PageSize, TotalCount);
        }

        private  int CalculateTotalPages(
            int pageSize,
            int totalCount)
        {
            return (int)Math.Ceiling(totalCount / (double)pageSize);
        }
        #endregion CTORs
    }
}

