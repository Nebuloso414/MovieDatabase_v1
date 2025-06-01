using Microsoft.EntityFrameworkCore;
using MovieDatabase.Data;
using System.Linq.Expressions;
using MovieDatabase.Repository.IRepository;

namespace MovieDatabase.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = _dbSet;

            if(filter != null)
            {
                query = query.Where(filter);
            }

            if(!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true, string? includeProperties = null)
        {
            IQueryable<T> query = _dbSet;

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public virtual async Task CreateAsync(T entity)
        {
            var now = DateTime.UtcNow;
            var createdDateProp = typeof(T).GetProperty("CreatedDate");
            var updatedDateProp = typeof(T).GetProperty("UpdatedDate");

            if (createdDateProp != null && createdDateProp.CanWrite)
                createdDateProp.SetValue(entity, now);

            if (updatedDateProp != null && updatedDateProp.CanWrite)
                updatedDateProp.SetValue(entity, now);

            _dbSet.Add(entity);
            await SaveAsync();
        }

        public virtual async Task UpdateAsync(T entity)
        {
            var now = DateTime.UtcNow;
            var updatedDateProp = typeof(T).GetProperty("UpdatedDate");

            if (updatedDateProp != null && updatedDateProp.CanWrite)
                updatedDateProp.SetValue(entity, now);

            _dbSet.Update(entity);
            await SaveAsync();
        }

        public virtual async Task RemoveAsync(T entity)
        {
            _dbSet.Remove(entity);
            await SaveAsync();
        }

        public virtual async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}