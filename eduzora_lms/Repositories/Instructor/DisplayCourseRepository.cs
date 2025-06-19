using eduzora_lms.Data;
using eduzora_lms.Models.Admin.Domain;
using eduzora_lms.Models.Instructor.Domain;
using Microsoft.EntityFrameworkCore;

namespace eduzora_lms.Repositories.Instructor
{
    public class DisplayCourseRepository : IDisplayCourseRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public DisplayCourseRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<Course>> GetCoursesByInstructorAsync(string InstructorId)
        {
            // Display All Courses Of Current Instructor ID OrderByDescending with UpdatedAt 
            return await applicationDbContext.Courses
                .Where(c => c.InstructorId == InstructorId)
                .OrderByDescending(c => c.UpdatedAt)
                .ToListAsync();
        }


        // Get Course By ID For Edit Course
        public Task<Course?> GetCourseById(Guid id)
        {
            // for getting single record
            return applicationDbContext.Courses.FirstOrDefaultAsync(x => x.Id == id);
        }


        // Update Basic Course Info
        public async Task<Course?> UpdateBasicCourseInfoAsync(Course course)
        {
            var existingBasicCourseInfo = await applicationDbContext.Courses.FindAsync(course.Id);

            if (existingBasicCourseInfo != null)
            {
                // Update Course Basic Info
                existingBasicCourseInfo.Title = course.Title;
                existingBasicCourseInfo.Slug = course.Slug;
                existingBasicCourseInfo.Price = course.Price;
                existingBasicCourseInfo.PriceOld = course.PriceOld;
                existingBasicCourseInfo.Category_id = course.Category_id;
                existingBasicCourseInfo.Level_id = course.Level_id;
                existingBasicCourseInfo.Language_id = course.Language_id;
                existingBasicCourseInfo.Description = course.Description;

                await applicationDbContext.SaveChangesAsync();

                return existingBasicCourseInfo;
            }

            return null;
        }


        // Update Course Featured Photo
        public async Task<Course?> UpdateCourseFeaturedPhoto(Course course)
        {
            var existingCourseFeaturedPhoto = await applicationDbContext.Courses.FindAsync(course.Id);

            if (existingCourseFeaturedPhoto != null)
            {
                // Update Course Featured Photo
                existingCourseFeaturedPhoto.FeaturedPhoto = course.FeaturedPhoto;
                existingCourseFeaturedPhoto.UpdatedAt = course.UpdatedAt;

                await applicationDbContext.SaveChangesAsync();

                return existingCourseFeaturedPhoto;
            }

            return null;
        }

        // Update Course Featured Bannr
        public async Task<Course?> UpdateCourseFeaturedBanner(Course course)
        {
            var existinCourseFeaturedBanner = await applicationDbContext.Courses.FindAsync(course.Id);

            if (existinCourseFeaturedBanner != null)
            {
                // update course Featured Banner
                existinCourseFeaturedBanner.FeaturedBanner = course.FeaturedBanner;
                existinCourseFeaturedBanner.UpdatedAt = course.UpdatedAt;

                await applicationDbContext.SaveChangesAsync();

                return existinCourseFeaturedBanner;
            }

            return null;
        }


        public async Task<Course?> UpdateCourseFeaturedVideo(Course course)
        {
            var existingCourseFeaturedVideo = await applicationDbContext.Courses.FindAsync(course.Id);

            if (existingCourseFeaturedVideo != null)
            {
                existingCourseFeaturedVideo.FeaturedVideoType = course.FeaturedVideoType;
                existingCourseFeaturedVideo.FeaturedVideoContent = course.FeaturedVideoContent;
                existingCourseFeaturedVideo.UpdatedAt = course.UpdatedAt;

                await applicationDbContext.SaveChangesAsync();
                return existingCourseFeaturedVideo;
            }
            return null;
        }

        public async Task<Course?> DeleteCourse(Guid id, string instructor_id)
        {
            var course = await applicationDbContext.Courses
                .FirstOrDefaultAsync(c => c.Id == id && c.InstructorId == instructor_id);

            if (course != null)
            {
                applicationDbContext.Courses.Remove(course);
                await applicationDbContext.SaveChangesAsync();

                return course;
            }
            return null;
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
