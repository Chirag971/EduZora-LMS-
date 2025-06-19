using System.ComponentModel.DataAnnotations;

namespace eduzora_lms.Models.Admin.Domain
{
    public class Setting
    {
        // PK Guid id
        [Key]
        public Guid Id { get; set; }

        // Sales_Commission
        [Required(ErrorMessage = "Sales Commission Can't be Empty")]
        [Display(Name = "Sales Commission")]
        [Range(0,100, ErrorMessage = "Sales Commission must be between {1} and {2}.")]
        public int SalesCommission { get; set; }
    }
}
