using Microsoft.EntityFrameworkCore;
using MovieDatabase.Core.Data;
using MovieDatabase.Core.Models;
using MovieDatabase.Core.Repository.IRepository;

namespace MovieDatabase.Core.Repository
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