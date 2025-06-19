using System.ComponentModel.DataAnnotations;

namespace eduzora_lms.Models.Admin.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Invalid email address format"),]
        [RegularExpression(@"^[^@\s]+@(gmail\.com|hotmail\.com)$",
                      ErrorMessage = "Email Pattern MisMatch: Only Gmail and Hotmail are allowed.")]
        public string? Email { get; set; }
    }
}
