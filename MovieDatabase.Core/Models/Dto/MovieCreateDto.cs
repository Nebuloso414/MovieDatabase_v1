namespace MovieDatabase.Core.Models.Dto
{
    public class MovieCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public DateOnly ReleaseDate { get; set; }
        public int? Length { get; set; }
        public decimal Rating { get; set; } 
        public List<string> Genres { get; set; } = new();
    }
}
