using Core.Entity;
using Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public  interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById (int id);
        Task AddAsync (T entity);
        void  Update(T entity);
        void Delete(T entity );

        Task<T>FindAsync(Expression<Func<T, bool>> match);
        Task<IEnumerable<T>>GetAllEntityWithSpecs(ISpecification<T> specification);

        Task<IEnumerable<T>>GetAllPredicated(Expression<Func<T, bool>> match, string[]include=null!);
        T GetEntityPredicated(Expression<Func<T, bool>> match, string[] include = null!);
        Task<T>GetEntityWithSpecs(ISpecification<T> specification);

        Task<int> GetCount(ISpecification<T> specification);
        Task<bool>IsValid(int id);

    }
}
