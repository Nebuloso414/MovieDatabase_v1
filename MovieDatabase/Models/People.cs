namespace MovieDatabase.Models
{
    public class People
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string Name => $"{FirstName} {LastName}";
        public int Age => DateOfBirth.HasValue ? DateTime.Now.Year - DateOfBirth.Value.Year : 0;
        
    }
}
