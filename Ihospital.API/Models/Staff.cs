using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ihospital.API.Models
{
    [Table("STAFF")]
    public class Staff
    {
        [Key]
        [Column("Staff_ID")]
        public int StaffId { get; set; }

        [Required]
        [StringLength(100)]
        [Column("UserName")]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        [Column("Password")]
        public string Password { get; set; } = string.Empty;
    }
}
