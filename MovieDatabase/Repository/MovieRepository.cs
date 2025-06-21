using Microsoft.EntityFrameworkCore;
using MovieDatabase.Data;
using MovieDatabase.Models;
using MovieDatabase.Models.Dto;
using MovieDatabase.Repository.IRepository;

namespace MovieDatabase.Repository
{
    public class MovieRepository : BaseRepository<Movie>, IMovieRepository
    {
        public MovieRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Movie?> GetByTitleAsync(string title)
        {
            return await _dbSet.FirstOrDefaultAsync(m => m.Title.ToLower() == title.ToLower());
        }

        public async Task<IEnumerable<MovieDto>> GetMoviesWithProjectionAsync(bool includeCast = false)
        {
            // Start with base query
            IQueryable<Movie> query = _dbSet;

            if (includeCast)
            {
                // Use projection to select only the needed properties
                return await query
                    .Select(m => new MovieDto
                    {
                        Id = m.Id,
                        Title = m.Title,
                        ReleaseDate = m.ReleaseDate,
                        Length = m.Length,
                        Rating = m.Rating,
                        Genres = m.Genres.Select(g => g.Name).ToList(),
                        Cast = m.Cast.Select(c => new MovieCastDto
                        {
                            Person = c.Person.FirstName + " " + c.Person.LastName,
                            Role = c.Role.Name
                        }).ToList()
                    })
                    .ToListAsync();
            }
            else
            {
                // Without cast, use a simpler projection
                return await query
                    .Select(m => new MovieDto
                    {
                        Id = m.Id,
                        Title = m.Title,
                        ReleaseDate = m.ReleaseDate,
                        Length = m.Length,
                        Rating = m.Rating,
                        Genres = m.Genres.Select(g => g.Name).ToList(),
                        Cast = new List<MovieCastDto>() // Empty list
                    })
                    .ToListAsync();
            }
        }
    }
}