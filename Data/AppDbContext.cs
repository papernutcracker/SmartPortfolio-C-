using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SmartDividendTracker.Models;

namespace SmartDividendTracker.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserProfile> Users { get; set; }
        public DbSet<DividendStock> Stocks { get; set; }
        public DbSet<CustomGoal> CustomGoals { get; set; }

        // Повертаємо назад для сумісності з OnboardingService
        public DbSet<CustomGoal> Goals { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Динамічно зчитуємо рядок підключення з appsettings.json
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();

                var connectionString = configuration.GetConnectionString("DefaultConnection")
                                       ?? "Server=diana\\SQLEXPRESS;Database=SmartDividendTrackerDB;Trusted_Connection=True;TrustServerCertificate=True;";

                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}