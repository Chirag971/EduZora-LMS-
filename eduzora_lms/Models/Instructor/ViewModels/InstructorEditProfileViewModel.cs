using System.ComponentModel.DataAnnotations;

namespace eduzora_lms.Models.Instructor.ViewModels
{
    public class InstructorEditProfileViewModel
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

        // Add Migration
        [Required(ErrorMessage = "Phone no is Required")]
        [Display(Name = "Phone")]
        [MaxLength(10, ErrorMessage = "Phone No Must Be At least <= 10 numbers.")]
        [MinLength(10, ErrorMessage = "Phone No Must Be At least >= 10 numbers.")]
        public string Phone { get; set; }

        // Add Migration
        [Required(ErrorMessage = "Address is Required")]
        [Display(Name = "Address")]
        public string Address { get; set; }

        // Add MIgration
        [Required(ErrorMessage = "Country is Required")]
        [Display(Name = "Country")]
        public string Country { get; set; }

        // Add Migration
        [Required(ErrorMessage = "State is Required")]
        [Display(Name = "State")]
        public string State { get; set; }

        // Add Migration
        [Required(ErrorMessage = "City is Required")]
        [Display(Name = "City")]
        public string City { get; set; }

        // Add Migration
        [Required(ErrorMessage = "Zipcode is Required")]
        [Display(Name = "Zipcode")]
        [MaxLength(6, ErrorMessage = "ZipCode Must 6 be Digit. ")]
        [MinLength(6, ErrorMessage = "ZipCode Always 6 Digit Long. ")]
        public string Zipcode { get; set; }

        [Required(ErrorMessage = "Password Can't be Empty")]
        [DataType(DataType.Password)]
        [MinLength(3, ErrorMessage = "Password Must Be at least > 3 Characters Long")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Retype Password is required.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The Password and Confirm Password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Photo")]
        public IFormFile? UserPhoto { get; set; } // User Upload New Photo
        public string? PhotoPath { get; set; }
    }
}
