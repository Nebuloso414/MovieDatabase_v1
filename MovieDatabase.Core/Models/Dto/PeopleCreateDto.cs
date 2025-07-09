namespace MovieDatabase.Core.Models.Dto
{
    public class PeopleCreateDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
    }
}