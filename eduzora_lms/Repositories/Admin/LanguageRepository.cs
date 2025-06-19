using eduzora_lms.Data;
using eduzora_lms.Models.Admin.Domain;
using Microsoft.EntityFrameworkCore;

namespace eduzora_lms.Repositories.Admin
{
    public class LanguageRepository : ILanguageRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public LanguageRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }


        public async Task<Language> AddAsync(Language language)
        {
            // Add Languages to Tbl
            await applicationDbContext.Languages.AddAsync(language);
            // Commit
            await applicationDbContext.SaveChangesAsync();

            return language;
        }

        public async Task<Language?> DeleteAsync(Guid id)
        {
            // Find Existing Language by id
            var existingLanguage = await applicationDbContext.Languages.FindAsync(id);

            if (existingLanguage != null)
            {
                // Delete Language
                applicationDbContext.Languages.Remove(existingLanguage);
                // Commit
                await applicationDbContext.SaveChangesAsync();

                return existingLanguage;
            }

            return null;
        }

        public async Task<IEnumerable<Language>> GetAllAsync()
        {
            // Get All Languages
            return await applicationDbContext.Languages.ToListAsync();
        }

        public Task<Language?> GetAsync(Guid id)
        {
            // Getting Single Language From The ID
            return applicationDbContext.Languages.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Language?> UpdateAsync(Language language)
        {
            var existingLanguage = await applicationDbContext.Languages.FindAsync(language.Id);

            if (existingLanguage != null)
            {
                // Update Existing Language
                existingLanguage.LanguageName = language.LanguageName;
                // Commit
                await applicationDbContext.SaveChangesAsync();

                return existingLanguage;
            }

            return null;
        }
    }
}
