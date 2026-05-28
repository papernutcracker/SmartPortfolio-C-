using System.Collections.Generic;

namespace SmartDividendTracker.Services
{
    public static class LocalizationManager
    {
        private static string _currentLanguage = "en";

        public static void SetLanguage(string lang)
        {
            _currentLanguage = lang.ToLower() == "uk" ? "uk" : "en";
        }

        public static string GetCurrentLanguage() => _currentLanguage;

        private static readonly Dictionary<string, (string En, string Uk)> _phrases = new()
        {
            { "SelectLang", ("Select your language", "Оберіть вашу мову") },
            { "Welcome", ("Welcome to Smart Dividend Tracker AI!", "Ласкаво просимо до Smart Dividend Tracker AI!") },
            { "ExpLevel", ("What is your investment experience?", "Який ваш досвід інвестування?") },
            { "ExpBeginner", ("Beginner (Just starting)", "Новачок (Тільки починаю)") },
            { "ExpPro", ("Experienced", "Досвідчений") },
            { "GoalInfo", ("Type your main investment goal (e.g., Passive Income): ", "Введіть вашу головну мету (напр., Пасивний дохід): ") },
            { "HorizonPrompt", ("Select your investment horizon", "Оберіть ваш термін інвестування") },
            { "Horiz5", ("Up to 5 years", "До 5 років") },
            { "Horiz10", ("Up to 10 years", "До 10 років") },
            { "HorizMore", ("More than 10 years", "Більше 10 років") },
            { "HasPort", ("Do you already have an investment portfolio?", "Чи є у вас вже інвестиційний портфель?") },
            { "Yes", ("Yes", "Так") },
            { "No", ("No", "Ні") },
            { "ProfileSaved", ("Profile saved successfully! Let's start.", "Профіль успішно збережено! Починаємо.") },
            { "MainMenu", ("MAIN MENU", "ГОЛОВНЕ МЕНЮ") },
            { "MenuOpt1", ("View Portfolio", "Переглянути портфель") },
            { "MenuOpt2", ("AI Market Analysis", "AI Аналіз ринку") },
            { "MenuOpt3", ("Exit", "Вихід") },
            { "GoalPrompt", ("Select your main investment goal", "Оберіть вашу головну мету інвестування") },
            { "GoalPassive", ("Passive Income", "Пасивний дохід (Дивіденди)") },
            { "GoalGrowth", ("Capital Growth", "Збільшення капіталу") },
            { "GoalPurchase", ("Major Purchase (Real estate, etc.)", "Велика покупка (Нерухомість тощо)") },
            { "GoalRemovedWarning", (
                "Warning: 'Passive Income' goal was removed because it's too risky for a <5 years horizon.",
                "Увага: Мету 'Пасивний дохід' вилучено, оскільки це ризиковано для горизонту до 5 років."
            ) },
            { "MenuOpt4", ("Profile Settings", "Налаштування профілю") },
            { "SettingsMenu", ("PROFILE SETTINGS", "НАЛАШТУВАННЯ ПРОФІЛЮ") },
            { "SetLang", ("Change Language", "Змінити мову") },
            { "SetExp", ("Change Experience Level", "Змінити рівень досвіду") },
            { "SetGoals", ("Change Investment Goals", "Змінити цілі інвестування") },
            { "SetHorizon", ("Change Investment Horizon", "Змінити горизонт інвестування") },
            { "Back", ("Back to Main Menu", "Назад до головного меню") },
            { "ExitMessage", ("Saving progress... See you later, Investor!", "Зберігаємо прогрес... До зустрічі, Інвесторе!") }
        };

        public static string Get(string key)
        {
            if (!_phrases.ContainsKey(key)) return key;
            return _currentLanguage == "uk" ? _phrases[key].Uk : _phrases[key].En;
        }
    }
}