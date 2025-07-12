using MovieDatabase.Core.Models;
using MovieDatabase.Core.Models.Dto;
using System.Linq.Expressions;

namespace MovieDatabase.Core.Services
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieDto>> GetAllAsync (Expression<Func<Movie, bool>>? filter = null, bool includeCast = false);
        Task<MovieDto?> GetByIdAsync(int id, bool includeCast = false);
        Task<MovieDto> CreateAsync(MovieCreateDto movie);
        Task<bool> DeleteAsync(int id);
        Task<MovieDto?> UpdateAsync(MovieUpdateDto movie);
    }
}
