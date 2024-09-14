using Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
    {
        public BaseSpecification(Expression<Func<T,bool>> creiteria)
        {
            Creiteria = creiteria;
            
        }
        public Expression<Func<T, bool>> Creiteria { get; }

        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();

        public Expression<Func<T, object>> OrderBy { get; private set; }

        public Expression<Func<T, object>> OrderByDescinding {  get;private set; }

        public int Skip {  get; private set; }

        public int Take {  get; private set; }

        public bool IsPaginated {  get; private set; }

        protected void AddInclude(Expression<Func<T, object>> include)
            =>Includes.Add(include);
        protected void AddOrderBy(Expression<Func<T, object>> orderby)
            =>OrderBy=orderby;
        protected void AddOrderByDesc(Expression<Func<T, object>> orderbyDesc)
            =>OrderByDescinding=orderbyDesc;

        protected void AddPagination(int skip ,int take )
        {
            Skip = skip;
            Take = take;
            IsPaginated = true;
        }
    }
}
