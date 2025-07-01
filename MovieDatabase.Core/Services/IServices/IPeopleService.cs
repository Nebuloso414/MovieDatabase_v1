using MovieDatabase.Core.Models;
using MovieDatabase.Core.Models.Dto;
using System.Linq.Expressions;

namespace MovieDatabase.Core.Services
{
    public interface IPeopleService
    {
        Task<IEnumerable<People>> GetAllAsync(Expression<Func<People, bool>>? filter = null, string? includeProperties = null);
        Task<People?> GetByIdAsync(Expression<Func<People, bool>>? filter = null, bool tracked = true, string? includeProperties = null);
        Task<IEnumerable<People?>> GetByNameAsync(string name);
        Task<People> CreateAsync(PeopleCreateDto peopleDto);
        Task DeleteAsync(People people);
        Task<People> UpdateAsync(PeopleUpdateDto updatedPeople);
    }
}
