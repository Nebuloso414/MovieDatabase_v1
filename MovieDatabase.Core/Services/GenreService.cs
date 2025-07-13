using FluentValidation;
using MovieDatabase.Core.Mapping;
using MovieDatabase.Core.Models;
using MovieDatabase.Core.Models.Dto;
using MovieDatabase.Core.Repository.IRepository;
using System.Linq.Expressions;

namespace MovieDatabase.Core.Services
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IValidator<Genre> _genreValidator;

        public GenreService(IGenreRepository genreRepository, IValidator<Genre> genreValidator)
        {
            _genreRepository = genreRepository;
            _genreValidator = genreValidator;
        }

        public async Task<IEnumerable<GenreDto>> GetAllAsync(Expression<Func<Genre, bool>>? filter = null, string? includeProperties = null)
        {
            var genres = await _genreRepository.GetAllAsync(filter, includeProperties);
            return genres.Select(g => g.MapToGenreResponse());
        }

        public async Task<GenreDto?> GetByIdAsync(int id, bool tracked = true, string? includeProperties = null)
        {
            var genre = await _genreRepository.GetByIdAsync(id, tracked, includeProperties);
            
            if (genre == null)
                return null;

            return genre.MapToGenreResponse();
        }
        public async Task<GenreDto> CreateAsync(GenreCreateDto newGenre)
        {
            var genre = newGenre.MapToGenre();
            await _genreValidator.ValidateAndThrowAsync(genre);
            await _genreRepository.CreateAsync(genre);
            return genre.MapToGenreResponse();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var genre = await _genreRepository.GetByIdAsync(id, tracked: true);
            
            if (genre == null)
                return false;

            return await _genreRepository.RemoveAsync(genre);
        }

        public async Task<GenreDto?> UpdateAsync(GenreUpdateDto updatedGenre)
        {
            var existingGenre = await _genreRepository.GetByIdAsync(updatedGenre.Id, tracked: true);

            if (existingGenre == null)
                return null;

            existingGenre = updatedGenre.MapToGenre(existingGenre);
            await _genreValidator.ValidateAndThrowAsync(existingGenre);
            await _genreRepository.UpdateAsync(existingGenre);

            return existingGenre.MapToGenreResponse();
        }

        public async Task<List<Genre>?> GetGenresByNamesAsync(List<string> genreNames)
        {
            if (genreNames == null || !genreNames.Any())
            {
                return null;
            }
            return await _genreRepository.GetGenresByNamesAsync(genreNames);
        }
    }
}
