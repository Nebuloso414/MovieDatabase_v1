using Microsoft.EntityFrameworkCore;
using MovieDatabase.Data;
using MovieDatabase.Models;
using MovieDatabase.Repository.IRepository;

namespace MovieDatabase.Repository
{
    public class GenreRepository : BaseRepository<Genre>, IGenreRepository
    {
        public GenreRepository(ApplicationDbContext context) : base(context) { }

        public async Task<List<Genre>> GetGenresByNamesAsync(IEnumerable<string> genreNames)
        {
            if (genreNames == null || !genreNames.Any())
                return new List<Genre>();

            var normalizedNames = genreNames.Select(g => g.ToLower());

            return await _dbSet
                .Where(g => normalizedNames.Contains(g.Name.ToLower()))
                .ToListAsync();
        }
    }
}