using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Models
{
    public class Genre
    {
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        // Many-to-many: a genre can belong to multiple movies
        public List<Movie> Movies { get; set; } = new();
    }
}