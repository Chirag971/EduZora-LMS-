using System.ComponentModel.DataAnnotations;

namespace eduzora_lms.Models.Admin.ViewModels
{
    public class EditLevelRequest
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Level Name can't Empty")]
        public string LevelName { get; set; }
    }
}
