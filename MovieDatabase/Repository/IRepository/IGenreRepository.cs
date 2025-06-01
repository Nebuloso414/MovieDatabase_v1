using MovieDatabase.Models;

namespace MovieDatabase.Repository.IRepository
{
    public interface IGenreRepository : IBaseRepository<Genre>
    {
        Task<List<Genre>> GetGenresByNamesAsync(IEnumerable<string> genreNames);
    }
}