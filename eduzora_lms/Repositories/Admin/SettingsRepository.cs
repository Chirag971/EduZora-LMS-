using eduzora_lms.Data;
using eduzora_lms.Models.Admin.Domain;
using Microsoft.EntityFrameworkCore;

namespace eduzora_lms.Repositories.Admin
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public SettingsRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public async Task<Setting?> GetAsync()
        {
            // get First Record
            return await applicationDbContext.Settings.FirstOrDefaultAsync();
        }

        public async Task<Setting?> UpdateAsync(Setting setting)
        {
            var existingSetting = await applicationDbContext.Settings.FindAsync(setting.Id);

            // if id find
            if (existingSetting != null)
            {
                // update setting
                existingSetting.SalesCommission = setting.SalesCommission;
                await applicationDbContext.SaveChangesAsync();

                return existingSetting;

            }
            return null;
        }
    }
}
