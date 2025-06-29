using MovieDatabase.Models;
using MovieDatabase.Models.Dto;
using System.Linq.Expressions;

namespace MovieDatabase.Repository.IRepository
{
    public interface IMovieRepository : IBaseRepository<Movie>
    {
        Task<Movie?> GetByTitleAsync(string title);
        Task<IEnumerable<MovieDto>> GetMoviesAsync(Expression<Func<Movie, bool>>? filter = null, bool includeCast = false);
        Task<bool> MovieExistsAsync(string title);
    }
}