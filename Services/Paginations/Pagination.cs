using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Paginations
{
    public  class Pagination<T>where T : class
    {
        public Pagination(int pageSize, int pageIndex, int count, List<T> items)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
            Count = count;
            Items = items;
        }

        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int Count { get; set; }
        public List<T>Items { get; set; }    
    }
}
