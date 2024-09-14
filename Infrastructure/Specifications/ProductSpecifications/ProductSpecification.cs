using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications.ProductSpecifications
{
    public  class ProductSpecification
    {
        private const int MaxSize=20;

        public int? Category {  get; set; }
        public int? Brand { get; set; }
        public int? Type { get; set; }
        public string?Sort { get; set; }

        private string _Search;

        public string? Search  
        {
            get => _Search;
            set => _Search = value.Trim().ToLower();
        }

        public int PageIndex { get; set; } = 1;
        private int _PageSize=6;

        public int PageSize
        {
            get => _PageSize;
            set => _PageSize = (value > MaxSize) ? MaxSize : value;
        }



    }
}
