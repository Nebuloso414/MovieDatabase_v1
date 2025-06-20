using Microsoft.EntityFrameworkCore;
using MovieDatabase.Data;
using MovieDatabase.Models;
using MovieDatabase.Repository.IRepository;

namespace MovieDatabase.Repository
{
    public class MovieRepository : BaseRepository<Movie>, IMovieRepository
    {
        public MovieRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Movie?> GetByTitleAsync(string title)
        {
            return await _dbSet.FirstOrDefaultAsync(m => m.Title.ToLower() == title.ToLower());
        }
    }
}