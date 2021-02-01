using System;
using System.Collections.Generic;
using System.Text;
using Covid19App.Models.UserModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Covid19App.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Covid19App.Data.Patient> Patients { get; set; }
    }
}
