using eduzora_lms.Data;
using eduzora_lms.Models.Admin.Domain;
using eduzora_lms.Models.Instructor.Domain;
using Microsoft.EntityFrameworkCore;

namespace eduzora_lms.Repositories.Instructor
{
    public class CreateCourseRepository : ICreateCourseRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public CreateCourseRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<Course> AddAsync(Course course)
        {
            // Add Course to table
            await applicationDbContext.Courses.AddAsync(course);
            await applicationDbContext.SaveChangesAsync();

            return course;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            // Get All categories
            return await applicationDbContext.Categories.ToListAsync();
        }

        public async Task<IEnumerable<Language>> GetAllLanguagesAsync()
        {
            // Get All languages
            return await applicationDbContext.Languages.ToListAsync();

        }

        public async Task<IEnumerable<Level>> GetAllLevelsAsync()
        {
            // get all Levels
            return await applicationDbContext.Levels.ToListAsync();
        }
      
    }
}
