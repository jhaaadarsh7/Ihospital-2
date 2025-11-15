using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ihospital.API.Models
{
    [Table("RESPONSE_DETAIL")]
    public class ResponseDetail
    {
        [Key]
        [Column("ResponseDetail_ID")]
        public int ResponseDetailId { get; set; }

        [Required]
        [Column("SurveyResponse_ID")]
        public int SurveyResponseId { get; set; }

        [Required]
        [Column("Question_ID")]
        public int QuestionId { get; set; }

        [Column("Option_ID")]
        public int? OptionId { get; set; }

        [StringLength(500)]
        [Column("Answer_Text")]
        public string? AnswerText { get; set; }

        [Column("Created_DateTime")]
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("SurveyResponseId")]
        public virtual SurveyResponse SurveyResponse { get; set; } = null!;

        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; } = null!;

        [ForeignKey("OptionId")]
        public virtual OptionList? Option { get; set; }
    }
}
