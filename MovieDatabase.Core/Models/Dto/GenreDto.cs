using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Core.Models.Dto
{
    public class GenreDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "Name cannot exceed 30 characters.")]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
