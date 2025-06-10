namespace MovieDatabase.Models
{
    public class MovieCast
    {
        // Foreign Keys & Composite Primary Key
        public int MovieId { get; set; }
        public int PersonId { get; set; }
        public int RoleId { get; set; }

        // Navigation properties
        public Movie Movie { get; set; } = null!;
        public People Person { get; set; } = null!;
        public Role Role { get; set; } = null!;

        // Audit fields
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
