﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
        public DbSet<SEM_project.Models.Entities.EmployeeToComputer> EmployeeToComputer { get; set; }
        public DbSet<SEM_project.Models.Entities.ComputerHistory> ComputerHistory { get; set; }
        public DbSet<SEM_project.Models.Entities.Employee> Employee { get; set; }
        public DbSet<SEM_project.Models.Entities.Licence> Licence { get; set; }
        public DbSet<SEM_project.Models.Entities.ComputerToLicence> ComputerToLicence { get; set; }
    }
}