using System.ComponentModel.DataAnnotations;

namespace eduzora_lms.Models.Instructor.ViewModels
{
    public class EditCourseFeaturedVideoRequestViewModel
    {
        public Guid Id { get; set; }

        public string? CurrentFeaturedVideoType { get; set; }
        public string? CurrentFeaturedVideoContent { get; set; }

        [Required(ErrorMessage = "Select Course Video Type")]
        public string? FeaturedVideoType { get; set; }
        public string? FeaturedVideoContentYoutube{ get; set; }
        public string? FeaturedVideoContentVimeo { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? FeaturedVideoContentMP4File { get; set; } 
    }
}
