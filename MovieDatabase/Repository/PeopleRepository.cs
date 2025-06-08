using MovieDatabase.Data;
using MovieDatabase.Models;
using MovieDatabase.Repository.IRepository;
using System.Linq.Expressions;

namespace MovieDatabase.Repository
{
    public class PeopleRepository : BaseRepository<People>, IPeopleRepository
    {
        public PeopleRepository(ApplicationDbContext context) : base(context) { }
        
        public Task<IEnumerable<People>> GetAllWithDetailsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<People?>> GetByNameAsync(string name)
        {
            return await GetAllAsync(p => p.FirstName.ToLower().Contains(name.ToLower()) || p.LastName.ToLower().Contains(name.ToLower()));
        }
    }
}
