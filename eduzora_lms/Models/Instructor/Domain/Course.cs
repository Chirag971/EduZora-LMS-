using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using eduzora_lms.Data;
using eduzora_lms.Models.Admin.Domain;

namespace eduzora_lms.Models.Instructor.Domain
{
    
    public class Course
    {
        // PK Guid id
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Title Field Is Required")]
        [Display(Name = "Title")]
        [StringLength(255, ErrorMessage = "Course Title cannot exceed 255 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Slug Field Is Required")]
        [StringLength(255, ErrorMessage = "Course Slug cannot exceed 255 characters.")]
        public string Slug { get; set; }

        [Required(ErrorMessage = "Description Field Is Required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price Field Is Required")]
        public int Price { get; set; }

        public int? PriceOld { get; set; }

        // Category ID (FK)
        [Required]
        public Guid Category_id { get; set; }

        [ForeignKey("Category_id")]
        public Category Category { get; set; }


        // Level ID (FK)
        [Required]
        public Guid Level_id { get; set; }
        [ForeignKey("Level_id")]
        public Level Level { get; set; }


        // Langauge ID (FK)
        [Required]
        public Guid  Language_id{ get; set; }
        [ForeignKey("Language_id")]
        public Language Language { get; set; }


        // Instructor ID (FK ---> AspNetUsers  --> id)
        public string InstructorId { get; set; }

        [ForeignKey("InstructorId")]
         public ApplicationUser Instructor { get; set; }


        // Total Students Default = 0 
        public int TotalStudent { get; set; } = 0;

        // Total Rating Deafult = 0
        public int TotalRating { get; set; } = 0;

        // Total Rating Score Deafult = 0 
        public int TotalRatingScore { get; set; } = 0;

        // Average Rating Score Deafult = 0
        public int AverageRating { get; set; } = 0;

        [Required(ErrorMessage = "Featured Photo Field Is Required")]
        public string FeaturedPhoto { get; set; }

        [Required(ErrorMessage = "Featured Banner Field Is Required")]
        public string FeaturedBanner { get; set; }

        [Required(ErrorMessage = "Featured Video Type Field Is Required")]
        public string FeaturedVideoType { get; set; }

        [Required(ErrorMessage = "Featured Video Content Field Is Required")]
        public string FeaturedVideoContent { get; set; }


        // Total Video Hours Default = 0
        public int TotalVideoHours { get; set; } = 0;
        // Total Videos Default = 0
        public int TotalVideos { get; set; } = 0;
        // Total Resources Default = 0
        public int TotalResources { get; set; } = 0;

        // Default Set to Current Time
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Course Status is Required")]
        public string Status { get; set; }
    }
}
