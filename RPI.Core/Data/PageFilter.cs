
namespace tesr.Data
{
    public abstract class PageFilter : IPageFilter
    {
        protected PageFilter();
        protected PageFilter(int pageNo, int pageSize, SortExpression sortExpressions = null);

        public PagingRequest PagingRequest { get; set; }
    }
}
