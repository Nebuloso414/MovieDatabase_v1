using MovieDatabase.Core.Models;

namespace MovieDatabase.Core.Repository.IRepository
{
    public interface IPeopleRepository :IBaseRepository<People>
    {
        Task<IEnumerable<People>> GetAllWithDetailsAsync();
        Task<IEnumerable<People?>> GetByNameAsync(string name);
    }
}
