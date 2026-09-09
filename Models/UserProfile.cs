using System.Collections.Generic;

namespace SmartDividendTracker.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string Language { get; set; } = "en";

        // Базова валюта портфеля (наприклад, "UAH", "USD", "EUR")
        public string Currency { get; set; } = "UAH";

        // Окремі курси для конвертації (з актуальними дефолтними значеннями)
        public decimal UsdExchangeRate { get; set; } = 44.70m;
        public decimal EurExchangeRate { get; set; } = 52.00m;

        public ExperienceLevel Experience { get; set; }
        public List<InvestmentGoal> Goals { get; set; } = new List<InvestmentGoal>();

        public InvestmentHorizon Horizon { get; set; }
        public bool HasExistingPortfolio { get; set; }
        public bool HasCompletedTutorial { get; set; } = false;

        public List<CustomGoal> SavedCustomGoals { get; set; } = new();
    }
}