using eduzora_lms.Data;
using eduzora_lms.Models.Admin.Domain;
using Microsoft.EntityFrameworkCore;

namespace eduzora_lms.Repositories.Admin
{
    public class LevelRepository : ILevelRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public LevelRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<Level> AddAsync(Level level)
        {
            // Add Level to Tbl
            await applicationDbContext.Levels.AddAsync(level);
            // Commit
            await applicationDbContext.SaveChangesAsync();

            return level;
        }

        public async Task<Level?> DeleteAsync(Guid id)
        {
            var existingLevel = await applicationDbContext.Levels.FindAsync(id);

            if (existingLevel != null)
            {
                applicationDbContext.Levels.Remove(existingLevel);
                await applicationDbContext.SaveChangesAsync();

                return existingLevel;
            }

            return null;
        }

        public async Task<IEnumerable<Level>> GetAllAsync()
        {
            // Get All Levels From the tbl
            return await applicationDbContext.Levels.ToListAsync();
        }

        public Task<Level?> GetAsync(Guid id)
        {
            // for getting single record with the help of ID
            return applicationDbContext.Levels.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Level?> UpdateAsync(Level level)
        {
            var existingLevel = await applicationDbContext.Levels.FindAsync(level.Id);

            if (existingLevel != null)
            {
                // update existing to new value
                existingLevel.LevelName = level.LevelName;
                await applicationDbContext.SaveChangesAsync();

                return existingLevel;
            }

            return null;
        }
    }
}
