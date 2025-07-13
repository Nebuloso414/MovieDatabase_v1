using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MovieDatabase.Core.Repository.IRepository
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        Task<T?> GetByIdAsync(int id, bool tracked = true, string? includeProperties = null);
        Task<bool> CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task<bool> RemoveAsync(T entity);
        Task SaveAsync();
    }
}