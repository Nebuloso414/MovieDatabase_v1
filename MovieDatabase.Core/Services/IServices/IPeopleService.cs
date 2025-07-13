using MovieDatabase.Core.Models;
using MovieDatabase.Core.Models.Dto;
using System.Linq.Expressions;

namespace MovieDatabase.Core.Services
{
    public interface IPeopleService
    {
        Task<IEnumerable<PeopleDto>> GetAllAsync(Expression<Func<People, bool>>? filter = null, string? includeProperties = null);
        Task<PeopleDto?> GetByIdAsync(int id, bool tracked = true, string? includeProperties = null);
        Task<IEnumerable<PeopleDto>> GetByNameAsync(string name);
        Task<PeopleDto> CreateAsync(PeopleCreateDto peopleDto);
        Task<bool> DeleteAsync(int id);
        Task<PeopleDto?> UpdateAsync(PeopleUpdateDto updatedPeople);
    }
}
