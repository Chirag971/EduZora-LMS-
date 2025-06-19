using System.ComponentModel.DataAnnotations;

namespace eduzora_lms.Models.Instructor.ViewModels
{
    public class InstructorRegistrationViewModel
    {
        [Required(ErrorMessage = "Name Field is Required")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Instructor Designation is Required")]
        [Display(Name = "Designation")]
        public string Designation { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Invalid email address format"),]
        [RegularExpression(@"^[^@\s]+@(gmail\.com|hotmail\.com)$",
                     ErrorMessage = "Email Pattern MisMatch: Only Gmail and Hotmail are allowed.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm Password is required.")]
        [Compare("Password", ErrorMessage = "The Password and Confirm Password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
