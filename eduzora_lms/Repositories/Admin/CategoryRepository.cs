using eduzora_lms.Data;
using eduzora_lms.Models.Admin.Domain;
using Microsoft.EntityFrameworkCore;

namespace eduzora_lms.Repositories.Admin
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public CategoryRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<Category> AddAsync(Category category)
        {
            // Add Category To Tbl
            await applicationDbContext.Categories.AddAsync(category);
            // Commit
            await applicationDbContext.SaveChangesAsync();

            return category;
        }

        public async Task<Category?> DeleteAsync(Guid id)
        {
            var existingCategory = await applicationDbContext.Categories.FindAsync(id);

            if (existingCategory != null)
            {
                applicationDbContext.Categories.Remove(existingCategory);
                await applicationDbContext.SaveChangesAsync();

                return existingCategory;
            }

            return null;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            // Get All Categories From The Tbl
            return await applicationDbContext.Categories.ToListAsync();
        }

        public Task<Category?> GetAsync(Guid id)
        {
            // for getting Single Record With the help of ID
            return applicationDbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Category?> UpdateAsync(Category category)
        {
            var existingCategory = await applicationDbContext.Categories.FindAsync(category.Id);

            if (existingCategory != null)
            {
                // Update existing to new Value
                existingCategory.CategoryName = category.CategoryName;
                await applicationDbContext.SaveChangesAsync();

                return existingCategory;
            }

            return null;
        }
    }
}
