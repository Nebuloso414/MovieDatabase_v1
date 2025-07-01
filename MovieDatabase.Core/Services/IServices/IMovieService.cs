using MovieDatabase.Core.Models;
using MovieDatabase.Core.Models.Dto;
using System.Linq.Expressions;

namespace MovieDatabase.Core.Services
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieDto>> GetMoviesAsync (Expression<Func<Movie, bool>>? filter = null, bool includeCast = false);
        Task<Movie> CreateAsync(MovieCreateDto movieDto);
        Task DeleteAsync(Movie movie);
        Task<Movie> UpdateAsync(MovieUpdateDto updatedMovie);
        Task<bool> MovieExistsAsync(string title);
    }
}
