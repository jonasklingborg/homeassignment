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

        public DbSet<License> Licenses { get; set; }
    }
}
