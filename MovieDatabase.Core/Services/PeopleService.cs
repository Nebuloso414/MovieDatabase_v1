using FluentValidation;
using MovieDatabase.Core.Mapping;
using MovieDatabase.Core.Models;
using MovieDatabase.Core.Models.Dto;
using MovieDatabase.Core.Repository.IRepository;
using System.Linq.Expressions;

namespace MovieDatabase.Core.Services
{
    public class PeopleService : IPeopleService
    {
        private readonly IPeopleRepository _peopleRepository;
        private readonly IValidator<People> _peopleValidator;

        public PeopleService(IPeopleRepository peopleRepository, IValidator<People> peopleValidator)
        {
            _peopleRepository = peopleRepository;
            _peopleValidator = peopleValidator;
        }

        public async Task<IEnumerable<PeopleDto>> GetAllAsync(Expression<Func<People, bool>>? filter = null, string? includeProperties = null)
        {
            var peopleList = await _peopleRepository.GetAllAsync(filter, includeProperties);

            return peopleList.Select(p => p.MapToResponse());
        }

        public async Task<PeopleDto?> GetByIdAsync(int id, bool tracked = true, string? includeProperties = null)
        {
            var people = await _peopleRepository.GetByIdAsync(id, tracked, includeProperties);

            if (people == null)
                return null;

            return people.MapToResponse();
        }

        public async Task<IEnumerable<PeopleDto>> GetByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Enumerable.Empty<PeopleDto>();
            }

            var peopleList = await _peopleRepository.GetByNameAsync(name);
            return peopleList.Select(p => p.MapToResponse());
        }
        public async Task<PeopleDto> CreateAsync(PeopleCreateDto peopleDto)
        {
            var people = peopleDto.MapToPeople();
            await _peopleValidator.ValidateAndThrowAsync(people);

            await _peopleRepository.CreateAsync(people);
            return people.MapToResponse();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var people = await _peopleRepository.GetByIdAsync(id, tracked: true);

            if (people is null)
                return false;

            return await _peopleRepository.RemoveAsync(people);
        }

        public async Task<PeopleDto?> UpdateAsync(PeopleUpdateDto updatedPeople)
        {
            var existingPerson = await _peopleRepository.GetByIdAsync(updatedPeople.Id, true);

            if (existingPerson == null)
                return null;

            existingPerson = updatedPeople.MapToPeople(existingPerson);
            await _peopleValidator.ValidateAndThrowAsync(existingPerson);

            await _peopleRepository.UpdateAsync(existingPerson);
            return existingPerson.MapToResponse();
        }
    }
}
