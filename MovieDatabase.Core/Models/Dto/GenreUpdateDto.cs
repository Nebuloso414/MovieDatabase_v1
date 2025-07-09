using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Core.Models
{
    public class GenreUpdateDto
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        [StringLength(30, ErrorMessage = "Name cannot exceed 30 characters.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Description cannot exceed 100 characters.")]
        public string? Description { get; set; }
    }
}
