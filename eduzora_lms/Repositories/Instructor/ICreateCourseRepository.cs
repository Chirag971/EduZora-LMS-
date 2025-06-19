using eduzora_lms.Models.Admin.Domain;
using eduzora_lms.Models.Instructor.Domain;

namespace eduzora_lms.Repositories.Instructor
{
    public interface ICreateCourseRepository
    {
        // Add Course
        Task<Course> AddAsync(Course course);

        // Get All Categories For DropDown
        Task<IEnumerable<Category>> GetAllCategoriesAsync();

        // Get All Levels For DropDown
        Task<IEnumerable<Level>> GetAllLevelsAsync();

        // Get All Langauges For DropDown
        Task<IEnumerable<Language>> GetAllLanguagesAsync();

    }
}
