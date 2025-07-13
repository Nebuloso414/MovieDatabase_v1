namespace MovieDatabase.Core.Models.Dto
{
    public class PeopleUpdateDto
    {
        public int Id { get; private set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }

        public void SetId(int id)
        {
            Id = id;
        }
    }
}
