using MovieDatabase.Models;

namespace MovieDatabase.Repository.IRepository
{
    public interface IMovieRepository : IBaseRepository<Movie>
    {
        // Add Movie-specific methods here if needed
        Task<IEnumerable<Movie>> GetAllWithGenresAsync();
        Task<Movie?> GetByIdWithGenresAsync(int id);
        Task<Movie?> GetByTitleAsync(string title);
    }
}