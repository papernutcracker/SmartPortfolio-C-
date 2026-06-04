using System.Collections.Generic;

namespace SmartDividendTracker.Models
{
    public class UserProfile
    {
        public string Language { get; set; } = "en";
        public ExperienceLevel Experience { get; set; }

        public List<InvestmentGoal> Goals { get; set; } = new List<InvestmentGoal>();

        public InvestmentHorizon Horizon { get; set; }
        public bool HasExistingPortfolio { get; set; }
        public bool HasCompletedTutorial { get; set; } = false;

        public System.Collections.Generic.List<CustomGoal> SavedCustomGoals { get; set; } = new();
    }
}