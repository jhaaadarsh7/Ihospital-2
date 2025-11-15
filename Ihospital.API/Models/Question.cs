using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ihospital.API.Models
{
    [Table("QUESTION")]
    public class Question
    {
        [Key]
        [Column("Question_ID")]
        public int QuestionId { get; set; }

        [Required]
        [StringLength(500)]
        [Column("Question_Text")]
        public string QuestionText { get; set; } = string.Empty;

        [StringLength(100)]
        [Column("Question_Code")]
        public string? QuestionCode { get; set; }

        [StringLength(100)]
        [Column("Question_Category")]
        public string? QuestionCategory { get; set; }

        [StringLength(50)]
        [Column("Response_Type")]
        public string? ResponseType { get; set; } // SingleChoice, MultiChoice, Text, Numeric, Date

        [Column("Is_Active")]
        public bool IsActive { get; set; } = true;

        [Column("Is_Required")]
        public bool IsRequired { get; set; } = false;

        [Column("Max_Selections")]
        public int? MaxSelections { get; set; }

        [Column("Display_Order")]
        public int DisplayOrder { get; set; } = 0;

        // Navigation properties
        public virtual ICollection<OptionList> Options { get; set; } = new List<OptionList>();
        public virtual ICollection<ResponseDetail> ResponseDetails { get; set; } = new List<ResponseDetail>();
    }
}
