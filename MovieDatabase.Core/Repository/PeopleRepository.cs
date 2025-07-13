using Microsoft.EntityFrameworkCore;
using MovieDatabase.Core.Data;
using MovieDatabase.Core.Models;
using MovieDatabase.Core.Repository.IRepository;

namespace MovieDatabase.Core.Repository
{
    public class PeopleRepository : BaseRepository<People>, IPeopleRepository
    {
        public PeopleRepository(ApplicationDbContext context) : base(context) { }

        public Task<IEnumerable<People>> GetAllWithDetailsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<People>> GetByNameAsync(string name)
        {
            return await GetAllAsync(p => p.FirstName.ToLower().Contains(name.ToLower()) || p.LastName.ToLower().Contains(name.ToLower()));
        }

        public async Task<People?> CheckIfExistsByNameAndDob(People people)
        {
            var existingPerson = await _dbSet.FirstOrDefaultAsync(p => p.FirstName == people.FirstName && p.LastName == people.LastName && p.DateOfBirth == people.DateOfBirth);
            return existingPerson ?? null;
        }
    }
}
