using System.ComponentModel.DataAnnotations;
using eduzora_lms.Models.Instructor.Domain;

namespace eduzora_lms.Models.Admin.Domain
{
    public class Language
    {
        // PK Guid id
        [Key]
        public Guid Id { get; set; }

        // Category Name
        [Required(ErrorMessage = "Language Name Field Is Required")]
        [Display(Name = "Level Name")]
        [StringLength(100, ErrorMessage = "Language Name cannot exceed 100 characters.")]
        public string LanguageName { get; set; }


        // [PK] One Language Many Courses
        public ICollection<Course>  Courses { get; set; }
    }
}
