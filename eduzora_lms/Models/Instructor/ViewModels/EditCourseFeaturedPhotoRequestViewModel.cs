using System.ComponentModel.DataAnnotations;

namespace eduzora_lms.Models.Instructor.ViewModels
{
    public class EditCourseFeaturedPhotoRequestViewModel
    {
        public Guid id { get; set; }

        public string? CurrentFeaturedPhoto { get; set; }

        [Required(ErrorMessage = "Please Choose A New Featured Photo")]
        [DataType(DataType.Upload)]
        public IFormFile? FeaturedPhotoFile { get; set; }

    }
}
