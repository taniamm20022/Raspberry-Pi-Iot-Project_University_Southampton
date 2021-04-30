using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using tesr.Data.Enums;

namespace tesr.Data
{
    public class SortExpression
    {
        public string Title { get; set; }
        public string Expression { get; set; }
        public SortDirection SortDirection { get; set; }
    }
}
