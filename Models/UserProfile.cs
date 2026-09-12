using System.Collections.Generic;

namespace SmartDividendTracker.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string Language { get; set; } = "en";

        public string? ApiKey { get; set; }
        public string? Currency { get; set; }
        public decimal? UsdExchangeRate { get; set; }
        public decimal? EurExchangeRate { get; set; }

        public ExperienceLevel Experience { get; set; }
        public List<InvestmentGoal> Goals { get; set; } = new List<InvestmentGoal>();

        public InvestmentHorizon Horizon { get; set; }
        public bool HasExistingPortfolio { get; set; }
        public bool HasCompletedTutorial { get; set; } = false;


        public List<CustomGoal> SavedCustomGoals { get; set; } = new();
    }
}