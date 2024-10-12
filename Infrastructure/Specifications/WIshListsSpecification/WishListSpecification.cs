using Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications.WIshListsSpecification
{
    public  class WishListSpecification
    {
		private const int MaxSize = 10;
		public int PageIndex { get; set; } = 1;

		private int _PageSize;

		public int PageSize
		{
			get => _PageSize;
			set => _PageSize = (value> MaxSize) ? MaxSize : value;
		}

	}
}
