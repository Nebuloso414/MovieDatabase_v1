using Microsoft.EntityFrameworkCore;
using MovieDatabase.Data;
using MovieDatabase.Models;
using MovieDatabase.Repository.IRepository;

namespace MovieDatabase.Repository
{
    public class MovieRepository : BaseRepository<Movie>, IMovieRepository
    {
        public MovieRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Movie>> GetAllWithGenresAsync()
        {
            return await _dbSet.Include(m => m.Genres).ToListAsync();
        }

        public async Task<Movie?> GetByIdWithGenresAsync(int id)
        {
            return await _dbSet.Include(m => m.Genres)
                               .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Movie?> GetByTitle(string title)
        {
            return await _dbSet.FirstOrDefaultAsync(m => m.Title.ToLower() == title.ToLower());
        }
    }
}