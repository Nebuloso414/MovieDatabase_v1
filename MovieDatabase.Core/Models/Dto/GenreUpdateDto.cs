using System.ComponentModel.DataAnnotations;

namespace MovieDatabase.Core.Models
{
    public class GenreUpdateDto
    {
        public int Id { get; private set; }        
        public string Name { get; init; } = string.Empty;
        public string? Description { get; init; }

        public void SetId(int id)
        {
            Id = id;
        }
    }
}
