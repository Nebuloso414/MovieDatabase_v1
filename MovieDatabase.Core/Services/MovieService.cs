using Microsoft.AspNetCore.Http;
using MovieDatabase.Core.Models;
using MovieDatabase.Core.Models.Dto;
using MovieDatabase.Core.Repository.IRepository;
using System.Linq.Expressions;
using System.Net;

namespace MovieDatabase.Core.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IGenreRepository _genreRepository;

        public MovieService(IMovieRepository movieRepository, IGenreRepository genreRepository)
        {
            _movieRepository = movieRepository;
            _genreRepository = genreRepository;
        }

        public async Task<IEnumerable<MovieDto>> GetMoviesAsync(Expression<Func<Movie, bool>>? filter = null, bool includeCast = false)
        {
            return await _movieRepository.GetMoviesAsync(filter, includeCast);
        }

        public async Task<bool> CreateAsync(Movie movie)
        {
            if (movie.Genres != null && movie.Genres.Any())
            {
                var foundGenres = await _genreRepository.GetGenresByNamesAsync(movie.Genres.Select(g => g.Name));

                if (foundGenres.Count != movie.Genres.Count)
                {
                    var missingGenres = movie.Genres
                        .Where(g => !foundGenres.Any(fg => fg.Name.Equals(g.Name, StringComparison.OrdinalIgnoreCase)))
                        .ToList();

                    throw new BadHttpRequestException($"The following genres do not exist: {string.Join(", ", missingGenres)}");
                }

                movie.Genres = foundGenres;
            }

            return await _movieRepository.CreateAsync(movie);
        }

        public async Task<bool> DeleteAsync(Movie movie)
        {
            return await _movieRepository.RemoveAsync(movie);
        }

        public async Task<Movie> UpdateAsync(Movie movie)
        {
            var existingMovie = await _movieRepository.GetByIdAsync(m => m.Id == movie.Id, tracked: true, includeProperties: "Genres");
            
            if (existingMovie == null)
            {
                throw new BadHttpRequestException($"Movie with ID {movie.Id} not found.");
            }

            existingMovie.Title = movie.Title;
            existingMovie.ReleaseDate = movie.ReleaseDate;
            existingMovie.Length = movie.Length;
            existingMovie.Rating = movie.Rating;
            
            existingMovie.Genres.Clear();
            foreach (var genre in movie.Genres)
            {
                existingMovie.Genres.Add(genre);
            }
            
            await _movieRepository.UpdateAsync(existingMovie);
            return existingMovie;
        }

        public async Task<bool> MovieExistsAsync(string title)
        {
            return await _movieRepository.MovieExistsAsync(title);
        }
    }
}
