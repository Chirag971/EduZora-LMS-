using System.ComponentModel.DataAnnotations;

namespace eduzora_lms.Models.Admin.ViewModels
{
    public class EditProfileViewModel
    {
        [Required(ErrorMessage = "Full Name Can't Be Empty!")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email Can't be Empty!")]
        [EmailAddress(ErrorMessage = "Email Is Invalid!")]
        [RegularExpression(@"^[^@\s]+@(gmail\.com|hotmail\.com)$",
                      ErrorMessage = "Email Pattern MisMatch: Only Gmail and Hotmail are allowed.")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password Can't be Empty")]
        [DataType(DataType.Password)]
        [MinLength(3, ErrorMessage = "Password Must Be at least > 3 Characters Long")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Retype Password is required.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The Password and Retype Password do not match.")]
        public string RetypePassword { get; set; }

        [Display(Name = "Photo")]
        public IFormFile? UserPhoto { get; set; } // User Upload New Photo

        public string? PhotoPath { get; set; } // To Display the Path Of Current Photo
    }
}
