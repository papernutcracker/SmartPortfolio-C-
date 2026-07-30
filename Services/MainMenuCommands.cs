using System;
using Smart_Dividend_Portfolio_Tracker.Services;
using SmartDividendTracker.Models;

namespace SmartDividendTracker.Services
{
    public class ViewPortfolioCommand : IMenuCommand
    {
        private readonly PortfolioManager _portfolioManager;
        private readonly UserProfile _profile; // Додали поле

        // Оновили конструктор
        public ViewPortfolioCommand(PortfolioManager portfolioManager, UserProfile profile)
        {
            _portfolioManager = portfolioManager;
            _profile = profile;
        }

        public string DisplayName => LocalizationManager.Get("MenuOpt1");

        // Передаємо _profile у метод
        public void Execute() => Program.ShowPortfolioMenu(_portfolioManager, _profile);
    }

    public class CheatSheetCommand : IMenuCommand
    {
        public string DisplayName => LocalizationManager.Get("CheatSheetOpt");
        public void Execute()
        {
            bool isUa = LocalizationManager.GetCurrentLanguage() == "uk";
            CheatSheetService.Show(isUa);
        }
    }

    public class TutorialMenuCommand : IMenuCommand
    {
        private readonly UserProfile _profile;
        public TutorialMenuCommand(UserProfile profile) => _profile = profile;

        public string DisplayName => LocalizationManager.Get("EduMenuTitle");
        public void Execute() => TutorialService.ShowMenu(_profile);
    }

    public class CompoundCalculatorCommand : IMenuCommand
    {
        public string DisplayName => LocalizationManager.Get("MenuOpt5");
        public void Execute()
        {
            bool isUa = LocalizationManager.GetCurrentLanguage() == "uk";
            CompoundCalculatorService.RunCalculator(isUa);
        }
    }

    public class GoalCalculatorCommand : IMenuCommand
    {
        private readonly UserProfile _profile;
        private readonly OnboardingService _onboarding;

        public GoalCalculatorCommand(UserProfile profile, OnboardingService onboarding)
        {
            _profile = profile;
            _onboarding = onboarding;
        }

        public string DisplayName => LocalizationManager.Get("MenuOptGoalCalc");
        public void Execute() => GoalCalculatorService.Run(_profile, _onboarding);
    }

    public class OpenSettingsCommand : IMenuCommand
    {
        private readonly UserProfile _profile;
        private readonly OnboardingService _onboarding;

        public OpenSettingsCommand(UserProfile profile, OnboardingService onboarding)
        {
            _profile = profile;
            _onboarding = onboarding;
        }

        public string DisplayName => LocalizationManager.Get("MenuOpt4");
        public void Execute() => _onboarding.OpenSettings(_profile);
    }
}