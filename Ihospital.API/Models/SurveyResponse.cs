using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ihospital.API.Models
{
    [Table("SURVEY_RESPONSE")]
    public class SurveyResponse
    {
        [Key]
        [Column("SurveyResponse_ID")]
        public int SurveyResponseId { get; set; }

        [Required]
        [Column("Respondent_ID")]
        public int RespondentId { get; set; }

        [Column("Response_DateTime")]
        public DateTime ResponseDateTime { get; set; } = DateTime.Now;

        [StringLength(50)]
        [Column("Mac_Address")]
        public string? MacAddress { get; set; }

        // Navigation properties
        [ForeignKey("RespondentId")]
        public virtual Respondent Respondent { get; set; } = null!;

        public virtual ICollection<ResponseDetail> ResponseDetails { get; set; } = new List<ResponseDetail>();
    }
}
