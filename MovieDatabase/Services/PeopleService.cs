using AutoMapper;
using MovieDatabase.Models;
using MovieDatabase.Models.Dto;
using MovieDatabase.Repository.IRepository;
using System.Linq.Expressions;

namespace MovieDatabase.Services
{
    public class PeopleService : IPeopleService
    {
        private readonly IPeopleRepository _peopleRepository;
        private readonly IMapper _mapper;

        public PeopleService(IPeopleRepository peopleRepository, IMapper mapper)
        {
            _peopleRepository = peopleRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<People>> GetAllAsync(Expression<Func<People, bool>>? filter = null, string? includeProperties = null)
        {
            return await _peopleRepository.GetAllAsync(filter, includeProperties);
        }

        public async Task<People?> GetByIdAsync(Expression<Func<People, bool>>? filter = null, bool tracked = true, string? includeProperties = null)
        {
            return await _peopleRepository.GetByIdAsync(filter, tracked, includeProperties);
        }

        public async Task<IEnumerable<People?>> GetByNameAsync(string name)
        {
            return await _peopleRepository.GetByNameAsync(name);
        }
        public async Task<People> CreateAsync(PeopleCreateDto peopleDto)
        {
            var people = _mapper.Map<People>(peopleDto);
            await _peopleRepository.CreateAsync(people);
            return people;
        }

        public async Task DeleteAsync(People people)
        {
            await _peopleRepository.RemoveAsync(people);
        }

        public async Task<People> UpdateAsync(PeopleUpdateDto updatedPeople)
        {
            var people = _mapper.Map<People>(updatedPeople);
            await _peopleRepository.UpdateAsync(people);
            return people;
        }
    }
}
