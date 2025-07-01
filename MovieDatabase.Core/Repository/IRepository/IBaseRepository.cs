using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MovieDatabase.Core.Repository.IRepository
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        Task<T?> GetByIdAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true, string? includeProperties = null);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task RemoveAsync(T entity);
        Task SaveAsync();
    }
}