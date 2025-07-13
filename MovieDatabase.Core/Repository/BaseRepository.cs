using Microsoft.EntityFrameworkCore;
using MovieDatabase.Core.Data;
using System.Linq.Expressions;
using MovieDatabase.Core.Repository.IRepository;

namespace MovieDatabase.Core.Repository
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

        public virtual async Task<T?> GetByIdAsync(int id, bool tracked = true, string? includeProperties = null)
        {
            IQueryable<T> query = _dbSet;

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            query = query.Where(e => EF.Property<int>(e, "Id") == id);

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public virtual async Task<bool> CreateAsync(T entity)
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
            return true;
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

        public virtual async Task<bool> RemoveAsync(T entity)
        {
            _dbSet.Remove(entity);
            await SaveAsync();
            return true;
        }

        public virtual async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}