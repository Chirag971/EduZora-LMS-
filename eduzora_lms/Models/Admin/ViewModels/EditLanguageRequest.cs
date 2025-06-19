using System.ComponentModel.DataAnnotations;

namespace eduzora_lms.Models.Admin.ViewModels
{
    public class EditLanguageRequest
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Language Name can't Empty")]
        public string LanguageName { get; set; }
    }
}
