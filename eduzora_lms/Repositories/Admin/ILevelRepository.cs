using eduzora_lms.Models.Admin.Domain;

namespace eduzora_lms.Repositories.Admin
{
    public interface ILevelRepository
    {
        // Get All Levels 
        Task<IEnumerable<Level>> GetAllAsync();

        // Add Levels
        Task<Level> AddAsync(Level level);

        // Update Levels
        Task<Level?> UpdateAsync(Level level);

        // Delete Levels
        Task<Level?> DeleteAsync(Guid id);

        // For Get Single Record
        Task<Level?> GetAsync(Guid id);
    }
}
