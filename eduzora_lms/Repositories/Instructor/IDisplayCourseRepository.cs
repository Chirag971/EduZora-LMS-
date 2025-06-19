using eduzora_lms.Models.Admin.Domain;
using eduzora_lms.Models.Instructor.Domain;

namespace eduzora_lms.Repositories.Instructor
{
    public interface IDisplayCourseRepository
    {
        // Display All Courses By Instructor ID
        Task<IEnumerable<Course>> GetCoursesByInstructorAsync(string InstructorId);

        // Get Course By id
        Task<Course?> GetCourseById(Guid id);

        // Update Course Basic Info
        Task<Course?> UpdateBasicCourseInfoAsync(Course course);

        // Update Course Featured Photo
        Task<Course?> UpdateCourseFeaturedPhoto(Course course);

        // Update Course Featured Banner
        Task<Course?> UpdateCourseFeaturedBanner(Course course);

        // Update Course Featured Video
        Task<Course?> UpdateCourseFeaturedVideo(Course course);

        Task<Course?> DeleteCourse(Guid id, string instructor_id);

        // Get All Categories For DropDown
        Task<IEnumerable<Category>> GetAllCategoriesAsync();

        // Get All Levels For DropDown
        Task<IEnumerable<Level>> GetAllLevelsAsync();

        // Get All Langauges For DropDown
        Task<IEnumerable<Language>> GetAllLanguagesAsync();

    }
}
