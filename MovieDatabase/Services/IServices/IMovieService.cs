using MovieDatabase.Models;
using MovieDatabase.Models.Dto;
using System.Linq.Expressions;

namespace MovieDatabase.Services
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
