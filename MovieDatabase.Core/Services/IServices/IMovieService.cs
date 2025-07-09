using MovieDatabase.Core.Models;
using MovieDatabase.Core.Models.Dto;
using System.Linq.Expressions;

namespace MovieDatabase.Core.Services
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieDto>> GetMoviesAsync (Expression<Func<Movie, bool>>? filter = null, bool includeCast = false);
        Task<bool> CreateAsync(Movie movie);
        Task<bool> DeleteAsync(Movie movie);
        Task<Movie> UpdateAsync(Movie movie);
        Task<bool> MovieExistsAsync(string title);
    }
}
