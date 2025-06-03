using AutoMapper;
using MovieDatabase.Models;
using MovieDatabase.Repository.IRepository;
using System.Linq.Expressions;

namespace MovieDatabase.Services
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
    }
}
