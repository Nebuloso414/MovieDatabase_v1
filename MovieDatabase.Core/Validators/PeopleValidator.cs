using FluentValidation;
using MovieDatabase.Core.Models;
using MovieDatabase.Core.Repository.IRepository;

namespace MovieDatabase.Core.Validators
{
    public class PeopleValidator : AbstractValidator<People>
    {
        private readonly IPeopleRepository _peopleRepository;
        public PeopleValidator(IPeopleRepository peopleRepository)
        {
            _peopleRepository = peopleRepository;

            RuleFor(p => p.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .Length(2, 100).WithMessage("First name must be between 2 and 100 characters.");

            RuleFor(p => p.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .Length(2, 100).WithMessage("Last name must be between 2 and 100 characters.");

            RuleFor(p => p.DateOfBirth)
                .LessThan(DateTime.UtcNow).WithMessage("Date of birth must be in the past.");

            RuleFor(p => p)
                .MustAsync(PersonExistsAsync)
                .WithMessage("A person with the same name and date of birth already exists.");
        }

        private async Task<bool> PersonExistsAsync(People person, CancellationToken token)
        {
            var existing = await _peopleRepository.CheckIfExistsByNameAndDob(person);
            return existing == null || person.Id == existing.Id;
        }
    }
}
