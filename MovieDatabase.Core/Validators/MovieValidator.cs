using FluentValidation;
using MovieDatabase.Core.Models;
using MovieDatabase.Core.Repository.IRepository;

namespace MovieDatabase.Core.Validators
{    
    public class MovieValidator : AbstractValidator<Movie>
    {
        private readonly IMovieRepository _movieRepository;

        public MovieValidator(IMovieRepository movieRepository)
        { 
            _movieRepository = movieRepository;

            RuleFor(m => m.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title cannot exceed 50 characters.")
                .MustAsync(ValidateMovieExistence).WithMessage("Movie already exists.");

            RuleFor(m => m.ReleaseDate)
                .NotEmpty().WithMessage("Release date is required.")
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow)).WithMessage("Release date cannot be in the future.");

            RuleFor(m => m.Length)
                .GreaterThan(0).WithMessage("Length must be a positive number.");

            RuleFor(m => m.Rating)
                .InclusiveBetween(0, 10).WithMessage("Rating must be between 0 and 10.");
        }

        private async Task<bool> ValidateMovieExistence(Movie movie, string title, CancellationToken token)
        {
            var existingMovie = await _movieRepository.GetByIdAsync(m => m.Title == title, false);
            
            return existingMovie == null || existingMovie.ReleaseDate != movie.ReleaseDate;
        }
    }
}
