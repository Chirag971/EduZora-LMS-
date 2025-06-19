namespace eduzora_lms.Models.Instructor.ViewModels
{
    public class InstructorCoursesDisplayViewModel
    {
        public Guid CourseId { get; set; }
        public string Title { get; set; }
        public string FeaturedPhoto { get; set; }
        public int Price { get; set; }
        public string Status { get; set; }

    }
}
