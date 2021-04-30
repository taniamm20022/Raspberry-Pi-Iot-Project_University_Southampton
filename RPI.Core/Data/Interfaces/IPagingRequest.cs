using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tesr.Data.Interfaces
{
    public interface IPagingRequest
    {
        int PageNo{ get; set; }
        int PageIndex { get; set; }
        int PageSize { get; set; }

       
      SortExpression SortExpression { get; set; }
    }
}
