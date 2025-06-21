using MovieDatabase.Models;
using MovieDatabase.Models.Dto;
using System.Linq.Expressions;

namespace MovieDatabase.Repository.IRepository
{
    public interface IMovieRepository : IBaseRepository<Movie>
    {
        Task<Movie?> GetByTitleAsync(string title);
        Task<IEnumerable<MovieDto>> GetMoviesWithProjectionAsync(bool includeCast = false);
    }
}