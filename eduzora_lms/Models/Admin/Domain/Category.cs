using System.ComponentModel.DataAnnotations;
using eduzora_lms.Models.Instructor.Domain;

namespace eduzora_lms.Models.Admin.Domain
{
    public class Category
    {
        // PK Guid id
        [Key]
        public Guid Id { get; set; }

        // Category Name
        [Required(ErrorMessage = "Category Name Field Is Required")]
        [Display(Name = "Category Name")]
        [StringLength(100,ErrorMessage = "Category Name cannot exceed 100 characters.")]
        public string CategoryName { get; set; }

        // [PK] one category Many Courses
        public ICollection<Course> Courses { get; set; }
    }
}
