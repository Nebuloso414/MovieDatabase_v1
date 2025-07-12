using Azure.Core;
using MovieDatabase.Core.Models;
using MovieDatabase.Core.Models.Dto;

namespace MovieDatabase.Core.Mapping
{
    public static class MovieMappings
    {
        public static Movie MapToMovie(this MovieCreateDto request)
        {
            return new Movie
            {
                Title = request.Title,
                ReleaseDate = request.ReleaseDate,
                Length = request.Length,
                Rating = request.Rating
            };
        }
        public static Movie MapToMovie(this MovieUpdateDto request, Movie existingMovie)
        {
            existingMovie.Title = request.Title;
            existingMovie.ReleaseDate = request.ReleaseDate;
            existingMovie.Length = request.Length;
            existingMovie.Rating = request.Rating;
            return existingMovie;
        }

        public static MovieDto MapToMovieResponse(this Movie movie)
        {
            return new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                ReleaseDate = movie.ReleaseDate,
                Length = movie.Length,
                Rating = movie.Rating,
                Genres = movie.Genres.Select(g => g.Name).ToList()
            };
        }
    }
}
