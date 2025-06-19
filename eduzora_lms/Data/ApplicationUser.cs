using eduzora_lms.Models.Instructor.Domain;
using Microsoft.AspNetCore.Identity;

namespace eduzora_lms.Data
{
    public class ApplicationUser : IdentityUser
    {
        // Add Custom Field That we want to add in ASPNETUsers Tbl
        public string? PhotoPath { get; set; } // store the PhotoPath

        // Add Custom Field For Student Edit-Profile Page
        public string? Address { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? Zipcode { get; set; }

        // Add Custom Field For Instructor Registration Page
        public string? Designation { get; set; }

        // [PK] one category Many Courses
        public ICollection<Course> Courses { get; set; }
    }
}
