using System.ComponentModel.DataAnnotations;

namespace eduzora_lms.Models.Admin.ViewModels
{
    public class AddLanguageRequest
    {
        [Required(ErrorMessage = "Language Name can't Empty")]
        public string LanguageName { get; set; }
    }
}
