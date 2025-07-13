using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Core.Models
{
    public class GenreCreateDto
    {
        public required string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
