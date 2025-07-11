﻿using MovieDatabase.Core.Models;
using System.Linq.Expressions;

namespace MovieDatabase.Core.Services
{
    public interface IGenreService
    {
        Task<IEnumerable<Genre>> GetAllAsync(Expression<Func<Genre, bool>>? filter = null, string? includeProperties = null);
        Task<Genre?> GetByIdAsync(Expression<Func<Genre, bool>>? filter = null, bool tracked = true, string? includeProperties = null);
        Task<Genre> CreateAsync(Genre newGenre);
        Task DeleteAsync(Genre genre);
        Task<Genre> UpdateAsync(Genre updatedGenre);
        Task<(List<Genre> FoundGenres, List<string> NotFoundGenres)> ProcessGenreNamesAsync(List<string> genreNames);
    }
}
