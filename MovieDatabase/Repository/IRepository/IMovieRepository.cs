using MovieDatabase.Models;
using System.Linq.Expressions;

namespace MovieDatabase.Repository.IRepository
{
    public interface IMovieRepository : IBaseRepository<Movie>
    {
        Task<Movie?> GetByTitleAsync(string title);
    }
}