using System.ComponentModel.DataAnnotations;
using eduzora_lms.Models.Instructor.Domain;

namespace eduzora_lms.Models.Admin.Domain
{
    public class Level
    {
        // PK Guid id
        [Key]
        public Guid Id { get; set; }

        // Level Name
        [Required(ErrorMessage = "Level Name Field Is Required")]
        [Display(Name = "Level Name")]
        [StringLength(100, ErrorMessage = "Level Name cannot exceed 100 characters.")]
        public string LevelName { get; set; }

        // [PK] One Level Many Courses
        public ICollection<Course> Courses { get; set; }
    }
}
