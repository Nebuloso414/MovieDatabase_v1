using FluentValidation;
using MovieDatabase.Core.Models;
using MovieDatabase.Core.Repository.IRepository;

namespace MovieDatabase.Core.Validators
{
    public class GenreValidator : AbstractValidator<Genre>
    {
        private readonly IGenreRepository _genreRepository;
        public GenreValidator(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;

            RuleFor(g => g.Name)
                .NotEmpty().WithMessage("Genre name is required.")
                .MaximumLength(255).WithMessage("Genre name cannot exceed 255 characters.")
                .MustAsync(BeUniqueNameAsync).WithMessage("Genre already exists.");

            RuleFor(g => g.Description)
                .MaximumLength(255).WithMessage("Genre description cannot exceed 500 characters.");
        }

        public async Task<bool> BeUniqueNameAsync(Genre genre, string name, CancellationToken token)
        {
            var existingGenre = await _genreRepository.GetByNameAsync(name);
            return existingGenre == null || existingGenre.Id == genre.Id;
        }
    }
}
