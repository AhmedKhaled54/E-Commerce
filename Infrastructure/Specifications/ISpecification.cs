using Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public  interface ISpecification<T>where T : BaseEntity 
    {
        Expression<Func<T,bool>>Creiteria {  get; }
        List<Expression<Func<T,object>>> Includes{ get; }
        Expression<Func<T,object>>OrderBy { get; }
        Expression<Func<T,object>>OrderByDescinding { get; }

        int Skip {  get; }
        int Take { get; }   
        bool IsPaginated { get; }   

    }
}
