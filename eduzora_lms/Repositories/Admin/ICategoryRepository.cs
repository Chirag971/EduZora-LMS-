using Azure;
using eduzora_lms.Models.Admin.Domain;

namespace eduzora_lms.Repositories.Admin
{
    public interface ICategoryRepository
    {
        // Get All Category
        Task<IEnumerable<Category>> GetAllAsync();

        // Add Category
        Task<Category> AddAsync(Category category);

        // Update Category
        Task<Category?> UpdateAsync(Category category);

        // Delete Category
        Task<Category?> DeleteAsync(Guid id);

        // For Get Single Record
        Task<Category?> GetAsync(Guid id);
    }
}
