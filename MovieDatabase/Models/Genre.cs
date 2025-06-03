using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Models
{
    public class Genre
    {
        public int Id { get; init; }
        [Required]
        public required string Name { get; init; }
        public string? Description { get; init; }
        public DateTime CreatedDate { get; init; }
        public DateTime UpdatedDate { get; init; }

        // Many-to-many: a genre can belong to multiple movies
        public List<Movie> Movies { get; init; } = new();
    }
}