using System.ComponentModel.DataAnnotations;

namespace eduzora_lms.Models.Admin.ViewModels
{
    public class AddCategoryRequest
    {
        [Required(ErrorMessage = "Category Name can't Empty")]
        public string CategoryName { get; set; }
    }
}
