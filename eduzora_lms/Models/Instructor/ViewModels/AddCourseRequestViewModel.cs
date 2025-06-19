using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eduzora_lms.Models.Instructor.ViewModels
{
    public class AddCourseRequestViewModel
    {

        [Required(ErrorMessage = "Course Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Course Slug is required.")]
        [RegularExpression(@"^[a-zA-Z0-9-]+$", ErrorMessage = "Course Slug can only contain alphanumeric characters and hyphens.")]
        public string Slug { get; set; }


        [Required(ErrorMessage = "Course Price is required.")]
        [Range(599, 5999, ErrorMessage = "Price should be between ₹{1} and ₹{2}")]
        public int? Price { get; set; }


        [Range(599, 5999, ErrorMessage = "Old Price should be between ₹{1} and ₹{2}")]
        public int? PriceOld{ get; set; }

        // Category 
        [Required(ErrorMessage = "Course Category is Required.")]
        public Guid? Category_id { get; set; }
        public IEnumerable<SelectListItem>? Categories { get; set; }

        // Levels
        [Required(ErrorMessage = "Course Level is Required.")]
        public Guid? Level_id { get; set; }
        public IEnumerable<SelectListItem>? Levels { get; set; }

        // Languages
        [Required(ErrorMessage = "Course Language is Required.")]
        public Guid? Language_id { get; set; }
        public IEnumerable<SelectListItem>? Languages { get; set; }

        [Required(ErrorMessage = "Course Description is required.")]
        public string Description { get; set; }


        [Required(ErrorMessage = "Course Featured Photo is required.")]
        [DataType(DataType.Upload)] // Hint for UI, not validation
        public IFormFile? FeaturedPhotoFile { get; set; }

        [Required(ErrorMessage = "Course Featured Banner is required.")]
        [DataType(DataType.Upload)]
        public IFormFile? FeaturedBannerFile { get; set; }

        [Required(ErrorMessage = "Course Featured Video is required.")]
        [Display(Name = "Featured Video Type")]
        public string? FeaturedVideoType { get; set; }


        // Youtube Link
        
        public string? FeaturedVideoContentYoutube { get; set; }

        // Vimeo Link
        
        public string? FeaturedVideoContentVimeo { get; set; }

        // MP4 file upload
        [DataType(DataType.Upload)] 
        public IFormFile? FeaturedVideoContentMp4File { get; set; }

    }
}
