using Core.Data;
using Core.Entity;
using Infrastructure.Interfaces;
using Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly AppDBContext context;

        public GenericRepository(AppDBContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<T>> GetAll()
            =>await context.Set<T>().ToListAsync();

        public async Task<T> GetById(int id)
            => await context.Set<T>().FirstOrDefaultAsync(c=>c.Id==id);
        public async Task AddAsync(T entity)
            =>await context.Set<T>().AddAsync(entity);
        public void Update(T entity)
            =>context.Set<T>().Update(entity);


        public void Delete(T entity)
            => context.Set<T>().Remove(entity);

        public async Task<T> FindAsync(Expression<Func<T, bool>> match)
            => await context.Set<T>().FirstOrDefaultAsync(match);
        public async Task<IEnumerable<T>> GetAllEntityWithSpecs(ISpecification<T> specification)
            =>await Apply(specification).ToListAsync();

        public async Task<T> GetEntityWithSpecs(ISpecification<T> specification)
            => await Apply(specification).FirstOrDefaultAsync();

        public async Task<int> GetCount(ISpecification<T> specification)
            =>await Apply(specification).CountAsync();
        private  IQueryable<T> Apply(ISpecification<T> specification)
            => EvaluationSpecification<T>.GetQuery(context.Set<T>().AsQueryable(),specification);

        public async Task<bool> IsValid(int id)
            =>await context.Set<T>().AnyAsync(c=>c.Id==id);
    }
}
