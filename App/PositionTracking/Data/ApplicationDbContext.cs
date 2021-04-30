using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

/*
 * 
 * 
 * dotnet ef migrations add InitialModel
 * 
 * dotnet ef database update
 * 
 */

namespace PositionTracking.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Project> Projects { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
