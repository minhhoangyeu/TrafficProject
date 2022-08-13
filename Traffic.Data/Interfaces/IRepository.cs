using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Traffic.Data.Interfaces
{
    public interface IRepository<T, K> where T : class
    {
        Task<T> FindByIdAsync(K id, params Expression<Func<T, object>>[] includeProperties);

        T FindSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        IQueryable<T> FindAll(params Expression<Func<T, object>>[] includeProperties);

        IQueryable<T> FindAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);
    

        void Add(T entity);
        Task AddAsync(T entity);

        void AddRange(List<T> entities);
        Task AddRangeAsync(List<T> entities);

        void Update(T entity);

        void Remove(T entity);

        Task Remove(K id);


        void RemoveRange(List<T> entities);

        Task<bool> ExistAsync(Expression<Func<T, bool>> predicate);
    }
}
