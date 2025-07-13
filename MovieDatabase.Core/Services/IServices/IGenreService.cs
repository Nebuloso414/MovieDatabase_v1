using MovieDatabase.Core.Models;
using MovieDatabase.Core.Models.Dto;
using System.Linq.Expressions;

namespace MovieDatabase.Core.Services
{
    public interface IGenreService
    {
        Task<IEnumerable<GenreDto>> GetAllAsync(Expression<Func<Genre, bool>>? filter = null, string? includeProperties = null);
        Task<GenreDto?> GetByIdAsync(int id, bool tracked = true, string? includeProperties = null);
        Task<GenreDto> CreateAsync(GenreCreateDto newGenre);
        Task<bool> DeleteAsync(int id);
        Task<GenreDto?> UpdateAsync(GenreUpdateDto updatedGenre);
        Task<List<Genre>?> GetGenresByNamesAsync(List<string> genreNames);
    }
}
