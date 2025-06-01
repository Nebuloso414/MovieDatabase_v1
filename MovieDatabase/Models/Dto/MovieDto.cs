namespace MovieDatabase.Models.Dto
{
    public class MovieDto
    {

        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public int? Length { get; set; }
        public decimal Rating { get; set; }
        public List<string> Genres { get; set; } = new();
    }
}
