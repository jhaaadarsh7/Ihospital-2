using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ihospital.API.Models
{
    [Table("OPTION_LIST")]
    public class OptionList
    {
        [Key]
        [Column("Option_ID")]
        public int OptionId { get; set; }

        [Required]
        [Column("Question_ID")]
        public int QuestionId { get; set; }

        [Required]
        [StringLength(255)]
        [Column("Option_Text")]
        public string OptionText { get; set; } = string.Empty;

        [Column("Is_Active")]
        public bool IsActive { get; set; } = true;

        [Column("Option_Order")]
        public int OptionOrder { get; set; } = 0;

        // Navigation property
        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; } = null!;

        public virtual ICollection<ResponseDetail> ResponseDetails { get; set; } = new List<ResponseDetail>();
    }
}
