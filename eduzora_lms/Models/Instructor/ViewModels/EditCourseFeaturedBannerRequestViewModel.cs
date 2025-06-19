using System.ComponentModel.DataAnnotations;

namespace eduzora_lms.Models.Instructor.ViewModels
{
    public class EditCourseFeaturedBannerRequestViewModel
    {
        public Guid id { get; set; }

        public string? CurrentFeaturedBanner { get; set; }

        [Required(ErrorMessage = "Please Choose A New Featured Banner")]
        [DataType(DataType.Upload)]
        public IFormFile? FeaturedBannerFile { get; set; }

    }
}
