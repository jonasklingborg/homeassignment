using CandyLicense.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CandyLicense.Api.Data
{
    public class CandyLicenseContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "LicenseDb");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<License>()
                .HasData(new List<License>
                {
                    new() { Id = 1, Name = "Gelehallon" }, 
                    new() { Id = 2, Name = "Sega råttor" },
                    new() { Id = 3, Name = "Chokladpraliner" },
                    new() { Id = 4, Name = "Vaniljtoppar" }
                });
        }

        public DbSet<License> Licenses { get; set; }
    }
}
