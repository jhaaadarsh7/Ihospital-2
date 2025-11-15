namespace Ihospital.API.DTOs
{
    public class RespondentDto
    {
        public int RespondentId { get; set; }
        public bool IsAnonymous { get; set; }
        public string? Title { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? ContactPhone { get; set; }
        public string? Email { get; set; }
        public string? State { get; set; }
        public string? HomeSuburb { get; set; }
        public string? HomePostcode { get; set; }
        public string? MacAddress { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }

    public class CreateRespondentDto
    {
        public bool IsAnonymous { get; set; }
        public string? Title { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? ContactPhone { get; set; }
        public string? Email { get; set; }
        public string? State { get; set; }
        public string? HomeSuburb { get; set; }
        public string? HomePostcode { get; set; }
        public string? MacAddress { get; set; }
    }

    public class UpdateRespondentDto
    {
        public bool? IsAnonymous { get; set; }
        public string? Title { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? ContactPhone { get; set; }
        public string? Email { get; set; }
        public string? State { get; set; }
        public string? HomeSuburb { get; set; }
        public string? HomePostcode { get; set; }
    }
}
