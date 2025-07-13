using AutoMapper;
using Microsoft.AspNetCore.Http;
using MovieDatabase.Core.Models;
using MovieDatabase.Core.Models.Dto;
using MovieDatabase.Core.Repository.IRepository;
using System.Linq.Expressions;

namespace MovieDatabase.Core.Services
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

        public async Task<People?> GetByIdAsync(int id, bool tracked = true, string? includeProperties = null)
        {
            return await _peopleRepository.GetByIdAsync(id, tracked, includeProperties);
        }

        public async Task<IEnumerable<People?>> GetByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Enumerable.Empty<People>();
            }

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
            var existingPerson = await _peopleRepository.GetByIdAsync(updatedPeople.Id, true);

            if (existingPerson == null)
            {
                throw new BadHttpRequestException($"Person with ID {updatedPeople.Id} not found.");
            }

            _mapper.Map(updatedPeople, existingPerson);

            await _peopleRepository.UpdateAsync(existingPerson);
            return existingPerson;
        }
    }
}
