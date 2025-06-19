using eduzora_lms.Models.Admin.Domain;

namespace eduzora_lms.Repositories.Admin
{
    public interface ISettingsRepository
    {
        // Get First Setting Record
        Task<Setting?> GetAsync();

        // Update Setting
        Task<Setting?> UpdateAsync(Setting setting);
    }
}
