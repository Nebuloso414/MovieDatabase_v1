using MovieDatabase.Core.Models;
using MovieDatabase.Core.Repository.IRepository;
using System.Linq.Expressions;

namespace MovieDatabase.Core.Services
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;

        public GenreService(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<IEnumerable<Genre>> GetAllAsync(Expression<Func<Genre, bool>>? filter = null, string? includeProperties = null)
        {
            return await _genreRepository.GetAllAsync(filter, includeProperties);
        }

        public async Task<Genre?> GetByIdAsync(Expression<Func<Genre, bool>>? filter = null, bool tracked = true, string? includeProperties = null)
        {
            return await _genreRepository.GetByIdAsync(filter, tracked, includeProperties);
        }
        public async Task<Genre> CreateAsync(Genre newGenre)
        {
            await _genreRepository.CreateAsync(newGenre);
            return newGenre;
        }

        public async Task DeleteAsync(Genre genre)
        {
            await _genreRepository.RemoveAsync(genre);
            return;
        }

        public async Task<Genre> UpdateAsync(Genre updatedGenre)
        {
            await _genreRepository.UpdateAsync(updatedGenre);
            return updatedGenre;
        }

        public async Task<(List<Genre> FoundGenres, List<string> NotFoundGenres)> ProcessGenreNamesAsync(List<string> genreNames)
        {
            if (genreNames == null || !genreNames.Any())
            {
                return (new List<Genre>(), new List<string>());
            }

            // Get genres from the repository
            var foundGenres = await _genreRepository.GetGenresByNamesAsync(genreNames);
            
            // Find genre names that were not found in the database
            var foundGenreNames = foundGenres.Select(g => g.Name).ToList();
            var notFoundGenres = genreNames
                .Where(name => !foundGenreNames.Any(foundName => 
                    foundName.Equals(name, StringComparison.OrdinalIgnoreCase)))
                .ToList();
            
            return (foundGenres, notFoundGenres);
        }
    }
}
