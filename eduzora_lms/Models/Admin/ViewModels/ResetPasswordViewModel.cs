using System.ComponentModel.DataAnnotations;

namespace eduzora_lms.Models.Admin.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm Password is required.")]
        [Compare("Password", ErrorMessage = "The Password and Confirm Password do not match.")]
        public string? ConfirmPassword { get; set; }

        [Required] // Hidden field to carry the UserId from the URL
        public string? UserId { get; set; }

        [Required] // Hidden field to carry the token from the URL
        public string? Token { get; set; }
    }
}
