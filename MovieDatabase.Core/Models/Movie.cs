using System.Collections.Generic;

namespace MovieDatabase.Core.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public int? Length { get; set; } // Length in minutes
        public decimal Rating { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        // Many-to-many: a movie can have multiple genres
        public List<Genre> Genres { get; set; } = new();
        public List<MovieCast> Cast { get; set; } = new();
    }
}