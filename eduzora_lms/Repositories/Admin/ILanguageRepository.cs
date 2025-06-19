using eduzora_lms.Models.Admin.Domain;

namespace eduzora_lms.Repositories.Admin
{
    public interface ILanguageRepository
    {
        // Get All Language
        Task<IEnumerable<Language>> GetAllAsync();

        // Add Language
        Task<Language> AddAsync(Language language);

        // Update Language
        Task<Language?> UpdateAsync(Language language);

        // Delete Language
        Task<Language?> DeleteAsync(Guid id);

        // Get Language By Id (Single Record)
        Task<Language?> GetAsync(Guid id);

    }
}
