using MovieDatabase.Core.Models;

namespace MovieDatabase.Core.Repository.IRepository
{
    public interface IGenreRepository : IBaseRepository<Genre>
    {
        Task<List<Genre>?> GetGenresByNamesAsync(IEnumerable<string> genreNames);
    }
}