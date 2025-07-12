using Microsoft.EntityFrameworkCore;
using MovieDatabase.Core.Data;
using MovieDatabase.Core.Models;
using MovieDatabase.Core.Models.Dto;
using MovieDatabase.Core.Repository.IRepository;
using System.Linq.Expressions;

namespace MovieDatabase.Core.Repository
{
    public class MovieRepository : BaseRepository<Movie>, IMovieRepository
    {
        public MovieRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Movie?> GetByTitleAsync(string title)
        {
            return await _dbSet.FirstOrDefaultAsync(m => m.Title.ToLower() == title.ToLower());
        }

        public async Task<IEnumerable<MovieDto>> GetMoviesAsync(Expression<Func<Movie, bool>>? filter = null, bool includeCast = false)
        {
            IQueryable<Movie> query = _dbSet;
            _dbSet.AsNoTracking();

            if (filter != null)
                query = query.Where(filter);

            if (includeCast)
            {
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
                return await query
                    .Select(m => new MovieDto
                    {
                        Id = m.Id,
                        Title = m.Title,
                        ReleaseDate = m.ReleaseDate,
                        Length = m.Length,
                        Rating = m.Rating,
                        Genres = m.Genres.Select(g => g.Name).ToList(),
                        Cast = new List<MovieCastDto>()
                    })
                    .ToListAsync();
            }
        }

        public async Task<bool> MovieExistsAsync(int id)
        {
            return await _dbSet.AnyAsync(m => m.Id == id);
        }

        public async Task<Movie?> GetByTitleAndReleaseDate(string title, DateOnly releaseDate)
        {
            return await _dbSet.FirstOrDefaultAsync(m => m.Title.ToLower() == title.ToLower() && m.ReleaseDate == releaseDate);
        }
    }
}