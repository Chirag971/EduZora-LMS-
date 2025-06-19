using eduzora_lms.Models.Admin.Domain;
using eduzora_lms.Models.Instructor.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eduzora_lms.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
        {
            
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        //--- DbSets (Tables Name) ---

        public DbSet<Category> Categories { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Course> Courses { get; set; }



        //--- DbSets (Tables Name) ---

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            // For Asp.Net Roles Tbl
            var adminRoleId = "868CC020-1257-4C2B-8F43-8AE013F4AFC4";
            var roles = new IdentityRole 
            { 
                Id = adminRoleId,
                Name = "Admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = adminRoleId
            };
            builder.Entity<IdentityRole>().HasData(roles);


            // For Asp.net Users Tbl
            var adminId = "E4978D01-A126-4DF7-A04E-9C66DC9FDE2A";
            var adminUser = new ApplicationUser
            { 
                Id = adminId,
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@gmail.com",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                EmailConfirmed = true,
            };

            // For hashing Password
            adminUser.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(adminUser, "123");
            builder.Entity<ApplicationUser>().HasData(adminUser);

            // For Asp.Net UserRoles Tbl
            builder.Entity<IdentityUserRole<string>>().HasData( new IdentityUserRole<string> 
            {
                RoleId = adminRoleId,
                UserId = adminId
            });
        }
    }
}
