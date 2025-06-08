using MovieDatabase.Models;

namespace MovieDatabase.Repository.IRepository
{
    public interface IPeopleRepository :IBaseRepository<People>
    {
        Task<IEnumerable<People>> GetAllWithDetailsAsync();
        Task<IEnumerable<People?>> GetByNameAsync(string name);
    }
}
