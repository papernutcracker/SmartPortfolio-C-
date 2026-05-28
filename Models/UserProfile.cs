using System.Collections.Generic;

namespace SmartDividendTracker.Models
{
    public class UserProfile
    {
        public string Language { get; set; } = "en";
        public ExperienceLevel Experience { get; set; }

        // ТЕПЕР ЦЕ СПИСОК ЦІЛЕЙ
        public List<InvestmentGoal> Goals { get; set; } = new List<InvestmentGoal>();

        public InvestmentHorizon Horizon { get; set; }
        public bool HasExistingPortfolio { get; set; }
    }
}