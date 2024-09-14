using Core.Data;
using Core.Entity;
using Infrastructure.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UintOfWork : IUnitOfWork
    {
        private readonly AppDBContext context;
        private Hashtable _hashtable;
        public UintOfWork(AppDBContext context)
        {
            this.context = context;
        }
        public async Task<int> Complete()
            =>await context.SaveChangesAsync();

        public void Dispose()
            =>context.Dispose();

        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            if (_hashtable == null)
                _hashtable = new Hashtable();
            var Key=typeof(T).Name;
            
            if (!_hashtable.ContainsKey(Key))
            {
                var Type = typeof(GenericRepository<>);
                var instanse = Activator.CreateInstance(Type.MakeGenericType(typeof(T)),context);
                _hashtable.Add(Key, instanse);
            }
            return (IGenericRepository<T>)_hashtable[Key];
        }
    }
}
