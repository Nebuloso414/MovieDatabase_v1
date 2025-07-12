using FluentValidation;
using Microsoft.AspNetCore.Http;
using MovieDatabase.Core.Mapping;
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
        private readonly IValidator<Movie> _movieValidator;
        private readonly IGenreService _genreService;

        public MovieService(IMovieRepository movieRepository, IValidator<Movie> movieValidator, IGenreService genreService)
        {
            _movieRepository = movieRepository;
            _movieValidator = movieValidator;
            _genreService = genreService;
        }

        public async Task<IEnumerable<MovieDto>> GetAllAsync(Expression<Func<Movie, bool>>? filter = null, bool includeCast = false)
        {
            return await _movieRepository.GetMoviesAsync(filter, includeCast);
        }

        public async Task<MovieDto?> GetByIdAsync(int id, bool includeCast = false)
        {
            var movies = await _movieRepository.GetMoviesAsync(m => m.Id == id, includeCast);
            return movies.SingleOrDefault();
        }

        public async Task<MovieDto> CreateAsync(MovieCreateDto newMovie)
        {
            var movie = newMovie.MapToMovie();
            await _movieValidator.ValidateAndThrowAsync(movie);

            var genres = await _genreService.GetGenresByNamesAsync(newMovie.Genres);

            if (genres != null)
                movie.Genres = genres;

            await _movieRepository.CreateAsync(movie);
            return movie.MapToMovieResponse();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var movie = await _movieRepository.GetByIdAsync(m => m.Id == id, tracked: true);

            if (movie is null)
                return false;

            return await _movieRepository.RemoveAsync(movie);
        }

        public async Task<MovieDto?> UpdateAsync(MovieUpdateDto updatedMovie)
        {
            var existingMovie = await _movieRepository.GetByIdAsync(m => m.Id == updatedMovie.Id, tracked: true, includeProperties: "Genres");

            if (existingMovie is null)
                return null;

            existingMovie = updatedMovie.MapToMovie(existingMovie);
            await _movieValidator.ValidateAndThrowAsync(existingMovie);

            existingMovie.Genres.Clear();
            var genres = await _genreService.GetGenresByNamesAsync(updatedMovie.Genres);

            if (genres != null)
                existingMovie.Genres = genres;

            await _movieRepository.UpdateAsync(existingMovie);
            return existingMovie.MapToMovieResponse();
        }
    }
}
