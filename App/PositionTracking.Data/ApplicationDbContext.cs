using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

/*
 * 
 * MAC-OS
 * dotnet ef migrations add InitialModel
 * 
 * 
 * call from PositionTracking
 * dotnet ef migrations add InitialMigration --project ../PositionTracking.Data
 * 
 * dotnet ef database update
 * 
 */

namespace PositionTracking.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        private readonly bool isTest;
        public DbSet<Project> Projects { get; set; }

        public DbSet<Keyword> Keywords { get; set; }

        public DbSet<UserPermission> UserPermission { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }

        public ApplicationDbContext()
        {
            isTest = true;
        }
#if DEBUG 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (isTest)
            optionsBuilder.UseSqlite("Data Source=../../../../PositionTracking/UserDatabase.db");
            
            base.OnConfiguring(optionsBuilder);
        }
#endif
    }
}