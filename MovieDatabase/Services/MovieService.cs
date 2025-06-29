using AutoMapper;
using MovieDatabase.Models;
using MovieDatabase.Models.Dto;
using MovieDatabase.Repository.IRepository;
using System.Linq.Expressions;
using System.Net;

namespace MovieDatabase.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;

        public MovieService(IMovieRepository movieRepository, IGenreRepository genreRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _genreRepository = genreRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MovieDto>> GetMoviesAsync(Expression<Func<Movie, bool>>? filter = null, bool includeCast = false)
        {
            return await _movieRepository.GetMoviesAsync(filter, includeCast);
        }

        public async Task<Movie> CreateAsync(MovieCreateDto movieDto)
        {
            var movie = _mapper.Map<Movie>(movieDto);
            if (movieDto.Genres != null && movieDto.Genres.Any())
            {
                var foundGenres = await _genreRepository.GetGenresByNamesAsync(movieDto.Genres);

                if (foundGenres.Count != movieDto.Genres.Count)
                {
                    var missingGenres = movieDto.Genres
                        .Where(g => !foundGenres.Any(fg => fg.Name.Equals(g, StringComparison.OrdinalIgnoreCase)))
                        .ToList();

                    throw new BadHttpRequestException($"The following genres do not exist: {string.Join(", ", missingGenres)}");
                }

                movie.Genres = foundGenres;
            }

            await _movieRepository.CreateAsync(movie);
            return movie;
        }

        public async Task DeleteAsync(Movie movie)
        {
            await _movieRepository.RemoveAsync(movie);
        }

        public async Task<Movie> UpdateAsync(MovieUpdateDto updatedMovie)
        {
            var existingMovie = await _movieRepository.GetByIdAsync(
                x => x.Id == updatedMovie.Id, 
                tracked: true,
                includeProperties: "Genres");
                
            if (existingMovie == null)
            {
                throw new BadHttpRequestException($"Movie with ID {updatedMovie.Id} not found.");
            }

            _mapper.Map(updatedMovie, existingMovie);

            if (updatedMovie.Genres != null)
            {
                var newGenres = await _genreRepository.GetGenresByNamesAsync(updatedMovie.Genres);

                if (newGenres.Count != updatedMovie.Genres.Count)
                {
                    var missingGenres = updatedMovie.Genres
                        .Where(g => !newGenres.Any(fg => fg.Name.Equals(g, StringComparison.OrdinalIgnoreCase)))
                        .ToList();

                    throw new BadHttpRequestException($"The following genres do not exist: {string.Join(", ", missingGenres)}");
                }

                existingMovie.Genres.Clear();
                foreach (var genre in newGenres)
                {
                    existingMovie.Genres.Add(genre);
                }
            }
            else
            {
                existingMovie.Genres.Clear();
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
