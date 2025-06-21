using MovieDatabase.Models;
using MovieDatabase.Models.Dto;
using System.Linq.Expressions;

namespace MovieDatabase.Services
{
    public interface IMovieService
    {
        Task<IEnumerable<Movie>> GetAllAsync(Expression<Func<Movie, bool>>? filter = null, string? includeProperties = null);
        Task<Movie?> GetByIdAsync(Expression<Func<Movie, bool>>? filter = null, bool tracked = true, string? includeProperties = null);
        Task<Movie> CreateAsync(MovieCreateDto movieDto);
        Task DeleteAsync(Movie movie);
        Task<Movie> UpdateAsync(MovieUpdateDto updatedMovie);
        Task<IEnumerable<MovieDto>> GetMoviesOptimizedAsync(bool includeCast = false);
    }
}
