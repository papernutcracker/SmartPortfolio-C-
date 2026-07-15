using Microsoft.EntityFrameworkCore;
using SmartDividendTracker.Models;

namespace SmartDividendTracker.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserProfile> Users { get; set; }
        public DbSet<DividendStock> Stocks { get; set; }
        public DbSet<CustomGoal> Goals { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=diana\\SQLEXPRESS;Database=SmartDividendTrackerDB;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}