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

        public async Task<IEnumerable<Movie>> GetAllAsync(Expression<Func<Movie, bool>>? filter = null, string? includeProperties = null)
        {
            return await _movieRepository.GetAllAsync(filter, includeProperties);
        }

        public async Task<Movie?> GetByIdAsync(Expression<Func<Movie, bool>>? filter = null, bool tracked = true, string? includeProperties = null)
        {
            return await _movieRepository.GetByIdAsync(filter, tracked, includeProperties);
        }

        public async Task<Movie> CreateAsync(MovieCreateDto movieDto)
        {
            var movie = _mapper.Map<Movie>(movieDto);
            if (movieDto.Genres != null && movieDto.Genres.Any())
            {
                var foundGenres = await _genreRepository.GetGenresByNamesAsync(movieDto.Genres);

                // Check if all genres were found
                if (foundGenres.Count != movieDto.Genres.Count)
                {
                    var missingGenres = movieDto.Genres
                        .Where(g => !foundGenres.Any(fg => fg.Name.Equals(g, StringComparison.OrdinalIgnoreCase)))
                        .ToList();

                    throw new ArgumentException($"The following genres do not exist: {string.Join(", ", missingGenres)}");
                }

                movie.Genres = foundGenres;
            }

            await _movieRepository.CreateAsync(movie);
            return movie;
        }
    }
}
