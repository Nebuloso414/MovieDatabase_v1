using MovieDatabase.Core.Models;
using MovieDatabase.Core.Models.Dto;
using System.Linq.Expressions;

namespace MovieDatabase.Core.Repository.IRepository
{
    public interface IMovieRepository : IBaseRepository<Movie>
    {
        Task<Movie?> GetByTitleAsync(string title);
        Task<IEnumerable<MovieDto>> GetMoviesAsync(Expression<Func<Movie, bool>>? filter = null, bool includeCast = false);
        Task<bool> MovieExistsAsync(int id);
        Task<Movie?> GetByTitleAndReleaseDate(string title, DateOnly releaseDate);
    }
}