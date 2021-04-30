using System;
using tesr.Data.Interfaces;

namespace tesr.Data
{
    public abstract class PagingRequest : IPagingRequest
    {
        public const int StandardPageSize = 20;
        public static readonly int[] StandardPageSizes = { 5, 10, StandardPageSize, 50, 100 };
        private int _PageIndex;
        private int _PageNo;

        public int PageNo
        {
            get => _PageNo;
            set
            {
                _PageNo = value;
                _PageIndex = value - 1;
            }
        }

        public int PageIndex
        {
            get => _PageIndex;
            set
            {
                _PageIndex = value;
                _PageNo = value + 1;
            }
        }

        public int PageSize { get; set; }

        public SortExpression SortExpression { get; set; }


        #region CTORs
        public PagingRequest()
        {
            PageNo = 1;
            PageSize = StandardPageSize;
        }


        public PagingRequest(
            int pageNo,
            int pageSize)
        {
            PageNo = pageNo;
            PageSize = pageSize;
           
        }
        #endregion

        #region  Helper methods 
        public int TotalCount { get; protected set; }

        /// <summary>
        /// Gets the total pages.
        /// </summary>
        /// <value>The total pages.</value>
        public int TotalPages { get; protected set; }

        /// <summary>
        /// Is this the first available page?
        /// </summary>
        public bool IsFirstPage => PageNo <= 1;

        /// <summary>
        /// Is this the last available page?
        /// </summary>
        public bool IsLastPage => PageNo >= TotalPages;


        /// <summary>
        /// Calculates the total number of pages available
        /// </summary>
        /// <param name="pageSize">The requested page size</param>
        /// <param name="totalCount">The total number of items available</param>
        /// <returns>The total number of pages</returns>
        
        #endregion
    }
}