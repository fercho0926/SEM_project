using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SEM_project.Models;

namespace SEM_project.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<SEM_project.Models.Users_App> Users_App { get; set; }
        public DbSet<SEM_project.Models.Entities.Computer> Computer { get; set; }
        public DbSet<SEM_project.Models.Entities.UserToComputer> UserToComputer { get; set; }
        public DbSet<SEM_project.Models.Entities.ComputerHistory> ComputerHistory { get; set; }


        //public DbSet<SEM_project.Models.Logs> User_Logs { get; set; }
        //public DbSet<SEM_project.Models.User_contracts> User_contracts { get; set; }
        //public DbSet<SEM_project.Models.Entities.Balance> Balance { get; set; }
        //public DbSet<SEM_project.Models.Entities.MovementsByBalance> MovementsByBalance { get; set; }
        //public DbSet<SEM_project.Models.ReferedByUser> ReferedByUser { get; set; }
        //public DbSet<SEM_project.Models.ReferedByUserMovement> ReferedByUserMovement { get; set; }
    }
}