using System.ComponentModel.DataAnnotations;

namespace eduzora_lms.Models.Admin.ViewModels
{
    public class EditCategoryRequest
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Category Name can't Empty")]
        public string CategoryName { get; set; }
    }
}
