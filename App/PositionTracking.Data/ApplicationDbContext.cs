using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using static PositionTracking.Models.MembersViewModel;

/*
 * 
 * MAC-OS
 * dotnet ef migrations add InitialModel
 * 
 * dotnet ef database update
 * 
 */

namespace PositionTracking.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Project> Projects { get; set; }

        public DbSet<Keyword> Keywords { get; set; }

        


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }
    }
}