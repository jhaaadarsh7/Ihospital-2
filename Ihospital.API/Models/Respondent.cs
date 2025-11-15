using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ihospital.API.Models
{
    [Table("RESPONDENT")]
    public class Respondent
    {
        [Key]
        [Column("Respondent_ID")]
        public int RespondentId { get; set; }

        [Column("Is_Anonymous")]
        public bool IsAnonymous { get; set; } = false;

        [StringLength(20)]
        [Column("Title")]
        public string? Title { get; set; }

        [StringLength(100)]
        [Column("First_Name")]
        public string? FirstName { get; set; }

        [StringLength(100)]
        [Column("Last_Name")]
        public string? LastName { get; set; }

        [Column("Date_Of_Birth")]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(20)]
        [Column("Gender")]
        public string? Gender { get; set; }

        [StringLength(30)]
        [Column("Contact_Phone")]
        public string? ContactPhone { get; set; }

        [StringLength(150)]
        [Column("Email")]
        public string? Email { get; set; }

        [StringLength(100)]
        [Column("State")]
        public string? State { get; set; }

        [StringLength(100)]
        [Column("Home_Suburb")]
        public string? HomeSuburb { get; set; }

        [StringLength(10)]
        [Column("Home_Postcode")]
        public string? HomePostcode { get; set; }

        [StringLength(50)]
        [Column("Mac_Address")]
        public string? MacAddress { get; set; }

        [Column("Created_DateTime")]
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

        // Navigation property
        public virtual ICollection<SurveyResponse> SurveyResponses { get; set; } = new List<SurveyResponse>();
    }
}
