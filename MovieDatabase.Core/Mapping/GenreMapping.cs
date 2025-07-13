using MovieDatabase.Core.Models;
using MovieDatabase.Core.Models.Dto;

namespace MovieDatabase.Core.Mapping
{
    public static class GenreMapping
    {
        public static Genre MapToGenre(this GenreDto genreDto)
        {
            return new Genre
            {
                Id = genreDto.Id,
                Name = genreDto.Name,
                Description = genreDto.Description
            };
        }
        public static GenreDto MapToGenreResponse(this Genre genre)
        {
            return new GenreDto
            {
                Id = genre.Id,
                Name = genre.Name,
                Description = genre.Description
            };
        }

        public static Genre MapToGenre(this GenreCreateDto genreCreateDto)
        {
            return new Genre
            {
                Name = genreCreateDto.Name,
                Description = genreCreateDto.Description
            };
        }

        public static Genre MapToGenre(this GenreUpdateDto genreUpdateDto, Genre existingGenre)
        {
            existingGenre.Name = genreUpdateDto.Name;
            existingGenre.Description = genreUpdateDto.Description;

            return existingGenre;

        }
    }
}
