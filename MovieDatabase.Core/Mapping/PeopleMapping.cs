using MovieDatabase.Core.Models;
using MovieDatabase.Core.Models.Dto;

namespace MovieDatabase.Core.Mapping
{
    public static class PeopleMapping
    {
        public static PeopleDto MapToResponse(this People people)
        {
            return new PeopleDto
            {
                Id = people.Id,
                FirstName = people.FirstName,
                LastName = people.LastName,
                DateOfBirth = people.DateOfBirth
            };
        }
        public static People MapToPeople(this PeopleCreateDto peopleCreateDto)
        {
            return new People
            {
                FirstName = peopleCreateDto.FirstName,
                LastName = peopleCreateDto.LastName,
                DateOfBirth = peopleCreateDto.DateOfBirth
            };
        }

        public static People MapToPeople(this PeopleUpdateDto peopleUpdateDto, People existingPerson)
        {
            existingPerson.FirstName = peopleUpdateDto.FirstName;
            existingPerson.LastName = peopleUpdateDto.LastName;
            existingPerson.DateOfBirth = peopleUpdateDto.DateOfBirth;
            return existingPerson;
        }
    }
}
