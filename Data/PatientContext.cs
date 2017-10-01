// Context class coordinates Entity Framework functionality for a given data model

using HealthcareNetCoreSample.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthcareNetCoreSample.Data
{
    public class PatientContext : DbContext
    {
        public PatientContext(DbContextOptions<PatientContext> options) : base(options)
        {
        }

        // Create DbSet for each entity set(database table)
        public DbSet<InsProvider> InsProviders { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<Patient> Patients { get; set; }

        // Override pluralization on db creation
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<InsProvider>().ToTable("InsProvider");
            modelBuilder.Entity<Claim>().ToTable("Claim");
            modelBuilder.Entity<Patient>().ToTable("Patient");
        }
    }

}