using FluentValidation;
using MovieDatabase.Core.Models;

namespace MovieDatabase.Core.Validators
{
    public class MovieValidator : AbstractValidator<Movie>
    {
        public MovieValidator() 
        { 
            RuleFor(m => m.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title cannot exceed 50 characters.");

            RuleFor(m => m.ReleaseDate)
                .NotEmpty().WithMessage("Release date is required.")
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow)).WithMessage("Release date cannot be in the future.");

            RuleFor(m => m.Length)
                .GreaterThan(0).WithMessage("Length must be a positive number.");

            RuleFor(m => m.Rating)
                .InclusiveBetween(0, 10).WithMessage("Rating must be between 0 and 10.");
        }
    }
}
