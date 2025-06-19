using System.ComponentModel.DataAnnotations;

namespace eduzora_lms.Models.Admin.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Invalid email address format"),]
        [RegularExpression(@"^[^@\s]+@(gmail\.com|hotmail\.com)$",
                      ErrorMessage = "Email Pattern MisMatch: Only Gmail and Hotmail are allowed.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(3, ErrorMessage = "Password Must Be at least > 3 Characters Long")]
        public string? Password { get; set; }

    }
}
